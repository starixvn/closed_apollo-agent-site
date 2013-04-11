/// <reference path="../../../_GlobalResources/Js/jquery.js" />
var ACCOUNTLIST = function ( $ ) {
    return {
        init: function () {
            var txtUsername = $( '#txtUserName' );
            txtUsername.unbind( 'click' ).bind( 'click', function () {
                if ( txtUsername.val() == BASELANGUAGES.EnterUsername ) {
                    txtUsername.val( '' );
                }
            } ),
            txtUsername.blur( function () {
                if ( txtUsername.val() == '' ) {
                    txtUsername.val( BASELANGUAGES.EnterUsername );
                }
            } )
        }
    };
} ( jQuery );
$( document ).ready( function () {
    ACCOUNTLIST.init();
} );