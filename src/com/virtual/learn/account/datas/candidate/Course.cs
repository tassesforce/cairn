using System.Collections.Generic;

namespace cairn.Models.Accounts
{ 
    /// <summary>Course of a person</summary>
    public partial class Course
    { 
        /// <summary>Scholar experience</summary>
        public List<string> Scholar { get; set; }

        /// <summary>Professional experience</summary>
        public List<string> Experiences { get; set; }

        /// <summary>Liste des missions done by the account</summary>
        public List<string> Missions { get; set; }

        /// <summary>Successes linked to this account</summary>
        public List<string> Successes { get; set; }

    }
}
