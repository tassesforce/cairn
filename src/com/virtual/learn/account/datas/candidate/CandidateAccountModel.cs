using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using cairn.Json;

namespace cairn.Models.Accounts
{ 
    /// <summary>Resource for a candidate account</summary>
    public partial class CandidateAccountModel : Account
    { 
        /// <summary>civility (m., mme, etc.)</summary>
        [Required]
        public string Civility { get; set; }

        /// <summary>First name of the candidate</summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>Last name of the candidate</summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>Adress of the candidate</summary>
        [Required]
        public Adress Adress { get; set; }

        /// <summary>Account identifier</summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>Disponibility date : date after which an agency can send a candidate in a mission</summary>
        [Required]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime DispoDate { get; set; }

        /// <summary>Display informations about the possible disability of the candidate</summary>
        public Disability Disability { get; set; }

        /// <summary>How to contact the candidate</summary>
        public Contact Contact { get; set; }

        /// <summary>Skills of the candidate</summary>
        public Modules Skills { get; set; }

        /// <summary>Modules validated by the candidate</summary>
        public Modules Modules { get; set; }

        /// <summary>Informations about the possible mobility of the candidate</summary>
        public Mobility Mobility { get; set; }

        /// <summary>set of information about the background of the candidate (scholar, experiences, etc.)</summary>
        public Course Course { get; set; }

        /// <summary>Preferences of the account</summary>
        public Preferences Preferences { get; set; }

        /// <summary>Partners of the account (agency and companies)</summary>
        public Partners Partners { get; set; }


    }
}
