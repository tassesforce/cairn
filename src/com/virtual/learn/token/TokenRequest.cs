using System.Text;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace cairn.Models.Auth.Token
{ 
    /// <summary>Request to refresh a token</summary>
    public class TokenRequest
    { 
        /// <summary>Refresh token to use</summary>
        [Required]
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>Access token to refresh</summary>
        [Required]
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>GrantType to communicate to Garda</summary>
        [Required]
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        /// <summary>Identifier of the account</summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>Client id of the application</summary>
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        /// <summary>Password of the application</summary>
        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        /// <summary>Returns the string presentation of the object</summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class TokenRequest {\n");
            sb.Append("  RefreshToken: ").Append(RefreshToken).Append("\n");
            sb.Append("  GrantType: ").Append(GrantType).Append("\n");
            sb.Append("  UserId: ").Append(UserId).Append("\n");
            sb.Append("  ClientId: ").Append(ClientId).Append("\n");
            sb.Append("  ClientSecret: ").Append(ClientSecret).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>Returns the JSON string presentation of the object</summary>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

    }
}
