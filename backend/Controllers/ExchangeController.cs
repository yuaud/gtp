using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly ExchangeRateService _exchangeRateService;

        public ExchangeController(ExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet("{from}/{to}")]
        public async Task<IActionResult> GetRate(string from, string to)
        {
            var rate = await _exchangeRateService.GetRate(from, to);
            if(rate == null)
                return NotFound();
            return Ok(new { from, to, rate });
        }
    }
}
