using Newtonsoft.Json;
using RealTimeDashboard.DTO;

namespace RealTimeDashboard.Hub
{
    using Microsoft.AspNetCore.SignalR;
    public class CurrencyHub : Hub
    {
        private readonly HttpClient _httpClient;

        public CurrencyHub(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendCurrencyRates()
        {
            // Get exchange rates from external API
            var exchangeRates = await GetExchangeRates();

            // Get country flags using currency codes
            var ratesWithFlags = await AddCountryFlags(exchangeRates);

            await Clients.All.SendAsync("ReceiveCurrencyRates", ratesWithFlags);
        }

        private async Task<Dictionary<string, decimal>> GetExchangeRates()
        {
            // Example using Exchange Rates Data API
            var response = await _httpClient.GetStringAsync("https://api.exchangerate-api.com/v4/latest/USD");
            var data = JsonConvert.DeserializeObject<ExchangeRateResponse>(response);
            return data.Rates;
        }

        private async Task<Dictionary<string, CurrencyInfo>> AddCountryFlags(Dictionary<string, decimal> rates)
        {
            var result = new Dictionary<string, CurrencyInfo>();

            foreach (var rate in rates)
            {
                try
                {
                    if(rate.Key =="INR")
                    {

                    }
                    // Get country info using RestCountries API
                    var countryResponse = await _httpClient.GetStringAsync(
                        $"https://restcountries.com/v3/currency/{rate.Key}");

                    var countryData = JsonConvert.DeserializeObject<List<CountryInfo>>(countryResponse);

                    result[rate.Key] = new CurrencyInfo
                    {
                        Rate = rate.Value,
                        Flag = countryData?.FirstOrDefault()?.Flags?.Count > 1 ? countryData?.FirstOrDefault()?.Flags[1] : countryData?.FirstOrDefault()?.Flags[0],
                        CurrencyName = countryData?.FirstOrDefault()?.Currencies?.FirstOrDefault().Value.Name
                    };
                }
                catch (Exception ex)
                {

                    ;
                }
            }

            return result;
        }
    }
}
