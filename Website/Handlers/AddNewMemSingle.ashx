<%@ WebHandler Language="C#" Class="AddNewMem_Single" %>

using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Web.Security;
using System.Web.SessionState;

using DNAide.Utils;
using DNAide.Core;
using Fanex.Data.DNAide;

using Sunplus.Agent.Model.Security;
using Sunplus.Agent.Model.Entities.Permission;

public class AddNewMem_Single : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        HttpFrequenceRule rule1 = new HttpFrequenceRule(context.Request.RawUrl, 2, TimeSpan.FromSeconds(60));
        rule1.ResetSliding = TimeSpan.FromMinutes(15);
        if (!rule1.Hit())
        {
            return;
        }

        context = IbcHandler.InitializeCulture(context);
        context.Response.ContentType = "text/html";
        int errCode = -1;
        string errMsg = string.Empty;
        string customErrMsg = string.Empty, username = string.Empty;

        if (!Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.CreateAccount)
        {
            errMsg = Resources.Lang.lblNotAuthorized;
            CommonGenerateHTML.GenerateErrorMsg(errMsg, ref customErrMsg);
            context.Response.Write("<script type=\"text/javascript\">parent.UpdateErr('" + customErrMsg + "');</script>");
            return;
        }

        DbResult drs = null;
        Hashtable hsParam = new Hashtable();
        try
        {
            username = Sunplus.Agent.Model.Entities.UserSession.Current.UserName + context.Request["Number1"].Trim() + context.Request["Number2"].Trim() + context.Request["Number3"].Trim();

            hsParam.Add("firstname", context.Request["txtFName"]);
            hsParam.Add("lastname", context.Request["txtLName"]);
            hsParam.Add("phone", context.Request["txtPhone"]);
            hsParam.Add("mobilephone", context.Request["txtMobilePhone"]);
            hsParam.Add("fax", context.Request["txtFax"]);

            hsParam.Add("creator", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
            hsParam.Add("custid", 0);
            hsParam.Add("username", username);
            hsParam.Add("userpwd", Encryption.CfsEnCode(context.Request["txtPass"].Trim()));
            hsParam.Add("discount", context.Request["slGroup"]);//Group a,b,c,d decide the commission.
            hsParam.Add("discountcs", context.Request["hdDiscountcs"]);//discountcs for other bet type
            hsParam.Add("recommend", Sunplus.Agent.Model.Entities.UserSession.Current.CustId);
            hsParam.Add("ugroup", context.Request["hdGroup"]);
            hsParam.Add("answer", "");
            hsParam.Add("success", 0);
            hsParam.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);

            hsParam.Add("playerdiscount", context.Request["slComHdp"]);
            hsParam.Add("playerdiscountcs", context.Request["slComOther"]);
            hsParam.Add("playerdiscount1x2", context.Request["slcom1x2"]);
            hsParam.Add("playerdiscountnumber", context.Request["slcomnumbergame"]);
            hsParam.Add("credit", context.Request["txtMaxCredit"]);

            if (!string.IsNullOrEmpty(context.Request["chkSuspended"]) || context.Request["UplineSuspended"] == "1")
                hsParam.Add("suspended", true);
            else
                hsParam.Add("suspended", false);
            bool isClose = false;
            if (!string.IsNullOrEmpty(context.Request["chkClosed"]))
                isClose = true;
            hsParam.Add("closed", isClose);
            //Add param for all sport type
            AddParaAllSportype(ref hsParam, context);

            hsParam.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
            hsParam.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
            hsParam.Add("returnerrcode", 1);
            hsParam.Add("errcode", 0);
            hsParam.Add("errmsg", "");
            drs = AccountInfo.Acc_CustomerInsMember_Ag(hsParam);

            if (CastAide.AsInt32(drs.Outputs["errcode"].ToString()) != 0)
            {
                context.Response.Write("<script type=\"text/javascript\">parent.UpdateErr('" + drs.Outputs["errmsg"].ToString() + "');</script>");
            }
            else
            {
                DbResult drStatus = AccountInfo.Age_CustomerSelStatus(Sunplus.Agent.Model.Entities.UserSession.Current.CustId);
                DataRow row = drStatus.ResultSet.Tables[0].Rows[0];
                bool isStatusCS = false;
                if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowCasino)
                {
                    isStatusCS = true;
                }

                string isDisableFI = ((CastAide.AsBoolean(row["DisableFI"]) == true) ? "1" : "0");
                string isDisableCas = ((CastAide.AsBoolean(row["DisableCasino"]) == true || isStatusCS == false) ? "1" : "0");
                string isDisableHR = ((!UserPermission.IsShowRacing) ? "1" : "0");
                string isDisableBG = (UserPermission.IsShowBingo == true) ? "0" : "1";
                string isDisableLiveCS = "1"; //(UserPermission.IsShowLiveCasino == true) ? "0" : "1";
                context.Response.Write("<script type='text/javascript'>parent.RedirectHRFICS('" + username + "', '" + drs.Outputs["custid"].ToString() + "','" + isDisableFI + "','" + isDisableCas + "','" + isDisableHR + "','" + isDisableBG + "','" + isDisableLiveCS + "','1');</script>");

            }
        }
        catch (Exception ex)
        {
            DNAide.Web.DNAideLog.Write(ex.Message);
            errCode = -1;
            errMsg = Common.NotAuthorized(ex.Message);
            context.Response.Write("<script type=\"text/javascript\">parent.UpdateErr('" + errMsg + "');</script>");
        }

    }
    void AddParaAllSportype(ref Hashtable param, HttpContext context)
    {
        string strHdpPT = context.Request["slPT"];
        string strCpy = context.Request["chkCpyHdp"];
        //chkCpyHdp
        string strHdpOtherPT = context.Request["slOtherPT"];
        string strOutrightPT = context.Request["slOutrightPT"];
        string strLiveHdpPT = context.Request["slLivePT"];
        string strNumberGamePT = context.Request["slNumberGamePT"];
        if (strCpy == "on")
        {
            strHdpOtherPT = strHdpPT;
            strOutrightPT = strHdpPT;
            strLiveHdpPT = strHdpPT;
            strNumberGamePT = strHdpPT;
        }

        param.Add("apositiontaking_1", strHdpPT);
        param.Add("apositiontaking_3", strHdpPT);
        param.Add("apositiontaking_7", strHdpPT);
        param.Add("apositiontaking_8", strHdpPT);
        param.Add("apositiontaking_2", strHdpPT);

        param.Add("apositiontaking_5", strHdpOtherPT);
        param.Add("apositiontaking_4", strHdpOtherPT);
        param.Add("apositiontaking_6", strHdpOtherPT);
        param.Add("apositiontaking_9", strHdpOtherPT);
        param.Add("apositiontaking_10", strOutrightPT);

        param.Add("alivepositiontaking_1", strLiveHdpPT);
        param.Add("alivepositiontaking_3", strLiveHdpPT);
        param.Add("alivepositiontaking_7", strLiveHdpPT);
        param.Add("alivepositiontaking_8", strLiveHdpPT);

        //Basketball
        param.Add("v2apositiontaking_1", strHdpPT);
        param.Add("v2apositiontaking_2", strHdpPT);
        param.Add("v2apositiontaking_3", strHdpPT);
        param.Add("v2apositiontaking_7", strHdpPT);
        param.Add("v2apositiontaking_8", strHdpPT);
        param.Add("v2apositiontaking_9", strHdpOtherPT);
        param.Add("v2apositiontaking_20", strHdpOtherPT);

        //Football
        param.Add("v3apositiontaking_1", strHdpPT);
        param.Add("v3apositiontaking_2", strHdpPT);
        param.Add("v3apositiontaking_3", strHdpPT);
        param.Add("v3apositiontaking_20", strHdpOtherPT);

        //Tennis
        param.Add("v5apositiontaking_1", strHdpPT);
        param.Add("v5apositiontaking_2", strHdpPT);
        param.Add("v5apositiontaking_3", strHdpPT);
        param.Add("v5apositiontaking_20", strHdpOtherPT);

        //Baseball
        param.Add("v8apositiontaking_1", strHdpPT);
        param.Add("v8apositiontaking_3", strHdpPT);
        param.Add("v8apositiontaking_20", strHdpOtherPT);

        //Golf
        param.Add("v10apositiontaking_1", strHdpPT);
        param.Add("v10apositiontaking_2", strHdpPT);
        param.Add("v10apositiontaking_3", strHdpPT);
        param.Add("v10apositiontaking_20", strHdpOtherPT);

        //Motosports
        param.Add("v11apositiontaking_1", strHdpPT);
        param.Add("v11apositiontaking_20", strHdpOtherPT);

        //Other Sports
        param.Add("v99apositiontaking_1", strHdpPT);
        param.Add("v99apositiontaking_2", strHdpPT);
        param.Add("v99apositiontaking_3", strHdpPT);
        param.Add("v99apositiontaking_20", strHdpOtherPT);
        param.Add("v99apositiontaking_9", strHdpOtherPT);

        param.Add("v161apositiontaking_81", strNumberGamePT);
        param.Add("v161apositiontaking_83", strNumberGamePT);
        param.Add("v161apositiontaking_86", strNumberGamePT);
        param.Add("v161apositiontaking_88", strNumberGamePT);

        param.Add("v161alivepositiontaking_85", strNumberGamePT);
        param.Add("v161alivepositiontaking_86", strNumberGamePT);
        param.Add("v161alivepositiontaking_87", strNumberGamePT);
        param.Add("v161alivepositiontaking_89", strNumberGamePT);
        param.Add("v161alivepositiontaking_90", strNumberGamePT);

        double strMinBet = DNAide.Core.CastAide.AsDouble(context.Request["txtMinBet"], 0);
        double strMaxBet = DNAide.Core.CastAide.AsDouble(context.Request["txtMaxBet"], 0);
        double strMaxPerMatch = DNAide.Core.CastAide.AsDouble(context.Request["txtMaxPerMatch"], 0);

        double MaxPerBall_Max = DNAide.Core.CastAide.AsDouble(context.Request["hdMaxPerBallMax"], 0);
        double strMaxPerball = DNAide.Core.CastAide.AsDouble(context.Request["txtMaxPerBall"], 0);
        //if (MaxPerBall_Max != 0)
        //{
        //    strMaxPerball = (MaxPerBall_Max < strMaxPerMatch) ? MaxPerBall_Max : strMaxPerMatch;
        //}

        param.Add("minbet", strMinBet);
        param.Add("maxbet", strMaxBet);
        param.Add("maxpermatch", strMaxPerMatch);

        param.Add("minbetv2", strMinBet);
        param.Add("maxbetv2", strMaxBet);
        param.Add("maxpermatchv2", strMaxPerMatch);

        param.Add("minbetv3", strMinBet);
        param.Add("maxbetv3", strMaxBet);
        param.Add("maxpermatchv3", strMaxPerMatch);

        param.Add("minbetv5", strMinBet);
        param.Add("maxbetv5", strMaxBet);
        param.Add("maxpermatchv5", strMaxPerMatch);

        param.Add("minbetv8", strMinBet);
        param.Add("maxbetv8", strMaxBet);
        param.Add("maxpermatchv8", strMaxPerMatch);

        param.Add("minbetv10", strMinBet);
        param.Add("maxbetv10", strMaxBet);
        param.Add("maxpermatchv10", strMaxPerMatch);

        param.Add("minbetv11", strMinBet);
        param.Add("maxbetv11", strMaxBet);
        param.Add("maxpermatchv11", strMaxPerMatch);

        param.Add("minbetv99", strMinBet);
        param.Add("maxbetv99", strMaxBet);
        param.Add("maxpermatchv99", strMaxPerMatch);

        param.Add("minbetv99mp", strMinBet);
        param.Add("maxbetv99mp", strMaxBet);
        param.Add("maxpermatchv99mp", strMaxPerMatch);

        param.Add("minbetv161", strMinBet);
        param.Add("maxbetv161", strMaxBet);
        param.Add("maxpermatchv161", strMaxPerMatch);
        param.Add("maxperballv161", strMaxPerball);
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}