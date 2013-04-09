using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNAide.Web;
using DNAide.Utils;
using System.Text;
using System.Data;
using DNAide.Core;
using Sunplus.Agent.Model.Entities;

public partial class zTest_NewMessageForm : TplPage
{
    private string[] _ArrParamValue = new string[4] { "normal", "special", "allSpecial", "private" };
    private const int NORMAL = 0, SPECIAL = 1, OLD_SPECIAL = 2, PRIVATE = 3;

    public int AgentNumber
    {
        get
        {
            if (Sunplus.Agent.Model.Entities.UserSession.Current.AdminId > 0)
            {
                return 0;
            }
            return 1;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "main";


        string pageQuery = _ArrParamValue[NORMAL];
        DateTime tDate = DateTime.Now;
        DateTime fDate = tDate.AddDays(-3);
        int extra = 0;

        ProcessParams(ref pageQuery, ref fDate, ref tDate, ref extra);

        if (pageQuery.Equals(_ArrParamValue[PRIVATE]) && (extra == 1))
        {
            if (Sunplus.Agent.Model.Entities.UserSession.Current.AdminId <= 0)
            {
                Sunplus.Agent.Model.BL.UserMessage.UserMessageBO userMessageBO = new Sunplus.Agent.Model.BL.UserMessage.UserMessageBO(Session.SessionID);
                userMessageBO.SetReadPriMessage();
            }
        }

        this.TplSetContent(GenerateUI(pageQuery, fDate, tDate));
    }

    private void ProcessParams(ref string pageQuery, ref DateTime fDate, ref DateTime tDate, ref int extra)
    {
        string pageQuery2 = string.Empty;
        DateTime tDate2 = DateTime.Now;
        DateTime fDate2 = tDate2.AddDays(-3);
        int extra2 = 0;

        try
        {
            if (!string.IsNullOrEmpty(Request.QueryString["pageQuery"]))
            {
                pageQuery2 = Request.QueryString["pageQuery"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["s_date"]))
            {
                fDate2 = Convert.ToDateTime(Request.QueryString["s_date"]);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["e_date"]))
            {
                tDate2 = Convert.ToDateTime(Request.QueryString["e_date"]);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["extra"]))
            {
                extra2 = Convert.ToInt32(Request.QueryString["extra"]);
            }
        }
        catch (Exception ex)
        {
            Sunplus.Agent.Utilities.Logs.Write(ex.ToString());
        }

