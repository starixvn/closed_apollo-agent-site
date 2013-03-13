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

        const int MAXLENGTH = 15;
        const string SESSION_MEMBERUSERNAME = "MEMBERUSERNAME";
        const string SESSION_MEMBERROLEID = "SESSION_MEMBERROLEID";
        const string DISABLE_HTML_ELEMENT = "disabled='disabled'";
        const string ENABLE_HTML_ELEMENT = "style='cursor:pointer;'";
        const string CHECKED_CHECKBOX = "checked='checked'";
        const string CLASS_NAME = "b_nleft";
        private int custId = -1; // custRoleId = -1;
        private int agentLevel = -1;
        private bool isMember = false;
        private bool isChangedUserName = false;
        private string userNameRequest;
        private DataSet dataset = null;
        // private DataRow dtrUpdaterInfo = null, dtrCustomerInfo = null, dtrDownlineInfo = null, dtrBalanceInfo = null;
        private DataRow dtrCustomerInfo = null;

        #endregion Defination

        //[System.Web.Services.WebMethod]
        //[System.Web.Script.Services.ScriptMethod(UseHttpGet = true)]
        //public static List<TreeViewNode> FileSystemInfos(string root)
        //{
        //    int? nodeId = (root == "source") ? (int?)null : Convert.ToInt32(root);
        //    // IEnumerable<FileSystemInfo> children = FileSystemInfosRepository.GetFileSystemInfos(nodeId);

        //    //AccountInfo_Page page = new AccountInfo_Page();
        //    //AccountInfoBO bo = new AccountInfoBO(page.CurrentUserSession.NetSessionId);
        //    //var ds = bo.Age_Customer_GetDownLine(page.CurrentUserSession.CustId);

        //    //var dtAgentList = ds.Tables[0];
        //    //var dtCustList = ds.Tables[1];

        //    List<TreeViewNode> nodes = new List<TreeViewNode>();
        //    //foreach (DataRow row in dtAgentList.Rows)
        //    //{
        //    //    bool isAgent = true;
        //    //    nodes.Add(new TreeViewNode()
        //    //    {
        //    //        Id = "Agent_" + row["AgentId"].ToString(),
        //    //        Text = row["Username"].ToString(),
        //    //        Classes = "folder",
        //    //        HasChildren = isAgent
        //    //    });
        //    //}

        //    //foreach (DataRow row in dtAgentList.Rows)
        //    //{
        //    //    bool isAgent = false;
        //    //    nodes.Add(new TreeViewNode()
        //    //    {
        //    //        Id = "Agent_" + row["PlayerId"].ToString(),
        //    //        Text = row["Username"].ToString(),
        //    //        Classes = "file",
        //    //        HasChildren = isAgent
        //    //    });
        //    //}

        //    return nodes;
        //}


        #region Override

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.userNameRequest = Request.QueryString["username"];

            this.isMember = Request.QueryString["isMember"] == "on" ? true : false;
            
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
            this.dataset = bo.GetCustomerInfo(this.userNameRequest, this.isMember);
            //Increase user search sugesstion
            if (this.dataset.Tables.Count > 0)
            {
                if (this.dataset.Tables[0].Rows.Count > 0)
                {
                    //this.dtrUpdaterInfo = this.dataset.Tables[0].Rows[0]; //Updater Info
                    this.dtrCustomerInfo = this.dataset.Tables[0].Rows[0]; //Customer Info
                    //this.dtrDownlineInfo = this.dataset.Tables[2].Rows[0]; //Downline Info
                    //this.dtrBalanceInfo = null;                 //Balance Info

                    //if (this.dataset.Tables.Count > 3)
                    //{
                    //    this.dtrBalanceInfo = this.dataset.Tables[3].Rows[0];
                    //}

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
                this.agentLevel = string.IsNullOrEmpty(this.dtrCustomerInfo["AgentLevel"].ToString()) ? 0 : int.Parse(this.dtrCustomerInfo["AgentLevel"].ToString());
                // this.custRoleId = (byte)this.dtrCustomerInfo["roleid"];

                //if (!this.IsMemberDownlineMe(this.dtrDownlineInfo))
                //{
                //    this.FillCustomerEmpty(ref tpl, Resources.Lang.usernamenotexisted);
                //}
                //else
                //{
                tpl.searchBox = this.RenderSearchBox();

                if (this.dtrCustomerInfo["ObjectId"].ToString().Trim() == CurrentUserSession.CustId.ToString().Trim())
                {
                    tpl.TheRestAcc = null;
                }
                else
                {
                    //tpl.TheRestAcc.balanceBox = RenderBalanceBox(dtrCustomerInfo, dtrBalanceInfo);
                    tpl.TheRestAcc.settingBox = RenderSettingBox(dtrCustomerInfo);
                }
                //}

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

        #region Render Search Box

        public string RenderSearchBox()
        {
            // Use DataRow row1, row2.
            SearchBox_Template tpl = new SearchBox_Template();
            string htmlFrame = "<iframe id='frmDownlineList' name='frmDownlineList' src='DownLineTreeView.aspx?root={0}' style='width:95%;height:100px;'></iframe>";

            #region Set Basic Information: Name, Phone, mobile, Fax

            string userName = this.dtrCustomerInfo["username"].ToString();
            /*
            string fullName = this.dtrCustomerInfo["firstname"].ToString() + " " + this.dtrCustomerInfo["lastname"].ToString();
            string phone = this.dtrCustomerInfo["phone"].ToString();
            string mobilephone = this.dtrCustomerInfo["mobilephone"].ToString();
            string fax = this.dtrCustomerInfo["fax"].ToString();
            */
            tpl.classStyle = "text_f";
            tpl.usernamesearch = this.userNameRequest;
            tpl.CustInfo.CustomerUsername = userName;

            tpl.CustInfo.lblParentname = "Parent";
            tpl.CustInfo.ParentUsername = this.dtrCustomerInfo["ParentName"].ToString();

            //if (custId > 0)
            //{
            //    tpl.CustInfo.TreeViewDownLineFrame = string.Format(htmlFrame, this.custId);
            //}

            

            if (!bool.Parse(this.dtrCustomerInfo["HasAgentDownLine"].ToString()))
            {
                tpl.ButtonAgentList.Hide();
            }

            if (!bool.Parse(this.dtrCustomerInfo["HasPlayerDownLine"].ToString()))
            {
                tpl.ButtonMemberList.Hide();
            }

            if (!bool.Parse(this.dtrCustomerInfo["IsMember"].ToString()))
            {
                tpl.ButtonBetList.Hide();
            }

            // tpl.CustInfo.CustomerUsername = this.FormatUsername(userName, string.IsNullOrEmpty(this.dtrCustomerInfo["enabled"].ToString()) ? false : Convert.ToBoolean(this.dtrCustomerInfo["enabled"].ToString()), true); //if string exceed this MaxLength, it will be truncated with tooltip!
            /*
           if ((CurrentUserSession.RoleId - int.Parse(this.dtrCustomerInfo["roleid"].ToString())) == 1)
           {
               string nickName = this.dtrCustomerInfo["username2"].ToString();
               
               if (string.IsNullOrEmpty(nickName))
               {
                   tpl.CustInfo.blCustomerNickname = null;
               }
               else
               {
                   tpl.CustInfo.blCustomerNickname.CustomerNickname = Formater.TruncateWithTip(nickName, MAXLENGTH);
               }
                 
           }
           else
           {
               tpl.CustInfo.blCustomerNickname = null;
           }
           
           if (string.IsNullOrEmpty(fullName.Trim()))
           {
               tpl.CustInfo.blCustomerFullName = null;
           }
           else
           {
               tpl.CustInfo.blCustomerFullName.CustomerFullName = Formater.TruncateWithTip(fullName, MAXLENGTH);
           }

           if (string.IsNullOrEmpty(phone))
           {
               tpl.CustInfo.blCustomerPhone = null;
           }
           else
           {
               tpl.CustInfo.blCustomerPhone.CustomerPhone = Formater.TruncateWithTip(phone, MAXLENGTH);
           }
            
           if (string.IsNullOrEmpty(mobilephone))
           {
               tpl.CustInfo.blCustomerMobile = null;
           }
           else
           {
               tpl.CustInfo.blCustomerMobile.CustomerMobile = Formater.TruncateWithTip(mobilephone, MAXLENGTH);
           }

           if (string.IsNullOrEmpty(fax))
           {
               tpl.CustInfo.blCustomerFax = null;
           }
           else
           {
               tpl.CustInfo.blCustomerFax.CustomerFax = Formater.TruncateWithTip(fax, MAXLENGTH);
           }
           */
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


            #region Name/List/Statment Link

            string[] statementLink = { string.Format("../_Reports/BetList/DailyStatement.aspx?username={0}&option=1&custid={1}&chk_ShowSB=on&chk_ShowCasino=on&chk_ShowRB=on&chk_showng=off&chk_ShowBI=on", userName, this.custId),
                                   string.Format("../_Reports/Common/Statement.aspx?custid={0}&roleid=2", this.custId),
                                   string.Format("../_Reports/Common/Statement.aspx?custid={0}&roleid=3", this.custId),
                                   string.Format("../_Reports/Common/Statement.aspx")
                                 };

            string[] roleName = { Resources.Lang.lblBetLists, Resources.Lang.memberList, Resources.Lang.agentList, Resources.Lang.masterlist, Resources.Lang.lblPlayerList };

            string[] customerListLink = { string.Format("../_Reports/BetList/OutstandingDetail.aspx?username={0}&option=4&custid={1}&chk_all=on&chk_ShowSB=on&chk_ShowCasino=on&chk_ShowRB=on&chk_showng=on&chk_ShowBI=on", userName, this.custId),
                              string.Format("../_MemberInfo/CustomerList/MemberList.aspx?custid={0}", this.custId),
                              string.Format("../_MemberInfo/CustomerList/AgentList.aspx?custid={0}", this.custId),
                              string.Format("../_MemberInfo/CustomerList/AgentList.aspx"),
                              string.Format("../_MemberInfo/CustomerList/MemberList.aspx?custid={0}", this.custId),
                              string.Format("../_MemberInfo/CustomerList/AgentList.aspx"),
                              string.Format("../_MemberInfo/CustomerList/MemberList.aspx")
                            };

            #region Group Customer List
            /*
            if (CurrentUserSession.SubPageAccess.ViewMemberInfo)
            {
                tpl.GroupCustomerList.memberlist = roleName[this.custRoleId - 1];

                if (roleName[this.custRoleId - 1] == Resources.Lang.lblBetLists)
                {
                    tpl.GroupCustomerList.memberlistlink = customerListLink[0];
                }
                else
                {
                    if (CurrentUserSession.IsSuper)
                    {
                        tpl.GroupCustomerList.memberlistlink = customerListLink[this.custRoleId - 1];
                    }
                    else if (CurrentUserSession.IsMaster)
                    {
                        tpl.GroupCustomerList.memberlistlink = customerListLink[this.custRoleId + 2];
                    }
                    else
                    {
                        tpl.GroupCustomerList.memberlistlink = customerListLink[this.custRoleId + 4];
                    }
                }
            }
            else
            {
                tpl.GroupCustomerList.Hide();
            }
            */
            if (CurrentUserSession.SubPageAccess.ViewMemberInfo)
            {
                /*
                tpl.CustInfo.memberlist = roleName[this.custRoleId - 1];
                tpl.CustInfo.lblPlayerList = roleName[4];

                if (roleName[this.custRoleId - 1] == Resources.Lang.lblBetLists)
                {
                    tpl.CustInfo.memberlistlink = customerListLink[0];
                }
                else
                {
                    if (CurrentUserSession.IsSuper)
                    {
                        tpl.CustInfo.memberlistlink = customerListLink[this.custRoleId - 1];
                    }
                    else if (CurrentUserSession.IsMaster)
                    {
                        tpl.CustInfo.memberlistlink = customerListLink[this.custRoleId + 2];
                    }
                    else
                    {
                        tpl.CustInfo.memberlistlink = customerListLink[this.custRoleId + 4];
                    }
                }
                */
            }
            else
            {
                tpl.CustInfo.Hide();
            }
            #endregion Group Customer List

            #region Group Statement

            if (CurrentUserSession.SubPageAccess.ViewReports)
            {
                // tpl.GroupStatement.statementlink = statementLink[this.custRoleId - 1];
            }
            else
            {
                tpl.GroupStatement.Hide();
            }

            #endregion Group Statement

            #endregion Name/List/Statment Link

            //Upline Info:
            //tpl.fornotdirectdownline.uplineinfo.SetValue(this.GenerateUplineInfo(this.custRoleId));

            //If enter itself, just show the customer section only!
            //if (this.IsDirectDownline(this.custRoleId) || this.userNameRequest.Equals(CurrentUserSession.UserName, StringComparison.OrdinalIgnoreCase))
            //{
            //    tpl.fornotdirectdownline = null;
            //}

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

        /*
        private string GenerateUplineInfo(int custRoleId)
        {
            // Use DataRow row2
            bool isupLineEnabled;
            string uplineInfo = string.Empty;
            string msgInfo = string.Empty;
            string[] roleName = { string.Empty, "agent", "master", "super" };
            string[] fieldAgentNameList = { string.Empty, "agentName", "masterName", "superName" };

            for (int i = CurrentUserSession.RoleId - 2; i >= custRoleId; i--)
            {
                SearchBox_fornotdirectdownline_uplineinfo_Block tpl = new SearchBox_fornotdirectdownline_uplineinfo_Block();
                string fieldAgentName = fieldAgentNameList[i];
                string fieldAgentNameValue = this.GetDBStringValue(this.dtrDownlineInfo[fieldAgentName]);
                if (fieldAgentNameValue.Equals(string.Empty))
                {
                    break;
                }

                // For Super View Member
                if (custRoleId == 1 && roleName[i] == "master")
                {
                    isupLineEnabled = true;
                }
                else
                {
                    isupLineEnabled = this.GetDBBooleanValue(this.dtrCustomerInfo["uplineEnabled"]);
                }
               
                tpl.agent = GetGlobalResourceObject("lang", roleName[i]).ToString();
                tpl.agentname = this.FormatUsername(fieldAgentNameValue, isupLineEnabled, false);
                tpl.AgentNameValue = fieldAgentNameValue;

                uplineInfo += tpl.ToString();
            }

            return uplineInfo;
        }
        */
        #endregion Render Search Box


    }
}
