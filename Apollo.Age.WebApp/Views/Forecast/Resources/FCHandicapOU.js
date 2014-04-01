/* File Created: April 3, 2013 */
/*global DEFAULT_OBJECT*/
/*global DEFAULT_LAYOUT*/
/*global APOLLO*/
/*global countDownTimer*/
'use strict';
if (typeof jQuery !== 'undefined') {
	var FCHANDICAPOU = (function ($) {
		return {
			currentLeague: 0,
			currentLeagueName: DEFAULT_OBJECT.leagueName,
			init: function () {
				FCHANDICAPOU.currentLeague = $('#ddlLeague').val();
				if (FCHANDICAPOU.currentLeague) {
					FCHANDICAPOU.currentLeagueName = $('#ddlLeague option:selected').text();
					$('#ddlLeague').on("change", function () {
						var $this = $(this);
						FCHANDICAPOU.currentLeague = $this.val();
						FCHANDICAPOU.currentLeagueName = $('#ddlLeague option:selected').text();
						if (FCHANDICAPOU.currentLeague) {
							FCHANDICAPOU.changeLeague();
						}
					});
					FCHANDICAPOU.registerEvents();
				}
			},
			registerEvents: function () {
				$('#forecast-body')
			.off("click")
			.on("click", ".view-forecast-detail", function () {
				var $this = $(this);
				if (countDownTimer != null) {
					clearTimeout(countDownTimer); 
				}
				var url = APOLLO.resolveUrl('Forecast/ForecastByMatch?matchId=' + $this.attr("match-id")
					+ '&betType=' + $this.attr("bet-type")
					+ '&leagueName=' + FCHANDICAPOU.currentLeagueName);
				if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
					url += "&mainShareHolderId=" + DEFAULT_LAYOUT.currentMainShareHolderId;
				}

				APOLLO.openDialogFrame(url, DEFAULT_LAYOUT.forecastString, {
					width: 680,
					height: 350,
					position: { my: "top+50", at: "center top", of: window },
					close: function () {
					/*jshint ignore:start */
					countDownTimer = setTimeout(CountDownRefreshTick, 1000);
					/*jshint ignore:end */
					}
				});
			});
			},
			changeLeague: function () {
				var url = APOLLO.resolveUrl('Forecast/FCHandicapOverUnderMatches?leagueId=' + FCHANDICAPOU.currentLeague
				+ '&mainShareHolderId=' + DEFAULT_LAYOUT.currentMainShareHolderId);
				$("#forecast-body").load(url);
				FCHANDICAPOU.registerEvents();
			}
		};
	} (jQuery));

	$(function () {
		if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
			$("#ddlMainShareHolder").change(function () {
				var $this = $(this);
				window.location.href = DEFAULT_LAYOUT.currentActionLink + "?mainShareHolderId=" + $this.val();
			});

			$(".selectLeague label").width($("#main-shareholder-box label").width());
		}

		FCHANDICAPOU.init();
	}).ajaxStart($.blockUI).ajaxStop($.unblockUI);
}
/*jshint ignore:start */
function UpdateForecastHDValue() {
    var url = APOLLO.resolveUrl("Forecast/FCHandicapOverUnder?leagueId=" + FCHANDICAPOU.currentLeague);
    if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
        url += '&mainShareHolderId=' + DEFAULT_LAYOUT.currentMainShareHolderId;
    }
    window.location.href = url;
}
/*jshint ignore:end */