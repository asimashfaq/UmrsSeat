var refresh = false;
function updatedata() {

    if ($('#booking-entry-form').valid() == true) {

        $("#dv1").append('<div id="aaa" class="panel-default-overlay-down">\
                        <div class="col-sm-12 text-center" style="height:100%">\
                            <div class="progress-circle-indeterminate m-t-45">\
                            </div>\
                            <br>\
                            <p class="small hint-text">Loading data</p>\
                        </div>\
                    </div>');



        var playload = {};
        playload["pnrNumber"] = $("#pnrNumber").val();
        playload["airline"] = $("#airlineId").val();
        playload["country"] = $("#Country1").val();
        playload["stockId"] = $("#stockId").val();
        playload["outbounddate"] = $("#outbounddate").val();
        playload["inbounddate"] = $("#inbounddate").val();
        playload["inboundsector"] = $("#inboundsector").val();
        playload["outboundsector"] = $("#outboundsector").val();
        playload["noOfSeats"] = $("#noOfSeats").val();
        playload["cost"] = $("#cost").inputmask('unmaskedvalue');
        playload["category"] = $("#cat").val();
        playload["recevingbranch"] = $("#branches").val();
        playload["emdNumber"] = $("#emdNumber").val();
        playload["timelimit"] = $("#timelimit").val();
        console.log(playload);
        $.ajax({
            type: "post",
            url: "/booking/edit",
            data: JSON.stringify({ seatconfirmation: (playload) }),
            datatype: "json",
            contentType: "application/json",
            success: function (data) {
                $("#dv1").html("");
                if (data.length != 0) {

                    var response = JSON.parse(data);
                    $.each(response, function (index, row) {
                        if (row.isSuccess == true) {
                            $("#dv1").append('<div class="alert alert-success" role="alert"><button class="close" data-dismiss="alert"></button><strong>Success: </strong>' + row.Message + '</div>');
                        }
                        else {
                            $("#dv1").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: </strong>' + row.ErrorMessage + '</div>');
                        }


                    });
                   
                }



            },
            error: function (xhr, ajaxOptions, thrownError) {
                $("#dv1").html("");
                $("#dv1").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: ' + xhr.status + ' </strong>' + thrownError + '</div>');

            }
        });
    }

}
function clearall() { }
function bclearall() {

    $("#pnrNumber").val("");
    $("#airlineId").val('');
    $("#Country1").val('');
    $("#stockId").val('');
    $("#outbounddate").val('');
    $("#inbounddate").val('');
    $("#inboundsector").val('');
    $("#outboundsector").val('');
    $("#noOfSeats").val('');
    $("#cost").val('');
    $("#cat").val('');
    $("#branches").val('');
    $("#emdNumber").val('');
    $("#timelimit").val('');
}



$('body').on('hidden.bs.modal', function (e) {
    if ($(e.target).attr('data-refresh') == 'true') {
        // Remove modal data
        $(e.target).removeData('bs.modal').find(".modal-content").html('<div class="modal-header clearfix text-left">\
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">\
                        <i class="pg-close fs-14"></i>\
                    </button>\
                    <h5>Please wait <span class="semi-bold">...</span></h5>\
                </div>\
                <div class="modal-body" id="mc">\
                    Loading Content\
                </div>');


    }
});

$('#modalSlideUp').on('loaded.bs.modal', function (e) {



    bookingmodal();

});

$('#modalSlideUp').on('shown.bs.modal', function (e) {


    bookingmodal();
});

function bookingmodal() {
   
    $("#country").select2({
        placeholder: "Select a Country",
        allowClear: true
    });
    $("#airlineId1").select2({
        placeholder: "Select a Airline",
        allowClear: true
    });
    $("#dv2").html('');
}

$("#noOfSeats").inputmask('Regex', {
    regex: "^[0-9]{1,5}",
    greedy: false,
    "oncomplete": function () {
        $(this).parent().removeClass('has-error');
    },
    "onincomplete": function () {
        $(this).parent().addClass('has-error');
    },
    oncleared: function () {
        $(this).parent().removeClass('has-error');
    },
    onKeyValidation: function (result) {

        if (result == false && $("#noOfSeats").val().length < 5) {
            $('body').pgNotification({

                style: 'flip',

                message: "Only Numeric Letters Allowed",
                timeout: 3000,
                type: "danger"
            }).show();
        }


    }
});

$("#cost").inputmask({
    "oncomplete": function () {
        $(this).parent().removeClass('has-error');
    },
    "onincomplete": function () {
        $(this).parent().addClass('has-error');
    },
    oncleared: function () {
        $(this).parent().removeClass('has-error');
    },
    onKeyValidation: function (result) {

        if (result == false && $("#cost").val().length < 9) {
            $('body').pgNotification({

                style: 'flip',

                message: "Only Numeric Letters Allowed",
                timeout: 3000,
                type: "danger"
            }).show();
        }


    }
});
$("#emdNumber").inputmask('Regex', {
    regex: "^[0-9+/.-]{1,20}",
    greedy: false,
    "oncomplete": function () {
        $(this).parent().removeClass('has-error');
    },
    "onincomplete": function () {
        $(this).parent().addClass('has-error');
    },
    oncleared: function () {
        $(this).parent().removeClass('has-error');
    },
    onKeyValidation: function (result) {

        if (result == false && $("#emdNumber").val().length < 20) {
            $('body').pgNotification({

                style: 'flip',

                message: "Only Numeric Letters and '-' Allowed",
                timeout: 3000,
                type: "danger"
            }).show();
        }


    }
});

