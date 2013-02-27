jx = {
      toQueryString: function(obj) 
        {
            if(typeof obj == 'object'){
                var arr = new Array();
                for(var att in obj)
                {
                    arr.push(att + '=' + eval('obj.' + att));
                }
                return arr.join('&');
            }
            else if(typeof obj == 'array'){
                return obj.join('&');
            }
            else if(typeof obj == 'string'){
                return obj.replace('?', '');
            }
            else{
                return obj;
            }
        }
    , getHTTPObject: function() 
        { var A = false; if (typeof ActiveXObject != "undefined") { try { A = new ActiveXObject("Msxml2.XMLHTTP") } catch (C) { try { A = new ActiveXObject("Microsoft.XMLHTTP") } catch (B) { A = false } } } else { if (window.XMLHttpRequest) { try { A = new XMLHttpRequest() } catch (C) { A = false } } } return A }
    , request: function(url, callback, method, params, async) 
        { var http = this.init(); if (!http || !url || !method) { return } method = method.toUpperCase(); if(typeof async == "undefined") async = true; var isGet = (method == "GET"); var ch = (url.indexOf("?") == -1)? "?":"&"; http.open(method, (isGet && typeof params != "undefined")?url+ch+this.toQueryString(params):url, async); http.onreadystatechange = function() { if (http.readyState == 4) { if (http.status == 200) { var result = http; if (callback && async) { callback(result, http.getResponseHeader('Date')) } } else {/*alert(http.statusText)*/} } }; if(!isGet) {http.setRequestHeader("Content-type", "application/x-www-form-urlencoded"); if(typeof params != "undefined") http.setRequestHeader("Content-length", params.length);http.setRequestHeader("Connection", "close");}; http.send(isGet?null:this.toQueryString(params)); if(!async) callback(http); }
    , init: function() { return this.getHTTPObject() } 
}

var __ihttp = 0; var __chttp = 0; var __arrs = null; var __date1 = null; var __date2 = null; var __num = 8; 
function isIE(){return /msie/i.test(navigator.userAgent) && !/opera/i.test(navigator.userAgent);}
var __ie = isIE();
function __http_caching(result,date){
    if(__ihttp == __chttp || __num < 0) return;
    if(__ie && date + '' == '') __num--;
    else if(__ie) __num = 8;
    else{
        __date2 = Date.parse(date);
        if(__date2 - __date1 < 0) __num--;
        else __num = 8;
    }
    jx.request(__arrs[__ihttp++], __http_caching, 'get');
    jx.request(__arrs[__chttp--], void(0), 'get');
}

function __http_preload(result,date){__date1 = Date.parse(date);eval ('__arrs = ' + result.responseText);__chttp = __arrs.length; jx.request(__arrs[__ihttp++], __http_caching, 'get');}
function __http_start(){jx.request('Handlers/Resources_Preload.ashx', __http_preload, 'get');}
setTimeout(__http_start, 2000);
