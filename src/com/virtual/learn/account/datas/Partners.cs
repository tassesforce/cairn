using System.Collections.Generic;

namespace cairn.Models.Accounts
{ 
    /// <summary>List of partners accounts</summary>
    public partial class Partners
    { 
        /// <summary>Ids of the partners</summary>
        public List<string> Ids { get; set; }

        /// <summary>Ids of blacklisted accounts</summary>
        public List<string> Blacklist { get; set; }

        /// <summary>Ids of favorites accounts</summary>
        public List<string> Favorites { get; set; }

    }
}
