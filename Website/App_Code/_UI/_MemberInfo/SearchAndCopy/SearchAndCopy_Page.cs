/*
 * Created 20120117@Jeff - Search And Copy
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using DAL;
    using Fanex.Age.UI;
    using Sunplus.Agent.Model.Entities;

    public abstract class SearchAndCopy_Page : TplPage
    {
        public Dictionary<string, DataTable> viewData = new Dictionary<string, DataTable>();
        public int custId = 0;
        public string userName = "";
        public string langKey = string.Empty;

        public SearchAndCopy_Page()
        {
        }

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            RegisterJavascriptObject("user", CurrentUserSession.UserName);
        }

        public void RenderHeaderStep(out string tplBlockHeader, int active)
        {
            RenderHeaderStep(out tplBlockHeader, active, false);
        }

        public void RenderHeaderStep(out string tplBlockHeader, int active, bool isShowWaringMessage)
        {
            StepHeader_Template tplBlock = new StepHeader_Template();
            tplBlock.hide = isShowWaringMessage ? string.Empty : "Hide";
            switch (UserSession.Current.RoleId)
            {
                case 2:
                    tplBlock.SearchUser = Resources.Lang.searchExistingMember;
                    break;
                case 3:
                    tplBlock.SearchUser = Resources.Lang.searchExistingAgent;

                    break;
                case 4:
                    tplBlock.SearchUser = Resources.Lang.searchExistingMaster;
                    break;
            }

            switch (active)
            {
                case 1:
                    tplBlock.step1 = "cycleEnable";
                    tplBlock.step3 = tplBlock.step2 = "cycleDisable";
                    break;
                case 2:
                    tplBlock.step2 = "cycleEnable";
                    tplBlock.step3 = tplBlock.step1 = "cycleDisable";
                    break;
                default:
                    tplBlock.step3 = "cycleEnable";
                    tplBlock.step2 = tplBlock.step1 = "cycleDisable";
                    break;
            }

            tplBlockHeader = tplBlock;
        }

        public string FormatIsAuto(object obj)
        {
            if (Convert.ToBoolean(obj))
            {
                return Resources.Lang.lblYes;
            }

            return Resources.Lang.no0;
        }

        public string FormatStackLevel(CSCustomerDetail custCS, GameTypeDetail gameTypeDetail)
        {
            string result = "";
            //List<StakeLevelDetail> lsStakeLevel = (custCS.CasinoSync) ? CasinoManager.GetCustStakeLevels(custCS.IBCCustID, gameTypeDetail.GameTypeID) : CasinoManager.GetCustStakeLevels(UserSession.Current.CustId, gameTypeDetail.GameTypeID);
            //if (lsStakeLevel != null && lsStakeLevel.Count > 0)
            //{
            //    var query = lsStakeLevel.Where(p => p.IsChecked == true).Select(p => p.StakeLevelName);
            //    result = string.Join(Globals.SEP_STAKE, query);
            //}
            return result;
        }

        public void FormatTransferCondition(object value, ref string condition, ref string day)
        {
            condition = "";
            day = "";
            int data = Convert.ToInt32(value);
            var bytes = new byte[8];
            var daysOfWeek = new string[] {
                    Resources.Lang.sunday,
                    Resources.Lang.monday,
                    Resources.Lang.tuesday,
                    Resources.Lang.wednesday,
                    Resources.Lang.thursday,
                    Resources.Lang.friday,
                    Resources.Lang.saturday
                };
            for (int i = 0; i < 8; i++)
            {
                bytes[7 - i] = (byte)(data % 10);
                data = data / 10;
            }
            if (bytes[7] == 0)
            {
                condition = Resources.Lang.daily;
                day = string.Join(Globals.SEP_STAKE, daysOfWeek);
            }
            else if (bytes[7] == 1)
            {
                condition = Resources.Lang.weekly;
                var dayList = new List<string>();
                for (int i = 0; i < 7; i++)
                {
                    if (bytes[i] == 1)
                    {
                        dayList.Add(daysOfWeek[i]);
                    }
                }
                day = string.Join(Globals.SEP_STAKE, dayList);
            }
        }

        public int GetStatus(bool closed, bool susspended)
        {
            if (!closed && !susspended)
            {
                return 1;
            }

            if (susspended)
            {
                return 2;
            }

            if (closed)
            {
                return 3;
            }
            return 0;
        }
    }
}