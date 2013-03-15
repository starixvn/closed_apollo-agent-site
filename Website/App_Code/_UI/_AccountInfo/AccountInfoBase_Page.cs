namespace Sunplus.Agent.UI._AccountInfo.AccountInfoBase
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Age.UI;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.UI._MemberInfo.CustomerList.Components;
    using Sunplus.Agent.Model.BL.Definitions;
    using Fanex.LiveCasinoSetting.Business;
    using Sunplus.Agent.Utilities;
    using Fanex.Agent.UI._AccountInfo;    

    public abstract class AccountInfoBase_Page : TplPage
    {
        #region Variable
        
        private int custId = -1, custRoleId = -1;
        private bool isMember = false;
        const string CLASS_NAME = "b_nleft";
        const string DISABLE_HTML_ELEMENT = "disabled='disabled'";
        const string ENABLE_HTML_ELEMENT = "style='cursor:pointer;'";
        const string CHECKED_CHECKBOX = "checked='checked'";   
        public string userNameRequest;
        #endregion

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {

        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {

            return null;
        }

        #region Utility

        public bool IsDirectDownline(int roleId)
        {
            return CurrentUserSession.RoleId - roleId == 1;
        }

        /// <summary>
        /// Get DB boolean value
        /// If null, return false
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool GetDBBooleanValue(object data)
        {
            if (Convert.IsDBNull(data))
            {
                return false;
            }

            return (bool)data;
        }

        /// <summary>
        /// Get DB disable boolean value
        /// If null, return false
        /// If data = true return false
        /// else return true
        /// Use for disable value:
        ///         disable HR
        ///         disable Outright
        ///         .............
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool GetDBEnabledBooleanValue(object data)
        {
            if (Convert.IsDBNull(data))
            {
                return false;
            }

            return (bool)data ? false : true;
        }

        /// <summary>
        /// Get int value from database
        /// return 0 if the object is DBNull
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int GetDBIntValue(object data)
        {
            if (Convert.IsDBNull(data))
            {
                return 0;
            }

            return (int)data;
        }

        /// <summary>
        /// Get decimal value from database
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public decimal GetDBDecimalValue(object data)
        {
            if (Convert.IsDBNull(data))
            {
                return 0;
            }

            return (decimal)data;
        }

        /// <summary>
        /// Get string value from database
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetDBStringValue(object data)
        {
            if (Convert.IsDBNull(data))
            {
                return string.Empty;
            }

            return data.ToString();
        }

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
                tpl.NoData.usernamesearch = this.userNameRequest;
            }
        }

        public bool IsMemberDownlineMe(DataRow memberLine)
        {
            string[] arrColumParent = { "agentID", "masterID", "superID" };
            return CurrentUserSession.RoleId > 1 && memberLine[arrColumParent[CurrentUserSession.RoleId - 2]].Equals(CurrentUserSession.CustId); //Check if your parent is ME?
        }

        #endregion Utility

        #region Render Setting Box

        /// <summary>
        /// Render Setting box
        /// </summary>
        /// <returns></returns>
        public string RenderSettingBox(DataRow dtrCustomerInfo)
        {
            this.custId = (int)dtrCustomerInfo["ObjectId"];
            this.isMember = string.IsNullOrEmpty(dtrCustomerInfo["IsMember"].ToString()) ? true : bool.Parse(dtrCustomerInfo["IsMember"].ToString());
            //this.custRoleId = (byte)dtrCustomerInfo["roleid"];
            string htmlFrame = "<iframe id='frmDownlineList' name='frmDownlineList' src='../_AccountInfo/DownLineTreeView.aspx?root={0}' style='width:98%;height:100%;'></iframe>";

            Fanex.Age.DTO.Reports.TreeView.TreeViewNode._SearchingID = (int)dtrCustomerInfo["ObjectId"];
            Fanex.Age.DTO.Reports.TreeView.TreeViewNode._Type = 1; //1: tree in tab account
            // Use DataRow row1, row2, row3.
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
                //tpl.HiddenBlockTabMember.MemberList = "<a href='#'> member 1.1.1.2 </a>";
                //tpl.HiddenBlockTabAgent.AgentList = "Agent";
                tpl.HiddenBlockHeaderTabAgent.AgentID = this.custId.ToString();
                tpl.HiddenBlockHeaderTabMember.AgentID = this.custId.ToString();
                tpl.HiddenBlockTabAgent.TreeViewDownLineFrame = string.Format(htmlFrame, this.custId.ToString()); ;
            }

            //bool isUpdaterDisableCasino = GetDBBooleanValue(dtrUpdaterInfo["DisableCasino"]);
            //bool isUpdaterDisableBingo = dtrUpdaterInfo.Table.Columns.Contains("DisableBingo") ? GetDBBooleanValue(dtrUpdaterInfo["DisableBingo"]) : true;
            //bool isUpdaterNumber = dtrUpdaterInfo.Table.Columns.Contains("DisableNumber") ? GetDBBooleanValue(dtrUpdaterInfo["DisableNumber"]) : true;

            //// CN 2 3 4 5 6 7: 00000000 (Last Number: weekly/daily use for transfer function)
            //string updaterTransferOption = Convert.IsDBNull(dtrUpdaterInfo["TransferOption"]) ? "00000000" : dtrUpdaterInfo["TransferOption"].ToString();
            //bool isUpdaterTransferDaily = updaterTransferOption[7] == '0';
            //bool isSyncCasino = GetDBBooleanValue(dtrCustomerInfo["synccasino"]);
            //bool isSyncLiveCasino = GetDBBooleanValue(dtrCustomerInfo["synclivecasino"]);
            //bool isSyncBingo = dtrCustomerInfo.Table.Columns.Contains("syncbingo") ? GetDBBooleanValue(dtrCustomerInfo["syncbingo"]) : false;

            ////Status
            //this.RenderSettingBoxStatus(tpl, dtrCustomerInfo);

            //// hasDownline
            //this.RenderSettingBoxHasDownline(tpl);

            //this.HideDisabledSettingBox(tpl, isUpdaterDisableCasino, isSyncCasino, isUpdaterDisableBingo, isSyncBingo, isUpdaterNumber);


            this.RenderSettingBoxEditLink(tpl, dtrCustomerInfo);

            //this.RenderSettingBoxLoginInfo(tpl, dtrCustomerInfo, dtrBalanceInfo);

            return tpl.ToString();
        }

        /// <summary>
        /// Set the checkbox in settingbox checked or not, disabled or not
        /// </summary>
        /// <param name="tpl"></param>
        private void RenderSettingBoxStatus(SettingBox_Template tpl, DataRow dtrCustomerInfo)
        {
            // Checked or not checked
            bool isEnabledCS = this.GetDBEnabledBooleanValue(dtrCustomerInfo["DisableCasino"]);
            bool isEnabledBingo = dtrCustomerInfo.Table.Columns.Contains("DisableBingo") ? this.GetDBEnabledBooleanValue(dtrCustomerInfo["DisableBingo"]) : true;

            bool isSyncBingo = dtrCustomerInfo.Table.Columns.Contains("syncbingo") ? GetDBBooleanValue(dtrCustomerInfo["syncbingo"]) : false;

            bool isEnabledNumberGame = dtrCustomerInfo.Table.Columns.Contains("DisableNumber") ? this.GetDBEnabledBooleanValue(dtrCustomerInfo["DisableNumber"]) : true;
            bool isEnabledOutright = this.GetDBEnabledBooleanValue(dtrCustomerInfo["DisableOutright"]);
            bool isEnabledAutoPT = this.GetDBEnabledBooleanValue(dtrCustomerInfo["DisableAutoMPT"]);
            bool isEnabledVirtualSports = (UserPermission.IsShowVirtualSports && dtrCustomerInfo.Table.Columns.Contains("DisableVirtualSports")) ? this.GetDBEnabledBooleanValue(dtrCustomerInfo["DisableVirtualSports"]) : false;
            bool isEnabledDoubleCommLosing = (UserPermission.IsShowDoubleCommission && dtrCustomerInfo.Table.Columns.Contains("DoubleCommLosing")) ? !this.GetDBEnabledBooleanValue(dtrCustomerInfo["DoubleCommLosing"]) : false;

            bool isClosed = GetDBBooleanValue(dtrCustomerInfo["closed"]);
            bool isSuspend = GetDBBooleanValue(dtrCustomerInfo["suspended"]);

            //tpl.checkedClosed = isClosed ? CHECKED_CHECKBOX : string.Empty;
            //tpl.checkedSuspended = isSuspend ? CHECKED_CHECKBOX : string.Empty;

            //tpl.checkedOutright = isEnabledOutright ? CHECKED_CHECKBOX : string.Empty;
            /*
            tpl.StatusAutoPT.checkedMasterAutoPT = isEnabledAutoPT ? CHECKED_CHECKBOX : string.Empty;
            tpl.StatusCasino.checkedCasino = isEnabledCS ? CHECKED_CHECKBOX : string.Empty;
            tpl.StatusNumberGame.checkedNumberGame = isEnabledNumberGame ? CHECKED_CHECKBOX : string.Empty;
            tpl.StatusVirtualSports.checkedVirtualSports = isEnabledVirtualSports ? CHECKED_CHECKBOX : string.Empty;
            tpl.StatusDoubleComm.checkedDoubleComm = isEnabledDoubleCommLosing ? CHECKED_CHECKBOX : string.Empty;
           
            if (CurrentUserSession.IsAgent && this.custRoleId == 1)
            {
                var cust = new SettingService().GetCustomerInfo(this.custId);

                //tpl.StatusLiveCasino.checkedLiveCasino = !cust.UplineDisableLiveCasino && !cust.DisableLiveCasino ? CHECKED_CHECKBOX : string.Empty;
                //tpl.StatusLiveCasino.disableLiveCasino = cust.UplineDisableLiveCasino ? DISABLE_HTML_ELEMENT : string.Empty;
            }
            else
            {
                tpl.StatusLiveCasino.Clear();
            }
             */
            // Disabled or not
            bool isUplineSuspend = GetDBBooleanValue(dtrCustomerInfo["upLineSuspend"]);
            bool isUplineClosed = GetDBBooleanValue(dtrCustomerInfo["upLineClosed"]);
            bool isUplineDisableOutright = GetDBBooleanValue(dtrCustomerInfo["uplineDisableOutright"]);
            bool isUplineDisabledCS = GetDBBooleanValue(dtrCustomerInfo["uplineDisableCS"]);
            bool isUplineDisabledBingo = GetDBBooleanValue(dtrCustomerInfo["uplineDisableBingo"]);
            bool isUplineDisabledNumberGame = GetDBBooleanValue(dtrCustomerInfo["uplineDisableNumber"]);
            bool isUplineDisabledVirtualSports = (UserPermission.IsShowVirtualSports && dtrCustomerInfo.Table.Columns.Contains("DisableVirtualSports")) ? GetDBBooleanValue(dtrCustomerInfo["uplineDisableVirtualSports"]) : true;
            bool isUplineDoubleCommLosing = (UserPermission.IsShowDoubleCommission && dtrCustomerInfo.Table.Columns.Contains("DoubleCommLosing")) ? GetDBBooleanValue(dtrCustomerInfo["uplineDoubleCommLosing"]) : true;

            bool subAccountPermission = !CurrentUserSession.IsSubAcc || CurrentUserSession.SubPageAccess.UpdateAccount;
            //tpl.disableSuspend = isUplineSuspend || !subAccountPermission ? DISABLE_HTML_ELEMENT : ENABLE_HTML_ELEMENT;
            //tpl.disableClosed = isUplineClosed || !subAccountPermission ? DISABLE_HTML_ELEMENT : ENABLE_HTML_ELEMENT;
            //tpl.disableOutright = isUplineDisableOutright || !subAccountPermission ? DISABLE_HTML_ELEMENT : ENABLE_HTML_ELEMENT;
            //tpl.StatusCasino.disableCasino = isUplineDisabledCS || !subAccountPermission || !IsDirectDownline(this.custRoleId) ? DISABLE_HTML_ELEMENT : ENABLE_HTML_ELEMENT;
            //tpl.StatusBingo.disableBingo = isUplineDisabledBingo || !subAccountPermission || !IsDirectDownline(this.custRoleId) ? DISABLE_HTML_ELEMENT : ENABLE_HTML_ELEMENT;
            //tpl.StatusBingo.checkedBingo = isEnabledBingo ? CHECKED_CHECKBOX : string.Empty;

            //tpl.StatusNumberGame.disableNumberGame = (isUplineDisabledNumberGame || !subAccountPermission || !IsDirectDownline(this.custRoleId) || Sunplus.Agent.Model.BL.Definitions.AppSettings.DisableNumberGameStatus) ? DISABLE_HTML_ELEMENT : ENABLE_HTML_ELEMENT;
            //tpl.StatusAutoPT.disableMasterAutoPT = !subAccountPermission || !IsDirectDownline(this.custRoleId) ? DISABLE_HTML_ELEMENT : ENABLE_HTML_ELEMENT;
            //tpl.StatusVirtualSports.disableVirtualSports = (isUplineDisabledVirtualSports || !subAccountPermission || !IsDirectDownline(this.custRoleId)) ? DISABLE_HTML_ELEMENT : ENABLE_HTML_ELEMENT;
            //tpl.StatusDoubleComm.disableDoubleComm = (!isUplineDoubleCommLosing || !subAccountPermission || !IsDirectDownline(this.custRoleId)) ? DISABLE_HTML_ELEMENT : ENABLE_HTML_ELEMENT;

            // Render tooltip
            //tpl.SuspendedTooltip = isUplineSuspend ? Resources.Lang.UplineHasSuspended : string.Empty;
            //tpl.ClosedTooltip = isUplineClosed ? Resources.Lang.UplineHasClosed : string.Empty;
            //tpl.OutrightTooltip = isUplineDisableOutright ? Resources.Lang.OutrightDisabledUpline : string.Empty;
        }

        /// <summary>
        /// Set hasdownline param for javascript function 'changeStatus'
        /// </summary>
        /// <param name="tpl"></param>
        private void RenderSettingBoxHasDownline(SettingBox_Template tpl)
        {
            string hasDownline = this.custRoleId > 1 ? "true" : "false";
           /*
            tpl.StatusCasino.hasDownline =
                tpl.StatusBingo.hasDownline =
                tpl.StatusNumberGame.hasDownline =
                tpl.StatusVirtualSports.hasDownline =
                tpl.StatusDoubleComm.hasDownline =
                tpl.StatusRacing.hasDownline = hasDownline;

            tpl.StatusRacingPopup.ukirishtote = this.custRoleId > 1 ? Formater.TruncateWithTip(Resources.Racing.ukirishtote, 13) : Resources.Racing.ukirishtote;
           */
        }

        /// <summary>
        /// Render Login info in setting box
        /// </summary>
        /// <param name="tpl"></param>
        private void RenderSettingBoxLoginInfo(SettingBox_Template tpl, DataRow dtrCustomerInfo, DataRow dtrBalanceInfo)
        {
            string createDate = Convert.IsDBNull(dtrCustomerInfo["createdate"]) ? string.Empty : Convert.ToDateTime(dtrCustomerInfo["createdate"]).ToShortDateString();
            string createDateTime = Convert.IsDBNull(dtrCustomerInfo["createdate"]) ? string.Empty : Convert.ToDateTime(dtrCustomerInfo["createdate"]).ToShortTimeString();
            string lastLoginDate = Convert.IsDBNull(dtrCustomerInfo["lastlogged"]) ? string.Empty : Convert.ToDateTime(dtrCustomerInfo["lastlogged"]).ToShortDateString();
            string lastLoginTime = Convert.IsDBNull(dtrCustomerInfo["lastlogged"]) ? string.Empty : " " + Convert.ToDateTime(dtrCustomerInfo["lastlogged"]).ToShortTimeString();
            string lastIp = this.GetDBStringValue(dtrCustomerInfo["lastloginip"]);
            string lastTxn = this.custRoleId < 4 ? this.GetDBStringValue(dtrBalanceInfo["lastTxn"]) : string.Empty;

            //tpl.createDate = createDate + " " + createDateTime;
            //tpl.lastLogin = lastLoginDate + " " + lastLoginTime;
            //tpl.lastTXN = lastTxn;
            //tpl.lastLoginIP = Sunplus.Agent.Model.Common.AgentIPFormat.FormatIP(lastIp);
        }

        /// <summary>
        /// Render edit link in settting box
        /// Edit info
        /// </summary>
        /// <param name="tpl"></param>
        /// <param name="isUpdaterDisableCasino"></param>
        /// <param name="isSyncCasino"></param>
        /// <param name="isUpdaterTransferDaily"></param>
        private void RenderSettingBoxEditLink(SettingBox_Template tpl, DataRow dtrCustomerInfo)
        {
            //Other settings
            int agentId = Convert.IsDBNull(dtrCustomerInfo["ObjectId"]) ? -1 : (int)dtrCustomerInfo["ObjectId"];
            // string agentName = this.GetDBStringValue(dtrCustomerInfo["UserName"]);
            //string masterName = this.GetDBStringValue(dtrDownlineInfo["masterName"]);

            string[] editPTLink = { string.Format("../_MemberInfo/PositionTaking/Agent/EditMember.aspx?m=0&custid={0}&USER={1}&agentid={2}&CallBack=Fanex.Customer.NewMember.OnRedirectToPage()", this.custId, this.userNameRequest, CurrentUserSession.CustId),
                        string.Format("../_MemberInfo/PositionTaking/Master/EditAgent.aspx?m=0&custid={0}&USER={1}&masterid={2}&CallBack=Fanex.EditPT.OnRedirectToPage()", this.custId, this.userNameRequest, CurrentUserSession.CustId),
                        string.Format("../_MemberInfo/PositionTaking/Super/EditMaster.aspx?m=0&custid={0}&USER={1}&superid={2}&CallBack=Fanex.EditPT.OnRedirectToPage()", this.custId, this.userNameRequest, CurrentUserSession.CustId),
                        string.Empty
                        };

            string[] editPTOnMemberLink = { "",
                                String.Format("../UserMaster/EditMember.aspx?m=0&custid={0}&USER={1}&agentid={2}&nobackground=1", custId, userNameRequest, agentId),
                                String.Format("../UserSuper/EditMember.aspx?m=0&custid={0}&USER={1}&agentid={2}&nobackground=1", custId, userNameRequest, agentId), 
                                ""
                                };

            string[] editHROnMemberLink = { string.Empty,
                                string.Format("../_MemberInfo/RaceBook/{1}/EditMember.aspx?custid={0}&popup=false", new object[] { this.custId, ((UserTypes)CurrentUserSession.RoleId).ToString() }),
                                string.Format("../_MemberInfo/RaceBook/{1}/EditMember.aspx?custid={0}&popup=false", new object[] { this.custId, ((UserTypes)CurrentUserSession.RoleId).ToString() }),
                                string.Empty
                                };
            

            if (this.isMember)
            {
                // tpl.otherSettings = this.RenderOtherSettingViewMember(editPTLink, editPTOnMemberLink, editHROnMemberLink);
                tpl.otherSettings = this.RenderOtherSettingViewAgent(editPTLink, editPTOnMemberLink, editHROnMemberLink);
                RegisterJavascriptObject("agenttype", Resources.Lang.member);
            }
            else
            {
                tpl.otherSettings = this.RenderOtherSettingViewAgent(editPTLink, editPTOnMemberLink, editHROnMemberLink);
                RegisterJavascriptObject("agenttype", Resources.Lang.agent);
            }
        }

        private string RenderOtherSettingViewMember(string[] editPTLink, string[] editPTOnMemberLink, string[] editHROnMemberLink)
        {
            SettingBoxViewMember_Template tplMember = new SettingBoxViewMember_Template();

            //if (!IsDirectDownline(this.custRoleId))
            //{
            //    tplMember.ForDirectDownline.Hide();
            //}
            //else
            //{
                
                //tplMember.ForDirectDownline.LnkResetPassword.resetPasswordLink = string.Format("../_AccountInfo/ResetPassword.aspx?custid={0}&usn={1}&nobackground=1&isSyncCasino={2}", this.custId, this.userNameRequest, isSyncCasino);
                tplMember.ForDirectDownline.editRacingLink = string.Format("../_MemberInfo/RaceBook/{1}/EditMember.aspx?custid={0}&popup=false", this.custId, ((UserTypes)CurrentUserSession.RoleId).ToString());
                /*
                if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowCasino)
                {
                    tplMember.ForDirectDownline.Casino.editCasinoLink = string.Format("../MemberInfo/PositionTakingCS.aspx?custId={0}&nobackground=1", this.custId);
                    tplMember.ForDirectDownline.Casino.editCasinoLabel = isSyncCasino ? Resources.Lang.lbleditcasino : Sunplus.Agent.Utilities.Formater.TruncateWithTip(Resources.Lang.synccasino, 14);
                }
                else
                {
                    tplMember.ForDirectDownline.Casino = null;
                }

                if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowBingo)
                {
                    tplMember.ForDirectDownline.Bingo.editBingoLink = string.Format("../MemberInfo/PositionTakingBingo.aspx?custId={0}&nobackground=1", this.custId);
                    tplMember.ForDirectDownline.Bingo.editBingoLabel = isSyncBingo ? Resources.Bingo.lbleditBingo : Sunplus.Agent.Utilities.Formater.TruncateWithTip(Resources.Bingo.syncBingo, 14);
                }
                else
                {
                    tplMember.ForDirectDownline.Bingo = null;
                }
               
                //For P2P
                if (UserPermission.IsShowP2P)
                {
                    tplMember.ForDirectDownline.ForP2P.editP2P = string.Format("../_MemberInfo/P2P/P2P_Page.aspx?custids={0}&uplineRoleId={1}&roleid={2}&username={3}", this.custId, CurrentUserSession.RoleId.ToString(), this.custRoleId, this.userNameRequest);
                }
                else
                {
                    tplMember.ForDirectDownline.ForP2P.Clear();
                }

                if (UserPermission.IsShowLiveCasino)
                {
                    tplMember.ForDirectDownline.ForLiveCasino.editLiveCasino = string.Format("../_MemberInfo/LiveCasino/Agent/EditMember.aspx?custids={0}&username={1}&roleid={2}&isMult=0", this.custId, this.userNameRequest, this.custRoleId);
                }
                else
                {
                    tplMember.ForDirectDownline.ForLiveCasino.Clear();
                }
                
                //For Virtual Sport
                if (UserPermission.IsShowVirtualSports)
                {
                    tplMember.ForDirectDownline.ForVirtualSports.editVirtualSports = string.Format("../_MemberInfo/VirtualSports/EditSingle.aspx?custid={0}&roleid={1}&username={2}", this.custId, this.custRoleId, this.userNameRequest);
                }
                else
                {
                    tplMember.ForDirectDownline.ForVirtualSports.Clear();
                }
                */
                //string[] editCommLink = { string.Format("../_MemberInfo/EditCommission/EditMemberCommission.aspx?custId={0}&username={1}", this.custId, this.userNameRequest),
                //              string.Format("../_MemberInfo/EditCommission/EditCommission.aspx?custId={0}&username={1}&roleId={2}", this.custId, this.userNameRequest, this.custRoleId),
                //              string.Format("../_MemberInfo/EditCommission/EditCommission.aspx?custId={0}&username={1}&roleId={2}", this.custId, this.userNameRequest, this.custRoleId),
                //              string.Empty
                //            };
                if (isMember)
                {
                    tplMember.ForDirectDownline.editPTLink = string.Format("../../_MemberInfo/CustomerSetting/Pages/EditMemberPositionTaking.aspx?custId={0}&username={1}", this.custId, this.userNameRequest);
                }
                else
                {
                    tplMember.ForDirectDownline.editPTLink = string.Format("../../_MemberInfo/CustomerSetting/Pages/EditAgentPositionTaking.aspx?custId={0}&username={1}", this.custId, this.userNameRequest);
                }
                 
            if (isMember)
                {
                    tplMember.ForDirectDownline.editCommLink = string.Format("../../_MemberInfo/CustomerSetting/Pages/EditMemberCommission.aspx?custId={0}&username={1}", this.custId, this.userNameRequest);
                }
                else
                {
                    tplMember.ForDirectDownline.editCommLink = string.Format("../../_MemberInfo/CustomerSetting/Pages/EditAgentCommission.aspx?custId={0}&username={1}", this.custId, this.userNameRequest);
                }
                 
                //tplMember.ForDirectDownline.editInfoLink = string.Format("EditCustomerInfo.aspx?custId={0}&username={1}&isSyncCasino={2}&isSyncBingo={3}", this.custId, this.userNameRequest, isSyncCasino ? "1" : "0", isSyncBingo ? "1" : "0");
                if (isMember)
                {
                    tplMember.ForDirectDownline.betSettingLink = string.Format("../../_MemberInfo/CustomerSetting/Pages/EditMemberBetLimit.aspx?custId={0}&username={1}", this.custId, this.userNameRequest);
                }
                else
                {
                    tplMember.ForDirectDownline.betSettingLink = string.Format("../../_MemberInfo/CustomerSetting/Pages/EditAgentBetLimit.aspx?custId={0}&username={1}", this.custId, this.userNameRequest);
                }
            //}

            if (CurrentUserSession.RoleId == (int)UserTypes.Agent)
            {
                tplMember.ForMember = null;
            }
            else
            {
                tplMember.ForMember.editSBLink = editPTOnMemberLink[CurrentUserSession.RoleId - 2];
                tplMember.ForMember.editRacingLink = editHROnMemberLink[CurrentUserSession.RoleId - 2];
                /*
                // For Casino
                if (!isSyncCasino || !UserPermission.IsShowCasino || isUpdaterDisableCasino)
                {
                    tplMember.ForMember.StatusCasinoMember = null;
                }
                else
                {
                    tplMember.ForMember.StatusCasinoMember.editCasinoLink = string.Format("../MemberInfo/PositionTakingCS.aspx?custId={0}&nobackground=1", this.custId);
                    tplMember.ForMember.StatusCasinoMember.editCasinoLabel = Resources.Lang.lbleditcasino;
                }

                // For Bingo
                if (!isSyncBingo || !UserPermission.IsShowBingo)
                {
                    tplMember.ForMember.StatusBingoMember = null;
                }
                else
                {
                    tplMember.ForMember.StatusBingoMember.editBingoLink = string.Format("../MemberInfo/PositionTakingBingo.aspx?custId={0}&nobackground=1", this.custId);
                    tplMember.ForMember.StatusBingoMember.editBingoLabel = Resources.Bingo.lbleditBingo;
                }
               
                //For Virtual Sport
                if (UserPermission.IsShowVirtualSports)
                {
                    tplMember.ForMember.StatusVirtualSportsMember.editVirtualSports = string.Format("../_MemberInfo/VirtualSports/EditSingle.aspx?custid={0}&roleid={1}&username={2}", this.custId, this.custRoleId, this.userNameRequest);
                }
                else
                {
                    tplMember.ForMember.StatusVirtualSportsMember.Clear();
                }

                // For Live Casino
                if (UserPermission.IsShowLiveCasino && isSyncLiveCasino)
                {
                    if (CurrentUserSession.IsMaster)
                    {
                        tplMember.ForMember.StatusLiveCasinoMember.editLiveCasino = string.Format("../_MemberInfo/LiveCasino/Master/EditMember.aspx?custids={0}&username={1}&roleid={2}&isMult=0", this.custId, this.userNameRequest, this.custRoleId);
                    }
                    else if (CurrentUserSession.IsSuper)
                    {
                        tplMember.ForMember.StatusLiveCasinoMember.editLiveCasino = string.Format("../_MemberInfo/LiveCasino/Super/EditMember.aspx?custids={0}&username={1}&roleid={2}&isMult=0", this.custId, this.userNameRequest, this.custRoleId);
                    }
                }
                else
                {
                    tplMember.ForMember.StatusLiveCasinoMember.Clear();
                }
                 */
            }
            /*
            if (CurrentUserSession.IsSubAcc && !CurrentUserSession.SubPageAccess.UpdateAccount)
            {
                tplMember.ForDirectDownline.LnkResetPassword.Hide();
            }
            */
            tplMember.ClearCredit = null;
            return tplMember.ToString();
        }

        private string RenderOtherSettingViewAgent(string[] editPTLink, string[] editPTOnMemberLink, string[] editHROnMemberLink)
        {
            SettingBoxViewAgent_Template tplAgent = new SettingBoxViewAgent_Template();
            /*
            if (CurrentUserSession.IsSubAcc && !CurrentUserSession.SubPageAccess.UpdateAccount)
            {
                tplAgent.ForDirectDownline.LnkResetPassword.Hide();
            }
            */
            //if (!CurrentUserSession.IsSuper || (CurrentUserSession.IsSubAcc && !CurrentUserSession.SubPageAccess.UpdateAccount))
            //{
            //    tplAgent.ForSuperViewAgent.Hide();
            //}

            //if (!IsDirectDownline(this.custRoleId))
            //{
            //    tplAgent.ForDirectDownline.Hide();
            //    //tplAgent.ForSuperViewAgent.resetPasswordLink = string.Format("../_AccountInfo/ResetPassword.aspx?custid={0}&usn={1}&nobackground=1&isSynCasino={2}", this.custId, this.userNameRequest, isSyncCasino);
            //}
            //else
            //{
            //    //tplAgent.ForDirectDownline.LnkResetPassword.resetPasswordLink = string.Format("../_AccountInfo/ResetPassword.aspx?custid={0}&usn={1}&nobackground=1&isSynCasino={2}", this.custId, this.userNameRequest, isSyncCasino);
            //    tplAgent.ForDirectDownline.spMinPTLink = string.Format("../UserSuper/EditMinPT.aspx?custid={0}&username={1}&nobackground=1", this.custId, this.userNameRequest);
            //    tplAgent.ForDirectDownline.racingMinPTLink = string.Format("../_MemberInfo/RaceBook/{1}/EditMinPT.aspx?custid={0}&popup=false", new object[] { this.custId, ((UserTypes)CurrentUserSession.RoleId).ToString() });
            //    tplAgent.ForDirectDownline.editPTLink = editPTLink[this.custRoleId - 1];
            //    tplAgent.ForDirectDownline.editRacingLink = string.Format("../_MemberInfo/RaceBook/{1}/EditAgent.aspx?custid={0}&popup=false", this.custId, ((UserTypes)CurrentUserSession.RoleId).ToString());
            //    /*
            //    if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowCasino)
            //    {
            //        tplAgent.ForDirectDownline.Casino.editCasinoLink = string.Format("../MemberInfo/PositionTakingCS.aspx?custId={0}&nobackground=1", this.custId);
            //        tplAgent.ForDirectDownline.Casino.editCasinoLabel = isSyncCasino ? Resources.Lang.lbleditcasino : Sunplus.Agent.Utilities.Formater.TruncateWithTip(Resources.Lang.synccasino, 14);
            //    }
            //    else
            //    {
            //        tplAgent.ForDirectDownline.Casino = null;
            //    }

            //    if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowBingo)
            //    {
            //        tplAgent.ForDirectDownline.Bingo.editBingoLink = string.Format("../MemberInfo/PositionTakingBingo.aspx?custId={0}&nobackground=1", this.custId);
            //        tplAgent.ForDirectDownline.Bingo.editBingoLabel = isSyncBingo ? Resources.Bingo.lbleditBingo : Sunplus.Agent.Utilities.Formater.TruncateWithTip(Resources.Bingo.syncBingo, 14);
            //    }
            //    else
            //    {
            //        tplAgent.ForDirectDownline.Bingo = null;
            //    }
               
            //    //For P2P
            //    if (UserPermission.IsShowP2P)
            //    {
            //        tplAgent.ForDirectDownline.ForP2P.editP2P = string.Format("../_MemberInfo/P2P/P2P_Page.aspx?custids={0}&uplineRoleId={1}&roleid={2}&username={3}", this.custId, CurrentUserSession.RoleId.ToString(), this.custRoleId, this.userNameRequest);
            //    }
            //    else
            //    {
            //        tplAgent.ForDirectDownline.ForP2P.Clear();
            //    }

            //    if (UserPermission.IsShowLiveCasino)
            //    {
            //        tplAgent.ForDirectDownline.ForLiveCasino.editLiveCasino = string.Format("../_MemberInfo/LiveCasino/Master/EditAgent.aspx?custids={0}&username={1}&roleid={2}&isMult=0", this.custId, this.userNameRequest, this.custRoleId);
            //    }
            //    else
            //    {
            //        tplAgent.ForDirectDownline.ForLiveCasino.Clear();
            //    }

            //    //For Virtual Sport
            //    if (UserPermission.IsShowVirtualSports)
            //    {
            //        tplAgent.ForDirectDownline.ForVirtualSports.editVirtualSports = string.Format("../_MemberInfo/VirtualSports/EditSingle.aspx?custid={0}&roleid={1}&username={2}", this.custId, this.custRoleId, this.userNameRequest);
            //    }
            //    else
            //    {
            //        tplAgent.ForDirectDownline.ForVirtualSports.Clear();
            //    }
            //     */
            //    string[] editCommLink = { string.Format("../_MemberInfo/EditCommission/EditMemberCommission.aspx?custId={0}&username={1}", this.custId, this.userNameRequest),
            //                  string.Format("../_MemberInfo/EditCommission/EditCommission.aspx?custId={0}&username={1}&roleId={2}", this.custId, this.userNameRequest, this.custRoleId),
            //                  string.Format("../_MemberInfo/EditCommission/EditCommission.aspx?custId={0}&username={1}&roleId={2}", this.custId, this.userNameRequest, this.custRoleId),
            //                  string.Empty
            //                };

            //    tplAgent.ForDirectDownline.editCommLink = editCommLink[this.custRoleId - 1];
            //   // tplAgent.ForDirectDownline.editInfoLink = string.Format("EditCustomerInfo.aspx?custId={0}&username={1}&isSyncCasino={2}&isSyncBingo={3}", this.custId, this.userNameRequest, isSyncCasino ? "1" : "0", isSyncBingo ? "1" : "0");
            //    tplAgent.ForDirectDownline.editBetSettingLink = string.Format("../_MemberInfo/BetSetting/EditBetSetting.aspx?custId={0}&username={1}", this.custId, this.userNameRequest);
            //}

            //if updater's transfer condition is not daily, then do not allow to edit TransferCondtion
            //if (!IsDirectDownline(this.custRoleId) || !isUpdaterTransferDaily)
            //{
            //    tplAgent.ForAgent = null;
            //}
            //else
            //{
            //    tplAgent.ForAgent.tranferCondLink = string.Format("../_MemberInfo/TransferCondition/EditTransferCondition.aspx?custid={0}&username={1}", this.custId, this.userNameRequest);
            //}

            //tplAgent.ForDirectDownline.editSportBookLink = string.Format("../_MemberInfo/BetSetting/EditBetSetting.aspx?custid={0}&username={1}", this.custId, this.userNameRequest);

            if (isMember)
            {
                //Page.ResolveUrl("~/
                tplAgent.ForDirectDownline.editSportBookLink = string.Format("../_MemberInfo/CustomerSetting/Pages/EditMemberSetting.aspx?custId={0}&username={1}", this.custId, this.userNameRequest);
            }
            else
            {
                tplAgent.ForDirectDownline.editSportBookLink = string.Format("../_MemberInfo/CustomerSetting/Pages/EditAgentSetting.aspx?custId={0}&username={1}", this.custId, this.userNameRequest);
            }

            if (isMember)
            {
                tplAgent.ForDirectDownline.editCommLink = string.Format("../_MemberInfo/CustomerSetting/Pages/EditMemberCommission.aspx?custId={0}&username={1}", this.custId, this.userNameRequest);
            }
            else
            {
                tplAgent.ForDirectDownline.editCommLink = string.Format("../_MemberInfo/CustomerSetting/Pages/EditAgentCommission.aspx?custId={0}&username={1}", this.custId, this.userNameRequest);
            }

            //tplMember.ForDirectDownline.editInfoLink = string.Format("EditCustomerInfo.aspx?custId={0}&username={1}&isSyncCasino={2}&isSyncBingo={3}", this.custId, this.userNameRequest, isSyncCasino ? "1" : "0", isSyncBingo ? "1" : "0");
            if (isMember)
            {
                tplAgent.ForDirectDownline.editBetSettingLink = string.Format("../_MemberInfo/CustomerSetting/Pages/EditMemberBetLimit.aspx?custId={0}&username={1}", this.custId, this.userNameRequest);
            }
            else
            {
                tplAgent.ForDirectDownline.editBetSettingLink = string.Format("../_MemberInfo/CustomerSetting/Pages/EditAgentBetLimit.aspx?custId={0}&username={1}", this.custId, this.userNameRequest);
            }

            if (isMember)
            {
                //Page.ResolveUrl("~/
                tplAgent.ForDirectDownline.editPTLink = string.Format("../_MemberInfo/CustomerSetting/Pages/EditMemberPositionTaking.aspx?custId={0}&username={1}", this.custId, this.userNameRequest);
            }
            else
            {
                tplAgent.ForDirectDownline.editPTLink = string.Format("../_MemberInfo/CustomerSetting/Pages/EditAgentPositionTaking.aspx?custId={0}&username={1}", this.custId, this.userNameRequest);
            }


            return tplAgent.ToString();
        }

        /// <summary>
        /// Hide FT,CS, AutoPT if it is disabled in appsetting or in upper downline
        /// </summary>
        /// <param name="tpl"></param>
        /// <param name="isUpdaterDisableCasino"></param>
        /// <param name="isSyncCasino"></param>
        private void HideDisabledSettingBox(SettingBox_Template tpl, bool isUpdaterDisableCasino, bool isSyncCasino, bool isUpdaterDisableBingo, bool isSyncBingo, bool isUpdaterDisableNumber)
        {
            /*
            if (!UserPermission.IsShowCasino || isUpdaterDisableCasino || !isSyncCasino)
            {
                tpl.StatusCasino = null;
            }

            if (!UserPermission.IsShowBingo || Sunplus.Agent.Model.BL.Definitions.AppSettings.DisableBingoStatus || !isSyncBingo)
            {
                tpl.StatusBingo = null;
            }

            if (!UserPermission.IsShowNumberGame)
            {
                tpl.StatusNumberGame = null;
            }

            if (!UserPermission.IsShowVirtualSports)
            {
                tpl.StatusVirtualSports = null;
            }

            if (!UserPermission.IsShowDoubleCommission)
            {
                tpl.StatusDoubleComm.disableDoubleComm = DISABLE_HTML_ELEMENT;
            }

            if (!UserPermission.IsShowRacing)
            {
                tpl.StatusRacing = null;
                tpl.StatusRacingPopup = null;
            }

            if (this.custRoleId != 3)
            {
                tpl.StatusAutoPT = null;
            }
            */
        }

        #endregion Render Setting Box

        #region Render Balance Box
        private const string htmlFrame = "<iframe id='frmDownlineList' name='frmDownlineList' src='DownlineListInfo.aspx?custid={0}' style='width:1px;height:1px;display:none;'></iframe>";
        
        /// <summary>
        /// Get downline list
        /// </summary>
        /// <returns></returns>
        public string GetDownlineFrameHtml(int custId)
        {
            return (custId > 0) ? string.Format(htmlFrame, custId) : string.Empty;          
        }

        /// <summary>
        /// Render balance box
        /// </summary>
        /// <returns></returns>
        public string RenderBalanceBox(DataRow dtrCustomerInfo, DataRow dtrBalanceInfo)
        {
            this.custId = (int)dtrCustomerInfo["custid"];
            this.custRoleId = (byte)dtrCustomerInfo["roleid"];

            BalanceBox_Template tpl = new BalanceBox_Template();

            

            decimal givenCredit = this.GetDBDecimalValue(dtrCustomerInfo["credit"]);
            bool isAllowTransfer = CurrentUserSession.SubPageAccess.Transfer;
            tpl.DownlineList.CustId = this.custId.ToString();

            switch (this.custRoleId)
            {
                case (int)UserTypes.Member:
                    this.RenderBalanceBoxMemberInfo(dtrCustomerInfo, dtrBalanceInfo, tpl, givenCredit, isAllowTransfer);
                    break;
                case (int)UserTypes.Agent:
                    this.RenderBalanceBoxAgentInfo(dtrCustomerInfo, dtrBalanceInfo, tpl, givenCredit, isAllowTransfer);
                    break;
                case (int)UserTypes.Master:
                    this.RenderBalanceBoxMasterInfo(dtrCustomerInfo, dtrBalanceInfo, tpl, isAllowTransfer);
                    break;
            }

            this.RenderBalanceBoxCSOutstanding(dtrCustomerInfo, dtrBalanceInfo, tpl);
            if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowBingo)
            {
                this.RenderBalanceBoxBingoOutstanding(dtrCustomerInfo, dtrBalanceInfo, tpl);
            }
            else
            {
                tpl.SyncBingo = null;
            }

            this.RenderBalanceBoxP2POutstanding(dtrBalanceInfo, tpl);

            this.RenderBalanceBoxDownlineStatus(dtrCustomerInfo, tpl);

            tpl.GivenCredit = Formater.FormatNumber20(givenCredit, CurrentUserSession.LangKey);

            return tpl.ToString();
        }

        /// <summary>
        /// Show downline status on balance box
        /// </summary>
        /// <param name="tpl"></param>
        private void RenderBalanceBoxDownlineStatus(DataRow dtrCustomerInfo, BalanceBox_Template tpl)
        {
            //tpl.DownlineListFrame = GetDownlineFrameHtml(this.custId);

            if (this.custRoleId == 1)
            {
                tpl.DownlineStatusAgent.Hide();
            }
            else
            {
                tpl.DownlineStatusAgent.TotalMemStatus = Sunplus.Agent.Utilities.Formater.TruncateWithTip(Resources.Lang.totalMemACS, 18);
                tpl.DownlineStatusAgent.TotalMemDesc = Resources.Lang.lblTotalMemberActiveClosed;
                tpl.DownlineStatusAgent.TotalMemActive = this.GetDBIntValue(dtrCustomerInfo["TotalMemActive"]).ToString();
                tpl.DownlineStatusAgent.TotalMemClosed = this.GetDBIntValue(dtrCustomerInfo["TotalMemClosed"]).ToString();
                tpl.DownlineStatusAgent.TotalMemSuspended = this.GetDBIntValue(dtrCustomerInfo["TotalMemSuspended"]).ToString();
            }

            if (this.custRoleId != 3)
            {
                tpl.DownlineStatusMaster.Hide();
            }
            else
            {
                tpl.DownlineStatusMaster.TotalMemStatus = Sunplus.Agent.Utilities.Formater.TruncateWithTip(Resources.Lang.totalAgentACS, 18);
                tpl.DownlineStatusMaster.TotalMemDesc = Resources.Lang.lblTotalAgentActiveClosed;
                tpl.DownlineStatusMaster.TotalMemActive = this.GetDBIntValue(dtrCustomerInfo["TotalAgActive"]).ToString();
                tpl.DownlineStatusMaster.TotalMemClosed = this.GetDBIntValue(dtrCustomerInfo["TotalAgClosed"]).ToString();
                tpl.DownlineStatusMaster.TotalMemSuspended = this.GetDBIntValue(dtrCustomerInfo["TotalAgSuspended"]).ToString();
            }
        }

        /// <summary>
        /// Render outstanding
        /// </summary>
        /// <param name="tpl"></param>
        private void RenderBalanceBoxOutstanding(ref BalanceBox_Template tpl, decimal agentoutstanding)
        {
            if (CurrentUserSession.SubPageAccess.ViewReports && agentoutstanding > 0)
            {
                tpl.ForAgent.AgentOutstanding = this.FormatNumber2WithColor(agentoutstanding, true);
                tpl.ForAgent.AgentOutstandingClass = CLASS_NAME;
                tpl.ForAgent.AgentOutstandingLink = string.Format("href='../_Reports/Common/Outstanding.aspx?custid={0}&roleid={1}'", this.custId, this.custRoleId);
            }
            else
            {
                tpl.ForAgent.AgentOutstanding = this.FormatNumber2WithColor(agentoutstanding, false);
                tpl.ForAgent.AgentOutstandingClass = null;
                tpl.ForAgent.AgentOutstandingLink = null;
            }
        }

        /// <summary>
        /// Render casino outstanding
        /// </summary>
        /// <param name="tpl"></param>
        private void RenderBalanceBoxCSOutstanding(DataRow dtrCustomerInfo, DataRow dtrBalanceInfo, BalanceBox_Template tpl)
        {
            decimal casinoOutstanding = 0, casinoTransfer = 0;
            bool isSyncCasino = this.GetDBBooleanValue(dtrCustomerInfo["synccasino"]);
            RegisterJavascriptObject("isSyncCasino", isSyncCasino ? "1" : "0");
            if (isSyncCasino)
            {
                if (this.custRoleId < 4)
                {
                    casinoOutstanding = this.GetDBDecimalValue(dtrBalanceInfo["csOutstanding"]);
                    casinoTransfer = this.GetDBDecimalValue(dtrBalanceInfo["csTransfer"]);
                    tpl.SyncCS.CasinoTransf = this.FormatNumber2WithColor(casinoTransfer, false);
                    if (CurrentUserSession.SubPageAccess.ViewReports && casinoOutstanding > 0 && this.custRoleId == 1)
                    {
                        tpl.SyncCS.OutsCSClass = CLASS_NAME;
                        tpl.SyncCS.CasinoOuts = this.FormatNumber2WithColor(casinoOutstanding, true);
                        tpl.SyncCS.OutstandingCSLink = casinoOutstanding > 0 ? string.Format("href='../_Reports/BetList/OutstandingDetail.aspx?custid={0}&username={1}&type={2}&chkgetall=on&chk_showcasino=on&chk_showbi=off&chk_showng=off&chk_showsb=off&chk_showrb=off&chk_p2p=off'", this.custId, this.userNameRequest, "RunByDate_Mem") : string.Empty;
                    }
                    else
                    {
                        tpl.SyncCS.OutsCSClass = null;
                        tpl.SyncCS.CasinoOuts = this.FormatNumber2WithColor(casinoOutstanding, false);
                        tpl.SyncCS.OutstandingCSLink = null;
                    }
                }
            }
            else
            {
                tpl.SyncCS = null;
            }
        }

        /// <summary>
        /// Render Bingo outstanding
        /// </summary>
        /// <param name="tpl"></param>
        private void RenderBalanceBoxBingoOutstanding(DataRow dtrCustomerInfo, DataRow dtrBalanceInfo, BalanceBox_Template tpl)
        {
            decimal bingoOutstanding = 0, bingoTransfer = 0;
            bool isSyncBingo = dtrCustomerInfo.Table.Columns.Contains("syncbingo") ? this.GetDBBooleanValue(dtrCustomerInfo["syncbingo"]) : false;
            RegisterJavascriptObject("isSyncBingo", isSyncBingo ? "1" : "0");
            if (isSyncBingo)
            {
                if (this.custRoleId < 4)
                {
                    bingoOutstanding = this.GetDBDecimalValue(dtrBalanceInfo["bingoOutstanding"]);
                    bingoTransfer = this.GetDBDecimalValue(dtrBalanceInfo["bingoTransfer"]);
                    tpl.SyncBingo.BingoTransf = this.FormatNumber2WithColor(bingoTransfer, false);
                    if (CurrentUserSession.SubPageAccess.ViewReports && bingoOutstanding > 0 && this.custRoleId == 1)
                    {
                        tpl.SyncBingo.OutsBingoClass = CLASS_NAME;
                        tpl.SyncBingo.BingoOuts = this.FormatNumber2WithColor(bingoOutstanding, true);
                        tpl.SyncBingo.OutstandingBingoLink = bingoOutstanding > 0 ? string.Format("href='../_Reports/BetList/OutstandingDetail.aspx?custid={0}&username={1}&type={2}&chkgetall=on&chk_showcasino=off&chk_showbi=on&chk_showng=off&chk_showsb=off&chk_showrb=off&chk_p2p=off'", this.custId, this.userNameRequest, "RunByDate_Mem") : string.Empty;
                    }
                    else
                    {
                        tpl.SyncBingo.OutsBingoClass = null;
                        tpl.SyncBingo.BingoOuts = this.FormatNumber2WithColor(bingoOutstanding, false);
                        tpl.SyncBingo.OutstandingBingoLink = null;
                    }
                }
            }
            else
            {
                tpl.SyncBingo = null;
            }
        }

        /// <summary>
        /// Render P2P outstanding
        /// </summary>
        /// <param name="tpl"></param>
        private void RenderBalanceBoxP2POutstanding(DataRow dtrBalanceInfo, BalanceBox_Template tpl)
        {
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.IsEnableP2P)
            {
                decimal p2pOutstanding = 0, p2pTransfer = 0;
                if (this.custRoleId < 4)
                {
                    p2pOutstanding = this.GetDBDecimalValue(dtrBalanceInfo["p2pOutstanding"]);
                    p2pTransfer = this.GetDBDecimalValue(dtrBalanceInfo["p2pTransfer"]);
                    tpl.SyncP2P.P2PTransf = this.FormatNumber2WithColor(p2pTransfer, false);
                    if (CurrentUserSession.SubPageAccess.ViewReports && p2pOutstanding > 0 && this.custRoleId == 1)
                    {
                        tpl.SyncP2P.OutsP2PClass = CLASS_NAME;
                        tpl.SyncP2P.P2POuts = this.FormatNumber2WithColor(p2pOutstanding, true);
                        tpl.SyncP2P.OutstandingP2PLink = p2pOutstanding > 0 ? string.Format("href='../_Reports/BetList/OutstandingDetail.aspx?custid={0}&username={1}&type={2}&chkgetall=on&chk_showcasino=off&chk_showbi=off&chk_showng=off&chk_showsb=off&chk_showrb=off&chk_p2p=on'", this.custId, this.userNameRequest, "RunByDate_Mem") : string.Empty;
                    }
                    else
                    {
                        tpl.SyncP2P.OutsP2PClass = null;
                        tpl.SyncP2P.P2POuts = this.FormatNumber2WithColor(p2pOutstanding, false);
                        tpl.SyncP2P.OutstandingP2PLink = null;
                    }
                }
            }
            else
            {
                tpl.SyncP2P = null;
            }
        }

        /// <summary>
        /// Render balnace box for member
        /// </summary>
        /// <param name="tpl"></param>
        /// <param name="givenCredit"></param>
        private void RenderBalanceBoxMemberInfo(DataRow dtrCustomerInfo, DataRow dtrBalanceInfo, BalanceBox_Template tpl, decimal givenCredit, bool isAllowTransfer)
        {
            tpl.ForAgent = null;
            tpl.ForAgent1 = null;
            tpl.MemMaxCredit = null;
            tpl.DownlineList = null;

            decimal totalCash = this.GetDBDecimalValue(dtrBalanceInfo["Cash"]);
            decimal yesterdayCash = this.GetDBDecimalValue(dtrBalanceInfo["yesterday_Cash"]);
            decimal outstanding = this.GetDBDecimalValue(dtrBalanceInfo["outstanding"]);
            decimal betCredit = givenCredit + totalCash - outstanding;
            betCredit = Convert.ToDecimal(Formater.DecFormat(betCredit, 2, true));

            //Agent view member
            if (CurrentUserSession.IsAgent)
            {
                if (Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowedTotalBalanceTransfer)
                {
                    tpl.ForMember.TotalBalance = this.GenTransferInfo(this.custId, totalCash, isAllowTransfer);
                    tpl.ForMember.YesterdayBalance = this.FormatNumber2WithColor(yesterdayCash, false);
                }
                else
                {
                    tpl.ForMember.TotalBalance = this.FormatNumber2WithColor(totalCash, false);
                    tpl.ForMember.YesterdayBalance = this.GenTransferInfo(this.custId, yesterdayCash, isAllowTransfer);
                }
            }
            else
            {
                //Master/Super view member
                tpl.ForMember.TotalBalance = this.FormatNumber2WithColor(totalCash, false);
                tpl.ForMember.YesterdayBalance = this.FormatNumber2WithColor(yesterdayCash, false);
            }

            decimal memberMaxCreditMax = this.GetDBDecimalValue(dtrCustomerInfo["MemMaxCredit_Max"]);
            tpl.GivenCreditLimit = string.Format("<= {0}", Formater.FormatNumber20(memberMaxCreditMax, CurrentUserSession.LangKey));
            tpl.ForMember2.BetCredit = this.FormatNumber2WithColor(betCredit, false);

            if (CurrentUserSession.SubPageAccess.ViewReports && outstanding > 0)
            {
                tpl.ForMember1.Outstanding = this.FormatNumber2WithColor(outstanding, true);
                tpl.ForMember1.OutstandingClass = CLASS_NAME;
                tpl.ForMember1.OutstandingLink = string.Format("href='../_Reports/BetList/OutstandingDetail.aspx?custid={0}&username={1}&type={2}&chkgetall=on&chk_all=on&chk_showcasino=on&chk_showbi=on&chk_showng=on&chk_showsb=on&chk_showrb=on'", this.custId, this.userNameRequest, "RunByDate_Mem");
            }
            else
            {
                tpl.ForMember1.Outstanding = this.FormatNumber2WithColor(outstanding, false);
                tpl.ForMember1.OutstandingClass = null;
                tpl.ForMember1.OutstandingLink = null;
            }
        }

        /// <summary>
        /// Render balance box info for agent
        /// </summary>
        /// <param name="tpl"></param>
        /// <param name="givenCredit"></param>
        /// <param name="isAllowTransfer"></param>
        private void RenderBalanceBoxAgentInfo(DataRow dtrCustomerInfo, DataRow dtrBalanceInfo, BalanceBox_Template tpl, decimal givenCredit, bool isAllowTransfer)
        {
            tpl.ForMember = null;
            tpl.ForMember1 = null;
            tpl.ForMember2 = null;
            tpl.ForAgent.AgentOutstandingLabel = Sunplus.Agent.Utilities.Formater.TruncateWithTip(Resources.Lang.lblAgentOutstanding, 18);

            decimal totalCash = this.GetDBDecimalValue(dtrBalanceInfo["agentCash"]);
            decimal yesterdayCash = this.GetDBDecimalValue(dtrBalanceInfo["yesterday_agentCash"]);
            decimal agentoutstanding = this.GetDBDecimalValue(dtrBalanceInfo["agentPositionTaking"]);
            decimal usedCredit = this.GetDBDecimalValue(dtrBalanceInfo["agentUsedCredit"]);
            decimal memberMaxCredit = this.GetDBDecimalValue(dtrBalanceInfo["memberMaxCredit"]);

            decimal customerCash = this.GetDBDecimalValue(dtrBalanceInfo["customerCash"]);
            decimal yesterdayCustCash = this.GetDBDecimalValue(dtrBalanceInfo["yesterday_customerCash"]);
            decimal totalBalance = totalCash + customerCash;
            decimal yesterdayBalance = yesterdayCash + yesterdayCustCash;

            decimal agentCreditMin = this.GetDBDecimalValue(dtrCustomerInfo["AgentCredit_Min"]);
            decimal agentCreditMax = this.GetDBDecimalValue(dtrCustomerInfo["AgentCredit_Max"]);
            tpl.GivenCreditLimit = string.Format(">={0} {1} <={2}", Formater.FormatNumber20(agentCreditMin, CurrentUserSession.LangKey), Resources.Lang.and, Formater.FormatNumber20(agentCreditMax, CurrentUserSession.LangKey));
            tpl.ForAgent1.UsedCredit = this.FormatNumber2WithColor(usedCredit, false);
            string memMaxCreditLimit = string.Empty;
            if (!CurrentUserSession.IsSuper)
            {
                decimal memberMaxCreditMin = this.GetDBDecimalValue(dtrCustomerInfo["MemMaxCredit_Min"]);
                memMaxCreditLimit = string.Format(">={0} {1} <={2}", Formater.FormatNumber20(memberMaxCreditMin, CurrentUserSession.LangKey), Resources.Lang.and, Formater.FormatNumber20(givenCredit, CurrentUserSession.LangKey));
            }

            tpl.MemMaxCredit.MemMaxCreditLimit = memMaxCreditLimit;

            //member max credit
            string allowEditMaxCredit = "class='b_nleft' style='cursor:pointer;' onclick='return changeCredit(true, \"txtMemMaxCredit\");'";

            //do not allow Super or Sub-acc to update
            if (CurrentUserSession.IsSuper || !CurrentUserSession.SubPageAccess.UpdateAccount)
            {
                allowEditMaxCredit = "class='positive'";
            }

            tpl.MemMaxCredit.EditMemMaxCredit = allowEditMaxCredit;
            tpl.MemMaxCredit.MemberMaxCredit = Formater.FormatNumber20(memberMaxCredit, CurrentUserSession.LangKey);

            if (CurrentUserSession.IsSuper)
            {
                isAllowTransfer = false; //Super view Agent: do not allow to transfer
            }

            this.RenderBalanceBoxCashBalance(ref tpl, totalCash, yesterdayCash, totalBalance, yesterdayBalance, isAllowTransfer);
            this.RenderBalanceBoxOutstanding(ref tpl, agentoutstanding);
        }

        /// <summary>
        /// Render balance box info for master
        /// </summary>
        /// <param name="tpl"></param>
        /// <param name="isAllowTransfer"></param>
        private void RenderBalanceBoxMasterInfo(DataRow dtrCustomerInfo, DataRow dtrBalanceInfo, BalanceBox_Template tpl, bool isAllowTransfer)
        {
            tpl.ForMember = null;
            tpl.ForMember1 = null;
            tpl.ForMember2 = null;
            tpl.MemMaxCredit = null;
            tpl.ForAgent.AgentOutstandingLabel = Sunplus.Agent.Utilities.Formater.TruncateWithTip(Resources.Lang.lblMasterOutstanding, 18); ;

            decimal totalCash = this.GetDBDecimalValue(dtrBalanceInfo["masterCash"]);
            decimal yesterdayCash = this.GetDBDecimalValue(dtrBalanceInfo["yesterday_masterCash"]);
            decimal masterOutstanding = this.GetDBDecimalValue(dtrBalanceInfo["masterPositionTaking"]);
            decimal usedCredit = this.GetDBDecimalValue(dtrBalanceInfo["masterUsedCredit"]);

            decimal customerCash = this.GetDBDecimalValue(dtrBalanceInfo["customerCash"]);
            decimal yesterdayCustCash = this.GetDBDecimalValue(dtrBalanceInfo["yesterday_customerCash"]);
            decimal agentCash = this.GetDBDecimalValue(dtrBalanceInfo["agentCash"]);
            decimal yesterdayAgentCash = this.GetDBDecimalValue(dtrBalanceInfo["yesterday_agentCash"]);
            decimal totalBalance = totalCash + agentCash + customerCash;
            decimal yesterdayBalance = yesterdayCash + yesterdayAgentCash + yesterdayCustCash;

            decimal masterCreditMin = this.GetDBDecimalValue(dtrCustomerInfo["MasterCredit_Min"]);
            decimal masterCreditMax = this.GetDBDecimalValue(dtrCustomerInfo["MasterCredit_Max"]);
            tpl.GivenCreditLimit = string.Format(">={0} {1} <={2}", Formater.FormatNumber20(masterCreditMin, CurrentUserSession.LangKey), Resources.Lang.and, Formater.FormatNumber20(masterCreditMax, CurrentUserSession.LangKey));
            tpl.ForAgent1.UsedCredit = this.FormatNumber2WithColor(usedCredit, false);

            this.RenderBalanceBoxCashBalance(ref tpl, totalCash, yesterdayCash, totalBalance, yesterdayBalance, isAllowTransfer);
            this.RenderBalanceBoxOutstanding(ref tpl, masterOutstanding);
        }

        private void RenderBalanceBoxCashBalance(ref BalanceBox_Template tpl, decimal totalCash, decimal yesterdayCash, decimal totalBalance, decimal yesterdayBalance, bool isAllowTransfer)
        {
            tpl.ForAgent.TotalCash = this.FormatNumber2WithColor(totalCash, false);
            tpl.ForAgent.YesterdayCash = this.FormatNumber2WithColor(yesterdayCash, false);
            if (Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowedTotalBalanceTransfer)
            {
                tpl.ForAgent.TotalBalance = this.GenTransfer2Agent(this.custId, totalBalance, isAllowTransfer);
                tpl.ForAgent.YesterdayBalance = this.FormatNumber2WithColor(yesterdayBalance, false);
            }
            else
            {
                tpl.ForAgent.TotalBalance = this.FormatNumber2WithColor(totalBalance, false);
                tpl.ForAgent.YesterdayBalance = this.GenTransfer2Agent(this.custId, yesterdayBalance, isAllowTransfer);
            }
        }

        private string FormatNumber2WithColor(decimal number, bool hasColorOver)
        {
            string className = string.Empty;
            if (number < 0)
            {
                className = "negative ";
                if (hasColorOver)
                {
                    className += CLASS_NAME;
                }
            }
            else
                if (number >= 0)
                {
                    if (hasColorOver && number != 0)
                    {
                        className = CLASS_NAME; //IE6
                    }
                    else
                    {
                        className = "positive ";
                    }
                }

            return string.Format("<font class='{0}'>{1}</font>", className, Formater.FormatNumber20(number, CurrentUserSession.LangKey));
        }

        private string GenTransferInfo(int userid, decimal value, bool isTransfer)
        {
            string html = string.Empty;
            string transferClass = string.Empty;
            string transferLink = string.Empty;
            if (value != 0 && isTransfer)
            {
                transferClass = CLASS_NAME;
                transferLink = string.Format("href='../_Transfer/Agent/MemberTransfer.aspx?custid={0}&CallBack=false'", userid);
                html = "<a class=" + transferClass + " onclick='clearTopMenu();' target='main' " + transferLink + " >" + this.FormatNumber2WithColor(value, true) + "</a>";
            }
            else
            {
                transferClass = transferLink = string.Empty;
                html = this.FormatNumber2WithColor(value, false);
            }

            return html;
        }

        private string GenTransfer2Agent(int userid, decimal value, bool isTransfer)
        {
            string html = string.Empty;
            string transferClass = string.Empty;
            string transferLink = string.Empty;
            if (value != 0 && isTransfer)
            {
                transferClass = CLASS_NAME;
                if (CurrentUserSession.RoleId == 4)
                {
                    transferLink = string.Format("href='../_Transfer/Super/MasterTransfer.aspx?amt={0}&custid={1}&status={2}&CallBack=false'", value, userid, "1");
                }
                else if (CurrentUserSession.RoleId == 3)
                {
                    transferLink = string.Format("href='../_Transfer/Master/AgentTransfer.aspx?amt={0}&custid={1}&status={2}&CallBack=false'", value, userid, "1");
                }

                html = "<a class=" + transferClass + " onclick='clearTopMenu();' target='main' " + transferLink + " >" + this.FormatNumber2WithColor(value, true) + "</a>";
            }
            else
            {
                transferClass = transferLink = string.Empty;
                html = this.FormatNumber2WithColor(value, false);
            }

            return html;
        }

        #endregion Render Balance Box
    }
}
