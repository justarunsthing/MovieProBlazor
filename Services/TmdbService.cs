using MovieProBlazor.Components.Pages;
using MovieProBlazor.Models;
using System.Net.Http.Json;
using System.Text.Json;

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

        public async Task<MovieListResponse> GetSearchResultsAsync(string query)
        {
            string url = $"https://api.themoviedb.org/3/search/movie?query={query}&language=en-US";
            var response = await _http.GetFromJsonAsync<MovieListResponse>(url, _jsonOptions)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to retrieve search results");

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

        public async Task<MovieDetails> GetMovieDetailsAsync(int movieId)
        {
            string url = $"https://api.themoviedb.org/3/movie/{movieId}";
            var response = await _http.GetFromJsonAsync<MovieDetails>(url, _jsonOptions)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to retrieve movie details");

            response.PosterPath = !string.IsNullOrEmpty(response.PosterPath)
                                  ? $"https://image.tmdb.org/t/p/w500{response.PosterPath}"
                                  : "img/poster.png";

            // Some backdrop images makes movie details hard to read
            //response.BackdropPath = !string.IsNullOrEmpty(response.BackdropPath)
            //                        ? $"https://image.tmdb.org/t/p/w500{response.BackdropPath}"
            //                        : "img/backdrop.jpg";

            return response;
        }

        public async Task<Video?> GetMovieTrailerAsync(int movieId)
        {
            string url = $"https://api.themoviedb.org/3/movie/{movieId}/videos"; // ?language=en-US
            var response = await _http.GetFromJsonAsync<MovieVideosResponse>(url, _jsonOptions)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to retrieve movie videos");

            return response.Results.FirstOrDefault(v =>
                v.Site!.Contains("YouTube", StringComparison.OrdinalIgnoreCase)
                && v.Type!.Contains("Trailer", StringComparison.OrdinalIgnoreCase));
        }

        public async Task<CreditsResponse> GetMovieCreditsAsync(int movieId)
        { 
            string url = $"https://api.themoviedb.org/3/movie/{movieId}/credits"; // ?language=en-US
            var response = await _http.GetFromJsonAsync<CreditsResponse>(url, _jsonOptions)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to retrieve movie credits");

            foreach (var cast in response.Casts)
            {
                cast.ProfilePath = !string.IsNullOrEmpty(cast.ProfilePath)
                                   ? $"https://image.tmdb.org/t/p/w500{cast.ProfilePath}"
                                   : "img/profile.jpg";
            }

            foreach (var crew in response.Crew)
            {
                crew.ProfilePath = !string.IsNullOrEmpty(crew.ProfilePath)
                                   ? $"https://image.tmdb.org/t/p/w500{crew.ProfilePath}"
                                   : "img/profile.jpg";
            }

            return response;
        }
    }
}