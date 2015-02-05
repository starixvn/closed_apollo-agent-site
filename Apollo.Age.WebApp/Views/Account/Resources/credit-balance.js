/*global APOLLO, Validators, LANGUAGES*/

var creditBalancePopup = (function ($) {
    'use strict';
    var minAmount;
    var maxAmount;
    return {
        init: function () {
            minAmount = parseFloat($('#amount').data('min'));
            maxAmount = parseFloat($('#amount').data('max'));

            $('#amount').autoNumeric('init', {
                mDec: 0,
                vMin: 0,
                vMax: 999999999999,
                lZero: 'deny',
                aPad: false
            })
            .on('keyup', function () {
                creditBalancePopup.validateAmount(this);
            });

            $('#amount').on('keypress', function (event) {
                if (event.keyCode === 13) {
                    event.preventDefault();
                }
            });

            $('#deposit-agent-submit').on('click', function () {
                if ($('#amount').val() === '' || $('#amount').val() === '0') {
                    Validators.animate(document.getElementById('amount'));
                    return false;
                }

                if (!creditBalancePopup.validateAmount('#amount')) {
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: APOLLO.resolveUrl("Account/DepositAgent"),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: JSON.stringify({ Amount: $('#amount').autoNumeric('get') }),
                    success: function (rs) {
                        if (parseInt(rs.ErrorCode) === 0) {
                            APOLLO.showMessage("Deposit Successfully!", 'success');
                            setTimeout(function () {
                                if ($("#back-link").val() === '') {
                                    location.reload();
                                }
                                else {
                                    window.location.href = $("#back-link").val();
                                }
                            }, 3000);
                        }
                        else {
                            var errorMessage;
                            switch (parseInt(rs.ErrorCode)) {
                                case 8003:
                                    errorMessage = LANGUAGES.error8003;
                                    break;
                                case 8004:
                                    errorMessage = LANGUAGES.error8004;
                                    break;
                                case 8005:
                                    errorMessage = LANGUAGES.error8005;
                                    break;
                                case 8006:
                                    errorMessage = LANGUAGES.error8006;
                                    break;
                                case 8007:
                                    errorMessage = LANGUAGES.error8007;
                                    break;
                                case 5220:
                                    errorMessage = LANGUAGES.error5220;
                                    break;
                                case 5106:
                                    errorMessage = LANGUAGES.error5106;
                                    break;
                                default:
                                    errorMessage = LANGUAGES.error8012;
                            }

                            APOLLO.showMessage(errorMessage, 'error');
                        }
                    }
                });
            });

            $('#draw-agent-submit').on('click', function () {
                if ($('#amount').val() === '' || $('#amount').val() === '0') {
                    Validators.animate(document.getElementById('amount'));
                    return false;
                }

                if (!creditBalancePopup.validateAmount('#amount')) {
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: APOLLO.resolveUrl("Account/DrawAgent"),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: JSON.stringify({ Amount: $('#amount').autoNumeric('get') }),
                    success: function (rs) {
                        if (parseInt(rs.ErrorCode) === 0) {
                            APOLLO.showMessage("Draw Successfully!", 'success');
                            setTimeout(function () {
                                if ($("#back-link").val() === '') {
                                    location.reload();
                                }
                                else {
                                    window.location.href = $("#back-link").val();
                                }
                            }, 3000);
                        }
                        else {
                            var errorMessage;
                            switch (parseInt(rs.ErrorCode)) {
                                case 8003:
                                    errorMessage = LANGUAGES.error8003;
                                    break;
                                case 8008:
                                    errorMessage = LANGUAGES.error8004;
                                    break;
                                case 8009:
                                    errorMessage = LANGUAGES.error8005;
                                    break;

                                default:
                                    errorMessage = LANGUAGES.error8012;
                            }

                            APOLLO.showMessage(errorMessage, 'error');
                        }
                    }
                });
            });

            $('#deposit-member-submit').on('click', function () {
                if ($('#amount').val() === '' || $('#amount').val() === '0') {
                    Validators.animate(document.getElementById('amount'));
                    return false;
                }

                if (!creditBalancePopup.validateAmount('#amount')) {
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: APOLLO.resolveUrl("Account/DepositMember"),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: JSON.stringify({ Amount: $('#amount').autoNumeric('get') }),
                    success: function (rs) {
                        if (parseInt(rs.ErrorCode) === 0) {
                            APOLLO.showMessage("Deposit Successfully!", 'success');
                            setTimeout(function () {
                                if ($("#back-link").val() === '') {
                                    location.reload();
                                }
                                else {
                                    window.location.href = $("#back-link").val();
                                }
                            }, 3000);
                        }
                        else {
                            var errorMessage;
                            switch (parseInt(rs.ErrorCode)) {
                                case 8003:
                                    errorMessage = LANGUAGES.error8003;
                                    break;
                                case 8006:
                                    errorMessage = LANGUAGES.error8006;
                                    break;
                                case 8007:
                                    errorMessage = LANGUAGES.error8007;
                                    break;
                                case 5220:
                                    errorMessage = LANGUAGES.error5220;
                                    break;
                                case 5106:
                                    errorMessage = LANGUAGES.error5106;
                                    break;
                                default:
                                    errorMessage = LANGUAGES.error8012;
                            }

                            APOLLO.showMessage(errorMessage, 'error');
                        }
                    }
                });
            });

            $('#draw-member-submit').on('click', function () {
                if ($('#amount').val() === '' || $('#amount').val() === '0') {
                    Validators.animate(document.getElementById('amount'));
                    return false;
                }

                if (!creditBalancePopup.validateAmount('#amount')) {
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: APOLLO.resolveUrl("Account/DrawMember"),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: JSON.stringify({ Amount: $('#amount').autoNumeric('get') }),
                    success: function (rs) {
                        if (parseInt(rs.ErrorCode) === 0) {
                            APOLLO.showMessage("Draw Successfully!", 'success');
                            setTimeout(function () {
                                if ($("#back-link").val() === '') {
                                    location.reload();
                                }
                                else {
                                    window.location.href = $("#back-link").val();
                                }
                            }, 3000);
                        }
                        else {
                            var errorMessage;
                            switch (parseInt(rs.ErrorCode)) {
                                case 8003:
                                    errorMessage = LANGUAGES.error8003;
                                    break;
                                case 8008:
                                    errorMessage = LANGUAGES.error8004;
                                    break;
                                case 8009:
                                    errorMessage = LANGUAGES.error8005;
                                    break;

                                default:
                                    errorMessage = LANGUAGES.error8012;
                            }

                            APOLLO.showMessage(errorMessage, 'error');
                        }
                    }
                });
            });
        },
        validateAmount: function (target) {
            var newAmount = $(target).autoNumeric('get');
            if (newAmount < minAmount || (!isNaN(maxAmount) && newAmount > maxAmount)) {
                Validators.animate(document.getElementById('amount'));
                return false;
            }

            return true;
        }
    };
} (jQuery));

$(function () {
    'use strict';
    creditBalancePopup.init();
});