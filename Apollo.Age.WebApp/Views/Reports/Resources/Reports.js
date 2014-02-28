'use strict';
var REPORTS = (function ($) {
	return {
		init: function () {
			$("#btnCancel").on('click', function () {
				$.blockUI();
				history.back();
				return false;
			});

			$("#lnkExit").on('click', function () {
				$.blockUI();
				history.back();
				return false;
			});

			$("#btnBack").on('click', function () {
				$.blockUI();
				history.back();
				return false;
			});
		}
	};
} (jQuery));

$(document).ready(function () {
	REPORTS.init();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);