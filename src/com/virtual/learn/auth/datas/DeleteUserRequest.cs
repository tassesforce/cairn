using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace cairn.Models.Auth
{
    /// <summary>Body of the request to delete an account on Garda</summary>
    public class DeleteUserRequest
    {
        /// <summary>Identifiant de l'application</summary>
        [JsonProperty("client_id")]
        [Required]
        public string ClientId {get;set;}

        /// <summary>Identifiant secret de l'application</summary>
        [JsonProperty("client_secret")]
        [Required]
        public string ClientSecret {get;set;}

    }
}