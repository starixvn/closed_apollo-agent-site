<%@ WebHandler Language="C#" Class="UpdateExchangeRate" %>

using System;
using System.Web;
using Sunplus.Agent.Model.BL.Admin;
using Fanex.Age.UI;
using System.Web.SessionState;
using System.Collections;
using Sunplus.Agent.Model.Common;

public class UpdateExchangeRate : TplHandler, IRequiresSessionState
{
    //private AdminBO _adminBO = new AdminBO(HttpRequest);

    //public void ProcessRequest (HttpContext context) {
    //    context.Response.ContentType = "text/plain";

    //    context.Response.Write("OK");
    //}

    private int custId = 0;
    private string ip = string.Empty;

    private int exchangeId;
    private string description = string.Empty;
    private double multipliedBy;
    private double baseRate;
    private double minBet;
    private double minBetLive;

    public override object ValidateRequest(Hashtable p)
    {
        //custId = Convert.ToInt32(p["custId"]);

        //if (custId <= 0 || (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal))
        //{
        //    return new Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
        //}

        return null;
    }

    public override void ProcessRequest(ref Hashtable p)
    {
        p.AddHttpParameters(new string[]{
                "exchangeId",
                "description",
                "multipliedBy",
                "baseRate",
                "minBet",
                "minBetLive",
            }, Request);
    }

    public override object InvokeData(Hashtable p)
    {
        var adminBO = new AdminBO(CurrentUserSession.NetSessionId);

        int errCode = 0;
        string errMsg = string.Empty;

        p.Add("ip", AgentIPFormat.GetClientIP());
        ip = p["ip"].ToString();

        // todo: validates params

        exchangeId = Convert.ToInt32(p["ExchangeId"]);
        description = HttpUtility.HtmlDecode(p["description"].ToString());
        multipliedBy = Convert.ToDouble(p["multipliedBy"]);
        baseRate = Convert.ToDouble(p["baseRate"]);
        minBet = Convert.ToDouble(p["minBet"]);
        minBetLive = Convert.ToDouble(p["minBetLive"]);

        adminBO.UpdateExchangeRate(custId, ip,
            exchangeId, description, multipliedBy, baseRate, minBet, minBetLive,
            ref errCode, ref errMsg);

        return new Result(errCode, errMsg);
    }

    public override void RenderResult(ref JSONData data)
    {
        data.AsResult(View as Result);
    }
}