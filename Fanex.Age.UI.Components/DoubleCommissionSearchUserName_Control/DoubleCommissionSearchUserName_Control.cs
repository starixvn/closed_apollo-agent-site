using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fanex.Age.UI;
using Sunplus.Agent.Model.Entities.Permission;
using TplCore.Element;

namespace Fanex.Age.UI.Components.DoubleCommissionSearchUserName_Control
{
    public class DoubleCommissionSearchUserName : IComponent
    {
        Dictionary<object, int> statusFilterDict = new Dictionary<object, int>() {
            {Resources.Lang.all, 0},{Resources.Lang.open, 1},
            {Resources.Lang.suspended, 2},{Resources.Lang.closed, 3},
            {Resources.Lang.disabled, 4}
        };

        Dictionary<object, int> dcommFilterDict = new Dictionary<object, int>() {
            {Resources.Lang.all, 2},{Resources.Lang.allowed, 1},
            {Resources.Lang.disallowed, 0}
        };

        string usernameOld = string.Empty;
        DoubleCommissionSearchUserName_Control_Template tpl = new DoubleCommissionSearchUserName_Control_Template();

        public DoubleCommissionSearchUserName(IElement parent, string textValue, int indexOption, int dcommIndexOption, string onclickFunction, bool isFilter = false, bool isChecked = false)
        {
            if (parent != null) parent.RegisterComponent("SearchUserName_Control", this);

            tpl.username = Resources.Lang.username;
            //tpl.status = Resources.Lang.status;
            tpl.enterusername = Resources.Lang.enterusername;
            tpl.submit = Resources.Lang.submit;

            tpl.onClick_function = onclickFunction;
            if (textValue.ToLower() == Resources.Lang.enterusername.ToLower())
            {
                tpl.textClass = "text_italic";
            }
            else
            {
                tpl.textClass = "text_normal";
            }

            tpl.value = textValue;
            usernameOld = textValue;
            CreateStatusFilterOption(indexOption);
            CreateDcommFilterOption(dcommIndexOption);
            if (isFilter)
            {
                tpl.CheckingFilter.chkId = "chkYesterdayBalance";
                tpl.CheckingFilter.CheckBoxText = Resources.Lang.allyesterdaybalance;
                tpl.CheckingFilter.ischecked = isChecked ? "checked" : "";
            }
            else
            {
                tpl.CheckingFilter = null;
            }
        }

        public DoubleCommissionSearchUserName(IElement parent, string textValue, int indexOption, int dcommIndexOption, string onclickFunction, Dictionary<object, int> dcommOptions, bool isFilter = false, bool isChecked = false)
        {
            if (parent != null) parent.RegisterComponent("SearchUserName_Control", this);

            tpl.username = Resources.Lang.username;
            //tpl.status = Resources.Lang.status;
            tpl.enterusername = Resources.Lang.enterusername;
            tpl.submit = Resources.Lang.submit;

            SetDoubleCommissionFilterStatus(dcommOptions);
            /*
            if (!UserPermission.IsShowDoubleCommission)
            {
                tpl.DCom.Hide();
            }
            else
            {
                tpl.DCom.doublecommission = Resources.Lang.doublecommission;
            }
            */
            tpl.onClick_function = onclickFunction;
            if (textValue.ToLower() == Resources.Lang.enterusername.ToLower())
            {
                tpl.textClass = "text_italic";
            }
            else
            {
                tpl.textClass = "text_normal";
            }

            tpl.value = textValue;
            usernameOld = textValue;
            CreateStatusFilterOption(indexOption);
            CreateDcommFilterOption(dcommIndexOption);
            if (isFilter)
            {
                tpl.CheckingFilter.chkId = "chkYesterdayBalance";
                tpl.CheckingFilter.CheckBoxText = Resources.Lang.allyesterdaybalance;
                tpl.CheckingFilter.ischecked = isChecked ? "checked" : "";
            }
            else
            {
                tpl.CheckingFilter = null;
            }
        }

        private void CreateStatusFilterOption(int index)
        {
            /*
            DoubleCommissionSearchUserName_Control_statusOption_Block tplOption = new DoubleCommissionSearchUserName_Control_statusOption_Block();
            tpl.statusOption.Clear();
            foreach (object key in statusFilterDict.Keys)
            {
                if (index == statusFilterDict[key])
                {
                    tplOption.selected = "selected";
                }
                else
                {
                    tplOption.selected = string.Empty;
                }

                tplOption.optionText = key as string;
                tplOption.optionValue = statusFilterDict[key].ToString();
                tpl.statusOption.Append(tplOption);
            }
             * */
        }

        private void CreateDcommFilterOption(int index)
        {
            /* DoubleCommissionSearchUserName_Control_DCom_dCommOption_Block tplOption = new DoubleCommissionSearchUserName_Control_DCom_dCommOption_Block();
             tpl.DCom.dCommOption.Clear();
             foreach (object key in dcommFilterDict.Keys)
             {
                 if (index == dcommFilterDict[key])
                 {
                     tplOption.selected = "selected";
                 }
                 else
                 {
                     tplOption.selected = string.Empty;
                 }

                 tplOption.optionText = key as string;
                 tplOption.optionValue = dcommFilterDict[key].ToString();
                 tpl.DCom.dCommOption.Append(tplOption);
             }
            */
        }

        private string usernamOld()
        {
            return usernameOld;
        }

        public static implicit operator string(DoubleCommissionSearchUserName value)
        {
            return value.ToString();
        }

        public void HideLinkedResources()
        {
            tpl.ExternalResources = null;
        }

        public override string ToString()
        {
            return tpl.ToString();
        }

        public void SetDoubleCommissionFilterStatus(Dictionary<object, int> statusDict)
        {
            this.dcommFilterDict.Clear();
            foreach (KeyValuePair<object, int> e in statusDict)
            {
                this.dcommFilterDict.Add(e.Key, e.Value);
            }
        }
    }
}