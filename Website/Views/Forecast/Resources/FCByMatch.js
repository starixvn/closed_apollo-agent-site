/* File Created: April 4, 2013 */
if (typeof jQuery != 'undefined') {
	var FORECASTBYMATCH = function ($) {
		return {
			matchId: matchId,
			betType: betType,
			currentScore: currentScore,
			contentLink: APOLLO.resolveUrl('Forecast/FORECASTBYMATCHContent'),
			init: function () {
				FORECASTBYMATCH.loadContentView();
			},
			updateValue: function () {
			},
			loadContentView: function () {
				var url = FORECASTBYMATCH.contentLink + '?matchId=' + FORECASTBYMATCH.matchId
        + '&betType=' + FORECASTBYMATCH.betType
        + '&currentScore=' + FORECASTBYMATCH.currentScore;
				$.get(url, function (contentView) {
					$("#forecast-body").html(contentView);
					FORECASTBYMATCH.registerEvents();
				});
			},
			registerEvents: function () {
				//// Register View BestList
				$('#forecast-body .view-bet-list').click(function () {
					var $this = $(this);
					var url = APOLLO.resolveUrl('_BetList/BetList.aspx?matchId=' + $this.attr("match-id")
            + '&betType=' + $this.attr("bet-type")
            + '&type=BettypeMatch'
            + '&leagueName=' + leagueName);
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
    RefreshCountDown_Init();
	FORECASTBYMATCH.loadContentView();
}