<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.AutoInsertAccountRacingFromExisting_Handler" %>

/*
 * Created 20120423@Jeff - auto insert account racing from exist
 * Revision:
 * ?@? - ...
 *
 */

using System;
using System.Web;
using System.Data;
using System.Collections;

using DAL;
using BLL;

using Fanex.Age.UI;
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Model.Entities.Permission;
using Sunplus.Agent.Model.BL.MemberInfo.SearchAndCopy;
using Sunplus.Agent.Model.BL.Racing;

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers
{
    public class AutoInsertAccountRacingFromExisting_Handler : TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        int custId = 0;
        int custIdOld = 0;
        string errMsg = string.Empty;
        int errCode = 0;
        Hashtable infoCustomer = new Hashtable();
        bool isMinPTChange = false;

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            int.TryParse(Request.Form["custid"], out custId);
            int.TryParse(Request["custidOld"], out custIdOld);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (custId < 0)
            {
                errCode = 101;
                errMsg = Resources.Lang.notExist;
                return new Result(errCode, errMsg);
            }
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            if (UserSession.Current.IsInternal)
            {
                errCode = 601;
                errMsg = Resources.Lang.lblNotAuthorized;
                return new Result(errCode, errMsg);
            }

            RacingBO racingBO = new RacingBO(CurrentUserSession.NetSessionId);

            Hashtable outputs = null;
            DataSet ds = racingBO.GetHRSetting_Member(custIdOld, out outputs);
            var checkConstraintData = new CheckConstraintDataForRaceBook();
            checkConstraintData.GeneratePositionTaking(ds.Tables[0].Rows[0], ref infoCustomer);

            infoCustomer.Add("custid", custId);
            infoCustomer.Add("subaccid", UserSession.Current.SubAccId);
            racingBO.AgentUpdateMemberHR(infoCustomer, out outputs);

            errMsg = outputs["errmsg"].ToString();

            return new Result(0, errMsg);
        }

        public override void RenderResult(ref JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }

    }
}