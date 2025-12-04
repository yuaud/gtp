using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

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
        public async Task<List<PriceMetalDto>> GetMetalPriceHistory(string targetMetal, int days)
        {
            string base_curr = "USD";
            var startDate = DateTimeOffset.UtcNow.AddDays(-days);
            try
            {
                var param = Expression.Parameter(typeof(Price_Metal), "column");
                var property = Expression.Property(param, targetMetal);
                var property2 = Expression.Property(param, base_curr + targetMetal);
                var lambda_metal = Expression.Lambda<Func<Price_Metal, decimal>>(property, param);
                var lambda_baseXmetal = Expression.Lambda<Func<Price_Metal, decimal>>(property2, param);
                var result = await _db.Prices_Metal
                    .Where(p => p.API_LastUpdatedUtc >= startDate)
                    .OrderBy(p => p.Saved_at)
                    .Select(p => new PriceMetalDto
                    {
                        USDXMetal = lambda_baseXmetal.Compile().Invoke(p),
                        XMetal = lambda_metal.Compile().Invoke(p),
                        LastUpdated = p.API_LastUpdatedUtc.ToOffset(TimeSpan.FromHours(3))
                    })
                    .ToListAsync();
                return result;
            } catch(Exception ex)
            {
                _logger.LogError(ex, "Prices_Metal tablosu {targetMetal} için fetch edilirken hata.", targetMetal);
                throw;
            }
        }
    }

}
