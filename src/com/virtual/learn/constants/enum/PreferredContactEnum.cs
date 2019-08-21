using System;
using System.Collections.Generic;
using cairn.Enum;
using lug.Enum;

namespace cairn.Enum
{
    public sealed class PreferredContactEnum : StringEnum {

        public static readonly PreferredContactEnum PHONE = new PreferredContactEnum ("PHONE", "Téléphone");
        public static readonly PreferredContactEnum MAIL = new PreferredContactEnum ("MAIL", "Mail");
        public static readonly PreferredContactEnum WEB = new PreferredContactEnum ("WEB", "Site web");        
    
        public PreferredContactEnum(string value, string text) : base(value, text) {}
        public PreferredContactEnum() {}

        /// <summary>Retourne la liste de tous les elements de l'enum</summary>
        public override List<StringEnum> ToList()
        {
            return new List<StringEnum> {
                PHONE,
                MAIL,
                WEB
            };
        }
        
    }
}