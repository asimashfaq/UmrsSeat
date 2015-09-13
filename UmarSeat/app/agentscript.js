
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

     
        playload["Person.firstName"] = $("#Person_firstName").val();
        playload["Person.lastName"] = $("#Person_lastName").val();
        playload["Person.email"] = $("#Person_email").val();
        
        playload["Person.mobileNumber"] = $("#Person_mobileNumber").val().replace(/\D/g, '');
       

        $.ajax({
            type: "post",
            url: "/agents/edit",
            data: JSON.stringify({ agents: (playload) }),
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

    $("#Person_firstName").val("");
    $("#Person_lastName").val('');
    $("#Person_email").val('');
    $("#Person_mobileNumber").val('');

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



    agentmodal();

});

$('#modalSlideUp').on('shown.bs.modal', function (e) {

    agentmodal();
   
});
var agentmodal = function () {
    if ($("#entry-form").length > 0) {
        var form = $('#entry-form').validate({ // initialize the plugin
            onkeyup: function (element) {


            },
            onfocusout: function (element) {

            },
            rules: {
                "Person.firstName": "required",
                "Person.lastName": "required",
                "Person.email": "required",
                "Person.mobileNumber": "required",

            },
            messages: {
                'Person.firstName': "Please Enter First Name",
                "Person.lastName": "Please Enter First Name",
                "Person.email": "Please Enter Email",
                "Person.mobileNumber": "Please Enter Mobile Number",
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


        $("#Person_firstName").inputmask('Regex', {
            regex: "[a-zA-Z]{3,10}",
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

                if (result == false && $("#Person_firstName").val().length < 9) {
                    $('.modal-content').pgNotification({

                        style: 'flip',

                        message: "Only Alpha Letters Allowed",
                        timeout: 3000,
                        type: "danger"
                    }).show();
                }


            }
        });
        $("#Person_lastName").inputmask('Regex', {
            regex: "[a-zA-Z]{3,10}",
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

                if (result == false && $("#Person_lastName").val().length < 9) {
                    $('.modal-content').pgNotification({

                        style: 'flip',

                        message: "Only Alpha Letters Allowed",
                        timeout: 3000,
                        type: "danger"
                    }).show();
                }


            }
        });

        $("#Person_email").inputmask({
            mask: "*{1,20}[.*{1,20}][.*{1,20}][.*{1,20}]@*{1,20}[.*{2,6}][.*{1,2}]",
            greedy: false,
            onBeforePaste: function (pastedValue, opts) {
                pastedValue = pastedValue.toLowerCase();
                return pastedValue.replace("mailto:", "");
            },
            definitions: {
                '*': {
                    validator: "[0-9A-Za-z!#$%&'*+/=?^_`{|}~\-]",
                    cardinality: 1,
                    casing: "lower"
                }
            }, "oncomplete": function () {
                $(this).parent().removeClass('has-error');
            },
            "onincomplete": function () {
                $(this).parent().addClass('has-error');
                $('.modal-content').pgNotification({

                    style: 'flip',

                    message: "Email Not Correct",
                    timeout: 3000,
                    type: "danger"
                }).show();
            },
            oncleared: function () {
                $(this).parent().removeClass('has-error');
            },
            onKeyValidation: function (result) {

                if (result == false && $("#Person_email").val().length < 19) {
                    $('.modal-content').pgNotification({

                        style: 'flip',

                        message: "Email Not Correct",
                        timeout: 3000,
                        type: "danger"
                    }).show();
                }


            }
        });


        $("#Person_mobileNumber").inputmask("phone", {
            alias: 'phonebe',
            onBeforeMask: function (value, opts) {
                var processedValue = value.replace(/^0/g, "");
                console.log(processedValue);
                if (processedValue.indexOf("92") > 0) {
                    processedValue = "92" + processedValue;
                }

                return processedValue;
            },
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

                if (result == false) {
                    $('.modal-content').pgNotification({

                        style: 'flip',

                        message: "Not Valid Phone Number",
                        timeout: 3000,
                        type: "danger"
                    }).show();
                }


            }
        });
    }
}

