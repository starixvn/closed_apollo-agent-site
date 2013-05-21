
if (typeof jQuery !== 'undefined') {
    $(document).ready(function () {
        if (typeof APOLLO !== 'undefined') {
            APOLLO.signOut();
        }
    });
}