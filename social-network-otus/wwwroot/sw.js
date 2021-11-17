//self.addEventListener('install', function (event) {
//    event.waitUntil(
//        caches.open('v1').then(function (cache) {
//            return cache.addAll([
//                '/Home/Index',
//                'Profile/RandomUser'
//            ]);
//        })
//    );
//});

//self.addEventListener('fetch', function (event) {
//    event.respondWith(caches.match(event.request).then(function (response) {
//        // caches.match() always resolves
//        // but in case of success response will have value
//        if (response !== undefined) {
//            return response;
//        } else {
//            return fetch(event.request).then(function (response) {

//                // response may be used only once
//                // we need to save clone to put one copy in cache
//                // and serve second one
//                const responseClone = response.clone();

//                if (!/^https?:$/i.test(new URL(event.request.url).protocol)) {
//                    return response;
//                }

//                caches.open('v1').then(function (cache) {
//                    cache.put(event.request, responseClone);
//                });

//                return response;
//            }).catch(function () {
//                return caches.match("~/icons/mstile-150x150.png");
//            });
//        }
//    }));
//});