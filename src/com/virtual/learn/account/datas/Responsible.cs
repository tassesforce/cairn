using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace cairn.Models.Accounts
{ 
    /// <summary>Person in charge of the account</summary>
    public class Responsible
    { 
        /// <summary>First Name of the person</summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>Last name of the person</summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>Role of the person inside the agency or company</summary>
        public string Position { get; set; }

    }
}
