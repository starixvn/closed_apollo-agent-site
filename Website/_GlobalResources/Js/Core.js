/*
* Created 20091224@Lee - Core javascript functions and classes
*
* !!!CAUTION: ANY REVISION MUST BE APPROVED BEFORE PROCEEDING!!!
* Revision ?@? - ...
*
*/
function $(id) {
	return document.getElementById(id);
}
// Returns the string with all the beginning
// and ending whitespace removed
String.prototype.trim = function () {
	return this.replace(/^\s+/, '').replace(/\s+$/, '');
};

Object.prototype.clone = function () {
    var newObj;
    if (this instanceof Array) {
        newObj = [];
    }
    else if (typeof this == 'string') {
        newObj = this + String.Empty;
    }
    else if (typeof this == 'number') {
        newObj = this + 0;
    }
    else {
        newObj = {};
    }

    for (i in this) {
        if (i == 'clone') continue;
        if (this[i] && typeof this[i] == "object") {
            newObj[i] = this[i].clone();
        } else {
            newObj[i] = this[i];
        }
    }
    return newObj;
};

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

String.prototype.contains = function (sub) {
	return this.indexOf(sub, 0) != -1;
}

String.Empty = ""; // Extend static string

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

(function () { // Parse float with string has commas inside parseFloat(string, true)
	var proxied = window.parseFloat;
	window.parseFloat = function () {
		if (arguments[1] === true && typeof (arguments[0]) == 'string') {
			arguments[0] = arguments[0].replace(/,/g, String.Empty); // Replace all occurrences of comma to empty
		}
		return proxied.apply(this, arguments);
	};
})();

// Detect browser

function isIE() {
	return /msie/i.test(navigator.userAgent) && !/opera/i.test(navigator.userAgent);
}
IE = isIE();

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
	if (typeof element == 'string') element = $(element);
	var elementClassName = element.className;
	if (typeof elementClassName == 'undefined') return false;
	return (elementClassName.length > 0 && (elementClassName == className || new RegExp("(^|\\s)" + className + "(\\s|$)").test(elementClassName)));
}

function AddClassName(element, className) {
	if (typeof element == 'string') element = $(element);
	if (!HasClassName(element, className)) element.className += (element.className ? ' ' : '') + className;
	return element;
}

