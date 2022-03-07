using Application.Services.BinanceService.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.BinanceService
{
    internal interface IBinanceService
    {
        public Task OnConnected(Func<CriptoPriceResponse, int> onConnected);
    }
}
