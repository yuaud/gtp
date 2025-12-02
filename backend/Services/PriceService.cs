using backend.Data;
using backend.DTOs;
using backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class PriceService : IPriceService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<PriceService> _logger;

        public PriceService(AppDbContext db, ILogger<PriceService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<PriceDto>> GetPriceHistory(string baseCurrency, string targetCurrency, int days)
        {
            var startDate = DateTimeOffset.UtcNow.AddDays(-days);
            try
            {
                var result = await _db.Prices
                .Where(p =>
                p.BaseCurrency == baseCurrency &&
                p.TargetCurrency == targetCurrency &&
                p.API_LastUpdatedAtUtc >= startDate)
                .OrderBy(p => p.SavedAtUtc)
                .Select(p => new PriceDto
                {
                    Rate = p.Rate,
                    LastUpdatedUtc = p.API_LastUpdatedAtUtc
                })
                .ToListAsync();

                return result;
            } catch(Exception ex)
            {
                _logger.LogError(ex, "Price tablosu {Base}/{Target} icin fetch edilirken hata.", baseCurrency, targetCurrency);
                throw;
            }
            
        }
    }

}
