/*global APOLLO*/
'use strict';
if (typeof jQuery !== 'undefined') {
    $(document).ready(function () {
        if (typeof APOLLO !== 'undefined') {
            try {
                APOLLO.signOut();
            }
            catch (err) {
                APOLLO.console.warn(err);
            }
        }
    });
}