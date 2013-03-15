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
    using Fanex.Age.UI;
    using IcoTransferMulti_Control;
    using TplCore.Element;

    public class IcoTransferMulti : IComponent
    {
        private IcoTransferMulti_Control_Template tpl = new IcoTransferMulti_Control_Template();

        public IcoTransferMulti(IElement parent)
        {
            if (parent != null)
            {
                parent.RegisterComponent("IcoTransferMulti_Control", this);
            }

            this.tpl.ButtonText = Resources.Lang.transfer;
        }

        public static implicit operator string(IcoTransferMulti value)
        {
            return value.ToString();
        }

        public void HideLinkedResources()
        {
            this.tpl.ExternalResources = null;
        }

        public override string ToString()
        {
            return this.tpl.ToString();
        }
    }
}