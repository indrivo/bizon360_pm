using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Gear.CloudStorage.Abstractions.Domain
{
    public class CloudStorageApiRequestModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "accessToken")]
        public string AccessToken { get; set; }

        public string Path { get; set; } = "drive/root/children";

        public string ElementName { get; set; }

        public string FolderName { get; set; } = "FolderFromApp";

        public IFormFile File { get; set; }

    }
}