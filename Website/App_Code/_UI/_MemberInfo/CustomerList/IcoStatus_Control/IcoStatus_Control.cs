using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fanex.Age.UI;
using TplCore.Element;

namespace Sunplus.Agent.UI._MemberInfo.CustomerList.Components
{
    using IcoStatus_Control;
    using Sunplus.Agent.Model.Entities;
    public class IcoStatus : IComponent
    {
        string divClass = string.Empty;
        IcoStatus_Control_Template tpl = new IcoStatus_Control_Template();

        public IcoStatus(IElement parent, Dictionary<object, bool> dictStatus, ref string bgColor, string statusOnClick, string statusMenuOnClick) : this(parent, dictStatus,ref bgColor, statusOnClick, statusMenuOnClick, false) { }
        /// <summary>
        /// dictStatus : Enabled, Closed, Suspended, DisableOutright
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="dictStatus"></param>
        /// <param name="statusOnClick"></param>
        /// <param name="statusMenuOnClick"></param>
        public IcoStatus(IElement parent, Dictionary<object, bool> dictStatus, ref string bgColor, string statusOnClick, string statusMenuOnClick, bool removeLink)
        {
            if (parent != null) parent.RegisterComponent("IcoStatus_Control", this);
            tpl.ExternalResources = null;
            string linkText = string.Empty;
            if (dictStatus["Enabled"])
            {
                if (dictStatus["Closed"])
                {
                    linkText = Resources.Lang.close;
                    bgColor = "RowBgClosed";
                }
                else if (dictStatus["Suspended"])
                {
                    linkText = Resources.Lang.suspend;
                    bgColor = "RowBgSuspended";
                }
                else
                {
                    linkText = Resources.Lang.open;                    
                    bgColor = "RowBgOpen";
                }
            }
            else
            {
                linkText = Resources.Lang.disabled;                
                bgColor = "RowBgDisabled";
            }
            if (!removeLink)
                tpl.remove_text.text = linkText;
            else
                tpl.remove_text.SetValue(linkText);

            tpl.remove_text.onClick_Status = statusOnClick;
            tpl.onClick_statusMenu = statusMenuOnClick;
            tpl.Status = Resources.Lang.status;
        }        

        public string color()
        {
            return divClass;
        }


        #region IComponent Members
        public static implicit operator string(IcoStatus value)
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

        #endregion
    }

}
