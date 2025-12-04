using backend.Data;
using backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class ExchangeRateService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _db;
        private readonly ILogger<ExchangeRateService> _logger;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        

        public ExchangeRateService(HttpClient httpClient, ILogger<ExchangeRateService> logger, AppDbContext db, IConfiguration config)
        {
            _httpClient = httpClient;
            _db = db;
            _logger = logger;
            _apiKey = config["ExchangeApi:ApiKey"];
            _baseUrl = config["ExchangeApi:BaseUrl"];
        }

        public async Task<ExchangeResponseDto?> GetRate(string baseCurrency, string targetCurrency)
        {
            var url = $"{_baseUrl}{_apiKey}/latest/{baseCurrency}";
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ExchangeResponse>(url);
                if (response == null)
                {
                    _logger.LogInformation("ExchangeRate-API response null");
                    return null;
                }
                if (response.conversion_rates.TryGetValue(targetCurrency, out decimal rate))
                    return new ExchangeResponseDto
                    {
                        result = response.result,
                        time_last_update_utc = response.time_last_update_utc,
                        base_code = response.base_code,
                        rate = rate
                    };
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ExchangeRate-API da bir hata meydana geldi");
                return null;
            }
        }
        public async Task<PriceDto?> GetLastRate(string baseCurrency, string targetCurrency)
        {
            try
            {
                var response = await _db.Prices
                    .Where(p => p.BaseCurrency == baseCurrency && p.TargetCurrency == targetCurrency)
                    .OrderByDescending(date => date.API_LastUpdatedAtUtc)
                    .Select(s => new PriceDto
                    {
                        Rate = s.Rate,
                        LastUpdatedUtc = s.API_LastUpdatedAtUtc
                    })
                    .FirstOrDefaultAsync();
                    if (response == null)
                    {
                        _logger.LogInformation($"{baseCurrency}->{targetCurrency} için Prices Tablosundan veriler bulunamadı.", baseCurrency, targetCurrency);
                        return null;
                    }
                    
                return response;
            } catch(Exception ex)
            {
                _logger.LogError(ex, "Prices Tablosundan veriler getirilirken hata meydana geldi.");
                return null;
            }
        }
    }
}
