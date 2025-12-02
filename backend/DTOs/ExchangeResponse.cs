using backend.Services.Customs;

namespace backend.DTOs
{
    public class ExchangeResponse
    {
        public string result { get; set; }
        public long time_last_update_unix { get; set; }
        [System.Text.Json.Serialization.JsonConverter(typeof(CustomDateTimeOffsetConverter))]
        public DateTimeOffset time_last_update_utc { get; set; }
        public string base_code { get; set; }
        public Dictionary<string, decimal> conversion_rates { get; set; }
    }
}
