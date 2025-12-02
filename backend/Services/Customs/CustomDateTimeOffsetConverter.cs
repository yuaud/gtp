using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace backend.Services.Customs
{
    public class CustomDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        // API'den gelen string'in formatı
        private const string DateTimeFormat = "ddd, dd MMM yyyy HH:mm:ss zzzz";
        private readonly ILogger<CustomDateTimeOffsetConverter> _logger;

        public CustomDateTimeOffsetConverter(ILogger<CustomDateTimeOffsetConverter> logger)
        {
            _logger = logger;
        }

        public override DateTimeOffset Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if(reader.TokenType != JsonTokenType.String)
            {
                _logger.LogError($"Beklenen token tipi String iken, {reader.TokenType} geldi.");
                throw new JsonException($"Beklenen token tipi String iken, {reader.TokenType} geldi.");
            }
            string dateString = reader.GetString();

            // Gelen string'i belirli format ve InvariantCulture kullanarak ayrıştır
            if(DateTimeOffset.TryParseExact(
                dateString,
                DateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal,
                out var parsedDate))
            {
                return parsedDate;
            }
            _logger.LogError($"'{dateString}' formatında tarih ayrıştırılamadı.");
            throw new JsonException($"'{dateString}' formatında tarih ayrıştırılamadı.");
        }

        // Serileştirme (Write) işlemi gerekmediği için basit bırakılacak
        public override void Write(
            Utf8JsonWriter writer,
            DateTimeOffset value,
            JsonSerializerOptions options)
        {
            // DateTimeOffset'i JSON'a yazarken tekrar aynı formatta yazmak için ama kullanılmayacak.
            writer.WriteStringValue(value.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
        }
    }
}
