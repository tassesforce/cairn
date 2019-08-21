using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace cairn.Accounts.Auth
{
    /// <summary>Information to send to the web service of Garda to revoke a token</summary>
    public class TokenRevokeRequest
    {
        /// <summary>RefreshToken to revoke</summary>
        [JsonProperty("refresh_token")]
        [Required]
        public string RefreshToken {get; set;}

        /// <summary>Application id</summary>
        [JsonProperty("client_id")]
        [Required]
        public string ClientId {get;set;}

        /// <summary>Client secret of the application</summary>
        [JsonProperty("client_secret")]
        [Required]
        public string ClientSecret {get;set;}
    }
}