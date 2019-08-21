using System.ComponentModel.DataAnnotations;
using cairn.Models.Accounts;
using Newtonsoft.Json;

namespace cairn.Accounts.Auth
{
    /// <summary>Parent object for adding a n account</summary>
    public class AddUserAuthForm
    {

        /// <summary>Type of the account (agency, company, candidate, collaborator, etc.)</summary>
        [JsonProperty("accountType")]
        [Required]
        public string AccountType {get; set;}
        
        /// <summary>Mail adress used for contacting the account</summary>
        [JsonProperty("mail")]
        [Required]
        public string MailAdress {get; set;}

        /// <summary>Phone number used for contacting the account</summary>
        [JsonProperty("phone")]
        public string PhoneNumber {get; set;}

        /// <summary>Log in informations (login, password)</summary>
        [JsonProperty("auth")]
        [Required]
        public AuthForm Auth {get; set;}
        
        /// <summary>default constructor</summary>
        public AddUserAuthForm() {}

        /// <summary>cloning constructor</summary>
        public AddUserAuthForm(AddUserAuthForm baseForm)
        {
            this.AccountType = baseForm.AccountType;
            this.Auth = baseForm.Auth;
            this.MailAdress = baseForm.MailAdress;
            this.PhoneNumber = baseForm.PhoneNumber;
        }
        
    }
}