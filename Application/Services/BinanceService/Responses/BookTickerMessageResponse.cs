using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Services.BinanceService.Responses
{
    public class BookTickerMessageResponse
    {

        [JsonPropertyName("s")]
        public string Symbol { get; set; }

        [JsonPropertyName("b")]
        public string BidPrice { get; set; }

        [JsonPropertyName("B")]
        public string BidQuantity { get; set; }

        [JsonPropertyName("a")]
        public string AskPrice { get; set; }

        [JsonPropertyName("A")]
        public string AskQuantity { get; set; }
    }
}
