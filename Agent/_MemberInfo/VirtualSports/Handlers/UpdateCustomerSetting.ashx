<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.VirtualSports.UpdateCustomerSetting_Handler" %>

namespace Sunplus.Agent.UI._MemberInfo.VirtualSports
{
    using System;
    using System.Web;
    using Sunplus.Agent.Model.Entities;
    using System.Globalization;
    using System.Threading;
    using System.Web.SessionState;
    using System.Collections;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Fanex.PTEngine.BusinessObjects;
    using Fanex.PTVirtualSports.Services;
    using System.Collections.Generic;

    public class UpdateCustomerSetting_Handler : Fanex.Age.UI.TplHandler, IReadOnlySessionState
    {
        private int targetCustId = 0;
        private int targetRoleId = 0;
        private bool? updateDownline = null;
        private ICustomer oldCustomer;
        private ICustomer newCustomer;
        private Hashtable otherSetting;
        private bool isChanged = false;
        private bool isMulti = false;

        // "All": 0,
        // "Status": 1,
        // "BetSetting": 2,
        // "PositionTaking": 3
        private byte updateType = 0;
        private List<Fanex.Age.UI.Result> uiResult = new List<Fanex.Age.UI.Result>();

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            Response.Write(JsonConvert.SerializeObject(View, settings));
            Response.End();
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            int.TryParse(Request["targetCustId"], out targetCustId);
            int.TryParse(Request["targetRoleId"], out targetRoleId);
            byte.TryParse(Request["updateType"], out updateType);
            bool.TryParse(Request["isMulti"], out isMulti);
            if (!string.IsNullOrEmpty(Request["updateDownline"]))
            {
                updateDownline = Convert.ToBoolean(Request["updateDownline"]);
            }

            oldCustomer = JsonConvert.DeserializeObject<Customer>(Request.Form[0]);
            newCustomer = JsonConvert.DeserializeObject<Customer>(Request.Form[1]);
            otherSetting = JsonConvert.DeserializeObject<Hashtable>(Request.Form[2]);

            if (isMulti)
            {
                // update oldCustomer
                var customerService = new CustomerService();
                IRequestContext currentContext = CreateCurrentContext();

                oldCustomer = customerService.GetCustomer(currentContext);
            }

            switch (updateType)
            {
                case 0:
                    break;
                case 1: // status
                    oldCustomer.BettingLimit = newCustomer.BettingLimit;
                    oldCustomer.PtSetting = newCustomer.PtSetting;
                    break;
                case 2: // bet setting
                    oldCustomer.StatusSetting = newCustomer.StatusSetting;
                    oldCustomer.PtSetting = newCustomer.PtSetting;
                    break;
                case 3: // position taking
                    oldCustomer.StatusSetting = newCustomer.StatusSetting;
                    oldCustomer.BettingLimit = newCustomer.BettingLimit;
                    break;
            }
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal)
            {
                return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            return null;
        }

        private IRequestContext CreateCurrentContext()
        {
            IRequestContext currentContext = new RequestContext
            {
                CustId = CurrentUserSession.CustId,
                TargetCustId = targetCustId,
                TargetRoleId = targetRoleId,
                FormId = FormType.EditPT,
                SubAccId = CurrentUserSession.SubAccId,
                AdminId = CurrentUserSession.UserId,
                Language = CurrentUserSession.LangKey,
                LangKey = CurrentUserSession.LangKey,
                IP = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP()
            };

            IUserSession userSession = UserSession.Current;

            if (userSession.IsSuper)
            {
                currentContext.RoleId = (int)UserSessionBase.UserTypes.Super;
            }
            else if (userSession.IsMaster)
            {
                currentContext.RoleId = (int)UserSessionBase.UserTypes.Master;
            }
            else if (userSession.IsAgent)
            {
                currentContext.RoleId = (int)UserSessionBase.UserTypes.Agent;
            }

            return currentContext;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            if (newCustomer != null)
            {
                IRequestContext currentContext = CreateCurrentContext();

                CustomerService customerService = new CustomerService();
                var results = customerService.SaveCustomer(oldCustomer, newCustomer, currentContext, otherSetting);

                UpdateStatusMessages(currentContext, results.ResultStatus);
                UpdateBettingLimitMessages(currentContext, results.ResultBetSetting);
                UpdatePTMessages(currentContext, results.ResultPT);

                if (!isChanged)
                {
                    return new Fanex.Age.UI.Result(50, Resources.Lang.noChangeToUpdate);
                }

                return uiResult.ToArray();
            }

            return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
        }

        private void UpdateStatusMessages(IRequestContext currentContext, IList<Result> results)
        {
            if (results.Count > 0)
            {
                isChanged = true;
                string msg = Resources.Lang.status + ": ";
                
                foreach (var r in results)
                {
                    int errCode = Convert.ToInt32(r.ErrorCode);
                    if (errCode == 0)
                    {
                        errCode = -2;
                        r.ErrorMessage = msg + Resources.Lang.updatesuccessfull;
                    }
                    else if (errCode == 101 || errCode == 102 || errCode == 103 || errCode == 104)
                    {
                        r.ErrorMessage = msg + r.ErrorMessage;
                    }

                    uiResult.Add(new Fanex.Age.UI.Result(errCode, r.ErrorMessage));
                }
            }
        }

