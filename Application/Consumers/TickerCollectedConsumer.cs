using Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Consumers
{
    public class TickerCollectedConsumer :
            IConsumer<ITickerCollected>
    {
        private ILogger<TickerCollectedConsumer> _logger;

        public TickerCollectedConsumer(ILogger<TickerCollectedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ITickerCollected> context)
        {
            _logger.LogInformation("Msg Consumed: {@0}", context.Message);
            return Task.CompletedTask;
        }
    }
}
