
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
        playload["airline"] = $("#airlineId").val();
        playload["country"] = $("#Country").val();
        playload["stockId"] = $("#stockId").val();
        playload["idAgent"] = $("#agentId").val();
        playload["sellingBranch"] = $("#sb").val();
        playload["noOfSeats"] = $("#noOfSeats").val();
        playload["id_StockTransfer"] = $("#id_StockTransfer").val();

        playload["cost"] = $("#cost").inputmask('unmaskedvalue');
        playload["margin"] = $("#margin").inputmask('unmaskedvalue');
        playload["sellingPrice"] = $("#sellingPrice").inputmask('unmaskedvalue');
        playload["advanceAmount"] = $("#advanceAmount").inputmask('unmaskedvalue');
        playload["advancedate"] = $("#advancedate").val();
        playload["gdsPnrNumber"] = $("#gdsPnrNumber").val();
        playload["catalystInvoiceNumber"] = $("#catalystInvoiceNumber").val();
        playload["isPackage"] = $("#checkbox1").is(':checked');
        playload["isTickted"] = $("#checkbox2").is(':checked');
        
        $.ajax({
            type: "post",
            url: "/stock/sellingedit",
            data: JSON.stringify({ stocktransfer: (playload) }),
            datatype: "json",
            contentType: "application/json",
            success: function (data) {
                $("#dv1").html("");
                if (data.length != 0) {

                    var response = JSON.parse(data);
                    $.each(response, function (index, row) {
                        if (row.isSuccess == true) {
                            $("#dv1").append('<div class="alert alert-success" role="alert"><button class="close" data-dismiss="alert"></button><strong>Success: </strong>' + row.Message + '  Please wait window will reload in 2 seconds...</div>');
                            setTimeout(function () {
                                window.location.reload(true);
                            }, 2000);
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
    $("#airlin").val('');
    $("#Country").val('');
    $("#stockId").val('');
    $("#idAgent").val('');
    $("#sellingBranch").val('');
    $("#cost").val('');
    $("#margin").val('');
    $("#noOfSeats").val('');
    $("#sb").val('');
    $("#advanceAmount").val('');
    $("#advanceDate").val('');
    $("#gdsPnrNumber").val('');
    $("#catalystInvoiceNumber").val('');
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
        idAgent: {
            required: true
        },
        sellingBranch: {
            required: true
        },
        pnrNumber: {

            required: true

        },
        noOfSeats: {
            min: 1,
            max: function () {
                return maxNumber;
            }
        },
        cost: {
            required: true
        },

       
        sellingPrice: {
            required: true
        },
        advanceAmount: {
            required: true
        }


    },
    messages: {
        country: "Please Select Country Name",
        pnrNumber: "Please Specify PNR #",
        cost: "Please Specify cost",
        margin: "Please Specify margin",
        idAgent: "Please Select agent",
        sellingBranch: "Please Select Selling Branch",
        sellingPrice: "Please Specify selling price",
        advanceAmount: "Please Specify advance amount",
     
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


    var moneyMask = function (a)
    {
        $(a).inputmask({
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

                if (result == false && $(a).val().length < 9) {
                    $('body').pgNotification({

                        style: 'flip',

                        message: "Only Numeric Letters Allowed",
                        timeout: 3000,
                        type: "danger"
                    }).show();
                }


            }
        });

    };


    moneyMask("#cost");
    moneyMask("#margin");
    moneyMask("#sellingPrice");
    moneyMask("#advanceAmount");
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

    $("#Country").select2({
        placeholder: "Select a Country",
        allowClear: true
    });
    if ($("select#pnrNumber") != [])
    {
        $("select#pnrNumber").select2({
            placeholder: "Select PNR #",
            allowClear: true
        });
        $('#pnrNumber').change(function () {
            $.ajax({
                type: "get",
                url: "/booking/getPnr",
                data: { pnr: $('#pnrNumber').val() },
                datatype: "json",
                traditional: true,
                success: function (data) {

                    if (data.length > 0) {
                        data = JSON.parse(data);

                        $("#Country").select2('val', data.pnrInfo.country, true);
                        $("#stockId").select2('val', data.pnrInfo.stockId, true);
                        $("#airlineId").select2('val', data.pnrInfo.airLine, true);
                        $("#sb").select2('val', data.pnrInfo.recevingBranch, true);
                        $("#noOfSeats").val(data.tsa);
                        $("#cost,#sellingPrice").val(data.pnrInfo.cost);


                        var chartData = [];
                        chartData.push({ "noOfSeats": data.tsa, "Type": "Seats Avaliable", });
                        chartData.push({ "noOfSeats": data.tgs, "Type": "Seats Split/ Group Split", });
                        chartData.push({ "noOfSeats": data.tss, "Type": "Seats Sold", });
                        chartData.push({ "noOfSeats": data.tts, "Type": "Seats Transfer", });

                        maxNumber = data.tsa;
                        $('#entry-form').valid();
                        drawchart(chartData);
                    }


                }
            });
        });

    }
    
    $("#stockId").select2({
        placeholder: "Select a Stock",
        allowClear: true
    });

    $("#airlineId").select2({
        placeholder: "Select Airline",
        allowClear: true
    });
    $("#agentId").select2({
        placeholder: "Select Agent",
        allowClear: true
    });
    $("#tb").select2({
        placeholder: "Select Transfering Branch",
        allowClear: true
    });
    $("#rb").select2({
        placeholder: "Select Receving Branch",
        allowClear: true
    });
    $("#sb").select2({
        placeholder: "Select Selling Branch",
        allowClear: true
    });

    $('#sb').change(function () {
        $("#s2id_sb").parent().removeClass('has-error');
    });

  

    $("#advancedate").datepicker({
        defaultDate: '@Model.advanceDate',
        format: 'dd/mm/yyyy',
        showClose: true
    });

    $("#margin").bind("change", function () {
        var cost = $("#cost").inputmask('unmaskedvalue');
        var margin = $(this).inputmask('unmaskedvalue');
        // console.log(parseInt(parseInt(cost) + parseInt(margin)));
        $("#sellingPrice").attr('value', (parseInt(parseInt(cost) + parseInt(margin))));
        $('#entry-form').valid();

    });
    $("#sellingPrice").bind("change", function () {
        var cost = $("#cost").inputmask('unmaskedvalue');
        var sp = $("#sellingPrice").inputmask('unmaskedvalue');
        $("#margin").attr('value', parseInt(sp) - parseInt(cost));
        $('#entry-form').valid();

    });
});
