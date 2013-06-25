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
        numPageDisplay: 5,
        itemPerPage: 0,
        totalItem: 0,
        prevText: '<',
        nextText: '>',
        startText: '<<',
        endText: '>>',
        itemWidth: 36,
        callback: function () { return false; }
    };

    var Fanex = Fanex || {};

    Fanex.SBPager = function (pager, options) {
        this.$pager = $(pager);
        this.options = options;

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
            numItem = Math.round(options.totalItem / options.itemPerPage),
            itemHtml = ''
            currentItem = '';

            if (!me.$pager.hasClass('sbpager')) {
                me.$pager.addClass('sbpager');
            }

            var numDisplayItem = numItem > options.numPageDisplay ? options.numPageDisplay : numItem;
            html.push('<a href="javascript:void(0);" class="move-start">' + options.startText + '</a>');
            html.push('<a href="javascript:void(0);" class="move-pre">' + options.prevText + '</a>');
            html.push('<div class="item-container" style="width:' + (numDisplayItem * options.itemWidth - 2)  + 'px">');
            for (var i = 0; i < numItem; i++) {
                currentItem = i + 1;
                currentItem = (currentItem < 10 ? '0' + currentItem : currentItem);
                itemHtml = '<a href="javascript:void(0);" class="pager-item' + (i == options.currentPage ? ' selected' : '') + '" index="' + i + '">' + currentItem + '</a>';
                html.push(itemHtml);
            }
            html.push('</div>');
            html.push('<a href="javascript:void(0);" class="move-next">' + options.nextText + '</a>');
            html.push('<a href="javascript:void(0);" class="move-end">' + options.endText + '</a>');


            var pagerHtml = html.join('');
            me.$pager.append(pagerHtml);

            me.$itemContainer = me.$pager.find('.item-container');

            if (options.currentPage > 0) {
                me.$itemContainer.scrollLeft(options.currentPage * options.itemWidth);
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
                    options.callback(parseInt($this.attr('index')));

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
                    me.$itemContainer.scrollLeft(1000);
                    me.$itemContainer.find(':last-child').addClass('selected').trigger('click');
                });
            me.$pager.find('.move-pre')
                .unbind('click')
                .bind('click', function (e) {
                    var currentIndex = me.$itemContainer.scrollLeft();
                    me.$itemContainer.animate(
                    {
                        scrollLeft: currentIndex - options.itemWidth
                    }, 100);
                });

            me.$pager.find('.move-next')
                .unbind('click')
                .bind('click', function (e) {
                    var currentIndex = me.$itemContainer.scrollLeft();
                    me.$itemContainer.animate(
                    {
                        scrollLeft: currentIndex + options.itemWidth
                    }, 100);

                });
        }
    };
})(jQuery);
