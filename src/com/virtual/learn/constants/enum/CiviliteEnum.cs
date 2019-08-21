using System.Collections;
using System.Collections.Generic;
using lug.Enum;

namespace cairn.Enum
{
    /// <summary>Enum which manages the civility of a person</summary>
    public class CiviliteEnum : StringEnum
    {

        public static readonly CiviliteEnum MONSIEUR = new CiviliteEnum ("M", "M.");  
        public static readonly CiviliteEnum MADAME = new CiviliteEnum ("MME", "Mme");  
        public static readonly CiviliteEnum MADEMOISELLE = new CiviliteEnum ("MLLE", "Mlle");  

        /// <summary>Default constructor</summary>
        protected CiviliteEnum(string value, string text) : base(value, text)
        {
        }

        /// <summary>Default constructor</summary>
        public CiviliteEnum()
        {
        }

        /// <summary>Returns the list of all the elements of the enum</summary>
        public override List<StringEnum> ToList()
        {
            return new List<StringEnum> {
                MONSIEUR,
                MADAME,
                MADEMOISELLE
            };
        }

    }
}