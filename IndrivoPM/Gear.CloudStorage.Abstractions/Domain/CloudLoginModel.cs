using Newtonsoft.Json;

namespace Gear.CloudStorage.Abstractions.Domain
{
    public class CloudLoginModel
    {
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }


        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }


        [JsonProperty(PropertyName = "expires_in")]
        public string ExpiresIn { get; set; }


        [JsonProperty(PropertyName = "ext_expires_in")]
        public string ExtExpiresIn { get; set; }


        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }


        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }
    }
}
