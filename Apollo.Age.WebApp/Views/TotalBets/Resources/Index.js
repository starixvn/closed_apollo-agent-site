
$(function () {
    $('.menu').on('click', 'li', function () {
        var $this = $(this);
        if ($this.attr('id') !== 'more-link') {
            $this.parents('.menu').find('li').removeClass('active');
            $this.addClass('active');
        }
    });

    $('#more-link').on('click', '>a', function () {
        $(this).siblings('.sub-menu').slideToggle('fast');
    }).on('click', 'ul a', function () {
        $(this).parents('.sub-menu').slideToggle('fast');
    });
});