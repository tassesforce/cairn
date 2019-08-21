using System.ComponentModel.DataAnnotations;
using cairn.Enum;
using Newtonsoft.Json;

namespace cairn.Models.Accounts
{ 
    /// <summary>Contact methods</summary>
    public partial class Contact
    { 
        /// <summary>Phone number</summary>
        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }

        /// <summary>mail adress</summary>
        [Required]
        [JsonProperty("mail")]
        public string MailAdress { get; set; }

        /// <summary>Possible web site </summary>
        [JsonProperty("web")]
        public string WebPage { get; set; }

        /// <summary>Which contact to use in priority</summary>
        public PreferredContactEnum PreferredContact { get; set; }

    }
}
