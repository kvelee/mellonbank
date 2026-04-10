public interface ICurrencyService
{
    Task<decimal> GetUsdRateAsync();
}

public class CurrencyService : ICurrencyService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public CurrencyService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<decimal> GetUsdRateAsync()
    {   
        /* Temporary line, in case we overdo it with the API calls*/
        return 1.7002M;
        string apiKey = _configuration["FixerApi:ApiKey"];
        string url = $"http://data.fixer.io/api/latest?access_key={apiKey}&symbols=USD";

        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                using var data = System.Text.Json.JsonDocument.Parse(content);
                
                if (data.RootElement.GetProperty("success").GetBoolean())
                {
                    return data.RootElement.GetProperty("rates").GetProperty("USD").GetDecimal();
                }
            }
        }
        catch
        {
            return 1.1M;
        }
        return 1.1M;
    }
}