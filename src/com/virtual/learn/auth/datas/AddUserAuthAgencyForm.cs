using System.ComponentModel.DataAnnotations;
using cairn.Models.Accounts;
using Newtonsoft.Json;

namespace cairn.Accounts.Auth
{
    /// <summary>Informations of the form to add an agency account</summary>
    public class AddUserAuthAgencyForm : AddUserAuthForm
    {

        /// <summary>Contains the informations about the account</summary>
        [JsonProperty("model")]
        [Required]
        public AgencyAccountModel Model {get; set;}
        
        /// <summary>Default constructor</summary>
        public AddUserAuthAgencyForm() : base() {}

        /// <summary>Constructor used to clone</summary>
        public AddUserAuthAgencyForm(AddUserAuthAgencyForm baseForm) : base(baseForm)
        {
            this.Model = baseForm.Model;
        }
    }
}