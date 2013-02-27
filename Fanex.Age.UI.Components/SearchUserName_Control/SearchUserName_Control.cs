using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

using Fanex.Age.UI;
using TplCore.Element;

/// <summary>
/// Summary description for SearchUserName_Control
/// </summary>
/// 
namespace Fanex.Age.UI.Components
{
    using SearchUserName_Control;

    public class SearchUserName : IComponent
    {
        Dictionary<object, int> dict = new Dictionary<object, int>() { 
            {Resources.Lang.all, 0},{Resources.Lang.open, 1},
            {Resources.Lang.suspended, 2},{Resources.Lang.closed, 3},
            {Resources.Lang.disabled, 4}
        };

        string usernameOld = string.Empty;
        SearchUserName_Control_Template tpl = new SearchUserName_Control_Template();

        public SearchUserName(IElement parent, string textValue, int indexOption, string onclickFunction, bool isFilter = false, bool isChecked = false)
        {
            if (parent != null) parent.RegisterComponent("SearchUserName_Control", this);

            tpl.username = Resources.Lang.username;
            tpl.status = Resources.Lang.status;
            tpl.enterusername = Resources.Lang.enterusername;
            tpl.submit = Resources.Lang.submit;

            tpl.onClick_function = onclickFunction;
            if (textValue.ToLower() == Resources.Lang.enterusername.ToLower())
                tpl.textClass = "text_italic";
            else
                tpl.textClass = "text_normal";

            tpl.value = textValue;
            usernameOld = textValue;
            CreateOption(tpl.option, indexOption);
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

        private void CreateOption(SearchUserName_Control_option_Block tplOption, int index)
        {
            tplOption = new SearchUserName_Control_option_Block();
            tpl.option.Clear();
            foreach (object key in dict.Keys)
            {
                if (index == dict[key])
                    tplOption.selected = "selected";
                else
                    tplOption.selected = string.Empty;

                tplOption.optionText = key as string;
                tplOption.optionValue = dict[key].ToString();
                tpl.option.Append(tplOption);
            }

        }

        private string usernamOld()
        {
            return usernameOld;
        }
        public static implicit operator string(SearchUserName value)
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
    }
}