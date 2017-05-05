$(function () {
    var editor = ace.edit("editor");
    editor.setTheme("ace/theme/monokai");
    editor.getSession().setMode("ace/mode/javascript");
    editor.$blockScrolling = Infinity

    var numberOfConnectedUsers = 0;
    var lobbyName = location.href.substr(location.href.lastIndexOf('/') + 1);
    // Reference the auto-generated proxy for the hub.
    var chat = $.connection.chatHub;

    chat.client.joined = function (connectedUsersCount, users) {
        numberOfConnectedUsers = connectedUsersCount;
        console.log(connectedUsersCount);
        updateNumberOfConnectedUsers();
    }

    // Create a function that the hub can call back to display messages.
    chat.client.addNewMessageToPage = function (name, message) {
        // Add the message to the page.
        $('#discussion').append('<li><strong>' + htmlEncode(name) + '</strong>: ' + htmlEncode(message) + '</li>');
        console.log($('#discussion')[0].scrollHeight);
        $('.sidebar-chat').scrollTop($('.sidebar-chat')[0].scrollHeight);
    };

    // Gets called when a user joins the lobby
    chat.client.userJoinedLobby = function (name) {
        numberOfConnectedUsers++;

        updateNumberOfConnectedUsers();
        $('#discussion').append('<li><strong>' + htmlEncode(name) + ' Joined the lobby </li>');
        $('.sidebar-chat').scrollTop($('#sidebar-chat')[0].scrollHeight);
    }
   
    // Gets called when a user leaves the lobby
    chat.client.userLeftLobby = function (name) {
        numberOfConnectedUsers--;

        updateNumberOfConnectedUsers();
        $('#discussion').append('<li><strong>' + htmlEncode(name) + ' Left the lobby </li>');
        $('.sidebar-chat').scrollTop($('.sidebar-chat')[0].scrollHeight);
    }

    chat.client.insertCode = function (row, column, value) {
        editor.session.insert({ "row": parseInt(row), "column": parseInt(column) }, value);
    }

    chat.client.removeCode = function (row, column, endrow, endcolumn) {
        editor.session.remove({ "start": { "row": row, "column": column }, "end": { "row": endrow, "column": endcolumn } });
    }

    chat.client.newFileAdded = function (filename) {
        $('#files').append('<li><strong>' + htmlEncode(filename) + '</li>');
    }

    // Set initial focus to message input box.
    $('#message').focus();
    // Start the connection.
    $.connection.hub.start().done(function () {
        //chat.server.hello();
        chat.server.joinLobby(lobbyName);

        $('#sendmessage').submit(function () {
            // Call the Send method on the hub.
            chat.server.send(lobbyName, $('#message').val());
            // Clear text box and reset focus for next comment.
            $('#message').val('').focus();

            return false
        });

        $("#createfile").submit(function () {
            chat.server.addFile(lobbyName, $("#newfile").val());

            return false;
        });

        editor.getSession().on('change', function (e) {
            // Make sure this was a user change.
            if (editor.curOp && editor.curOp.command.name) {
                if (e.action == "insert") {
                    var lines = e.lines.join("\n");

                    chat.server.insertCode(lobbyName, e.start.row, e.start.column, lines);
                }
                else if (e.action == "remove") {
                    chat.server.removeCode(lobbyName, e.start.row, e.start.column, e.end.row, e.end.column);
                }
            }
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