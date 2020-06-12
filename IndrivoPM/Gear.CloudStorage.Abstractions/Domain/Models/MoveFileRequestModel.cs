using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Gear.CloudStorage.Abstractions.Domain.Models
{
    public class MoveFileRequestModel
    {
        [JsonProperty(PropertyName = "accessToken")]
        public string AccessToken { get; set; }

        public ParentRequestReference ParentReference { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Can't be instantiated without parameters.
        /// </summary>
        private MoveFileRequestModel()
        {
            // Special purpose
        }

        public MoveFileRequestModel([Required] string newParentFolderId, string fullFileName)
        {
            ParentReference = new ParentRequestReference(newParentFolderId);

            Name = fullFileName;
        }
    }

    public class ParentRequestReference
    {
        public string Id { get; set; }

        /// <summary>
        /// Can't be instantiated without parameters.
        /// </summary>
        private ParentRequestReference()
        {
            // Special purpose
        }

        public ParentRequestReference(string parentFolderId)
        {
            Id = parentFolderId;
        }
    }
}
