using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace cairn.Models.Accounts
{ 
    /// <summary>Fund of a company</summary>
    public partial class Fund
    { 
        /// <summary>Name of the fund</summary>
        [Required]
        public string Name { get; set; }

        /// <summary>Adress of the fund</summary>
        [Required]
        public Adress Adress { get; set; }

    }
}
