/*jshint strict:false*/
/*global ApoRes*/
/* jshint maxlen:false */
var APOLLO = {}; // Global namespace for common functions

if (typeof jQuery !== 'undefined') {
    // Functions in this namespace require jquery and jquery-ui
    APOLLO = function ($) {
        return {
            showBlockLoading: function () {
                $.blockUI();
            },
            hidePopupBlockLoading: function () {
                if (APOLLO.browser.msie && parseInt(APOLLO.browser.version, 10) < 9) {
                    parent.$('.block-loading').hide();
                    $('.block-loading').hide();
                }
                else {
                    $("iframe").siblings('.block-loading').hide();
                }
            },
            dialogOpt: {
                position: { my: "top+20%", at: "center top", of: window },
                modal: true,
                width: 'auto'
            },
            isResizeDialog: true,
            currentOpenDialog: null,
            openDialog: function (target, title, options) {
                options = options || {};
                if (title) {
                    options.title = title;
                }

                if (target instanceof jQuery === false) {
                    target = $(target);
                }

                APOLLO.currentOpenDialog = target.dialog($.extend({}, APOLLO.dialogOpt, options));

                var iframeLoading = target.find('iframe').get(0);

                if (iframeLoading) {

                    iframeLoading.contentWindow.onload = function () {
                        APOLLO.hidePopupBlockLoading();
                    };

                }

                //APOLLO.closeDialog(target);

                return APOLLO.currentOpenDialog;
            },
            openDialogFrame: function (url, title, options, singleFrame) {
                var frameHtml = '<iframe src="' + url + '" width="99%" height="99%" overflow="auto" frameborder="0" border="0"></iframe>',
					accessClass = "";

                var blockLoading = '<p class="block-loading">Loading..............................................</p>';



                if (typeof singleFrame === 'undefined'){ singleFrame = true;}  // not allow multiple instances of dialog by default

                if (singleFrame) { // only one instance of dialog can be opened
                    var frame = $("#apo-dialog-frame");

                    // check FC1X2 popup
                    if (typeof (url) !== 'object') {
                        if (url.substring(1, 15).toLowerCase() === 'forecast/fc1x2') {
                            accessClass = "fc1x2";
                        }
                    }

                    if (frame.length > 0) {
                        frame.html(blockLoading + frameHtml);
                    }
                    else {
                        //                        if (APOLLO.browser.msie && parseInt(APOLLO.browser.version, 10) < 9) {
                        //                            blockLoading = '';
                        //                        }
                        frame = '<div id="apo-dialog-frame">' + blockLoading + frameHtml + '</div>';
                    }

                    return APOLLO.openDialog(frame, title, options);
                }

                // check scoremap popup
                if (typeof (url) !== 'object') {
                    if (url.substring(1, 18).toLowerCase() === 'forecast/scoremap') {
                        accessClass = "loading-scoremap";
                    }
                }

                // multiple instances of dialog can be opened
                return APOLLO.openDialog('<div>' + blockLoading + frameHtml + '</div>', title, options);
            },
            closeDialog: function (target) {
                target.dialog("close");
            },
            resizeDialog: function (width, height) {
                if (APOLLO.currentOpenDialog && APOLLO.currentOpenDialog.dialog("isOpen")) {

                    if ($(window).width() >= 1024) {
                        APOLLO.currentOpenDialog.dialog("option", { width: width + 30, height: height + 50 });
                    } else {
                        APOLLO.currentOpenDialog.dialog("option", { width: width, height: height });
                    }
                }
            },
            setDialogHeight: function (target, height) {
                target.dialog("option", "height", height);
            },
            resolveUrl: function (url) {
                var apoRoot = $('#ApoRoot');

                if (apoRoot.length === 0) {
                    APOLLO.console.error('"ApoRoot" does not exist! Please add the following code to your page (or _Layout page): @Html.Hidden("ApoRoot", Url.Content("~/"))');
                } else {
                    if (url.substr(0, 2) === '~/') {
                        url = url.substr(2);
                    } else if (url.substr(0, 1) === '/') {
                        url = url.substr(1);
                    }

                    return apoRoot.val() + url;
                }
            },

            generalErrorMsg: 'Your request is temporarily cannot be processed, please try again later!',
            alertGeneralError: function () {
                alert(APOLLO.generalErrorMsg);
            },
            handleAjaxError: function () {
                APOLLO.alertGeneralError();
            },
            /*jshint ignore:start */
            showMessage: function (message, type, timeOut, isMultple) {
                switch (type) {
                    case 'success':
                        noty({
                            text: message,
                            type: type,
                            template: '<div class="noty_message success"><span class="noty_text"></span></div>',
                            timeout: timeOut || 5000
                        });
                        break;
                    case 'error':
                        noty({
                            text: message,
                            type: type,
                            errorType: 'single',
                            template: '<div class="noty_message error' + (isMultple ? ' errormultiple' : '') + '"><span class="noty_text"></span></div>',
                            buttons: true,
                            buttons: [{
                                addClass: 'btn btn-primary', text: 'Close', onClick: function ($noty) {
                                    $noty.close();
                                }
                            }]
                        });
                        break;

                }
            },
            /*jshint ignore:end */
            console: {
                write: function (msg, consoleFunc) {
                    if (typeof console !== 'undefined' && !APOLLO.detectBrowser().msie) {
                        //consoleFunc(msg);
                    }
                },
                log: function (msg) { APOLLO.console.write(msg, console.log); },
                info: function (msg) { APOLLO.console.write(msg, console.info); },
                warn: function (msg) { APOLLO.console.write(msg, console.warn); },
                error: function (msg) { APOLLO.console.write(msg, console.error); }
            },

            parseJSON: function (value) {
                return $.parseJSON(value.replaceAll("'", "\""));
            },

            initDatePicker: function () {
                var datePickers = $('.datepicker');

                if (datePickers.length > 0) {
                    var isValidDate = function (dateString) {
                        // Parse the date parts to integers
                        var parts = dateString.split("-");
                        var day = parseInt(parts[2], 10);
                        var month = parseInt(parts[1], 10);
                        var year = parseInt(parts[0], 10);

                        // Check the ranges of month and year
                        if (year < 1000 || year > 3000 || month === 0 || month > 12)
                            {return false;}

                        var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

                        // Adjust for leap years
                        if (year % 400 === 0 || (year % 100 !== 0 && year % 4 === 0))
                            {monthLength[1] = 29;}

                        // Check the range of the day
                        return day > 0 && day <= monthLength[month - 1];
                    };
                    var validateDate = function (target) {
                        var minDate = target.datepicker("option", "minDate");
                        var maxDate = target.datepicker("option", "maxDate");
                        var inputDate = target.val(); // the visible value on screen
                        var currentDate = target.datepicker("getDate");
                        var errMsg = '';

                        // if selected date is invalid
                        if (!isValidDate(inputDate)) {
                            errMsg = ApoRes.InvalidDate;
                        }
                        if (errMsg.length === 0 && currentDate < minDate) {
                            errMsg = ApoRes.DateIsOutOfRange;
                        }
                        if (errMsg.length === 0 && maxDate < currentDate) {
                            errMsg = ApoRes.DateIsOutOfRange;
                        }

                        target.attr('title', errMsg).tooltip({
                            position: { my: "left top+2", at: "left bottom" },
                            tooltipClass: "warning"
                        });

                        if (errMsg.length > 0) {
                            target.tooltip('open');
                            return false;
                        }

                        target.tooltip('destroy');
                        target.removeAttr('title');
                    };
                    var limitDate = function (target, limit) { // limit: mindate/maxdate
                        var _limit = target.attr(limit);

                        if (_limit) { // if limit attribute exists
                            _limit = _limit.split("-");

                            var year = _limit[0],
								month = _limit[1],
								day = _limit[2];

                            return new Date(year, month - 1, day);
                        }

                        return null;
                    };
                    var shiftDate = function (target, increment) {
                        var currDate = target.datepicker('getDate');
                        var d = currDate.getDate() + increment;
                        var m = currDate.getMonth();
                        var y = currDate.getFullYear();

                        target.datepicker('setDate', new Date(y, m, d));
                    };
                    var defaultDate = function ($this) {
                        var dateRegex = /^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$/;
                        if (!(dateRegex.test($this.val()))) {
                            var today = new Date();
                            return today.getYear + '-' + today.getMonth + '-' + today.getDay;
                        }
                        return $this.val();
                    };
                    datePickers.each(function () {
                        var $this = $(this);

                        $this.datepicker({
                            changeMonth: true,
                            changeYear: true,
                            defaultDate: defaultDate($this),
                            dateFormat: "yy-mm-dd",
                            minDate: limitDate($this, "mindate"),
                            maxDate: limitDate($this, "maxdate"),
                            showOn: "button",
                            buttonImage: "../_globalResources/css/img/calendar.gif",
                            buttonImageOnly: true,
                            buttonText: "",
                            beforeShow: function (input, inst) {
                                inst.dpDiv.css({ marginTop: '2px' });
                            },
                            onSelect: function () {
                                validateDate($this);
                            }
                        }).keydown(function (e) {
                            var arrow = { left: 37, up: 38, right: 39, down: 40 };
                            switch (e.keyCode) {
                                case arrow.up:
                                    shiftDate($this, 1);
                                    break;
                                case arrow.down:
                                    shiftDate($this, -1);
                                    break;
                            }
                        })
						.blur(function () { validateDate($this); })
						.parents('form').submit(function () { return validateDate($this); });
                    });
                }
            },

            initPageInfo: function () {
                var infos = $('.infos');

                infos.each(function () {
                    var $this = $(this),
						thisId = $this.attr('id'),
						title = $this.attr('title'),
						tipTitle = $this.attr('tip-title'),
						trigger;

                    trigger = thisId === 'page-info'
						? $('<div class="page-info" />').appendTo('#header_main')
						: $('#' + thisId.substr(0, thisId.length - 5)); // 5 is the length of "-info";

                    if (trigger.length > 0) {
                        trigger.attr('title', tipTitle ? tipTitle : title)
						.tooltip({
						position: { my: "right-10 center", at: "center center+5" },
						show: null,
						hide: null
						})
						.click(function () {
							APOLLO.openDialog($this, title, { modal: false });
							$('.ui-widget-header').css({ 'background': '#4A7AA9', 'color': '#fff', 'border-color': '#fff' });
							$('.ui-widget-header .ui-state-default').css({ 'border-color': '#395E82', 'background-color': '#fff' });
						});
                    }
                });
            },

            signOut: function () {
                try {
                    if (parent.IsApoRoot) {
                        parent.location = APOLLO.resolveUrl('~/Internal/SignOut');
                    }
                } catch (err) {
                    APOLLO.console.warn(err);
                }
            },

            detectBrowser: function () {
                var matchedBrowser, browserDectect;

                // Use of jQuery.browser is frowned upon.
                // More details: http://api.jquery.com/jQuery.browser
                // jQuery.uaMatch maintained for back-compat
                function ubMatch(ua) {
                    ua = ua.toLowerCase();

                    var match = /(chrome)[ \/]([\w.]+)/.exec(ua) ||
								/(webkit)[ \/]([\w.]+)/.exec(ua) ||
								/(opera)(?:.*version|)[ \/]([\w.]+)/.exec(ua) ||
								/(msie) ([\w.]+)/.exec(ua) ||
								ua.indexOf("compatible") < 0 && /(mozilla)(?:.*? rv:([\w.]+)|)/.exec(ua) ||
								[];

                    return {
                        browserDectect: match[1] || "",
                        version: match[2] || "0"
                    };
                }

                matchedBrowser = ubMatch(navigator.userAgent);
                browserDectect = {};

                if (matchedBrowser.browserDectect) {
                    browserDectect[matchedBrowser.browserDectect] = true;
                    browserDectect.version = matchedBrowser.version;
                }

                // Chrome is Webkit, but Webkit is also Safari.
                if (browserDectect.chrome) {
                    browserDectect.webkit = true;
                } else if (browserDectect.webkit) {
                    browserDectect.safari = true;
                }
                return browserDectect;
            },

            htmlEncode: function (str) {
                return $('<div/>').text(str).html();
            },
            htmlDecode: function (str) {
                return $('<div/>').html(str).text();
            },

            initFreeHeader: function () {
                //APOLLO.browser
                if (APOLLO.browser.msie && parseInt(APOLLO.browser.version, 10) < 9) {
                    return;
                }
                var clonedHeaderRow;
                $(".persist-area").each(function () {
                    clonedHeaderRow = $(".persist-header", this);
                    clonedHeaderRow
					.before(clonedHeaderRow.clone())
					.css({ "width": clonedHeaderRow.width() })
					.addClass("floatingHeader")
					.removeClass("persist-header");
                });

                $(window)
				.scroll(APOLLO.updateTableHeaders)
				.trigger("scroll");
                $(window).resize(function () {
                    APOLLO.updateTableHeaders();
                });
            },

            updateTableHeaders: function () {
                if ($(".persist-area").val() !== undefined) {
                    var widthspace = $(".persist-area").width();

                    $('.floatingHeader').width(widthspace);
                    var left = $('table thead tr:first').position()["left"];
                    $(".persist-area").each(function () {
                        var el = $(this),
						offset = el.offset(),
						scrollTop = $(window).scrollTop(),
						floatingHeader = $(".floatingHeader", this);
                        if ((scrollTop > offset.top) && (scrollTop < offset.top + el.height())) {
                            floatingHeader.css({
                                "visibility": "visible",
                                "left": (left - $(window).scrollLeft()),
                                "z-index": "34"
                            });
                        } else {
                            floatingHeader.css({
                                "visibility": "hidden"
                            });
                        }
                    });

                    var i = 0;

                    var persistheader = $("table.persist-area thead.persist-header tr td");
                    var floatingheader = $("table.persist-area thead.floatingHeader tr td");

                    if (persistheader.length <= 0) {
                        persistheader = $("table.persist-area thead.persist-header tr th");
                    }

                    if (floatingheader.length <= 0) {
                        floatingheader = $("table.persist-area thead.floatingHeader tr th");
                    }

                    persistheader.each(function () {
                        var x = $(this).width();
                        $(floatingheader[i]).width(x);
                        i++;
                    });
                }
            },

            addAntiForgeryToken: function (data) {
                data.__RequestVerificationToken = $('#forgery-token-div input[name=__RequestVerificationToken]').val();
                return data;
            },

            IsValidDate: function (date) {
                var dateRegex = /^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$/;
                if (!(dateRegex.test(date))) {
                    return false;
                }

                var opera = date.split('-');
                if (opera.length > 1) {
                    var yy = parseInt(opera[0]);
                    var mm = parseInt(opera[1]);
                    var dd = parseInt(opera[2]);
                    var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
                    if (mm === 1 || mm > 2) {
                        if (dd > ListofDays[mm - 1]) {
                            return false;
                        }
                    }
                    if (mm === 2) {
                        var lyear = false;
                        if ((!(yy % 4) && yy % 100) || !(yy % 400)) {
                            lyear = true;
                        }

                        if ((lyear === false) && (dd >= 29)) {
                            return false;
                        }

                        if ((lyear === true) && (dd > 29)) {
                            return false;
                        }
                    }
                }
                else {
                    return false;
                }

                return true;
            },

            init: function () {
                APOLLO.initDatePicker();
                //APOLLO.initFreeHeader();
            },
            resolutionForScreens: function (width, height) {
                var resolution = new Array();
                if (($(window).width() >= 1024)) {
                    resolution["width"] = width;
                    resolution["height"] = height;
                } else {
                    if (width < 960) {
                        resolution["width"] = width;
                    } else {
                        resolution["width"] = 960;
                    }

                    if (height < 510) {
                        resolution["height"] = height;
                    } else {
                        resolution["height"] = 510;
                    }
                }
                return resolution;
            }
        };
    } (jQuery);

    APOLLO.browser = APOLLO.detectBrowser();

    $(document).ready(function () {
        APOLLO.init();
    });
}

