using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Text;
using DNAide.Web;
using DNAide.Utils;
using DNAide.Core;
using Fanex.Data.DNAide;
using System.IO;
using Sunplus.Agent.Model.Entities.Permission;
public partial class Common
{

    #region ViewLogDependencies

    public static string GenGoToPage(string url)
    {
        string goToPageHtml = TemplateHandler.LoadTemplate("GotoPage.html");
        TemplateHandler.Assign(ref goToPageHtml, "textgotopage", Resources.Lang.gotopage);
        TemplateHandler.Assign(ref goToPageHtml, "textgo", Resources.Lang.go);
        TemplateHandler.Assign(ref goToPageHtml, "Url", url);
        TemplateHandler.Assign(ref goToPageHtml, "Site", "");
        return goToPageHtml;
    }


    #endregion



    public static readonly string KEY_ENC_STRING = "01a2bAW3c4d5e6f9g123DETESMSITsmsit";
    public static string GetLangChar(HttpContext hc)
    {
        switch (Sunplus.Agent.Model.Entities.UserSession.Current.Language)
        {
            case "en-US": return "E";
            case "ja-JP": return "J";
            case "zh-TW": return "CT";
            case "zh-CN": return "CS";
            case "th-TH": return "TH";
            default: return "E";
        }
    }

    public static string GetLangChar(string Locale)
    {
        switch (Locale)
        {
            case "en-US": return "E";
            case "ja-JP": return "J";
            case "zh-TW": return "CT";
            case "zh-CN": return "CS";
            case "th-TH": return "TH";
            default: return "E";
        }
    }
    public static string NoInfo(int intNumCols)
    {
        string strNoInfo = TemplateHandler.LoadTemplate("NoInfo.html");
        TemplateHandler.Assign(ref strNoInfo, "noinfo", Resources.Lang.noInfo);
        TemplateHandler.Assign(ref strNoInfo, "num-cols", intNumCols.ToString());
        return strNoInfo;
    }
    public static string NoInfo(int intNumCols, string msg)
    {
        string strNoInfo = TemplateHandler.LoadTemplate("NoInfo.html");
        TemplateHandler.Assign(ref strNoInfo, "noinfo", msg);
        TemplateHandler.Assign(ref strNoInfo, "num-cols", intNumCols.ToString());
        return strNoInfo;
    }

    public static string ShowErr_JS(string ex_err)
    {
        string _StrMain = ex_err;
        if (ex_err != "")
        {
            string Temp = ex_err.Substring(1, ex_err.Length - 1).Replace("'", "");
            string _Begin = ex_err.Substring(0, 1);
            string _End = ex_err.Substring(ex_err.Length - 1, 1);
            _StrMain = _Begin + Temp + _End;
        }
        return _StrMain;
    }
    /// <summary>
    /// Create date time selector
    /// </summary>
    /// <param name="txtDateId"></param>
    /// <param name="currentPos"></param>
    /// <param name="triggerId"></param>
    /// <param name="currentDate"></param>
    /// <param name="noJs"></param>
    /// <param name="showTime"></param>
    /// <returns></returns>
    public static string GenCalendarControl(string txtDateId, string currentPos, string triggerId, string currentDate, bool noJs, bool showTime)
    {

        if (string.IsNullOrEmpty(txtDateId) || string.IsNullOrEmpty(triggerId)) return null;
        string js = (noJs ? "" : TemplateHandler.LoadTemplate("CalendarJs.html"));
        string txtClass = showTime ? "txtdate-show-time" : "date_no";
        string singleClick = showTime ? "false" : "true";
        TemplateHandler.Assign(ref js, "basePath", TplCore.TplSettings.GetBaseUrl());
        return TemplateHandler.LoadHtml("Calendar.html",
            new string[] { "{txtDate}", "{themepath}", "{triggerId}", "{currentDate}", "{JS}", "{ShowTime}", "{TxtDateCls}", "{SecFormat}", "{IsSingleClick}", "{submit}" },
            new string[] { txtDateId, currentPos + AppSettings.ThemeDir, triggerId, currentDate, js, showTime.ToString().ToLower(), txtClass, (showTime ? " %H:%M:%S" : ""), singleClick, Resources.Lang.submit });

    }

    /// <summary>
    /// Create date time selector
    /// @Phuong.Nguyen modified <2008-10-16>
    /// Set server datetime for calendar with "{currentServerDate}" parameter
    /// </summary>
    /// <param name="txtDateId"></param>
    /// <param name="currentPos"></param>
    /// <param name="triggerId"></param>
    /// <param name="currentDate"></param>
    /// <param name="noJs"></param>
    /// <param name="showTime"></param>
    /// <param name="showOtherDays"></param>
    /// <param name="useMultiLang"></param>

    /// <returns></returns>

    public static string GenCalendarControl(string txtDateId, string currentPos, string triggerId, string currentDate,
                                            bool noJs, bool showTime, bool showOtherDays, bool useMultiLang)
    {

        if (string.IsNullOrEmpty(txtDateId) || string.IsNullOrEmpty(triggerId)) return null;
        string txtClass = showTime ? "txtdate-show-time" : "date_no";
        string singleClick = showTime ? "false" : "true";
        string strShowOthersDay = showOtherDays ? "true" : "false";
        string calendarTpl = TemplateHandler.LoadHtml("Calendar2.html",
                                        new string[]
                                                {
                                                    "{txtDate}", "{themepath}", "{triggerId}", "{currentDate}", "{currentServerDate}", 
                                                    "{ShowTime}", "{TxtDateCls}", "{SecFormat}", "{IsSingleClick}",
                                                    "{submit}", "{ShowOthers}","{AllowMonth}"
                                                },
                                        new string[]
                                                {
                                                    txtDateId, currentPos + AppSettings.ThemeDir, triggerId, currentDate, DateTime.Now.ToShortDateString(),
                                                     showTime.ToString().ToLower(), txtClass,
                                                    (showTime ? " %H:%M:%S" : ""), singleClick, Resources.Lang.submit, strShowOthersDay,
                                                    CheckTimeWorldCup()
                                                });

        TemplateHandler.Assign(ref calendarTpl, "basePath", TplCore.TplSettings.GetBaseUrl());

        string js = (noJs ? "" : TemplateHandler.CheckOutBlock(ref calendarTpl, "calendar-include"));
        string lang = useMultiLang ? GetLangChar(Sunplus.Agent.Model.Entities.UserSession.Current.Language) : GetLangChar("en-US");

        TemplateHandler.Assign(ref js, "{Lang}", lang);
        if (!string.IsNullOrEmpty(js))
            TemplateHandler.CheckInBlock(ref calendarTpl, "calendar-include", js);
        else
            TemplateHandler.RemoveBlock(ref calendarTpl, "calendar-include");
        return calendarTpl;

    }
    /// <summary>
    /// Will be remove once finished WorldCup
    /// </summary>
    /// <returns></returns>
    public static string CheckTimeWorldCup()
    {
        DateTime fromDateWC = new DateTime(2010, 06, 11);
        DateTime toDateWC = new DateTime(2010, 07, 12);
        if (DateTime.Compare(DateTime.Now, fromDateWC) >= 0 && DateTime.Compare(DateTime.Now, toDateWC) <= 0)
        {
            if (DateTime.Now.TimeOfDay.Hours >= 10 && DateTime.Now.TimeOfDay.Hours <= 17) //from 10:00 PM to 5:00 AM GMT +8
            {
                return "1"; // Allow 1 month
            }
        }
        return "3"; // Allow 3 months
    }
    public static bool IsTimeWorldCup()
    {
        DateTime fromDateWC = new DateTime(2010, 06, 11);
        DateTime toDateWC = new DateTime(2010, 07, 12);
        if (DateTime.Compare(DateTime.Now, fromDateWC) >= 0 && DateTime.Compare(DateTime.Now, toDateWC) <= 0)
        {
            if (DateTime.Now.TimeOfDay.Hours >= 10 && DateTime.Now.TimeOfDay.Hours <= 17) //from 10:00 PM to 5:00 AM GMT +8
            {
                return true; // WorldCup Time
            }
        }
        return false; // Normal Time
    }
    public static string GenCalendarControlNoLimit(string txtDateId, string currentPos, string triggerId, string currentDate,
                                            bool noJs, bool showTime, bool showOtherDays, bool useMultiLang)
    {

        if (string.IsNullOrEmpty(txtDateId) || string.IsNullOrEmpty(triggerId)) return null;
        string txtClass = showTime ? "txtdate-show-time" : "date_no";
        string singleClick = showTime ? "false" : "true";
        string strShowOthersDay = showOtherDays ? "true" : "false";
        string calendarTpl = TemplateHandler.LoadHtml("Calendar3.html",
                                        new string[]
                                                {
                                                    "{txtDate}", "{themepath}", "{triggerId}", "{currentDate}", "{currentServerDate}", 
                                                    "{ShowTime}", "{TxtDateCls}", "{SecFormat}", "{IsSingleClick}",
                                                    "{submit}", "{ShowOthers}"
                                                },
                                        new string[]
                                                {
                                                    txtDateId, currentPos + AppSettings.ThemeDir, triggerId, currentDate, DateTime.Now.ToShortDateString(),
                                                     showTime.ToString().ToLower(), txtClass,
                                                    (showTime ? " %H:%M:%S" : ""), singleClick, Resources.Lang.submit, strShowOthersDay
                                                });

        TemplateHandler.Assign(ref calendarTpl, "basePath", TplCore.TplSettings.GetBaseUrl());

        string js = (noJs ? "" : TemplateHandler.CheckOutBlock(ref calendarTpl, "calendar-include"));
        string lang = useMultiLang ? GetLangChar(Sunplus.Agent.Model.Entities.UserSession.Current.Language) : GetLangChar("en-US");

        TemplateHandler.Assign(ref js, "{Lang}", lang);
        if (!string.IsNullOrEmpty(js))
            TemplateHandler.CheckInBlock(ref calendarTpl, "calendar-include", js);
        else
            TemplateHandler.RemoveBlock(ref calendarTpl, "calendar-include");
        return calendarTpl;

    }
    public static string GenCalendarControl(string txtDateId, string currentPos, string triggerId, string currentDate, bool noJs)
    {

        return GenCalendarControl(txtDateId, currentPos, triggerId, currentDate, noJs, false);

    }

