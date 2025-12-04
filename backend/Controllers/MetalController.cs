using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetalController : ControllerBase
    {
        private readonly MetalPriceService _metalPriceService;

        public MetalController(MetalPriceService metalPriceService)
        {
            _metalPriceService = metalPriceService;
        }


        // api/Metal/USD?currencies=XAU,XAG,XPT
        [HttpGet("{base_curr}")]
        public async Task<IActionResult> GetMetals(string base_curr, [FromQuery] string currencies)
        {
            var metals = await _metalPriceService.GetMetals(base_curr, currencies);
            if(metals == null)
                return NotFound();
            return Ok(metals);
        }
    }
}
