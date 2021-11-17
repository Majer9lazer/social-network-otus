//import { initializeApp } from "https://www.gstatic.com/firebasejs/9.4.0/firebase-app.js";
//import { getAnalytics } from "https://www.gstatic.com/firebasejs/9.4.0/firebase-analytics.js";
//import { getMessaging, getToken, onMessage, isSupported } from "https://www.gstatic.com/firebasejs/9.4.0/firebase-messaging.js";

//const messaging_1 = getMessaging();
//console.log('messaging_1 = ', messaging_1);
//messaging_1.onBackgroundMessage(function (payload) {
//    console.log('[firebase-messaging-sw.js] Received background message ', payload);
//    // Customize notification here
//    const notificationTitle = 'Background Message Title';
//    const notificationOptions = {
//        body: 'Background Message body.'
//    };

//    self.registration.showNotification(notificationTitle,
//        notificationOptions);
//});

self.addEventListener('push', function (event) {
    const promiseChain = self.registration.showNotification('Hello, World.');

    event.waitUntil(promiseChain);
    if (event.data) {
        console.log('This push event has data: ', event.data.text());
    } else {
        console.log('This push event has no data.');
    }
});