    public static void SetGUIByLang(ref string template)
    {
        string widthSubmitBtn = "style='width:55px'";
        if (Sunplus.Agent.Model.Entities.UserSession.Current.LangKey.Equals("KO"))
        {
            widthSubmitBtn = "style='width:120px'";
        }
        TemplateHandler.Assign(ref template, "{widthSubmitBtn}", widthSubmitBtn);

        //string btnCss = "btn";
        //string btnHoverCss = "btnhover";

        //if (Sunplus.Agent.Model.Entities.UserSession.Current.LangKey == "KO")
        //{
        //    btnCss += Sunplus.Agent.Model.Entities.UserSession.Current.LangKey;
        //    btnHoverCss = String.Format("btn{0}hover", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        //}

        //TemplateHandler.Assign(ref template, "{btnCss}", btnCss);
        //TemplateHandler.Assign(ref template, "{btnHoverCss}", btnHoverCss);
    }

    /// <summary>
    /// Create date/time range selector
    /// </summary>
    /// <param name="currentPos"></param>
    /// <param name="noDayReset"></param>
    /// <param name="fdateId"></param>
    /// <param name="tdateId"></param>
    /// <param name="fdate"></param>
    /// <param name="tdate"></param>
    /// <param name="noJs"></param>
    /// <param name="showTime"></param>
    /// <returns></returns>
    public static string CreateDateRangeSelector(string currentPos, int noDayReset, string fdateId, string tdateId, DateTime fdate, DateTime tdate, bool noJs, bool showTime, bool blShowToday)
    {

        string dateRangeTpl = TemplateHandler.LoadTemplate("DateRangeSelect.html");

        //SetGUIByLang(ref dateRangeTpl);

        TemplateHandler.Assign(ref dateRangeTpl, "{CurrentPos}", currentPos);
        TemplateHandler.Assign(ref dateRangeTpl, "{DayNoReset}", noDayReset.ToString());
        TemplateHandler.Assign(ref dateRangeTpl, "{Fdate}", fdateId);
        TemplateHandler.Assign(ref dateRangeTpl, "{Tdate}", tdateId);
        TemplateHandler.Assign(ref dateRangeTpl, "{submit}", Resources.Lang.submit);
        TemplateHandler.Assign(ref dateRangeTpl, "{today_now}", DateTime.Now.ToShortDateString());
        TemplateHandler.Assign(ref dateRangeTpl, "{yesterday}", DateTime.Now.AddDays(-1).ToShortDateString());
        TemplateHandler.Assign(ref dateRangeTpl, "fdatetext", Resources.Lang.from);
        TemplateHandler.Assign(ref dateRangeTpl, "tdatetext", Resources.Lang.to);
        string strShowToday = blShowToday ? "" : "hidden";
        TemplateHandler.Assign(ref dateRangeTpl, "{hide}", strShowToday);
        string fdateStr = showTime ? DateFormat.ShortDateFormat(fdate) : DateFormat.ShortDateFormat(fdate);
        string tdateStr = showTime ? DateFormat.ShortDateFormat(tdate) : DateFormat.ShortDateFormat(tdate);
        TemplateHandler.Assign(ref dateRangeTpl, "{FromDateCal}", GenCalendarControl(fdateId, currentPos, fdateId + "_tg", fdateStr, noJs, showTime));
        TemplateHandler.Assign(ref dateRangeTpl, "{ToDateCal}", GenCalendarControl(tdateId, currentPos, tdateId + "_tg", tdateStr, true, showTime));
        return dateRangeTpl;

    }
    /// <summary>
    /// Author: Anh.Pham
    /// Create date/time range selector
    /// </summary>
    /// <param name="currentPos"></param>
    /// <param name="noDayReset"></param>
    /// <param name="fdateId"></param>
    /// <param name="tdateId"></param>
    /// <param name="fdate"></param>
    /// <param name="tdate"></param>
    /// <param name="noJs"></param>
    /// <param name="showTime"></param>
    /// <returns></returns>
    public static string CreateDateRangeSelectorOfMessageInDetail(string currentPos, int noDayReset, string fdateId, string tdateId, DateTime fdate, DateTime tdate, bool noJs, bool showTime, bool blShowToday, bool showtdate, bool showOtherDays, bool useMultiLang)
    {

        string dateRangeTpl = TemplateHandler.LoadTemplate("DateRangeSelect2.html");

        //SetGUIByLang(ref dateRangeTpl);

        TemplateHandler.Assign(ref dateRangeTpl, "{CurrentPos}", currentPos);
        TemplateHandler.Assign(ref dateRangeTpl, "{DayNoReset}", noDayReset.ToString());
        TemplateHandler.Assign(ref dateRangeTpl, "{Fdate}", fdateId);
        TemplateHandler.Assign(ref dateRangeTpl, "{Tdate}", tdateId);
        TemplateHandler.Assign(ref dateRangeTpl, "{submit}", Resources.Lang.submit);
        TemplateHandler.Assign(ref dateRangeTpl, "tdatetext", Resources.Lang.to);
        string strShowToday = blShowToday ? "" : "hidden";
        TemplateHandler.Assign(ref dateRangeTpl, "{hide}", strShowToday);
        string fdateStr = showTime ? DateFormat.ShortDateFormat(fdate) : DateFormat.ShortDateFormat(fdate);
        string tdateStr = showTime ? DateFormat.ShortDateFormat(tdate) : DateFormat.ShortDateFormat(tdate);
        if (showtdate == false)
        {
            TemplateHandler.RemoveBlock(ref dateRangeTpl, "limit-todate");
            TemplateHandler.Assign(ref dateRangeTpl, "fdatetext", Resources.Lang.date);
        }
        else
        {
            TemplateHandler.Assign(ref dateRangeTpl, "fdatetext", Resources.Lang.from);
        }
        TemplateHandler.Assign(ref dateRangeTpl, "{FromDateCal}", GenCalendarControl(fdateId, currentPos, fdateId + "_tg", fdateStr, noJs, showTime, showOtherDays, useMultiLang));
        TemplateHandler.Assign(ref dateRangeTpl, "{ToDateCal}", GenCalendarControl(tdateId, currentPos, tdateId + "_tg", tdateStr, true, showTime, showOtherDays, useMultiLang));
        TemplateHandler.Assign(ref dateRangeTpl, "{Fdate-value}", fdate.ToShortDateString());
        TemplateHandler.Assign(ref dateRangeTpl, "{Tdate-value}", tdate.ToShortDateString());
        return dateRangeTpl;

    }
    public static string CreateDateRangeSelector(string currentPos, int noDayReset, string fdateId, string tdateId, DateTime fdate, DateTime tdate, bool noJs, bool showTime, bool blShowToday, bool showtdate)
    {

        string dateRangeTpl = TemplateHandler.LoadTemplate("DateRangeSelect.html");

        //SetGUIByLang(ref dateRangeTpl);

        TemplateHandler.Assign(ref dateRangeTpl, "{CurrentPos}", currentPos);
        TemplateHandler.Assign(ref dateRangeTpl, "{DayNoReset}", noDayReset.ToString());
        TemplateHandler.Assign(ref dateRangeTpl, "{Fdate}", fdateId);
        TemplateHandler.Assign(ref dateRangeTpl, "{Tdate}", tdateId);
        TemplateHandler.Assign(ref dateRangeTpl, "{submit}", Resources.Lang.submit);
        TemplateHandler.Assign(ref dateRangeTpl, "{today_now}", DateTime.Now.ToShortDateString());
        TemplateHandler.Assign(ref dateRangeTpl, "{yesterday}", DateTime.Now.AddDays(-1).ToShortDateString());
        TemplateHandler.Assign(ref dateRangeTpl, "tdatetext", Resources.Lang.to);
        string strShowToday = blShowToday ? "" : "hidden";
        TemplateHandler.Assign(ref dateRangeTpl, "{hide}", strShowToday);
        string fdateStr = showTime ? DateFormat.ShortDateFormat(fdate) : DateFormat.ShortDateFormat(fdate);
        string tdateStr = showTime ? DateFormat.ShortDateFormat(tdate) : DateFormat.ShortDateFormat(tdate);
        if (showtdate == false)
        {
            TemplateHandler.RemoveBlock(ref dateRangeTpl, "limit-todate");
            TemplateHandler.Assign(ref dateRangeTpl, "fdatetext", Resources.Lang.date);
        }
        else
        {
            TemplateHandler.Assign(ref dateRangeTpl, "fdatetext", Resources.Lang.from);
        }
        TemplateHandler.Assign(ref dateRangeTpl, "{FromDateCal}", GenCalendarControl(fdateId, currentPos, fdateId + "_tg", fdateStr, noJs, showTime));
        TemplateHandler.Assign(ref dateRangeTpl, "{ToDateCal}", GenCalendarControl(tdateId, currentPos, tdateId + "_tg", tdateStr, true, showTime));
        return dateRangeTpl;

    }

