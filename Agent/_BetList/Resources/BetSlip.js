
function ViewResult(matchid, type, race, bettype) {
    var width = 750;
    var height = 250;
    var league = GetParameterValue("league", location.href);

    var params = '?matchid=' + matchid + '&ddlSport=' + type + '&race=' + race + '&bettype=' + bettype + '&ddlLeague=' + league;
    if (ageWnd.loaded) {
        ageWnd.close();
    }
    
    if (parent.ageWnd) {
        parent.ageWnd.open(age.GetBaseUrl() + '_Reports/Result/Result.aspx' + params, '', 50, 100, width, height);
    }
    else {
        ageWnd.open(age.GetBaseUrl() + '_Reports/Result/Result.aspx' + params, '', 50, 100, width, height);
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