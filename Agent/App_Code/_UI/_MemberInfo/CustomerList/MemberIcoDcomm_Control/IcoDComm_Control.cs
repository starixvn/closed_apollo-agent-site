using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fanex.Age.UI;
using TplCore.Element;
/// <summary>
/// Summary description for MemberIcoDComm_Control
/// </summary>
/// 
namespace Sunplus.Agent.UI._MemberInfo.CustomerList.Components
{    
    using Sunplus.Agent.UI._MemberInfo.CustomerList.Components.MemberIcoDcomm_Control;
    public class MemberIcoDComm : IComponent
    {
        MemberIcoDComm_Control_Template tpl = new MemberIcoDComm_Control_Template();

        /// <summary>
        /// Create Icon Edit Function
        /// Status: 0..3: Open, Closed, Disabled, Sync
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="status"></param>
        public MemberIcoDComm(IElement parent, bool status, string onClick)
        {
            if (parent != null) parent.RegisterComponent("MemberIcoDComm_Control", this);

            if (status == true) // allow double commission
            {
                tpl.DoubleComm = Resources.Lang.doublecommissionON;
                tpl.bkgDoubleComm = "bkgDcommAllowed";
            }
            else if (status == false) // disallow double commission
            {                
                tpl.DoubleComm = Resources.Lang.doublecommissionOFF;
                tpl.bkgDoubleComm = "bkgDcommDisallowed";
            }

            tpl.onClick = onClick;
        }        
        
        public static implicit operator string(MemberIcoDComm value)
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