    public static string CreateDateNoRangeSelector(string currentPos, int noDayReset, string fdateId, string tdateId, DateTime fdate, DateTime tdate, bool noJs, bool showTime, bool blShowToday, bool showtdate)
    {

        string dateRangeTpl = TemplateHandler.LoadTemplate("DateRangeSelect.html");

        //SetGUIByLang(ref dateRangeTpl);

        TemplateHandler.Assign(ref dateRangeTpl, "{CurrentPos}", currentPos);
        TemplateHandler.Assign(ref dateRangeTpl, "{DayNoReset}", noDayReset.ToString());
        TemplateHandler.Assign(ref dateRangeTpl, "{Fdate}", fdateId);
        TemplateHandler.Assign(ref dateRangeTpl, "{Tdate}", tdateId);
        TemplateHandler.Assign(ref dateRangeTpl, "{submit}", Resources.Lang.submit);
        TemplateHandler.Assign(ref dateRangeTpl, "{today_now}", DateTime.Now.ToShortDateString());
        TemplateHandler.Assign(ref dateRangeTpl, "{yesterday}", DateTime.Now.AddDays(-1).ToShortDateString());
        TemplateHandler.Assign(ref dateRangeTpl, "tdatetext", Resources.Lang.to);
        string strShowToday = blShowToday ? "" : "hidden";
        TemplateHandler.Assign(ref dateRangeTpl, "{hide}", strShowToday);
        string fdateStr = showTime ? DateFormat.ShortDateFormat(fdate) : DateFormat.ShortDateFormat(fdate);
        string tdateStr = showTime ? DateFormat.ShortDateFormat(tdate) : DateFormat.ShortDateFormat(tdate);
        if (showtdate == false)
        {
            TemplateHandler.RemoveBlock(ref dateRangeTpl, "limit-todate");
            TemplateHandler.Assign(ref dateRangeTpl, "fdatetext", Resources.Lang.date);
        }
        else
        {
            TemplateHandler.Assign(ref dateRangeTpl, "fdatetext", Resources.Lang.from);
        }
        TemplateHandler.Assign(ref dateRangeTpl, "{FromDateCal}", GenCalendarControlNoLimit(fdateId, currentPos, fdateId + "_tg", fdateStr, noJs, showTime, true, false));
        TemplateHandler.Assign(ref dateRangeTpl, "{ToDateCal}", GenCalendarControlNoLimit(tdateId, currentPos, tdateId + "_tg", tdateStr, true, showTime, true, false));
        return dateRangeTpl;

    }
    /// <summary>
    /// @Phuong.Nguyen
    /// Create date/time range selector
    /// </summary>
    /// <param name="currentPos"></param>
    /// <param name="noDayReset"></param>
    /// <param name="fdateId"></param>
    /// <param name="tdateId"></param>
    /// <param name="fdate"></param>
    /// <param name="tdate"></param>
    /// <param name="noJs"></param>
    /// <param name="showTime"></param>
    /// <param name="blShowToday"></param>
    /// <param name="showtdate"></param>
    /// <param name="showOtherDays"></param>
    /// <param name="useMultiLang"></param>
    /// <returns></returns>
    public static string CreateDateRangeSelector(string currentPos, int noDayReset, string fdateId, string tdateId, DateTime fdate, DateTime tdate, bool noJs, bool showTime, bool blShowToday, bool showtdate, bool showOtherDays, bool useMultiLang)
    {

        string dateRangeTpl = TemplateHandler.LoadTemplate("DateRangeSelect.html");

        //SetGUIByLang(ref dateRangeTpl);

        TemplateHandler.Assign(ref dateRangeTpl, "{CurrentPos}", currentPos);
        TemplateHandler.Assign(ref dateRangeTpl, "{DayNoReset}", noDayReset.ToString());
        TemplateHandler.Assign(ref dateRangeTpl, "{Fdate}", fdateId);
        TemplateHandler.Assign(ref dateRangeTpl, "{Tdate}", tdateId);
        TemplateHandler.Assign(ref dateRangeTpl, "{submit}", Resources.Lang.submit);
        TemplateHandler.Assign(ref dateRangeTpl, "{today_now}", DateTime.Now.ToShortDateString());
        TemplateHandler.Assign(ref dateRangeTpl, "{yesterday}", DateTime.Now.AddDays(-1).ToShortDateString());
        TemplateHandler.Assign(ref dateRangeTpl, "tdatetext", Resources.Lang.to);
        string strShowToday = blShowToday ? "" : "hidden";
        TemplateHandler.Assign(ref dateRangeTpl, "{hide}", strShowToday);
        string fdateStr = showTime ? DateFormat.ShortDateFormat(fdate) : DateFormat.ShortDateFormat(fdate);
        string tdateStr = showTime ? DateFormat.ShortDateFormat(tdate) : DateFormat.ShortDateFormat(tdate);
        if (showtdate == false)
        {
            TemplateHandler.RemoveBlock(ref dateRangeTpl, "limit-todate");
            TemplateHandler.Assign(ref dateRangeTpl, "fdatetext", Resources.Lang.date);
        }
        else
        {
            TemplateHandler.Assign(ref dateRangeTpl, "fdatetext", Resources.Lang.from);
        }
        TemplateHandler.Assign(ref dateRangeTpl, "{FromDateCal}", GenCalendarControl(fdateId, currentPos, fdateId + "_tg", fdateStr, noJs, showTime, showOtherDays, useMultiLang));
        TemplateHandler.Assign(ref dateRangeTpl, "{ToDateCal}", GenCalendarControl(tdateId, currentPos, tdateId + "_tg", tdateStr, true, showTime, showOtherDays, useMultiLang));
        TemplateHandler.Assign(ref dateRangeTpl, "{Fdate-value}", fdate.ToShortDateString());
        TemplateHandler.Assign(ref dateRangeTpl, "{Tdate-value}", tdate.ToShortDateString());
        return dateRangeTpl;

    }

    public static string CreateDateRangeSelector(string currentPos, int noDayReset, string fdateId, string tdateId, DateTime fdate, DateTime tdate, bool showTime, bool blShowToday, bool showtdate, bool showOtherDays, bool useMultiLang)
    {

        return CreateDateRangeSelector(currentPos, noDayReset, fdateId, tdateId, fdate, tdate, false, showTime, blShowToday,
                                 showtdate, showOtherDays, useMultiLang);

    }


