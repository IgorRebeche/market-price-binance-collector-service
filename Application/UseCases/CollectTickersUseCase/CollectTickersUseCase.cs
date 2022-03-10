using Application.Services.BinanceService;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.CollectTickersUseCase
{
    public class CollectTickersUseCase : ICollectTickersUseCase
    {
        private readonly IBinanceService _binanceService;
        private readonly ILogger<CollectTickersUseCase> _logger;

        public CollectTickersUseCase(IBinanceService binanceService, ILogger<CollectTickersUseCase> logger)
        {
            _binanceService = binanceService;
            _logger = logger;
        }
        public void RunAsync()
        {
            List<string> pairs = new List<string>();
            pairs.Add("btcusdt");
            pairs.Add("ethusdt");

            _binanceService.GetBookTickerStreams(pairs, msg => _logger.LogInformation($"Message received from UseCase: {msg}"));
        }
    }
}
