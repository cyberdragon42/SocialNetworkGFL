const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/notify")
    .build();

hubConnection.on("SendNotification", 
    function (notificationsCount) {
        document.getElementById("notificationBadge").textContent = notificationsCount;
});

hubConnection.start().then(function () {
    console.log("connection started");
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("unfollowBtn").addEventListener("click", function (e) {
    let userId = document.getElementById("unfollowBtn").getAttribute("data-userId");
    let message = "unfollow";
    hubConnection.invoke("SendNotification", message, userId);
});

document.getElementById("followBtn").addEventListener("click", function (e) {
    let userId = document.getElementById("followBtn").getAttribute("data-userId");
    let message = "follow";
    hubConnection.invoke("SendNotification", message, userId);
});