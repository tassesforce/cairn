using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace cairn.Models.Auth.Token
{ 
    /// <summary>Body for the request to revocate a token</summary>
    public class TokenRevocationRequest
    { 
        /// <summary>Refresh token to revocate</summary>
        [Required]
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("client_id")]
        [JsonRequired]
        /// <summary> Identifier of the application</summary> 
        public string ClientId {get;set;}
        [JsonProperty("client_secret")]
        [JsonRequired]
        /// <summary>Password of the application</summary> 
        public string ClientSecret {get;set;}

        /// <summary>Returns the JSON string presentation of the object</summary>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

    }
}
