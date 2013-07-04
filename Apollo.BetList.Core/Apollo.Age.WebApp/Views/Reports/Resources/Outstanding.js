var OUTSTANDING = function ($) {
	return {
		init: function () {
			OUTSTANDING.ddlSport = $("#ddlSport");
			OUTSTANDING.ddlBettype = $("#ddlBettype");

			$("#btnSubmit").bind('click', function () {

					$.blockUI();
					var url = APOLLO.resolveUrl('Reports/Outstanding?Sporttypes=' + OUTSTANDING.ddlSport.val()
                    + "&Bettypes=" + OUTSTANDING.ddlBettype.val());
					location.href = url;
			});

			$("#btnCancel").bind('click', function () {
				$.blockUI();
				history.back();
				return false;
			});

			$("#lnkExit").bind('click', function () {
				$.blockUI();
				history.back();
				return false;
			});

			$("#btnBack").bind('click', function () {
				$.blockUI();
				history.back();
				return false;
			});
		}
	};
} (jQuery);

$(document).ready(function () {
	$.blockUI();
	OUTSTANDING.init();
	$.unblockUI();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);
