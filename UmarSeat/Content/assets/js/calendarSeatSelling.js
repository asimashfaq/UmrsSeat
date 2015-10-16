/* ============================================================
 * Calendar
 * This is a Demo App that was created using Pages Calendar Plugin
 * We have demonstrated a few function that are useful in creating
 * a custom calendar. Please refer docs for more information
 * ============================================================ */

(function ($) {

    'use strict';

    $(document).ready(function () {

        var selectedEvent;
        $('body').pagescalendar({
            //Loading Dummy EVENTS for demo Purposes, you can feed the events attribute from 
            //Web Service

            onViewRenderComplete: function () {
                 var startDate = moment().startOf('month').format('YYYY-MM-D');
                 var enddate = moment().endOf('month').format('YYYY-MM-D');
                 generateReport(startDate, enddate);

            },
            onEventClick: function (event) {
                //Open Pages Custom Quick View
                if (!$('#calendar-event').hasClass('open'))
                    $('#calendar-event').addClass('open');


                selectedEvent = event;
                setEventDetailsToForm(selectedEvent);
            },
            onEventDragComplete: function (event) {
                selectedEvent = event;
                setEventDetailsToForm(selectedEvent);
            },
            onEventResizeComplete: function (event) {
                selectedEvent = event;
                setEventDetailsToForm(selectedEvent);
            }
        });
        //After the settings Render you Calendar
        $('body').pagescalendar('render');




    });

})(window.jQuery);
function generateReport(startDate, enddate) {
    stdate = startDate;
    etdate = enddate;
    $(".sectorNames tbody ").html('');
    $(".sectorVal").html('');
    var sdata;
    $.ajax({
        type: "GET",
        url: "/reports/seatsell",
         data: { startDate: startDate, endDate: enddate, airlineName: $("#airlineId").val() },
       // data: { startDate: "2015-09-01", endDate: "2015-09-30", airlineName: "PIA" },
        datatype: "json",
        traditional: true,
        success: function (data) {
            $("#ssr tbody").html('');
            if (data.length == 2) {
                $('body').pgNotification({

                    style: 'flip',

                    message: "No Content Found",
                    timeout: 1000,
                    type: "danger"
                }).show();
            }
            sdata = JSON.parse(data);
            var ss = [];
            

            var x = {};
            var i = sdata.length;
            while (i--) {
                var sHash = sdata[i].BasePnr.toString();
                if (typeof (x[sHash]) == "undefined")
                    x[sHash] = [];
                x[sHash].push(sdata[i]);
            }
            sdata = x;
           

            for (var ob in sdata) {

                var d = sdata[ob];
                var balance = 0;
                for (var row in d)
                {
                    var data = d[row];
                    if (row == 0)
                    {
                        var balance = data.TotalSeats - data.noOfSeats;
                        $("#ssr tbody").append('<tr class="rheader"><td>' + data.pnrNumber + '</td><td>' + data.sellingBranch + '</td><td>' + data.airLine + '</td><td>' + data.outboundSector + '</td><td>' + data.inboundSector +
                        '</td><td>' + data.TotalSeats + '</td><td>' + data.noOfSeats + '</td><td>' + data.sellingPrice + '</td><td>' + data.agentName + '</td><td>'+balance+'</td><tr>');
                    }
                    else
                    {
                        balance = balance - data.noOfSeats;
                        $("#ssr tbody").append('<tr><td>-</td><td>-</td><td>-</td><td>-</td><td>-</td><td>-</td><td>' + data.noOfSeats + '</td><td>' + data.sellingPrice + '</td><td>' + data.agentName + '</td><td>' + balance + '</td><tr>');
                    }
                    
                    console.log(row);
                }
                
               

            }


           
        }
    });



}