using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace cairn.Models.Accounts
{ 
    /// <summary>Mobility informations of an account</summary>
    public partial class Mobility
    { 
        /// <summary>Ready to move</summary>
        [Required]
        public bool Accepted { get; set; }

        /// <summary>How can he move</summary>
        public string Transport { get; set; }

        /// <summary>Number max of kilometers he is ready to do</summary>
        public string TransportArea { get; set; }

    }
}
