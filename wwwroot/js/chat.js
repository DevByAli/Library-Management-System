"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();

connection.on("ReceiveMessage", function (message) {
    var ele = document.getElementById("notiarea");
    ele.innerText = message
});


connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    document.getElementById("typingArea").disabled = false;
});


document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("typingArea").value;
    connection.invoke("sendMessage", message).catch(function (err) {
        console.error(err.ToString());
    });
    event.preventDefault();
});