        if (pageQuery2.Equals(_ArrParamValue[NORMAL]) ||
                pageQuery2.Equals(_ArrParamValue[SPECIAL]) ||
                pageQuery2.Equals(_ArrParamValue[OLD_SPECIAL]) ||
                pageQuery2.Equals(_ArrParamValue[PRIVATE]))
        {
            pageQuery = pageQuery2;
            fDate = fDate2;
            tDate = tDate2;
            extra = extra2;
        }
    }

    private string GenerateUI(string pageQuery, DateTime fDate, DateTime tDate)
    {
        string htmlContent = string.Empty;
        htmlContent = TemplateHandler.LoadTemplate("AccountInfo/NewMessageForm.html");

        TemplateHandler.Assign(ref htmlContent, "basePath", TplCore.TplSettings.GetBaseUrl());

        TemplateHandler.Assign(ref htmlContent, "invalidDateMessage", Resources.Lang.invalidDateMessage);
        TemplateHandler.Assign(ref htmlContent, "msgType", pageQuery);

        CreateDateRangeSelector(ref htmlContent, "../", 0, "fdate", "tdate", fDate, tDate, false, false, true, true, true, true);

        GenerateMessage(ref htmlContent, pageQuery, fDate, tDate);

        return htmlContent;
    }

    private void GenCalendarControl(ref string calendarTpl, string txtDateId, string currentPos, string triggerId, string currentDate,
                                            bool showTime, bool showOtherDays, bool useMultiLang)
    {
        try
        {
            if (string.IsNullOrEmpty(txtDateId) || string.IsNullOrEmpty(triggerId)) return;
            string txtClass = showTime ? "txtdate-show-time" : "date_no";
            string singleClick = showTime ? "false" : "true";
            string strShowOthersDay = showOtherDays ? "true" : "false";
            LoadHtml(ref calendarTpl,
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
        }
        catch
        {
            throw;
        }
    }
    private string GetLangChar(string Locale)
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
    private void CreateDateRangeSelector(ref string dateRangeTpl, string currentPos, int noDayReset, string fdateId, string tdateId, DateTime fdate, DateTime tdate)
    {
        // bool noJs = false;
        bool showTime = false;
        bool blShowToday = true;
        bool showtdate = true;
        bool showOtherDays = true;
        bool useMultiLang = true;

        try
        {
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


            string js = (noJs ? "" : TemplateHandler.CheckOutBlock(ref dateRangeTpl, "calendar-include"));
            string lang = useMultiLang ? GetLangChar(Sunplus.Agent.Model.Entities.UserSession.Current.Language) : GetLangChar("en-US");
            TemplateHandler.Assign(ref js, "{CurrentPos}", currentPos);
            TemplateHandler.Assign(ref js, "{Lang}", lang);
            if (!string.IsNullOrEmpty(js))
                TemplateHandler.CheckInBlock(ref dateRangeTpl, "calendar-include", js);
            else
                TemplateHandler.RemoveBlock(ref dateRangeTpl, "calendar-include");

            string strCalendar = TemplateHandler.CheckOutBlock(ref dateRangeTpl, "DateTimeSearch");
            if (!string.IsNullOrEmpty(strCalendar))
            {
                string strFromDate = strCalendar, strToDate = strCalendar;
                if (showtdate == false)
                {
                    GenCalendarControl(ref strFromDate, fdateId, currentPos, fdateId + "_tg", fdateStr, showTime, showOtherDays, useMultiLang);
                    TemplateHandler.Assign(ref strFromDate, "{dateText}", Resources.Lang.date);
                    TemplateHandler.Assign(ref dateRangeTpl, "{Fdate-value}", fdate.ToShortDateString());
                }
                else
                {
                    GenCalendarControl(ref strFromDate, fdateId, currentPos, fdateId + "_tg", fdateStr, showTime, showOtherDays, useMultiLang);
                    GenCalendarControl(ref strToDate, tdateId, currentPos, tdateId + "_tg", tdateStr, showTime, showOtherDays, useMultiLang);
                    TemplateHandler.Assign(ref strFromDate, "{dateText}", Resources.Lang.from);
                    TemplateHandler.Assign(ref strToDate, "{dateText}", Resources.Lang.to);
                    TemplateHandler.Assign(ref dateRangeTpl, "{Fdate-value}", fdate.ToShortDateString());
                    TemplateHandler.Assign(ref dateRangeTpl, "{Tdate-value}", tdate.ToShortDateString());
                }
                strCalendar = (strFromDate + strToDate);
            }
            TemplateHandler.CheckInBlock(ref dateRangeTpl, "DateTimeSearch", (string.IsNullOrEmpty(strCalendar) ? string.Empty : strCalendar));
        }
        catch (Exception ex)
        {
            Sunplus.Agent.Utilities.Logs.Write(ex.ToString());
        }
    }
    private void LoadHtml(ref string fileContent, string[] tags, string[] values)
    {
        if (string.IsNullOrEmpty(fileContent)) return;
        if (tags == null || values == null) return;
        for (int i = 0; i < tags.Length; ++i)
        {
            if (values.Length <= i) break;
            Assign(ref fileContent, tags[i], values[i]);
        }
    }
    private void Assign(ref string template, string tag, object value)
    {
        string val = DNAide.Core.CastAide.AsString(value, string.Empty);
        if (string.IsNullOrEmpty(template) || string.IsNullOrEmpty(tag)) return;
        if (tag[0] != '{' && tag[tag.Length - 1] != '}')
        {
            tag = "{" + tag + "}";
        }
        template = template.Replace(tag, val);
    }

    private void GenerateMessage(ref string htmlMsg, string pageQuery, DateTime fDate, DateTime tDate)
    {
        TemplateHandler.Assign(ref htmlMsg, "{messagehistory}", Resources.Lang.messagetitle);
        TemplateHandler.Assign(ref htmlMsg, "{normaltext}", Resources.Lang.normalmessage);
        TemplateHandler.Assign(ref htmlMsg, "{specialtext}", Resources.Lang.specialmessage);
        TemplateHandler.Assign(ref htmlMsg, "{personaltext}", Resources.Lang.personalmessage);
        if (pageQuery.Equals(_ArrParamValue[NORMAL]))
        {
            TemplateHandler.Assign(ref htmlMsg, "{normalstyle}", "id=\"current\"");
            TemplateHandler.Assign(ref htmlMsg, "{specialstyle}", string.Empty);
            TemplateHandler.Assign(ref htmlMsg, "{personalstyle}", string.Empty);
        }
        else if (pageQuery.Equals(_ArrParamValue[SPECIAL]) || pageQuery.Equals(_ArrParamValue[OLD_SPECIAL]))
        {
            TemplateHandler.Assign(ref htmlMsg, "{normalstyle}", string.Empty);
            TemplateHandler.Assign(ref htmlMsg, "{specialstyle}", "id=\"current\"");
            TemplateHandler.Assign(ref htmlMsg, "{personalstyle}", string.Empty);
        }
        else if (pageQuery.Equals(_ArrParamValue[PRIVATE]))
        {
            TemplateHandler.Assign(ref htmlMsg, "{normalstyle}", string.Empty);
            TemplateHandler.Assign(ref htmlMsg, "{specialstyle}", string.Empty);
            TemplateHandler.Assign(ref htmlMsg, "{personalstyle}", "id=\"current\"");
        }
        TemplateHandler.Assign(ref htmlMsg, "{No_Header}", Resources.Lang.no);
        TemplateHandler.Assign(ref htmlMsg, "{MessageID_Header}", Resources.Lang.messageIDHeader);
        TemplateHandler.Assign(ref htmlMsg, "{PostDate_Header}", Resources.Lang.lblPostdate);
        TemplateHandler.Assign(ref htmlMsg, "{Message_Header}", Resources.Lang.lblMessage);

        int maxsize = 50;
        int counter = 0;
        int siteid = 3000;
        StringBuilder result = new StringBuilder();
        if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 4)
        {
            siteid = 3100;
        }
        else if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 3)
        {
            siteid = 3200;
        }
        else
        {
            siteid = 3300;
        }

        // Get messages
        DataTable msgOfRole = null;
        if (pageQuery.Equals(_ArrParamValue[SPECIAL]) || pageQuery.Equals(_ArrParamValue[OLD_SPECIAL]))
        {
            msgOfRole = IBCAgent.Message.GetMessage(Sunplus.Agent.Model.Entities.UserSession.Current.CustId, siteid, 2, 1, maxsize, null, null, AppSettings.IsLocalTest);
        }
        else if (pageQuery.Equals(_ArrParamValue[NORMAL]))
        {
            msgOfRole =
                IBCAgent.Message.GetMessage(Sunplus.Agent.Model.Entities.UserSession.Current.CustId, siteid, 1, 0, maxsize, FormatHandler.ResetStartDate(fDate).ToString(), FormatHandler.ResetEndDate(tDate).ToString(), AppSettings.IsLocalTest);
        }
        else if (pageQuery.Equals(_ArrParamValue[PRIVATE]))
        {
            msgOfRole =
                IBCAgent.Message.GetMessage(Sunplus.Agent.Model.Entities.UserSession.Current.CustId, siteid, 0, 2, maxsize, FormatHandler.ResetStartDate(fDate).ToString(), FormatHandler.ResetEndDate(tDate).ToString(), AppSettings.IsLocalTest);
        }


        if ((msgOfRole != null) && (msgOfRole.Rows.Count > 0))
        {
            string strContentBlock = TemplateHandler.CopyBlock(htmlMsg, "MsgContent");

            // Show messages
            foreach (DataRow msg in msgOfRole.Rows)
            {
                string strContentRow = strContentBlock;

                counter++;
                TemplateHandler.Assign(ref strContentRow, "clor-over", AppSettings.RowBgOver);
                if (counter % 2 == 0)
                {
                    TemplateHandler.Assign(ref strContentRow, "row-class", "even_row");
                    TemplateHandler.Assign(ref strContentRow, "row-over", "live_row");
                    TemplateHandler.Assign(ref strContentRow, "clor-class", AppSettings.RowBgEven);
                }
                else
                {
                    TemplateHandler.Assign(ref strContentRow, "row-class", "odd_row");
                    TemplateHandler.Assign(ref strContentRow, "row-over", "live_row");
                    TemplateHandler.Assign(ref strContentRow, "clor-class", AppSettings.RowBgOdd);
                }

                TemplateHandler.Assign(ref strContentRow, "{No}", counter.ToString());
                TemplateHandler.Assign(ref strContentRow, "{MessageID}", msg["MessageId"]);
                string strPostDate = DateFormat.ShortDateFormat(msg["PostTime"]) + "<br/>" +
                    CastAide.AsDateTime(msg["PostTime"]).ToString("hh:mm:ss tt");
                TemplateHandler.Assign(ref strContentRow, "{PostDate}", strPostDate);
                // Show message content
                string normalMessage = "{0}", specialMessage = "<font color='Red'>{0}</font>", personalMessage = "<font color='Green'>{0}</font>";
                string originalMessage = IBCAgent.Message.GetContentMsg(msg, Sunplus.Agent.Model.Entities.UserSession.Current.Language);
                if (pageQuery.Equals(_ArrParamValue[SPECIAL]) || pageQuery.Equals(_ArrParamValue[OLD_SPECIAL]))
                {
                    TemplateHandler.Assign(ref strContentRow, "{Message}", string.Format(specialMessage, originalMessage));
                }
                else if (pageQuery.Equals(_ArrParamValue[PRIVATE]))
                {
                    TemplateHandler.Assign(ref strContentRow, "{Message}", string.Format(personalMessage, originalMessage));
                }
                else
                {
                    TemplateHandler.Assign(ref strContentRow, "{Message}", string.Format(normalMessage, originalMessage));
                }

                result.Append(strContentRow);
            }

            TemplateHandler.ReplaceBlock(ref htmlMsg, "MsgContent", result.ToString());
        }
        else if (counter == 0)
        {
            string noinfo = TemplateHandler.LoadTemplate("NoInfo.html");
            TemplateHandler.Assign(ref noinfo, "num-cols", "4");
            TemplateHandler.Assign(ref noinfo, "noinfo", Resources.Lang.noInfo);
            TemplateHandler.ReplaceBlock(ref htmlMsg, "MsgContent", noinfo);
        }
    }
}
