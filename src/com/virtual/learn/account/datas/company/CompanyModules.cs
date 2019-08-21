using System.Collections.Generic;

namespace cairn.Models.Accounts
{ 
    /// <summary>Linked to a company, show which modules are owned by it and which modules are mandatory for a candidate to join it</summary>
    public partial class CompanyModules
    { 
        /// <summary>Modules owned by the company</summary>
        public List<string> Dedicated { get; set; }

        /// <summary>Modules that are mandatory for a candidate to perform a mission in this company</summary>
        public List<Modules> Mandatory { get; set; }

    }
}
