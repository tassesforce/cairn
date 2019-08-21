namespace cairn.Models.Accounts
{ 
    /// <summary>Disability informations</summary>
    public partial class Disability
    { 
        /// <summary>Disable status for the account, yes or no</summary>
        public bool? DisableStatus { get; set; }

        /// <summary>Details about the disability</summary>
        public string DisabilityDetails { get; set; }

    }
}
