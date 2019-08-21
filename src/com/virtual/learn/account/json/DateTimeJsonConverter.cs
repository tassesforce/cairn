using Newtonsoft.Json.Converters;

namespace cairn.Json
{
    /// <summary>Convert string to DateTime from or to JSON</summary>
    class DateTimeJsonConverter : IsoDateTimeConverter
    {
        public DateTimeJsonConverter()
        {
            // Basic format for the dates in the application 
            base.DateTimeFormat = "dd/MM/yyyy";
        }
    }
}