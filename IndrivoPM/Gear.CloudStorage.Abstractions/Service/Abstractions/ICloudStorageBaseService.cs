using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.CloudStorage.Abstractions.Domain.Models;

namespace Gear.CloudStorage.Abstractions.Service.Abstractions
{
    public interface IStorageBaseService
    {
        /// <summary>
        /// Return url for code flow authentication
        /// Required LoginUrl, ClientId, Scopes CallbackUri
        /// </summary>
        /// <returns></returns>
        string GetCodeLoginUrl(string scopes);

        /// <summary>
        /// Delete element on path
        /// Required AccessToken, Path, ElementName
        /// </summary>
        /// userId from db
        /// <returns></returns>
        Task<HttpResponseMessage> DeleteElement(CloudStorageApiRequestModel model);

        /// <summary>
        /// Delete Folder on path
        /// Required FolderName, Path, AccessToken
        /// </summary>
        /// <returns></returns>
        Task<HttpResponseMessage> AddFolder(CloudStorageApiRequestModel model);

        /// <summary>
        /// Upload file to path
        /// Required AccessToken, File, Path
        /// </summary>
        /// <returns></returns>
        Task<HttpResponseMessage> UploadFile(CloudStorageApiRequestModel model);

        /// <summary>
        /// Get all files in a folder
        /// Required AccessToken, Path
        /// </summary>
        /// <returns></returns>
        Task<List<CloudMetaData>> GetChildren(CloudStorageApiRequestModel model);

        /// <summary>
        /// Moves file to the folder
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> MoveFileToFolder(MoveFileRequestModel model);

        /// <summary>
        /// Moves all the content of the source folder to the destination one.
        /// In the end the source folder is erased with its content. IMPORTANT: 
        /// the source folder should be a child in context of folders' hierarchy!!!
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> MoveFolderContentIntoAnotherDirectory(MoveFolderContentFolderRequestModel model);
        Task<bool> MoveFolderIntoAnotherDirectory(MoveFolderContentFolderRequestModel model);
    }
}
