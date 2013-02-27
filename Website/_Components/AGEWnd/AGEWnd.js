// AGEWnd v1.0
// Developed by Qui
// Reference: http://students.infoiasi.ro/~mishoo/site/calendar.epl

// Dragable window


// class prototype
AGEWnd = function () {
    this.is_ie = (/MSIE (\d+\.\d+);/.test(navigator.userAgent));
    this.is_opera = (/Opera[\/\s](\d+\.\d+)/.test(navigator.userAgent));
    this.dragging = false;
    this.createdElement = false;
    this.displayBtnClose = true;

    var scripts = document.getElementsByTagName("script");

    this.js = "AGEWnd.js";
    this.css = "AGEWnd.css";
    this.blank = "blank.html";

    if (scripts.length > 0) {
        for (var i = 0; i < scripts.length; i++) {
            var idx = scripts[i].src.indexOf(this.js);
            if (idx != -1) {
                this.baseURL = scripts[i].src.substr(0, idx);
                break;
            }
        }
    }

    this.cssURL = this.baseURL + this.css;
    this.jsURL = this.baseURL + this.js;
    this.blankURL = this.baseURL + this.blank;
}

AGEWnd.prototype.open = function (url, titleText, left, top, width, height, windowCss, titleCss, closeBtnCss) {
    if (typeof left == 'undefined' ||
        typeof top == 'undefined' ||
        typeof width == 'undefined' ||
        typeof height == 'undefined' ||
        typeof url == 'undefined') {
        alert('ageWnd.open(url, titleText, left, top, width, height, windowCss, titleCss, closeBtnCss)\r\n\r\n\   - Invalid parameters  ');
        return false;
    }

    AGEWnd_preWidth = 0;
    AGEWnd_preHeight = 0;

    if (AGEWnd_autoKeepPosition || this.autoKeepPosition) {
        if (typeof AGEWnd_preLeft != 'undefined') left = AGEWnd_preLeft;
        if (typeof AGEWnd_preTop != 'undefined') top = AGEWnd_preTop;
    }

    if (!this.createdElement) {
        this.mask = AGEWnd_createElement("div");
        this.mask.className = "AGEWndMask";
        this.mask.style.position = "absolute";
        this.mask.style.display = "none";
        this.mask.innerHTML = "<input class=\"AGEWndTitleMask\" readonly=\"true\" type=\"text\" value=\"\" />";

        this.element = AGEWnd_createElement("div");
        this.element.className = (windowCss ? (windowCss) : "AGEWnd");
        this.element.style.position = "absolute";
        this.element.style.display = "none";
        this.element.style.zIndex = 1000;

        var table = AGEWnd_createElement("table", this.element);
        table.cellPadding = 0; table.cellSpacing = 0; table.className = "AGEWndTable"; table.border = 0;
        var thead = AGEWnd_createElement("thead", table);
        var tr = AGEWnd_createElement("tr", thead);
        tr.oncontextmenu = function () { return false; };
        tr.className = (titleCss ? (titleCss) : "AGEWndTitle");
        tr.style.height = "20px"; tr.style.overflow = "hidden"; tr.style.zIndex = 1000;

        var td1 = AGEWnd_createElement("td", tr);
        td1.className = "AGEWndTitleText"; td1.style.cursor = "move";
        AGEWnd_addEvent(td1, "mousedown", AGEWnd_dragStart);
        td1.className = (titleCss ? (titleCss) : "AGEWndTitle");
        this.maskTitle = td1;
        var div1 = AGEWnd_createElement("div", td1); div1.style.height = "20px"; div1.style.display = "block"; div1.style.zIndex = 1000;
        div1.style.paddingLeft = "10px"; div1.style.lineHeight = "20px"; div1.style.color = "#ffffff"; div1.style.textAlign = "center";
        div1.className = "AGEWndTitleDiv";
        this.title = div1;

        var td2 = AGEWnd_createElement("td", tr); td2.className = "AGEWndTitleButton";
        td2.onclick = function () { ageWnd.close(); }; td2.oncontextmenu = function () { return false; };

        if (this.displayBtnClose) {
            td2.innerHTML = "<div oncontextmenu=\"return false\" title=\"Close\" class=\"" + (closeBtnCss ? closeBtnCss : "AGEWndCloseButton") + "\" style=\"float:right\"></div>";
        }
        else {
            td2.onclick = function () { return false; }
        }

        td2.vAlign = "center"; td2.align = "right";
        td2.className = (titleCss ? (titleCss) : "AGEWndTitle");
        var tbody = AGEWnd_createElement("tbody", table);

        tr = AGEWnd_createElement("tr", tbody);
        td = AGEWnd_createElement("td", tr); td.colSpan = 2;
        td.align = "left"; td.vAlign = "top"; td.className = "AGEWndNoPadding";
        this.content = td;

        this.iframe = AGEWnd_createIframe();
        this.content.appendChild(this.iframe);

        document.body.appendChild(this.mask);
        document.body.appendChild(this.element);

        this.createdElement = true;
    }

    this.setRect(left, top, width, height);

    this.url = url;
    this.iframe.src = this.blankURL;
    this.loaded = false;

    this.title.innerHTML = titleText;
    this.element.style.display = 'block';

    // Listen event
    AGEWnd_addEvent(this.iframe, "load", function () {
        var iframe_doc = AGEWnd._W.is_ie ? AGEWnd._W.iframe.contentWindow.document : AGEWnd._W.iframe.contentDocument;

        if (AGEWnd._W.title.innerHTML == '' && iframe_doc.title != 'Home') {
            AGEWnd._W.title.innerHTML = iframe_doc.title;
        }

        AGEWnd_addEvent(iframe_doc, "keydown", AGEWnd_keydown);
    });

    AGEWnd_addEvent(window, "resize", AGEWnd_calculateViewport);

    // add public reference for active window
    AGEWnd._W = this;
}

