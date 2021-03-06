﻿
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

        playload["id_Sector"] = $("#id_Sector").val();
        playload["sectorName"] = $("#sectorName").val();

        playload["country"] = $("#country").val();
        playload["airline"] = $("#airlineId1").val();
        playload["country"] = $("#country").val();
        var selectedVal = "";
        var selected = $("input[type='radio'][name='category']:checked");
        if (selected.length > 0) {
            selectedVal = selected.val();
        }
        playload["category"] = selectedVal;

        $.ajax({
            type: "post",
            url: "/sector/edit",
            data: JSON.stringify({ sector: (playload) }),
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
                $("#mSector").on('hidden.bs.modal', function (e) {
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

    $("#sectorName").val("");
    $("#id_Sector").val("");

 
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


$('#mSector').on('loaded.bs.modal', function (e) {


    sectorvalidation('#mSector');
});
$('#mSector').on('shown.bs.modal', function (e) {


    sectorvalidation('#mSector');
});
var sectorvalidation = function (a) {

    $("#country").select2({
        placeholder: "Select a Country",
        allowClear: true
    });
    $("#airlineId1").select2({
        placeholder: "Select a Airline",
        allowClear: true
    });

    var form = $('#entry-form').validate({ // initialize the plugin
        onkeyup: function (element) {


        },
        onfocusout: function (element) {

        },
        rules: {
            "sectorName": "required"
        },
        messages: {
            'sectorName': "Please Enter Sector Name"

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
            
            $('' + a + ' .modal-content').pgNotification({

                style: 'flip',

                message: error[0].innerText,
                timeout: 1000,
                type: "danger"
            }).show();
        }
    });


    $("#sectorName").inputmask('Regex', {
        regex: "[a-zA-Z+/.-]{3,20}",
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

            if (result == false && $("#sectorName").val().length < 19) {
              
                $(''+a+' .modal-content').pgNotification({

                    style: 'flip',

                    message: "Only Alpha Letters Allowed",
                    timeout: 1000,
                    type: "danger"
                }).show();
            }


        }
    });
    






}