// --------------------------------------------------------------------------
// <copyright file="Fanex.SBPager.js" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

(function ($) {

    //// Method expose for user use
    var methods =
    {
        createSbPager: function (opts) {
            opts = $.extend(true, {}, $.fn.sbPager.defaults, opts);
            return this.each(function () {
                return new Fanex.SBPager(this, opts);
            });
        },
        setCurrentPage: function (pageIndex) {
            $(this).find('.pager-item[index=' + pageIndex + ']').trigger('click');
        },
        getCurrentPage: function () {
            var index = $(this).find('.item-container .selected').attr('index');
            return parseInt(index);
        }
    };

    //// Plugin
    $.fn.sbPager = function (options) {
        if (methods[options]) {
            return methods[options].apply(this, Array.prototype.slice.call(arguments, 1));
        }
        else if (typeof options === 'object' || !options) {
            return methods.createSbPager.apply(this, arguments);
        }
        else {
            throw 'Options cannot be null';
        }
    };

    //// Default Options
    $.fn.sbPager.defaults =
    {
        currentPage: 0,
        numPageDisplay: 7,
        itemPerPage: 0,
        totalItem: 0,
        prevText: '<',
        nextText: '>',
        startText: '<<',
        endText: '>>',
        itemWidth: 35,
        callback: function () { return false; }
    };

    var Fanex = Fanex || {};

    Fanex.SBPager = function (pager, options) {
        this.$pager = $(pager);
        this.options = options;
        this.totalPage = 0;
        this.initialize();

        return this.$pager;
    };

    Fanex.SBPager.prototype =
    {
        initialize: function () {
            var me = this;
            var options = me.options;

            me.renderPager();
            me.registerEvents();
        },
        renderPager: function () {
            var me = this;
            var options = this.options;

            var html = [],
            numItem = Math.round((parseInt(options.totalItem) + options.itemPerPage / 2 - 1) / options.itemPerPage),
            itemHtml = ''
            currentItem = '';

            if (!me.$pager.hasClass('sbpager')) {
                me.$pager.addClass('sbpager');
            }

            var numDisplayItem = numItem > options.numPageDisplay ? options.numPageDisplay : numItem;
            if (numItem > options.numPageDisplay) {
                html.push('<a href="javascript:void(0);" class="move-start">' + options.startText + '</a>');
                html.push('<a href="javascript:void(0);" class="move-pre">' + options.prevText + '</a>');
            }
            html.push('<div class="item-container" style="width:' + (numDisplayItem * options.itemWidth) + 'px">');
            for (var i = 0; i < numItem; i++) {
                currentItem = i + 1;
                itemHtml = '<a href="javascript:void(0);" class="pager-item' + (i == options.currentPage ? ' selected' : '') + '" index="' + i + '">' + currentItem + '</a>';
                html.push(itemHtml);
            }
            html.push('</div>');
            if (numItem > options.numPageDisplay) {
                html.push('<a href="javascript:void(0);" class="move-next">' + options.nextText + '</a>');
                html.push('<a href="javascript:void(0);" class="move-end">' + options.endText + '</a>');
            }

            me.totalPage = numItem;
            var pagerHtml = html.join('');
            me.$pager.empty().append(pagerHtml);

            me.$itemContainer = me.$pager.find('.item-container');

            if (options.currentPage + 1 > numDisplayItem) {
                me.$itemContainer.scrollLeft(me.calculateScrollLeft(options.currentPage));
            }
        },
        registerEvents: function () {
            var me = this;
            var options = this.options;

            if (options.callback) {
                me.$pager.find('.pager-item')
                .unbind('click')
                .bind('click', function (e) {
                    var $this = $(this);
                    me.$pager.find('.pager-item').removeClass('selected');
                    $this.addClass('selected');
                    var currentPage = parseInt($this.attr('index'));
                    if (currentPage + 1 > options.numPageDisplay) {
                        me.$itemContainer.scrollLeft(me.calculateScrollLeft(currentPage));
                    }
                    options.callback(currentPage);

                    return false;
                });
            }

            me.$pager.find('.move-start')
                .unbind('click')
                .bind('click', function (e) {
                    var $this = $(this);
                    me.$pager.find('.pager-item').removeClass('selected');
                    me.$itemContainer.scrollLeft(0);
                    me.$itemContainer.find(':first-child').addClass('selected').trigger('click');
                });

            me.$pager.find('.move-end')
                .unbind('click')
                .bind('click', function (e) {
                    var $this = $(this);
                    me.$pager.find('.pager-item').removeClass('selected');
                    me.$itemContainer.scrollLeft(me.totalPage * options.itemWidth);

                    me.$itemContainer.find(':last-child').addClass('selected').trigger('click');
                });
            me.$pager.find('.move-pre')
                .unbind('click')
                .bind('click', function (e) {
                    var currentIndex = me.$itemContainer.scrollLeft();
                    me.$itemContainer.scrollLeft(currentIndex - options.itemWidth);
                });

            me.$pager.find('.move-next')
                .unbind('click')
                .bind('click', function (e) {

                    var currentIndex = me.$itemContainer.scrollLeft();
                    me.$itemContainer.scrollLeft(currentIndex + options.itemWidth);

                });
        },
        calculateScrollLeft: function (pageIndex) {
            var me = this;
            var options = this.options;
            var widthScrollLeft = 0;
            if (pageIndex > 9) {
                widthScrollLeft += (pageIndex - 13) * (me.$pager.find('.pager-item[index=9]').outerWidth());
                widthScrollLeft += 10 * (me.$pager.find('.pager-item[index=0]').outerWidth());
            }
            else {
                widthScrollLeft += (pageIndex + 2 - options.numPageDisplay / 2) * (me.$pager.find('.pager-item[index=0]').outerWidth() - 3);
            }

            return widthScrollLeft;
        }
    };
})(jQuery);