// JSON for old browsers
var JSON; if (!JSON) {JSON = {};} (function () { function f(n) { return n < 10 ? "0" + n : n; } if (typeof Date.prototype.toJSON !== "function") { Date.prototype.toJSON = function (key) { return isFinite(this.valueOf()) ? this.getUTCFullYear() + "-" + f(this.getUTCMonth() + 1) + "-" + f(this.getUTCDate()) + "T" + f(this.getUTCHours()) + ":" + f(this.getUTCMinutes()) + ":" + f(this.getUTCSeconds()) + "Z" : null; }; String.prototype.toJSON = Number.prototype.toJSON = Boolean.prototype.toJSON = function (key) { return this.valueOf(); }; } var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, gap, indent, meta = { "\b": "\\b", "\t": "\\t", "\n": "\\n", "\f": "\\f", "\r": "\\r", '"': '\\"', "\\": "\\\\" }, rep; function quote(string) { escapable.lastIndex = 0; return escapable.test(string) ? '"' + string.replace(escapable, function (a) { var c = meta[a]; return typeof c === "string" ? c : "\\u" + ("0000" + a.charCodeAt(0).toString(16)).slice(-4); }) + '"' : '"' + string + '"'; } function str(key, holder) { var i, k, v, length, mind = gap, partial, value = holder[key]; if (value && typeof value === "object" && typeof value.toJSON === "function") { value = value.toJSON(key); } if (typeof rep === "function") { value = rep.call(holder, key, value); } switch (typeof value) { case "string": return quote(value); case "number": return isFinite(value) ? String(value) : "null"; case "boolean": case "null": return String(value); case "object": if (!value) { return "null"; } gap += indent; partial = []; if (Object.prototype.toString.apply(value) === "[object Array]") { length = value.length; for (i = 0; i < length; i += 1) { partial[i] = str(i, value) || "null"; } v = partial.length === 0 ? "[]" : gap ? "[\n" + gap + partial.join(",\n" + gap) + "\n" + mind + "]" : "[" + partial.join(",") + "]"; gap = mind; return v; } if (rep && typeof rep === "object") { length = rep.length; for (i = 0; i < length; i += 1) { if (typeof rep[i] === "string") { k = rep[i]; v = str(k, value); if (v) { partial.push(quote(k) + (gap ? ": " : ":") + v); } } } } else { for (k in value) { if (Object.prototype.hasOwnProperty.call(value, k)) { v = str(k, value); if (v) { partial.push(quote(k) + (gap ? ": " : ":") + v); } } } } v = partial.length === 0 ? "{}" : gap ? "{\n" + gap + partial.join(",\n" + gap) + "\n" + mind + "}" : "{" + partial.join(",") + "}"; gap = mind; return v; } } if (typeof JSON.stringify !== "function") { JSON.stringify = function (value, replacer, space) { var i; gap = ""; indent = ""; if (typeof space === "number") { for (i = 0; i < space; i += 1) { indent += " "; } } else { if (typeof space === "string") { indent = space; } } rep = replacer; if (replacer && typeof replacer !== "function" && (typeof replacer !== "object" || typeof replacer.length !== "number")) { throw new Error("JSON.stringify"); } return str("", { "": value }); }; } if (typeof JSON.parse !== "function") { JSON.parse = function (text, reviver) { var j; function walk(holder, key) { var k, v, value = holder[key]; if (value && typeof value === "object") { for (k in value) { if (Object.prototype.hasOwnProperty.call(value, k)) { v = walk(value, k); if (v !== undefined) { value[k] = v; } else { delete value[k]; } } } } return reviver.call(holder, key, value); } text = String(text); cx.lastIndex = 0; if (cx.test(text)) { text = text.replace(cx, function (a) { return "\\u" + ("0000" + a.charCodeAt(0).toString(16)).slice(-4); }); } if (/^[\],:{}\s]*$/.test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, "@").replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, "]").replace(/(?:^|:|,)(?:\s*\[)+/g, ""))) { j = eval("(" + text + ")"); return typeof reviver === "function" ? walk({ "": j }, "") : j; } throw new SyntaxError("JSON.parse"); }; } } ());

