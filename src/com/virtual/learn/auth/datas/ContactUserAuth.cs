using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace cairn.Accounts.Auth
{
    /// <summary>Contact information for the Garda account</summary>
    public class ContactUserAuth
    {
        [JsonProperty("phoneNumber")]
        /// <summary>phone number</summary>
        public string PhoneNumber {get; set;}
        [JsonProperty("mailAdress")]
        [Required]
        /// <summary>mail adress to join</summary>
        public string MailAdress {get; set;}
    }
}