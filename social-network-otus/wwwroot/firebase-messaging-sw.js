var messageNotificationOptions = {
    badge: '/icons/message.png',
    actions: [
        {
            action: 'message-read-action',
            title: 'Прочитано'
        },
        {
            action: 'message-answer-action',
            title: 'Ответить',
        }],
    requireInteraction: true
};

self.addEventListener('push', function (event) {
    // const promiseChain = self.registration.showNotification('Hello, World.');
    if (event.data) {
        var pushData = event.data.json();
        console.log('This push event has data: ', pushData);

        messageNotificationOptions.body = pushData.notification.body;
        self.registration.showNotification(pushData.notification.title, messageNotificationOptions)
    } else {
        console.log('This push event has no data.');
    }
});

self.addEventListener('notificationclick', function (event) {
    if (!event.action) {
        // Was a normal notification click
        console.log('Notification Click.');
        return;
    }

    switch (event.action) {
        case 'message-read-action':
            console.log('Пользователь прочитал сообщение.');
            event.notification.close();
            break;
        case 'message-answer-action':
            console.log('Пользователь ответил на сообщение');
            event.notification.close();
            break;
        default:
            alert('Неизвестное действие с уведомлением.')
            console.log(`Unknown action clicked: '${event.action}'`);
            break;
    }
});