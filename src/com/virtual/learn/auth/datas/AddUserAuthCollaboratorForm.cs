using System.ComponentModel.DataAnnotations;
using cairn.Accounts.Collaborator;
using cairn.Models.Accounts;
using Newtonsoft.Json;

namespace cairn.Accounts.Auth
{
    /// <summary>Informations of the form to add a collaborator account</summary>
    public class AddUserAuthCollaboratorForm : AddUserAuthForm
    {

        /// <summary>Contains the informations about the account</summary>
        [JsonProperty("model")]
        [Required]
        public CollaboratorAccountModel Model {get; set;}
        
        /// <summary>Default constructor</summary>
        public AddUserAuthCollaboratorForm() : base() {}

        /// <summary>Constructor used to clone</summary>
        public AddUserAuthCollaboratorForm(AddUserAuthCollaboratorForm baseForm) : base(baseForm)
        {
            this.Model = baseForm.Model;
        }
    }
}