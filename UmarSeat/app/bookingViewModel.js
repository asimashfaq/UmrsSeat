
$(function () {
    var bookingSignalR = $.connection.booking;
    bookingSignalR.client.updateEmployee = function (id, key, val) {

        console.log(id + " " + key + " " + val);
    }

    bookingSignalR.client.lockEmployee = function (id) {
        console.log(id);
        $("tr#" + id + " .action a").hide();
        $("tr#" + id + " .action p").show();
    }

    bookingSignalR.client.unlockEmployee = function (id) {
        $("tr#" + id + " .action a").show();
        $("tr#" + id + " .action p").hide();
    }

    bookingSignalR.client.lockFail = function (id) {
        $("tr#" + id + " .action a").hide();
        $("tr#" + id + " .action p").show();
    }



    $.connection.hub.start().done(function () {
        if ($('#pnrNumber').val()!= "")
            {}
            //lockonload($('#pnrNumber').val());

    });
    $(".editbtn").click(function () {
       // event.preventDefault();
       // var id = $(this).attr('key');
       // bookingSignalR.server.lock(id);
    });
    var lockonload = function (id) {
        bookingSignalR.server.lock(id);
    }
   
});
