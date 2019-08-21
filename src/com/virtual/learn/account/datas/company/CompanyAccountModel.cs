using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace cairn.Models.Accounts
{ 
    /// <summary>Resource for a company account (moral)</summary>
    public partial class CompanyAccountModel : Account
    { 

        /// <summary>Account identifier</summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>Name of the company</summary>
        public string Name {get; set;}

        /// <summary>SIRET number</summary>
        [Required]
        public string Siret { get; set; }
        
        /// <summary>Sector of activity of the company</summary>
        [Required]
        public string Sector { get; set; }

        /// <summary>Adress of the company</summary>
        [Required]
        public Adress Adress { get; set; }

        /// <summary>How to contact the company</summary>
        public Contact Contact { get; set; }

        /// <summary>Partners of the company (agencies and candidates)</summary>
        public Partners Partners { get; set; }

        /// <summary>Person to contact in the company</summary>
        public Responsible Responsible { get; set; }

        /// <summary>Modules linked to this company</summary>
        public CompanyModules Modules { get; set; }

        /// <summary>Fund associated to this company (ex : CARSAT)</summary>
        public Fund Fund { get; set; }

    }
}
