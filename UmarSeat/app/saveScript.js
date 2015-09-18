function agentsavedata () {
   
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
        playload["CompanyName"] = $("#CompanyName").val();
        playload["Person.mobileNumber"] = $("#Person_mobileNumber").val().replace(/\D/g, '');

        $.ajax({
            type: "post",
            url: "/agents/create",
            data: JSON.stringify({ agents: (playload) }),
            datatype: "json",
            contentType: "application/json",
            success: function (data) {
                $("#dv2").html("");
                console.log("Asda");
                console.log(data);
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
                if (refresh)
                {
                    $("#modalSlideUp").on('hidden.bs.modal', function (e) {
                        window.location.reload(true);
                    });
                }
            

            },
            error: function (xhr, ajaxOptions, thrownError) {
                $("#dv2").html("");
                $("#dv2").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: ' + xhr.status + ' </strong>' + thrownError + '</div>');

            }
        });
    }

}
function airlinesavedata() {

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


        $.ajax({
            type: "post",
            url: "/airline/create",
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
                            var airlineDropdown = $("#airlineId");
                            if(airlineDropdown != null)
                            {
                                airlineDropdown.append('<option value="' + $("#airlineName").val() + '">' + $("#airlineName").val() + '</option>');
                                $("#airlineId").select2('val', $("#airlineName").val(), true);
                            }
                        }
                        else {
                            $("#dv2").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: </strong>' + row.ErrorMessage + '</div>');
                        }


                    });

                }

                if (refresh) {
                    $("#modalSlideUp").on('hidden.bs.modal', function (e) {
                        window.location.reload(true);
                    });
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                $("#dv2").html("");
                $("#dv2").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: ' + xhr.status + ' </strong>' + thrownError + '</div>');

            }
        });
    }

}
function bookingsavedata() {

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

        $.ajax({
            type: "post",
            url: "/booking/entry",
            data: JSON.stringify({ seatconfirmation: (playload) }),
            datatype: "json",
            contentType: "application/json",
            success: function (data) {
                $("#dv1").html("");
                if (data.length != 0) {

                    var response = JSON.parse(data);
                    $.each(response, function (index, row) {
                        if (row.isSuccess == true) {
                            $("#dv1").append('<div class="alert alert-success" role="alert"><button class="close" data-dismiss="alert"></button><strong>Success: </strong>' + row.Message + ' Please wait window will reload in 2 seconds...</div>');
                            setTimeout(function () {
                                window.location.reload(true);
                            }, 2000);
                        }
                        else {
                            $("#dv1").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: </strong>' + row.ErrorMessage + '</div>');
                        }


                    });
                    clearall();
                }



            },
            error: function (xhr, ajaxOptions, thrownError) {
                $("#dv1").html("");
                $("#dv1").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: ' + xhr.status + ' </strong>' + thrownError + '</div>');

            }
        });
    }

}
function branchsavedata() {

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

        playload["branchName"] = $("#branchName").val();
        playload["branchCity"] = $("#branchCity").val();
        playload["branchCountry"] = $("#branchCountry").val();
        playload["branchAddress"] = $("#branchAddress").val();


        $.ajax({
            type: "post",
            url: "/branches/create",
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
                            var branchDropdown = $("#branches");
                            if (branchDropdown != null) {
                                branchDropdown.append('<option value="' + $("#branchName").val() + '">' + $("#branchName").val() + '</option>');
                                $("#branches").select2('val', $("#branchName").val(), true);
                            }
                        }
                        else {
                            $("#dv2").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: </strong>' + row.ErrorMessage + '</div>');
                        }


                    });

                }

                if (refresh) {
                    $("#modalSlideUp").on('hidden.bs.modal', function (e) {
                        window.location.reload(true);
                    });
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                $("#dv2").html("");
                $("#dv2").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: ' + xhr.status + ' </strong>' + thrownError + '</div>');

            }
        });

    }

}
function categorysavedata() {

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

        playload["categoryName"] = $("#categoryName").val();


        $.ajax({
            type: "post",
            url: "/category/create",
            data: JSON.stringify({ category: (playload) }),
            datatype: "json",
            contentType: "application/json",
            success: function (data) {
                $("#dv2").html("");
                if (data.length != 0) {

                    var response = JSON.parse(data);
                    $.each(response, function (index, row) {
                        if (row.isSuccess == true) {
                            $("#dv2").append('<div class="alert alert-success" role="alert"><button class="close" data-dismiss="alert"></button><strong>Success: </strong>' + row.Message + '</div>');
                            var categoryDropdown = $("#cat");
                            if (categoryDropdown != null) {
                                categoryDropdown.append('<option value="' + $("#categoryName").val() + '">' + $("#categoryName").val() + '</option>');
                                $("#cat").select2('val', $("#categoryName").val(), true);
                            }
                        }
                        else {
                            $("#dv2").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: </strong>' + row.ErrorMessage + '</div>');
                        }


                    });

                }
                if (refresh) {
                    $("#modalSlideUp").on('hidden.bs.modal', function (e) {
                        window.location.reload(true);
                    });
                }




            },
            error: function (xhr, ajaxOptions, thrownError) {
                $("#dv2").html("");
                $("#dv2").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: ' + xhr.status + ' </strong>' + thrownError + '</div>');

            }
        });
    }

}
function groupsplitsavedata() {

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
        playload["airline"] = $("#airlineId").val();
        playload["stockId"] = $("#stockId").val();
        playload["outbounddate"] = $("#outbounddate").val();
        playload["inbounddate"] = $("#inbounddate").val();
        playload["inboundsector"] = $("#inboundsector").val();
        playload["outboundsector"] = $("#outboundsector").val();
        playload["noOfSeats"] = $("#noOfSeats").val();
        playload["cost"] = $("#cost").inputmask('unmaskedvalue');
        playload["category"] = $("#cat").val();
        if ($("#recevingBranch").val() != undefined)
        {
            playload["recevingbranch"] = $("#recevingBranch").val();
        }
        else
        {
            playload["recevingbranch"] = $("#branches").val();
        }

        
        playload["emdNumber"] = $("#emdNumber").val();
        playload["timelimit"] = $("#timelimit").val();

        $.ajax({
            type: "post",
            url: "/booking/groupsplit",
            data: JSON.stringify({ seatconfirmation: (playload) }),
            datatype: "json",
            contentType: "application/json",
            success: function (data) {
                $("#dv1").html("");
                if (data.length != 0) {

                    var response = JSON.parse(data);
                    $.each(response, function (index, row) {
                        if (row.isSuccess == true) {
                            $("#dv1").append('<div class="alert alert-success" role="alert"><button class="close" data-dismiss="alert"></button><strong>Success: </strong>' + row.Message + ' Please wait window will reload in 2 seconds...</div>');
                            setTimeout(function () {
                                window.location.reload(true);
                            }, 2000);
                        }
                        else {
                            $("#dv1").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: </strong>' + row.ErrorMessage + '</div>');
                        }


                    });
                    clearall();
                }



            },
            error: function (xhr, ajaxOptions, thrownError) {
                $("#dv1").html("");
                $("#dv1").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: ' + xhr.status + ' </strong>' + thrownError + '</div>');

            }
        });
    }

}
function sectorsavedata() {

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
            url: "/sector/create",
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
                         
                            var sectorDropdown = $("#outboundsector");
                            if (selectedVal == "Outbound Sector") {
                            
                                if (sectorDropdown != null) {
                                    sectorDropdown.append('<option value="' + $("#sectorName").val() + '">' + $("#sectorName").val() + '</option>');
                                    $("#outboundsector").select2('val', $("#sectorName").val(), true);
                                }
                            }
                            else if (selectedVal == "Inbound Sector") {
                             
                                sectorDropdown = $("#inboundsector");
                                if (sectorDropdown != null) {
                                    sectorDropdown.append('<option value="' + $("#sectorName").val() + '">' + $("#sectorName").val() + '</option>');
                                    $("#inboundsector").select2('val', $("#sectorName").val(), true);
                                }

                            }
                            else {

                                sectorDropdown = $("#outboundsector");
                             
                                if (sectorDropdown != null) {
                                    sectorDropdown.append('<option value="' + $("#sectorName").val() + '">' + $("#sectorName").val() + '</option>');
                                   // $("#outboundsector").select2('val', $("#sectorName").val(), true);
                                }
                                sectorDropdown = $("#inboundsector");
                                if (sectorDropdown != null) {
                                    sectorDropdown.append('<option value="' + $("#sectorName").val() + '">' + $("#sectorName").val() + '</option>');
                                   // $("#inboundsector").select2('val', $("#sectorName").val(), true);
                                }

                            }

                          
                        }
                        else {
                            $("#dv2").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: </strong>' + row.ErrorMessage + '</div>');
                        }


                    });

                }

                if (refresh) {
                    $("#modalSlideUp").on('hidden.bs.modal', function (e) {
                        window.location.reload(true);
                    });
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                $("#dv2").html("");
                $("#dv2").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: ' + xhr.status + ' </strong>' + thrownError + '</div>');

            }
        });
    }

}
function sellingsavedata() {

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
        if ($("#sellingBranch").val() != undefined) {
            playload["sellingBranch"] = $("#sellingBranch").val();
        }
        else {
            playload["sellingBranch"] = $("#sb").val();
        }
      
        playload["noOfSeats"] = $("#noOfSeats").val();

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
            url: "/stock/sellingcreate",
            data: JSON.stringify({ stocktransfer: (playload) }),
            datatype: "json",
            contentType: "application/json",
            success: function (data) {
                $("#dv1").html("");
                if (data.length != 0) {

                    var response = JSON.parse(data);
                    $.each(response, function (index, row) {
                        if (row.isSuccess == true) {
                            $("#dv1").append('<div class="alert alert-success" role="alert"><button class="close" data-dismiss="alert"></button><strong>Success: </strong>' + row.Message + ' Please wait window will reload in 2 seconds...</div>');
                            setTimeout(function () {
                                window.location.reload(true);
                            }, 2000);
                        }
                        else {
                            $("#dv1").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: </strong>' + row.ErrorMessage + '</div>');
                        }


                    });
                    clearall();
                }



            },
            error: function (xhr, ajaxOptions, thrownError) {
                $("#dv1").html("");
                $("#dv1").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: ' + xhr.status + ' </strong>' + thrownError + '</div>');

            }
        });
    }

}
function stockidsavedata() {

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

        playload["stockName"] = $("#stockName").val();


        $.ajax({
            type: "post",
            url: "/stockId/create",
            data: JSON.stringify({ stockId: (playload) }),
            datatype: "json",
            contentType: "application/json",
            success: function (data) {
                $("#dv2").html("");
                if (data.length != 0) {

                    var response = JSON.parse(data);
                    $.each(response, function (index, row) {
                        if (row.isSuccess == true) {
                            $("#dv2").append('<div class="alert alert-success" role="alert"><button class="close" data-dismiss="alert"></button><strong>Success: </strong>' + row.Message + '</div>');
                            var stockIdDropdown = $("#stockId");
                            if (stockIdDropdown != null) {
                                stockIdDropdown.append('<option value="' + $("#stockId").val() + '">' + $("#stockId").val() + '</option>');
                                $("#stockId").select2('val', $("#stockId").val(), true);
                            }
                        }
                        else {
                            $("#dv2").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: </strong>' + row.ErrorMessage + '</div>');
                        }


                    });

                }
                if (refresh) {
                    $("#modalSlideUp").on('hidden.bs.modal', function (e) {
                        window.location.reload(true);
                    });
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                $("#dv2").html("");
                $("#dv2").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: ' + xhr.status + ' </strong>' + thrownError + '</div>');

            }
        });
    }

}
function transfersavedata() {

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
        playload["noOfSeats"] = $("#noOfSeats").val();
        if ($("#transferingBranch").val() != undefined) {
            playload["transferingBranch"] = $("#transferingBranch").val();
        }
        else {
            playload["transferingBranch"] = $("#tb").val();
        }

     
        playload["recevingBranch"] = $("#rb").val();


        playload["cost"] = $("#cost").inputmask('unmaskedvalue');
        playload["margin"] = $("#margin").inputmask('unmaskedvalue');
        playload["sellingPrice"] = $("#sellingPrice").inputmask('unmaskedvalue');


        $.ajax({
            type: "post",
            url: "/stock/transfercreate",
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
                    clearall();
                }



            },
            error: function (xhr, ajaxOptions, thrownError) {
                $("#dv1").html("");
                $("#dv1").append('<div class="alert alert-danger" role="alert"><button class="close" data-dismiss="alert"></button><strong>Error: ' + xhr.status + ' </strong>' + thrownError + '</div>');

            }
        });
    }

}
