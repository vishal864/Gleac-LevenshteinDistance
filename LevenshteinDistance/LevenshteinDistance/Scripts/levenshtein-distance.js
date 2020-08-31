$(document).ready(function ()
{
    $("#btnLogin").click(function () {

        if (!isFieldsValidated('txtName','txtPassword'))
            return;

        $('#frmLogin').validate();

        if ($('#frmLogin').valid()) {

            var loginFieldData = $("#frmLogin").serialize();

            $.ajax({
                type: "post",
                url: "api/account/generate",
                data: loginFieldData,
                datatype: "json",
                contenttype: "application/json; charset=utf-8",
                success: function (response) {
                    if (response != null && response.Status == "Success") {
                        //get the Jwt Token
                        sessionStorage.JwtToken = response.JwtToken;

                        //validate the token against user
                        ValidateToken();
                    }
                    else if (response != null && response == "Invalid") {
                        alert("Token generation error");
                        return;
                    }
                },
                error: function (xhr, status, error) {                    
                    alert("Result: " + xhr.status);
                }
            });
        }
    });

    function ValidateToken()
    {        
        var url = "api/account/validate";
        var jwtToken = sessionStorage.JwtToken;
        var userName = $("#txtName").val();
        url = url + '?jwtToken=' + jwtToken + '&username=' + userName;        

        $.ajax({
            type: "get",
            url: url,           
            datatype: "json",
            //headers: { 'Authorization': 'Bearer ' + sessionStorage.JwtToken },
            contenttype: "application/json; charset=utf-8",
            success: function (response) {
                if (response != null && response.Status == "Success") {
                    $('#divLogin').hide();
                    $('#divDashboard').show();
                }
                else if (response != null && response == "Invalid") {
                    alert("Token validation failed or invalid token. Please try again");
                    return;
                }
            },
            error: function (xhr, status, error) {
                alert("Result: " + status + " " + error + " " + xhr.status + " " + xhr.statusText);
            }
        });
    }

    $("#btnCalculate").click(function ()
    {        
        if (!isFieldsValidated('txtVal1','txtVal2'))
            return;

        $('#trCalDistance').hide();

        var val1 = $('#txtVal1').val();
        var val2 = $('#txtVal2').val();

        $.ajax({
            type: "get",
            url: "login/calculatedistance",
            datatype: "json",
            //headers: { 'Authorization': 'Bearer ' + sessionStorage.JwtToken },
            data : {'value1' : val1, 'value2' : val2 },
            contenttype: "application/json; charset=utf-8",
            success: function (response) {
                if (response != null) {
                    sessionStorage.calculatedResult = response.Result; //Matrix Representation

                    $('#spnCalculatedDistance').html(response.CalculatedDistance); //Calculated Distance b/w 2 strings

                    $('#trCalDistance').show();
                    $('#divMatrix').html(response.Result); 
                    $('#divMatrix').show();
                }
                else if (response != null && response == "InventoryAvailable") {
                    alert("Token validation failed. Please try again");
                    return;
                }
            },
            error: function (xhr, status, error) {
                alert("Result: " + status + " " + error + " " + xhr.status + " " + xhr.statusText);
            }
        });
    });

    function isFieldsValidated(field1, field2)
    {
        var requiredFields = $('input.requrd');

        //for (var counter = 0; counter < requiredFields.length; counter++) {
        //    if ($(requiredFields[counter]).val() == '') {
        //        $(requiredFields[counter]).addClass('highlight');
        //        $(requiredFields[counter - 1]).focus();//name textbox 
        //    }
        //    else {
        //        $(requiredFields[counter]).removeClass('highlight');                
        //    }
        //}

        
        var field1 = $('#'+field1);
        var field2 = $('#'+field2);

        field1.removeClass('highlight');  
        field2.removeClass('highlight');

        if (field1.val() == "")
        {
            field1.addClass('highlight'); 
            field1.focus();
            return false;
        }

        if (field2.val() == "")
        {
            field2.addClass('highlight');
            field2.focus();
            return false;
        }
        return true;
    }
});