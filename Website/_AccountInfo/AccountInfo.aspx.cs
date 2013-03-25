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
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.DTO.Accounts;

    public partial class AccountInfo_Page : AccountInfoBase_Page
    {
        #region Defination
        const string HTML_IFRAME = "<iframe id='frmDownlineList' name='frmDownlineList' src='../_AccountInfo/DownLineTreeView.aspx?root={0}' style='width:98%;height: 543px'></iframe>";
        const string SESSION_MEMBERUSERNAME = "MEMBERUSERNAME";
        const string CHECKED_CHECKBOX = "checked='checked'";
        const string SELECTED_DROPDOWN = "selected";
        const string CLASS_NAME = "b_nleft";
        private int custId = -1; 
        private bool isMember = false;
        private bool isChangedUserName = false;
        private string userNameRequest;

        private AccountInfoDTO accountInfo = null;

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
            IAccountInfoService service = new AccountInfoService();

            AccountInfoCriteria criteria = new AccountInfoCriteria (){ 
                CurrentAgentId = CurrentUserSession.AgentId,
                CustId = this.custId,
                IsMember = this.isMember,
                UserName = this.userNameRequest
            };

            accountInfo = service.GetCustomerInfo(criteria);

            return accountInfo;
        }

        #endregion Override

        #region Render Content

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            AccountInfo_Template tpl = new AccountInfo_Template();

            if (accountInfo == null)
            {
                this.FillCustomerEmpty(ref tpl, Resources.Lang.usernamenotexisted);
            }
            else
            {
                tpl.searchBox = this.RenderSearchBox();

                if (accountInfo.Id.ToString() == CurrentUserSession.AgentId.ToString().Trim())
                {
                    tpl.TheRestAcc = null;
                }
                else
                {
                    tpl.TheRestAcc.settingBox = RenderSettingBox();
                }

                tpl.NoData = null;
            }

            writer.Write(tpl);
        }

        #endregion Render Content

        public string RenderSearchBox()
        {
            SearchBox_Template tpl = new SearchBox_Template();

            tpl.classStyle = "text_f";
            tpl.usernamesearch = accountInfo.UserName;
            tpl.CustInfo.CustomerUsername = accountInfo.UserName;

            tpl.CustInfo.ParentUsername = accountInfo.ParentName;

            if (!this.isMember)
            {
                tpl.CustInfo.AgentID = accountInfo.Id.ToString();
                tpl.CustInfo.lblParentname = Resources.Lang.lblParentname;
                
                tpl.IsAgent = SELECTED_DROPDOWN;
                tpl.IsMember = string.Empty;

                tpl.ButtonBetList.Hide();

                if (!accountInfo.HasAgentDownline)
                {
                    tpl.ButtonAgentList.Hide();
                }
                else
                {
                    tpl.ButtonAgentList.agentlistlink = "../_MemberInfo/CustomerList/AgentList.aspx?custid=" + accountInfo.Id + "&us=" + accountInfo.UserName;
                }

                if (!accountInfo.HasMemberDownline)
                {
                    tpl.ButtonMemberList.Hide();
                }
                else
                {
                    tpl.ButtonMemberList.memberlistlink = "../_MemberInfo/CustomerList/MemberList.aspx?custid=" + accountInfo.Id + "&us=" + accountInfo.UserName;
                }
            }
            else
            {
                tpl.ButtonAgentList.Hide();
                tpl.ButtonMemberList.Hide();
                tpl.CustInfo.AgentID = accountInfo.ParentId.ToString();
                tpl.CustInfo.lblParentname = Resources.Lang.agent;
                tpl.IsAgent = string.Empty;
                tpl.IsMember = SELECTED_DROPDOWN;
            }

            if (this.isMember)
            {
                tpl.CustInfo.lblUsername = Resources.Lang.lblMember;
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

        #region Render Setting Box

        /// <summary>
        /// Render Setting box
        /// </summary>
        /// <returns></returns>
        public string RenderSettingBox()
        {
            TreeViewNodeDTO.Root= accountInfo.Id;
            TreeViewNodeDTO.Type = 1; //1: tree in tab account
            // Use DataRow row1, row2, row3.
            SettingBox_Template tpl = new SettingBox_Template();

            if (accountInfo.IsMember)
            {
                tpl.HiddenBlockHeaderTabAgent.Hide();
                tpl.HiddenBlockHeaderTabMember.Hide();
                tpl.HiddenBlockTabAgent.Hide();
                tpl.HiddenBlockTabMember.Hide();
            }
            else
            {
                tpl.HiddenBlockHeaderTabAgent.AgentID = accountInfo.Id.ToString();
                tpl.HiddenBlockHeaderTabMember.AgentID = accountInfo.Id.ToString();

                if (!accountInfo.HasAgentDownline)
                {
                    tpl.HiddenBlockTabAgent.TreeViewDownLineFrame = "<span class=\"no-data-agent\"> " + Resources.Lang.noInfo + "<span>";
                }
                else
                {
                    tpl.HiddenBlockTabAgent.TreeViewDownLineFrame = string.Format(HTML_IFRAME, accountInfo.Id.ToString());
                }
            }

            this.RenderSettingBoxEditLink(tpl);
            return tpl.ToString();
        }

        /// <summary>
        /// Render edit link in settting box
        /// Edit info
        /// </summary>
        /// <param name="tpl"></param>
        /// <param name="isUpdaterDisableCasino"></param>
        /// <param name="isSyncCasino"></param>
        /// <param name="isUpdaterTransferDaily"></param>
        private void RenderSettingBoxEditLink(SettingBox_Template tpl)
        {
            //Other settings
            SettingBoxView_Template tplSetting = new SettingBoxView_Template();

            if (this.accountInfo.IsMember)
            {
                //Page.ResolveUrl("~/
                tplSetting.ForDirectDownline.editSportBookLink = Response.ApplyAppPathModifier(string.Format("~/_MemberInfo/CustomerSetting/Pages/EditMemberSetting.aspx?custId={0}&username={1}&showheader=1&ugroup={2}", this.accountInfo.Id, this.accountInfo.UserName, this.accountInfo.UserGroup));
                tplSetting.ForDirectDownline.editCommLink = Response.ApplyAppPathModifier(string.Format("~/_MemberInfo/CustomerSetting/Pages/EditMemberCommission.aspx?custId={0}&username={1}&showheader=1&ugroup={2}", this.accountInfo.Id, this.accountInfo.UserName, this.accountInfo.UserGroup));
                tplSetting.ForDirectDownline.editBetSettingLink = Response.ApplyAppPathModifier(string.Format("~/_MemberInfo/CustomerSetting/Pages/EditMemberBetLimit.aspx?custId={0}&username={1}&showheader=1&ugroup={2}", this.accountInfo.Id, this.accountInfo.UserName, this.accountInfo.UserGroup));
                tplSetting.ForDirectDownline.editPTLink = Response.ApplyAppPathModifier(string.Format("~/_MemberInfo/CustomerSetting/Pages/EditMemberPositionTaking.aspx?custId={0}&username={1}&showheader=1&ugroup={2}", this.accountInfo.Id, this.accountInfo.UserName, this.accountInfo.UserGroup));
            }
            else
            {
                tplSetting.ForDirectDownline.editSportBookLink = Response.ApplyAppPathModifier(string.Format("~/_MemberInfo/CustomerSetting/Pages/EditAgentSetting.aspx?custId={0}&username={1}&showheader=1", this.accountInfo.Id, this.accountInfo.UserName));
                tplSetting.ForDirectDownline.editCommLink = Response.ApplyAppPathModifier(string.Format("~/_MemberInfo/CustomerSetting/Pages/EditAgentCommission.aspx?custId={0}&username={1}&showheader=1", this.accountInfo.Id, this.accountInfo.UserName));
                tplSetting.ForDirectDownline.editBetSettingLink = Response.ApplyAppPathModifier(string.Format("~/_MemberInfo/CustomerSetting/Pages/EditAgentBetLimit.aspx?custId={0}&username={1}&showheader=1", this.accountInfo.Id, this.accountInfo.UserName));
                tplSetting.ForDirectDownline.editPTLink = Response.ApplyAppPathModifier(string.Format("~/_MemberInfo/CustomerSetting/Pages/EditAgentPositionTaking.aspx?custId={0}&username={1}&showheader=1", this.accountInfo.Id, this.accountInfo.UserName));
            }

            tpl.otherSettings = tplSetting.ToString();
        }

        #endregion Render Setting Box

        /// <summary>
        /// Fill data in case the customer can not be found
        /// </summary>
        /// <param name="tpl"></param>
        /// <param name="errMsg"></param>
        public void FillCustomerEmpty(ref AccountInfo_Template tpl, string errMsg)
        {
            tpl.searchBox = null;
            tpl.TheRestAcc = null;

            tpl.NoData.errMsg = errMsg;
            if (string.IsNullOrEmpty(errMsg))
            {
                tpl.NoData.classStyle = "text_f1";
                tpl.NoData.usernamesearch = Resources.Lang.enterusername;
            }
            else
            {
                tpl.NoData.classStyle = "text_f";
                tpl.NoData.usernamesearch = this.accountInfo.UserName;
            }
        }
    }
}
