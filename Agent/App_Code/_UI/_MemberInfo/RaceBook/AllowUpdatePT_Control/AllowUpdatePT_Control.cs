using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Fanex.Age.UI;
using TplCore.Element;

/// <summary>
/// Summary description for AllowUpdatePT_Control
/// </summary>
/// 
namespace Sunplus.Agent.UI._MemberInfo.RaceBook
{
    using AllowUpdatePT_Control;

    public class AllowUpdatePT : IComponent
    {
        AllowUpdatePT_Control_Template tpl = new AllowUpdatePT_Control_Template();
        public AllowUpdatePT(IElement parent, string chkId, string sporttype)
        {
            parent.RegisterComponent("AllowUpdatePT", this);

            tpl.sporttype = sporttype;
            tpl.chkUpdateId = chkId;
        }

        public static implicit operator string(AllowUpdatePT value)
        {
            return value.ToString();
        }


        public void HideLinkedResources()
        {
            
        }

        public override string ToString()
        {
            return tpl.ToString();
        }
    }
}