using System.Text.Json.Serialization;

namespace backend.DTOs
{
    public class MetalResponseDto
    {
        [JsonPropertyName("base")]
        public string base_curr { get; set; }
        public long timestamp { get; set; }
        public Dictionary<string, decimal> rates { get; set; }
    }
}
