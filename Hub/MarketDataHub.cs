namespace RealTimeDashboard.Hub
{
    using Microsoft.AspNetCore.SignalR;
    using Newtonsoft.Json;
    using RealTimeDashboard.DTO;
    using System.Net.Http;
    using System.Threading.Tasks;
    public class MarketDataHub : Hub
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        public MarketDataHub(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
        }

        public async Task FetchMarketData()
        {
            //var httpClient = _httpClientFactory.CreateClient();
            var response = await _httpClient.GetStringAsync("https://api.coindesk.com/v1/bpi/currentprice.json");

            await Clients.All.SendAsync("ReceiveMarketData", response);
        }
        
    }

}
