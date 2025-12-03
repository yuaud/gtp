using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class ExchangeRateBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ExchangeRateBackgroundService> _logger;

        public ExchangeRateBackgroundService(IServiceProvider serviceProvider, ILogger<ExchangeRateBackgroundService> logger)
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
                }catch(Exception ex)
                {
                    _logger.LogError(ex, "Daily Job Failed");
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

            string taskName = "ExchangeRateDailyTask";

            var log = await db.ScheduledTaskLogs
                .Where(x => x.TaskName == taskName)
                .OrderByDescending(x => x.LastRunUtc)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            //var turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            //var todayTurkey = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, turkeyTimeZone).Date;

            /* API UTC + 0'da güncellendiği için UtcNow olarak alıyorum, tr-TR saati 3 saat ileride.
             Yani TR saati ile 03:00 olduğunda today değişkeni bir sonraki güne geçecek if bloğu atlanacak. */
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
            var exchangeService = scope.ServiceProvider.GetRequiredService<ExchangeRateService>();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            List<string?> currencies = await db.Subcategories
                .Where(s => s.Category_id == 1)
                .Select(s => s.Code)
                .ToListAsync();
            // Her bir currency için kendileri hariç(USD->USD Yasak) diğer currencylerle exchange rateleri
            // USD->TRY, USD->EUR, USD->UAH, USD->RUB,
            // TRY->USD, TRY->EUR, TRY->UAH, TRY->RUB,
            foreach(var from in currencies)
            {
                foreach(var to in currencies)
                {
                    // kendi kendine(USD->USD, TRY->TRY) istek göndermesin
                    if (from == to) continue;

                    var dto = await exchangeService.GetRate(from, to);
                    if (dto == null) continue;
                    var price = new Price
                    {
                        BaseCurrency = from,
                        TargetCurrency = to,
                        Rate = dto.rate,
                        SavedAtUtc = DateTimeOffset.UtcNow,
                        API_LastUpdatedAtUtc = dto.time_last_update_utc
                    };
                    db.Prices.Add(price);
                }
            }
            _logger.LogInformation("Prices Table Updated");
            await db.SaveChangesAsync();
        }
    }
}
