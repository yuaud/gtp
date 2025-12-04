using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Price_Metal> Prices_Metal { get; set; }
        public DbSet<ScheduledTaskLog> ScheduledTaskLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tablo isimlerinin Tekil olması 
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Subcategory>().ToTable("Subcategory");

            // Category Id -> Subcategory Category_id FK Maplemesi
            modelBuilder.Entity<Subcategory>()
                .HasOne(c => c.Category)
                .WithMany(s => s.Subcategories)
                .HasForeignKey(s => s.Category_id);


            // --- 1. Category Seed Data ---
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Currency" },
                new Category { Id = 2, Name = "Metals" },
                new Category { Id = 3, Name = "Crypto" }
            );

            // --- 2. Subcategory Seed Data ---
            modelBuilder.Entity<Subcategory>().HasData(
                new Subcategory { Id = 1, Name = "Turkish Lira", Code = "TRY", Category_id = 1 },
                new Subcategory { Id = 2, Name = "Euro", Code = "EUR", Category_id = 1 },
                new Subcategory { Id = 3, Name = "US Dollar", Code = "USD", Category_id = 1 },
                new Subcategory { Id = 4, Name = "Ukrainian Hryvnia", Code = "UAH", Category_id = 1 },
                new Subcategory { Id = 5, Name = "Russian Ruble", Code = "RUB", Category_id = 1 },

                new Subcategory { Id = 6, Name = "Gold", Code = "XAU", Category_id = 2 },
                new Subcategory { Id = 7, Name = "Silver", Code = "XAG", Category_id = 2 },
                new Subcategory { Id = 8, Name = "Platinum", Code = "XPT", Category_id = 2 },

                new Subcategory { Id = 9, Name = "Bitcoin", Code = "BTC", Category_id = 3 },
                new Subcategory { Id = 10, Name = "Etherium", Code = "ETH", Category_id = 3 },
                new Subcategory { Id = 11, Name = "Ripple", Code = "XRP", Category_id = 3 }
            );

            modelBuilder.Entity<Price>().HasData(GetPriceSeedData());
        }
        private static List<Price> GetPriceSeedData()
        {
            //Zaman farklari
            var offsetPlus3 = new TimeSpan(3, 0, 0);
            var offsetZero = TimeSpan.Zero;

            return new List<Price>
            {
                new Price
                {
                    Id = 1,
                    BaseCurrency = "USD",
                    TargetCurrency = "EUR",
                    Rate = 0.863M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 2,
                    BaseCurrency = "USD",
                    TargetCurrency = "TRY",
                    Rate = 42.5054M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 3,
                    BaseCurrency = "USD",
                    TargetCurrency = "RUB",
                    Rate = 77.73M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 4,
                    BaseCurrency = "USD",
                    TargetCurrency = "UAH",
                    Rate = 41.98M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 5,
                    BaseCurrency = "EUR",
                    TargetCurrency = "USD",
                    Rate = 1.1588M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 6,
                    BaseCurrency = "EUR",
                    TargetCurrency = "TRY",
                    Rate = 49.2618M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 7,
                    BaseCurrency = "EUR",
                    TargetCurrency = "RUB",
                    Rate = 89.88M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 8,
                    BaseCurrency = "EUR",
                    TargetCurrency = "UAH",
                    Rate = 48.68M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 9,
                    BaseCurrency = "TRY",
                    TargetCurrency = "USD",
                    Rate = 0.02353M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 10,
                    BaseCurrency = "TRY",
                    TargetCurrency = "EUR",
                    Rate = 0.02029M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 11,
                    BaseCurrency = "TRY",
                    TargetCurrency = "RUB",
                    Rate = 1.825M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 12,
                    BaseCurrency = "TRY",
                    TargetCurrency = "UAH",
                    Rate = 0.9885M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 13,
                    BaseCurrency = "RUB",
                    TargetCurrency = "USD",
                    Rate = 0.0129M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 14,
                    BaseCurrency = "RUB",
                    TargetCurrency = "EUR",
                    Rate = 0.01113M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 15,
                    BaseCurrency = "RUB",
                    TargetCurrency = "TRY",
                    Rate = 0.548M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 16,
                    BaseCurrency = "RUB",
                    TargetCurrency = "UAH",
                    Rate = 0.5417M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 17,
                    BaseCurrency = "UAH",
                    TargetCurrency = "USD",
                    Rate = 0.02382M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 18,
                    BaseCurrency = "UAH",
                    TargetCurrency = "EUR",
                    Rate = 0.02054M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 19,
                    BaseCurrency = "UAH",
                    TargetCurrency = "TRY",
                    Rate = 1.012M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 20,
                    BaseCurrency = "UAH",
                    TargetCurrency = "RUB",
                    Rate = 1.846M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 15, 31, 849, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 11, 30, 0, 0, 1, offsetZero)
                },

                // Grup 2 - API_LastUpdatedAtUtc: 2025-12-01 00:00:01 +00:00

                new Price
                {
                    Id = 21,
                    BaseCurrency = "USD",
                    TargetCurrency = "EUR",
                    Rate = 0.8625M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 43, 821, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 22,
                    BaseCurrency = "USD",
                    TargetCurrency = "TRY",
                    Rate = 42.5085M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 44, 119, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 23,
                    BaseCurrency = "USD",
                    TargetCurrency = "RUB",
                    Rate = 77.6748M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 44, 207, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 24,
                    BaseCurrency = "USD",
                    TargetCurrency = "UAH",
                    Rate = 42.2858M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 44, 294, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 25,
                    BaseCurrency = "EUR",
                    TargetCurrency = "USD",
                    Rate = 1.1594M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 44, 377, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 26,
                    BaseCurrency = "EUR",
                    TargetCurrency = "TRY",
                    Rate = 49.2876M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 44, 463, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 27,
                    BaseCurrency = "EUR",
                    TargetCurrency = "RUB",
                    Rate = 90.4284M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 44, 545, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 28,
                    BaseCurrency = "EUR",
                    TargetCurrency = "UAH",
                    Rate = 48.9455M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 44, 628, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 29,
                    BaseCurrency = "TRY",
                    TargetCurrency = "USD",
                    Rate = 0.02352M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 44, 715, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 30,
                    BaseCurrency = "TRY",
                    TargetCurrency = "EUR",
                    Rate = 0.02029M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 44, 802, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 31,
                    BaseCurrency = "TRY",
                    TargetCurrency = "RUB",
                    Rate = 1.8244M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 44, 887, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 32,
                    BaseCurrency = "TRY",
                    TargetCurrency = "UAH",
                    Rate = 0.9951M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 44, 979, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 33,
                    BaseCurrency = "RUB",
                    TargetCurrency = "USD",
                    Rate = 0.01283M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 45, 076, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 34,
                    BaseCurrency = "RUB",
                    TargetCurrency = "EUR",
                    Rate = 0.01111M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 45, 172, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 35,
                    BaseCurrency = "RUB",
                    TargetCurrency = "TRY",
                    Rate = 0.5481M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 45, 254, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 36,
                    BaseCurrency = "RUB",
                    TargetCurrency = "UAH",
                    Rate = 0.5423M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 45, 343, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 37,
                    BaseCurrency = "UAH",
                    TargetCurrency = "USD",
                    Rate = 0.02365M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 45, 427, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 38,
                    BaseCurrency = "UAH",
                    TargetCurrency = "EUR",
                    Rate = 0.02043M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 45, 505, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 39,
                    BaseCurrency = "UAH",
                    TargetCurrency = "TRY",
                    Rate = 1.0050M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 45, 592, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                },
                new Price
                {
                    Id = 40,
                    BaseCurrency = "UAH",
                    TargetCurrency = "RUB",
                    Rate = 1.8440M,
                    SavedAtUtc = new DateTimeOffset(2025, 12, 1, 22, 52, 45, 680, offsetPlus3),
                    API_LastUpdatedAtUtc = new DateTimeOffset(2025, 12, 1, 0, 0, 1, offsetZero)
                }
            };
        }
    }
}
