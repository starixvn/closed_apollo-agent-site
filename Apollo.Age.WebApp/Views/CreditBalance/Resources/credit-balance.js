/*global APOLLO, tooltip*/

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
                    $('#msg').html(tooltip.show('amount', "Required Field!")).show();
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
                            setTimeout(function () { window.parent.$("#apo-dialog-frame", window.parent.document).dialog('close'); }, 3000);
                        }
                    }
                });
            });

            $('#draw-agent-submit').on('click', function () {
                if ($('#amount').val() === '') {
                    $('#msg').html(tooltip.show('amount', "Required Field!")).show();
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
                            setTimeout(function () { window.parent.$("#apo-dialog-frame", window.parent.document).dialog('close'); }, 3000);
                        }
                    }
                });
            });

            $('#deposit-member-submit').on('click', function () {
                if ($('#amount').val() === '') {
                    $('#msg').html(tooltip.show('amount', "Required Field!")).show();
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
                            setTimeout(function () { window.parent.$("#apo-dialog-frame", window.parent.document).dialog('close'); }, 3000);
                        }
                    }
                });
            });

            $('#draw-member-submit').on('click', function () {
                if ($('#amount').val() === '') {
                    $('#msg').html(tooltip.show('amount', "Required Field!")).show();
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
                            setTimeout(function () { window.parent.$("#apo-dialog-frame", window.parent.document).dialog('close'); }, 3000);
                        }
                    }
                });
            });
        },
        validateAmount: function (target) {
            var newAmount = $(target).autoNumeric('get');
            if (newAmount < minAmount || (!isNaN(maxAmount) && newAmount > maxAmount)) {
                $('#msg').html(tooltip.show('amount', "Invalid Value!")).show();
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