    /// <summary>
    /// @Khoa.Tran
    /// </summary>
    /// <param name="fdateId"></param>
    /// <param name="tdateId"></param>
    /// <returns></returns>
    /// 
    //ismatchwinloss = true --> Match winloss and Match winloss detail Form
    //ismatchwinloss = false --> All report which have not match winloss and match winloss detail
    public static string CreateDateRangeSelector(string fdateId, string tdateId, bool blShowToday, DateTime fdateSelect, DateTime tdateSelect, bool ismatchwinloss)
    {

        string dateRangeTpl = TemplateHandler.LoadTemplate("DateRangeSelect.html");

        //SetGUIByLang(ref dateRangeTpl);

        TemplateHandler.Assign(ref dateRangeTpl, "{CurrentPos}", "");
        TemplateHandler.Assign(ref dateRangeTpl, "{DayNoReset}", "1");
        TemplateHandler.Assign(ref dateRangeTpl, "{Fdate}", fdateId);
        TemplateHandler.Assign(ref dateRangeTpl, "{Tdate}", tdateId);
        TemplateHandler.Assign(ref dateRangeTpl, "{submit}", Resources.Lang.submit);
        TemplateHandler.Assign(ref dateRangeTpl, "{today_now}", Resources.Lang.today);
        TemplateHandler.Assign(ref dateRangeTpl, "{yesterday}", Resources.Lang.yesterday);
        TemplateHandler.Assign(ref dateRangeTpl, "fdatetext", Resources.Lang.from);
        TemplateHandler.Assign(ref dateRangeTpl, "tdatetext", Resources.Lang.to);
        string strShowToday = blShowToday ? "" : "hidden";
        TemplateHandler.Assign(ref dateRangeTpl, "{hide}", strShowToday);
        string fdate = Age_BalDaySel(fdateId, fdateSelect, ismatchwinloss);
        string tdate = Age_BalDaySel(tdateId, tdateSelect, ismatchwinloss);
        TemplateHandler.Assign(ref dateRangeTpl, "{FromDateCal}", fdate);
        TemplateHandler.Assign(ref dateRangeTpl, "{ToDateCal}", tdate);
        return dateRangeTpl;

    }

    /// <summary>
    /// @Khoa.Tran
    /// </summary>
    /// <param name="fdateId"></param>
    /// <param name="fdateSelect"></param>
    /// <returns></returns>
    public static string CreateDateRangeSelector(string fdateId, DateTime fdateSelect, bool ismatchwinloss)
    {

        string fdate = Age_BalDaySel(fdateId, fdateSelect, ismatchwinloss);
        return fdate;

    }

    public static string CreateDateRangeSelector(string currentPos, int noDayReset, string fdateId, string tdateId, DateTime fdate, DateTime tdate, bool blShowToday)
    {

        return CreateDateRangeSelector(currentPos, noDayReset, fdateId, tdateId, fdate, tdate, false, false, blShowToday);

    }
    public static string CreateDateRangeSelectorOfMessage(string currentPos, int noDayReset, string fdateId, string tdateId, DateTime fdate, DateTime tdate, bool showTime, bool blShowToday, bool showtdate, bool showOtherDays, bool useMultiLang)
    {

        return CreateDateRangeSelectorOfMessageInDetail(currentPos, noDayReset, fdateId, tdateId, fdate, tdate, false, showTime, blShowToday,
                                showtdate, showOtherDays, useMultiLang);

    }
    public static string CreateDateRangeSelector(string currentPos, int noDayReset, string fdateId, string tdateId, DateTime fdate, DateTime tdate, bool blShowToday, bool showtdate)
    {

        return CreateDateRangeSelector(currentPos, noDayReset, fdateId, tdateId, fdate, tdate, false, false, blShowToday, showtdate);

    }
    public static string CreateDateRangeSelector(string currentPos, int noDayReset, string fdateId, string tdateId)
    {

        return CreateDateRangeSelector(currentPos, noDayReset, fdateId, tdateId, DateTime.Now.AddDays(-noDayReset), DateTime.Now, false);

    }
    public static string CreateDateRangeSelector(string currentPos, string fdateId, string tdateId)
    {

        return CreateDateRangeSelector(currentPos, 1, fdateId, tdateId);

    }
    public static Hashtable GetAccessPage(string strAccess)
    {
        Hashtable hashAccess = new Hashtable();
        hashAccess.Add("isCreate", false);
        hashAccess.Add("isUpdate", false);
        hashAccess.Add("isTransfer", false);
        hashAccess.Add("isViewTT", false);
        hashAccess.Add("isViewR", false);

        if (!string.IsNullOrEmpty(strAccess))
        {
            if (strAccess.IndexOf("[A]") != -1)
            {
                hashAccess["isCreate"] = true;
            }
            else
            {
                hashAccess["isCreate"] = false;
            }

            if (strAccess.IndexOf("[B]") != -1)
            {
                hashAccess["isUpdate"] = true;
            }
            else
            {
                hashAccess["isUpdate"] = false;
            }

            if (strAccess.IndexOf("[C]") != -1)
            {
                hashAccess["isTransfer"] = true;
            }
            else
            {
                hashAccess["isTransfer"] = false;
            }

            if (strAccess.IndexOf("[D]") != -1)
            {
                hashAccess["isViewTT"] = true;
            }
            else
            {
                hashAccess["isViewTT"] = false;
            }

            if (strAccess.IndexOf("[E]") != -1)
            {
                hashAccess["isViewR"] = true;
            }
            else
            {
                hashAccess["isViewR"] = false;
            }
        }

        return hashAccess;
    }
    //----Paging
    public static string GenPagingFooter(string url, int pageNum, int cpage, int colsNo, string site)
    {

        string pageIndexTpl = TemplateHandler.LoadTemplate("PageIndex.html");
        string footerHtml = TemplateHandler.LoadTemplate("Paging.html");
        string GotoPageHtml = TemplateHandler.LoadTemplate("GotoPage.html");
        TemplateHandler.Assign(ref footerHtml, "{CurrentPage}", cpage.ToString());
        TemplateHandler.Assign(ref footerHtml, "{Colspan}", colsNo.ToString());
        TemplateHandler.Assign(ref pageIndexTpl, "Site", site);
        string pageList = string.Empty;

        int startpage, endpage;
        startpage = cpage - 3;
        if (startpage < 1)
            startpage = 0;

        endpage = startpage + 5;
        if (endpage > pageNum)
            endpage = pageNum;

        if ((endpage - startpage) < 5 && (cpage < pageNum) && (pageNum - 5 > 0))
        {
            startpage = pageNum - 5;
            endpage = pageNum;
        }

        if ((endpage + 1) == pageNum)
            endpage = pageNum;

        if (cpage >= 4)
        {
            string pageIndex = (string)pageIndexTpl.Clone();
            TemplateHandler.Assign(ref pageIndex, "{Url}", url);
            TemplateHandler.Assign(ref pageIndex, "{CurrentPage}", string.Empty);
            TemplateHandler.Assign(ref pageIndex, "{PageText}", "1");
            TemplateHandler.Assign(ref pageIndex, "{Page}", "1");
            if ((cpage - 2) == 2)
                pageList += pageIndex;
            else
                pageList += pageIndex + " ... ";
        }

        if ((endpage == cpage) && (endpage - 5) > 0)
            startpage = endpage - 5;

        for (int i = startpage; i < endpage; ++i)
        {
            string pageIndex = (string)pageIndexTpl.Clone();

            TemplateHandler.Assign(ref pageIndex, "{Url}", url);
            if (cpage == (i + 1))
            {
                pageIndex = "<strong>" + cpage.ToString() + "</strong>";
            }
            else
            {
                TemplateHandler.Assign(ref pageIndex, "{CurrentPage}", string.Empty);
                TemplateHandler.Assign(ref pageIndex, "{PageText}", "" + (i + 1));
                TemplateHandler.Assign(ref pageIndex, "{Page}", "" + (i + 1));
            }
            pageList += pageIndex;
        }

        if ((endpage + 1) < pageNum)
        {
            string pageIndex = (string)pageIndexTpl.Clone();
            TemplateHandler.Assign(ref pageIndex, "{Url}", url);
            TemplateHandler.Assign(ref pageIndex, "{CurrentPage}", string.Empty);
            TemplateHandler.Assign(ref pageIndex, "{PageText}", " " + pageNum);
            TemplateHandler.Assign(ref pageIndex, "{Page}", " " + pageNum);
            pageList += " ... " + pageIndex;
        }

        if (pageList.Length > 0)
        {
            TemplateHandler.Assign(ref footerHtml, "{PageList}", pageList.Remove(pageList.Length - 1));
            TemplateHandler.Assign(ref footerHtml, "{GotoPage}", GotoPageHtml);
            TemplateHandler.Assign(ref footerHtml, "{Url}", url);
        }
        else
        {
            TemplateHandler.Assign(ref footerHtml, "{PageList}", "<strong>1</strong>");
            TemplateHandler.Assign(ref footerHtml, "{Url}", "");
        }

        if (pageNum <= 1) return string.Empty;

        return footerHtml;

    }
    /*
     * 20090214@AnhPham
     * Task: Overload to center position of paging section
    */
    public static string GenPagingFooter(string url, int pageNum, int cpage, int colsNo, string site, int position)
    {

        string condition = @"class=""c""";
        string leftItem = @"class=""l""", centerItem = @"class=""c""";
        string pageIndexTpl = TemplateHandler.LoadTemplate("PageIndex.html");
        string footerHtml = TemplateHandler.LoadTemplate("Paging.html");
        // center position of paging section - right center is default
        if (position == 1) //left
        {
            int firstIndex = footerHtml.IndexOf(condition);
            footerHtml = footerHtml.Remove(firstIndex, condition.Length);
            footerHtml = footerHtml.Insert(firstIndex, leftItem);
        }
        else if (position == 2)//center
        {
            int firstIndex = footerHtml.IndexOf(condition);
            footerHtml = footerHtml.Remove(firstIndex, condition.Length);
            footerHtml = footerHtml.Insert(firstIndex, centerItem);
        }
        string GotoPageHtml = TemplateHandler.LoadTemplate("GotoPage.html");
        TemplateHandler.Assign(ref footerHtml, "{CurrentPage}", cpage.ToString());
        TemplateHandler.Assign(ref footerHtml, "{Colspan}", colsNo.ToString());
        TemplateHandler.Assign(ref pageIndexTpl, "Site", site);
        string pageList = string.Empty;

        int startpage, endpage;
        startpage = cpage - 3;
        if (startpage < 1)
            startpage = 0;

        endpage = startpage + 5;
        if (endpage > pageNum)
            endpage = pageNum;

        if ((endpage - startpage) < 5 && (cpage < pageNum) && (pageNum - 5 > 0))
        {
            startpage = pageNum - 5;
            endpage = pageNum;
        }

        if ((endpage + 1) == pageNum)
            endpage = pageNum;

        if (cpage >= 4)
        {
            string pageIndex = (string)pageIndexTpl.Clone();
            TemplateHandler.Assign(ref pageIndex, "{Url}", url);
            TemplateHandler.Assign(ref pageIndex, "{CurrentPage}", string.Empty);
            TemplateHandler.Assign(ref pageIndex, "{PageText}", "1");
            TemplateHandler.Assign(ref pageIndex, "{Page}", "1");
            if ((cpage - 2) == 2)
                pageList += pageIndex;
            else
                pageList += pageIndex + " ... ";
        }

        if ((endpage == cpage) && (endpage - 5) > 0)
            startpage = endpage - 5;

        for (int i = startpage; i < endpage; ++i)
        {
            string pageIndex = (string)pageIndexTpl.Clone();

            TemplateHandler.Assign(ref pageIndex, "{Url}", url);
            if (cpage == (i + 1))
            {
                pageIndex = "<strong>" + cpage.ToString() + "</strong>";
            }
            else
            {
                TemplateHandler.Assign(ref pageIndex, "{CurrentPage}", string.Empty);
                TemplateHandler.Assign(ref pageIndex, "{PageText}", "" + (i + 1));
                TemplateHandler.Assign(ref pageIndex, "{Page}", "" + (i + 1));
            }
            pageList += pageIndex;
        }

        if ((endpage + 1) < pageNum)
        {
            string pageIndex = (string)pageIndexTpl.Clone();
            TemplateHandler.Assign(ref pageIndex, "{Url}", url);
            TemplateHandler.Assign(ref pageIndex, "{CurrentPage}", string.Empty);
            TemplateHandler.Assign(ref pageIndex, "{PageText}", " " + pageNum);
            TemplateHandler.Assign(ref pageIndex, "{Page}", " " + pageNum);
            pageList += " ... " + pageIndex;
        }

        if (pageList.Length > 0)
        {
            TemplateHandler.Assign(ref footerHtml, "{PageList}", pageList.Remove(pageList.Length - 1));
            TemplateHandler.Assign(ref footerHtml, "{GotoPage}", GotoPageHtml);
            TemplateHandler.Assign(ref footerHtml, "{Url}", url);
        }
        else
        {
            TemplateHandler.Assign(ref footerHtml, "{PageList}", "<strong>1</strong>");
            TemplateHandler.Assign(ref footerHtml, "{Url}", "");
        }

        if (pageNum <= 1) return string.Empty;

        return footerHtml;

    }

