/*
    Set up ProjectOptions
*/

function ProjectOptions() {
    this.theme = "ace/theme/monokai";
    this.mode = "ace/mode/javascript";
}

ProjectOptions.prototype = {
    setTheme: function () {
        console.log("SetTheme");
    },
    setMode: function () {
        console.log("SetMode");
    }
}

/* Set up ProjectSession */
function ProjectSession(projectOptions) {
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
    this.users = [];

    // then finally set up the hub.
    this.setupProjectHub();
}

ProjectSession.prototype = {
    setupProjectHub: function () {
        var context = this; //setup context to this

        // Gets called when you join the server
        context.projectHub.client.joined = function (usersData) {
            var users = JSON.parse(usersData);
            context.users = users;
            for (var i = 0; i < users.length; i++) {
                context.addActiveUser(users[i]);
            }
        }

        // Gets called when a user joins the lobby
        context.projectHub.client.userJoinedLobby = function (userData) {
            console.log(userData);
            var user = JSON.parse(userData);
            context.addServerMessage(htmlEncode(user.DisplayName) + " Joined the lobby");

            context.addActiveUser(user)
        }

        // Gets called when a user leaves the lobby
        context.projectHub.client.userLeftLobby = function (id, name) {
            context.removeActiveUser(id);
            context.addServerMessage(htmlEncode(name) + " Left the lobby");
        }

        // Add a new user message
        context.projectHub.client.addNewMessage = function (name, message) {
            context.addUserMessage(name, message);
        };

        //Gets called when your changes have been received by the server.
        context.projectHub.client.changesSaved = function () {
            if (context.allChangesSentToServer) {
                $(".status-saved").html("All your changes have been saved.");
            }
            else {
                // Happens if you made changes after sending the payload to the server.
                $(".status-saved").html("Unsaved changes have been made.");
            }
        }

        // Gets called when someone makes a change to the file
        context.projectHub.client.fileChanged = function (obj) {
            context.silent = true;
            context.editor.getSession().getDocument().applyDelta(JSON.parse(obj));
            context.silent = false;
        }

        // When a user adds new file this gets called on all clients
        context.projectHub.client.newFileAdded = function (projectFileID, filename) {
            $('#files').append('<li class="file" data-fileid="' + projectFileID + '">' + htmlEncode(filename) + '</li>');
        }

        // When a user removes a file this gets called on all clients
        context.projectHub.client.fileRemoved = function (fileID) {
            $("#files").find('[data-fileid=' + fileID + ']').remove();
        }

        context.projectHub.client.openFile = function (projectFile) {
            context.setupMarker();
            context.silent = true;
            // Convert to object
            var data = JSON.parse(projectFile);

            // Set the code value to the newly opened file
            context.currentFileID = data.ID;
            context.currentFileName = data.Name;
            context.editor.setValue(data.Content, 1);
            context.silent = false;
            context.editor.focus();
        }

        // Set focus to the chat box
        $('#message').focus();

        // Start the connection with the server
        $.connection.hub.start().done(function () {
            context.projectHub.server.joinLobby(context.lobbyName);

            $('#sendmessage').submit(function () {
                // Send the message to the server
                context.projectHub.server.send(context.lobbyName, $('#message').val());

                // Clear the message input box
                $('#message').val('').focus();

                return false
            });

            $("#files").on("click", "li", function () {
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
                context.projectHubserver.removeFile(context.lobbyName, file);
                return false;
            });

            context.editor.on('change', function (e) {
                if (context.silent) {
                    return;
                }
                context.allChangesSentToServer = false;

                context.projectHub.server.fileChanged(context.lobbyName, context.currentFileID, JSON.stringify(e));
                if(context.timer != null)
                {
                    clearTimeout(context.timer);
                }
                $(".status-saved").html("Autosaving...");
                context.timer = setTimeout(context.save, 3000);
                
            });
            context.save = function () {
                context.allChangesSentToServer = true;
                context.projectHub.server.saveFile(context.lobbyName, context.currentFileID, context.editor.getValue());
            }
        });
    },
    setupMarker: function () {
        console.log("Setting markers");
        var ra = ace.require('ace/range').Range;
        var range = new ra(0, 0, 0, 10);
        var marker = this.editor.getSession().addMarker(range, 'ace_myclass', 'text');
        console.log(marker);
    },
    addActiveUser: function (user) {
        $("#activeusers").append('<li class="'+ user.MarkerClass +'" data-userconnectionid="' + user.ConnectionID + '"><span class="glyphicon glyphicon-user"></span> ' + htmlEncode(user.DisplayName) + '</li>');
    },
    removeActiveUser: function (id) {
        $("#activeusers").find('[data-userconnectionid=' + id + ']').remove();
    },
    addUserMessage: function (name, message) {
        $('#discussion').append('<li><strong>' + htmlEncode(name) + '</strong>: ' + htmlEncode(message) + '</li>');
    },
    addServerMessage: function (message) {
        $('#discussion').append('<li><strong>' + htmlEncode(message) + '</strong></li>');
    },
    chatScrollToBottom: function () {
        //$('.sidebar-chat').scrollTop($('#sidebar-chat')[0].scrollHeight);
    }
}


function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}

$(function () {
    /* Setup and start */
    var projectOptions = new ProjectOptions();
    var projectSession = new ProjectSession(projectOptions);
});