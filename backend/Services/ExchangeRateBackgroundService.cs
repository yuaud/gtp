using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class ExchangeRateBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ExchangeRateBackgroundService> _logger;

        private readonly string[] currencies = new[] { "USD", "EUR", "TRY", "RUB", "UAH" };

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



                /* Backend her kapanıp açıldığında istek atmaması için:
                var now = DateTime.UtcNow;
                var nextRun = now.Date.AddDays(1).AddMinutes(5); // Bir sonraki 00:05 UTC
                var delay = nextRun - now;
                await Task.Delay(delay, cancellationToken);
                await RunJob();     // API Çağrısı
                */

                // Uygulama çalıştığında çalışsın
                //await RunJob();

                // 24 Hour Cycle
                //await Task.Delay(TimeSpan.FromHours(24), cancellationToken);

                // Testing
                // await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }

        private async Task RunIfNeeded(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            string taskName = "ExchangeRateDailyTask";

            var log = await db.ScheduledTaskLogs.FirstOrDefaultAsync(x => x.TaskName == taskName, cancellationToken: cancellationToken);

            var today = DateTime.UtcNow.Date;
            if (log != null && log.LastRunUtc.Date == today)
                return; // bu task bugün zaten çalışmış

            await RunJob(); //API Çağrıları

            // Çalıştığı için ScheduledTaskLog'a kayıt ekle
            if(log == null)
            {
                log = new ScheduledTaskLog
                {
                    TaskName = taskName,
                    LastRunUtc = DateTime.UtcNow
                };
                db.ScheduledTaskLogs.Add(log);
            }
            else
            {
                log.LastRunUtc = DateTime.UtcNow;
                db.ScheduledTaskLogs.Update(log);
            }
            _logger.LogInformation("ScheduledTaskLogs Table Updated");
            await db.SaveChangesAsync(cancellationToken);
        }

        private async Task RunJob()
        {
            using var scope = _serviceProvider.CreateScope();
            var exchangeService = scope.ServiceProvider.GetRequiredService<ExchangeRateService>();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();


            // Üç currency için kendileri hariç diğer currencylerle exchange rateleri
            // USD->TRY, USD->EUR, USD->UAH, USD->RUB,
            // TRY->USD, TRY->EUR, TRY->UAH, TRY->RUB,
            // EUR->USD, EUR->TRY, EUR->UAH, EUR->RUB,
            // RUB->USD, RUB->EUR, RUB->UAH, RUB->TRY
            // UAH->USD, UAH->EUR, UAH->RUB, UAH-TRY
            foreach(var from in currencies)
            {
                foreach(var to in currencies)
                {
                    // kendi kendine(USD->USD, TRY->TRY) exchange rate göndermesin
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
