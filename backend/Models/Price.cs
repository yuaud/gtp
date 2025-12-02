namespace backend.Models
{
    public class Price
    {
        public int Id { get; set; }
        public string BaseCurrency { get; set; }       // Base Currency (USD, EUR, TRY)     -from USD
        public string TargetCurrency { get; set; }     // Target Currency (USD, EUR, TRY)   -to   EUR
        public decimal Rate { get; set; }              // Exchange Rate
        public DateTimeOffset? SavedAtUtc { get; set; }       // Databaseye kayıt zamanı
        public DateTimeOffset API_LastUpdatedAtUtc { get; set; } // API'dan gelen Last updated datetime
    }
}
