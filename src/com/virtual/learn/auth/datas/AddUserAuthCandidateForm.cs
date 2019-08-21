using System.ComponentModel.DataAnnotations;
using cairn.Models.Accounts;
using Newtonsoft.Json;

namespace cairn.Accounts.Auth
{
    /// <summary>Informations of the form to add a candidate account</summary>
    public class AddUserAuthCandidateForm : AddUserAuthForm
    {

        /// <summary>Contains the informations about the account</summary>
        [JsonProperty("model")]
        [Required]
        public CandidateAccountModel Model {get; set;}

        /// <summary>Default constructor</summary>
        public AddUserAuthCandidateForm() : base() {}

        /// <summary>Constructor used for cloning</summary>
        public AddUserAuthCandidateForm(AddUserAuthCandidateForm baseForm) : base(baseForm)
        {
            this.Model = baseForm.Model;
        }
    }
}