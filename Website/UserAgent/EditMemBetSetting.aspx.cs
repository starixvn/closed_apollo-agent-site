using System;
using System.Data;
using System.Text;

using DNAide.Web;
using DNAide.Core;
using IBCAgent;
using Fanex.Data.DNAide;
using DNAide.Utils;

using Sunplus.Agent.Model.Entities.Permission;

public partial class EditMemBetSetting : TplPage
{
    protected int CustId
    {
        get
        {
            return CastAide.AsInt32(Request.QueryString["custId"], -1);
        }
    }

    protected string CustName
    {
        get
        {
            return Request.QueryString["username"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        this.CssGroup = "popup";

        //Template
        string template = TemplateHandler.LoadTemplate("UserInfo/MemberBetSetting.html");
        string errorTpl = TemplateHandler.LoadTemplate("ErrorMsg.html");
        TemplateHandler.Assign(ref template, "error-message", errorTpl);

        //Languages
        TemplateHandler.Assign(ref template, "lbleditbetsetting", Resources.Lang.lbleditbetsetting + " " + CustName);
        TemplateHandler.Assign(ref template, "lbl_copyallsettingfromsoccer", (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowNumberGame) ? Resources.Lang.lblcopyallsettingfromsoccer : Resources.Lang.lblcopyallsettingfromsoccer_nonumbergame);
        TemplateHandler.Assign(ref template, "lblminbet", Resources.Lang.lblMinBet);
        TemplateHandler.Assign(ref template, "lblmaxbet", Resources.Lang.lblMaxBet);
        TemplateHandler.Assign(ref template, "lblmaxpermatch", Resources.Lang.lblMaxPerMatch);        
        
        TemplateHandler.Assign(ref template, "update", Resources.Lang.update);
        TemplateHandler.Assign(ref template, "reset", Resources.Lang.reset);
        TemplateHandler.Assign(ref template, "loading", Resources.Lang.processing);

        TemplateHandler.Assign(ref template, "lblMaxPerBall", Resources.Lang.NumberGameMaxperball);
        TemplateHandler.Assign(ref template, "othersetting", Resources.Lang.othersetting);
        //Handlers
        TemplateHandler.Assign(ref template, "updateinfohandler", String.Format("../Handlers/UpdateMemBetSetting.ashx?custid={0}&username={1}",this.CustId, this.CustName));

        //Load commission
        this.LoadUserBetSetting(ref template);

        //Show message succeful
        if (!String.IsNullOrEmpty(Request.QueryString["succ"]))
        {
            template += String.Format(" <script>showErrMsg('{0}', 1);</script> ", Resources.Lang.updatesuccessfull);
        }

        this.TplSetContent(template);

        AddJsVariable("errorinputnumber", Resources.Lang.errorinputnumber);
        AddJsVariable("lblalertMinbetCompare", Resources.Lang.lblalertMinbetCompare);
        AddJsVariable("alertMaxLessMin", Resources.Lang.alertMaxLessMin);
        AddJsVariable("lblalertMinBetNegativeHR", Resources.Lang.lblalertMinBetNegativeHR);
        AddJsVariable("lblalerMaxbetCompare", Resources.Lang.lblalerMaxbetCompare);
        AddJsVariable("alertMaxLessMaxPer", Resources.Lang.alertMaxLessMaxPer);
        AddJsVariable("lblalertMaxBetNegativeHR", Resources.Lang.lblalertMaxBetNegativeHR);
        AddJsVariable("lblalertMaxPerMatchCompareMax", Resources.Lang.lblalertMaxPerMatchCompareMax);
        AddJsVariable("lblalertMaxPerMatchCompareMin", Resources.Lang.lblalertMaxPerMatchCompareMin);
        AddJsVariable("lblalertMaxPerMatchNegativeHR", Resources.Lang.lblalertMaxPerMatchNegativeHR);
        AddJsVariable("alertMaxperball", Resources.Lang.alertIncorrectMaxPerBall);
        AddJsVariable("alertMaxperballCMax", Resources.Lang.lblalertMaxPerBallCompareMax);
        AddJsVariable("alertMaxperballC", Resources.Lang.lblalertMaxPerBallCompare);
        AddJsVariable("alertMaxPerMatchLessMaxPerBall", Resources.Lang.alertMaxPerMatchLessMaxPerBall);
        AddJsVariable("alertMaxPerBallLessMaxBet", Resources.Lang.alertMaxPerBallLessMaxBet);
        AddJsVariable("isAgent", (AppSettings.IsAgentSite ? "1" : "0"));

    }

    public void LoadUserBetSetting(ref string template)
    {
        int userId = Sunplus.Agent.Model.Entities.UserSession.Current.CustId;
        DbResult dr = UserInfo.SelectBetSettingByCustomer(this.CustId, userId);
        DataRow row = dr.ResultSet.Tables[0].Rows[0];        

        StringBuilder settingTpls = new StringBuilder();
        string settingTemplate = TemplateHandler.CheckOutBlock(ref template, "tblbetsettting");

        string[] arrSportTypeId = { "1", "2", "3", "5", "8", "10", "11", "99", "99MP", "161" };
        string[] arrSportTypeName = { Resources.Lang.soccer, Resources.Lang.Basketball, Resources.Lang.Football, Resources.Lang.Tennis, Resources.Lang.Baseball, Resources.Lang.Golf, Resources.Lang.MotorSports, Resources.Lang.OtherSports, Resources.Lang.mixsports, Resources.Lang.numbergame };

        if (!UserPermission.IsShowNumberGame && UserPermission.IsShowMixedSport)
        { 
            arrSportTypeId = new string[] { "1", "2", "3", "5", "8", "10", "11", "99", "99MP" };
            arrSportTypeName =new string[] { Resources.Lang.soccer, Resources.Lang.Basketball, Resources.Lang.Football, Resources.Lang.Tennis, Resources.Lang.Baseball, Resources.Lang.Golf, Resources.Lang.MotorSports, Resources.Lang.OtherSports, Resources.Lang.mixsports };
        }
        else if (UserPermission.IsShowNumberGame && !UserPermission.IsShowMixedSport)
        {
            arrSportTypeId = new string[] { "1", "2", "3", "5", "8", "10", "11", "99", "161" };
            arrSportTypeName = new string[] { Resources.Lang.soccer, Resources.Lang.Basketball, Resources.Lang.Football, Resources.Lang.Tennis, Resources.Lang.Baseball, Resources.Lang.Golf, Resources.Lang.MotorSports, Resources.Lang.OtherSports , Resources.Lang.numbergame};
        }
        else if (!UserPermission.IsShowNumberGame && !UserPermission.IsShowMixedSport)
        {
            arrSportTypeId = new string[] { "1", "2", "3", "5", "8", "10", "11", "99"};
            arrSportTypeName = new string[] { Resources.Lang.soccer, Resources.Lang.Basketball, Resources.Lang.Football, Resources.Lang.Tennis, Resources.Lang.Baseball, Resources.Lang.Golf, Resources.Lang.MotorSports, Resources.Lang.OtherSports };
        }

        double maxperballMax = CastAide.AsDouble(row["MaxPerBallv161_Max"], 0);
        TemplateHandler.Assign(ref template, "MaxPerBall-val", FormatHandler.FormatNumber1(row["maxperballv161"], Sunplus.Agent.Model.Entities.UserSession.Current.LangKey));
        TemplateHandler.Assign(ref template, "MaxPerBall-max", FormatHandler.FormatNumber1(maxperballMax, Sunplus.Agent.Model.Entities.UserSession.Current.LangKey));
        if (!UserPermission.IsShowNumberGame)
        {            
            TemplateHandler.Assign(ref template, "OtherSetting_clss", "hidden");
        }
        else
        {
            TemplateHandler.Assign(ref template, "OtherSetting_clss", string.Empty);
        }
        
        if (maxperballMax == 0)
        {
            TemplateHandler.Assign(ref template, "maxperball_clss", "hidden");
        }
        else
        {
            TemplateHandler.Assign(ref template, "maxperball_clss", "minmaxbet"); 
        }

        for (int i = 0; i < arrSportTypeId.Length; i++)
        {
            string settingTpl = (string)settingTemplate.Clone();
            string sportId = arrSportTypeId[i];
            string sportName = arrSportTypeName[i];//CacheRef.GetSportName(sportId);

            //column name
            string suffix = ((i == 0) ? "" : "v" + sportId);
            string colMinBet = "minbet" + suffix;
            string colMaxBet = "maxbet" + suffix;
            string colMaxPerMatch = "maxpermatch" + suffix;

            string colMinBetMax = colMinBet + "_max"; //limit
            string colMaxBetMin = colMaxBet + "_min";
            string colMaxPerMatchMin = colMaxPerMatch + "_min";
            string colMaxPerMatchMax = colMaxPerMatch + "_max";

            //column value            
            double minBetVal = RoundUpNumber(row[colMinBet]);
            double maxBetVal = RoundUpNumber(row[colMaxBet]);
            double maxPerMatchVal = RoundUpNumber(row[colMaxPerMatch]);

            double minBetMaxVal = RoundUpNumber(row[colMinBetMax]); //limit
            double maxBetMinVal = RoundUpNumber(row[colMaxBetMin]);
            double maxBetMatchMin = RoundUpNumber(row[colMaxPerMatchMin]);
            double maxBetMatchMax = RoundUpNumber(row[colMaxPerMatchMax]);

            //assign value
            TemplateHandler.Assign(ref settingTpl, "sportid", sportId);
            TemplateHandler.Assign(ref settingTpl, "lblsportname", sportName);
            TemplateHandler.Assign(ref settingTpl, "minbetval", FormatHandler.FormatNumber1(minBetVal, Sunplus.Agent.Model.Entities.UserSession.Current.LangKey));
            TemplateHandler.Assign(ref settingTpl, "maxbetval", FormatHandler.FormatNumber1(maxBetVal, Sunplus.Agent.Model.Entities.UserSession.Current.LangKey));
            TemplateHandler.Assign(ref settingTpl, "maxpermatchval", FormatHandler.FormatNumber1(maxPerMatchVal, Sunplus.Agent.Model.Entities.UserSession.Current.LangKey));

            TemplateHandler.Assign(ref settingTpl, "minbet_max", String.Format("{0}", FormatHandler.FormatNumber1(minBetMaxVal, Sunplus.Agent.Model.Entities.UserSession.Current.LangKey)));
            TemplateHandler.Assign(ref settingTpl, "maxbet_min", String.Format("{0}", FormatHandler.FormatNumber1(maxBetMinVal, Sunplus.Agent.Model.Entities.UserSession.Current.LangKey)));
            TemplateHandler.Assign(ref settingTpl, "maxpermatch_max", String.Format("{0}", FormatHandler.FormatNumber1(maxBetMatchMax, Sunplus.Agent.Model.Entities.UserSession.Current.LangKey)));
            TemplateHandler.Assign(ref settingTpl, "maxpermatch_min", String.Format("{0}", FormatHandler.FormatNumber1(maxBetMatchMin, Sunplus.Agent.Model.Entities.UserSession.Current.LangKey)));

            if (CastAide.AsDouble(minBetMaxVal) == 0.0) TemplateHandler.RemoveBlock(ref settingTpl, "blkminbetmax");
            if (CastAide.AsDouble(maxBetMinVal) == 0.0) TemplateHandler.RemoveBlock(ref settingTpl, "blkmaxbetmin");
            if (CastAide.AsDouble(maxBetMatchMax) == 0.0) TemplateHandler.RemoveBlock(ref settingTpl, "blkmaxpermatchmax");

            settingTpls.Append(settingTpl);
        }

        TemplateHandler.CheckInBlock(ref template, "tblbetsettting", settingTpls.ToString());
    }

    protected double RoundUpNumber(object number)
    {

        return Math.Round(CastAide.AsDouble(number), 1);
    }

}
