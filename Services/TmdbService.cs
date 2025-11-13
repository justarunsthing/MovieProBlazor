using System.Text.Json;
using System.Net.Http.Json;
using MovieProBlazor.Models;

namespace MovieProBlazor.Services
{
    public class TmdbService
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        public TmdbService(HttpClient httpClient, IConfiguration config)
        {
            _http = httpClient;
            string? tmdbAccessKey = config["TmdbAccessKey"];

            if (!string.IsNullOrEmpty(tmdbAccessKey))
            {
                _http.DefaultRequestHeaders.Authorization = new("Bearer", tmdbAccessKey);
            }
        }

        public async Task<MovieListResponse> GetNowPlayingMoviesAsync()
        {
            string url = "https://api.themoviedb.org/3/movie/now_playing?language=en-US";
            var response = await _http.GetFromJsonAsync<MovieListResponse>(url, _jsonOptions)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to retrieve now playing movies");

            foreach (var movie in response.Results)
            {
                if (!string.IsNullOrEmpty(movie.PosterPath))
                {
                    movie.PosterPath = $"https://image.tmdb.org/t/p/w500{movie.PosterPath}";
                }
                else
                {
                    movie.PosterPath = "img/poster.png";
                }
            }

            return response;
        }

        public async Task<MovieListResponse> GetPopularMoviesAsync()
        {
            string url = "https://api.themoviedb.org/3/movie/popular?language=en-US";
            var response = await _http.GetFromJsonAsync<MovieListResponse>(url, _jsonOptions)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to retrieve popular movies");

            foreach (var movie in response.Results)
            {
                if (!string.IsNullOrEmpty(movie.PosterPath))
                {
                    movie.PosterPath = $"https://image.tmdb.org/t/p/w500{movie.PosterPath}";
                }
                else
                {
                    movie.PosterPath = "img/poster.png";
                }
            }

            return response;
        }
    }
}