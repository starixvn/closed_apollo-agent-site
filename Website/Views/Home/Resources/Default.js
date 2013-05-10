
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
});