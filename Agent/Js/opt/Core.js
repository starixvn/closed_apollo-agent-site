function $(id) {
    return document.getElementById(id);
}

window.$ = $;

if (typeof window.parent.$ == 'undefined') {
    window.parent.$ = function(id) {
        return this.document.getElementById(id);
    }
}

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

String.prototype.contains = function(sub) {
    return this.indexOf(sub, 0) != -1;
}

// Detect browser
function isIE() { return /msie/i.test(navigator.userAgent) && !/opera/i.test(navigator.userAgent); }

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

function setCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

// Utility functions
var IE = isIE();

var AGE = function() { };
// !!!IMPORTANT: addOnce = true if you want just one function for this event
AGE.prototype.addEvent = function(el, evname, func, addOnce) {
    if (el.attachEvent && addOnce != true) { // IE
        el.attachEvent("on" + evname, func);
    } else if (el.addEventListener && addOnce != true) { // Gecko / W3C
        el.addEventListener(evname, func, true);
    } else { // Opera (or old browsers)
        el["on" + evname] = func;
    }
}

AGE.prototype.removeEvent = function(el, evname, func) {
    if (el.detachEvent) { // IE
        el.detachEvent("on" + evname, func);
    } else if (el.removeEventListener) { // Gecko / W3C
        el.removeEventListener(evname, func, true);
    } else { // Opera (or old browsers)
        el["on" + evname] = null;
    }
}

AGE.prototype.stopEvent = function(ev) {
    if (IE) {
        window.event.cancelBubble = true;
        window.event.returnValue = false;
    } else {
        ev.preventDefault();
        ev.stopPropagation();
    }
    return false;
};

AGE.prototype.calculateViewport = function() {
    window.viewport = [];
    if (!IE) {
        // in standards compliant mode (i.e. with a valid doctype as the first line in the document)
        if (typeof document.documentElement != 'undefined'
         && typeof document.documentElement.scrollWidth !=
         'undefined' && document.documentElement.scrollWidth != 0) {
            window.viewport.width = document.documentElement.scrollWidth;
            window.viewport.height = document.documentElement.scrollHeight;
        }

        window.viewport.width = Math.max(window.viewport.width, (Math.max(document.body.scrollWidth, document.body.clientWidth)));
        window.viewport.height = Math.max(window.viewport.height, (Math.max(document.body.scrollHeight, document.body.clientHeight)));
    }
    else {
        // in standards compliant mode (i.e. with a valid doctype as the first line in the document)
        if (typeof document.documentElement != 'undefined'
         && typeof document.documentElement.clientWidth !=
         'undefined' && document.documentElement.clientWidth != 0) {
            window.viewport.width = document.documentElement.clientWidth;
            window.viewport.height = document.documentElement.clientHeight;
        }
        // older versions of IE
        else {
            window.viewport.width = document.getElementsByTagName('body')[0].clientWidth;
            window.viewport.height = document.getElementsByTagName('body')[0].clientHeight;
        }

        window.viewport.width = Math.max(document.body.scrollWidth, window.viewport.width);
        window.viewport.height = Math.max(document.body.scrollHeight, window.viewport.height);
    }
}

AGE.prototype.createElement = function(type, parent) {
    var el = null;
    if (document.createElementNS) {
        // use the XHTML namespace; IE won't normally get here unless
        // _they_ "fix" the DOM2 implementation.
        el = document.createElementNS("http://www.w3.org/1999/xhtml", type);
    } else {
        el = document.createElement(type);
    }
    if (typeof parent != "undefined") {
        parent.appendChild(el);
    }
    return el;
}

// HTML page functions
function CreateMaskLoadingDiv() {
    window.divMaskLoading = AGE.createElement("div");
    var div1 = AGE.createElement("div");
    div1.style.width = '100px'; div1.style.height = '100px';
    div1.style.position = 'relative';
    div1.style.background = 'url(../../App_Themes/Login/loader3.gif)';
    window.divMaskLoading.style.display = 'none';
    window.divMaskLoading.style.position = 'absolute';
    window.divMaskLoading.style.top = '0px';
    window.divMaskLoading.style.left = '0px';
    window.divMaskLoading.style.filter = 'alpha(opacity=50)';
    window.divMaskLoading.style.opacity = '0.5';
    window.divMaskLoading.style.backgroundColor = 'white';
    window.divMaskLoading.appendChild(div1);
    document.body.appendChild(window.divMaskLoading);
}

