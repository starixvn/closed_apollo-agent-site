<%@ WebHandler Language="C#" Class="UpdateMemBetSetting" %>

using System;
using System.Web;
using System.Collections;
using DNAide.Web;
using Fanex.Data.DNAide;
using System.Web.SessionState;
using DNAide.Core;

using System.Web.Security;
using Sunplus.Agent.Utilities;

public class UpdateMemBetSetting : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {

        context.Response.ContentType = "text/html";
        Common.UpdateCulture();

        string errMsg = string.Empty;

        if (!Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.UpdateAccount)
        {
            errMsg = Resources.Lang.lblNotAuthorized;
            context.Response.Write("<script type='text/javascript'>parent.showInfoMsg('" + PositionTaking.ReplaceData(errMsg) + "',0);</script>");
            return;
        }

        int custid = CastAide.AsInt32(context.Request.QueryString["custid"], -1);
        string username = CastAide.AsString(context.Request.QueryString["username"], String.Empty);
        if (custid > 0)
        {
            HttpRequest request = context.Request;

            bool isUpdateMinMaxBet = true, isUpdateMinMaxBetv2 = true, isUpdateMinMaxBetv3 = true, isUpdateMinMaxBetv5 = true,
                 isUpdateMinMaxBetv8 = true, isUpdateMinMaxBetv10 = true, isUpdateMinMaxBetv11 = true,
                 isUpdateMinMaxBetv99 = true, isUpdateMinMaxBetv99MP = true, isUpdateMinMaxBetv161 = true;

            isUpdateMinMaxBetv161 = Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowNumberGame ? true : false;

            string chkCopyAll = CastAide.AsString(request["chkCopyAll"]);
            bool isCheckAll = chkCopyAll.Equals("on", StringComparison.OrdinalIgnoreCase);

            decimal minBet = CastAide.AsDecimal(request["txtMinbet1"]),
                    maxBet = CastAide.AsDecimal(request["txtMaxbet1"]),
                    maxPerMatch = CastAide.AsDecimal(request["txtMaxPerMatch1"]),

                    minBetv2 = CastAide.AsDecimal(request["txtMinbet2"]),
                    maxBetv2 = CastAide.AsDecimal(request["txtMaxbet2"]),
                    maxPerMatchv2 = CastAide.AsDecimal(request["txtMaxPerMatch2"]),

                    minBetv3 = CastAide.AsDecimal(request["txtMinbet3"]),
                    maxBetv3 = CastAide.AsDecimal(request["txtMaxbet3"]),
                    maxPerMatchv3 = CastAide.AsDecimal(request["txtMaxPerMatch3"]),

                    minBetv5 = CastAide.AsDecimal(request["txtMinbet5"]),
                    maxBetv5 = CastAide.AsDecimal(request["txtMaxbet5"]),
                    maxPerMatchv5 = CastAide.AsDecimal(request["txtMaxPerMatch5"]),

                    minBetv8 = CastAide.AsDecimal(request["txtMinbet8"]),
                    maxBetv8 = CastAide.AsDecimal(request["txtMaxbet8"]),
                    maxPerMatchv8 = CastAide.AsDecimal(request["txtMaxPerMatch8"]),

                    minBetv10 = CastAide.AsDecimal(request["txtMinbet10"]),
                    maxBetv10 = CastAide.AsDecimal(request["txtMaxbet10"]),
                    maxPerMatchv10 = CastAide.AsDecimal(request["txtMaxPerMatch10"]),

                    minBetv11 = CastAide.AsDecimal(request["txtMinbet11"]),
                    maxBetv11 = CastAide.AsDecimal(request["txtMaxbet11"]),
                    maxPerMatchv11 = CastAide.AsDecimal(request["txtMaxPerMatch11"]),

                    minBetv99 = CastAide.AsDecimal(request["txtMinbet99"]),
                    maxBetv99 = CastAide.AsDecimal(request["txtMaxbet99"]),
                    maxPerMatchv99 = CastAide.AsDecimal(request["txtMaxPerMatch99"]),

                    minBetv161 = CastAide.AsDecimal(request["txtMinbet161"], minBet),
                    maxBetv161 = CastAide.AsDecimal(request["txtMaxbet161"], maxBet),
                    maxPerMatchv161 = CastAide.AsDecimal(request["txtMaxPerMatch161"], maxPerMatch),
                    maxperBallv161 = Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowNumberGame ? CastAide.AsDecimal(request["txtMaxPerBall"], maxPerMatch) : maxPerMatch,

                    minBetv99MP = CastAide.AsDecimal(request["txtMinbet99MP"], 0),
                    maxBetv99MP = CastAide.AsDecimal(request["txtMaxbet99MP"], 0),
                    maxPerMatchv99MP = CastAide.AsDecimal(request["txtMaxPerMatch99MP"], 0);

            //copy from soccer's betsettings!
            if (isCheckAll)
            {
                minBetv2 = minBetv3 = minBetv5 = minBetv8 = minBetv10 = minBetv11 = minBetv99 = minBetv99MP = minBet;
                maxBetv2 = maxBetv3 = maxBetv5 = maxBetv8 = maxBetv10 = maxBetv11 = maxBetv99 = maxBetv99MP = maxBet;
                maxPerMatchv2 = maxPerMatchv3 = maxPerMatchv5 = maxPerMatchv8 = maxPerMatchv10 = maxPerMatchv11 = maxPerMatchv99 = maxPerMatchv99MP = maxPerMatch;
            }

            int errorCode = 0;
            string errorMsg = String.Empty;

            try
            {
                Hashtable param = new Hashtable();

                param.Add("custid", custid);
                //param.Add("updatemultiple", true);
                param.Add("updateminmaxbet", isUpdateMinMaxBet);
                param.Add("updateminmaxbetv2", isUpdateMinMaxBetv2);
                param.Add("updateminmaxbetv3", isUpdateMinMaxBetv3);
                param.Add("updateminmaxbetv5", isUpdateMinMaxBetv5);
                param.Add("updateminmaxbetv8", isUpdateMinMaxBetv8);
                param.Add("updateminmaxbetv10", isUpdateMinMaxBetv10);
                param.Add("updateminmaxbetv11", isUpdateMinMaxBetv11);
                //param.Add("updateminmaxbetv201", isUpdateMinMaxBetv201);
                param.Add("updateminmaxbetv99", isUpdateMinMaxBetv99);
                param.Add("updateminmaxbetv99mp", isUpdateMinMaxBetv99MP);
                param.Add("updateminmaxbetv161", isUpdateMinMaxBetv161);

                param.Add("minbet", minBet);
                param.Add("maxbet", maxBet);
                param.Add("maxpermatch", maxPerMatch);

                param.Add("minbetv2", minBetv2);
                param.Add("maxbetv2", maxBetv2);
                param.Add("maxpermatchv2", maxPerMatchv2);

                param.Add("minbetv3", minBetv3);
                param.Add("maxbetv3", maxBetv3);
                param.Add("maxpermatchv3", maxPerMatchv3);

                param.Add("minbetv5", minBetv5);
                param.Add("maxbetv5", maxBetv5);
                param.Add("maxpermatchv5", maxPerMatchv5);

                param.Add("minbetv8", minBetv8);
                param.Add("maxbetv8", maxBetv8);
                param.Add("maxpermatchv8", maxPerMatchv8);

                param.Add("minbetv10", minBetv10);
                param.Add("maxbetv10", maxBetv10);
                param.Add("maxpermatchv10", maxPerMatchv10);

                param.Add("minbetv11", minBetv11);
                param.Add("maxbetv11", maxBetv11);
                param.Add("maxpermatchv11", maxPerMatchv11);

                param.Add("minbetv99", minBetv99);
                param.Add("maxbetv99", maxBetv99);
                param.Add("maxpermatchv99", maxPerMatchv99);

                param.Add("minbetv99mp", minBetv99MP);
                param.Add("maxbetv99mp", maxBetv99MP);
                param.Add("maxpermatchv99mp", maxPerMatchv99MP);

                param.Add("minbetv161", minBetv161);
                param.Add("maxbetv161", maxBetv161);
                param.Add("maxpermatchv161", maxPerMatchv161);

                param.Add("maxperballv161", maxperBallv161);
                //default
                param.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.CustId);
                param.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);
                param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
                param.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
                param.Add("errcode", 0);
                param.Add("errmsg", "");

