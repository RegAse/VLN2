/*var editor = ace.edit("editor");
editor.setTheme("ace/theme/monokai");
editor.getSession().setMode("ace/mode/javascript");

var chat = $.connection.chatHub;
var lobbyName = location.href.substr(location.href.lastIndexOf('/') + 1);

$.connection.hub.start().done(function () {
    editor.getSession().on('change', function (e) {
        if (e.action == "insert") {
            console.log("Insert at row: " + e.start.row + ", col: " + e.start.column);
            var row = e.start.row;
            var column = e.start.column;
            var lines = e.lines;

            chat.server.insertCode(lobbyName, row, column, lines);
        }
        else if (e.action == "remove") {
            console.log("Remove");

        }
        console.log(e);
    });
});*/