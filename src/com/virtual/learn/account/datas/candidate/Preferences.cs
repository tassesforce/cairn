using System.Collections.Generic;

namespace cairn.Models.Accounts
{ 
    /// <summary>Preferences of an account</summary>
    public partial class Preferences
    { 
        /// <summary>Favourite sectors</summary>
        public List<string> DesiredSectors { get; set; }

        /// <summary>Favourite Types of contract</summary>
        public List<string> DesiredContracts { get; set; }

        /// <summary>Favourite types of schedule</summary>
        public List<string> ScheduleType { get; set; }

        /// <summary>Favourites trades (Soudeur, Cariste)</summary>
        public List<string> DesiredTrades { get; set; }

    }
}
