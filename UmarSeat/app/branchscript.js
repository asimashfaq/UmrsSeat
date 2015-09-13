
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

        playload["id_branch"] = $("#id_branch").val();
        playload["branchName"] = $("#branchName").val();
        playload["branchCity"] = $("#branchCity").val();
        playload["branchCountry"] = $("#branchCountry").val();
        playload["branchAddress"] = $("#branchAddress").val();



        $.ajax({
            type: "post",
            url: "/branches/edit",
            data: JSON.stringify({ branches: (playload) }),
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
                $("#mBranch").on('hidden.bs.modal', function (e) {
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

    $("#branchName").val("");
    $("#branchCity").val('');
    $("#branchCountry").val('');
    $("#branchAddress").val('');


    
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

$('#mBranch').on('loaded.bs.modal', function (e) {

    branchModal();

});



$('#mBranch').on('shown.bs.modal', function (e) {
    branchModal();
});

var branchModal = function () {
    if ($('#entry-form').length > 0) {

        var form = $('#entry-form').validate({ // initialize the plugin
            onkeyup: function (element) {


            },
            onfocusout: function (element) {

            },
            rules: {
                "branchName": "required"
            },
            messages: {
                'branchName': "Please Enter Branch Name"

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


        $("#branchName").inputmask('Regex', {
            regex: "[a-zA-Z0-9+/. ]{3,200}",
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

                if (result == false && $("#branchName").val().length < 200) {
                    $('.modal-content').pgNotification({

                        style: 'flip',

                        message: "Only Alpha Letters Allowed",
                        timeout: 3000,
                        type: "danger"
                    }).show();
                }


            }
        });

        $("#branchAddress").inputmask('Regex', {
            regex: "[a-zA-Z0-9+/. ,-.]{3,200}",
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

                if (result == false && $("#branchAddress").val().length < 300) {
                    $('.modal-content').pgNotification({

                        style: 'flip',

                        message: "Only Alpha Letters Allowed",
                        timeout: 3000,
                        type: "danger"
                    }).show();
                }


            }
        });

        $('#branchCountry').change(function () {
            $.ajax({
                type: "post",
                url: "/Branches/GetCities",
                data: { countryId: $('#branchCountry').val() },
                datatype: "json",
                traditional: true,
                success: function (data) {
                    var city = "<select id='branchCity' name='branchCity'>";
                    city = city + '<option value="">Select City</option>';
                    for (var i = 0; i < data.length; i++) {
                        data[i].Value = data[i].Value.replace(/\s/g, '');
                        city = city + '<option value=' + data[i].Value + '>' + data[i].Text + '</option>';
                    }
                    city = city + '</select>';
                    $('#citites').html(city);
                    $("#branchCity").select2({
                        placeholder: "Select a City",
                        allowClear: true
                    });
                    $("#st").show();
                }
            });
        });



    }





}