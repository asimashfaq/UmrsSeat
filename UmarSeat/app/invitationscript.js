$("#PersonInfo_firstName").inputmask('Regex', {
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

        if (result == false && $("#PersonInfo_firstName").val().length < 9) {
            $('body').pgNotification({

                style: 'flip',

                message: "Only Alpha Letters Allowed",
                timeout: 3000,
                type: "danger"
            }).show();
        }


    }
});
$("#UserName").inputmask('Regex', {
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

        if (result == false && $("#UserName").val().length < 9) {
            $('body').pgNotification({

                style: 'flip',

                message: "Only AlphaNumeric Letters Allowed",
                timeout: 3000,
                type: "danger"
            }).show();
        }


    }
});
$("#PersonInfo_lastName").inputmask('Regex', {
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

        if (result == false && $("#PersonInfo_lastName").val().length < 9) {
            $('body').pgNotification({

                style: 'flip',

                message: "Only Alpha Letters Allowed",
                timeout: 3000,
                type: "danger"
            }).show();
        }


    }
});

$("#PersonInfo_email").inputmask({
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
        $('body').pgNotification({

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

        if (result == false && $("#PersonInfo_email").val().length < 19) {
            $('body').pgNotification({

                style: 'flip',

                message: "Email Not Correct",
                timeout: 3000,
                type: "danger"
            }).show();
        }


    }
});


$("#PersonInfo_mobileNumber").inputmask("phone", {
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