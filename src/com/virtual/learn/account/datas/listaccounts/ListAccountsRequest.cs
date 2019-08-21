using Newtonsoft.Json;

namespace cairn.Accounts.List
{
    public class ListAccountsRequest
    {
        [JsonProperty("accountType")]
        public string AccountType {get; set;}
        [JsonProperty("societyName")]
        public string SocietyName {get; set;}
        [JsonProperty("name")]
        public string Name {get; set;}
        [JsonProperty("firstName")]
        public string FirstName {get; set;}
        [JsonProperty("sector")]
        public string Sector {get; set;}
        [JsonProperty("town")]
        public string Town {get; set;}
        [JsonProperty("postalCode")]
        public string PostalCode {get; set;}
        [JsonProperty("county")]
        public string County {get; set;}
        [JsonProperty("dispoDate")]
        public string DispoDate {get; set;}
    }
}