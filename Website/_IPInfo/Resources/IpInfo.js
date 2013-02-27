function getGeoFromIP(ip) {
    script = document.createElement('script')
    script.src = _page.ipservice + "?ip=" + ip + "&output=json&callback=fillGeoIP";
    document.body.appendChild(script)
}

function fillGeoIP(geoDict) {
    countryCode = geoDict['CountryCode'];
    countryName = geoDict['CountryName'];
    cityIP = geoDict['City'];
    flagPath = geoDict['FlagPath'];
    document.getElementById('countryname').innerHTML = countryName;
    document.getElementById('city').innerHTML = cityIP;
    document.getElementById('flag').innerHTML = "<img src='" + flagPath + "' onerror='this.style.display=\"none\"' />";
}

function initGMap() {
    if (GBrowserIsCompatible()) {
        var map = new GMap2(document.getElementById("map_canvas"));
        map.setCenter(latlng, 5);
        map.addControl(new GSmallMapControl());
        map.addControl(new GMapTypeControl());
        map.addOverlay(new GMarker(latlng));
    }
}
