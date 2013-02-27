<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.SetNickName.SetNickName_Handler" %>

using System;
using System.Web;
using DNAide.Core;
using Sunplus.Agent.Model.BL.MemberInfo.SetNickname;
using Sunplus.Agent.Model.Entities;
using Fanex.Age.UI;

namespace Sunplus.Agent.UI._MemberInfo.SetNickName
{
    public class SetNickName_Handler : Fanex.Age.UI.TplHandler
    {
        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r, true);
        }

        string nickName, status;
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            nickName = Request["nickname"];
            status = Request["status"];
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if ((string.IsNullOrEmpty(nickName) && string.IsNullOrEmpty(status)) || (nickName.Length < 5) || nickName.Length > 15)
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.msgValidNickName);
            }

            if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal)
            {
                return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }            
            
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            SetNicknameBO bo = new SetNicknameBO(UserSession.Current.NetSessionId);
            int errCode = 0;
            string errMsg = string.Empty;
            if (UserSession.Current.NickName != null && UserSession.Current.NickName != "")
            {
                if (Convert.ToBoolean(status) != UserSession.Current.IsNicknameLoginOnly)
                {
                    UserSession.Current.IsNicknameLoginOnly = Convert.ToBoolean(status);
                    bo.UpdateStatusNicknameLoginOnly(Convert.ToBoolean(status), out errMsg);
                    errMsg = Resources.Lang.updatesuccessfull;
                }
            }
            else
            {
                errCode = bo.SetNickname(nickName, out errMsg);
                if (errCode == 0)
                {
                    bo.UpdateStatusNicknameLoginOnly(Convert.ToBoolean(status), out errMsg);
                    UserSession.Current.NickName = nickName.ToUpper();
                    UserSession.Current.IsNicknameLoginOnly = Convert.ToBoolean(status);
                    errMsg = Resources.Lang.configuredNicknameSuccessfully;
                }
            }
            return new Result(errCode, errMsg);
        }

    }
}