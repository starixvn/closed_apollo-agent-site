namespace Sunplus.Agent.UI._AccountInfo.AccountInfoBase
{
    using System;
    using System.Data;
    using Fanex.Age.UI;

    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.UI._MemberInfo.CustomerList.Components;
    using Sunplus.Agent.Model.BL.Definitions;
    //using Fanex.LiveCasinoSetting.Business;
    using Sunplus.Agent.Utilities;
    using Fanex.Agent.UI._AccountInfo;

    public abstract class AccountInfoBase_Page : TplPage
    {
        #region Variable

        private int custId = -1;
        private bool isMember = false;
        private string userGroup = string.Empty;
        const string HTML_IFRAME = "<iframe id='frmDownlineList' name='frmDownlineList' src='../_AccountInfo/DownLineTreeView.aspx?root={0}' style='width:98%;height: 560px'></iframe>";
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
            this.userGroup = dtrCustomerInfo["UGroup"].ToString();

            Fanex.Age.DTO.Reports.TreeView.TreeViewNode.SearchingID = (int)dtrCustomerInfo["ObjectId"];
            Fanex.Age.DTO.Reports.TreeView.TreeViewNode.Type = 1; //1: tree in tab account
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
                tpl.HiddenBlockHeaderTabAgent.AgentID = this.custId.ToString();
                tpl.HiddenBlockHeaderTabMember.AgentID = this.custId.ToString();

                if (!bool.Parse(dtrCustomerInfo["HasAgentDownLine"].ToString()))
                {
                    tpl.HiddenBlockTabAgent.TreeViewDownLineFrame = "<span class=\"no-data\"> " + Resources.Lang.noInfo + "<span>";
                }
                else
                {
                    tpl.HiddenBlockTabAgent.TreeViewDownLineFrame = string.Format(HTML_IFRAME, this.custId.ToString()); 
                }
            }

            this.RenderSettingBoxEditLink(tpl, dtrCustomerInfo);
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
        private void RenderSettingBoxEditLink(SettingBox_Template tpl, DataRow dtrCustomerInfo)
        {
            //Other settings
            SettingBoxView_Template tplSetting = new SettingBoxView_Template();

            if (isMember)
            {
                //Page.ResolveUrl("~/
                tplSetting.ForDirectDownline.editSportBookLink = Response.ApplyAppPathModifier(string.Format("~/_MemberInfo/CustomerSetting/Pages/EditMemberSetting.aspx?custId={0}&username={1}&showheader=1&ugroup={2}", this.custId, this.userNameRequest, this.userGroup));
                tplSetting.ForDirectDownline.editCommLink = Response.ApplyAppPathModifier(string.Format("~/_MemberInfo/CustomerSetting/Pages/EditMemberCommission.aspx?custId={0}&username={1}&showheader=1&ugroup={2}", this.custId, this.userNameRequest, this.userGroup));
                tplSetting.ForDirectDownline.editBetSettingLink = Response.ApplyAppPathModifier(string.Format("~/_MemberInfo/CustomerSetting/Pages/EditMemberBetLimit.aspx?custId={0}&username={1}&showheader=1&ugroup={2}", this.custId, this.userNameRequest, this.userGroup));
                tplSetting.ForDirectDownline.editPTLink = Response.ApplyAppPathModifier(string.Format("~/_MemberInfo/CustomerSetting/Pages/EditMemberPositionTaking.aspx?custId={0}&username={1}&showheader=1&ugroup={2}", this.custId, this.userNameRequest, this.userGroup));
            }
            else
            {
                tplSetting.ForDirectDownline.editSportBookLink = Response.ApplyAppPathModifier(string.Format("~/_MemberInfo/CustomerSetting/Pages/EditAgentSetting.aspx?custId={0}&username={1}&showheader=1", this.custId, this.userNameRequest));
                tplSetting.ForDirectDownline.editCommLink = Response.ApplyAppPathModifier(string.Format("~/_MemberInfo/CustomerSetting/Pages/EditAgentCommission.aspx?custId={0}&username={1}&showheader=1", this.custId, this.userNameRequest));
                tplSetting.ForDirectDownline.editBetSettingLink = Response.ApplyAppPathModifier(string.Format("~/_MemberInfo/CustomerSetting/Pages/EditAgentBetLimit.aspx?custId={0}&username={1}&showheader=1", this.custId, this.userNameRequest));
                tplSetting.ForDirectDownline.editPTLink = Response.ApplyAppPathModifier(string.Format("~/_MemberInfo/CustomerSetting/Pages/EditAgentPositionTaking.aspx?custId={0}&username={1}&showheader=1", this.custId, this.userNameRequest));
            }

            tpl.otherSettings = tplSetting.ToString();
        }

        #endregion Render Setting Box
    }
}