String.Empty = ""; // Extend static string

/**
* ReplaceAll by Fagner Brack (MIT Licensed)
* Replaces all occurrences of a substring in a string
*/
String.prototype.replaceAll = function (token, newToken, ignoreCase) {
    var _token;
    var str = this + "";
    var i = -1;

    if (typeof token === "string") {
        if (ignoreCase) {
            _token = token.toLowerCase();

            while ((i = str.toLowerCase().indexOf(token, i >= 0 ? i + newToken.length : 0)) !== -1) {
                str = str.substring(0, i) + newToken + str.substring(i + token.length);
            }
        } else {
            return this.split(token).join(newToken);
        }
    }

    return str;
};

String.prototype.toBoolean = function () {
    switch (this.toLowerCase()) {
        case "true": case "yes": case "1": return true;
        case "false": case "no": case "0": case null: return false;
        default: return Boolean(this);
    }
};

// Returns the string with all the beginning
// and ending whitespace removed
String.prototype.trim = function () {
    return this.replace(/^\s+/, '').replace(/\s+$/, '');
};

String.prototype.contains = function (sub) {
    return this.indexOf(sub, 0) !== -1;
};

String.prototype.Format = function () {
    var formatted = this;
    for (var i = 0; i < arguments.length; i++) {
        var regexp = new RegExp('\\{' + i + '\\}', 'gi');
        formatted = formatted.replace(regexp, arguments[i]);
    }
    return formatted;
};

String.prototype.FormatNumber = function (number) { // Format a number to 1,234,567.89
    number += String.Empty;
    var x = number.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : String.Empty;
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return this + x1 + x2;
};

String.prototype.escapeHtml = function () { // Replace all ampersands with &amp; and all <’s and >’s with &lt; and &gt;, respectively:
    var result = String.Empty;
    for (var i = 0; i < this.length; i++) {
        if (this.charAt(i) === "&" && this.length - i - 1 >= 4 && this.substr(i, 4) !== "&amp;") {
            result = result + "&amp;";
        } else if (this.charAt(i) === "<") {
            result = result + "&lt;";
        } else if (this.charAt(i) === ">") {
            result = result + "&gt;";
        } else {
            result = result + this.charAt(i);
        }
    }
    return result;
};