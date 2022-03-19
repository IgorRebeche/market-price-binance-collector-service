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
        public Task OnConnected(Func<TickerMessageResponse, int> onConnected);

        public Task GetTickerStreams(List<string> pairs, Action<TickerMessageResponse> handleMessage);

        public Task GetBookTickerStreams(List<string> pairs, Action<BookTickerMessageResponse> handleMessage);

        public void Disconnect();
    }
}