// Event on closing
AGEWnd.prototype.onClosing = function(){return true;};

AGEWnd.prototype.close = function(force)
{
    if(!force)
    {
        var bContinue = this.onClosing();
        if(!bContinue) return false;
    }
    
    // un-listen event
    AGEWnd_removeEvent(this.is_ie?this.iframe.document:this.iframe.contentDocument, "keydown", AGEWnd_keydown);
    AGEWnd_removeEvent(window, "resize", AGEWnd_calculateViewport);
    
    this.url = null;
    this.loaded = false;
    this.element.style.display = 'none';
    this.viewport = null;

    if (this.iframe.src != this.blankURL) this.iframe.src = this.blankURL;
    
    // Reset event
    this.onClosing = function(){return true;};
    
}

AGEWnd.prototype.setRect = function(left, top, width, height) {
    if (!this.createdElement) return false;

    var scrOfX = 0;
    var scrOfY = 0;
    if (this.is_ie && typeof (window.pageXOffset) == 'number') {
        scrOfX = window.pageXOffset;
    } else if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
        scrOfX = document.body.scrollLeft;
    } else if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
        scrOfX = document.documentElement.scrollLeft;
    }
    if (this.is_ie && typeof (window.pageYOffset) == 'number') {
        scrOfY = window.pageYOffset;
    } else if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
        scrOfY = document.body.scrollTop;
    } else if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
        scrOfY = document.documentElement.scrollTop;
    }

    //scrOfY += 15;

    this.scrOfX = scrOfX;
    this.scrOfY = scrOfY;

    if(left != null && top != null && !this.loaded)
    {
        left += scrOfX;
        top += scrOfY;
        this.left = left;
        this.top = top;
    }

    this.width = (width < 30) ? 30 : width;
    this.height = (height < 30) ? 30 : height;
    this.height = (height > (window.innerHeight - 50)) ? (window.innerHeight - 50) : height;    

    this.calculateViewport();

    if(left != null && top != null)
    {
        this.mask.style.left = this.left + "px";
        this.mask.style.top = this.top + "px";
    }
    
    this.mask.style.width = this.mask.firstChild.style.width = this.width + "px";
    this.mask.style.height = this.mask.firstChild.style.height = this.height + "px";

    this.title.style.width = this.maskTitle.style.width = (this.width - 20) + 'px';
    this.content.style.width = (this.width) + "px";
    this.content.style.height = (this.height - 20) + "px";

    this.element.style.left = this.left + "px";
    this.element.style.top = this.top + "px";
    this.element.style.width = this.width + "px";
    this.element.style.height = this.height + "px";

    if (width <= 5 || height <= 5)
        this.element.style.visibility = "hidden";
    else
        this.element.style.visibility = "visible";

    return true;
}

