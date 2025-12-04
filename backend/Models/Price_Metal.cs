namespace backend.Models
{
    public class Price_Metal
    {
        public int Id { get; set; }
        public string Basecurrency { get; set; }
        public decimal XAG { get; set; }
        public decimal XAU { get; set; }
        public decimal XPT { get; set; }
        public decimal USDXAG { get; set; }
        public decimal USDXAU { get; set; }
        public decimal USDXPT { get; set; }
        public DateTimeOffset Saved_at { get; set; }
        public DateTimeOffset API_LastUpdatedUtc { get; set; }
    }
}
