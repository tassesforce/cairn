using System;

namespace cairn.Constant
{
    /// <summary>Manage the constants linked to the session</summary>
    public sealed class SessionConstant
    {
        /// <summary>Represents the JWT in session</summary>
        public static readonly string JWT = "JWT";
        /// <summary>Represents the account in session</summary>
        public static readonly string ACCOUNT = "Account";
        /// <summary>Represents the accountType in session</summary>
        public static readonly string ACCOUNT_TYPE = "AccountType";
        // /// <summary>Constante de sauvegarde du login d'un compte tiers de collaborateur</summary>
        // public static readonly string USER_ID_COLLAB = "UserIdCollab";

        /// <summary>Basic constant to serve as a reference for the dates</summary>
        public static readonly DateTime EPOCH = new DateTime(1970, 1, 1);
    }
}