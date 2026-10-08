using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace BankAPI.Workers
{
    public class MyBackgroundWorker : BackgroundService
    {
        private readonly ILogger<MyBackgroundWorker> _logger;

        public MyBackgroundWorker(ILogger<MyBackgroundWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Воркер работает: {time}", DateTimeOffset.Now);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }

}
