var APOLLO = {}; // Global namespace for common functions

if (typeof jQuery != 'undefined') {
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
                if (title) options.title = title;

                if (target instanceof jQuery == false) target = $(target);

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



                if (typeof singleFrame === 'undefined') singleFrame = true; // not allow multiple instances of dialog by default

                if (singleFrame) { // only one instance of dialog can be opened
                    var frame = $("#apo-dialog-frame");

                    // check FC1X2 popup
                    if (typeof (url) != 'object') {
                        if (url.substring(1, 15).toLowerCase() == 'forecast/fc1x2') {
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
                if (typeof (url) != 'object') {
                    if (url.substring(1, 18).toLowerCase() == 'forecast/scoremap') {
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

                if (apoRoot.length == 0) {
                    APOLLO.console.error('"ApoRoot" does not exist! Please add the following code to your page (or _Layout page): @Html.Hidden("ApoRoot", Url.Content("~/"))');
                } else {
                    if (url.substr(0, 2) == '~/') {
                        url = url.substr(2);
                    } else if (url.substr(0, 1) == '/') {
                        url = url.substr(1);
                    }

                    return apoRoot.val() + url;
                }
            },

            generalErrorMsg: 'Your request is temporarily cannot be processed, please try again later!',
            alertGeneralError: function () {
                alert(APOLLO.generalErrorMsg);
            },
            handleAjaxError: function (jqXHR, textStatus, errorThrown) {
                APOLLO.alertGeneralError();
            },

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

            console: {
                write: function (msg, consoleFunc) {
                    if (typeof console != 'undefined' && !APOLLO.detectBrowser().msie) {
                        //consoleFunc(msg);
                    }
                },
                log: function (msg) { APOLLO.console.write(msg, console.log) },
                info: function (msg) { APOLLO.console.write(msg, console.info) },
                warn: function (msg) { APOLLO.console.write(msg, console.warn) },
                error: function (msg) { APOLLO.console.write(msg, console.error) }
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
                        if (year < 1000 || year > 3000 || month == 0 || month > 12)
                            return false;

                        var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

                        // Adjust for leap years
                        if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
                            monthLength[1] = 29;

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
                        if (errMsg.length == 0 && currentDate < minDate) {
                            errMsg = ApoRes.DateIsOutOfRange;
                        }
                        if (errMsg.length == 0 && maxDate < currentDate) {
                            errMsg = ApoRes.DateIsOutOfRange;
                        }

                        target.attr('title', errMsg).tooltip({
                            position: { my: "left top+2", at: "left bottom" },
                            tooltipClass: "warning"
                        })

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
                        var date_regex = /^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$/;
                        if (!(date_regex.test($this.val()))) {
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
                            onSelect: function (dateText, inst) {
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
						.blur(function () { validateDate($this) })
						.parents('form').submit(function () { return validateDate($this) });
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

                    trigger = thisId == 'page-info'
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
                };

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
                if ($(".persist-area").val() != undefined) {
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
                        };
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
                var date_regex = /^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$/;
                if (!(date_regex.test(date))) {
                    return false;
                }

                var opera = date.split('-');
                if (opera.length > 1) {
                    var yy = parseInt(opera[0]);
                    var mm = parseInt(opera[1]);
                    var dd = parseInt(opera[2]);
                    var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
                    if (mm == 1 || mm > 2) {
                        if (dd > ListofDays[mm - 1]) {
                            return false;
                        }
                    }
                    if (mm == 2) {
                        var lyear = false;
                        if ((!(yy % 4) && yy % 100) || !(yy % 400)) {
                            lyear = true;
                        }

                        if ((lyear == false) && (dd >= 29)) {
                            return false;
                        }

                        if ((lyear == true) && (dd > 29)) {
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
var JSON; if (!JSON) { JSON = {} } (function () { function f(n) { return n < 10 ? "0" + n : n } if (typeof Date.prototype.toJSON !== "function") { Date.prototype.toJSON = function (key) { return isFinite(this.valueOf()) ? this.getUTCFullYear() + "-" + f(this.getUTCMonth() + 1) + "-" + f(this.getUTCDate()) + "T" + f(this.getUTCHours()) + ":" + f(this.getUTCMinutes()) + ":" + f(this.getUTCSeconds()) + "Z" : null }; String.prototype.toJSON = Number.prototype.toJSON = Boolean.prototype.toJSON = function (key) { return this.valueOf() } } var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, gap, indent, meta = { "\b": "\\b", "\t": "\\t", "\n": "\\n", "\f": "\\f", "\r": "\\r", '"': '\\"', "\\": "\\\\" }, rep; function quote(string) { escapable.lastIndex = 0; return escapable.test(string) ? '"' + string.replace(escapable, function (a) { var c = meta[a]; return typeof c === "string" ? c : "\\u" + ("0000" + a.charCodeAt(0).toString(16)).slice(-4) }) + '"' : '"' + string + '"' } function str(key, holder) { var i, k, v, length, mind = gap, partial, value = holder[key]; if (value && typeof value === "object" && typeof value.toJSON === "function") { value = value.toJSON(key) } if (typeof rep === "function") { value = rep.call(holder, key, value) } switch (typeof value) { case "string": return quote(value); case "number": return isFinite(value) ? String(value) : "null"; case "boolean": case "null": return String(value); case "object": if (!value) { return "null" } gap += indent; partial = []; if (Object.prototype.toString.apply(value) === "[object Array]") { length = value.length; for (i = 0; i < length; i += 1) { partial[i] = str(i, value) || "null" } v = partial.length === 0 ? "[]" : gap ? "[\n" + gap + partial.join(",\n" + gap) + "\n" + mind + "]" : "[" + partial.join(",") + "]"; gap = mind; return v } if (rep && typeof rep === "object") { length = rep.length; for (i = 0; i < length; i += 1) { if (typeof rep[i] === "string") { k = rep[i]; v = str(k, value); if (v) { partial.push(quote(k) + (gap ? ": " : ":") + v) } } } } else { for (k in value) { if (Object.prototype.hasOwnProperty.call(value, k)) { v = str(k, value); if (v) { partial.push(quote(k) + (gap ? ": " : ":") + v) } } } } v = partial.length === 0 ? "{}" : gap ? "{\n" + gap + partial.join(",\n" + gap) + "\n" + mind + "}" : "{" + partial.join(",") + "}"; gap = mind; return v } } if (typeof JSON.stringify !== "function") { JSON.stringify = function (value, replacer, space) { var i; gap = ""; indent = ""; if (typeof space === "number") { for (i = 0; i < space; i += 1) { indent += " " } } else { if (typeof space === "string") { indent = space } } rep = replacer; if (replacer && typeof replacer !== "function" && (typeof replacer !== "object" || typeof replacer.length !== "number")) { throw new Error("JSON.stringify") } return str("", { "": value }) } } if (typeof JSON.parse !== "function") { JSON.parse = function (text, reviver) { var j; function walk(holder, key) { var k, v, value = holder[key]; if (value && typeof value === "object") { for (k in value) { if (Object.prototype.hasOwnProperty.call(value, k)) { v = walk(value, k); if (v !== undefined) { value[k] = v } else { delete value[k] } } } } return reviver.call(holder, key, value) } text = String(text); cx.lastIndex = 0; if (cx.test(text)) { text = text.replace(cx, function (a) { return "\\u" + ("0000" + a.charCodeAt(0).toString(16)).slice(-4) }) } if (/^[\],:{}\s]*$/.test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, "@").replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, "]").replace(/(?:^|:|,)(?:\s*\[)+/g, ""))) { j = eval("(" + text + ")"); return typeof reviver === "function" ? walk({ "": j }, "") : j } throw new SyntaxError("JSON.parse") } } } ());

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

            while ((
				i = str.toLowerCase().indexOf(
					token, i >= 0 ? i + newToken.length : 0
				)) !== -1
			) {
                str = str.substring(0, i) +
					newToken +
					str.substring(i + token.length);
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
    return this.indexOf(sub, 0) != -1;
}

String.prototype.Format = function (args) {
    var formatted = this;
    for (var i = 0; i < arguments.length; i++) {
        var regexp = new RegExp('\\{' + i + '\\}', 'gi');
        formatted = formatted.replace(regexp, arguments[i]);
    }
    return formatted;
};

String.prototype.FormatNumber = function (number) { // Format a number to 1,234,567.89
    number += String.Empty;
    x = number.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : String.Empty;
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return this + x1 + x2;
};

String.prototype.escapeHtml = function () { // Replace all ampersands with &amp; and all <’s and >’s with &lt; and &gt;, respectively:
    var result = String.Empty;
    for (var i = 0; i < this.length; i++) {
        if (this.charAt(i) == "&" && this.length - i - 1 >= 4 && this.substr(i, 4) != "&amp;") {
            result = result + "&amp;";
        } else if (this.charAt(i) == "<") {
            result = result + "&lt;";
        } else if (this.charAt(i) == ">") {
            result = result + "&gt;";
        } else {
            result = result + this.charAt(i);
        }
    }
    return result;
};

/////////////// LEGACY CODE ///////////////

function SetParameterValue(name, value, url) {
    var newParameter = (value != null) ? (name + "=" + value) : null;
    if (!url.contains("?")) {
        url = (newParameter == null) ? url : (url + "?" + newParameter);
        return url;
    }
    // Already contain ?
    var separator = url.contains("?" + name + "=") ? "?" : "&";
    if (!url.contains("&" + name + "=") && !url.contains("?" + name + "=")) {
        url = (newParameter == null) ? url : (url + "&" + newParameter);
        return url;
    }
    else {
        var i1 = url.indexOf(separator + name + "=");
        var tmp = url.substr(i1);
        var i2 = tmp.indexOf("&", 1);
        var oldParameter = i2 >= 0 ? url.substr(i1, i2) : tmp;
        url = url.replace(oldParameter, (newParameter == null) ? null : (separator + newParameter));
        return url;
    }
}

function GetParameterValue(name, url) {
    if (null == url || null == name || 0 == name.length) return null;
    var urlToCompare = url.toLowerCase();
    name = name.toLowerCase();
    var separator = urlToCompare.contains("?" + name + "=") ? "?" : "&";
    var parameter = separator + name + "=";
    var i1 = urlToCompare.indexOf(parameter);
    if (i1 == -1) return null;
    var tmp = url.substr(i1);
    var i2 = tmp.indexOf("&", 1);
    var value = i2 >= 0 ? tmp.substr(parameter.length, i2 - parameter.length) : tmp.substr(parameter.length);
    return value;
}

(function () { // Parse float with string has commas inside parseFloat(string, true)
    var proxied = window.parseFloat;
    window.parseFloat = function () {
        if (arguments[1] === true && typeof (arguments[0]) == 'string') {
            arguments[0] = arguments[0].replace(/,/g, String.Empty); // Replace all occurrences of comma to empty
        }
        return proxied.apply(this, arguments);
    };
})();

function isIE() { // Detect browser
    return /msie/i.test(navigator.userAgent) && !/opera/i.test(navigator.userAgent);
}
window.IE = isIE();

// Cookie
function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function setCookie(name, value, days, domain) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else {
        var expires = "";
    }
    document.cookie = name + "=" + value + expires + "; path=/;" + ((typeof domain != 'undefined') ? ("domain=" + domain) : String.Empty);
}

function deleteAllCookies() {
    var cookies = document.cookie.split(";");

    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i];
        var eqPos = cookie.indexOf("=");
        var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
        document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
    }
}

// Detect flash
FLASH_ENABLE = ((navigator.userAgent.match(/iPod/i)) || (navigator.userAgent.match(/iPhone/i)) || (navigator.userAgent.match(/iPad/i))) == null;
setCookie('Flash', !FLASH_ENABLE, 1);

// Manipulate style sheet class name
function HasClassName(element, className) {
    if (typeof element == 'string') element = document.getElementById(element);
    var elementClassName = element.className;
    if (typeof elementClassName == 'undefined') return false;
    return (elementClassName.length > 0 && (elementClassName == className || new RegExp("(^|\\s)" + className + "(\\s|$)").test(elementClassName)));
}

function AddClassName(element, className) {
    if (typeof element == 'string') element = document.getElementById(element);
    if (!HasClassName(element, className)) element.className += (element.className ? ' ' : '') + className;
    return element;
}

function RemoveClassName(element, className) {
    if (typeof element == 'string') element = document.getElementById(element);
    var elementClassName = element.className;
    if (typeof elementClassName == 'undefined') return false;
    element.className = element.className.replace(
	new RegExp("(^|\\s+)" + className + "(\\s+|$)"), ' ');
    return element;
}
// !!!IMPORTANT: This function is reserved for system management, do not use it.

function _addEvent(el, evname, func) {
    if (!el) return;
    if (el.attachEvent) { // IE
        el.attachEvent("on" + evname, func);
    }
    else if (el.addEventListener) { // Gecko / W3C
        el.addEventListener(evname, func, true);
    }
    else { // Opera (or old browsers)
        el["on" + evname] = func;
    }
}
// Prototype of AGE

function AGE() { };

AGE.prototype.FormatNumberFloat = function (number) { // Format a number to 12,345
    if (number == String.Empty) return 0;
    var sign;
    if (isNaN(number)) {
        number = String.Empty;
    }
    sign = (number == (number = Math.abs(number)));
    number = Math.floor(number * 100 + 0.50000000001);
    number = Math.floor(number / 100).toString();
    for (var i = 0; i < Math.floor((number.length - (1 + i)) / 3); i++) {
        number = number.substring(0, number.length - (4 * i + 3)) + ',' + number.substring(number.length - (4 * i + 3));
    }
    return (((sign) ? String.Empty : '-') + number);
};
// Format input number on event e, using: onkeyup=age.FormatNumber(event)
AGE.prototype.FormatNumber = function (event, isFloat) {
    var evt = event ? event : window.event; // Fix for IE7
    var obj = evt.currentTarget ? evt.currentTarget : evt.srcElement; // Fix for IE7
    var value = obj.value;
    var num;
    var dotPosition = 0;
    var afterDot;
    num = value.toString().replace(/\$|\,/g, String.Empty);
    dotPosition = num.indexOf(".");
    if ((typeof (isFloat) != 'undefined') && isFloat && dotPosition > 0) {
        afterDot = num.substring(dotPosition + 1, num.length).replace(/[^0-9]/g, String.Empty);
        num = num.substring(0, dotPosition);
    }
    num = age.FormatNumberFloat(num);
    if ((typeof (isFloat) != 'undefined') && isFloat && dotPosition > 0) {
        eval(obj).value = num + "." + afterDot;
    } else {
        eval(obj).value = num;
    }
    return true;
};

// !!!IMPORTANT: isSingle = true if you want just one function for this event
AGE.prototype.addEvent = function (el, evname, func, isSingle) {
    if (!el) return;
    if (el.attachEvent && isSingle != true) { // IE
        el.attachEvent("on" + evname, func);
    }
    else if (el.addEventListener && isSingle != true) { // Gecko / W3C
        el.addEventListener(evname, func, true);
    }
    else { // Opera (or old browsers)
        el["on" + evname] = func;
    }
}
AGE.prototype.removeEvent = function (el, evname, func) {
    if (!el) return;
    if (el.detachEvent) { // IE
        el.detachEvent("on" + evname, func);
    }
    else if (el.removeEventListener) { // Gecko / W3C
        el.removeEventListener(evname, func, true);
    }
    else { // Opera (or old browsers)
        el["on" + evname] = null;
    }
}
AGE.prototype.stopEvent = function (ev) {
    if (IE) {
        window.event.cancelBubble = true;
        window.event.returnValue = false;
    }
    else {
        ev.preventDefault();
        ev.stopPropagation();
    }
    return false;
};
// Create a html element
AGE.prototype.createElement = function (type, parent) {
    var el = null;
    if (document.createElementNS) {
        // use the XHTML namespace; IE won't normally get here unless
        // _they_ "fix" the DOM2 implementation.
        el = document.createElementNS("http://www.w3.org/1999/xhtml", type);
    }
    else {
        el = document.createElement(type);
    }
    if (typeof parent != "undefined") {
        parent.appendChild(el);
    }
    return el;
}
// Which element is nearer origin Oxy
AGE.prototype.beforeElement = function (o1, o2) {
    if (typeof o2 == 'undefined') return o1;
    if (typeof o1 == 'undefined') return o2;
    // Tto get offset position
    o1.style.position = 'relative';
    o2.style.position = 'relative';
    // Compare distance from origin
    var d1 = o1.offsetTop * o1.offsetTop + o1.offsetLeft * o1.offsetLeft;
    var d2 = o2.offsetTop * o2.offsetTop + o2.offsetLeft * o2.offsetLeft;
    if (d1 > d2) return o2;
    else
        return o1;
}
// Find first form element in px position with tag name
AGE.prototype.firstFormElement = function (tag) {
    var items = document.getElementsByTagName(tag);
    if (items.length > 0) {
        var c = items.length > 5 ? 5 : items.length;
        for (var i = 0; i < c; i++) {
            if (!items[i].getAttribute('noFocus') && // property indicate do not focus on this item
			!items[i].disabled && items[i].type != 'hidden' && !items[i].readonly && items[i].type != 'checkbox' && items[i].type != 'radio') {
                return items[i];
            }
        }
    }
}
// Calculate view area
AGE.prototype.CalculateViewport = function () {
    this.viewport = function ()
    { };
    if (!IE) {
        // in standards compliant mode (i.e. with a valid doctype as the first line in the document)
        if (typeof document.documentElement != 'undefined' && typeof document.documentElement.scrollWidth != 'undefined' && document.documentElement.scrollWidth != 0) {
            this.viewport.width = document.documentElement.scrollWidth;
            this.viewport.height = document.documentElement.scrollHeight;
        }
        this.viewport.width = Math.max(this.viewport.width, (Math.max(document.body.scrollWidth, document.body.clientWidth)));
        this.viewport.height = Math.max(this.viewport.height, (Math.max(document.body.scrollHeight, document.body.clientHeight)));
    }
    else {
        // in standards compliant mode (i.e. with a valid doctype as the first line in the document)
        if (typeof document.documentElement != 'undefined' && typeof document.documentElement.clientWidth != 'undefined' && document.documentElement.clientWidth != 0) {
            this.viewport.width = document.documentElement.clientWidth;
            this.viewport.height = document.documentElement.clientHeight;
        }
        this.viewport.width = Math.max(document.body.scrollWidth, this.viewport.width);
        this.viewport.height = Math.max(document.body.scrollHeight, this.viewport.height);
    }
}
// Create a mask div will cover whole page
AGE.prototype.CreateMaskDiv = function () {
    this.divMaskLoading = this.createElement("div");
    var div1 = this.createElement("div");
    div1.style.width = '100px';
    div1.style.height = '100px';
    div1.style.position = 'relative';
    this.divMaskLoading.style.display = 'none';
    this.divMaskLoading.style.position = 'absolute';
    this.divMaskLoading.style.top = '0px';
    this.divMaskLoading.style.left = '0px';
    this.divMaskLoading.style.filter = 'alpha(opacity=50)';
    this.divMaskLoading.style.opacity = '0.5';
    this.divMaskLoading.style.backgroundColor = 'white';
    this.divMaskLoading.appendChild(div1);
    document.body.appendChild(this.divMaskLoading);
}
AGE.prototype.RemoveBookmarksInUrl = function (url) {
    if (typeof url != 'string') return null;
    var i = url.indexOf('#');
    if (i == -1) return url;
    return url.substr(0, i);
}
// Show a mask div cover whole page
AGE.prototype.ShowMaskDiv = function (withIcon) {
    if (!this.divMaskLoading) return;
    this.CalculateViewport();
    this.divMaskLoading.style.width = (this.viewport.width - 2) + 'px';
    this.divMaskLoading.style.height = (this.viewport.height - 2) + 'px';
    this.divMaskLoading.firstChild.style.left = Math.floor(this.viewport.width / 2) + 'px';
    this.divMaskLoading.firstChild.style.top = document.documentElement.scrollTop + 'px';
    this.divMaskLoading.firstChild.className = withIcon ? 'MaskLoadingDiv' : '';
    this.divMaskLoading.style.display = 'block';
}
// Hide showed mask div
AGE.prototype.HideMaskDiv = function () {
    if (!this.divMaskLoading) return;
    this.divMaskLoading.style.display = 'none';
}
AGE.prototype.ReloadPage = function (time) {
    var age = new AGE();
    this.ShowMaskDiv(true);
    var delay = time ? time : 2000;
    this.delayTimer = setTimeout("age.HideMaskDiv()", delay);
}

AGE.prototype.ReloadParentPage = function (url, time) {
    this.ShowMaskDiv(true);
    var delay = time ? time : 2000;
    if (!url) {
        parent.location.nextUrl = this.RemoveBookmarksInUrl(location.href);
        this.delayTimer = setTimeout("parent.location = location.nextUrl", delay);
    }
    else {
        this.delayTimer = setTimeout("parent.location = '" + url + "'", delay);
    }
}

// Reload page with provided url and delay time
AGE.prototype.DelayReloadPage = function (url, time) {
    this.ShowMaskDiv(true);
    var delay = time ? time : 2000;
    if (!url) {
        location.nextUrl = this.RemoveBookmarksInUrl(location.href);
        this.delayTimer = setTimeout("location = location.nextUrl", delay);
    }
    else {
        this.delayTimer = setTimeout("location = '" + url + "'", delay);
    }
}
// Catch enter event and do the action, using by onkeydown="age.DoEnter(...)"
AGE.prototype.DoEnter = function (evt, action) {
    if (null == age) return;
    if (IE) {
        if (window.event.keyCode == 13) eval(action);
    }
    else {
        if (evt.keyCode == 13) eval(action);
    }
}
AGE.prototype.Lock = function (withIcon) {
    this.ShowMaskDiv(withIcon);
    this.locked = true;
}
AGE.prototype.UnLock = function () {
    this.HideMaskDiv();
    this.locked = false;
}
// Refresh whole site to update session, cookies...
AGE.prototype.Refresh = function () {
    var wnd = window.parent;
    while (wnd != wnd.parent) // Jump out
    {
        wnd = wnd.parent;
    }
    wnd.location = wnd.location.href;
}
AGE.prototype.GetBaseUrl = function () {
    var baseUrl = '';
    if (parent.baseURL) {
        baseUrl = parent.baseURL;
    }
    else if (parent.parent.baseURL) {
        baseUrl = parent.parent.baseURL;
    }
    else if (parent.parent.parent.baseURL) {
        baseUrl = parent.parent.parent.baseURL;
    }
    return baseUrl;
    //        var url = window.location.href;
    //        return url.toString().replace(/^(.*\/\/[^\/?#]*).*$/, "$1") + "/";
    //return parent.location.host + "/";
}
AGE.prototype.GetQueryString = function (key, defaultValue) {
    if (defaultValue == null) defaultValue = "";
    key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
    var qs = regex.exec(window.location.href);
    if (qs == null) return defaultValue;
    else
        return qs[1];
} /*End*/
/*
* Created 20091224@Lee - Light ajax library
*
*!!!CAUTION: ANY REVISION MUST BE APPROVED BEFORE PROCEEDING!!!
* Revision ?@? - ...
*
*/
//jx.request(url,function(result){...},method,params,async);
//method:post,get
//async:true,false
//params:a string, an object or an array
jx = {
    toQueryString: function (obj) {
        if (typeof obj == 'object') {
            var arr = new Array();
            for (var att in obj) {
                arr.push(att + '=' + eval('obj.' + att));
            }
            return arr.join('&');
        }
        else if (typeof obj == 'string') {
            return obj.replace('?', '');
        }
        else {
            return obj;
        }
    },
    getHTTPObject: function () {
        var A = false;
        if (typeof ActiveXObject != "undefined") {
            try {
                A = new ActiveXObject("Msxml2.XMLHTTP")
            }
            catch (C) {
                try {
                    A = new ActiveXObject("Microsoft.XMLHTTP")
                }
                catch (B) {
                    A = false
                }
            }
        }
        else {
            if (window.XMLHttpRequest) {
                try {
                    A = new XMLHttpRequest()
                }
                catch (C) {
                    A = false
                }
            }
        }
        return A
    },
    request: function (url, callback, method, params, async) {
        var http = this.init();
        if (!http || !url || !method) {
            return
        }
        try {
            method = method.toUpperCase();
            if (typeof async == "undefined") async = true;
            var isGet = (method == "GET");
            var ch = (url.indexOf("?") == -1) ? "?" : "&";
            ch = params ? ch : "";
            http.open(method, (isGet && typeof params != "undefined") ? url + ch + this.toQueryString(params) : url, async);
            http.onreadystatechange = function () {
                if (http.readyState == 4) {
                    if (http.status == 200) {
                        var result = http;
                        if (callback && async) {
                            callback(result, http.getResponseHeader('Date'))
                        }
                    }
                    else if (http.status != 0) { /*alert(http.statusText)*/
                        if (callback && async) {
                            callback({ 'errCode': http.status, 'errMsg': http.statusText }, http.getResponseHeader('Date'))
                        }
                    }
                }
            }; if (!isGet) {
                http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
            };
            http.send(isGet ? null : this.toQueryString(params));
            if (!async) callback(http);
            return http;
        }
        catch (e) { callback(http); }
    },
    init: function () {
        return this.getHTTPObject()
    }
}
// Ajax class

function AJAX() { };

AJAX.prototype.Request = function (url, options) {
    //url:'url?var1=val1&var2=val2...'
    //options:{method:'get/post',asynchronous:true/false,parameters:'var3=val3&var4=val4',onComplete:function(data){}}
    var method = 'post';
    var callback = function () { },
		params = null;
    var async = true;
    if (options) {
        if (typeof options.method != 'undefined') method = options.method;
        if (typeof options.parameters != 'undefined') params = options.parameters;
        if (typeof options.onComplete != 'undefined') callback = options.onComplete;
        else if (typeof options.onSuccess != 'undefined') callback = options.onSuccess;
        if (typeof options.asynchronous != 'undefined') async = options.asynchronous;
    }
    return jx.request(url, callback, method, params, async);
}
//JSON Get
//JSON Post
//asynchronous=true
AJAX.prototype.CreateParams = function () {
    // Create params string from array of element id
    var params = new Array();
    var c = arguments.length;
    for (var i = 0; i < c; i++) {
        var element = document.getElementById(arguments[i]);
        var query = element.name;
        if (null == query || '' == query) {
            query = element.id;
        }
        var value = null;
        if (element.tagName == 'INPUT') {
            if (element.type == 'checkbox' || element.type == 'radio') value = element.checked;
            else
                value = element.value;
        }
        else if (element.tagName == 'SELECT') value = element.value;
        if (null != value) params.push(query + '=' + encodeURIComponent(value));
    }
    return params.join('&');
}
AJAX.prototype.CreateParams2 = function (ids) {
    // Create params string from array of element id
    var params = new Array();
    var c = ids.length;
    for (var i = 0; i < c; i++) {
        var element = document.getElementById(ids[i]);
        var query = element.name;
        if (null == query || '' == query) {
            query = element.id;
        }
        var value = null;
        if (element.tagName == 'INPUT') {
            if (element.type == 'checkbox' || element.type == 'radio') value = element.checked;
            else
                value = element.value;
        }
        else if (element.tagName == 'SELECT') value = element.value;
        if (null != value) params.push(query + '=' + encodeURIComponent(value));
    }
    return params.join('&');
}
AJAX.prototype.GetJSON = function (url, params, callback, async) {
    var callback1 = function (result) {
        var data = '({"errCode":-1, "errMsg":"Oops... you are here because of a system error, please notify the administrators!"})';
        try {
            if (result.getResponseHeader("Content-type").indexOf("text/plain") != -1) {
                data = '(' + result.responseText + ')';
            }
        }
        catch (e) {
        }
        var obj = eval(data);
        callback(obj);
    }
    if (typeof async != 'boolean') async = true;
    return jx.request(url, callback1, 'GET', params, async);
}
AJAX.prototype.PostJSON = function (url, params, callback, async) {
    var callback1 = function (result) {
        var data = '({"errCode":-1, "errMsg":"Oops... you are here because of a system error, please notify the administrators!"})';
        try {
            if (result.getResponseHeader("Content-type").indexOf("text/plain") != -1) {
                data = '(' + result.responseText + ')';
            }
        }
        catch (e) {
        }
        var obj = eval(data);
        callback(obj);
    }
    if (typeof async != 'boolean') async = true;
    return jx.request(url, callback1, 'POST', params, async);
}
/**************OPEN ASYNS TASK SECTION************/
// Support register from child frame or child window
function RegisterAsyncRequest(url, parameters, method, delay) {
    setTimeout(function () {
        ajax.Request(url.clone(), { method: method.clone(), parameters: parameters.clone() });
    }, delay.clone());
}

/**************CLOSE ASYNS TASK SECTION***********/

/**************OPEN START-UP SECTION**************/
_page = {}; // root JSON object
var _startupCommands = new Array();

function RegisterStartUp(command) {
    _startupCommands.push(command);
}
// Delay init object to window.onload event to boots up performance
age = null;
ajax = null;

function _startup() {
    age = new AGE();
    ajax = new AJAX();
    // Focus at startup
    if (typeof _focusElementId == 'undefined') {
        var input = age.firstFormElement("input");
        var select = age.firstFormElement("select");
        var item = age.beforeElement(input, select);
        try {
            if (item) {
                if (!$('input').hasClass('datepicker')) {
                    item.focus();
                }
            }
        }
        catch (e)
		{ }
    }
    else if (_focusElement != -1) {
        var _focusElement = document.getElementById(_focusElementId);
        if (_focusElement != null && typeof _focusElement == 'object') _focusElement.focus();
    }
    age.CreateMaskDiv();
    // Calculate width and height of view port
    age.CalculateViewport();
    // Execute startup commands
    var commands = _startupCommands;
    var length = commands.length;
    for (var i = 0; i < length; i++) {
        var command = commands[i];
        if (typeof (command) == 'string') {
            eval(command);
        }
        else if (typeof (command) == 'function') {
            command.call(this);
        };
    }

    window.loaded = true;
    checkPermission();
}

function checkPermission() {
    try {
        var bolBtnSubmit = checkButtonPermission('btnSubmit');
        var bolBtnSubmitFooter = checkButtonPermission('btnSubmitFooter');
        var bolBtnCancel = checkButtonPermission('btnCancel');
        var bolBtnReset = checkButtonPermission('btnReset');
        var bolBtnUpdRacingStatus = checkButtonPermission('btnUpdRacingStatus');
        if (bolBtnSubmit || bolBtnSubmitFooter || bolBtnCancel || bolBtnReset || bolBtnUpdRacingStatus) {
            // Check Type Input
            var ids = document.getElementsByTagName("input");
            for (var i = 0; i < ids.length; i++) {
                var permission = ids[i].getAttribute('AccountPermisssion');
                if (permission != null) {
                    if (permission.length != 0) {
                        if (top.AGE.PERMISSIONS.indexOf(permission) == -1) {
                            ids[i].disabled = 'disable';
                        }
                    }
                }
            }

            // Check Type Button
            var ids = document.getElementsByTagName("button");
            for (var i = 0; i < ids.length; i++) {
                var permission = ids[i].getAttribute('AccountPermisssion');
                if (permission != null) {
                    if (permission.length != 0) {
                        if (top.AGE.PERMISSIONS.indexOf(permission) == -1) {
                            ids[i].disabled = 'disable';
                        }
                    }
                }
            }
        }
    }
    catch (e) {
    }
}

function checkButtonPermission(id) {
    try {
        if (document.getElementById(id) != null && document.getElementById(id) != undefined) {
            var permission = document.getElementById(id).getAttribute('AccountPermisssion');
            if (permission == null || permission.length == 0) return false;
            if (top.AGE.PERMISSIONS.indexOf(permission) == -1) {
                document.getElementById(id).disabled = 'disable';
                return true;
            }
        }
    }
    catch (e) { return false; }
    return false;
}

_addEvent(window, "load", _startup); /************CLOSE START-UP SECTION*****************/

/*
* Created 20091224@Lee - Common business javascript functions
* Revision ?@? - ...
*
*/
// Frameset
// top.menu, top.main, top.frHeader, top.frFooter
// Error code
var UNKNOWN = -1;
var ACCESS_DENIED = 1;
var ACCOUNT_CLOSED = 2;
var KICKED_OUT = 3;
var UNDER_MAINTENANCE = 4;
// Reload frames to update language or configuration
AGE.prototype.Invalidate = function () {
    top.location = age.RemoveBookmarksInUrl(top.location.href);
}
// SignOut
//AGE.prototype.SignOut = function () {
//    var index = window;

//    while (index != index.parent) // Jump out
//    {
//        index = index.parent;
//    }

//    //index.location = this.GetBaseUrl() + '_Authorization/Handlers/SignOut.ashx';
//    //var signOutUrl = APOLLO.resolveUrl('~/_Authorization/Handlers/SignOut.ashx');

//    index.location = APOLLO.resolveUrl('~/Auth/SignOut');
//}

// Password must be at least 6 in length and at least 2 characters and 2 digits
function IsPassword(str) {
    str = str.toLowerCase();
    numbers = "0123456789";
    var i = 0;
    var nCount = 0;
    while (i < str.length) {
        (numbers.indexOf(str.substring(i, i + 1)) >= 0) ? nCount++ : false;
        i++;
    }
    var validNumber = ((nCount > (str.length - 2)) || nCount < 2 || str.length < 6) ? false : true;

    letters = "abcdefghijklmnopqrstuvwxyz";
    i = 0;
    nCount = 0;
    while (i < str.length) {
        (letters.indexOf(str.substring(i, i + 1)) >= 0) ? nCount++ : false;
        i++;
    }
    var validLetter = ((nCount > (str.length - 2)) || nCount < 2 || str.length < 6) ? false : true;

    return validNumber & validLetter;
}

function getPrint(print_area) {
    var printContent = document.getElementById(print_area);
    var printWindow = window.open('', '', 'left=500,top=400,width=200,height=5');
    printWindow.document.write("<html>");
    printWindow.document.write("<head>");
    printWindow.document.write("</head>");
    printWindow.document.write("<body style='margin-top: 100px'>");
    printWindow.document.write(printContent.innerHTML);
    printWindow.document.write("</body></html>");
    printWindow.document.close();
    printWindow.focus();
    printWindow.print();
    printWindow.close();
}