function RemoveBookmarksInUrl(url) {
    if (typeof url != 'string') return null;
    var i = url.indexOf('#');
    if (i == -1) return url;
    return url.substr(0, i);
}

function ReloadParentPage(url, time) {
    if (!window.divMaskLoading) return;
    if (typeof ageWnd != 'undefined') if (ageWnd.loaded) ageWnd.close(true);

    window.divMaskLoading.style.width = (window.viewport.width - (IE ? 22 : 2)) + 'px';
    window.divMaskLoading.style.height = (window.viewport.height) + 'px';

    window.divMaskLoading.firstChild.style.left = Math.floor(window.viewport.width / 2) + 'px';
    window.divMaskLoading.firstChild.style.top = document.documentElement.scrollTop + 'px';
    window.divMaskLoading.style.display = 'block';
    var delay = time ? time : 2000;
    if (!url) {
        window.loadingT = setTimeout("parent.location = RemoveBookmarksInUrl(location.href)", delay);
    }
    else {
        window.loadingT = setTimeout("parent.location = '" + url + "'", delay);
    }
}

function DelayReloadPage(url, time) {
    if (!window.divMaskLoading) return;
    if (typeof ageWnd != 'undefined') if (ageWnd.loaded) ageWnd.close(true);

    window.divMaskLoading.style.width = (window.viewport.width - (IE ? 22 : 2)) + 'px';
    window.divMaskLoading.style.height = (window.viewport.height) + 'px';

    window.divMaskLoading.firstChild.style.left = Math.floor(window.viewport.width / 2) + 'px';
    window.divMaskLoading.firstChild.style.top = document.documentElement.scrollTop + 'px';
    window.divMaskLoading.style.display = 'block';
    var delay = time ? time : 2000;
    if (!url) {
        window.loadingT = setTimeout("location = RemoveBookmarksInUrl(location.href)", delay);
    }
    else {
        window.loadingT = setTimeout("location = '" + url + "'", delay);
    }
}

function DelayPage(time) {
    if (!window.divMaskLoading) return;
    if (typeof ageWnd != 'undefined') if (ageWnd.loaded) ageWnd.close(true);

    window.divMaskLoading.style.width = (window.viewport.width - (IE ? 22 : 2)) + 'px';
    window.divMaskLoading.style.height = (window.viewport.height) + 'px';

    window.divMaskLoading.firstChild.style.left = Math.floor(window.viewport.width / 2) + 'px';
    window.divMaskLoading.firstChild.style.top = document.documentElement.scrollTop + 'px';
    window.divMaskLoading.style.display = 'block';
    var delay = time ? time : 2000;
    window.loadingT = setTimeout("HideLoadingDiv()", delay);
}

function HideLoadingDiv() {
    if (!window.divMaskLoading) return;
    window.divMaskLoading.style.display = 'none';
    if (window.loadingT) {
        clearTimeout(window.loadingT);
    }
}

function DisablePage(isDisabled) {
    if (!window.divMaskLoading) return;
    if (typeof ageWnd != 'undefined') if (ageWnd.loaded) ageWnd.close();
    if (typeof isDisabled == 'undefined') isDisabled = true;

    window.divMaskLoading.style.width = (window.viewport.width - (IE ? 22 : 2)) + 'px';
    window.divMaskLoading.style.height = (window.viewport.height) + 'px';

    window.divMaskLoading.firstChild.style.left = Math.floor(window.viewport.width / 2) + 'px';
    window.divMaskLoading.firstChild.style.top = document.documentElement.scrollTop + 'px';
    window.divMaskLoading.style.display = isDisabled ? 'block' : 'none';
}

function DelayReloadPageSimple() {
    var delay2 = 2000;
    setTimeout("window.location=RemoveBookmarksInUrl(window.location.href)", delay2);
}

