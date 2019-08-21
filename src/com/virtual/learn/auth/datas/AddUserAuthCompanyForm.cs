using System.ComponentModel.DataAnnotations;
using cairn.Models.Accounts;
using Newtonsoft.Json;

namespace cairn.Accounts.Auth
{
    /// <summary>Informations of the form to add a company account</summary>
    public class AddUserAuthCompanyForm : AddUserAuthForm
    {

        /// <summary>Contains the informations about the account</summary>
        [JsonProperty("model")]
        [Required]
        public CompanyAccountModel Model {get; set;}

        /// <summary>Default constructor</summary>
        public AddUserAuthCompanyForm() : base() {}

        /// <summary>Constructor used for cloning</summary>
        public AddUserAuthCompanyForm(AddUserAuthCompanyForm baseForm) : base(baseForm)
        {
            this.Model = baseForm.Model;
        }
    }
}