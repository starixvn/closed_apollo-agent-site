using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fanex.Age.UI;
using TplCore.Element;

/// <summary>
/// Summary description for IcoViewDownline_Control
/// </summary>
///
namespace Sunplus.Agent.UI._MemberInfo.CustomerList.Components
{
    using Fanex.Agent.UI._MemberInfo.CustomerList.Components.IcoViewDownline_Control;

    public class IcoViewDownline : IComponent
    {
        IcoViewDownline_Control_Template tpl = new IcoViewDownline_Control_Template();

        public IcoViewDownline(IElement parent, string viewClassName, string onClick)
        {
            if (parent != null) parent.RegisterComponent("IcoViewDownline_Control", this);
            tpl.onClick = onClick;
            tpl.text = Resources.Lang.view;
            tpl.viewClassName = viewClassName;
        }

        public static implicit operator string(IcoViewDownline value)
        {
            return value.ToString();
        }

        public void HideLinkedResources()
        {
            return;
        }

        public override string ToString()
        {
            return tpl.ToString();
        }
    }
}