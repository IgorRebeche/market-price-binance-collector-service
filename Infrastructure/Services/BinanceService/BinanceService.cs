﻿using Application.Services.BinanceService;
using Application.Services.BinanceService.Responses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Websocket.Client;

namespace Infrastructure.Services.BinanceService
{
    public class BinanceService : IBinanceService
    {
        private readonly ILogger<BinanceService> _logger;
        private ManualResetEvent exitEvent;
        private WebsocketClient _client;

        public BinanceService(ILogger<BinanceService> logger)
        {
            _logger = logger;
            exitEvent = new ManualResetEvent(false);
        }
        public Task OnConnected(Func<TickerMessageResponse, int> onConnected)
        {
            _logger.LogInformation("[{ServiceName}] On Connected Method", nameof(BinanceService));
            throw new NotImplementedException();
        }

        public Task GetTickerStreams(List<string> pairs, Action<TickerMessageResponse> handleMessage)
        {
            JObject obj = new JObject();
            obj["method"] = "SUBSCRIBE";
            obj["params"] = JArray.FromObject(pairs.Select(pair => pair + "@ticker").ToArray());
            obj["id"] = 1;

            _logger.LogInformation("Message to be send {message}", obj.ToString());

            ConnectWebSocket<TickerMessageResponse>(obj, handleMessage);

            return Task.CompletedTask;
        }

        public Task GetBookTickerStreams(List<string> pairs, Action<BookTickerMessageResponse> handleMessage)
        {
            JObject obj = new JObject();
            obj["method"] = "SUBSCRIBE";
            obj["params"] = JArray.FromObject(pairs.Select(pair => pair + "@bookTicker").ToArray());
            obj["id"] = 1;

            _logger.LogInformation("Message to be send {message}", obj.ToString());

            ConnectWebSocket<BookTickerMessageResponse>(obj, handleMessage);

            return Task.CompletedTask;
        }

        public void Disconnect()
        {
            exitEvent.Set();
            _client.Dispose();
            _logger.LogInformation("Discconecting Binance Stream!");
        }

        public async Task SubscribeBookTicker(List<string> pairs)
        {
            JObject obj = new JObject();
            obj["method"] = "SUBSCRIBE";
            obj["params"] = JArray.FromObject(pairs.Select(pair => pair + "@bookTicker").ToArray());
            obj["id"] = 1;

            _client.Send(obj.ToString());
        }

        public async Task UnsubscribeBookTicker(List<string> pairs)
        {
            JObject obj = new JObject();
            obj["method"] = "UNSUBSCRIBE";
            obj["params"] = JArray.FromObject(pairs.Select(pair => pair + "@bookTicker").ToArray());
            obj["id"] = 1;

            _client.Send(obj.ToString());
        }

        private void ConnectWebSocket<T>(JObject message, Action<T> handleMessage)
        {
            _logger.LogInformation("Trying to Connect to Websocket");

            var url = new Uri("wss://stream.binance.com:9443/ws");

            _client = new WebsocketClient(url);
            _client.ReconnectTimeout = TimeSpan.FromSeconds(30);
            _client.ReconnectionHappened.Subscribe(info =>
            _logger.LogInformation($"Reconnection happened, type: {info.Type}"));

            //var a = JsonSerializer.Deserialize<ITickerMessageResponse>(msg.Text);
            _client.MessageReceived.Subscribe(msg =>
            {
                try
                {
                    var response = JsonSerializer.Deserialize<T>(msg.Text);
                       
                    if (response is null)
                    {
                        _logger.LogWarning("Something went wrong, deserialization returned null for msg = {message}", msg);
                        return;
                    }

                    handleMessage(response);
                } catch (Exception ex)
                {
                    _logger.LogWarning("Something went wrong, could not deserializate msg = {message}", msg);
                    throw;
                }

            });
            
            Task.Run(() =>
            {
                _client.Start();
                _client.Send(message.ToString());
            });

            exitEvent.WaitOne();
        }


    }
}
