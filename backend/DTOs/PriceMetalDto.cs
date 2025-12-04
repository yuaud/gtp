namespace backend.DTOs
{
    public class PriceMetalDto
    {
        public decimal XMetal { get; set; }
        public decimal USDXMetal { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
    }
}
