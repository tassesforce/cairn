using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace cairn.Models.Accounts
{ 
    /// <summary>Resource for an agency account (moral)</summary>
    public partial class AgencyAccountModel : Account
    { 

        /// <summary>Account identifier</summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>Name of the agency</summary>
        [Required]
        public string Name {get; set;}

        /// <summary>SIRET number</summary>
        [Required]
        public string Siret { get; set; }

        /// <summary>Specialised sector of activity of the agency</summary>
        [Required]
        public string Sector { get; set; }

        /// <summary>Adress of the agency</summary>
        [Required]
        public Adress Adress { get; set; }

        /// <summary>How to contact this agency</summary>
        public Contact Contact { get; set; }

        /// <summary>Partners of the agency (companies and candidates)</summary>
        public Partners Partners { get; set; }

        /// <summary>Person in charge of the agency account</summary>
        public Responsible Responsible { get; set; }

    }
}