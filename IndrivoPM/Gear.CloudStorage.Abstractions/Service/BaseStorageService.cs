using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.CloudStorage.Abstractions.Domain.Models;
using Gear.CloudStorage.Abstractions.Service.Abstractions;

namespace Gear.CloudStorage.Abstractions.Service
{
    /// <summary>
    /// Implementation of the storage service
    /// TODO: Base should contain the local storage service
    /// </summary>
    public abstract class BaseStorageService : IStorageBaseService
    {
        protected string ApiRoot { get; }

        protected string CallBackUri { get; }

        protected string ClientId { get; }

        protected string ClientSecret { get; }

        protected string ApplicationName { get;}

        protected HttpClient WebRequest { get; } = new HttpClient();

        public string LoginUrl { get; }

        protected BaseStorageService(string clientId, string clientSecret, string callBackUri, 
            string apiRoot, string loginUrl, string applicationName)
        {
            ApiRoot = apiRoot;
            ClientSecret = clientSecret;
            CallBackUri = callBackUri;
            ClientId = clientId;
            LoginUrl = loginUrl;
            ApplicationName = applicationName;
        }

        public virtual string GetCodeLoginUrl(string scopes)
        {
            return "No Could Service configured";
        }

        public virtual Task<HttpResponseMessage> DeleteElement(CloudStorageApiRequestModel model)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
        }

        public virtual Task<HttpResponseMessage> AddFolder(CloudStorageApiRequestModel model)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
        }

        public virtual Task<HttpResponseMessage> UploadFile(CloudStorageApiRequestModel model)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
        }

        public virtual Task<List<CloudMetaData>> GetChildren(CloudStorageApiRequestModel model)
        {
            return null;
        }

        public virtual Task<HttpResponseMessage> MoveFileToFolder(MoveFileRequestModel model)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<bool> MoveFolderContentIntoAnotherDirectory(MoveFolderContentFolderRequestModel model)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<bool> MoveFolderIntoAnotherDirectory(MoveFolderContentFolderRequestModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
