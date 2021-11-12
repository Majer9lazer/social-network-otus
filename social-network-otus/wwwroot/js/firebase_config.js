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


getToken(messaging, { vapidKey: 'BLDAmpGgtGzvEj-Iu5cMBcjAGuvox8RF1gzLzjqZwmBAHOL90UIhvZ7U1XQlGwNWjeJewEf5B8SUbkTwGXbBIew' }).then((currentToken) => {
    if (currentToken) {
        console.log(`current token = ${currentToken}`);
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

console.log(messaging);

isSupported().then(resp => {
    console.log('Is Window supported = ', resp);
});