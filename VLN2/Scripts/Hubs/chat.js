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
    this.numberOfConnectedUsers = 0;

    // Setup ace editor
    this.editor = ace.edit("editor");
    this.editor.setTheme(projectOptions.theme);
    this.editor.getSession().setMode(projectOptions.mode);
    this.editor.setShowPrintMargin(false);
    this.editor.$blockScrolling = Infinity;

    // Reference the chathub and other related variables
    this.lobbyName = location.href.substr(location.href.lastIndexOf('/') + 1);
    this.projectHub = $.connection.projectHub;
    this.silent = false;
    this.currentFileID = -1;
    this.currentFileName = "";
    this.autosaveTimer;
    this.allChangesSentToServer = true;

    // then finally set up the hub.
    this.setupProjectHub();
}

ProjectSession.prototype = {
    setupProjectHub: function () {
        var context = this; //setup context to this

        context.projectHub.client.joined = function (connectedUsersCount, users){
            numberOfConnectedUsers = connectedUsersCount;
            console.log(connectedUsersCount);
            updateNumberOfConnectedUsers();
        }

        // Create a function that the hub can call back to display messages.
        context.projectHub.client.addNewMessageToPage = function (name, message){
            // Add the message to the page.
            $('#discussion').append('<li><strong>' + htmlEncode(name) + '</strong>: ' + htmlEncode(message) + '</li>');
            $('.sidebar-chat').scrollTop($('.sidebar-chat')[0].scrollHeight);
        };

        // Gets called when a user joins the lobby
        context.projectHub.client.userJoinedLobby = function (name){
            numberOfConnectedUsers++;

            updateNumberOfConnectedUsers();
            $('#discussion').append('<li><strong>' + htmlEncode(name) + ' Joined the lobby </li>');
            $('.sidebar-chat').scrollTop($('#sidebar-chat')[0].scrollHeight);
        }

        // Gets called when a user leaves the lobby
        context.projectHub.client.userLeftLobby = function (name){
            numberOfConnectedUsers--;

            updateNumberOfConnectedUsers();
            $('#discussion').append('<li><strong>' + htmlEncode(name) + ' Left the lobby </li>');
            $('.sidebar-chat').scrollTop($('.sidebar-chat')[0].scrollHeight);
        }

        context.projectHub.client.changesSaved = function () {
            if (context.allChangesSentToServer) {
                // All Changes have been successfuly saved.
                $(".status-saved").html("All your changes have been saved.");
            }
            else {
                // Happens if you made changes after sending the payload to the server.
                $(".status-saved").html("Unsaved changes have been made.");
            }
        }

        context.projectHub.client.fileChanged = function (obj) {
            context.silent = true;
            console.log("File Change: " + obj);
            console.log(obj);
            context.editor.getSession().getDocument().applyDelta(JSON.parse(obj));
            context.silent = false;
        }

        // When a user adds new file this gets called on all clients
        context.projectHub.client.newFileAdded = function (projectFileID, filename){
            $('#files').append('<li class="file" data-fileid="' + projectFileID + '"><strong>' + htmlEncode(filename) + '</li>');
        }

        // When a user removes a file this gets called on all clients
        context.projectHub.client.fileRemoved = function (fileID) {
            $("#files").find('[data-fileid=' + fileID + ']').remove();
        }

        context.projectHub.client.openFile = function (projectFile) {
            context.silent = true;
            // Convert to object
            var data = JSON.parse(projectFile);

            // Set the code value to the newly opened file
            context.currentFileID = data.ID;
            context.currentFileName = data.Name;
            context.editor.setValue(data.Content);
            $(".currently-opened-filename").html(context.currentFileName);
            context.silent = false;
        }

        // Set initial focus to message input box.
        $('#message').focus();

        // Start the connection.
        $.connection.hub.start().done(function (){
            context.projectHub.server.joinLobby(context.lobbyName);

            $('#sendmessage').submit(function (){
                // Call the Send method on the hub.
                context.projectHub.server.send(context.lobbyName, $('#message').val());

                // Clear the textbox
                $('#message').val('').focus();

                return false
            });

            $("#files").on("click", "li", function () {
                console.log("Open");
                // Send changes to server
                if (!context.allChangesSentToServer) {
                    context.save();
                }

                $(".selected-file").removeClass("selected-file");
                $(this).addClass("selected-file");
                context.projectHub.server.requestFile(context.lobbyName, $(this).data("fileid"));
            });

            $("#createfile").submit(function () {
                var val = $("#newfile").val();
                $("#newfile").val('');

                $(this).hide();
                context.projectHub.server.addFile(context.lobbyName, val);

                return false;
            });

            $("#files").on("click", "li span", function () {
                var file = $(this).parent().data("fileid");
                console.log("Remove file: " + file);
                context.projectHubserver.removeFile(context.lobbyName, file);
                return false;
            });

            context.editor.on('change', function (e){
                if (context.silent) {
                    return;
                }
                context.allChangesSentToServer = false;

                context.projectHub.server.fileChanged(context.lobbyName, context.currentFileID, JSON.stringify(e));
                if(context.timer != null)
                {
                    clearTimeout(context.timer);
                }
                context.timer = setTimeout(context.save, 3000);
                
            });
            context.save = function(){
                $(".status-saved").html("Autsaving...");
                context.allChangesSentToServer = true;
                context.projectHub.server.saveFile(context.lobbyName, context.currentFileID, context.editor.getValue());
            }
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