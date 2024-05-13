"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    console.log("ReceiveMessage called with user: " + user + ", message: " + message);
    var li = document.createElement("li");
    li.textContent = `${user} says ${message}`;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("ReceiveFile", function (user, fileName) {
    console.log("ReceiveFile called with user: " + user + ", file name: " + fileName);
    var li = document.createElement("li");
    var link = document.createElement("a");
    link.textContent = `File shared by ${user}: ${fileName}`;
    link.href = `/uploads/${fileName}`;
    li.appendChild(link);
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    var fileInput = document.getElementById("fileInput");

    if (fileInput.files.length > 0) {
        var file = fileInput.files[0];
        var formData = new FormData();
        formData.append("file", file);
        formData.append("user", user);
        formData.append("message", message);
        console.info("Senging  message for user: " + user + ", message: " + message + ", file: " + file.fileName);

        fetch('/Customer/Message/SendMessage', {
            method: 'POST',
            body: formData
        }).then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        }).then(json => {
            if (json.success) {
                var li = document.createElement("li");
                var link = document.createElement("a");
                link.textContent = `File shared by ${user}: ${json.fileName}`;
                link.href = `/uploads/${json.fileName}`;
                li.appendChild(link);
                document.getElementById("messagesList").appendChild(li);
            }
        }).catch(e => {
            console.error('An error occurred while sending the file: ' + e.message);
        });
    } else {
        connection.invoke("SendMessage", user, message).catch(function (err) {
            console.error(err.toString());
        });
    }

    event.preventDefault();
});
