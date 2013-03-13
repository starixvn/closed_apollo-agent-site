/*
* Created: David
* Task: Summary description for CustomerList_Page
* Revisions:
*    - 20111228@Terry: Edit getting resource for Close Downline - InitJsLang()
*    - 20120530@Kaden: Refactor code and check StyleCop
*/

namespace Sunplus.Agent.UI._MemberInfo.CustomerList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using Fanex.Age.UI;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.UI._MemberInfo.CustomerList.Components;

    public abstract class CustomerList_Page : TplPage
    {
        #region Variable

        private MasterPage_Template _templateMain = new MasterPage_Template();
        private Hashtable _hashTableQueryString = new Hashtable();
        private Dictionary<string, DataTable> _viewData = new Dictionary<string, DataTable>();

        private int _pageSize = Sunplus.Agent.Model.BL.Definitions.AppSettings.PageSize;
        private int _pageIndex = 1;
        private int _status = -1;
        private int _dcommStatus = 2;

        private object[] arrayDisabledPopupMenu = null;
        private object[] arrayTextPopupMenu = null;
        private object[] arrayIdPopupMenu = null;
        private string isHideTransfer = string.Empty;
        private Hashtable hashTableVars = new Hashtable();
        private List<object> pageSizeView = new List<object>(new object[] { 10, 50, 100, 500 });
        private DataRow row = null;
        private bool disabledEdit = false;

        #endregion Variable

        #region Properties

        public CustomerList_Page()
        {
        }

        public MasterPage_Template TemplateMain
        {
            get
            {
                return this._templateMain;
            }
        }

        public Hashtable HashTableQueryString
        {
            get
            {
                return this._hashTableQueryString;
            }
        }

        public int ParentId { get; set; }

        public int CustId { get; set; }

        public int TotalPage { get; set; }

        public string UserName { get; set; }

        public Dictionary<string, DataTable> ViewData
        {
            get
            {
                return this._viewData;
            }

            set
            {
                this._viewData = value;
            }
        }

        public int PageSize
        {
            get
            {
                return this._pageSize;
            }

            set
            {
                this._pageSize = value;
            }
        }

        public int PageIndex
        {
            get
            {
                return this._pageIndex;
            }

            set
            {
                this._pageIndex = value;
            }
        }

        public int Status
        {
            get
            {
                return this._status;
            }

            set
            {
                this._status = value;
            }
        }

        public int DCommStatus
        {
            get
            {
                return this._dcommStatus;
            }

            set
            {
                this._dcommStatus = value;
            }
        }

        #endregion Properties

        public void InitJsLang()
        {
            this.disabledEdit = !CurrentUserSession.SubPageAccess.UpdateAccount;
            this.JavaScriptRegister("lblConfirmClearCredit", Resources.Lang.lblConfirmClearCreditMember);
            this.JavaScriptRegister("confirmDisableNumberGameDownline", Resources.Lang.confirmDisableNumberGame);
            this.JavaScriptRegister("confirmSuspendMem", Resources.Lang.confirmSuspendMem);
            this.JavaScriptRegister("confirmSuspendDownline", Resources.Lang.confirmSuspendDownline);
            this.JavaScriptRegister("confirmUnSuspendMem", Resources.Lang.confirmUnSuspendMem);
            this.JavaScriptRegister("confirmUnSuspendDownline", Resources.Lang.confirmUnSuspended);
            this.JavaScriptRegister("confirmCloseMemDownline", Resources.Lang.confirmCloseDownline);
            this.JavaScriptRegister("confirmCloseMem", Resources.Lang.confirmCloseMem);
        }

        public void InitPopupStatus(bool isSuper)
        {
            // Init Div Status's Popup
            this.arrayDisabledPopupMenu = new object[4];

            this.arrayTextPopupMenu = new object[4];

            this.arrayIdPopupMenu = new object[4];

            this.arrayDisabledPopupMenu[0] = 0;

            this.arrayTextPopupMenu[0] = Resources.Lang.closed;

            this.arrayIdPopupMenu[0] = "chk_closed";

            this.arrayDisabledPopupMenu[1] = 0;

            this.arrayTextPopupMenu[1] = Resources.Lang.suspended;

            this.arrayIdPopupMenu[1] = "chk_suspended";

            this.arrayDisabledPopupMenu[2] = isSuper ? 0 : 1;

            this.arrayTextPopupMenu[2] = Resources.Lang.allowautopt;

            this.arrayIdPopupMenu[2] = "chk_allowAutoPT";

            this.arrayDisabledPopupMenu[3] = 0;

            this.arrayTextPopupMenu[3] = Resources.Lang.lblenableOutRight;

            this.arrayIdPopupMenu[3] = "chk_outright";

            this.RenderMenuPopupStatus(this.arrayTextPopupMenu, this.arrayIdPopupMenu, this.arrayDisabledPopupMenu);
        }

        public void InitPopupMulti()
        {
            // Init Div MultiEdit's Popup

            int multiEditItemNumber = 8;

            this.arrayDisabledPopupMenu = new object[multiEditItemNumber];

            this.arrayTextPopupMenu = new object[multiEditItemNumber];

            this.arrayIdPopupMenu = new object[multiEditItemNumber];

            this.arrayDisabledPopupMenu[0] = 0;

            this.arrayTextPopupMenu[0] = Resources.Lang.editmultsportbook;

            this.arrayIdPopupMenu[0] = "sportBook";

            this.arrayDisabledPopupMenu[1] = 0;

            this.arrayTextPopupMenu[1] = Resources.Lang.editmulthr;

            this.arrayIdPopupMenu[1] = "racing";

            this.arrayDisabledPopupMenu[2] = 0;

            this.arrayTextPopupMenu[2] = Resources.Lang.editmultcs;

            this.arrayIdPopupMenu[2] = "casino";

            this.arrayDisabledPopupMenu[3] = 0;

            this.arrayTextPopupMenu[3] = Resources.Lang.editmultbingo;

            this.arrayIdPopupMenu[3] = "bingo";

            this.arrayDisabledPopupMenu[4] = 0;

            this.arrayTextPopupMenu[4] = Resources.Lang.editmultp2p;

            this.arrayIdPopupMenu[4] = "P2P";

            this.arrayDisabledPopupMenu[5] = 0;

            this.arrayTextPopupMenu[5] = Resources.Lang.EditLiveCasinoMultiple;

            this.arrayIdPopupMenu[5] = "livecasino";

            this.arrayDisabledPopupMenu[6] = 0;

            this.arrayTextPopupMenu[6] = Resources.Lang.editMultipleVirtualSports;

            this.arrayIdPopupMenu[6] = "virtualsports";

            this.arrayDisabledPopupMenu[7] = 0;

            this.arrayTextPopupMenu[7] = Resources.Lang.editMultipleDoubleComm;

            this.arrayIdPopupMenu[7] = "doublecomm";

            this.RenderPopupMultiEdit(this.arrayTextPopupMenu, this.arrayDisabledPopupMenu, this.arrayIdPopupMenu);
        }

        public void InitPopupOther_EditAgent()
        {
            // Init Div Other's Popup
            int number = 5;

            if (CurrentUserSession.IsSubAcc && !CurrentUserSession.SubPageAccess.UpdateAccount)
            {
                number--;
            }

            this.arrayDisabledPopupMenu = new object[number];
            this.arrayTextPopupMenu = new object[number];
            this.arrayIdPopupMenu = new object[number];

            this.arrayDisabledPopupMenu[0] = 0;
            this.arrayTextPopupMenu[0] = Resources.Lang.horseRacing;
            this.arrayIdPopupMenu[0] = "racingPT";

            this.arrayDisabledPopupMenu[1] = 0;
            this.arrayTextPopupMenu[1] = Resources.Lang.casino;
            this.arrayIdPopupMenu[1] = "casinoPT";

            this.arrayDisabledPopupMenu[2] = 0;
            this.arrayTextPopupMenu[2] = Resources.Lang.P2P;
            this.arrayIdPopupMenu[2] = "p2pPT";

            this.arrayDisabledPopupMenu[3] = 0;
            this.arrayTextPopupMenu[3] = Resources.Lang.bingo;
            this.arrayIdPopupMenu[3] = "bingoPT";

            if (number > 4)
            {
                this.arrayDisabledPopupMenu[4] = 0;
                this.arrayTextPopupMenu[4] = Resources.Lang.password;
                this.arrayIdPopupMenu[4] = "password";
            }

            this.RenderOtherStatus(this.arrayTextPopupMenu, this.arrayDisabledPopupMenu, this.arrayIdPopupMenu);
        }

        public void InitPopupOther_EditMember()
        {
            // Init Div Other's Popup
            int number = 10;

            if (CurrentUserSession.RoleId == 4 || CurrentUserSession.RoleId == 3)
            {
                number = 7;
            }

            if (CurrentUserSession.IsSubAcc && !CurrentUserSession.SubPageAccess.UpdateAccount)
            {
                number--;
            }

            this.arrayDisabledPopupMenu = new object[number];
            this.arrayTextPopupMenu = new object[number];
            this.arrayIdPopupMenu = new object[number];

            this.arrayDisabledPopupMenu[0] = 0;
            this.arrayTextPopupMenu[0] = Resources.Lang.horseRacing;
            this.arrayIdPopupMenu[0] = "racingPT";

            this.arrayDisabledPopupMenu[1] = 0;
            this.arrayTextPopupMenu[1] = Resources.Lang.casino;
            this.arrayIdPopupMenu[1] = "casinoPT";

            this.arrayDisabledPopupMenu[2] = 0;
            this.arrayTextPopupMenu[2] = Resources.Lang.P2P;
            this.arrayIdPopupMenu[2] = "p2pPT";

            this.arrayDisabledPopupMenu[3] = 0;
            this.arrayTextPopupMenu[3] = Resources.Lang.bingo;
            this.arrayIdPopupMenu[3] = "bingoPT";

            this.arrayDisabledPopupMenu[4] = 0;
            this.arrayTextPopupMenu[4] = Resources.Lang.LiveCasino;
            this.arrayIdPopupMenu[4] = "livecasinoPT";

            this.arrayDisabledPopupMenu[5] = 0;
            this.arrayTextPopupMenu[5] = Resources.Lang.VirtualSports;
            this.arrayIdPopupMenu[5] = "virtualsportsPT";

            if (CurrentUserSession.RoleId != 4 && CurrentUserSession.RoleId != 3)
            {
                this.arrayDisabledPopupMenu[6] = this.disabledEdit ? 1 : 0;
                this.arrayTextPopupMenu[6] = Resources.Lang.lblEditInfo;
                this.arrayIdPopupMenu[6] = "editInfo";

                this.arrayDisabledPopupMenu[7] = this.disabledEdit ? 1 : 0;
                this.arrayTextPopupMenu[7] = Resources.Lang.lblBetSetting;
                this.arrayIdPopupMenu[7] = "betSetting";

                this.arrayDisabledPopupMenu[8] = this.disabledEdit ? 1 : 0;
                this.arrayTextPopupMenu[8] = Resources.Lang.commission;
                this.arrayIdPopupMenu[8] = "commission";

                if (number > 9)
                {
                    this.arrayDisabledPopupMenu[9] = 0;
                    this.arrayTextPopupMenu[9] = Resources.Lang.password;
                    this.arrayIdPopupMenu[9] = "password";
                }
            }
            else
            {
                if (number > 6)
                {
                    this.arrayDisabledPopupMenu[6] = 0;
                    this.arrayTextPopupMenu[6] = Resources.Lang.password;
                    this.arrayIdPopupMenu[6] = "password";
                }
            }

            this.RenderOtherStatus(this.arrayTextPopupMenu, this.arrayDisabledPopupMenu, this.arrayIdPopupMenu);
        }

        /// <summary>
        /// Apollo Agent - Init Edit Menu Control
        /// </summary>
        public void InitEditMenuControl()
        {
            int itemArray = 3;

            this.arrayDisabledPopupMenu = new object[itemArray];
            this.arrayTextPopupMenu = new object[itemArray];
            this.arrayIdPopupMenu = new object[itemArray];

            this.arrayDisabledPopupMenu[0] = 0;
            this.arrayTextPopupMenu[0] = Resources.Lang.commission;
            this.arrayIdPopupMenu[0] = "editCommission";

            this.arrayDisabledPopupMenu[1] = 0;
            this.arrayTextPopupMenu[1] = Resources.Lang.lblBetSetting;
            this.arrayIdPopupMenu[1] = "editBetSetting";

            this.arrayDisabledPopupMenu[2] = 0;
            this.arrayTextPopupMenu[2] = Resources.Lang.positiontaking;
            this.arrayIdPopupMenu[2] = "editPositionTaking";

            this.RenderEditControl(arrayTextPopupMenu, arrayIdPopupMenu, arrayDisabledPopupMenu);
        }

        public void InitPopupOther()
        {
            // Init Div Other's Popup
            int number = 13;

            if (CurrentUserSession.IsSubAcc && !CurrentUserSession.SubPageAccess.UpdateAccount)
            {
                number--;
            }

            this.arrayDisabledPopupMenu = new object[number];
            this.arrayTextPopupMenu = new object[number];
            this.arrayIdPopupMenu = new object[number];

            this.arrayDisabledPopupMenu[0] = 0;
            this.arrayTextPopupMenu[0] = Resources.Lang.horseRacing;
            this.arrayIdPopupMenu[0] = "racingPT";

            this.arrayDisabledPopupMenu[1] = 0;
            this.arrayTextPopupMenu[1] = Resources.Lang.casino;
            this.arrayIdPopupMenu[1] = "casinoPT";

            this.arrayDisabledPopupMenu[2] = 0;
            this.arrayTextPopupMenu[2] = Resources.Lang.P2P;
            this.arrayIdPopupMenu[2] = "p2pPT";

            this.arrayDisabledPopupMenu[3] = 0;
            this.arrayTextPopupMenu[3] = Resources.Lang.bingo;
            this.arrayIdPopupMenu[3] = "bingoPT";

            this.arrayDisabledPopupMenu[4] = 0;
            this.arrayTextPopupMenu[4] = Resources.Lang.LiveCasino;
            this.arrayIdPopupMenu[4] = "livecasinoPT";

            this.arrayDisabledPopupMenu[5] = 0;
            this.arrayTextPopupMenu[5] = Resources.Lang.VirtualSports;
            this.arrayIdPopupMenu[5] = "virtualsportsPT";

            this.arrayDisabledPopupMenu[6] = 0;
            this.arrayTextPopupMenu[6] = Resources.Lang.sbminpt;
            this.arrayIdPopupMenu[6] = "sportBookMinPT";

            this.arrayDisabledPopupMenu[7] = 0;
            this.arrayTextPopupMenu[7] = Resources.Lang.hrminpt;
            this.arrayIdPopupMenu[7] = "racingMinPT";

            this.arrayDisabledPopupMenu[8] = 0;
            this.arrayTextPopupMenu[8] = Resources.Lang.transfercondition;
            this.arrayIdPopupMenu[8] = "transfer";

            this.arrayDisabledPopupMenu[9] = this.disabledEdit ? 1 : 0;
            this.arrayTextPopupMenu[9] = Resources.Lang.lblEditInfo;
            this.arrayIdPopupMenu[9] = "editInfo";

            this.arrayDisabledPopupMenu[10] = this.disabledEdit ? 1 : 0;
            this.arrayTextPopupMenu[10] = Resources.Lang.lblBetSetting;
            this.arrayIdPopupMenu[10] = "betSetting";

            this.arrayDisabledPopupMenu[11] = this.disabledEdit ? 1 : 0;
            this.arrayTextPopupMenu[11] = Resources.Lang.commission;
            this.arrayIdPopupMenu[11] = "commission";

            if (number > 12)
            {
                this.arrayDisabledPopupMenu[12] = 0;
                this.arrayTextPopupMenu[12] = Resources.Lang.password;
                this.arrayIdPopupMenu[12] = "password";
            }

            this.RenderOtherStatus(this.arrayTextPopupMenu, this.arrayDisabledPopupMenu, this.arrayIdPopupMenu);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.CustId = this.GetIntRequest("custid", CurrentUserSession.CustId);
            this.UserName = this.GetStringRequest("username", CurrentUserSession.UserName);
            this.PageSize = this.GetIntRequest("pagesize", this.PageSize);
            this.PageIndex = this.GetIntRequest("pageindex", this.PageIndex);

            if (this.PageSize > 500)
            {
                this.PageSize = 500;
            }

           // this.UserName = string.IsNullOrEmpty(this.Request["username"]) == false ? this.Request["username"] : CurrentUserSession.UserName;
            this.HashTableQueryString.Add("custid", this.CustId.ToString());
            this.HashTableQueryString.Add("username", this.UserName);

            //if (this.ParentId == 0)
            //{
            //    this.nav.Add(this.CustId, new Navigation { AgentId = this.CustId, AgentName = this.UserName, ParentAgentId = this.ParentId });
            //    CurrentUserSession.AgentNavigation = this.nav;
            //}
            //else
            //{
            //    this.nav = CurrentUserSession.AgentNavigation;
            //    if (!this.nav.ContainsKey(this.CustId))
            //    {
            //        this.nav.Add(this.CustId, new Navigation { AgentId = this.CustId, AgentName = this.UserName, ParentAgentId = this.ParentId });
            //    }
            //    else
            //    {
            //        Dictionary<int, Navigation> dicTemp = new Dictionary<int, Navigation>();
            //        foreach (int key in this.nav.Keys)
            //        {
            //            //dicTemp.Add(key, new Navigation { AgentId = this.nav. });
            //        }
            //    }
            //}
        }

        public string RenderPageSize(string id, int pagesize)
        {
            return Resources.Lang.pagesize + Utilities.HtmlGenerator.ComboBox("sel" + id, (object)pagesize, this.pageSizeView, id + ".SetPageSize(this.value)");
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public void JavaScriptRegister(object name, object value)
        {
            this.hashTableVars.Add(name, value);
        }

        public void RegisterClientVariables()
        {
            foreach (DictionaryEntry item in this.hashTableVars)
            {
                this.RegisterJavascriptObject(item.Key, item.Value);
            }
        }

        /// <summary>
        /// Render  MultiEdit's Popup icon on Header
        /// </summary>
        /// <param name="texts"></param>
        /// <param name="isDisable"></param>
        /// <param name="ids"></param>
        public virtual void RenderPopupMultiEdit(object[] texts, object[] isDisable, object[] ids)
        {
            MenuPopup menuEditMulti = new MenuPopup();
            string editMulti = "MultiPopup";
            string multi = menuEditMulti.RenderLink(texts, ids, isDisable, editMulti);
            this.TemplateMain.Extend_tpl.Append(multi);
        }

        /// <summary>
        /// Render Satus's Popup arrowIcon on Status
        /// </summary>
        /// <param name="texts"></param>
        /// <param name="ids"></param>
        /// <param name="isDisableChk"></param>
        public virtual void RenderMenuPopupStatus(object[] texts, object[] ids, object[] isDisableChk)
        {
            MenuPopup menuStatus = new MenuPopup();
            string stringPopup = "Popup";
            string menuPopup = menuStatus.RenderCheckbox(texts, ids, isDisableChk, stringPopup);
            this.TemplateMain.Extend_tpl.Append(menuPopup);
        }

        /// <summary>
        /// Render Other's Popup
        /// </summary>
        /// <param name="textsOther"></param>
        /// <param name="isDisableOther"></param>
        /// <param name="idsOther"></param>
        public virtual void RenderOtherStatus(object[] textsOther, object[] isDisableOther, object[] idsOther)
        {
            MenuPopup menuOther = new MenuPopup();
            string editOther = "divUpdOthers";
            string other = menuOther.RenderLink(textsOther, idsOther, isDisableOther, editOther);
            this.TemplateMain.Extend_tpl.Append(other);
        }

        /// <summary>
        /// Apollo Agent - Render Menu Popup Control
        /// </summary>
        /// <param name="texts"></param>
        /// <param name="ids"></param>
        /// <param name="isDisabled"></param>
        public virtual void RenderEditControl(object[] texts, object[] ids, object[] isDisabled)
        {
            MenuPopup menuPopup = new MenuPopup();
            string menu = menuPopup.RenderLink(texts, ids, isDisabled, "editControl");
            this.TemplateMain.Extend_tpl.Append(menu);
        }

        public bool DisabledRacing(DataRow dr)
        {
            bool disable151_41 = Convert.ToBoolean(dr["disable151_41"]) || !UserPermission.IsShowRacing_151_41;
            bool disable151_31 = Convert.ToBoolean(dr["disable151_31"]) || !UserPermission.IsShowRacing_151_31;
            bool disable151_51 = Convert.ToBoolean(dr["disable151_51"]) || !UserPermission.IsShowRacing_151_51;
            bool disable152_41 = Convert.ToBoolean(dr["disable152_41"]) || !UserPermission.IsShowRacing_152_41;
            bool disable152_51 = Convert.ToBoolean(dr["disable151_51"]) || !UserPermission.IsShowRacing_152_51;
            bool disable153_51 = Convert.ToBoolean(dr["disable153_51"]) || !UserPermission.IsShowRacing_153_51;
            bool disable151 = Convert.ToBoolean(dr["Disable151"]);

            disable151 = disable151_41
                & disable151_31
                & disable151_51
                & disable152_41
                & disable152_51
                & disable153_51;

            return disable151;
        }
    }
}