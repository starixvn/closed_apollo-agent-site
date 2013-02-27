using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fanex.Age.UI;
using TplCore.Element;

/// <summary>
/// Summary description for IcoOther_Control
/// </summary>
/// 
namespace Sunplus.Agent.UI._MemberInfo.CustomerList.Components
{
    using IcoOther_Control;
    public class IcoOther : IComponent
    {
        IcoOther_Control_Template tpl = new IcoOther_Control_Template();
        public IcoOther(IElement parent, string onClick)
        {
            if (parent != null) parent.RegisterComponent("IcoOther_Control", this);
            tpl.onClick = onClick;
            tpl.Edit = Resources.Lang.edit;
        }

        public static implicit operator string(IcoOther value)
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
