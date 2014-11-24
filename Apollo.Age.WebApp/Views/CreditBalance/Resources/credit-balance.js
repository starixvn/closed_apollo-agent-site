/*global APOLLO, Validators*/

var creditBalancePopup = (function ($) {
    'use strict';
    var minAmount;
    var maxAmount;
    return {
        init: function () {
            minAmount = parseFloat($('#amount').data('min'));
            maxAmount = parseFloat($('#amount').data('max'));
            jQuery("#credit-form").validationEngine();
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

            $('#deposit-agent-submit').on('click', function () {
                if ($('#amount').val() === '') {
                    Validators.animate(document.getElementById('amount'));
                    return;
                }

                if (!creditBalancePopup.validateAmount('#amount')) {
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: APOLLO.resolveUrl("CreditBalance/DepositAgent"),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: JSON.stringify({ Amount: $('#amount').val() }),
                    success: function (rs) {
                        if (parseInt(rs.ErrorCode) === 0) {
                            APOLLO.showMessage("Deposit Successfully!", 'success');
                            setTimeout(function () { window.location.href = $("#back-link").val(); }, 5000);
                        }
                        else {
                            APOLLO.showMessage(rs.ErrorMsg, 'error');
                        }
                    }
                });
            });

            $('#draw-agent-submit').on('click', function () {
                if ($('#amount').val() === '') {
                    Validators.animate(document.getElementById('amount'));
                    return;
                }

                if (!creditBalancePopup.validateAmount('#amount')) {
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: APOLLO.resolveUrl("CreditBalance/DrawAgent"),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: JSON.stringify({ Amount: $('#amount').val() }),
                    success: function (rs) {
                        if (parseInt(rs.ErrorCode) === 0) {
                            APOLLO.showMessage("Draw Successfully!", 'success');
                            setTimeout(function () { window.location.href = $("#back-link").val(); }, 5000);
                        }
                        else {
                            APOLLO.showMessage(rs.ErrorMsg, 'error');
                        }
                    }
                });
            });

            $('#deposit-member-submit').on('click', function () {
                if ($('#amount').val() === '') {
                    Validators.animate(document.getElementById('amount'));
                    return;
                }

                if (!creditBalancePopup.validateAmount('#amount')) {
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: APOLLO.resolveUrl("CreditBalance/DepositMember"),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: JSON.stringify({ Amount: $('#amount').val() }),
                    success: function (rs) {
                        if (parseInt(rs.ErrorCode) === 0) {
                            APOLLO.showMessage("Deposit Successfully!", 'success');
                            setTimeout(function () { window.location.href = $("#back-link").val(); }, 5000);
                        }
                        else {
                            APOLLO.showMessage(rs.ErrorMsg, 'error');
                        }
                    }
                });
            });

            $('#draw-member-submit').on('click', function () {
                if ($('#amount').val() === '') {
                    Validators.animate(document.getElementById('amount'));
                    return;
                }

                if (!creditBalancePopup.validateAmount('#amount')) {
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: APOLLO.resolveUrl("CreditBalance/DrawMember"),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: JSON.stringify({ Amount: $('#amount').val() }),
                    success: function (rs) {
                        if (parseInt(rs.ErrorCode) === 0) {
                            APOLLO.showMessage("Draw Successfully!", 'success');
                            setTimeout(function () { window.location.href = $("#back-link").val(); }, 5000);
                        }
                        else {
                            APOLLO.showMessage(rs.ErrorMsg, 'error');
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