    public static string GenPagingFooter(string url, int pageNum, int cpage, int colsNo)
    {

        string pageIndexTpl = TemplateHandler.LoadTemplate("PageIndex.html");
        string footerHtml = TemplateHandler.LoadTemplate("Paging.html");
        string GotoPageHtml = TemplateHandler.LoadTemplate("GotoPage.html");
        TemplateHandler.Assign(ref footerHtml, "{CurrentPage}", cpage.ToString());
        TemplateHandler.Assign(ref footerHtml, "{Colspan}", colsNo.ToString());

        //nqminh changed
        //TemplateHandler.Assign(ref pageIndexTpl, "Site", Sunplus.Agent.Model.Entities.UserSession.Current.SiteName);  
        TemplateHandler.Assign(ref pageIndexTpl, "Site", AppSettings.SiteLevel);

        string pageList = string.Empty;

        int startpage, endpage;
        startpage = cpage - 3;
        if (startpage < 1)
            startpage = 0;

        endpage = startpage + 5;
        if (endpage > pageNum)
            endpage = pageNum;

        if ((endpage - startpage) < 5 && (cpage < pageNum) && (pageNum - 5 > 0))
        {
            startpage = pageNum - 5;
            endpage = pageNum;
        }

        if ((endpage + 1) == pageNum)
            endpage = pageNum;

        if (cpage >= 4)
        {
            string pageIndex = (string)pageIndexTpl.Clone();
            TemplateHandler.Assign(ref pageIndex, "{Url}", url);
            TemplateHandler.Assign(ref pageIndex, "{CurrentPage}", string.Empty);
            TemplateHandler.Assign(ref pageIndex, "{PageText}", "1");
            TemplateHandler.Assign(ref pageIndex, "{Page}", "1");
            if ((cpage - 2) == 2)
                pageList += pageIndex;
            else
                pageList += pageIndex + " ... ";
        }

        if ((endpage == cpage) && (endpage - 5) > 0)
            startpage = endpage - 5;

        for (int i = startpage; i < endpage; ++i)
        {
            string pageIndex = (string)pageIndexTpl.Clone();
            TemplateHandler.Assign(ref pageIndex, "{Url}", url);
            if (cpage == (i + 1))
            {
                pageIndex = "<strong>" + cpage.ToString() + "</strong>";
            }
            else
            {
                TemplateHandler.Assign(ref pageIndex, "{CurrentPage}", string.Empty);
                TemplateHandler.Assign(ref pageIndex, "{PageText}", "" + (i + 1));
                TemplateHandler.Assign(ref pageIndex, "{Page}", "" + (i + 1));
            }
            pageList += pageIndex;
        }

        if ((endpage + 1) < pageNum)
        {
            string pageIndex = (string)pageIndexTpl.Clone();
            TemplateHandler.Assign(ref pageIndex, "{Url}", url);
            TemplateHandler.Assign(ref pageIndex, "{CurrentPage}", string.Empty);
            TemplateHandler.Assign(ref pageIndex, "{PageText}", " " + pageNum);
            TemplateHandler.Assign(ref pageIndex, "{Page}", " " + pageNum);
            pageList += " ... " + pageIndex;
        }

        if (pageList.Length > 0)
        {
            TemplateHandler.Assign(ref footerHtml, "{PageList}", pageList.Remove(pageList.Length - 1));
            TemplateHandler.Assign(ref footerHtml, "{GotoPage}", GotoPageHtml);
            TemplateHandler.Assign(ref footerHtml, "{Url}", url);
        }
        else
        {
            TemplateHandler.Assign(ref footerHtml, "{PageList}", "<strong>1</strong>");
            TemplateHandler.Assign(ref footerHtml, "{Url}", "");
        }

        if (pageNum <= 1) return string.Empty;

        return footerHtml;

    }

