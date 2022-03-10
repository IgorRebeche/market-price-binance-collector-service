using Application.Services.BinanceService;
using Application.UseCases.CollectTickersUseCase;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Tasks
{
    public class ProcessTickerTask : IHostedService
    {
        private readonly ILogger<ProcessTickerTask> _logger;
        private readonly ICollectTickersUseCase _collectTickersUseCase;
        private readonly IBinanceService _binanceService;

        public ProcessTickerTask(ILogger<ProcessTickerTask> logger, IBinanceService binanceService)
        {
            _logger = logger;
            _binanceService = binanceService;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start Job");
            List<string> pairs = new List<string>();
            pairs.Add("btcusdt");
            pairs.Add("ethusdt");
            Task.Run(async () => _binanceService.GetBookTickerStreams(pairs, msg => _logger.LogInformation($"Message received from UseCase: {msg}")));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stop Job");
            _binanceService.Disconnect();
            return Task.CompletedTask;
        }
    }
}
