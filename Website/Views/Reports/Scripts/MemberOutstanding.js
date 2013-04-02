/* File Created: April 2, 2013 */
var memberOutstanding = {
    Init: function () {
          $.ajax({
                type: "POST",
                url: window.baseURL + "Account/ReCheckIn",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: null,
                timeout: 120000,
                success: function (rs) {
                    if (rs.ErrCode == 3) {
                        alert("Your Account was kicked out.")
                        location.href = window.baseURL + "Account/SignOut";
                    }
                },
                error: function (event, jqXHR, ajaxSettings) {
                },
                complete: function () {
                }
            });
    };
};

$(function)