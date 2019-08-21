using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace cairn.Models.Accounts
{ 
    /// <summary>
    /// Objet adresse
    /// </summary>
    public partial class Adress
    { 
        /// <summary>number on the road</summary>
        [Required]
        [JsonProperty("number")]
        public string Number { get; set; }

        /// <summary>label of the street</summary>
        [Required]
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>Adress Complement</summary>
        [JsonProperty("complement1")]
        public string Complement1 { get; set; }

        /// <summary>Adress Complement</summary>
        [JsonProperty("complement2")]
        public string Complement2 { get; set; }

        /// <summary>Postal code</summary>
        [Required]
        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        /// <summary>Town</summary>
        [Required]
        [JsonProperty("town")]
        public string Town { get; set; }

        /// <summary>Country</summary>
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>override of the ToString method to correspond to our standard</summary>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Number).Append(" ");
            builder.Append(Label);
            if (!string.IsNullOrEmpty(Complement1))
            {
                builder.Append(", ").Append(Complement1);
            }
            if (!string.IsNullOrEmpty(Complement1))
            {
                builder.Append(" ").Append(Complement2).Append(",");
            }
            builder.Append(" ").Append(PostalCode);
            builder.Append(" ").Append(Town);
            builder.Append(" (").Append(Country).Append(")");

            return builder.ToString();
        }

    }
}
