using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.CloudStorage.Abstractions.Domain.Models;
using Gear.CloudStorage.Abstractions.Service;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Gear.OneDriveCloudStorage.Service
{
    public class OneDriveService : BaseStorageService
    {
        private const int MagicNumber = 3932160;
        private const int FourMegaBytes = 4 * 1024 * 1024;

        #region Private methods

        private async Task<HttpResponseMessage> UploadSmallFile(CloudStorageApiRequestModel model)
        {
            using (var ms = new MemoryStream())
            {
                model.File.CopyTo(ms);
                var fileBytes = ms.ToArray();

                var requestByteContent = new ByteArrayContent(fileBytes);
                var uri = ApiRoot + model.Path + "/" + model.File.FileName + ":/content";
                var response = await WebRequest.PutAsync(uri, requestByteContent);
                return response;
            }
        }

        private async Task<HttpResponseMessage> UploadFileByChunks(FileChunksModel model)
        {
            using (var ms = new MemoryStream())
            {
                model.File.CopyTo(ms);
                var fileBytes = ms.ToArray();

                var range = model.GetChunk();

                var requestByteContent = new ByteArrayContent(fileBytes, range.Offset, range.Count);
                requestByteContent.Headers.Add("Content-Range", model.GetContentRange());
                var response = await WebRequest.PutAsync(model.UploadUrl, requestByteContent);
                return response;
            }
        }
        #endregion

        public OneDriveService(IOptionsMonitor<CloudServiceSettings> config) :
            base(config.CurrentValue.ClientId,
                config.CurrentValue.ClientSecret, config.CurrentValue.ReturnUrl,
                config.CurrentValue.ApiRoot, config.CurrentValue.LoginUrl, config.CurrentValue.AppName)
        {
        }

        /// <summary>
        /// Return url for code flow authentication
        /// Required LoginUrl, ClientId, Scopes CallbackUri
        /// </summary>
        /// <returns></returns>
        public override string GetCodeLoginUrl(string scopes)
        {
            return
                $"{LoginUrl}client_id={ClientId}&scope={scopes}&response_type=code&redirect_uri={CallBackUri}";
        }


        /// <summary>
        /// Delete element on path
        /// Required AccessToken, Path, ElementName
        /// </summary>
        /// userId from db
        /// <returns></returns>
        public override async Task<HttpResponseMessage> DeleteElement(CloudStorageApiRequestModel model)
        {
            if (!WebRequest.DefaultRequestHeaders.Contains("Authorization"))
                WebRequest.DefaultRequestHeaders.Add("Authorization", "Bearer " + model.AccessToken);
            var uri = ApiRoot + model.Path + "/" + model.ElementName;
            var response = await WebRequest.DeleteAsync(uri);
            return response;
        }


        /// <summary>
        /// Add Folder on path
        /// Required FolderName, Path, AccessToken
        /// </summary>
        /// <returns></returns>
        public override async Task<HttpResponseMessage> AddFolder(CloudStorageApiRequestModel model)
        {
            if (!WebRequest.DefaultRequestHeaders.Contains("Authorization"))
                WebRequest.DefaultRequestHeaders.Add("Authorization", "Bearer " + model.AccessToken);
            var content = new StringContent("{\"name\": \"" + model.FolderName +
                                            "\",\"folder\": { },\"@microsoft.graph.conflictBehavior\": \"rename\"}", Encoding.UTF8, "application/json");
            var uri = ApiRoot + "/me/" + model.Path;
            var response = await WebRequest.PostAsync(uri, content);
            return response;
        }


        /// <summary>
        /// Upload file to path
        /// Required AccessToken, File, Path
        /// </summary>
        /// <returns></returns>
        public override async Task<HttpResponseMessage> UploadFile(CloudStorageApiRequestModel model)
        {
            WebRequest.DefaultRequestHeaders.Add("Authorization", "Bearer " + model.AccessToken);
            try
            {
                if (model.File != null && model.File.Length > 0)
                {
                    if (model.File.Length <= FourMegaBytes)
                    {
                        return await UploadSmallFile(model);
                    }

                    HttpContent postRequestContent = new StringContent("\"@microsoft.graph.conflictBehavior\": \"rename | fail | replace\"", Encoding.UTF8);
                    postRequestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var httpResponseForChunks = await WebRequest.PostAsync($"{ApiRoot}{model.Path}/{model.File.FileName}:/createUploadSession", postRequestContent);
                    var sessionResponse = await httpResponseForChunks.Content.ReadAsStringAsync();

                    var url = JsonConvert.DeserializeObject<ResponseContentModel>(sessionResponse).UploadUrl;

                    var responseCode = HttpStatusCode.Continue;
                    var chunks = new FileChunksModel(url, model.File)
                    {
                        AccessToken = model.AccessToken
                    };
                    chunks.InisializeChunks(MagicNumber);
                    
                    while (responseCode != HttpStatusCode.OK && responseCode != HttpStatusCode.Created)
                    {
                        var response = await UploadFileByChunks(chunks);
                        responseCode = response.StatusCode;

                        if (responseCode == HttpStatusCode.Accepted)
                        {
                            var content = JsonConvert.DeserializeObject<ResponseContentModel>(await response.Content.ReadAsStringAsync());
                            if (content.NextExpectedRanges.Count == 0 || content.ExpirationDateTime < DateTime.Now)
                                continue;

                            chunks.AddRangeOfChunksFromContent(content, MagicNumber);
                        }
                        else if (!(responseCode == HttpStatusCode.OK || responseCode == HttpStatusCode.Created))
                        {
                            return response;
                        }
                    }
                }
            }
            catch
            {
                //
            }
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="longString"></param>
        /// <returns></returns>
        private static List<byte[]> SplitByteArray(string longString)
        {
            var source = Convert.FromBase64String(longString);
            var result = new List<byte[]>();

            for (var i = 0; i < source.Length; i += 128)
            {
                var buffer = new byte[128];
                Buffer.BlockCopy(source, i, buffer, 0, 128);
                result.Add(buffer);
            }
            return result;
        }

        /// <summary>
        /// Get all files in a folder
        /// Required AccessToken, Path
        /// </summary>l
        /// <returns></returns>
        public override async Task<List<CloudMetaData>> GetChildren(CloudStorageApiRequestModel model)
        {
            WebRequest.DefaultRequestHeaders.Add("Authorization", "Bearer " + model.AccessToken);
            var uri = ApiRoot + model.Path + ":/children";
            var responseMessage = await WebRequest.GetAsync(uri);
            if (!responseMessage.IsSuccessStatusCode) return new List<CloudMetaData>();
            
            var fileArray = JObject.Parse(await responseMessage.Content.ReadAsStringAsync())
                .GetValue("value");
            
            var leadFilesMetaData = new List<CloudMetaData>();
            
            foreach (var item in fileArray.Children())
            {
                leadFilesMetaData.Add(JsonConvert.DeserializeObject<CloudMetaData>(item.ToString()));
            }
            
            return leadFilesMetaData;
        }

        /// <summary>
        /// Items cannot be moved between Drives using this request!
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<HttpResponseMessage> MoveFileToFolder(MoveFileRequestModel model)
        {
            try
            {
                WebRequest.DefaultRequestHeaders.Add("Authorization", "Bearer " + model.AccessToken);

                var uri = ApiRoot + model.Name;
                var request = new HttpMethod("PATCH");
                var httpRequest = new HttpRequestMessage(request, uri)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")
                };
                var response = await WebRequest.SendAsync(httpRequest, CancellationToken.None);

                return response;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }

            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }

        public override async Task<bool> MoveFolderIntoAnotherDirectory(MoveFolderContentFolderRequestModel model)
        {
            WebRequest.DefaultRequestHeaders.Clear();
            WebRequest.DefaultRequestHeaders.Add("Authorization", "Bearer " + model.AccessToken);

            var fromUri = ApiRoot + model.OldPath;

            #region GetDestinationJson
            var toUri = ApiRoot + model.NewPath;
            var responseMessage = await WebRequest.GetAsync(toUri);
            var destinationMetadata = await responseMessage.Content.ReadAsStringAsync();
            var destination = JsonConvert.DeserializeObject<CloudMetaData>(destinationMetadata);
            #endregion
            var request = new HttpMethod("PATCH");

            var httpRequest = new HttpRequestMessage(request, fromUri)
            {
                Content = new StringContent($"{{\"parentReference\": {{\"id\":\"{destination.Id}\" }},\"name\": \"{model.FolderName}\"}}", Encoding.UTF8, "application/json")
            };

            var moveFolderToFolderResponse = await WebRequest.SendAsync(httpRequest);
            return moveFolderToFolderResponse.IsSuccessStatusCode;
        }
        public override async Task<bool> MoveFolderContentIntoAnotherDirectory(MoveFolderContentFolderRequestModel model)
        {
            var files = await GetChildren(new CloudStorageApiRequestModel
            {
                AccessToken = model.AccessToken,
                Path = $"{model.OldPath}",
                FolderName = model.FolderName
            });

            if (files.Count == 0)
                return true;

            var addFolderResult = await AddFolder(new CloudStorageApiRequestModel
            {
                AccessToken = model.AccessToken,
                FolderName = model.FolderName,
                Path = $"{model.NewPath}/P{model.ProjectNumber}"
            });

            if (!addFolderResult.IsSuccessStatusCode)
                return false;

            var folder = JsonConvert.DeserializeObject<CreateFolderResponseModel>(await addFolderResult.Content.ReadAsStringAsync());
            foreach (var file in files)
            {
                var moveFileToFolderResult = await MoveFileToFolder(new MoveFileRequestModel(folder.Id, file.Name));
                if (!moveFileToFolderResult.IsSuccessStatusCode)
                    return false;
            }

            foreach(var file in files)
            {
                var deleteOldFolderResult = await DeleteElement(new CloudStorageApiRequestModel
                {
                    AccessToken = model.AccessToken,
                    Path = model.OldPath,
                    ElementName = file.Name
                });
                if (!deleteOldFolderResult.IsSuccessStatusCode)
                    return false;
            }

            var result = await DeleteElement(new CloudStorageApiRequestModel
            {
                AccessToken = model.AccessToken,
                Path = model.OldPath,
                ElementName = string.Empty
            });

            return result.IsSuccessStatusCode;
        }
    }
}
