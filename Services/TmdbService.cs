namespace MovieProBlazor.Services
{
    public class TmdbService
    {
        private readonly HttpClient _http;
        private readonly string? _tmdbAccessKey;

        public TmdbService(HttpClient httpClient, IConfiguration config)
        {
            _http = httpClient;
            _tmdbAccessKey = config["TmdbAccessKey"];

            if (!string.IsNullOrEmpty(_tmdbAccessKey))
            {
                _http.DefaultRequestHeaders.Authorization = new("Bearer", _tmdbAccessKey);
            }
        }
    }
}