// start up script ---------------------------
AGE = new AGE();
AGE.addEvent(window, "load", _startup);
function _startup() {
    // focus at startup
    if (typeof _FocusEl == 'undefined') {
        var input = firstFormElement("input");
        var select = firstFormElement("select");
        var item = beforeElement(input, select);
        try { if (item) item.focus(); }
        catch (e) { }
    }
    else {
        $(_FocusEl).focus();
    }
    // create loading div
    CreateMaskLoadingDiv();
    // calculate width and height
    AGE.calculateViewport();
    // override submit method of HTMLFormElement to create floating div
    if (document.forms[0]) {
        AGE.addEvent(document.forms[0], "submit", _submit);
    }

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

function _submit() {
    var loading = $("loading");
    if (loading) loading.className = "loading1";
}

function beforeElement(o1, o2) {
    if (typeof o2 == 'undefined') return o1;
    if (typeof o1 == 'undefined') return o2;

    // to get offset position
    o1.style.position = 'relative';
    o2.style.position = 'relative';

    // compare distance from origin
    var d1 = o1.offsetTop * o1.offsetTop + o1.offsetLeft * o1.offsetLeft;
    var d2 = o2.offsetTop * o2.offsetTop + o2.offsetLeft * o2.offsetLeft;

    if (d1 > d2) return o2;
    else return o1;
}

function firstFormElement(tag) {
    var items = document.getElementsByTagName(tag);
    if (items.length > 0) {
        var c = items.length > 5 ? 5 : items.length;
        for (var i = 0; i < c; i++) {
            if (
                !items[i].getAttribute('noFocus') &&    // property indicate do not focus on this item
                !items[i].disabled &&
                items[i].type != 'hidden' &&
                !items[i].readonly &&
                items[i].type != 'checkbox' &&
                items[i].type != 'radio'
                ) {
                return items[i];
            }
        }
    }
}

function getShortDate(dtmDate) {
    return dtmDate.getMonth() + 1 + "/" + dtmDate.getDate() + "/" + dtmDate.getFullYear();
}

function check(str) {
    numbers = new String("0123456789");
    var i = 0;
    var nCount = 0;
    while (i < str.length) {
        (numbers.indexOf(str.substring(i, i + 1)) >= 0) ? nCount++ : false;
        i++;
    }
    return ((nCount > (str.length - 2)) || nCount < 2) ? false : true;
}

function formatNumber(num, dec, thou, pnt, curr1, curr2, n1, n2) {
    var sign = '';
    if (num < 0) sign = '-';
    var x = Math.round(num * Math.pow(10, dec));
    if (x >= 0) n1 = n2 = '';
    var y = ('' + Math.abs(x)).split('');
    var z = y.length - dec;
    y.splice(z, 0, pnt);
    while (z > 3) {
        z -= 3;
        y.splice(z, 0, thou);
    }
    var r = curr1 + n1 + y.join('') + n2 + curr2;
    if (num > -1 && num < 1 && r.indexOf('.') == 0) r = "0" + r;
    return sign + r;
}
//format number to form: #,###,###
function formatNumber2(nr) {
    return formatNumber(nr, 0, ',', '', '', '', '', '');
}
//format number to form: #,###,###.00
function formatNumber32(nr) {
    return formatNumber(nr, 2, ',', '.', '', '', '', '');
}

String.prototype.trimLeft = function() {
    return this.replace(/^\s*/, "");
}

String.prototype.trimRight = function() {
    return this.replace(/\s*$/, "");
}

String.prototype.trim = function() {
    return this.trimRight().trimLeft();
}

String.prototype.replaceAll = function(str, findStr, replacingStr) {
    return str.replace(eval('/' + findStr + '/g'), replacingStr);
}

String.prototype.replaceAll = function(findStr, replacingStr) {
    return this.replace(eval('/' + findStr + '/g'), replacingStr);
}

function IsNumeric(sText) {
    var ValidChars = "0123456789";
    var IsNumber = true;
    var Char;
    for (i = 0; i < sText.length && IsNumber == true; i++) {
        Char = sText.charAt(i);
        if (ValidChars.indexOf(Char) == -1) {
            IsNumber = false;
        }
    }
    return IsNumber;
}

function isEmpty(id) {
    if (!$(id)) {
        alert(id + ' does not exists.');
        return;
    }
    return $(id).value == "";
}

//assign error into span
function alert_error(errmsg, errHolderId) {
    $(errHolderId).innerHTML = errmsg;
}

//check string input field by id , return true or false
function isString(id) {
    return isNaN(ReplaceAll($(id).value, ',', ''));
}

//Find all occurrence of regexp in str and replace it with newstr
function ReplaceAll(str, regexp, newstr) {
    return str.replace(eval('/' + regexp + '/g'), newstr);
}

//check Numeric input field by id , return value
function isNumeric(id) {
    return parseFloat(ReplaceAll($(id).value, ',', ''));
}

function round(number, X) {
    X = (!X ? 2 : X);
    return Math.round(number * Math.pow(10, X)) / Math.pow(10, X);
}

//Check for decimal number
function CheckDecNumber(e) {
    var code = (e.which) ? e.which : e.keyCode;
    if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 0 && code != 46 && (code < 37 || code > 40)) {
        return false;
    }
    return true;
}

