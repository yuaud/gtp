using backend.DTOs;

namespace backend.Services
{
    public class ExchangeRateService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExchangeRateService> _logger;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        

        public ExchangeRateService(HttpClient httpClient, ILogger<ExchangeRateService> logger, IConfiguration config)
        {
            _httpClient = httpClient;
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
    }
}
