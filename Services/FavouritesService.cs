using System.Text.Json;
using Microsoft.JSInterop;
using MovieProBlazor.Models;

namespace MovieProBlazor.Services
{
    public class FavouritesService
    {
        private readonly string _localStorageKey = "favourites";
        private readonly IJSRuntime _jsRuntime;

        public FavouritesService(IJSRuntime jSRuntime)
        {
            _jsRuntime = jSRuntime;
        }

        public async Task<List<Movie>> GetFavourites()
        {
            List<Movie> movies = [];

            try
            {
                var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", _localStorageKey);
                movies = JsonSerializer.Deserialize<List<Movie>>(json) ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return movies;
        }
    }
}