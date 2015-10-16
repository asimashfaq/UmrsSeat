/* ============================================================
 * Calendar
 * This is a Demo App that was created using Pages Calendar Plugin
 * We have demonstrated a few function that are useful in creating
 * a custom calendar. Please refer docs for more information
 * ============================================================ */

(function($) {

    'use strict';

    $(document).ready(function() {

        var selectedEvent;
        $('body').pagescalendar({
            //Loading Dummy EVENTS for demo Purposes, you can feed the events attribute from 
            //Web Service
          
            onViewRenderComplete: function () {
               // var startDate = moment().startOf('month').format('YYYY-MM-D');
               /// var enddate = moment().endOf('month').format('YYYY-MM-D');
               // generateReport(startDate, enddate);
				
            },
            onEventClick: function(event) {
                //Open Pages Custom Quick View
                if (!$('#calendar-event').hasClass('open'))
                    $('#calendar-event').addClass('open');


                selectedEvent = event;
                setEventDetailsToForm(selectedEvent);
            },
            onEventDragComplete: function(event) {
                selectedEvent = event;
                setEventDetailsToForm(selectedEvent);
            },
            onEventResizeComplete: function(event) {
                selectedEvent = event;
                setEventDetailsToForm(selectedEvent);
            }
        });
        //After the settings Render you Calendar
        $('body').pagescalendar('render');
		

	
       
    });

})(window.jQuery);
function generateReport(startDate, enddate)
{
    stdate = startDate;
    etdate = enddate;
    $(".sectorNames tbody ").html('');
    $(".sectorVal").html('');
    var sdata;
    $.ajax({
        type: "GET",
        url: "/reports/airlinesold",
        data: { startDate: startDate, endDate: enddate, airlineName: $("#airlineId").val() },
        datatype: "json",
        traditional: true,
        success: function (data) {
            
            if (data.length == 2)
            {
                $('body').pgNotification({

                    style: 'flip',

                    message: "No Content Found",
                    timeout: 1000,
                    type: "danger"
                }).show();
            }
            sdata = JSON.parse(data);
            var ss = [];
            for (var ob in sdata) {

                var d = sdata[ob];

                $(".sectorNames tbody ").append('<tr><td>' + ob + '</td></tr>');
                if (d.length == 0) {
                    $(".sectorVal ").append('<tr><td>-</td></tr>');
                }
                else {
                    
                    $(".sectorVal ").append('<tr class="' + ob + '"><td>0</td></tr>');

                }

            }


            for (var ob in sdata) {

                var d = sdata[ob];

                if (!d.length == 0) {
                    var totalseats = 0;


                    var totalseats = 0;
                    for (var pnrindex in d) {



                        var pnr = d[pnrindex];
                       
                        var dt = moment(pnr.outBoundDate).format('D');
                       

                        var oldval = $("#date" + dt + " ." + ob + "").text();
                        var ts = Number(oldval)+ Number(pnr.TotalSeats);
                        $("#date" + dt + " ." + ob + "").html("<td class='popup" + dt + "'>" + ts + "</td>");
                        
                       
                        $("#popups .popup" + dt + " .content").append("<p>" + pnr.pnrNumber + "</p>");

                        $("#date" + dt + " ." + ob + "").popover({
                            html: true,
                            trigger: "hover",
                            container: 'body',
                            placement: function (context, source) {
                                var position = $(source).position();

                                if (position.left > 515) {
                                    return "left";
                                }

                                if (position.left < 515) {
                                    return "right";
                                }

                                if (position.top < 110) {
                                    return "bottom";
                                }

                                return "top";
                            },
                            title: function () {
                                return $("#popups .popup" + dt + "").find('.head').html();
                            },
                            content: function () {
                                var classname = $(this).find('td')[0].className;
                                return $("#popups ." + classname + "").find('.content').html();
                            }
                        });
                    }


                    for (var pnrindex in d) {
                        var pnr = d[pnrindex];
                        totalseats = totalseats + pnr.TotalSeats;
                        ss[pnr.pnrNumber] = pnr.TotalSeats;
                        var pnrData = pnr.data;

                        for (var pnrSaleListIndex in pnrData) {
                            ss[pnr.pnrNumber][pnrSaleListIndex] = pnr.TotalSeats;
                            var pnrSalesInfo = pnrData[pnrSaleListIndex].sale;

                            for (var saleindex in pnrSalesInfo) {
                                var sales = 0;
                               
                                var oldval = $("#date" + moment(pnrSalesInfo[saleindex].createAt).format('D') + " ." + ob + "").text();
                               

                                var date = moment(pnr.outBoundDate).format('D');
                              


                                var totaldays = $("td[id^=date]").length;
                               
                                    
                                        var isSaleExits = $("#date" + date + " ." + ob + " .sales");
                                       
                                       
                                            var tempSeats = $("#date" + date + " ." + ob + "").text();
                                            sales = Number(tempSeats) - Number(pnrSalesInfo[saleindex].noOfSeats);

                                           
                                           
                                            $("#date" + date + " ." + ob + "").html("<td  class='popup" + date + "'>" + sales + "</span></td>");
                                               
                                             

                                         



                                            $("#date" + date + " ." + ob + "").popover({
                                            html: true,
                                            trigger: "hover",
                                            container: 'body' ,
                                            placement: function (context, source) {
                                                var position = $(source).position();

                                                if (position.left > 515) {
                                                    return "left";
                                                }

                                                if (position.left < 515) {
                                                    return "right";
                                                }

                                                if (position.top < 110) {
                                                    return "bottom";
                                                }

                                                return "top";
                                            },
                                            title: function () {
                                                return $("#popups .popup"+date+"").find('.head').html();
                                            },
                                            content: function () {
                                                var classname = $(this).find('td')[0].className;
                                                return $("#popups ." + classname + "").find('.content').html();
                                            }
                                        });
                                    
                                                   
                                    		
                                
                            }

                        }
                        // $("#"+ob+" td").html(totalseats+','+sales);
                    }



                 

                }

            }
        }
    });

    
	
}