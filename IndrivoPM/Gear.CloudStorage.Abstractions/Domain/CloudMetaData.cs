using System;
using Newtonsoft.Json;

namespace Gear.CloudStorage.Abstractions.Domain
{
    public class CloudMetaData
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "lastModifiedDateTime")]
        public DateTime ModifiedAt { get; set; }


        [JsonProperty(PropertyName = "createdDateTime")]
        public DateTime CreateDateTime { get; set; }


        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }


        [JsonProperty(PropertyName = "size")]
        public long Size { get; set; }


        [JsonProperty(PropertyName = "webUrl")]
        public string WebUrl { get; set; }


        [JsonProperty(PropertyName = "@microsoft.graph.downloadUrl")]
        public string DownloadUrl { get; set; }


        [JsonProperty(PropertyName = "createdBy")]
        public UserMetaData CreatedBy { get; set; }


        [JsonProperty(PropertyName = "lastModifiedBy")]
        public UserMetaData LastModifiedBy { get; set; }

    }


    public class UserMetaData
    {

        [JsonProperty(PropertyName = "id")]
        public Guid UserId { get; set; }


        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }
    }
}