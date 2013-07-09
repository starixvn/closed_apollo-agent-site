/* File Created: April 4, 2013 */
if (typeof jQuery != 'undefined') {
	var FORECASTBYMATCH = function ($) {
		return {
			matchId: DEFAULT_OBJECT.matchId,
			betType: DEFAULT_OBJECT.betType,
			currentScore: DEFAULT_OBJECT.currentScore,
			contentLink: APOLLO.resolveUrl('Forecast/ForecastByMatchContent'),
			init: function () {
				parent.$(".ui-dialog .ui-dialog-title").text(DEFAULT_LAYOUT.forecastString);
				FORECASTBYMATCH.loadContentView();
			},
			updateValue: function () {
			},
			loadContentView: function () {
				var url = FORECASTBYMATCH.contentLink + '?matchId=' + FORECASTBYMATCH.matchId
					+ '&betType=' + FORECASTBYMATCH.betType
					+ '&currentScore=' + FORECASTBYMATCH.currentScore;
				if (DEFAULT_OBJECT.currentMainShareHolderId > 0) {
					url += '&mainShareHolderId=' + DEFAULT_OBJECT.currentMainShareHolderId;
				}
				$.get(url, function (contentView) {
					$("#forecast-body").html(contentView);
					FORECASTBYMATCH.registerEvents();
				});
			},
			registerEvents: function () {
				//// Register View BestList
				$('#forecast-body .view-bet-list').click(function () {
					var $this = $(this);
					var url = APOLLO.resolveUrl('Betlist/Forecast?matchId=' + $this.attr("match-id")
						+ '&betType=' + $this.attr("bet-type")
						+ '&type=BettypeMatch'
						+ '&leagueName=' + DEFAULT_OBJECT.leagueName);
					parent.$(".ui-dialog .ui-dialog-title").text(DEFAULT_LAYOUT.betListString);
					if (DEFAULT_OBJECT.currentMainShareHolderId > 0) {
						url += "&mainShareHolderId=" + DEFAULT_OBJECT.currentMainShareHolderId + "&isForecast=true";
					}
					location.href = url;
				});

				//// Register View Others betType
				$('#forecast-body .toggle-link').click(function () {
					var $this = $(this);
					FORECASTBYMATCH.betType = $this.attr('toggle-bet-type');
					FORECASTBYMATCH.currentScore = 0;
					FORECASTBYMATCH.loadContentView();
				});

				//// Register View Others Score
				$('#forecast-body .view-other-forecast').click(function () {
					var $this = $(this);
					FORECASTBYMATCH.currentScore = $this.attr('forecast-point');
					FORECASTBYMATCH.loadContentView();
				});
			}
		}
	} (jQuery);

	$(function () {
		FORECASTBYMATCH.init();

		$('#header_main').hide();
	}).ajaxStart($.blockUI).ajaxStop($.unblockUI);
}
function UpdateForecastValue() {
	FORECASTBYMATCH.loadContentView();
}