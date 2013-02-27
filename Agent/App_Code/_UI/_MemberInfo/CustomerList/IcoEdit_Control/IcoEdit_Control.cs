using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fanex.Age.UI;
using TplCore.Element;
/// <summary>
/// Summary description for IcoEdit_Control
/// </summary>
/// 
namespace Sunplus.Agent.UI._MemberInfo.CustomerList.Components
{
    using IcoEdit_Control;
    public class IcoEdit : IComponent
    {
        IcoEdit_Control_Template tpl = new IcoEdit_Control_Template();
        
        /// <summary>
        /// Create Icon Edit Function
        /// Status: 0..3: Open, Closed, Disabled, Sync
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="status"></param>
        public IcoEdit(IElement parent, int status, string divId, string divName, string onClick)
        {
            if (parent != null) parent.RegisterComponent("IcoEdit_Control", this);

            string divClass = string.Empty;
            switch (status)
            {
                case 0:
                    divClass = "ico_disable";
                    tpl.title = "";
                    break ;
                case 1:
                    divClass = "ico_Edit";
                    tpl.title = Resources.Lang.edit;
                    break;
                case 2:
                    divClass = "ico_edit_disable";
                    tpl.title = Resources.Lang.edit;
                    break;
                case 3:
                    divClass = "ico_Sync";
                    tpl.title = Resources.Lang.sync;
                    break;
                case 4:
                    divClass = "ico_ViewIcon";
                    tpl.title = Resources.Lang.view;
                    break;
            }
            tpl.divClass = divClass;
            tpl.divId = divId;
            tpl.divName = divName;
            tpl.onClick = onClick;
        }
        /// <summary>
        /// Create Icon Edit Function
        /// Status: 0..3: Open, Closed, Disabled, Sync
        /// divId = divName
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="status"></param>
        /// <param name="divId"></param>
        /// <param name="onClick"></param>
        public IcoEdit(IElement parent, int status, string divId, string onClick) : this(parent, status, divId, divId, onClick) { }
        public static implicit operator string(IcoEdit value)
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