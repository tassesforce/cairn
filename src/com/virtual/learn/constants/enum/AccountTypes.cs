using System;
using System.Collections.Generic;
using cairn.Enum;
using lug.Enum;

namespace cairn.Enum
{
    /// <summary>Enum to manage the types of account</summary>
    public sealed class AccountTypesEnum : StringEnum {

        /// <summary>Account type agency (moral)</summary>
        public static readonly AccountTypesEnum AGENCY = new AccountTypesEnum ("agency", "Agence");

        /// <summary>Account type candidate</summary>
        public static readonly AccountTypesEnum CANDIDATE = new AccountTypesEnum ("candidate", "Intérimaire");

        /// <summary>Account type company (moral)</summary>
        public static readonly AccountTypesEnum COMPANY = new AccountTypesEnum ("company", "Entreprise");     

        /// <summary>Account type collaborator (agency or company)</summary>
        public static readonly AccountTypesEnum COLLABORATOR = new AccountTypesEnum ("collaborator", "Collaborateur");     
           
        /// <summary>Default constructor</summary>            
        private AccountTypesEnum(string value, string text) : base(value, text) {}

        /// <summary>Default constructor</summary>            
        public AccountTypesEnum() {}

        /// <summary>Returns the list of all the elements of the enum</summary>
        public override List<StringEnum> ToList()
        {
            return new List<StringEnum> {
                AGENCY,
                CANDIDATE,
                COMPANY,
                COLLABORATOR
            };
        }

    }
}