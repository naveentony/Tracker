using Microsoft.Extensions.Logging;

namespace Tracker.GT06N
{
    public class GT06NWorker : BackgroundService
    {
        private readonly ILogger<GT06NWorker> _logger;

        public GT06NWorker(ILogger<GT06NWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}