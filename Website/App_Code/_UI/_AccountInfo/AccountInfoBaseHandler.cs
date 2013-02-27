
namespace Sunplus.Agent.UI._AccountInfo
{
    public class AccountInfoBase_Handler : Fanex.Age.UI.TplHandler, System.Web.SessionState.IRequiresSessionState
    {
        #region Variable
        protected int _errCode = 0;
        protected string _errMsg = "";
        #endregion

        public AccountInfoBase_Handler()
        {
            IsValidateRequest = false;
        }

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            data.Add("errCode", _errCode);
            data.Add("errMsg", _errMsg);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {

        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            Sunplus.Agent.Model.BL.Authorization.AuthorizationBO authorizationBO = new Sunplus.Agent.Model.BL.Authorization.AuthorizationBO(CurrentUserSession.NetSessionId);
            bool canAccess = authorizationBO.CheckPageAccess(Sunplus.Agent.Model.Entities.PageAccessType.UpdateAccount, out _errCode, out _errMsg);
            if (!canAccess) return new Fanex.Age.UI.Result(_errCode, _errMsg);
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {

            return null;
        }
    }
}
