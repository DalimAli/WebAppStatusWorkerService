using Microsoft.Extensions.Logging;

namespace WebAppStatus
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        HttpClient httpClient;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            httpClient = new HttpClient();
            _logger.LogInformation("Worker service started");
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            httpClient.Dispose();
            _logger.LogInformation("Worker service stopped");
            await base.StopAsync(cancellationToken);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                var result = await httpClient.GetAsync("https://www.iamtimcorey.com");
                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("the website is up and running. status code is {0}", result.StatusCode);
                }
                else
                {
                    _logger.LogInformation("the website is down. status code is {0}", result.StatusCode);
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}