$("#pnrNumber").inputmask('Regex', {
    regex: "[a-zA-Z0-9]{3,10}",
    greedy: false,
    "oncomplete": function () {
        $(this).parent().removeClass('has-error');
    },
    "onincomplete": function () {
        $(this).parent().addClass('has-error');
    },
    oncleared: function () {
        $(this).parent().removeClass('has-error');
    },
    onKeyValidation: function (result) {

        if (result == false && $("#pnrNumber").val().length < 9) {
            $('body').pgNotification({

                style: 'flip',

                message: "Only Captital and Numeric Letters Allowed",
                timeout: 3000,
                type: "danger"
            }).show();
        }


    }
});
var form = $('#booking-entry-form').validate({ // initialize the plugin
    onkeyup: function (element) {


    },
    onfocusout: function (element) {
      
    },
    rules: {

        country: {
            required: true
        },
        pnrNumber: {

            required: true

        },
        noOfSeats: {
            required: true,
            max: 99999,
            min: 1
        },
        cost: {
            required: true

        }
       

    },
    messages: {
        country: "Please Select Country Name",
      
        emdNumber: "Please Specify EMD #",
        cost: "Please Specify cost",
        noOfSeats: {
            required: "Please Specifiy the # of seats",
            min: "At least # of seat is 1",
            max: "Max # of seats are 99999"
        }
    },
    submitHandler: function (form) { // for demo
        return true;
    },
    invalidHandler: function (form, validator) {
        var errors = validator.numberOfInvalids();

    },
    errorClass: "has-error",

    errorPlacement: function (error, element) {
        var elem = $(element).parent().addClass('has-error');

        $('body').pgNotification({

            style: 'flip',

            message: error[0].innerText,
            timeout: 3000,
            type: "danger"
        }).show();
    }
});

$(document).ready(function () {
    $('#Country1').change(function () {
        $("#s2id_Country1").parent().removeClass('has-error');
        loadSectors();
    });
    $('#airlineId').change(function () {
        loadSectors();
      
    });

    $("#stockId").select2({
        placeholder: "Select Stock",
        allowClear: true
    });
});


var loadSectors = function () {
    var playload = {};

    
    playload["country"] = $("#Country1").val();
    playload["airline"] = $("#airlineId").val();
    $("#addibs").attr('href', encodeURI("/sector/create?country=" + playload["country"] + "&airline=" + playload["airline"]));
    $("#addobs").attr('href', encodeURI("/sector/create?country=" + playload["country"] + "&airline=" + playload["airline"]));

    $.ajax({
        type: "post",
        url: "/sector/getSectors",
        data: JSON.stringify({ sector: (playload) }),
        datatype: "json",
        contentType: "application/json",
        success: function (data) {
            
            if (data.length != 0) {
                $("#inboundsector").html('');
                $("#outboundsector").html('');
                $("#outboundsector,#inboundsector").append('<option value=""></option>');
                var response = JSON.parse(data);
                
                $.each(response.outbound, function (index, row) {

                    
                   
                    $("#outboundsector").append('<option value="' + row['sectorName'] + '">' + row['sectorName'] + '</option>');

                });
                $.each(response.inbound, function (index, row) {

                    $("#inboundsector").append('<option value="' + row['sectorName'] + '">' + row['sectorName'] + '</option>');

                });

            }
          


        },
        error: function (xhr, ajaxOptions, thrownError) {
            $("#dv2").html("");
            $("#dv2").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: ' + xhr.status + ' </strong>' + thrownError + '</div>');

        }
    });

}





$(document).ready(function () {
    $("#Country1").select2({
        placeholder: "Select a Country",
        allowClear: true,


    });

    $("#airlineId").select2({
        placeholder: "Select Airline",
        allowClear: true
    });
    $("#outboundsector").select2({
        placeholder: "Select Outbound Sector",
        allowClear: true
    });
    $("#inboundsector").select2({
        placeholder: "Select Inbound Sector",
        allowClear: true
    });
    $("#branches").select2({
        placeholder: "Select Branch",
        allowClear: true
    });
    $("#cat").select2({
        placeholder: "Select Category",
        allowClear: true
    });



    //The url we will send our get request to
});
$("#outbounddate").datepicker({
    defaultDate: '@Model.outBoundDate',
    format: 'dd/mm/yyyy',
    showClose: true,
    autoclose: true,
});
$("#timelimit").datepicker({
    defaultDate: '@Model.timeLimit',
    format: 'dd/mm/yyyy',
    showClose: true,
    autoclose: true,
});
$("#inbounddate").datepicker({
    defaultDate: '@Model.inBoundDate',
    format: 'dd/mm/yyyy',
    showClose: true,
        autoclose: true,
});