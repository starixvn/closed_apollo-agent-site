/// <reference path="../../../_GlobalResources/Js/jquery.js" />
var ACCOUNTLIST = function ( $ ) {
    return {
        init: function () {
            var txtUsername = $( '#txtUserName' );
            txtUsername.unbind( 'click' ).bind( 'click', function () {
                if ( APOLLO.htmlDecode(txtUsername.val()) == APOLLO.htmlDecode(BASELANGUAGES.EnterUsername) ) {
                    txtUsername.val( '' );
                }
            } ),
            txtUsername.blur( function () {
                if ( txtUsername.val() == '' ) {
                    txtUsername.val(APOLLO.htmlDecode(BASELANGUAGES.EnterUsername));
                }
            } )
        }
    };
} ( jQuery );
$( document ).ready( function () {
    ACCOUNTLIST.init();
} );