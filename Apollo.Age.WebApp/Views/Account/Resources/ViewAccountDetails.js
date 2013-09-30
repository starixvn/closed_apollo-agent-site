/* File Created: June 27, 2013 */

var VIEWACCOUNTDETAILS = (function ($) {
    return {
        init: function () {
            $('#back-link').unbind('click').bind('click', function () {
                if ($('#BackLink').val() != '') {
                    window.location.href = $('#BackLink').val();
                    
                }
                else {
                    history.go(-1);
                }
                return false;
            });
        }
    };
})(jQuery);

$(function () {
    VIEWACCOUNTDETAILS.init();
});