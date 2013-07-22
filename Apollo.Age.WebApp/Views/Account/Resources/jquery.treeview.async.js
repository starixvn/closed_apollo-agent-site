/*
* Async Treeview 0.1 - Lazy-loading extension for Treeview
* 
* http://bassistance.de/jquery-plugins/jquery-plugin-treeview/
*
* Copyright (c) 2007 Jörn Zaefferer
*
* Dual licensed under the MIT and GPL licenses:
*   http://www.opensource.org/licenses/mit-license.php
*   http://www.gnu.org/licenses/gpl.html
*
* Revision: $Id$
*
*/

; (function ($) {
    
    function stringFormat () {
      var s = arguments[0];
      for (var i = 0; i < arguments.length - 1; i++) {
          var reg = new RegExp("\\{" + i + "\\}", "gm");
          s = s.replace(reg, arguments[i + 1]);
      }
      return s;
    }

    function load(settings, root, child, container, showRoot) {
        function createNode(parent) {
                if (this.Id != undefined)
                {
                    var current = $("<li/>").attr("id", this.Id).html("<span>" + stringFormat((showRoot? settings.linkdirectAgents : settings.link) , this.Id, this.Username, this.Username, root)  + "</span>").appendTo(parent);

                    if (this.Classes) {
                        current.children("span").addClass(this.Classes);
                    }
                    if (this.Expanded) {
                        current.addClass("open");
                    }
                    if (this.HasAgentDownline || this.Children && this.Children.length) {
                        var branch = $("<ul/>").appendTo(current);
                        if (this.HasAgentDownline) {
                            current.addClass("hasChildren");
                            createNode.call({
                                text: "&nbsp;",
                                classes: "",
                                children: []
                            }, branch);
                        }
                        if (this.Children && this.Children.length) {
                            showRoot = false;
                            $.each(this.Children, createNode, [branch])
                        }
                    }
                }
            }
        if(showRoot)
        {
            $.getJSON(settings.rootLink, null, function (response) {
                $.each(response, createNode, [child]);
                $(container).treeview({ add: child });
            });
        }
        else
        {
            $.getJSON(settings.url+"?agentId=" + root, null, function (response) {
                $.each(response, createNode, [child]);
                $(container).treeview({ add: child });
            });
        }
    }

    var proxied = $.fn.treeview;
    $.fn.treeview = function (settings) {
        if (!settings.url) {
            return proxied.apply(this, arguments);
        }
        var container = this;
        //load(settings, "source", this, container);
        load(settings, settings.agentId, this, container, settings.showRoot); // -1 is dummy root node id
        var userToggle = settings.toggle;
        return proxied.call(this, $.extend({}, settings, {
            collapsed: true,
            toggle: function () {
                var $this = $(this);
                if ($this.hasClass("hasChildren")) {
                    var childList = $this.removeClass("hasChildren").find("ul");
                    childList.empty();
                    load(settings, this.id, childList, container, false);
                }
                if (userToggle) {
                    userToggle.apply(this, arguments);
                }
            }
        }));
    };

})(jQuery);