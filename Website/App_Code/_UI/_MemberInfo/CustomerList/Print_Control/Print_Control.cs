using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TplCore.Element;

/// <summary>
/// Summary description for MenuPopup_Control
/// </summary>
/// 
namespace Sunplus.Agent.UI._MemberInfo.CustomerList.Components
{
    using Print_Control;
    public class PrintControl : IComponent
    {
        Print_Control_Template tpl = new Print_Control_Template();

        public PrintControl(IElement parent, string onClick)
        {
            if (parent != null) parent.RegisterComponent("Print_Control", this);
            tpl.onClick = onClick;
        }

        public static implicit operator string(PrintControl value)
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
