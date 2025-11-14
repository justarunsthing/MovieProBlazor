using System.Text.Json;
using Microsoft.JSInterop;
using MovieProBlazor.Models;

namespace MovieProBlazor.Services
{
    public class FavouritesService(IJSRuntime jSRuntime)
    {
        private readonly string _localStorageKey = "favourites";

        public async Task<List<Movie>> GetFavouritesAsync()
        {
            List<Movie> movies = [];

            try
            {
                var json = await jSRuntime.InvokeAsync<string>("localStorage.getItem", _localStorageKey);
                movies = JsonSerializer.Deserialize<List<Movie>>(json) ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return movies;
        }

        // Local storage is an all or nothing add/remove, i.e. cannot add or remove a single item
        public async Task SaveToFavouritesAsync(List<Movie> movies)
        {
            try
            {
                var json = JsonSerializer.Serialize(movies);
                await jSRuntime.InvokeVoidAsync("localStorage.setItem", _localStorageKey, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task AddToFavouritesAsync(Movie movie)
        {
            var currentFavourites = await GetFavouritesAsync();

            if (currentFavourites.All(m => m.Id != movie.Id))
            {
                currentFavourites.Add(movie);
                await SaveToFavouritesAsync(currentFavourites);
            }
        }

        public async Task RemoveFromFavouritesAsync(Movie movie)
        {
            var currentFavourites = await GetFavouritesAsync();
            currentFavourites = currentFavourites.Where(m => m.Id != movie.Id).ToList();

            await SaveToFavouritesAsync(currentFavourites);
        }
    }
}