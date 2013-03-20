<%@ WebHandler Language="C#" Class="InsertExchangeRate" %>

using System;
using System.Web;
using Sunplus.Agent.Model.BL.Admin;
using Fanex.Age.UI;
using System.Web.SessionState;
using System.Collections;
using Sunplus.Agent.Model.Common;

public class InsertExchangeRate : TplHandler, IRequiresSessionState
{
    private int custId = 0;
    private string ip = string.Empty;

    private int exchangeId;
    private string currency;
    private string description = string.Empty;
    private double multipliedBy;
    private double baseRate;
    private double minBet;
    private double minBetLive;

    public override object ValidateRequest(Hashtable p)
    {
        custId = Convert.ToInt32(p["custId"]);

        if (custId <= 0 || (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal))
        {
            return new Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
        }

        return null;
    }

    public override void ProcessRequest(ref Hashtable p)
    {
        p.AddHttpParameters(new string[]{
                "custId",
                "currency",
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

        if (p["currency"] == null || string.IsNullOrWhiteSpace(p["currency"].ToString()))
        {
            return new Result(20,
                string.Format(Resources.Lang.FieldIsRequiredTpl, Resources.Lang.currency));
        }
        else
        {
            currency = p["currency"].ToString().Trim();

            // checks if currency existed
            var rate = adminBO.GetExchangeRateByCurrency(currency);

            if (rate != null)
            {
                return new Result(30,
                    string.Format(Resources.Lang.NameExistedTpl, currency));
            }
        }

        multipliedBy = Convert.ToDouble(p["multipliedBy"]);
        baseRate = Convert.ToDouble(p["baseRate"]);
        minBet = Convert.ToDouble(p["minBet"]);
        minBetLive = Convert.ToDouble(p["minBetLive"]);
        description = p["description"] == null ? string.Empty
            : HttpUtility.HtmlDecode(p["description"].ToString()).Trim();

        p.Add("ip", AgentIPFormat.GetClientIP());
        ip = p["ip"].ToString();

        adminBO.InsertExchangeRate(custId, ip,
            currency, description, multipliedBy, baseRate, minBet, minBetLive,
            out exchangeId, ref errCode, ref errMsg);

        return new Result(errCode, errMsg);
    }

    public override void RenderResult(ref JSONData data)
    {
        data.AsResult(View as Result);
    }
}