                errorCode = IBCAgent.UserInfo.UpdateMemberBetSetting(param, ref errorMsg);

                //WriteMsg(context, (errorCode > 0) ? errorMsg : Resources.Lang.updatesuccessfull);
                if (errorCode > 0)
                {
                    string tmp = errorMsg;//"AT1104: min bet must be less than max bet , max bet must be less than max bet per match and Max Per Ball must be greater than Max Bet and less than Max Per Match(for Number Game)100.00-200.00-500.00";
                    WriteMsg(context, ReplaceData(tmp), 0);
                }
                else
                {
                    //WriteMsg(context, Resources.Lang.updatesuccessfull, 1);
                    context.Response.Write(String.Format("<script>parent.DelayReloadPage('../UserAgent/EditMemBetSetting.aspx?custId={0}&username={1}&succ=1');</script>", custid, username));
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                Sunplus.Agent.Utilities.Logs.Write(ex);
                str = Common.NotAuthorized(str);
                WriteMsg(context, str, 0);
            }
        }
    }
    protected string ReplaceData(string str)
    {
        return str.Replace("\"", "&quot;").Replace("'", "&#39;").Replace("\n", "").Replace("\r", "");
    }
    //msgType = 0: error message
    //msgType = 1: info message
    protected void WriteMsg(HttpContext context, string msg, int msgType)
    {

        string jsReturn = String.Format("<script type='text/javascript'>parent.showInfoMsg('{0}', {1});</script>", msg.Replace("'", "\\'"), msgType);

        //string jsReturn = String.Format("<script type='text/javascript'>parent.DelayReloadPage();</script>");
        context.Response.Write(jsReturn);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}