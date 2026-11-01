# MoviePro

A Blazor WebAssembly app for browsing movies. Lists, search, and detail pages come from [The Movie Database](https://www.themoviedb.org/) (TMDB). Favourites stay in this browser.

This product uses the TMDB API but is not endorsed or certified by TMDB.

## What you can do

- Home, with links into the main lists.
- Now Playing and Popular, loaded from TMDB.
- Search by title from the nav bar. Results are at `/search?query=...`.
- A movie page at `/movie-details/{id}` with the poster, release date, runtime, genres, overview, user score, a YouTube trailer when TMDB has one, and a cast row.
- Favourites. The list is stored under the `favourites` key in `localStorage`, so it belongs to this browser and there is no account.
- A theme switcher at `/themes`.

Missing posters and profile photos fall back to images in `wwwroot/img`.

## Run it locally

You need the .NET 8 SDK and a TMDB read access token (the long API Read Access Token, used as a Bearer token).

`wwwroot/appsettings.Development.json` is gitignored. Create it:

```json
{
  "TmdbAccessKey": "YOUR_TMDB_READ_ACCESS_TOKEN"
}
```

Then, from the repository root:

```bash
dotnet run --launch-profile https
```

Open `https://localhost:7193`.

When `TmdbAccessKey` is set, `TmdbService` calls `https://api.themoviedb.org/3/` directly from the browser. Leave the file uncommitted.

## Netlify

The published site does not put the token in the browser. `TmdbService` calls `/tmdb/...` on the same origin, and `netlify/edge-functions/tmdb.js` forwards that path to TMDB with the token.

Set these in the Netlify site environment:

| Name | Value |
| --- | --- |
| `API_KEY` | TMDB read access token |
| `API_URL` | `https://api.themoviedb.org/3/` |

`netlify.build.sh` installs the .NET 8 SDK and publishes to `release`. Use that script as the build command and `release/wwwroot` as the publish directory. `wwwroot/_redirects` sends unknown paths back to `index.html` so client-side routes survive a refresh, and leaves `/tmdb/*` for the edge function.

## Stack

- .NET 8, Blazor WebAssembly
- Bootstrap
- TMDB API
- Netlify Edge Functions, for the hosted API proxy