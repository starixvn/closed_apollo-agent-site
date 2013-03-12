jQuery(document).ready(function () {

    jQuery("#treeview-link").click(function () {
        jQuery(".downline-title").toggle();
        jQuery(".agent-treeview").toggle();
    });

//    jQuery("#fileSystemInfos").treeview({
//        control: "#fileSystemInfosControl",
//        persist: "cookie",
//        cookieId: "treeview-black"
//    });

//    jQuery('#fileSystemInfos').treeview({
//        url: '<% = ResolveClientUrl("~/AccountInfo.aspx/FileSystemInfos") %>',
//       // url: '../AccountInfo.aspx/FileSystemInfos',
//                control: '#fileSystemInfosControl',
//                ajax: {
//                    contentType: 'application/json; charset=utf-8',
//                    xhr: function () {
//                        if (this.url.indexOf('=') >= 0) {
//                            this.url = this.url.replace(/=/g, '="') + '"';
//                        };
//                        return jQuery.ajaxSettings.xhr.call(this);
//                    },
//                    dataFilter: function (data, dataType) {
//                        return jQuery.parseJSON(data).d;
//                    }
//                }
//            });

            //Default callback for creating the XMLHttpRequest object
    var xhr = jQuery.ajaxSettings.xhr;

            //Changing default values for all Ajax requests
    jQuery.ajaxSetup({
                //Required content type
                contentType: 'application/json; charset=utf-8',
                //New callback for creating the XMLHttpRequest object
                xhr: function () {
                    //Wrapping url parameters with '"'
                    if (this.url.indexOf('=') >= 0) {
                        this.url = this.url.replace(/=/g, '="') + '"';
                    }
                    //Calling default callback
                    return xhr.call(this);
                },
                //Sanitize the response (remove wrapping)
                dataFilter: function (data, dataType) {
                    if (dataType == "json") {
                        var result = $.parseJSON(data);
                        if (result.d) {
                            return result.d;
                        } else {
                            return data;
                        }
                    } else {
                        return data;
                    }
                }
            });
            jQuery('#fileSystemInfos').treeview({
                url: '../AccountInfo.aspx/FileSystemInfos")'
            });

});
