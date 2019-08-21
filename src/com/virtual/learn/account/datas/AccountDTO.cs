using cairn.Accounts.Auth;
using Newtonsoft.Json;

namespace cairn.Models.Accounts
{ 
    /// <summary>
    /// DTO used to display informations about the account (UI)
    /// </summary>
    public class AccountDTO
    {
        /// <summary>Account type (Agency, candidate, company, collaborator, ...)</summary>
        [JsonProperty("accountType")]
        public string AccountType {get; set;}

        /// <summary>Information from the account in Garda</summary>
        [JsonProperty("auth")]
        public AuthForm Auth {get; set;}

        /// <summary>Information about the account</summary>
        [JsonProperty("account")]
        public Account Account {get; set;}
    }
}