function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}

//do not allow enter text on numberic field
function CheckNumber(e, onEnter) {
    var code = "";
    if (e.which)
        code = e.which;
    else if (e.keyCode)
        code = e.keyCode;
    if (code == 37 || code == 39)
        return true;
    if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 0 && code != 46 && code != 9) {
        return false;
    }
    if (code == 13) {
        onEnter();
    }
    return true;
}

function getPrint(print_area) {
    var printContent = $(print_area);
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

function validateEmail(email) {
    var emailRegEx = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return email.match(emailRegEx);
}

function ltrim(str) {
    for (var k = 0; k < str.length && isWhitespace(str.charAt(k)); k++);
    return str.substring(k, str.length);
}

function rtrim(str) {
    for (var j = str.length - 1; j >= 0 && isWhitespace(str.charAt(j)); j--);
    return str.substring(0, j + 1);
}

function trim(str) {
    return ltrim(rtrim(str));
}

function isWhitespace(charToCheck) {
    var whitespaceChars = " \t\n\r\f";
    return (whitespaceChars.indexOf(charToCheck) != -1);
}

function findItemInArr(item, array) {
    if (typeof array != 'object') return -1;
    var c = array.length;
    if (typeof c == 'undefined') return -1;
    for (var i = c; i >= 0; i--) {
        if (array[i] == item) return i;
    }
    return -1;
}

function findPosition(oElement) {
    if (typeof (oElement.offsetParent) != 'undefined') {
        for (var posX = 0, posY = 0; oElement; oElement = oElement.offsetParent) {
            posX += oElement.offsetLeft;
            posY += oElement.offsetTop;
        }
        return [posX, posY];
    } else {
        return [oElement.x, oElement.y];
    }
}

// Manipulate style sheet class name
function hasClassName(element, className) {
    if (typeof element == 'string') element = $(element);
    var elementClassName = element.className;
    if (typeof elementClassName == 'undefined') return false;
    return (elementClassName.length > 0 && (elementClassName == className ||
  new RegExp("(^|\\s)" + className + "(\\s|$)").test(elementClassName)));
}

function addClassName(element, className) {
    if (typeof element == 'string') element = $(element);
    if (!hasClassName(element, className))
        element.className += (element.className ? ' ' : '') + className;
    return element;
}

function removeClassName(element, className) {
    if (typeof element == 'string') element = $(element);
    var elementClassName = element.className;
    if (typeof elementClassName == 'undefined') return false;
    element.className = element.className.replace(
  new RegExp("(^|\\s+)" + className + "(\\s+|$)"), ' ');
    return element;
}

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

function AJAX()
{ };
AJAX.prototype.Request = function (url, options) {
    //url:'url?var1=val1&var2=val2...'
    //options:{method:'get/post',asynchronous:true/false,parameters:'var3=val3&var4=val4',onComplete:function(data){}}
    var method = 'post';
    var callback = 'void(0)',
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

// Init ajax object
ajax = new AJAX();
Ajax = ajax; // For old script