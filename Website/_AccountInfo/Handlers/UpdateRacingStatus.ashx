<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._AccountInfo.UpdateRacingStatus_Handler" %>

using System;
using System.Web;

using Sunplus.Agent.Model.BL.AccountInfo;
using Fanex.Age.UI;
using Sunplus.Agent.Model.BL.Racing;

using System.Data;

namespace Sunplus.Agent.UI._AccountInfo
{
    public class UpdateRacingStatus_Handler : TplHandler, System.Web.SessionState.IRequiresSessionState
    {
        int custId = 0;

        const string SESSION_MEMBERROLEID = "SESSION_MEMBERROLEID";

        public override void RenderResult(ref JSONData data)
        {
            string[] outputs = View as string[];

            string[] ArrCodes = outputs[0].ToString().Split("\a".ToCharArray());
            string[] ArrMsgs = outputs[1].ToString().Split("\a".ToCharArray());

            data.Add("ArrCodes", ArrCodes);
            data.Add("ArrMsgs", ArrMsgs);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            RacingBO racing = new RacingBO(CurrentUserSession.NetSessionId);

            string errCode = string.Empty;
            string errMsg = string.Empty;

            p.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
            p.Add("custid", custId);

            racing.UpdateStatus(p, out errCode, out errMsg);

            return new string[] { errCode, errMsg };
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            int.TryParse(Request["custid"], out custId);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal)
            {
                return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            try
            {
                p.Add("disable151_41", !bool.Parse(Request["chkEnabled_151_41"]));

                p.Add("disable151_31", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing_151_31 ? !bool.Parse(Request["chkEnabled_151_31"]) : p["disable151_41"]);
                p.Add("disable151_51", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing_151_51 ? !bool.Parse(Request["chkEnabled_151_51"]) : p["disable151_41"]);
                p.Add("disable152_41", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing_152_41 ? !bool.Parse(Request["chkEnabled_152_41"]) : p["disable151_41"]);
                p.Add("disable152_51", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing_152_51 ? !bool.Parse(Request["chkEnabled_152_51"]) : p["disable151_41"]);
                p.Add("disable153_51", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing_153_51 ? !bool.Parse(Request["chkEnabled_153_51"]) : p["disable151_41"]);

                p.Add("disabledownlines151_41", !bool.Parse(Request["chkEnabledDL_151_41"]));

                p.Add("disabledownlines151_31", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing_151_31 ? !bool.Parse(Request["chkEnabledDL_151_31"]) : p["disabledownlines151_41"]);
                p.Add("disabledownlines151_51", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing_151_51 ? !bool.Parse(Request["chkEnabledDL_151_51"]) : p["disabledownlines151_41"]);
                p.Add("disabledownlines152_41", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing_152_41 ? !bool.Parse(Request["chkEnabledDL_152_41"]) : p["disabledownlines151_41"]);
                p.Add("disabledownlines152_51", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing_152_51 ? !bool.Parse(Request["chkEnabledDL_152_51"]) : p["disabledownlines151_41"]);
                p.Add("disabledownlines153_51", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing_153_51 ? !bool.Parse(Request["chkEnabledDL_153_51"]) : p["disabledownlines151_41"]);

                p.Add("updatedisable151_41", bool.Parse(Request["updatedisable151_41"]));

                p.Add("updatedisable151_31", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing_151_31 ? bool.Parse(Request["updatedisable151_31"]) : p["updatedisable151_41"]);
                p.Add("updatedisable151_51", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing_151_51 ? bool.Parse(Request["updatedisable151_51"]) : p["updatedisable151_41"]);
                p.Add("updatedisable152_41", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing_152_41 ? bool.Parse(Request["updatedisable152_41"]) : p["updatedisable151_41"]);
                p.Add("updatedisable152_51", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing_152_51 ? bool.Parse(Request["updatedisable152_51"]) : p["updatedisable151_41"]);
                p.Add("updatedisable153_51", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing_153_51 ? bool.Parse(Request["updatedisable153_51"]) : p["updatedisable151_41"]);

            }
            catch
            {
                return new Result(ErrorType.Unknown, "Invalid status value!");
            }

            if (custId <= 0)
            {
                return new Result(ErrorType.Unknown, "Invalid customer id!");
            }

            //int roleId = 0;
            //int.TryParse(Session[SESSION_MEMBERROLEID].ToString(), out roleId);

            //if (CurrentUserSession.RoleId - roleId >= 2)
            //{
            //    return new Result(ErrorType.Unknown, "Invalid customer role id!");
            //}

            return null;
        }
    }
}