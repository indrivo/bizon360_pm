using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Gear.CloudStorage.Abstractions.Domain
{
    public class CloudServiceSettings
    {
        [Required]
        [DisplayName("Redirect Url")]
        [JsonProperty(PropertyName = "ReturnUrl")]
        public string ReturnUrl { get; set; }

        [Required]
        [DisplayName("Client App Id")]
        [JsonProperty(PropertyName = "ClientId")]
        public string ClientId { get; set; }

        [Required]
        [DisplayName("Client App Secret")]
        [JsonProperty(PropertyName = "ClientSecret")]
        public string ClientSecret { get; set; }

        [Required]
        [DisplayName("Api Root")]
        [JsonProperty(PropertyName = "ApiRoot")]
        public string ApiRoot { get; set; }

        [Required]
        [DisplayName("Login Url")]
        [JsonProperty(PropertyName = "LoginUrl")]
        public string LoginUrl { get; set; }


        [Required]
        [DisplayName("App Name")]
        [JsonProperty(PropertyName = "App Name")]
        public string AppName { get; set; }


    }
}
