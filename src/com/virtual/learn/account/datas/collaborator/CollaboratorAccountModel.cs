using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using cairn.Models.Accounts;
using Newtonsoft.Json;

namespace cairn.Accounts.Collaborator
{
    public class CollaboratorAccountModel : Account
    {
        /// <summary>Id of the account</summary>
        [JsonProperty("id")]
        public string UserId {get; set;}
        /// <summary>First name</summary>
        [JsonProperty("firstName")]
        [Required]
        public string FirstName {get; set;}
        /// <summary>Last name</summary>
        [Required]
        [JsonProperty("lastName")]
        public string LastName {get; set;}
        /// <summary>How to contact the collaborator</summary>
        [Required]
        [JsonProperty("contact")]
        public Contact Contact {get; set;}
        /// <summary>Referents of the collaborator</summary>
        [Required]
        [JsonProperty("referents")]
        public List<ReferentAccount> Referents {get; set;}
        /// <summary>Role of the collaborator in the company/agency</summary>
        [JsonProperty("position")]
        public string Position {get; set;}
    }
}