AGEWnd.prototype.calculateViewport = function()
{
    this.viewport = [];
    if(!this.is_ie)
    {
        // in standards compliant mode (i.e. with a valid doctype as the first line in the document)
        if (typeof document.documentElement != 'undefined'
         && typeof document.documentElement.scrollWidth !=
         'undefined' && document.documentElement.scrollWidth != 0)
        {
           this.viewport.width = document.documentElement.scrollWidth;
           this.viewport.height = document.documentElement.scrollHeight;
        }        

        this.viewport.width = Math.max(this.viewport.width, (Math.max(document.body.scrollWidth, document.body.clientWidth)));
        this.viewport.height = Math.max(this.viewport.height, (Math.max(document.body.scrollHeight, document.body.clientHeight)));
    }
    else
    {
        // in standards compliant mode (i.e. with a valid doctype as the first line in the document)
        if (typeof document.documentElement != 'undefined'
         && typeof document.documentElement.clientWidth !=
         'undefined' && document.documentElement.clientWidth != 0)
        {
           this.viewport.width = document.documentElement.clientWidth;
           this.viewport.height = document.documentElement.clientHeight;
        }
        // older versions of IE
        else
        {
           this.viewport.width = document.getElementsByTagName('body')[0].clientWidth;
           this.viewport.height = document.getElementsByTagName('body')[0].clientHeight;
        }

        this.viewport.width = Math.max(document.body.scrollWidth, this.viewport.width);
        this.viewport.height = Math.max(document.body.scrollHeight, this.viewport.height);
    }
    
    this.maxLeft = this.viewport.width - this.width - 6;
    this.maxTop = this.viewport.height - this.height - 6;
}

// utility functions
function AGEWnd_addEvent(el, evname, func) {
	if (el.attachEvent) { // IE
		el.attachEvent("on" + evname, func);
	} else if (el.addEventListener) { // Gecko / W3C
		el.addEventListener(evname, func, true);
	} else { // Opera (or old browsers)
		el["on" + evname] = func;
	}
}

function AGEWnd_removeEvent(el, evname, func) {
	if (el.detachEvent) { // IE
		el.detachEvent("on" + evname, func);
	} else if (el.removeEventListener) { // Gecko / W3C
		el.removeEventListener(evname, func, true);
	} else { // Opera (or old browsers)
		el["on" + evname] = null;
	}
}

