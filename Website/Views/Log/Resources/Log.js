/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var Log = function ($) {

    return {

        timerID: null,

        moreIsLoading: false,

        imagePath: null,

        init: function () {
            Log.imagePath = APOLLO.resolveUrl('Views/Message/Images/loading_2.gif');
        }
    }
} (jQuery);

$(document).ready(function () {
    Log.init();    
});