using Application.Services.BinanceService.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.BinanceService
{
    internal class BinanceService : IBinanceService
    {
        private readonly ILogger<BinanceService> logger;

        public BinanceService(ILogger<BinanceService> _logger)
        {
            logger = _logger;
        }
        public Task OnConnected(Func<CriptoPriceResponse, int> onConnected)
        {
            logger.LogInformation("[{ServiceName}] On Connected Method", nameof(BinanceService));
            throw new NotImplementedException();
        }
    }
}
