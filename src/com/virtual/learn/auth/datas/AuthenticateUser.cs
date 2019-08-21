using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace cairn.Accounts.Auth
{
    /// <summary>Information to send to the web service of Garda</summary>
    public class AuthenticateUser
    {

        /// <summary>Password of the account</summary>
        [JsonProperty("password")]
        // [Required]
        public string Password {get; set;}

        /// <summary>Client Id of the application</summary>
        [JsonProperty("client_id")]
        // [Required]
        public string ClientId {get;set;}

        /// <summary>Client secret of the application</summary>
        [JsonProperty("client_secret")]
        // [Required]
        public string ClientSecret {get;set;}

        /// <summary>Grant type to use (by default we use password)</summary>
        [JsonProperty("grant_type")]
        // [Required]
        public string GrantType {get;set;}

        /// <summary>List of the roles of the account</summary>
        [JsonProperty("roles")]
        // [Required]
        public string Roles {get;set;}
    }
}