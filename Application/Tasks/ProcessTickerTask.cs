using Application.Services.BinanceService;
using Application.UseCases.CollectTickersUseCase;
using Events;
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
            //pairs.Add("ethusdt");
            Task.Run(async () =>
            {
                await _binanceService.GetBookTickerStreams(pairs, async msg =>
                {
                    _logger.LogInformation($"Message received from UseCase: {msg}");
                    await _publishEndpoint.Publish<ITickerCollected>(new
                    {
                        Price = msg,
                        TimeStamp = DateTime.Now
                    });

                });
            });


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
