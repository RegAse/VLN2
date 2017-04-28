$(function () {
    var numberOfConnectedUsers = 0;
    // Reference the auto-generated proxy for the hub.
    var chat = $.connection.chatHub;

    chat.client.joined = function (connectedUsersCount) {
        numberOfConnectedUsers = connectedUsersCount;
        console.log(connectedUsersCount);
        updateNumberOfConnectedUsers();
    }

    // Create a function that the hub can call back to display messages.
    chat.client.addNewMessageToPage = function (name, message) {
        // Add the message to the page.
        $('#discussion').append('<li><strong>' + htmlEncode(name) + '</strong>: ' + htmlEncode(message) + '</li>');
    };

    // Gets called when a user joins the lobby
    chat.client.userJoinedLobby = function (name) {
        numberOfConnectedUsers++;

        updateNumberOfConnectedUsers();
        $('#discussion').append('<li><strong>' + htmlEncode(name) + ' Joined the lobby </li>');
    }
   
    // Gets called when a user leaves the lobby
    chat.client.userLeftLobby = function (name) {
        numberOfConnectedUsers--;

        updateNumberOfConnectedUsers();
        $('#discussion').append('<li><strong>' + htmlEncode(name) + ' Left the lobby </li>');
    }


    // Set initial focus to message input box.
    $('#message').focus();
    // Start the connection.
    $.connection.hub.start().done(function () {
        chat.server.hello();

        $('#sendmessage').submit(function () {
            // Call the Send method on the hub.
            chat.server.send($('#message').val());
            // Clear text box and reset focus for next comment.
            $('#message').val('').focus();

            return false
        });
    });

    function updateNumberOfConnectedUsers() {
        $("#number-of-users-connected").html("Users Connected: " + numberOfConnectedUsers);
    }
});
// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}