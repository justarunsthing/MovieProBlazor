using System.Text.Json;
using Microsoft.JSInterop;
using MovieProBlazor.Models;

namespace MovieProBlazor.Services
{
    public class FavouritesService(IJSRuntime jSRuntime)
    {
        private readonly string _localStorageKey = "favourites";
        private readonly SemaphoreSlim _gate = new(1, 1);
        private List<Movie>? _favourites;

        public async Task<List<Movie>> GetFavouritesAsync()
        {
            await _gate.WaitAsync();
            try
            {
                // Callers must not be able to add, remove, or clear the cached list.
                return (await LoadFavouritesAsync()).ToList();
            }
            finally
            {
                _gate.Release();
            }
        }

        // Local storage is an all or nothing add/remove, i.e. cannot add or remove a single item
        public async Task SaveToFavouritesAsync(List<Movie> movies)
        {
            await _gate.WaitAsync();
            try
            {
                if (await TryPersistAsync(movies))
                {
                    _favourites = movies.ToList();
                }
            }
            finally
            {
                _gate.Release();
            }
        }

        public async Task AddToFavouritesAsync(Movie movie)
        {
            await _gate.WaitAsync();
            try
            {
                var currentFavourites = await LoadFavouritesAsync();

                if (currentFavourites.Any(m => m.Id == movie.Id))
                {
                    return;
                }

                var updated = currentFavourites.ToList();
                updated.Add(movie);
                if (await TryPersistAsync(updated))
                {
                    _favourites = updated;
                }
            }
            finally
            {
                _gate.Release();
            }
        }

        public async Task RemoveFromFavouritesAsync(Movie movie)
        {
            await _gate.WaitAsync();
            try
            {
                var currentFavourites = await LoadFavouritesAsync();
                var updated = currentFavourites.Where(m => m.Id != movie.Id).ToList();

                if (await TryPersistAsync(updated))
                {
                    _favourites = updated;
                }
            }
            finally
            {
                _gate.Release();
            }
        }

        public async Task<bool> IsFavouriteAsync(int movieId)
        {
            var currentFavourites = await GetFavouritesAsync();

            return currentFavourites.Any(m => m.Id == movieId);
        }

        private async Task<List<Movie>> LoadFavouritesAsync()
        {
            if (_favourites is not null)
            {
                return _favourites;
            }

            var json = await jSRuntime.InvokeAsync<string>("localStorage.getItem", _localStorageKey);

            if (string.IsNullOrEmpty(json))
            {
                _favourites = [];
                return _favourites;
            }

            _favourites = JsonSerializer.Deserialize<List<Movie>>(json) ?? [];
            return _favourites;
        }

        private async Task<bool> TryPersistAsync(List<Movie> movies)
        {
            try
            {
                var json = JsonSerializer.Serialize(movies);
                await jSRuntime.InvokeVoidAsync("localStorage.setItem", _localStorageKey, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
