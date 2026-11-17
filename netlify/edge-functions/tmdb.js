const allowedPath = /^(?:movie\/now_playing|movie\/popular|search\/movie|movie\/\d+(?:\/(?:videos|credits))?)$/;

export function isAllowedTmdbPath(pathname) {
    let tmdbPath;

    try {
        tmdbPath = decodeURIComponent(pathname.replace(/^\/tmdb\/?/, ''));
    } catch {
        return false;
    }

    tmdbPath = tmdbPath.replace(/^\/+|\/+$/g, '');

    if (tmdbPath.includes('..') || tmdbPath.includes('\\') || tmdbPath.includes('\0')) {
        return false;
    }

    return allowedPath.test(tmdbPath);
}

export default async (request) => {
    const key = Netlify.env.get('API_KEY');
    const configuredUrl = Netlify.env.get('API_URL')?.trim();

    if (!key || !configuredUrl) {
        return new Response('TMDB proxy is not configured', { status: 500 });
    }

    if (request.method !== 'GET') {
        return new Response('Method not allowed', { status: 405 });
    }

    const url = new URL(request.url);

    if (!isAllowedTmdbPath(url.pathname)) {
        return new Response('Not found', { status: 404 });
    }

    const tmdbUrl = configuredUrl.endsWith('/') ? configuredUrl : `${configuredUrl}/`;
    const tmdbPath = decodeURIComponent(url.pathname.replace(/^\/tmdb\/?/, '')).replace(/^\/+|\/+$/g, '');

    return await fetch(tmdbUrl + tmdbPath + url.search, {
        headers: {
            'Authorization': `Bearer ${key}`
        }
    });
}

export const config = {
    path: '/tmdb/*'
}
