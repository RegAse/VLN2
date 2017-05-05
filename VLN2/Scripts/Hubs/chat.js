/*
    Set up ProjectOptions
*/

function ProjectOptions(){
    this.theme = "ace/theme/monokai";
    this.mode = "ace/mode/javascript";
}

ProjectOptions.prototype = {
    setTheme: function(){
        console.log("SetTheme");
    },
    setMode: function(){
        console.log("SetMode");
    }
}

/* Set up ProjectSession */
function ProjectSession(projectOptions){
    this.editor = ace.edit("editor");
    this.numberOfConnectedUsers = 0;
    this.lobbyName = location.href.substr(location.href.lastIndexOf('/') + 1);

    var editor = ace.edit("editor");
    editor.setTheme(projectOptions["theme"]);
    editor.getSession().setMode(projectOptions.mode);
    editor.$blockScrolling = Infinity;

    // Reference the chathub
    this.chat = $.connection.chatHub;
    this.setupChat();
}

ProjectSession.prototype.setupChat = function (){

}

ProjectSession.prototype = {
    setupChat: function () {
        var chat = this.chat;
        var lobbyName = this.lobbyName;
        var editor = this.editor;

        chat.client.joined = function (connectedUsersCount, users){
            numberOfConnectedUsers = connectedUsersCount;
            console.log(connectedUsersCount);
            updateNumberOfConnectedUsers();
        }

        // Create a function that the hub can call back to display messages.
        chat.client.addNewMessageToPage = function (name, message){
            // Add the message to the page.
            $('#discussion').append('<li><strong>' + htmlEncode(name) + '</strong>: ' + htmlEncode(message) + '</li>');
            console.log($('#discussion')[0].scrollHeight);
            $('.sidebar-chat').scrollTop($('.sidebar-chat')[0].scrollHeight);
        };

        // Gets called when a user joins the lobby
        chat.client.userJoinedLobby = function (name){
            numberOfConnectedUsers++;

            updateNumberOfConnectedUsers();
            $('#discussion').append('<li><strong>' + htmlEncode(name) + ' Joined the lobby </li>');
            $('.sidebar-chat').scrollTop($('#sidebar-chat')[0].scrollHeight);
        }

        // Gets called when a user leaves the lobby
        chat.client.userLeftLobby = function (name){
            numberOfConnectedUsers--;

            updateNumberOfConnectedUsers();
            $('#discussion').append('<li><strong>' + htmlEncode(name) + ' Left the lobby </li>');
            $('.sidebar-chat').scrollTop($('.sidebar-chat')[0].scrollHeight);
        }

        chat.client.insertCode = function (row, column, value){
            editor.session.insert({ "row": parseInt(row), "column": parseInt(column) }, value);
        }

        chat.client.removeCode = function (row, column, endrow, endcolumn){
            editor.session.remove({ "start": { "row": row, "column": column }, "end": { "row": endrow, "column": endcolumn } });
        }

        chat.client.newFileAdded = function (filename){
            $('#files').append('<li><strong>' + htmlEncode(filename) + '</li>');
        }

        // Set initial focus to message input box.
        $('#message').focus();
        // Start the connection.
        $.connection.hub.start().done(function (){
            //chat.server.hello();
            chat.server.joinLobby(lobbyName);

            $('#sendmessage').submit(function (){
                // Call the Send method on the hub.
                chat.server.send(lobbyName, $('#message').val());
                // Clear text box and reset focus for next comment.
                $('#message').val('').focus();

                return false
            });

            $("#createfile").submit(function (){
                chat.server.addFile(lobbyName, $("#newfile").val());

                return false;
            });

            editor.getSession().on('change', function (e){
                // Make sure this was a user change.
                if (editor.curOp && editor.curOp.command.name){
                    if (e.action == "insert") {
                        var lines = e.lines.join("\n");

                        chat.server.insertCode(lobbyName, e.start.row, e.start.column, lines);
                    }
                    else if (e.action == "remove"){
                        chat.server.removeCode(lobbyName, e.start.row, e.start.column, e.end.row, e.end.column);
                    }
                }
            });
        });

        function updateNumberOfConnectedUsers(){
            $("#number-of-users-connected").html("Users Connected: " + numberOfConnectedUsers);
        }
    }
}


function htmlEncode(value){
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}

$(function () {
    /* Setup and start */
    var projectOptions = new ProjectOptions();
    var projectSession = new ProjectSession(projectOptions);
});