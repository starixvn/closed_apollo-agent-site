// --------------------------------------------------------------------------
// <copyright file="AccountInfo.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.26">Created</change>
// </history>

namespace Sunplus.Agent.UI._AccountInfo
{
    using System.Collections;
    using System.Web.UI;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Entities;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Fanex.Agent.UI._AccountInfo;
    using Fanex.Common;
    using Sunplus.Agent.UI._AccountInfo.AccountInfoBase;

    public partial class AccountInfo_Page : AccountInfoBase_Page
    {
        #region Defination
        const string SESSION_MEMBERUSERNAME = "MEMBERUSERNAME";
        const string SESSION_ISMEMBER = "SESSIONISMEMBER";
        const string SELECTED_DROPDOWN = "selected";
        private int custId = -1; 
        private bool isMember = false;
        private string userNameRequest;

        private Agent agent = null;
        private Member member = null;

        public int CustId
        {
            get
            {
                return Converter.ToInt(Request.QueryString["custid"]);
            }
        }

        public string CustName 
        {
            get 
            {
                return Request.QueryString["txtUsername"];
            }
        }

        public bool IsMember 
        {
            get
            {
                return Request.QueryString["isMember"] == "1";
            }
        }

        #endregion Defination

        #region Override

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.custId = CustId == 0? -1 : CustId;
            this.userNameRequest = CustName;
            this.isMember = IsMember;

            if (!string.IsNullOrEmpty(this.userNameRequest))
            {
                this.userNameRequest = this.userNameRequest.Trim();
                Session[SESSION_MEMBERUSERNAME] = this.userNameRequest;
                Session[SESSION_ISMEMBER] = this.isMember;
            }
            else
            {
                this.userNameRequest = CustName;
                if (!string.IsNullOrEmpty(this.userNameRequest))
                {
                    this.userNameRequest = this.userNameRequest.Trim();

                    Session[SESSION_MEMBERUSERNAME] = this.userNameRequest;
                }
                else
                {
                    if (Session[SESSION_MEMBERUSERNAME] != null)
                    {
                        this.userNameRequest = Session[SESSION_MEMBERUSERNAME].ToString().Trim();
                        this.isMember = bool.Parse(Session[SESSION_ISMEMBER].ToString());
                    }

                    if (string.IsNullOrEmpty(this.userNameRequest))
                    {
                        this.userNameRequest = string.Empty;
                    }
                }
            }
        }

        public override object ValidateRequest(Hashtable p)
        {
            if (string.IsNullOrEmpty(this.userNameRequest) || this.userNameRequest.Equals(Resources.Lang.enterusername))
            {
                AccountInfo_Template tpl = new AccountInfo_Template();
                this.FillCustomerEmpty(ref tpl, string.Empty);
                return tpl.ToString();
            }

            return null;
        }

        public override object InvokeData(Hashtable p)
        {
            

            AccountInfoCriteria criteria = new AccountInfoCriteria (){ 
                CurrentAgentId = CurrentUserSession.AgentId,
                CustId = this.custId,
                IsMember = this.isMember,
                UserName = this.userNameRequest
            };

            if (this.isMember)
            {
                IMemberService service = new MemberService();
                member = service.GetCustomerInfo(criteria);
                return member;
            }
            else
            {
                IAgentService service = new AgentService();
                agent = service.GetCustomerInfo(criteria);
                return agent;
            }
        }

        #endregion Override

        #region Render Content

        public override void RenderContent(HtmlTextWriter writer)
        {
            AccountInfo_Template tpl = new AccountInfo_Template();

            if (this.isMember)
            {
                if (member == null)
                {
                    this.FillCustomerEmpty(ref tpl, Resources.Lang.usernamenotexisted);
                }
                else
                {
                    tpl.searchBox = this.RenderSearchBox();
                    tpl.TheRestAcc.settingBox = RenderSettingBox();
                    tpl.NoData = null;
                }
            }
            else
            {
                if (agent == null)
                {
                    this.FillCustomerEmpty(ref tpl, Resources.Lang.usernamenotexisted);
                }
                else
                {
                    tpl.searchBox = this.RenderSearchBox();

                    if (agent.Id.ToString() == CurrentUserSession.AgentId.ToString().Trim())
                    {
                        tpl.TheRestAcc = null;
                    }
                    else
                    {
                        tpl.TheRestAcc.settingBox = RenderSettingBox();
                    }

                    tpl.NoData = null;
                }
            }

            writer.Write(tpl);
        }

