import { initializeApp } from "https://www.gstatic.com/firebasejs/9.4.0/firebase-app.js";
import { getAnalytics } from "https://www.gstatic.com/firebasejs/9.4.0/firebase-analytics.js";
import { getMessaging, getToken, onMessage, isSupported } from "https://www.gstatic.com/firebasejs/9.4.0/firebase-messaging.js";
// Initialize the Firebase app in the service worker by passing in
// your app's Firebase config object.
// https://firebase.google.com/docs/web/setup#config-object
const firebaseApp = initializeApp({
    apiKey: "AIzaSyASuH-MOS1c03N1IYBB2OsueOQUr-FIH00",
    authDomain: "social-net-otus.firebaseapp.com",
    projectId: "social-net-otus",
    storageBucket: "social-net-otus.appspot.com",
    messagingSenderId: "463276883942",
    appId: "1:463276883942:web:de2c035353548b933d2027",
    measurementId: "G-27P59E6CTR"
});


const messaging = getMessaging(firebaseApp);

const vapidKey = 'BGDwQYc4JNDasQ90YbkuSxg8TzYMz9Vg9_WufR7WmqeM6G9oVsCxlpMO6AkGQ9s0j6UaocGQoHjdrynjOyMHxk8';

getToken(messaging, { vapidKey: vapidKey }).then((currentToken) => {
    if (currentToken) {
        console.log(`current token = ${currentToken}`);
        var _navigator = {};
        for (var i in navigator) _navigator[i] = navigator[i];
        console.log('navigator = ', JSON.stringify(_navigator));

        var firebaseTokenModel = {
            token: currentToken,
            additionalData: JSON.stringify(_navigator)
        };

        fetch(`/api/FirebaseTokens/Add`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(firebaseTokenModel)
        })
            .then(response => response.json())
            .then(result => {
                console.log('result of tokenAdd = ', result);
            })
            .catch(err => {
                console.error(err);
            });

        // Send the token to your server and update the UI if necessary
        // ...
    } else {
        // Show permission request UI
        console.log('No registration token available. Request permission to generate one.');
        // ...
    }
}).catch((err) => {
    console.log('An error occurred while retrieving token. ', err);
    // ...
});

onMessage(messaging, (payload) => {
    console.log('Message received. ', payload);
    // ...
});

isSupported().then(resp => {
    console.log('Is Window supported = ', resp);
});