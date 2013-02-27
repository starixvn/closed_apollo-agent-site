
function ChangeDate(el) {

    var val = el.value;
    GetDateTime(val);
}

function ChangeCurrency() {
    
}

function SetDateTimeRange(from, to) {
    $("fdate").value = from;
    $("tdate").value = to;
}

function GetDateTime(_value) {
    var now = _page._date;
    var tokens = now.split("/");
    var cmonth = tokens[0];
    var cday = tokens[1];
    var cyear = tokens[2];
    var $date = new DateTime(cyear, cmonth, cday);
    switch (parseInt(_value)) {
        case 0:
            return;
            break;
        case 1:
            SetDateTimeRange($date.ToString(), $date.ToString());
            break;
        case 2:
            var yesterdate = $date.Yesterday();
            SetDateTimeRange(yesterdate, yesterdate);
            break;
        case 3:
            SetDateTimeRange($date.CurrentWeek(), $date.ToString());
            break;
        case 4:
            SetDateTimeRange($date.LastWeekStart(), $date.LastWeekEnd());
            break;
        case 5:
            SetDateTimeRange($date.CurrentMonth(), $date.ToString());
            break;
        case 6:
            SetDateTimeRange($date.LastMonthStart(), $date.LastMonthEnd());
            break;
        case 7:
            SetDateTimeRange($date.LastMonthStart(), $date.ToString());
            break;
        case 8:
            return;
            break;
    }
    return;
}


//define a DateTime Class
function DateTime(year, month, day) {
    //field
    this.year = parseInt(year, 10);
    this.month = parseInt(month, 10);
    this.day = parseInt(day, 10);
    //end field
    //////////
    //Method
    this.ToString = function () {
        return this.month + "/" + this.day + "/" + this.year;
    };
    this.DaysInMonth = function () {
        var m = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if (this.month != 2) return m[this.month - 1];
        if (this.year % 4 != 0) return m[1];
        if (this.year % 100 == 0 && this.year % 400 != 0) return m[1];
        return m[1] + 1;
    };
    //
    this.Reset = function () {
        this.year = parseInt(year, 10);
        this.month = parseInt(month, 10);
        this.day = parseInt(day, 10);
    };
    this.Yesterday = function () {
        this.AddDay(-1);
        var temp = this.ToString();
        this.Reset();
        return temp;
    };
    this.AddDay = function (seek) {
        var max = this.DaysInMonth();
        var newday = this.day + seek;

        if (newday <= max && newday > 0) {
            this.day = newday;
        }
        else {
            if (newday < 0) {
                if (this.month == 1) {
                    this.year--;
                    this.month = 12;
                }
                else {
                    this.month--;
                }
                this.day = this.DaysInMonth() + newday;
            }
            else {
                //newday>max
                if (this.month == 12) {
                    this.year++;
                    this.month = 1;

                }
                else {
                    this.month++;

                }
                this.day = this.DaysInMonth() - newday;
            }
        }
    };
    this.CurrentWeek = function () {
        var d = new Date();
        d.setFullYear(this.year, this.month - 1, this.day);
        var dayofweek = parseInt(d.getDay());
        this.AddDay(-dayofweek);
        var temp = this.ToString();
        this.Reset();
        return temp;
    };
    this.LastWeekStart = function () {
        var d = new Date();
        d.setFullYear(this.year, this.month - 1, this.day);
        var dayofweek = parseInt(d.getDay());
        this.AddDay(-(dayofweek + 7));
        var temp = this.ToString();
        this.Reset();
        return temp;
    };
    this.LastWeekEnd = function () {
        var d = new Date();
        d.setFullYear(this.year, this.month - 1, this.day);
        var dayofweek = parseInt(d.getDay());
        this.AddDay(-(dayofweek + 1));
        var temp = this.ToString();
        this.Reset();
        return temp;
    };
    this.CurrentMonth = function () {
        this.day = 1;
        var temp = this.ToString();
        this.Reset();
        return temp;
    };
    this.LastMonthStart = function () {
        if (this.month == 1) {
            this.year--;
            this.month = 12;
        }
        else {
            this.month--;
        }
        this.day = 1;
        var temp = this.ToString();
        this.Reset();
        return temp;
    };
    this.LastMonthEnd = function () {
        if (this.month == 1) {
            this.year--;
            this.month = 12;
        }
        else {
            this.month--;
        }
        this.day = this.DaysInMonth();
        var temp = this.ToString();
        this.Reset();
        return temp;
    };

    //end Method
}



function Daily_Onclick() {  

    $('frmmain').action = 'Daily_Consolidated.aspx';
    $('frmmain').submit();
}

function Monthly_Onclick() {
    $('frmmain').action = 'Monthly_Consolidated.aspx';
    $('frmmain').submit();
   
}

function Sport_Onclick() {
    $('frmmain').action = 'Sport_Consolidated.aspx';
    $('frmmain').submit();
}

function Winloss_SearchByDate(fdate, tdate, targetCustId, actionPage) {
    //$("ddlSelectDate").value = 8;
    $('frmmain').submit();
}