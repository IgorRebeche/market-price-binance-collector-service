using Application.Services.BinanceService;
using Application.UseCases.CollectTickersUseCase;
using Market.Price.Binance.Collector.Service.Events;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Tasks
{
    public class ProcessTickerTask : IHostedService
    {
        private readonly ILogger<ProcessTickerTask> _logger;
        private readonly ICollectTickersUseCase _collectTickersUseCase;
        private readonly IBinanceService _binanceService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly string BINANCE_BROKER = "Binance";

        public ProcessTickerTask(ILogger<ProcessTickerTask> logger, IBinanceService binanceService, IServiceScopeFactory factory)
        {
            _logger = logger;
            _binanceService = binanceService;
            _publishEndpoint = factory.CreateScope().ServiceProvider.GetRequiredService<IPublishEndpoint>();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start Job");
            List<string> pairs = new List<string>();
            pairs.Add("btcusdt");
            pairs.Add("ethusdt");
            pairs.Add("bnbusdt");
            Task.Run(async () =>
            {
                await GetBookTickerStreams(pairs);
            });


            return Task.CompletedTask;
        }

        private async Task GetTickerStreams(List<string> pairs)
        {
            await _binanceService.GetTickerStreams(pairs, async msg =>
            {
                _logger.LogInformation("Message received from UseCase: {@msg}", msg);
                await _publishEndpoint.Publish<ITickerCollected>(new
                {
                    Symbol = msg.Symbol,
                    Price = msg.LastPrice,
                    Volume = msg.BidQuantity,
                    TimeStamp = msg.EventTime,
                    BrokerName = BINANCE_BROKER
                }); ;

            });
        }

        private async Task GetBookTickerStreams(List<string> pairs)
        {
            await _binanceService.GetBookTickerStreams(pairs, async msg =>
            {
                _logger.LogInformation("Message received from UseCase: {@msg}", msg);
                await _publishEndpoint.Publish<ITickerCollected>(new
                {
                    Symbol = msg.Symbol,
                    Price = msg.BidPrice,
                    Volume = msg.BidQuantity,
                    TimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    BrokerName = BINANCE_BROKER
                }); ;

            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stop Job");
            _binanceService.Disconnect();
            return Task.CompletedTask;
        }
    }
}