        private void UpdateBettingLimitMessages(IRequestContext currentContext, IList<Fanex.PTVirtualSports.BusinessObjects.Result> results)
        {
            if (results.Count > 0)
            {
                isChanged = true;
                foreach (var r in results)
                {
                    int errCode = Convert.ToInt32(r.ErrorCode);
                    string msg1 = Resources.Lang.lblMinBet + "/" + Resources.Lang.lblMaxBet + ": ";
                    string msg2 = Resources.Lang.lblMaxPerMatch + ": ";
                    
                    switch (errCode)
                    {
                        case -101:
                            r.ErrorMessage = msg1 + Resources.Lang.updatesuccessfull;
                            break;
                        case -102:
                            r.ErrorMessage = msg2 + Resources.Lang.updatesuccessfull;
                            break;
                        default:
                            switch (currentContext.TargetRoleId)
                            {
                                case Constant.MASTER_ROLEID:
                                    if (errCode == 147)
                                    {
                                        r.ErrorMessage = msg1 + r.ErrorMessage;
                                    }
                                    else if (errCode == 115)
                                    {
                                        r.ErrorMessage = msg2 + r.ErrorMessage;
                                    }
                                    break;

                                case Constant.AGENT_ROLEID:
                                    if (errCode == 105)
                                    {
                                        r.ErrorMessage = msg1 + r.ErrorMessage;
                                    }
                                    else if (errCode == 115)
                                    {
                                        r.ErrorMessage = msg2 + r.ErrorMessage;
                                    }
                                    break;

                                case Constant.MEMBER_ROLEID:
                                    if (errCode == 106)
                                    {
                                        r.ErrorMessage = msg1 + r.ErrorMessage;
                                    }
                                    else if (errCode == 115)
                                    {
                                        r.ErrorMessage = msg2 + r.ErrorMessage;
                                    }
                                    break;
                            }
                            
                            break;
                    }

                    string sportName = CacheRef.GetSportName(r.SportTypeId);
                    uiResult.Add(new Fanex.Age.UI.Result(errCode, sportName + " - " + r.ErrorMessage));
                }
            }
        }

        private void UpdatePTMessages(IRequestContext currentContext, IList<Fanex.PTVirtualSports.BusinessObjects.Result> results)
        {
            if (results.Count > 0)
            {
                isChanged = true;
                foreach (var r in results)
                {
                    int errCode = Convert.ToInt32(r.ErrorCode);
                    string msg1 = Resources.Lang.positiontaking + ": ";
                    string msg2 = Resources.Lang.minPT + ": ";
                    
                    switch (errCode)
                    {
                        case -101:
                            r.ErrorMessage = msg1 + Resources.Lang.updatesuccessfull;
                            break;
                        case -102:
                            r.ErrorMessage = msg2 + Resources.Lang.updatesuccessfull;
                            break;
                        case -103:
                            r.ErrorMessage = msg1 + Resources.Lang.noChangeToUpdate;
                            break;
                        default:
                            switch (currentContext.TargetRoleId)
                            {
                                case Constant.MASTER_ROLEID:
                                    if (errCode == 147)
                                    {
                                        r.ErrorMessage = msg1 + r.ErrorMessage;
                                    }
                                    else if (errCode == 115)
                                    {
                                        r.ErrorMessage = msg2 + r.ErrorMessage;
                                    }
                                    break;

                                case Constant.AGENT_ROLEID:
                                    if (errCode == 105)
                                    {
                                        r.ErrorMessage = msg1 + r.ErrorMessage;
                                    }
                                    else if (errCode == 115)
                                    {
                                        r.ErrorMessage = msg2 + r.ErrorMessage;
                                    }
                                    break;

                                case Constant.MEMBER_ROLEID:
                                    switch (currentContext.RoleId)
                                    {
                                        case Constant.SUPER_ROLEID:
                                            if (errCode == 106)
                                            {
                                                r.ErrorMessage = msg1 + r.ErrorMessage;
                                            }
                                            else if (errCode == 115)
                                            {
                                                r.ErrorMessage = msg2 + r.ErrorMessage;
                                            }
                                            else if (errCode == 99)
                                            {
                                                r.ErrorMessage = msg1 + r.ErrorMessage;
                                            }
                                            break;

                                        case Constant.MASTER_ROLEID:
                                            if (errCode == 106)
                                            {
                                                r.ErrorMessage = msg1;
                                            }
                                            else if (errCode == 115)
                                            {
                                                r.ErrorMessage = msg2;
                                            }
                                            else if (errCode == 98)
                                            {
                                                r.ErrorMessage = msg1 + r.ErrorMessage;
                                            }
                                            break;

                                        case Constant.AGENT_ROLEID:
                                            if (errCode == 106)
                                            {
                                                r.ErrorMessage = msg1 + r.ErrorMessage;
                                            }
                                            else if (errCode == 115)
                                            {
                                                r.ErrorMessage = msg2 + r.ErrorMessage;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            
                            break;
                    }

                    string sportName = CacheRef.GetSportName(r.SportTypeId);
                    uiResult.Add(new Fanex.Age.UI.Result(errCode, sportName + " - " + r.ErrorMessage));
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}