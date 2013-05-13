
$(document).ready(function () {
    window.IsApoRoot = true;

    // Check IE7
    if (APOLLO.browser.msie && parseInt(APOLLO.browser.version, 10) == 7) {
        var windowHeight = $(window).height(),
        headerHeight = $('.header').height(),
        contentHeight = windowHeight - headerHeight + 1;
        $('.menu').height(contentHeight - 10);
        $('.mainpage').height(contentHeight - 20);
    }

    $('#btn-mini-menu').click(function () {
        if ($('#btn-mini-menu').hasClass('collapse')) {
            $('#btn-mini-menu').addClass('expand').removeClass('collapse');
            // change size of menu navigation
            $('.menu').addClass('mini-menu');
            $('.mainpage').addClass('max-mainpage');
            $('.menu iframe').hide();
            $('ul.tab-menu-icon').show();
        } else {
            $('#btn-mini-menu').addClass('collapse').removeClass('expand');
            // change size of menu navigation
            $('.menu').removeClass('mini-menu');
            $('.mainpage').removeClass('max-mainpage');
            $('.menu iframe').show();
            $('ul.tab-menu-icon').hide();
        }
    });
});