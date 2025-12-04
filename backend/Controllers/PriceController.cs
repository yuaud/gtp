using backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly IPriceService _priceService;

        public PriceController(IPriceService priceService)
        {
            _priceService = priceService;
        }

        // https://localhost:7095/api/Price/USD/TRY?days=30
        [HttpGet("{baseCurrency}/{targetCurrency}")]
        public async Task<IActionResult> GetPriceHistory(
            string baseCurrency,
            string targetCurrency,
            [FromQuery] int days = 7)
        {
            var result = await _priceService.GetPriceHistory(baseCurrency, targetCurrency, days);
            if (result == null || result.Count == 0)
            {
                return NotFound($"No price data found for {baseCurrency} -> {targetCurrency}");
            }
            return Ok(result);
        }

        // https://localhost:7095/api/Price/Metals/XAU?days=30
        [HttpGet("Metals/{targetMetal}")]
        public async Task<IActionResult> GetPriceMetalHistory(
            string targetMetal,
            [FromQuery] int days = 7)
        {
            var result = await _priceService.GetMetalPriceHistory(targetMetal, days);
            if(result == null || result.Count == 0)
            {
                return NotFound($"No metal price data found for {targetMetal}");
            }
            return Ok(result);
        }
    }
}
