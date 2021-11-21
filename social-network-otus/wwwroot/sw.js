const CACHE_NAME = 'my-site-cache-v1';
var urlsToCache = [
    '/css/site.css',
    '/js/signalr/dist/browser/signalr.js',
    '/lib/bootstrap-icons/font/bootstrap-icons.min.css',
    '/lib/jquery/dist/jquery.min.js',
    '/lib/bootstrap_5/dist/js/bootstrap.bundle.min.js',
    '/lib/axios/axios.min.js',
    '/lib/vue/dist/vue.global.prod.js',
    '/lib/bootstrap-icons/font/fonts/bootstrap-icons.woff2?a97b3594ad416896e15824f6787370e0',
    '/icons/site.webmanifest',
    'https://www.gstatic.com/firebasejs/9.4.0/firebase-app.js',
    'https://www.gstatic.com/firebasejs/9.4.0/firebase-analytics.js',
    'https://www.gstatic.com/firebasejs/9.4.0/firebase-messaging.js',
    '/icons/favicon-32x32.png',
    '/icons/android-chrome-192x192.png',
];
var urlsNotToCache = [
    '/Profile',
    '/Profile/GetPaginated'
];

self.addEventListener('install', function (event) {
    console.log('sw install event');

    // Perform install steps
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(function (cache) {
                console.log('Opened cache. cache = ', cache);
                return cache.addAll(urlsToCache);
            })
    );
});

self.addEventListener('fetch', function (event) {
    event.respondWith(
        caches.open(CACHE_NAME).then(function (cache) {
            return cache.match(event.request).then(function (response) {
                return (
                    response ||
                    fetch(event.request).then(function (response) {
                        console.log('sw fetch event. request = ', event.request.url);
                        if (!/^https?:$/i.test(new URL(event.request.url).protocol)) {
                            return response;
                        }
                        var pathName = new URL(event.request.url).pathname;
                        console.log('pathName = ', pathName);
                        if (urlsNotToCache.indexOf(pathName) === -1) {
                            cache.put(event.request, response.clone());
                        }

                        return response;
                    })
                );
            });
        }),
    );
});

self.addEventListener('activate', function (event) {
    var cacheAllowlist = [CACHE_NAME];
    console.log('sw activate event.')
    event.waitUntil(
        caches.keys().then(function (cacheNames) {
            return Promise.all(
                cacheNames.map(function (cacheName) {
                    if (cacheAllowlist.indexOf(cacheName) === -1) {
                        return caches.delete(cacheName);
                    }
                })
            );
        })
    );
});