using Microsoft.JSInterop;

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
    }
}