    public static string PageList0101(int intLastPage, int intFromPage, int intToPage, int intCurrentPage, string strNavigatePage, string target)
    {
        string strTarget = "";
        int intMaxPage = 10;
        bool isAppendLeft = false;
        if (target != "")
        {
            strTarget = "target=" + target;
        }
        StringBuilder s = new StringBuilder();

        s.Append("<table id='pagelink' rows='1' cols='1' width='97%'><tr><td><font size='2' color='black' face='Verdana, Arial,Helvetica, sans-serif'>");
        //First Page
        if (intCurrentPage > 1)
        {
            s.Append("<a class='main' alt='Go to first page' " + strTarget + "  href='javascript:GoToPage(1)'>" + Resources.Lang.first + " </a>&nbsp;&nbsp;");
        }
        int intPageNext = intCurrentPage + 1;
        int intPagePrevious = intCurrentPage - 1;
        if (intPagePrevious < 1) intPagePrevious = 1;
        if (intPageNext > intLastPage) intPageNext = intLastPage;
        if (intCurrentPage > 1)
        {
            s.Append("<a class='main' " + strTarget + " href='javascript:GoToPage(" + intPagePrevious.ToString() + ")'>" + Resources.Lang.previous + " </a>&nbsp;");
        }
        //Link list
        if (intLastPage < intMaxPage)
        {
            for (int i = 1; i <= intLastPage; i++)
            {
                if (i == intCurrentPage)
                {
                    s.Append("[<font color='red'><b>" + i.ToString() + "</b></font>]&nbsp;");
                }
                else
                {
                    s.Append("<a class='main' " + strTarget + " href='javascript:GoToPage(" + i.ToString() + ")'>" + i.ToString() + "</a>&nbsp;");
                }
            }
        }
        else
        {
            for (int i = 1; i <= intMaxPage; i++)
            {
                if (intCurrentPage - i > 1 && intMaxPage < intLastPage)
                {
                    intMaxPage = (intMaxPage < intLastPage) ? (intMaxPage + 1) : intLastPage;
                    isAppendLeft = true;
                }
                else
                {
                    //Appen ... to the left
                    if (isAppendLeft)
                    {
                        s.Append("...&nbsp;");
                        isAppendLeft = false;
                    }
                    if (i == intCurrentPage)
                    {
                        s.Append("[<font color='red'><b>" + i.ToString() + "</b></font>]&nbsp;");
                    }
                    else
                    {
                        s.Append("<a class='main' " + strTarget + " href='javascript:GoToPage(" + i.ToString() + ")'>" + i.ToString() + "</a>&nbsp;");
                    }
                    //Appen ... to the right
                    if (intMaxPage < intLastPage && i == intMaxPage)
                    {
                        s.Append("...&nbsp;");
                    }
                }
            }
        }
        //Next Page
        if (intCurrentPage < intLastPage)
        {
            s.Append("&nbsp;&nbsp;&nbsp;<a class='main' alt='Go to last page' " + strTarget + " href='javascript:GoToPage(" + intPageNext.ToString() + ")'>" + Resources.Lang.next + " </a>&nbsp;");
            s.Append("&nbsp;&nbsp;<a class='main' alt='Go to last page' " + strTarget + " href='javascript:GoToPage(" + intLastPage.ToString() + ")'>" + Resources.Lang.last + "</a>&nbsp;");
        }
        //Last Page

        s.Append("</font></td></tr></table>");
        s = s.Append("<script>");
        s = s.Append(" function GoToPage(mypage){ ");
        s = s.Append("var myajax= new Ajax.Request('" + strNavigatePage + "',{");
        s = s.Append("method:'get',");
        if (AppSettings.IsAgentSite)
        {
            s = s.Append("parameters:'fdate=' + $('fdate').value + '&tdate=' + $('tdate').value + '&page=' + mypage,");
        }
        else
        {
            s = s.Append("parameters:'page=' + mypage,");
        }
        s = s.Append("onComplete:View_Data");
        s = s.Append("});");
        s = s.Append(" }");

        s = s.Append("function View_Data(data)");
        s = s.Append("{");
        s = s.Append("$('divContent').innerHTML=data.responseText;");
        s = s.Append("}");
        s = s.Append(" </script>");

        return s.ToString();
    }
    //------------
    public static string PageList01(int intLastPage, int intFromPage, int intToPage, int intCurrentPage, string strNavigatePage, string target, string strPage, string strOf)
    {
        string strTarget = "";
        int intMaxPage = 11;
        bool isAppendLeft = false;
        if (target != "")
        {
            strTarget = "target=" + target;
        }
        StringBuilder s = new StringBuilder();
        s.Append("<table id='pagelink' rows='1' cols='1' width='97%'><tr><td><font size='2' color='black' face='Verdana, Arial,Helvetica, sans-serif'>");
        int intPageNext = intCurrentPage + 1;
        int intPagePrevious = intCurrentPage - 1;
        if (intPagePrevious < 1) intPagePrevious = 1;
        if (intPageNext > intLastPage) intPageNext = intLastPage;
        //Link list    
        if (intLastPage < intMaxPage)
        {
            for (int i = 1; i <= intLastPage; i++)
            {
                if (i == intCurrentPage)
                {
                    s.Append("[<font color='red'><b>" + i.ToString() + "</b></font>]&nbsp;");
                }
                else
                {
                    s.Append("<a class='paging' " + strTarget + " href='javascript:GoToPage(" + i.ToString() + ")'>" + i.ToString() + "</a>&nbsp;");
                }
            }
        }
        else
        {
            for (int i = 1; i <= intMaxPage; i++)
            {
                if (intCurrentPage - i >= 6 && intMaxPage < intLastPage)
                {
                    intMaxPage = (intMaxPage < intLastPage) ? (intMaxPage + 1) : intLastPage;
                    isAppendLeft = true;
                }
                else
                {
                    //Appen ... to the left
                    if (isAppendLeft)
                    {
                        s.Append("<a class='paging' " + strTarget + " href='javascript:GoToPage(" + 1.ToString() + ")'>" + 1.ToString() + "</a>&nbsp;");
                        s.Append("...&nbsp;");
                        isAppendLeft = false;
                    }
                    if (i == intCurrentPage)
                    {
                        s.Append("[<font color='red'><b>" + i.ToString() + "</b></font>]&nbsp;");
                    }
                    else
                    {
                        s.Append("<a class='paging' " + strTarget + " href='javascript:GoToPage(" + i.ToString() + ")'>" + i.ToString() + "</a>&nbsp;");
                    }
                    //Appen ... to the right
                    if (intMaxPage < intLastPage && i == intMaxPage)
                    {
                        s.Append("...&nbsp;");
                        s.Append("<a class='paging' " + strTarget + " href='javascript:GoToPage(" + intLastPage.ToString() + ")'>" + intLastPage.ToString() + "</a>&nbsp;");
                    }
                }
            }
        }
        //Page of
        s.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a class='main' href='javascript:JumpTo()'>" + intCurrentPage.ToString() + " " + strOf + " " + intLastPage.ToString() + " " + strPage + "</a>\t");
        s.Append("</font></td></tr></table>");
        //javascript
        s = s.Append("<script>");
        s = s.Append(" function JumpTo(){ ");
        s = s.Append(" var page = prompt('Please input Page Number: ','');");
        s = s.Append(" if (page !== null && !isNaN(page) && page > 0)");
        s = s.Append(" GoToPage(page);");
        s = s.Append(" }");

        s = s.Append(" function GoToPage(mypage){ ");
        s = s.Append(" window.location.href='" + strNavigatePage + "&page=' + mypage");
        s = s.Append(" }");
        s = s.Append(" </script>");
        return s.ToString();
    }

    public static string PageList01(int intLastPage, int intFromPage, int intToPage, int intCurrentPage, string strNavigatePage, string target)
    {
        string strTarget = "";
        if (target != "")
        {
            strTarget = "target=" + target;
        }
        StringBuilder s = new StringBuilder();
        s.Append("<table id='pagelink' rows='1' cols='1' width='97%'><tr><td><font size='2' color='black' face='Verdana, Arial,Helvetica, sans-serif'>");
        //Link list          
        for (int i = 1; i <= intLastPage; i++)
        {
            if (i == intCurrentPage)
            {
                s.Append("[<font color='red'><b>" + i.ToString() + "</b></font>]&nbsp;");
            }
            else
            {
                s.Append("<a class='paging' " + strTarget + " href='javascript:GoToPage(" + i.ToString() + ")'>" + i.ToString() + "</a>&nbsp;");
            }
        }
        s.Append("</font></td></tr></table>");
        //javascript
        s = s.Append("<script>");
        s = s.Append(" function GoToPage(mypage){ ");
        s = s.Append(" window.location.href='" + strNavigatePage + "&page=' + mypage");
        s = s.Append(" }");
        s = s.Append(" </script>");

        return s.ToString();
    }


