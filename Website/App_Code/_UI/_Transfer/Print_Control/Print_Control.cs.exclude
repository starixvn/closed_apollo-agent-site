// --------------------------------------------------------------------------
// <copyright file="InfoMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2012.05.31">Improve performance, check StypeCop</change>
// </history>

namespace Sunplus.Agent.UI._Transfer.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Print_Control;
    using TplCore.Element;

    public class PrintControl : IComponent
    {
        private Print_Control_Template tpl = new Print_Control_Template();

        public PrintControl(IElement parent, string onClick)
        {
            if (parent != null)
            {
                parent.RegisterComponent("Print_Control", this);
            }

            this.tpl.onClick = onClick;
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
            return this.tpl.ToString();
        }
    }
}