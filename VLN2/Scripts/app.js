﻿var dragging = false;
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
$(document).ready(function () {
    $(".action-add-file").click(function () {
        $("#createfile").toggle();
        $("#newfile").focus();
    });
});
$("#addfollower").submit(function (e) {

    var form = $(this);
    // Do Ajax Here
    $.ajax({
        type: form.attr('method'),
        url: form.attr('action'),
        data: form.serialize(), // serializes the form's elements.
    });

    e.preventDefault();
});
$("#removefollower").submit(function (e) {

    var form = $(this);
    // Do Ajax Here
    $.ajax({
        type: form.attr('method'),
        url: form.attr('action'),
        data: form.serialize(), // serializes the form's elements.
    });

    e.preventDefault();
});