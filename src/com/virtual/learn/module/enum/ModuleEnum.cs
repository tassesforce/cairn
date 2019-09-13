using System;
using System.Collections.Generic;
using cairn.Enum;
using lug.Enum;

namespace cairn.Enum
{
    public sealed class ModuleEnum : StringEnum {

        public static readonly ModuleEnum BASE = new ModuleEnum ("Bases", "base.png");
        public static readonly ModuleEnum SAVOIR_ETRE = new ModuleEnum ("Savoir-être", "savoir_etre.png");
        public static readonly ModuleEnum SECURITE = new ModuleEnum ("Sécurité", "securite.png");        
        public static readonly ModuleEnum QUALITE = new ModuleEnum ("Qualité", "qualite.png");        
        public static readonly ModuleEnum TRANSPORT_LOGISTIQUE = new ModuleEnum ("Transport/Logistique", "logistique.png");        
        public static readonly ModuleEnum ORIENTATION = new ModuleEnum ("Orientation", "orientation.png");        
        public static readonly ModuleEnum POSTE = new ModuleEnum ("Poste", "poste.png");        
    
        public ModuleEnum(string value, string picto) : base(value, picto) {}
        public ModuleEnum() {}

        /// <summary>Retourne la liste de tous les elements de l'enum</summary>
        public override List<StringEnum> ToList()
        {
            return new List<StringEnum> {
                BASE,
                SAVOIR_ETRE,
                SECURITE,
                QUALITE,
                TRANSPORT_LOGISTIQUE,
                ORIENTATION,
                POSTE
            };
        }
        
    }
}