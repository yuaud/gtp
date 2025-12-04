using backend.DTOs;
using Serilog;

namespace backend.Services
{
    public class MetalPriceService
    {
        private readonly HttpClient _http;
        private readonly ILogger<MetalPriceService> _logger;

        private readonly string _apikey;
        private readonly string _baseurl;

        public MetalPriceService(HttpClient http, ILogger<MetalPriceService> logger, IConfiguration config)
        {
            _http = http;
            _logger = logger;
            _apikey = config["MetalsApi:ApiKey"];
            _baseurl = config["MetalsApi:BaseUrl"];
        }

        public async Task<MetalResponseDto?> GetMetals(string base_curr = "USD", string currencies = "")
        {
            // https://api.metalpriceapi.com/v1/latest?api_key=&base=USD&currencies=
            var url = $"{_baseurl}?api_key={_apikey}&base={base_curr}&currencies={currencies}";
            try
            {
                var response = await _http.GetFromJsonAsync<MetalResponseDto>(url);
                if(response == null)
                {
                    _logger.LogInformation("MetalPrice-API Response is null");
                    return null;
                }
                return response;
            } catch(Exception ex)
            {
                _logger.LogError(ex, "MetalPrice-API'da bir hata meydana geldi.");
                return null;
            }
        } 
    }
}