        #endregion Render Content

        public string RenderSearchBox()
        {
            SearchBox_Template tpl = new SearchBox_Template();

            if (!this.isMember)
            {
                tpl.usernamesearch = agent.Username;
                tpl.CustInfo.CustomerUsername = agent.Username;

                tpl.CustInfo.ParentUsername = agent.Parent.Username;

                tpl.CustInfo.AgentID = agent.Id.ToString();
                tpl.CustInfo.lblParentname = Resources.Lang.lblParentname;
                
                tpl.IsAgent = SELECTED_DROPDOWN;
                tpl.IsMember = string.Empty;

                tpl.ButtonBetList.Hide();

                if (!agent.HasAgentDownline)
                {
                    tpl.ButtonAgentList.Hide();
                }
                else
                {
                    tpl.ButtonAgentList.AgentId = agent.Id.ToString(); 
                    tpl.ButtonAgentList.AgentName = agent.Username;
                }

                if (!agent.HasMemberDownline)
                {
                    tpl.ButtonMemberList.Hide();
                }
                else
                {
                    tpl.ButtonMemberList.AgentId = agent.Id.ToString();
                    tpl.ButtonMemberList.AgentName = agent.Username;
                }

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
            else
            {
                tpl.usernamesearch = member.Username;
                tpl.CustInfo.CustomerUsername = member.Username;
                tpl.CustInfo.ParentUsername = member.Agent.Username;
                tpl.CustInfo.AgentID = member.Agent.Id.ToString();

                tpl.ButtonAgentList.Hide();
                tpl.ButtonMemberList.Hide();
                
                tpl.CustInfo.lblParentname = Resources.Lang.agent;
                tpl.IsAgent = string.Empty;
                tpl.IsMember = SELECTED_DROPDOWN;

                tpl.CustInfo.lblUsername = Resources.Lang.lblMember;
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
            SettingBox_Template tpl = new SettingBox_Template();
            if (this.isMember)
            {
                tpl.HiddenBlockHeaderTabAgent.Hide();
                tpl.HiddenBlockHeaderTabMember.Hide();
                tpl.HiddenBlockTabAgent.Hide();
                tpl.HiddenBlockTabMember.Hide();
            }
            else
            {
                tpl.HiddenBlockHeaderTabAgent.AgentID = agent.Id.ToString();
                tpl.HiddenBlockHeaderTabMember.AgentID = agent.Id.ToString();

                if (!agent.HasAgentDownline)
                {
                    tpl.HiddenBlockTabAgent.TreeViewBlock.Hide();
                }
                else
                {
                    tpl.HiddenBlockTabAgent.TreeViewBlock.AgentId = agent.Id.ToString();
                    tpl.HiddenBlockTabAgent.NoInfoBlock.Hide();
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

            if (this.isMember)
            {
                //Page.ResolveUrl("~/
                tplSetting.AgentSettingBlock.Hide();

                tplSetting.MemberSettingBlock.CustId = this.member.Id.ToString();
                tplSetting.MemberSettingBlock.UserName = this.member.Username;
                tplSetting.MemberSettingBlock.Group = this.member.Group;
            }
            else
            {
                tplSetting.MemberSettingBlock.Hide();

                tplSetting.AgentSettingBlock.CustId = this.agent.Id.ToString();
                tplSetting.AgentSettingBlock.UserName = this.agent.Username;
                tplSetting.AgentSettingBlock.Level = this.agent.Level.ToString();
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
                tpl.NoData.NoInputSearchBlock.usernamesearch = Resources.Lang.enterusername;
                tpl.NoData.InputSearchBlock.Hide();
            }
            else
            {
                tpl.NoData.InputSearchBlock.usernamesearch = this.userNameRequest;
                tpl.NoData.NoInputSearchBlock.Hide();

                if(this.isMember)
                {
                    tpl.NoData.IsMember = SELECTED_DROPDOWN;
                }
                else
                {
                    tpl.NoData.IsAgent = SELECTED_DROPDOWN;
                }
                
            }
        }
    }
}
