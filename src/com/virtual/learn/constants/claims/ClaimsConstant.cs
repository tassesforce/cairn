using System;
using System.Security.Claims;

namespace cairn.Constant
{
    /// <summary>Constants about the management of claims</summary>
    public sealed class ClaimsConstant
    {
        
        /// <summary>Constant to recover the userId in the claims</summary>
        public static readonly string USER_ID = "nameid";

        /// <summary>Constant to recover the login in the claims</summary>
        public static readonly string LOGIN = "unique_name";

        /// <summary>Constant to recover the expiration time in the claims</summary>
        public static readonly string EXPIRE = "exp";
        
        /// <summary>Constant to recover the accountType in the claims</summary>
        public static readonly string ACCOUNT_TYPE = ClaimTypes.Spn;

    }
}