    #region GetSportTitle
    public static void GetSportTitle(ref string tpl, bool isEditMin)
    {

        TemplateHandler.Assign(ref tpl, "soccer-text", Resources.Lang.soccer);
        TemplateHandler.Assign(ref tpl, "numbergame-text", Resources.Lang.numbergame);
        TemplateHandler.Assign(ref tpl, "basket-text", Resources.Lang.Basketball);
        TemplateHandler.Assign(ref tpl, "football-text", Resources.Lang.Football);
        TemplateHandler.Assign(ref tpl, "tennis-text", Resources.Lang.Tennis);
        TemplateHandler.Assign(ref tpl, "baseball-text", Resources.Lang.Baseball);
        TemplateHandler.Assign(ref tpl, "golf-text", Resources.Lang.Golf);
        TemplateHandler.Assign(ref tpl, "moto-text", Resources.Lang.MotorSports);
        TemplateHandler.Assign(ref tpl, "othersports-text", Resources.Lang.OtherSports);
        TemplateHandler.Assign(ref tpl, "mixsports-text", Resources.Lang.mixsports);


        if (isEditMin)
        {
            if (UserPermission.IsShowNumberGame)
            {
                TemplateHandler.Assign(ref tpl, "copy-all-text", Resources.Lang.lblCopyAllPTfromHdpMin);
                TemplateHandler.Assign(ref tpl, "copy-basket-text", Resources.Lang.lblCopyBasketHdpMin);
            }
            else
            {
                TemplateHandler.Assign(ref tpl, "copy-all-text", Resources.Lang.lblCopyAllPTfromHdpMinNoNumberGame );
                TemplateHandler.Assign(ref tpl, "copy-basket-text", Resources.Lang.lblCopyBasketHdpMinNoNumberGame);
            }
        }
        else
        {
            if (UserPermission.IsShowNumberGame)
            {
                TemplateHandler.Assign(ref tpl, "copy-all-text", Resources.Lang.lblCopyAllPTfromHdp);
                TemplateHandler.Assign(ref tpl, "copy-basket-text", Resources.Lang.lblCopyBasketHdp);
            }
            else
            {
                TemplateHandler.Assign(ref tpl, "copy-all-text", Resources.Lang.lblCopyAllPTfromHdpNoNumberGame);
                TemplateHandler.Assign(ref tpl, "copy-basket-text", Resources.Lang.lblCopyBasketHdpNoNumberGame);
            }
        }

        TemplateHandler.Assign(ref tpl, "financial-text", Resources.Lang.Financial);
    }
    #endregion

    /// <summary>
    /// Age_BalDaySel
    /// </summary>
    /// <param name="fdateID">fdateID</param>
    /// <param name="tdateID">tdateID</param>
    /// <param name="IsBoth"> if IsBoth = true then show fdate and tdate else only show fdate</param>
    /// <returns></returns>
    public static string Age_BalDaySel(string fdateID, DateTime selectdate, bool ismatchwinloss)
    {
        DateTime fdate = (ismatchwinloss) ? Sunplus.Agent.Model.BL.Reports.ReportsBO.MoveBetsDate : Sunplus.Agent.Model.BL.Reports.ReportsBO.BalanceUpDate;
        fdate = CastAide.AsDateTime(fdate.ToShortDateString());
        DateTime tdate = DateTime.Today;
        string strfdate = "";
        strfdate = "<select id='" + fdateID + "' name='" + fdateID + "'>";
        while (fdate <= tdate)
        {
            if (fdate.ToShortDateString().Equals(selectdate.ToShortDateString()))
                strfdate += " <option selected='selected' value='" + DateFormat.ShortDateFormat(fdate) + "'>" + DateFormat.ShortDateFormat(fdate) + "</option>";
            else
                strfdate += " <option value='" + DateFormat.ShortDateFormat(fdate) + "'>" + DateFormat.ShortDateFormat(fdate) + "</option>";
            fdate = fdate.AddDays(1);
        }
        strfdate += "</select>";
        return strfdate;
    }

    public static string PTDropdownlist(string strId, double minVal, double maxVal, double currentVal)
    {
        string html = String.Empty;
        StringBuilder strBody = new StringBuilder();
        string strSelect = TemplateHandler.LoadTemplate("MemberInfo/DdlMemPT.html");

        TemplateHandler.Assign(ref strSelect, "ddl-Id", strId);
        //Boolean isInRange = false;
        int intToValue = CastAide.AsInt32(maxVal * 100);
        int intFromValue = CastAide.AsInt32(minVal * 100);
        while (intToValue >= intFromValue)
        {
            Double ptValue = CastAide.AsDouble(intToValue) / 100;
            if (ptValue == currentVal)
            {
                //isInRange = true;
                strBody.Append("<option selected='true' value='" + NumberFormat.DecFormat(ptValue, 2) + "'>" + NumberFormat.DecFormat(ptValue, 2) + "</option>");
            }
            else
                strBody.Append("<option value='" + NumberFormat.DecFormat(ptValue, 2) + "'>" + NumberFormat.DecFormat(ptValue, 2) + "</option>");
            intToValue = intToValue - 1;
        }
        html += strBody.ToString();

        TemplateHandler.Assign(ref strSelect, "ddl-val", html);
        return strSelect;
    }

    public static void ExportToExcel(HttpResponse Response, string data, string filename)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=" + filename.Replace(" ", "") + ".xls");
        Response.Charset = "utf-8";
        Response.ContentType = "application/vnd.xls";
        StringBuilder content = new StringBuilder();
        content.AppendLine("<html>");
        content.AppendLine("<head>");
        content.AppendLine("<META http-EQUIV='Content-Type' CONTENT='text/html; charset=unicode-utf8'>");
        content.AppendLine("</head>");

        content.AppendLine("<style>");
        content.AppendLine("body{font-family:Tahoma; font-size:12px; color:#000000; background:#ffffff;}");
        content.AppendLine("td,table{border-collapse:collapse;border-color:#7C98A4; border-style:Solid; border-width:thin;}");
        content.AppendLine(".league{color: #000000; background: #aacccc;}");
        content.AppendLine("tr.RptHeader, td.RptHeader,tr.RptTitle,td.RptTitle, tr.RptTB, tr.RptFooter{color:#000000; background:#aaccee;font-size:12px;font-weight:bold}");
        content.AppendLine("td{text-align:center;font-size:12px;}");
        content.AppendLine("font, div, span{font-size:12px;text-decoration:none;}");
        content.AppendLine(".bl_underdog{font-weight:bold}");
        content.AppendLine(".bl_match{white-space: nowrap; width: 200px}");
        content.AppendLine(".bl_favorite{color: red;font-weight:bold}");
        content.AppendLine("a{font-size:12px;text-decoration: none;}");
        content.AppendLine(".r,td.r{text-align:right;font-size:12px;}");
        content.AppendLine(".l,td.l{text-align:left;font-size:12px;}");
        content.AppendLine(".c,td.c{text-align:center;font-size:12px;}");
        content.AppendLine(".endTbl{border-color:#ffffff;}");
        content.AppendLine("</style>");

        string strRpt = TemplateHandler.CopyBlock(data, "excel-export");
        TemplateHandler.RemoveBlock(ref strRpt, "not-excel-export");

        content.AppendLine(strRpt);
        content.Append("<table class='endTbl'><tr><td></td></tr></table>");
        content.Replace("href", "onclick");
        content.AppendLine("</html>");

        string strContent = content.ToString();
        TemplateHandler.RemoveComment(ref strContent);

