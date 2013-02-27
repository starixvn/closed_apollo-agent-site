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
    using IcoEditMulti_Control;
    public class IcoEditMulti : IComponent
    {
        IcoEditMulti_Control_Template tpl = new IcoEditMulti_Control_Template();
      
        public IcoEditMulti(IElement parent, string onClick)
        {
            if (parent != null) parent.RegisterComponent("IcoEditMulti", this);

            tpl.onClick = onClick;
        }

     
        public static implicit operator string(IcoEditMulti value)
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