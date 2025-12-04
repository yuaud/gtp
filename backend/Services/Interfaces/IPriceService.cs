using backend.DTOs;

namespace backend.Services.Interfaces
{
    public interface IPriceService
    {
        Task<List<PriceDto>> GetPriceHistory(string baseCurrency, string targetCurrency, int days);
        Task<List<PriceMetalDto>> GetMetalPriceHistory(string targetMetal, int days);
    }
}
