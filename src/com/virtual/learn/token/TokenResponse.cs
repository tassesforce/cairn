using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace cairn.Models.Auth.Token
{ 
    /// <summary>Response after the call to refreshToken</summary>
    [DataContract]
    public partial class TokenResponse
    { 
        /// <summary>the refreshed access token</summary>
        [Required]
        [DataMember(Name="access_token")]
        public string AccessToken { get; set; }

        /// <summary>Token type (Bearer in our case)</summary>
        [Required]
        [DataMember(Name="token_type")]
        public string TokenType { get; set; }

        /// <summary>Returns the string presentation of the object</summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class TokenResponse {\n");
            sb.Append("  AccessToken: ").Append(AccessToken).Append("\n");
            sb.Append("  TokenType: ").Append(TokenType).Append("\n");
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
