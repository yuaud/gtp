using backend.Services.Customs;

namespace backend.DTOs
{
    public class ExchangeResponseDto
    {
        public string result { get; set; }

        [System.Text.Json.Serialization.JsonConverter(typeof(CustomDateTimeOffsetConverter))]
        public DateTimeOffset time_last_update_utc { get; set; }
        public string base_code { get; set; }
        public decimal rate { get; set; }
    }
}
