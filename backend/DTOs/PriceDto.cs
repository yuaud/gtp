namespace backend.DTOs
{
    public class PriceDto
    {
        public decimal Rate { get; set; }
        public DateTimeOffset LastUpdatedUtc { get; set; }
    }
}