function RemoveClassName(element, className) {
	if (typeof element == 'string') element = $(element);
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

function AGE()
{ };

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
	var url = window.location.href;
	var mark = '))/';
	var end = url.indexOf(mark);
	if (end == -1) {
		var slashNum = 3; // Public site
		if (url.indexOf('.com') == -1 && url.indexOf('.net') == -1 && url.indexOf('.org') == -1) slashNum = 4; // Local site
		end = 0;
		for (var i = 0; i < slashNum; i++) {
			end = url.indexOf('/', end + 1);
			if (end == -1) return null;
		}
		end++;
	}
	else {
		end += mark.length;
	}
	return url.substr(0, end);
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

function AJAX()
{ };
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
	// $ function
	window.$ = $;
	if (typeof window.parent.$ == 'undefined') {
		window.parent.$ = function (id) {
			return this.document.getElementById(id);
		}
	}
	age = new AGE();
	ajax = new AJAX();
	// Focus at startup
	if (typeof _focusElementId == 'undefined') {
	    var input = age.firstFormElement("input");
	    var select = age.firstFormElement("select");
	    var item = age.beforeElement(input, select);
	    try {
	        if (item) item.focus();
	    }
	    catch (e)
        { }
	}
	else if (_focusElement != -1) {
	    var _focusElement = $(_focusElementId);
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
AGE.prototype.SignOut = function () {
	var index = window;
	while (index != index.parent) // Jump out
	{
		index = index.parent;
	}
	index.location = this.GetBaseUrl() + '_Authorization/Handlers/SignOut.ashx';
	if (index.location.href.indexOf('https://imb.') == 0) {
		index.close();
	}
}

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
// Add popup height when show error
var _preHeight = 0;

function AddPopupHeight(h) {
	ageWnd = parent.ageWnd;
	if (typeof ageWnd == 'undefined') return;
	if (_preHeight == 0) {
		_preHeight = ageWnd.height;
	}
	ageWnd.setRect(null, null, ageWnd.width, _preHeight + h);
}

function OpenIPInfo(ip) {
	ageWnd.open(age.GetBaseUrl() + '_IPInfo/IpInfo.aspx?ip=' + ip, '', 300, 250, 400, 150);
} /*End*/

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

/*
RequireJS 2.0.2 Copyright (c) 2010-2012, The Dojo Foundation All Rights Reserved.
Available via the MIT or new BSD license.
see: http://github.com/jrburke/requirejs for details
*/

var requirejs, require, define; (function (Z) { function w(a) { return J.call(a) === "[object Function]" } function G(a) { return J.call(a) === "[object Array]" } function q(a, b) { if (a) { var c; for (c = 0; c < a.length; c += 1) if (a[c] && b(a[c], c, a)) break } } function N(a, b) { if (a) { var c; for (c = a.length - 1; c > -1; c -= 1) if (a[c] && b(a[c], c, a)) break } } function x(a, b) { for (var c in a) if (a.hasOwnProperty(c) && b(a[c], c)) break } function K(a, b, c, d) { b && x(b, function (b, e) { if (c || !a.hasOwnProperty(e)) d && typeof b !== "string" ? (a[e] || (a[e] = {}), K(a[e], b, c, d)) : a[e] = b }); return a } function s(a, b) { return function () { return b.apply(a, arguments) } } function $(a) { if (!a) return a; var b = Z; q(a.split("."), function (a) { b = b[a] }); return b } function aa(a, b, c) { return function () { var e = ga.call(arguments, 0), g; if (c && w(g = e[e.length - 1])) g.__requireJsBuild = !0; e.push(b); return a.apply(null, e) } } function ba(a, b, c) { q([["toUrl"], ["undef"], ["defined", "requireDefined"], ["specified", "requireSpecified"]], function (e) { var g = e[1] || e[0]; a[e[0]] = b ? aa(b[g], c) : function () { var a = t[O]; return a[g].apply(a, arguments) } }) } function H(a, b, c, d) { b = Error(b + "\nhttp://requirejs.org/docs/errors.html#" + a); b.requireType = a; b.requireModules = d; if (c) b.originalError = c; return b } function ha() { if (I && I.readyState === "interactive") return I; N(document.getElementsByTagName("script"), function (a) { if (a.readyState === "interactive") return I = a }); return I } var ia = /(\/\*([\s\S]*?)\*\/|([^:]|^)\/\/(.*)$)/mg, ja = /require\s*\(\s*["']([^'"\s]+)["']\s*\)/g, ca = /\.js$/, ka = /^\.\//, J = Object.prototype.toString, y = Array.prototype, ga = y.slice, la = y.splice, u = !!(typeof window !== "undefined" && navigator && document), da = !u && typeof importScripts !== "undefined", ma = u && navigator.platform === "PLAYSTATION 3" ? /^complete$/ : /^(complete|loaded)$/, O = "_", S = typeof opera !== "undefined" && opera.toString() === "[object Opera]", t = {}, p = {}, P = [], L = !1, k, v, C, z, D, I, E, ea, fa; if (typeof define === "undefined") { if (typeof requirejs !== "undefined") { if (w(requirejs)) return; p = requirejs; requirejs = void 0 } typeof require !== "undefined" && !w(require) && (p = require, require = void 0); k = requirejs = function (a, b, c, d) { var e = O, f; !G(a) && typeof a !== "string" && (f = a, G(b) ? (a = b, b = c, c = d) : a = []); if (f && f.context) e = f.context; (d = t[e]) || (d = t[e] = k.s.newContext(e)); f && d.configure(f); return d.require(a, b, c) }; k.config = function (a) { return k(a) }; require || (require = k); k.version = "2.0.2"; k.jsExtRegExp = /^\/|:|\?|\.js$/; k.isBrowser = u; y = k.s = { contexts: t, newContext: function (a) { function b(a, b, c) { var d = b && b.split("/"), e = v.map, f = e && e["*"], g, h, i, j; if (a && a.charAt(0) === ".") if (b) { d = v.pkgs[b] ? [b] : d.slice(0, d.length - 1); b = a = d.concat(a.split("/")); for (g = 0; b[g]; g += 1) if (h = b[g], h === ".") b.splice(g, 1), g -= 1; else if (h === "..") if (g === 1 && (b[2] === ".." || b[0] === "..")) break; else g > 0 && (b.splice(g - 1, 2), g -= 2); g = v.pkgs[b = a[0]]; a = a.join("/"); g && a === b + "/" + g.main && (a = b) } else a.indexOf("./") === 0 && (a = a.substring(2)); if (c && (d || f) && e) { b = a.split("/"); for (g = b.length; g > 0; g -= 1) { i = b.slice(0, g).join("/"); if (d) for (h = d.length; h > 0; h -= 1) if (c = e[d.slice(0, h).join("/")]) if (c = c[i]) { j = c; break } !j && f && f[i] && (j = f[i]); if (j) { b.splice(0, g, j); a = b.join("/"); break } } } return a } function c(a) { u && q(document.getElementsByTagName("script"), function (b) { if (b.getAttribute("data-requiremodule") === a && b.getAttribute("data-requirecontext") === N.contextName) return b.parentNode.removeChild(b), !0 }) } function d(a) { var b = v.paths[a]; if (b && G(b) && b.length > 1) return c(a), b.shift(), N.undef(a), N.require([a]), !0 } function e(a, c, d, e) { var f = a ? a.indexOf("!") : -1, g = null, h = c ? c.name : null, i = a, j = !0, k = "", l, m; a || (j = !1, a = "_@r" + (D += 1)); f !== -1 && (g = a.substring(0, f), a = a.substring(f + 1, a.length)); g && (g = b(g, h, e), m = B[g]); a && (g ? k = m && m.normalize ? m.normalize(a, function (a) { return b(a, h, e) }) : b(a, h, e) : (k = b(a, h, e), l = N.nameToUrl(a, null, c))); a = g && !m && !d ? "_unnormalized" + (E += 1) : ""; return { prefix: g, name: k, parentMap: c, unnormalized: !!a, url: l, originalName: i, isDefine: j, id: (g ? g + "!" + k : k) + a} } function f(a) { var b = a.id, c = y[b]; c || (c = y[b] = new N.Module(a)); return c } function g(a, b, c) { var d = a.id, e = y[d]; if (B.hasOwnProperty(d) && (!e || e.defineEmitComplete)) b === "defined" && c(B[d]); else f(a).on(b, c) } function h(a, b) { var c = a.requireModules, d = !1; if (b) b(a); else if (q(c, function (b) { if (b = y[b]) b.error = a, b.events.error && (d = !0, b.emit("error", a)) }), !d) k.onError(a) } function i() { P.length && (la.apply(A, [A.length - 1, 0].concat(P)), P = []) } function j(a, b, c) { a = a && a.map; b = aa(c || N.require, a, b); ba(b, N, a); b.isBrowser = u; return b } function l(a) { delete y[a]; q(F, function (b, c) { if (b.map.id === a) return F.splice(c, 1), b.defined || (N.waitCount -= 1), !0 }) } function m(a, b) { var c = a.map.id, d = a.depMaps, e; if (a.inited) { if (b[c]) return a; b[c] = !0; q(d, function (a) { if (a = y[a.id]) return !a.inited || !a.enabled ? (e = null, delete b[c], !0) : e = m(a, K({}, b)) }); return e } } function n(a, b, c) { var d = a.map.id, e = a.depMaps; if (a.inited && a.map.isDefine) { if (b[d]) return B[d]; b[d] = a; q(e, function (e) { var e = e.id, f = y[e]; !O[e] && f && (!f.inited || !f.enabled ? c[d] = !0 : (f = n(f, b, c), c[e] || a.defineDepById(e, f))) }); a.check(!0); return B[d] } } function o(a) { a.check() } function p() { var a = v.waitSeconds * 1e3, b = a && N.startTime + a < (new Date).getTime(), e = [], f = !1, g = !0, i, j, k; if (!J) { J = !0; x(y, function (a) { i = a.map; j = i.id; if (a.enabled && !a.error) if (!a.inited && b) d(j) ? f = k = !0 : (e.push(j), c(j)); else if (!a.inited && a.fetched && i.isDefine && (f = !0, !i.prefix)) return g = !1 }); if (b && e.length) return a = H("timeout", "Load timeout for modules: " + e, null, e), a.contextName = N.contextName, h(a); g && (q(F, function (a) { if (!a.defined) { var a = m(a, {}), b = {}; a && (n(a, b, {}), x(b, o)) } }), x(y, o)); if ((!b || k) && f) if ((u || da) && !Q) Q = setTimeout(function () { Q = 0; p() }, 50); J = !1 } } function r(a) { f(e(a[0], null, !0)).init(a[1], a[2]) } function t(a) { var a = a.currentTarget || a.srcElement, b = N.onScriptLoad; a.detachEvent && !S ? a.detachEvent("onreadystatechange", b) : a.removeEventListener("load", b, !1); b = N.onScriptError; a.detachEvent && !S || a.removeEventListener("error", b, !1); return { node: a, id: a && a.getAttribute("data-requiremodule")} } var v = { waitSeconds: 7, baseUrl: "./", paths: {}, pkgs: {}, shim: {} }, y = {}, z = {}, A = [], B = {}, C = {}, D = 1, E = 1, F = [], J, M, N, O, Q; O = { require: function (a) { return j(a) }, exports: function (a) { a.usingExports = !0; if (a.map.isDefine) return a.exports = B[a.map.id] = {} }, module: function (a) { return a.module = { id: a.map.id, uri: a.map.url, config: function () { return v.config && v.config[a.map.id] || {} }, exports: B[a.map.id]} } }; M = function (a) { this.events = z[a.id] || {}; this.map = a; this.shim = v.shim[a.id]; this.depExports = []; this.depMaps = []; this.depMatched = []; this.pluginMaps = {}; this.depCount = 0 }; M.prototype = { init: function (a, b, c, d) { d = d || {}; if (!this.inited) { this.factory = b; if (c) this.on("error", c); else this.events.error && (c = s(this, function (a) { this.emit("error", a) })); this.depMaps = a && a.slice(0); this.depMaps.rjsSkipMap = a.rjsSkipMap; this.errback = c; this.inited = !0; this.ignore = d.ignore; d.enabled || this.enabled ? this.enable() : this.check() } }, defineDepById: function (a, b) { var c; q(this.depMaps, function (b, d) { if (b.id === a) return c = d, !0 }); return this.defineDep(c, b) }, defineDep: function (a, b) { this.depMatched[a] || (this.depMatched[a] = !0, this.depCount -= 1, this.depExports[a] = b) }, fetch: function () { if (!this.fetched) { this.fetched = !0; N.startTime = (new Date).getTime(); var a = this.map; if (this.shim) j(this, !0)(this.shim.deps || [], s(this, function () { return a.prefix ? this.callPlugin() : this.load() })); else return a.prefix ? this.callPlugin() : this.load() } }, load: function () { var a = this.map.url; C[a] || (C[a] = !0, N.load(this.map.id, a)) }, check: function (a) { if (this.enabled && !this.enabling) { var b = this.map.id, c = this.depExports, d = this.exports, e = this.factory, f; if (this.inited) if (this.error) this.emit("error", this.error); else { if (!this.defining) { this.defining = !0; if (this.depCount < 1 && !this.defined) { if (w(e)) { if (this.events.error) try { d = N.execCb(b, e, c, d) } catch (g) { f = g } else d = N.execCb(b, e, c, d); if (this.map.isDefine) if ((c = this.module) && c.exports !== void 0 && c.exports !== this.exports) d = c.exports; else if (d === void 0 && this.usingExports) d = this.exports; if (f) return f.requireMap = this.map, f.requireModules = [this.map.id], f.requireType = "define", h(this.error = f) } else d = e; this.exports = d; if (this.map.isDefine && !this.ignore && (B[b] = d, k.onResourceLoad)) k.onResourceLoad(N, this.map, this.depMaps); delete y[b]; this.defined = !0; N.waitCount -= 1; N.waitCount === 0 && (F = []) } this.defining = !1; if (!a && this.defined && !this.defineEmitted) this.defineEmitted = !0, this.emit("defined", this.exports), this.defineEmitComplete = !0 } } else this.fetch() } }, callPlugin: function () { var a = this.map, c = a.id, d = e(a.prefix, null, !1, !0); g(d, "defined", s(this, function (d) { var i = this.map.name, m = this.map.parentMap ? this.map.parentMap.name : null; if (this.map.unnormalized) { if (d.normalize && (i = d.normalize(i, function (a) { return b(a, m, !0) }) || ""), d = e(a.prefix + "!" + i, this.map.parentMap, !1, !0), g(d, "defined", s(this, function (a) { this.init([], function () { return a }, null, { enabled: !0, ignore: !0 }) })), d = y[d.id]) { if (this.events.error) d.on("error", s(this, function (a) { this.emit("error", a) })); d.enable() } } else i = s(this, function (a) { this.init([], function () { return a }, null, { enabled: !0 }) }), i.error = s(this, function (a) { this.inited = !0; this.error = a; a.requireModules = [c]; x(y, function (a) { a.map.id.indexOf(c + "_unnormalized") === 0 && l(a.map.id) }); h(a) }), i.fromText = function (a, b) { var c = L; c && (L = !1); f(e(a)); k.exec(b); c && (L = !0); N.completeLoad(a) }, d.load(a.name, j(a.parentMap, !0, function (a, b) { a.rjsSkipMap = !0; return N.require(a, b) }), i, v) })); N.enable(d, this); this.pluginMaps[d.id] = d }, enable: function () { this.enabled = !0; if (!this.waitPushed) F.push(this), N.waitCount += 1, this.waitPushed = !0; this.enabling = !0; q(this.depMaps, s(this, function (a, b) { var c, d; if (typeof a === "string") { a = e(a, this.map.isDefine ? this.map : this.map.parentMap, !1, !this.depMaps.rjsSkipMap); this.depMaps[b] = a; if (c = O[a.id]) { this.depExports[b] = c(this); return } this.depCount += 1; g(a, "defined", s(this, function (a) { this.defineDep(b, a); this.check() })); this.errback && g(a, "error", this.errback) } c = a.id; d = y[c]; !O[c] && d && !d.enabled && N.enable(a, this) })); x(this.pluginMaps, s(this, function (a) { var b = y[a.id]; b && !b.enabled && N.enable(a, this) })); this.enabling = !1; this.check() }, on: function (a, b) { var c = this.events[a]; c || (c = this.events[a] = []); c.push(b) }, emit: function (a, b) { q(this.events[a], function (a) { a(b) }); a === "error" && delete this.events[a] } }; return N = { config: v, contextName: a, registry: y, defined: B, urlFetched: C, waitCount: 0, defQueue: A, Module: M, makeModuleMap: e, configure: function (a) { a.baseUrl && a.baseUrl.charAt(a.baseUrl.length - 1) !== "/" && (a.baseUrl += "/"); var b = v.pkgs, c = v.shim, d = v.paths, f = v.map; K(v, a, !0); v.paths = K(d, a.paths, !0); if (a.map) v.map = K(f || {}, a.map, !0, !0); if (a.shim) x(a.shim, function (a, b) { G(a) && (a = { deps: a }); if (a.exports && !a.exports.__buildReady) a.exports = N.makeShimExports(a.exports); c[b] = a }), v.shim = c; if (a.packages) q(a.packages, function (a) { a = typeof a === "string" ? { name: a} : a; b[a.name] = { name: a.name, location: a.location || a.name, main: (a.main || "main").replace(ka, "").replace(ca, "")} }), v.pkgs = b; x(y, function (a, b) { a.map = e(b) }); if (a.deps || a.callback) N.require(a.deps || [], a.callback) }, makeShimExports: function (a) { var b; return typeof a === "string" ? (b = function () { return $(a) }, b.exports = a, b) : function () { return a.apply(Z, arguments) } }, requireDefined: function (a, b) { var c = e(a, b, !1, !0).id; return B.hasOwnProperty(c) }, requireSpecified: function (a, b) { a = e(a, b, !1, !0).id; return B.hasOwnProperty(a) || y.hasOwnProperty(a) }, require: function (b, c, d, g) { var j; if (typeof b === "string") { if (w(c)) return h(H("requireargs", "Invalid require call"), d); if (k.get) return k.get(N, b, c); b = e(b, c, !1, !0); b = b.id; return !B.hasOwnProperty(b) ? h(H("notloaded", 'Module name "' + b + '" has not been loaded yet for context: ' + a)) : B[b] } d && !w(d) && (g = d, d = void 0); c && !w(c) && (g = c, c = void 0); for (i(); A.length; ) if (j = A.shift(), j[0] === null) return h(H("mismatch", "Mismatched anonymous define() module: " + j[j.length - 1])); else r(j); f(e(null, g)).init(b, c, d, { enabled: !0 }); p(); return N.require }, undef: function (a) { var b = e(a, null, !0), c = y[a]; delete B[a]; delete C[b.url]; delete z[a]; if (c) { if (c.events.defined) z[a] = c.events; l(a) } }, enable: function (a) { y[a.id] && f(a).enable() }, completeLoad: function (a) { var b = v.shim[a] || {}, c = b.exports && b.exports.exports, e, f; for (i(); A.length; ) { f = A.shift(); if (f[0] === null) { f[0] = a; if (e) break; e = !0 } else f[0] === a && (e = !0); r(f) } f = y[a]; if (!e && !B[a] && f && !f.inited) if (v.enforceDefine && (!c || !$(c))) if (d(a)) return; else return h(H("nodefine", "No define call for " + a, null, [a])); else r([a, b.deps || [], b.exports]); p() }, toUrl: function (a, b) { var c = a.lastIndexOf("."), d = null; c !== -1 && (d = a.substring(c, a.length), a = a.substring(0, c)); return N.nameToUrl(a, d, b) }, nameToUrl: function (a, c, d) { var e, f, g, h, i, a = b(a, d && d.id, !0); if (k.jsExtRegExp.test(a)) c = a + (c || ""); else { e = v.paths; f = v.pkgs; d = a.split("/"); for (h = d.length; h > 0; h -= 1) if (i = d.slice(0, h).join("/"), g = f[i], i = e[i]) { G(i) && (i = i[0]); d.splice(0, h, i); break } else if (g) { a = a === g.name ? g.location + "/" + g.main : g.location; d.splice(0, h, a); break } c = d.join("/") + (c || ".js"); c = (c.charAt(0) === "/" || c.match(/^[\w\+\.\-]+:/) ? "" : v.baseUrl) + c } return v.urlArgs ? c + ((c.indexOf("?") === -1 ? "?" : "&") + v.urlArgs) : c }, load: function (a, b) { k.load(N, a, b) }, execCb: function (a, b, c, d) { return b.apply(d, c) }, onScriptLoad: function (a) { if (a.type === "load" || ma.test((a.currentTarget || a.srcElement).readyState)) I = null, a = t(a), N.completeLoad(a.id) }, onScriptError: function (a) { var b = t(a); if (!d(b.id)) return h(H("scripterror", "Script error", a, [b.id])) } } } }; k({}); ba(k); if (u && (v = y.head = document.getElementsByTagName("head")[0], C = document.getElementsByTagName("base")[0])) v = y.head = C.parentNode; k.onError = function (a) { throw a }; k.load = function (a, b, c) { var d = a && a.config || {}, e; if (u) return e = d.xhtml ? document.createElementNS("http://www.w3.org/1999/xhtml", "html:script") : document.createElement("script"), e.type = d.scriptType || "text/javascript", e.charset = "utf-8", e.setAttribute("data-requirecontext", a.contextName), e.setAttribute("data-requiremodule", b), e.attachEvent && !(e.attachEvent.toString && e.attachEvent.toString().indexOf("[native code") < 0) && !S ? (L = !0, e.attachEvent("onreadystatechange", a.onScriptLoad)) : (e.addEventListener("load", a.onScriptLoad, !1), e.addEventListener("error", a.onScriptError, !1)), e.src = c, E = e, C ? v.insertBefore(e, C) : v.appendChild(e), E = null, e; else da && (importScripts(c), a.completeLoad(b)) }; u && N(document.getElementsByTagName("script"), function (a) { if (!v) v = a.parentNode; if (z = a.getAttribute("data-main")) { D = z.split("/"); ea = D.pop(); fa = D.length ? D.join("/") + "/" : "./"; if (!p.baseUrl) p.baseUrl = fa; z = ea.replace(ca, ""); p.deps = p.deps ? p.deps.concat(z) : [z]; return !0 } }); define = function (a, b, c) { var d, e; typeof a !== "string" && (c = b, b = a, a = null); G(b) || (c = b, b = []); !b.length && w(c) && c.length && (c.toString().replace(ia, "").replace(ja, function (a, c) { b.push(c) }), b = (c.length === 1 ? ["require"] : ["require", "exports", "module"]).concat(b)); if (L && (d = E || ha())) a || (a = d.getAttribute("data-requiremodule")), e = t[d.getAttribute("data-requirecontext")]; (e ? e.defQueue : P).push([a, b, c]) }; define.amd = { jQuery: !0 }; k.exec = function (b) { return eval(b) }; k(p) } })(this)
