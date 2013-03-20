/*
 * Created 20100715@Simon - Render Account Info page
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._AccountInfo
{
    using System.Data;
    using Fanex.Agent.UI._AccountInfo;
    using Sunplus.Agent.Model.BL.AccountInfo;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.UI._AccountInfo.AccountInfoBase;

    public partial class AccountInfo_Page : AccountInfoBase_Page
    {
        #region Defination
        const string SESSION_MEMBERUSERNAME = "MEMBERUSERNAME";
        const string CHECKED_CHECKBOX = "checked='checked'";
        const string SELECTED_DROPDOWN = "selected";
        const string CLASS_NAME = "b_nleft";
        private int custId = -1; 
        private bool isMember = false;
        private bool isChangedUserName = false;
        private string userNameRequest;
        private DataSet dataset = null;
        private DataRow dtrCustomerInfo = null;

        #endregion Defination

        #region Override

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.custId = string.IsNullOrEmpty(Request.QueryString["custid"]) ? -1 : int.Parse(Request.QueryString["custid"].ToString());
            this.userNameRequest = Request.QueryString["txtUsername"];

            this.isMember = Request.QueryString["isMember"] == "1";

            if (!string.IsNullOrEmpty(this.userNameRequest))
            {
                this.userNameRequest = this.userNameRequest.Trim();
                if (Session[SESSION_MEMBERUSERNAME] != null)
                {
                    if (string.Compare(Session[SESSION_MEMBERUSERNAME].ToString(), this.userNameRequest, true) != 0)
                    {
                        this.isChangedUserName = true;
                    }
                }
                else
                {
                    this.isChangedUserName = true;
                }

                Session[SESSION_MEMBERUSERNAME] = this.userNameRequest;
                Session["SESSION_ISMEMBER"] = this.isMember;
            }
            else
            {
                this.userNameRequest = Request["txtUsername"];
                if (!string.IsNullOrEmpty(this.userNameRequest))
                {
                    this.userNameRequest = this.userNameRequest.Trim();
                    if (Session[SESSION_MEMBERUSERNAME] != null)
                    {
                        if (string.Compare(Session[SESSION_MEMBERUSERNAME].ToString(), this.userNameRequest, true) != 0)
                        {
                            this.isChangedUserName = true;
                        }
                    }
                    else
                    {
                        this.isChangedUserName = true;
                    }

                    Session[SESSION_MEMBERUSERNAME] = this.userNameRequest;
                }
                else
                {
                    if (Session[SESSION_MEMBERUSERNAME] != null)
                    {
                        this.userNameRequest = Session[SESSION_MEMBERUSERNAME].ToString().Trim();
                        this.isMember = bool.Parse(Session["SESSION_ISMEMBER"].ToString());
                    }

                    if (string.IsNullOrEmpty(this.userNameRequest))
                    {
                        this.userNameRequest = string.Empty;
                    }
                }
            }
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (string.IsNullOrEmpty(this.userNameRequest) || this.userNameRequest.Equals(Resources.Lang.enterusername))
            {
                AccountInfo_Template tpl = new AccountInfo_Template();
                this.FillCustomerEmpty(ref tpl, string.Empty);
                return tpl.ToString();
            }

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            AccountInfoBO bo = new AccountInfoBO(CurrentUserSession.NetSessionId);
            this.dataset = bo.Age_CustomerSelAllInfo(this.custId, this.userNameRequest, this.isMember);
            //Increase user search sugesstion
            if (this.dataset.Tables.Count > 0)
            {
                if (this.dataset.Tables[0].Rows.Count > 0)
                {
                    this.dtrCustomerInfo = this.dataset.Tables[0].Rows[0]; //Customer Info

                    if (this.isChangedUserName)
                    {
                        // bo.UpdateUserSearch(this.userNameRequest, this.dtrCustomerInfo["firstname"].ToString(), this.dtrCustomerInfo["lastname"].ToString());
                    }
                }
            }

            return this.dataset;
        }

        #endregion Override

        #region Render Content

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            AccountInfo_Template tpl = new AccountInfo_Template();

            string strBalanceBox = string.Empty, strSearchBox = string.Empty, strSettingBox = string.Empty;
            if (this.dataset.Tables.Count == 0 || this.dataset.Tables[0].Rows.Count == 0)
            {
                this.FillCustomerEmpty(ref tpl, Resources.Lang.usernamenotexisted);
            }
            else
            {

                this.custId = (int)this.dtrCustomerInfo["ObjectId"];

                tpl.searchBox = this.RenderSearchBox();

                if (this.dtrCustomerInfo["ObjectId"].ToString().Trim() == CurrentUserSession.CustId.ToString().Trim())
                {
                    tpl.TheRestAcc = null;
                }
                else
                {
                    tpl.TheRestAcc.settingBox = RenderSettingBox(dtrCustomerInfo);
                }

                tpl.NoData = null;
                // this.RenderJavascriptObject();
            }

            writer.Write(tpl);
        }

        /// <summary>
        /// Bind the javascript variable
        /// </summary>
        /*
        private void RenderJavascriptObject()
        {
            RegisterJavascriptObject("usernamerequired", Resources.Lang.usernamerequired);
            RegisterJavascriptObject("invalidvalue", Resources.Lang.invalidvalue);
            RegisterJavascriptObject("lblMemberMax", Resources.Lang.lblMemberMax);
            RegisterJavascriptObject("lblMemberMaxCredit", Resources.Lang.lblMemberMaxCredit);

            string outs = this.custRoleId == 3 ? Resources.Lang.lblMasOutst : Resources.Lang.lblAgOutst;
            string outsDesc = this.custRoleId == 3 ? Resources.Lang.lblMasterOutstanding : Resources.Lang.lblAgentOutstanding;

            RegisterJavascriptObject("lblAgOutst", outs);
            RegisterJavascriptObject("lblAgentOutstanding", outsDesc);

            if (this.custRoleId == 1)
            {
                RegisterJavascriptObject("confirmSuspendDownline", Resources.Lang.confirmSuspendMem);
                RegisterJavascriptObject("confirmSuspendMem", Resources.Lang.confirmUnSuspendMem);

                RegisterJavascriptObject("confirmCloseMem", Resources.Lang.confirmCloseMem);
                RegisterJavascriptObject("confirmOpenMem", Resources.Lang.confirmCloseMem);

                RegisterJavascriptObject("confirmEnableLiveCasino", Resources.Lang.confirmEnableLiveCasino);
                RegisterJavascriptObject("confirmDisableLiveCasino", Resources.Lang.confirmDisableLiveCasino);

                RegisterJavascriptObject("updateFailed", Resources.Lang.updateFailed);
            }
            else
            {
                RegisterJavascriptObject("confirmSuspendDownline", Resources.Lang.confirmSuspendDownline);
                RegisterJavascriptObject("confirmSuspendMem", Resources.Lang.confirmUnSuspended);

                RegisterJavascriptObject("confirmCloseMem", Resources.Lang.confirmCloseDownline);
                RegisterJavascriptObject("confirmOpenMem", Resources.Lang.confirmCloseMem);
            }

            RegisterJavascriptObject("alertCloseCustDownline", Resources.Lang.confirmCloseDownline);
            RegisterJavascriptObject("confirmAllowOutright", Resources.Lang.confirmAllowOutright);
            RegisterJavascriptObject("confirmDisableOutright", Resources.Lang.confirmDisableOutright);
            RegisterJavascriptObject("confirmDisableOutright_Member", Resources.Lang.confirmDisableOutright_Member);
            RegisterJavascriptObject("confirmDisableMAutoPT", Resources.Lang.confirmDisableMAutoPT);
            RegisterJavascriptObject("confirmEnableMAutoPT", Resources.Lang.confirmEnableMAutoPT);
            RegisterJavascriptObject("confirmClearCredit", Resources.Lang.confirmClearCredit);
            RegisterJavascriptObject("confirmClearCreditMem", Resources.Lang.confirmClearCreditMem);
            RegisterJavascriptObject("enablehr", Resources.Lang.enablesatote);
            RegisterJavascriptObject("enableopentote", Resources.Lang.enableuktote);
            RegisterJavascriptObject("downlines", Resources.Lang.downlines);

            RegisterJavascriptObject("lblAlertDisableHR", Resources.Lang.lblAlertDisableSATote);
            RegisterJavascriptObject("lblAlertDisableHRMember", Resources.Lang.lblAlertDisableSAToteMember);
            RegisterJavascriptObject("lblAlertEnableHR", Resources.Lang.lblAlertEnableSATote);

            RegisterJavascriptObject("lblAlertDisableOpenTote", Resources.Lang.lblAlertDisableUKTote);
            RegisterJavascriptObject("lblAlertDisableOpenToteMember", Resources.Lang.lblAlertDisableUKToteMember);
            RegisterJavascriptObject("lblAlertEnableOpenTote", Resources.Lang.lblAlertEnableUKTote);
            RegisterJavascriptObject("lblclearallcredit", Resources.Lang.clearallcredit);

            RegisterJavascriptObject("titleclose", Resources.Lang.close + " " + HttpUtility.HtmlEncode(this.userNameRequest.Replace("\\", "\\\\")));
            RegisterJavascriptObject("closedStatus", Resources.Lang.closed);

            // For Casino
            RegisterJavascriptObject("lblEnableCS", Resources.Lang.lblEnableCS);
            RegisterJavascriptObject("casinodownlines", Resources.Lang.casinodownlines);
            RegisterJavascriptObject("strConfirmDisableCasino", Resources.Lang.strConfirmDisableCS);
            RegisterJavascriptObject("lblAlertDisableCasinoMember", Resources.Lang.lblAlertDisableCSMember);
            RegisterJavascriptObject("strConfirmEnableCS", Resources.Lang.strConfirmEnableCS);

            // For Bingo
            RegisterJavascriptObject("lblEnableBingo", Resources.Bingo.lblEnableBingo);
            RegisterJavascriptObject("Bingodownlines", Resources.Bingo.Bingodownlines);
            RegisterJavascriptObject("strConfirmDisableBingo", Resources.Bingo.strConfirmDisableBingo);
            RegisterJavascriptObject("lblAlertDisableBingoMember", Resources.Bingo.lblAlertDisableBingoMember);
            RegisterJavascriptObject("strConfirmEnableBingo", Resources.Bingo.strConfirmEnableBingo);

            // For Number Game
            RegisterJavascriptObject("lblEnableNumber", Resources.Lang.lblEnableNumber);
            RegisterJavascriptObject("Numberdownlines", Resources.Lang.Numberdownlines);
            RegisterJavascriptObject("strConfirmDisableNumber", Resources.Lang.strConfirmDisableNumber);
            RegisterJavascriptObject("lblAlertDisableNumberMember", Resources.Lang.confirmDisableNumberGameDownline);
            RegisterJavascriptObject("strConfirmEnableNumber", Resources.Lang.strConfirmEnableNumber);

            // For Virtual Sports
            RegisterJavascriptObject("lblEnableVirtualSports", Resources.Lang.VirtualSports);
            RegisterJavascriptObject("VirtualSportsDownlines", Resources.Lang.Numberdownlines);
            RegisterJavascriptObject("strConfirmDisableVirtualSports", Resources.Lang.strConfirmDisableVirtualSports);
            RegisterJavascriptObject("lblAlertDisableVirtualSportsMember", Resources.Lang.confirmDisableVirtualSportsDownline);
            RegisterJavascriptObject("strConfirmEnableVirtualSports", Resources.Lang.strConfirmEnableVirtualSports);

            // For Double Commission
            RegisterJavascriptObject("lblEnableDoubleComm", Resources.Lang.doublecommission);
            RegisterJavascriptObject("DoubleCommDownlines", Resources.Lang.Numberdownlines);
            RegisterJavascriptObject("strConfirmDisableDoubleComm", Resources.Lang.confirmDisableDoubleComm);
            RegisterJavascriptObject("lblAlertDisableDoubleCommMember", Resources.Lang.strConfirmDisableDoubleCommMember);
            RegisterJavascriptObject("strConfirmEnableDoubleComm", Resources.Lang.strConfirmEnableDoubleComm);
            RegisterJavascriptObject("strConfirmEnableDoubleCommMember", Resources.Lang.strConfirmEnableDoubleCommMember);

            RegisterJavascriptObject("UserId", this.custId);
            RegisterJavascriptObject("CustRoleId", this.custRoleId);

            // Store role id
            Session[SESSION_MEMBERROLEID] = this.custRoleId;

            RegisterJavascriptObject("RoleId", CurrentUserSession.RoleId);

            RegisterJavascriptObject("alertUpdRacingStatus", Resources.Racing.alertUpdRacingStatus);

            RegisterJavascriptObject("IsShowRacing_151_41", UserPermission.IsShowRacing_151_41);
            RegisterJavascriptObject("IsShowRacing_151_31", UserPermission.IsShowRacing_151_31);
            RegisterJavascriptObject("IsShowRacing_151_51", UserPermission.IsShowRacing_151_51);
            RegisterJavascriptObject("IsShowRacing_152_41", UserPermission.IsShowRacing_152_41);
            RegisterJavascriptObject("IsShowRacing_152_51", UserPermission.IsShowRacing_152_51);
            RegisterJavascriptObject("IsShowRacing_153_51", UserPermission.IsShowRacing_153_51);
        }
        */
        #endregion Render Content

        public string RenderSearchBox()
        {
            SearchBox_Template tpl = new SearchBox_Template();

            #region Set Basic Information: Name, Phone, mobile, Fax

            string userName = this.dtrCustomerInfo["username"].ToString();

            tpl.classStyle = "text_f";
            tpl.usernamesearch = this.userNameRequest;
            tpl.CustInfo.CustomerUsername = userName;

            tpl.CustInfo.ParentUsername = this.dtrCustomerInfo["ParentName"].ToString();

            if (!this.isMember)
            {
                tpl.CustInfo.AgentID = this.custId.ToString();
                tpl.CustInfo.lblParentname = "Parent";
                
                tpl.IsAgent = SELECTED_DROPDOWN;
                tpl.IsMember = string.Empty;

                tpl.ButtonBetList.Hide();

                if (!bool.Parse(this.dtrCustomerInfo["HasAgentDownLine"].ToString()))
                {
                    tpl.ButtonAgentList.Hide();
                }
                else
                {
                    tpl.ButtonAgentList.agentlistlink = "../_MemberInfo/CustomerList/AgentList.aspx?custid=" + this.custId + "&us=" + userName;
                }

                if (!bool.Parse(this.dtrCustomerInfo["HasPlayerDownLine"].ToString()))
                {
                    tpl.ButtonMemberList.Hide();
                }
                else
                {
                    tpl.ButtonMemberList.memberlistlink = "../_MemberInfo/CustomerList/MemberList.aspx?custid=" + this.custId + "&us=" + userName;
                }
            }
            else
            {
                tpl.ButtonAgentList.Hide();
                tpl.ButtonMemberList.Hide();
                tpl.CustInfo.AgentID = this.dtrCustomerInfo["ParentId"].ToString();
                tpl.CustInfo.lblParentname = Resources.Lang.agent;
                tpl.IsAgent = string.Empty;
                tpl.IsMember = SELECTED_DROPDOWN;
            }



            #endregion Set Basic Information: Name, Phone, mobile, Fax

            if (this.isMember)
            {
                tpl.CustInfo.lblUsername = "Member";
            }
            else
            {
                tpl.CustInfo.lblUsername = Resources.Lang.agent;
                //switch (this.agentLevel)
                //{
                //    case 0:
                //        tpl.CustInfo.lblUsername = Resources.Lang.AgentLevel0;
                //        break;
                //    case 1:
                //        tpl.CustInfo.lblUsername = Resources.Lang.AgentLevel1;
                //        break;
                //    case 2:
                //        tpl.CustInfo.lblUsername = Resources.Lang.AgentLevel2;
                //        break;
                //    case 3:
                //        tpl.CustInfo.lblUsername = Resources.Lang.AgentLevel3;
                //        break;
                //    default:
                //        tpl.CustInfo.lblUsername = Resources.Lang.agent;
                //        break;
                //}
            }

            return tpl.ToString();
        }

        private string checkBackGroundStatus(ref string msgInfo)
        {
            bool isEnabled = this.GetDBBooleanValue(this.dtrCustomerInfo["enabled"]);
            bool isClosed = this.GetDBBooleanValue(this.dtrCustomerInfo["closed"]);
            bool isSuspend = this.GetDBBooleanValue(this.dtrCustomerInfo["suspended"]);
            if (isClosed)
            {
                msgInfo = Resources.Lang.accClosed;
                return "closed";
            }
            else if (isSuspend)
            {
                msgInfo = Resources.Lang.accSuspended;
                return "suspended";
            }
            else if (isEnabled)
            {
                msgInfo = Resources.Lang.viewlog;
                return "enabled";
            }
            else
            {
                msgInfo = Resources.Lang.accdisabled;
                return "disabled";
            }
        }

        protected string FormatUsername(string userName, bool isEnabled, bool isCurrentAgent)
        {
            string msgInfo = string.Empty;
            string className = "linkAg";

            if (isCurrentAgent)
            {
                className = this.checkBackGroundStatus(ref msgInfo);
            }

            if (!isEnabled)
            {
                userName = string.Format("<span class='textDisabled' title='{0}'>{1}</span>", Resources.Lang.accdisabled, userName);
                return userName;
            }

            //enable link to view log
            if (isCurrentAgent && UserSession.Current.SubPageAccess.ViewLog)
            {
                return string.Format("<a href='../_ViewLog/LogCustomerSetting.aspx?tabid=CustomerSetting&custname={0}' target='main' class='" + className + "' style='text-decoration:none;' title='{1}'>{0}</a>", userName, msgInfo);
            }

            return userName;
        }
    }
}
