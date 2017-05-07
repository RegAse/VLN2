var dragging = false;
$('#dragbar').mousedown(function (e) {
    e.preventDefault();
       
    dragging = true;
    $(document).mousemove(function (e) {
        var percentage = (e.pageX / window.innerWidth) * 100;
        var mainPercentage = 100 - percentage;

        $('#sidebar').css("width", percentage + "%");
        $('#main').css("width", mainPercentage + "%");
        $('#ghostbar').remove();
    });
});

$(document).mouseup(function (e) {
    if (dragging) {
        
        $(document).unbind('mousemove');
        dragging = false;
    }
});
