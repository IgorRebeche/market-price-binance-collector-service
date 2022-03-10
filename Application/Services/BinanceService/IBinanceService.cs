using Application.Services.BinanceService.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.BinanceService
{
    public interface IBinanceService
    {
        public Task OnConnected(Func<CriptoPriceResponse, int> onConnected);

        public Task GetBookTickerStreams(List<string> pairs, Action<string> handleMessage);

        public void Disconnect();
    }
}
