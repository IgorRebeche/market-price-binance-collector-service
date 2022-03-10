using Application.Services.BinanceService;
using Application.Services.BinanceService.Responses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Websocket.Client;

namespace Infrastructure.Services.BinanceService
{
    public class BinanceService : IBinanceService
    {
        private readonly ILogger<BinanceService> _logger;
        private ManualResetEvent exitEvent;

        public BinanceService(ILogger<BinanceService> logger)
        {
            _logger = logger;
            exitEvent = new ManualResetEvent(false);
        }
        public Task OnConnected(Func<CriptoPriceResponse, int> onConnected)
        {
            _logger.LogInformation("[{ServiceName}] On Connected Method", nameof(BinanceService));
            throw new NotImplementedException();
        }

        public Task GetBookTickerStreams(List<string> pairs, Action<string> handleMessage)
        {
            JObject obj = new JObject();
            obj["method"] = "SUBSCRIBE";
            obj["params"] = JArray.FromObject(pairs.Select(pair => pair + "@bookTicker").ToArray());
            obj["id"] = 1;

            _logger.LogInformation("Message to be send {message}", obj.ToString());

            ConnectWebSocket(obj, handleMessage);

            return Task.CompletedTask;
        }

        public void Disconnect()
        {
            exitEvent.Set();
            _logger.LogInformation("Discconecting Binance Stream!");
        }

        private void ConnectWebSocket(JObject message, Action<string> handleMessage)
        {
            _logger.LogInformation("Trying to Connect to Websocket");

            var url = new Uri("wss://stream.binance.com:9443/ws");

            using (var client = new WebsocketClient(url))
            {
                client.ReconnectTimeout = TimeSpan.FromSeconds(30);
                client.ReconnectionHappened.Subscribe(info =>
                    _logger.LogInformation($"Reconnection happened, type: {info.Type}"));

                client.MessageReceived.Subscribe(msg => handleMessage(msg.Text));
                client.Start();
                Task.Run(() => client.Send(message.ToString()));

                exitEvent.WaitOne();
            }
        }


    }
}
