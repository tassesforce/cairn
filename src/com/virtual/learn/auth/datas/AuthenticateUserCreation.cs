using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace cairn.Accounts.Auth
{
    /// <summary></summary>
    public class AuthenticateUserCreation : AuthenticateUser
    {

        /// <summary>Login of the account</summary>
        [JsonProperty("login")]
        // [Required]
        public string Login {get; set;}

        /// <summary>Account type (Agency, company, candidate, collaborator, etc.)</summary>
        [JsonProperty("account_type")]
        [Required]
        public string AccountType {get; set;}

        /// <summary>Contact of the account (mail adress/phone number)</summary>
        [JsonProperty("contact")]
        [Required]
        public ContactUserAuth Contact {get;set;}
    }
}