using Newtonsoft.Json;

namespace cairn.Accounts.Auth
{
    ///<summary>Class for the authentication form </summary>
    public class UserAuthForm
    {
        /// <summary>login of the account</summary>
        [JsonProperty("login")]
        public string Login {get; set;}

        /// <summary>password of the account</summary>
        [JsonProperty("password")]
        public string Password {get; set;}
    }
}