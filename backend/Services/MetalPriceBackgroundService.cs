
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class MetalPriceBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MetalPriceBackgroundService> _logger;

        public MetalPriceBackgroundService(IServiceProvider serviceProvider, ILogger<MetalPriceBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {

                try
                {
                    await RunIfNeeded(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "MetalPrice-API Daily Job Failed");
                }
                // Her saat scheduled task'in bugün çalışıp çalışmadığını kontrol et
                await Task.Delay(TimeSpan.FromHours(1), cancellationToken);
                // Testing
                // await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }

        private async Task RunIfNeeded(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            string taskName = "MetalPriceDailyTask";

            var log = await db.ScheduledTaskLogs
                .Where(x => x.TaskName == taskName)
                .OrderByDescending(x => x.LastRunUtc)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            var today = DateTime.UtcNow.Date;
            if (log != null && log.LastRunUtc.Date == today)
                return; // bu task bugün zaten çalışmış

            await RunJob(); //API Çağrıları

            // Çalıştığı için ScheduledTaskLog'a kayıt ekle
            log = new ScheduledTaskLog
            {
                TaskName = taskName,
                LastRunUtc = DateTime.UtcNow
            };
            db.ScheduledTaskLogs.Add(log);

            _logger.LogInformation("ScheduledTaskLogs Table Updated");
            await db.SaveChangesAsync(cancellationToken);
        }

        private async Task RunJob()
        {
            using var scope = _serviceProvider.CreateScope();
            var metalService = scope.ServiceProvider.GetRequiredService<MetalPriceService>();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            List<string?> metals = await db.Subcategories
                .Where(s => s.Category_id == 2)
                .Select(s => s.Code)
                .ToListAsync();
            string result = string.Join(",", metals);
            var dto = await metalService.GetMetals(currencies: result);

            var price_metal = new Price_Metal();

            // reflection
            foreach(var metal in metals)
            {
                // 1) Metal (XAG, XAU, XPT)
                if(dto.rates.TryGetValue(metal, out var value))
                {
                    var prop = typeof(Price_Metal).GetProperty(metal);
                    if (prop != null)
                        prop.SetValue(price_metal, value);
                }
                // 2) Base currency + metal (USDXAG, USDXAU, USDXPT)
                var combinedKey = dto.base_curr + metal; // USD + XAG = USDXAG
                if(dto.rates.TryGetValue(combinedKey, out var combinedValue))
                {
                    var combinedProp = typeof(Price_Metal).GetProperty(combinedKey);
                    if (combinedProp != null)
                        combinedProp.SetValue(price_metal, combinedValue);
                }
            }
            price_metal.Basecurrency = dto.base_curr;
            price_metal.Saved_at = DateTime.UtcNow;
            price_metal.API_LastUpdatedUtc = DateTimeOffset.FromUnixTimeSeconds(dto.timestamp);
            _logger.LogInformation("Prices_Metal Table Updated");
            db.Prices_Metal.Add(price_metal);
            await db.SaveChangesAsync();
        }
    }
}
