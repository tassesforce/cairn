using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace cairn.Accounts.Auth
{
    /// <summary>Informations took from the form used to create accounts</summary>
    public class AuthForm
    {

        /// <summary>login of the account</summary>
        [JsonProperty("login")]
        [Required]
        public string Login {get; set;}
        
        /// <summary>password of the account</summary>
        [JsonProperty("password")]
        [Required]
        public string Password {get; set;}

        /// <summary>default constructor</summary>
        public AuthForm() {}

        /// <summary>cloning constructor</summary>
        public AuthForm(AuthForm baseForm)
        {
            this.Login = baseForm.Login;
            this.Password = baseForm.Password;
        }
        
    }
}