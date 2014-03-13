/*
* Created 20100226@Lee - Error message javascript functions
* Revision ?@? - ...
*
*/

// Prototype
/*jshint strict:false*/
AGEMsg = function () {
    this.spmsgerr = 'spmsgerr';
    this.diverrmsg = 'diverrmsg';
};

AGEMsg.prototype.Show = function (msg, success) {
    document.getElementById(this.spmsgerr).innerHTML = msg;
    document.getElementById(this.diverrmsg).style.display = IE ? 'block' : 'table';
};

AGEMsg.prototype.Hide = function () {
    document.getElementById(this.diverrmsg).style.display = 'none';
};

ageMsg = new AGEMsg();