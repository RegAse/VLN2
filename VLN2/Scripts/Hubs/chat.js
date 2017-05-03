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
        // NEED TO FIX SOMETHING HERE
        //console.log({ "row": row, "column": column });
        //console.log(editor.session.getDocument().insertMergedLines({ "row": parseInt(row), "column": parseInt(column) }, ['', '']));
        //var val = JSON.parse(value).join(editor.document.getNewLineCharacter());

        /*if(val == "")
        {
            
        }*/
        //editor.session
        //editor.session.insert({ "row": row, "column": column }, val);
        editor.session.insert({ "row": parseInt(row), "column": parseInt(column) }, value);
    }

    chat.client.removeCode = function (row, column, endrow, endcolumn) {
        //console.log("Remove: ");
        editor.session.remove({ "start": { "row": row, "column": column }, "end": { "row": endrow, "column": endcolumn } });
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

        editor.getSession().on('change', function (e) {
            // Make sure this was a user change.
            if (editor.curOp && editor.curOp.command.name) {
                if (e.action == "insert") {
                    //console.log("Insert at row: " + e.start.row + ", col: " + e.start.column);
                    var lines = e.lines.join("\n");
                    //var lines = JSON.stringify(e.lines);

                    chat.server.insertCode(lobbyName, e.start.row, e.start.column, lines);
                }
                else if (e.action == "remove") {
                    //console.log("Remove");

                    chat.server.removeCode(lobbyName, e.start.row, e.start.column, e.end.row, e.end.column);
                }
                //console.log(e);
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