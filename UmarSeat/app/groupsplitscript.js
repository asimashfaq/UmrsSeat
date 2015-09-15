


function updatedata() {

    if ($('#entry-form').valid() == true) {

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
        playload["newPnrNumber"] = $("#newPnrNumber").val();
        playload["country"] = $("#Country").val();
        playload["stockId"] = $("#stockId").val();
        playload["airline"] = $("#airlineId").val();
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
            url: "/booking/groupsplitedit",
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
function clearall() {

    $("#pnrNumber").val("");
    $("#newPnrNumber").val('');
    $("#Country").val('');
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
var maxNumber = 1;
var form = $('#entry-form').validate({ // initialize the plugin
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
        newPnrNumber: {

            required: true

        },
        noOfSeats: {
            required: true,
            min: 1,
            max: function () {
                return maxNumber;
            }
        },
        cost: {
            required: true

        },
        stockId: {
            required: true

        },
        airLine: {
            required: true

        },
        inBoundSector: {
            required: true

        },
        outBoundSector: {
            required: true

        },
        inBoundDate: {
            required: true

        },
        outBoundDate: {
            required: true

        }
        

    },
    messages: {
        country: "Please Select Country Name",
        pnrNumber: "Please Specify PNR #",
        newPnrNumber: "Please Specify new PNR #",
    
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
        if (error[0].innerText.length > 0) {
            var elem = $(element).parent().addClass('has-error');

            $('body').pgNotification({

                style: 'flip',

                message: error[0].innerText,
                timeout: 3000,
                type: "danger"
            }).show();
        }
        else {
            $(element).closest('.has-error').parent().removeClass('has-error').addClass('has-success');
        }

    },
    success: function (element) {
        $(element).closest('.has-error').parent().removeClass('has-error').addClass('has-success');
    },
});
$(document).ready(function () {
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
   

    $("#pnrNumber,#newPnrNumber").inputmask('Regex', {
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
   
    $("#Country").select2({
        placeholder: "Select a Country",
        allowClear: true
    });
    $("#pnrNumber").select2({
        placeholder: "Select PNR #",
        allowClear: true
    });
    
    $("#stockId").select2({
        placeholder: "Select a Stock",
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
    $("#airlineId").select2({
        placeholder: "Select Airline",
        allowClear: true
    });
    $("#inbounddate").datepicker({
        defaultDate: '@Model.inBoundDate',
        format: 'dd/mm/yyyy',
        showClose: true,
        autoclose: true,
    });

    //The url we will send our get request to
});
$('#pnrNumber').change(function () {
    $.ajax({
        type: "get",
        url: "/booking/getPnr",
        data: { pnr: $('#pnrNumber').val() },
        datatype: "json",
        traditional: true,
        success: function (data) {
           
            if(data.length>0)
            {
                data = JSON.parse(data);
               
                $("#Country").select2('val', data.pnrInfo.country, true);
                $("#stockId").select2('val', data.pnrInfo.stockId, true);
                $("#outboundsector").select2('val', data.pnrInfo.outBoundSector, true);
                $("#inboundsector").select2('val', data.pnrInfo.inBoundSector, true);
                console.log(data.pnrInfo.recevingBranch);
                $("#branches").select2('val', data.pnrInfo.recevingBranch, true);
                $("#cat").select2('val', data.pnrInfo.category, true);
                $("#noOfSeats").val(data.tsa);
                $("#cost").val(data.pnrInfo.cost);
                $("#airlineId").select2('val', data.pnrInfo.airLine, true);
                $("#outbounddate").val(moment(data.pnrInfo.outBoundDate).format("DD/MM/YYYY"));
                $("#inbounddate").val(moment(data.pnrInfo.inBoundDate).format("DD/MM/YYYY"));
                $("#timelimit").val(moment(data.pnrInfo.timeLimit).format("DD/MM/YYYY"));
                maxNumber = data.tsa;
                $('#entry-form').valid();
                var chartData = [];
                chartData.push({ "noOfSeats": data.tsa, "Type": "Seats Avaliable", });
                chartData.push({ "noOfSeats": data.tgs, "Type": "Seats Split/ Group Split", });
                chartData.push({ "noOfSeats": data.tss, "Type": "Seats Sold", });
                chartData.push({ "noOfSeats": data.tts, "Type": "Seats Transfer", });

          
                drawchart(chartData);
            }

           
        }
    });
});