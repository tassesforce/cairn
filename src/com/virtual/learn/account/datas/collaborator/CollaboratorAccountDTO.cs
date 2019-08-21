using cairn.Accounts.Auth;
using Newtonsoft.Json;

namespace cairn.Models.Accounts
{ 
    /// <summary>
    /// DTO used to display informations about the account (UI)
    /// </summary>
    public class CollaboratorAccountDTO : AccountDTO
    {
        /// <summary>Name of the referent company/Agency</summary>
        [JsonProperty("name")]
        public string Name {get; set;}

        /// <summary>Adress of the referent company/agency</summary>
        [JsonProperty("adress")]
        public Adress Adress {get; set;}

    }
}