        Response.Write(strContent);
        Response.End();
    }
    public static string ScriptText(string scriptContent)
    {
        string _nl = Environment.NewLine;
        return "<script type='text/javascript'>" + _nl + scriptContent + _nl + "</script>" + _nl;
    }

    public static string NumberOdds(object dcNum)
    {
        return NumberMoney(dcNum);
    }
    public static string NumberMoney(object dcNumMoney)
    {
        decimal dcResult = CastAide.AsDecimal(dcNumMoney);
        return (dcResult >= 0) ? "<span class='bl_underdog'>" + FormatHandler.FormatNumber3(dcResult, Sunplus.Agent.Model.Entities.UserSession.Current.Language) + "</span>" : "<span class='bl_favorite'>" + FormatHandler.FormatNumber3(dcResult, Sunplus.Agent.Model.Entities.UserSession.Current.Language) + "</span>";
    }

    public static string NumberReport(object dcNum)
    {
        decimal dcResult = CastAide.AsDecimal(dcNum);
        return (dcResult >= 0) ? "<span class='bl_underdog'>" + NumberFormat.DecFormat(dcResult, 2) + "</span>" : "<span class='bl_favorite'>" + NumberFormat.DecFormat(dcResult, 2) + "</span>";
    }
    public static string NumberTransfer(object Num)
    {
        decimal dcnum = CastAide.AsDecimal(Num);
        return (dcnum >= 0) ? "<span class='trans_underdog'>" + FormatHandler.FormatNumber2(dcnum, Sunplus.Agent.Model.Entities.UserSession.Current.Language) + "</span>" : "<span class='trans_favorite'>" + FormatHandler.FormatNumber2(dcnum, Sunplus.Agent.Model.Entities.UserSession.Current.Language) + "</span>";
    }

    public static string NotAuthorized(string msg)
    {
        if (msg.Contains("The stored procedure") || msg.Contains("doesn't exist."))
            msg = Resources.Lang.lblNotAuthorized;
        return msg;
    }

    public static void RemoveKey(Hashtable parent, Hashtable child)
    {
        IDictionaryEnumerator Enumulator = child.GetEnumerator();
        while (Enumulator.MoveNext())
        {
            if (parent.ContainsKey(Enumulator.Key))
                parent.Remove(Enumulator.Key);
        }
    }

    public static string RedForNegativeAmount(object Amount, byte decDigits)
    {
        string str = "";
        if (CastAide.AsDecimal(NumberFormat.DecFormat(Amount, decDigits)) < 0)
            str = "<font color='#eb6003'>" + NumberFormat.DecFormat(Amount, decDigits) + "</font>";
        else
            str = NumberFormat.DecFormat(Amount, decDigits);
        return str;
    }

    public static string RedForPositiveAmount(object Amount, byte decDigits)
    {
        string str = "";
        if (CastAide.AsDecimal(NumberFormat.DecFormat(Amount, decDigits)) > 0)
            str = "<font color='#eb6003'>" + NumberFormat.DecFormat(Amount, decDigits) + "</font>";
        else
            str = NumberFormat.DecFormat(Amount, decDigits);
        return str;
    }

    public static string AssignMultipleHidden(Hashtable hash)
    {
        StringBuilder html = new StringBuilder();
        string hiddenTpl = TemplateHandler.LoadTemplate("MemberInfo/HiddenTpl.html");
        string tpl = String.Empty;
        IDictionaryEnumerator enumerator = hash.GetEnumerator();
        while (enumerator.MoveNext())
        {
            tpl = hiddenTpl;
            TemplateHandler.Assign(ref tpl, "hid-id", enumerator.Key);
            TemplateHandler.Assign(ref tpl, "hid-val", enumerator.Value);
            html.Append(tpl);
        }
        return html.ToString();
    }

    //phuong.nguyen
    //check customer's currency is in the allowence HR currency
    #region check currency
    public static bool IsAllowCurrencyHR(string currency)
    {
        try
        {
            Hashtable hValidCurrency = new Hashtable();
            hValidCurrency = GetHRAllowCurrencyList();

            if (!hValidCurrency.ContainsValue(currency) && !hValidCurrency.ContainsKey(currency))
                return false;

        }
        catch
        {
#if DEBUG
            throw;   
#endif
        }
        return true;
    }
    //Get HR currency list 
    public static Hashtable GetHRAllowCurrencyList()
    {
        Hashtable hCurreny = new Hashtable();
        try
        {
            const string key = "hrCurrencyList";
            //hCurreny = (Hashtable)HttpContext.Current.Session[key];            
            hCurreny = (Hashtable)HttpContext.Current.Cache[key]; //nqminh - 27Mar09 - Move to cache
            if (hCurreny == null)
            {
                DbResult dr = DNAide.Data.DbHelper.Execute("Acc_SelHRAllowedCurrency");
                if (dr.ResultSet.Tables.Count > 0)
                {
                    DataTable dt = dr.ResultSet.Tables[0];
                    hCurreny = new Hashtable();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow row1 = dt.Rows[i];
                        hCurreny.Add(row1["currency"].ToString(), row1["currencyName"].ToString());
                    }
                    //HttpContext.Current.Session[key] = hCurreny;                    
                    HttpContext.Current.Cache[key] = hCurreny;
                }
            }
        }
        catch
        {
#if DEBUG
            throw;
#endif
        }
        return hCurreny;
    }


    #endregion

    //phuong.nguyen
    //Generate Commission options
    public static string GetListGroup(double maxValue, double currentVal, int interval)
    {
        StringBuilder html = new StringBuilder();

        int groupValue = Convert.ToInt32(maxValue * 10000);
        if (currentVal > maxValue)
        {
            html.Append("<option>" + currentVal + "</option>");
        }
        while (groupValue >= 0)
        {
            double selValue = Convert.ToDouble(groupValue) / 10000;
            if (selValue == currentVal)
            {
                html.Append("<option selected='true'>" + selValue + "</option>");
            }
            else
                html.Append("<option>" + selValue + "</option>");
            groupValue = groupValue - interval;
        }

        return html.ToString();
    }

    public static string RemoveSpecialChars(string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;
        return str.Replace("\n", "\\n").Replace("\"", "\\\"").Replace("'", "\\'");
    }

    //@Dung Added to get the level name from roleid
    public static string GetLevel(int roleid)
    {
        switch (roleid)
        {
            case 2:
                return "Agent";
            case 3:
                return "Master";
            case 4:
                return "Super";
        }
        return "";
    }

    //nqminh - change to popup stylesheet!
    public static string ReplaceWithPopupStyle(string strAccInfo, int marginLeft, int marginTop)
    {
        strAccInfo = strAccInfo.Replace("id=\"page_main\"", String.Format("style=\"margin-left:{0}px; margin-top:{1}px;\"", marginLeft, marginTop));
        return strAccInfo;
    }
    public static string ReplaceWithPopupStyle(string strAccInfo)
    {
        return ReplaceWithPopupStyle(strAccInfo, 10, 10);
    }
    public static void AssignToStrBuilder(ref StringBuilder strBuilder, string tag, string value)
    {
        string str = strBuilder.ToString();
        TemplateHandler.Assign(ref str, tag, value);
        strBuilder = new StringBuilder(str);
    }
    public static void UpdateCulture() //use for handler .ashx
    {
        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(TplPage.CCulture);
        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(TplPage.CCulture);
    }

    public static string LoadFlashInfo(string flashNamepath, int flashWidth, int flashHeight)
    {
        string flashTpl = TemplateHandler.LoadTemplate("FlashIntro.htm");
        TemplateHandler.Assign(ref flashTpl, "flashnamepath", flashNamepath);
        TemplateHandler.Assign(ref flashTpl, "flashname", Path.GetFileName(flashNamepath));
        TemplateHandler.Assign(ref flashTpl, "flashwidth", flashWidth);
        TemplateHandler.Assign(ref flashTpl, "flashheight", flashHeight);
        TemplateHandler.Assign(ref flashTpl, "langcode", Sunplus.Agent.Model.Entities.UserSession.Current.Language);
        TemplateHandler.Assign(ref flashTpl, "userid", Sunplus.Agent.Model.Entities.UserSession.Current.CustId);
        TemplateHandler.Assign(ref flashTpl, "subid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);
        TemplateHandler.Assign(ref flashTpl, "lbldonotshowagain", CastAide.AsString(HttpContext.GetGlobalResourceObject("Lang", "donotshowagain")));
        return flashTpl;
    }

    //nqminh - check subaccess page
    public static bool SubAccCanAccessFIPage()
    {
        if (Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess == String.Empty) return true; //not subaccount
        Hashtable htbaccpage = (Hashtable)HttpContext.Current.Application["AccessPage"];

        PageUrl pageUrl = (PageUrl)htbaccpage["memberinfo/addeditfinancial.aspx"];
        int i;
        bool canAccess = false;
        for (i = 0; i < pageUrl.RoleRules.Length; i++)
        {
            if (Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.ToString().Contains(((RolePermissionPair)(pageUrl.RoleRules[i])).Permission))
            {
                if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId >= ((RolePermissionPair)pageUrl.RoleRules[i]).RoleId)
                    canAccess = true;
                break;
            }
        }
        return canAccess;
    }

    public static string GetMasterWinLossTitle()
    {
        string titleWinLoss = "";
        string typeGame = CastAide.AsString(HttpContext.Current.Request.QueryString["typegame"], "casinogame");

        switch (typeGame)
        {
            case "casinogame":
                titleWinLoss = Resources.Lang.masterRealTimeWinloss;
                break;
        }
        return titleWinLoss;
    }

    public static string GetAgentWinLossTitle()
    {
        string typeGame = CastAide.AsString(HttpContext.Current.Request.QueryString["typegame"], "casinogame");
        string titleWinLoss = "";
        switch (typeGame)
        {
            case "casinogame":
                titleWinLoss = Resources.Lang.agentRealTimeWinloss;
                break;
        }
        return titleWinLoss;
    }

    public static string GetMemberWinLossTitle()
    {
        string typeGame = CastAide.AsString(HttpContext.Current.Request.QueryString["typegame"], "casinogame");
        string titleWinLoss = "";
        switch (typeGame)
        {
            case "casinogame":
                titleWinLoss = Resources.Lang.memberRealTimeWinloss;
                break;
        }
        return titleWinLoss;
    }
}

public class IBCException : Exception
{
    public IBCException() : base() { }
    public IBCException(string message) : base(message) { }
    public IBCException(string message, Exception ex) : base(message, ex) { }
    protected IBCException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        : base(info, context) { }
}