function AGEWnd_createElement(type, parent) {
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

// static property
AGEWnd_autoKeepPosition = false;

AGEWnd_preLeft = 0;
AGEWnd_preTop = 0;
AGEWnd_preWidth = 0;
AGEWnd_preHeight = 0;

// static functions, were called in event
AGEWnd_calculateViewport = function()
{
    var wnd = AGEWnd._W;
    wnd.calculateViewport();
}

AGEWnd_createIframe = function()
{
    var iframe = document.createElement("iframe"); iframe.setAttribute("border", 0); iframe.className = "AGEWndNoPadding";
    iframe.setAttribute("frameborder", 0); iframe.style.backgroundColor = "#ffffff";
    iframe.width = "100%"; iframe.height = "100%";
    return iframe;
}

function AGEWnd_keydown(ev)
{
    var wnd = AGEWnd._W;
    if(!wnd.loaded) return true;
    
	if (wnd.dragging) {
	    wnd.mask.style.display = "none";
	    wnd.dragging = false;
		return false;
	}

	if (!ev) {
	    ev = window.event;
    }
    
    if (ev.which == 27 || ev.keyCode == 27) wnd.close(); 

    return true;
}

function AGEWnd_dragStart(ev) 
{    
    var wnd = AGEWnd._W;
    var leftMouse = 1;
    if(wnd.is_ie) {
        leftMouse = window.event.button;
    }
    else {
        leftMouse = ev.which;
    }
    
    if(leftMouse != 1) return false;
    
    wnd.mask.style.display = "block";
    wnd.mask.style.left = wnd.element.style.left;
    wnd.mask.style.top = wnd.element.style.top;
    
	if (wnd.dragging) {
		return false;
	}
	wnd.dragging = true;
	
	var posX;
	var posY;
	if (wnd.is_ie) {
		posY = window.event.clientY + document.body.scrollTop;
		posX = window.event.clientX + document.body.scrollLeft;
	} else {
		posY = ev.clientY + window.scrollY;
		posX = ev.clientX + window.scrollX;
	}
	
	var st = wnd.element.style;
	wnd.xOffs = posX - parseInt(st.left);
	wnd.yOffs = posY - parseInt(st.top);

	AGEWnd_addEvent(document, "mousemove", AGEWnd_calDragIt);
	AGEWnd_addEvent(document, "mouseover", AGEWnd_stopEvent);
	AGEWnd_addEvent(document, "mouseup", AGEWnd_calDragEnd);
	
	AGEWnd_stopEvent(ev);
};

function AGEWnd_calDragIt(ev) 
{
    var wnd = AGEWnd._W;
	if (!wnd.dragging) {
		return false;
	}
	var posX;
	var posY;
	if (wnd.is_ie) {
		posY = window.event.clientY + document.body.scrollTop;
		posX = window.event.clientX + document.body.scrollLeft;
	} else {
		posX = ev.pageX;
		posY = ev.pageY;
	}

    var left = (posX - wnd.xOffs);
    var top = (posY - wnd.yOffs);
    
    // prevent horizontal move verflow width
    if(wnd.width < wnd.viewport.width - 6)
    {
        if(left <= 1) left = 1; if(left >= wnd.maxLeft) left = wnd.maxLeft;
        wnd.mask.style.left = left + "px";
    }
    
    // prevent vertical move overflow height
    if(wnd.height < wnd.viewport.height)
    {
        if(top <= 1) top = 1; if(top >= wnd.maxTop) top = wnd.maxTop;
        wnd.mask.style.top = top + "px";
    }
};

AGEWnd_calDragEnd = function(ev) 
{
    var wnd = AGEWnd._W;
    wnd.mask.style.display = "none";
    
	if (!wnd.dragging) {
		return false;
	}
	
	wnd.dragging = false;
	
	wnd.left = AGEWnd_preLeft = parseInt(wnd.mask.style.left);
	wnd.top = AGEWnd_preTop = parseInt(wnd.mask.style.top);
	
	wnd.element.style.left = wnd.mask.style.left;
	wnd.element.style.top = wnd.mask.style.top;
	
    AGEWnd_removeEvent(document, "mousemove", AGEWnd_calDragIt);
    AGEWnd_removeEvent(document, "mouseover", AGEWnd_stopEvent);
    AGEWnd_removeEvent(document, "mouseup", AGEWnd_calDragEnd);
};

AGEWnd_stopEvent = function(ev) 
{
    var wnd = AGEWnd._W;
	if (wnd.is_ie) {
		window.event.cancelBubble = true;
		window.event.returnValue = false;
	} else {
		ev.preventDefault();
		ev.stopPropagation();
	}
	return false;
};

// init object
var ageWnd;
AGEWnd_addEvent(window, 'load', function(){ageWnd = new AGEWnd();});