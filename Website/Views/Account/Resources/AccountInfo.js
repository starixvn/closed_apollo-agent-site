var Account =
{
    InitSearchClick: function () {
        $(".search-button").bind('click', function () {

           // alert($("#ddlIsMember").val());
            $.get(parent.baseURL + '/Account/AccountInfo', { 'userName': $("#txtUsername").val(), 'isMember':  $("#ddlIsMember").val()})
                        .done(function (data) {
                            
                        });
                    return false;

            //location.href = url;
        });
        
    },
};