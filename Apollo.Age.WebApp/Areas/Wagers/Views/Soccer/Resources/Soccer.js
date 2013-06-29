/// <reference path="../../../../../_GlobalResources/Js/jquery.js" />

var Soccer = function ($) {
    var container;
    var groupBetName;
    var urlAction;
    return {
        init: function () {
            container = $("#container");
            groupBetName = $("GroupBetName").val();
            urlAction = APOLLO.resolveUrl("~/Soccer/") + groupBetName;
            Soccer.loadData()
        },

        loadData: function () {
             $.getJSON(urlAction)
                        .done(function (data) {
                            // clean and rebind
                            container.empty();
                            container.html(data);
                        });
        },
    }
} (jQuery);

$(function () {
    Soccer.init();
}); /* File Created: June 28, 2013 */