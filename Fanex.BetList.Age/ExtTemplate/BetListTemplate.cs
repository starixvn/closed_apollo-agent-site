// --------------------------------------------------------------------------
// <copyright file="BetListTemplate.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.BetList.Age.ExtTemplate
{
    using System.Web.UI;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListTemplate : BetList_Template
    {
        public void ToString(HtmlTextWriter writer)
        {
            if (!Visible)
            {
                return;
            }

            if (RenderChild)
            {
                writer.Write(s0);
                writer.Write(s1);
                writer.Write(s2);
                writer.Write(s3);
                writer.Write(s4);
                writer.Write(title);
                writer.Write(s5);
                writer.Write(s6);
                writer.Write(s7);
                writer.Write(content);
                writer.Write(s8);
                writer.Write(s9);
                writer.Write(s10);
                writer.Write(s11);
                writer.Write(s12);
                writer.Write(s13);
                writer.Write(s14);
            }
        }
    }
}
