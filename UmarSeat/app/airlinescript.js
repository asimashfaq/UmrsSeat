
function updatedata() {

    if ($('#entry-form').valid() == true) {

        $("#dv2").append('<div id="aaa" class="panel-default-overlay-down">\
                        <div class="col-sm-12 text-center" style="height:100%">\
                            <div class="progress-circle-indeterminate m-t-45">\
                            </div>\
                            <br>\
                            <p class="small hint-text">Loading data</p>\
                        </div>\
                    </div>');



        var playload = {};


        playload["airlineName"] = $("#airlineName").val();
        playload["Country"] = $("#country").val();
        playload["id_AirLine"] = $("#id_AirLine").val();



        $.ajax({
            type: "post",
            url: "/airline/edit",
            data: JSON.stringify({ airline: (playload) }),
            datatype: "json",
            contentType: "application/json",
            success: function (data) {
                $("#dv2").html("");
                if (data.length != 0) {

                    var response = JSON.parse(data);
                    $.each(response, function (index, row) {
                        if (row.isSuccess == true) {
                            $("#dv2").append('<div class="alert alert-success" role="alert"><button class="close" data-dismiss="alert"></button><strong>Success: </strong>' + row.Message + '</div>');
                        }
                        else {
                            $("#dv2").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: </strong>' + row.ErrorMessage + '</div>');
                        }


                    });

                }
                $("#modalSlideUp").on('hidden.bs.modal', function (e) {
                    window.location.reload(true);
                });


            },
            error: function (xhr, ajaxOptions, thrownError) {
                $("#dv2").html("");
                $("#dv2").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: ' + xhr.status + ' </strong>' + thrownError + '</div>');

            }
        });

    }

}



function clearall() {

    $("#airlineName").val("");
    $("#country").val('');
    $("#id_AirLine").val('');


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
    


    airlinemodal();

});

$('#modalSlideUp').on('shown.bs.modal', function (e) {

    
    airlinemodal();
});

var airlinemodal = function () {

    if ($("#entry-form").length > 0) {

        $("#country").select2({
            placeholder: "Select a Country",
            allowClear: true
        });
        $("#dv2").html('');



        var form = $('#entry-form').validate({ // initialize the plugin
            onkeyup: function (element) {


            },
            onfocusout: function (element) {

            },
            rules: {
                "airlineName": "required",
                "Country": "required",


            },
            messages: {
                'airlineName': "Please Enter Airline Name",
                "Country": "Please Specify  Country"

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

                $('.model-content').pgNotification({

                    style: 'flip',

                    message: error[0].innerText,
                    timeout: 3000,
                    type: "danger"
                }).show();
            }
        });


        $("#airlineName").inputmask('Regex', {
            regex: "[a-zA-Z+/. ]{3,20}",
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

                if (result == false && $("#airlineName").val().length < 19) {
                    $('.modal-content').pgNotification({

                        style: 'flip',

                        message: "Only Alpha Letters Allowed",
                        timeout: 3000,
                        type: "danger"
                    }).show();
                }


            }
        });


    }

}