// --------------------------------------------------------------------------
// <copyright file="ConfirmAgentAutoPT.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Andy.Nguyen" date="2012.4.13">Format styles</change>
// </history>

namespace Fanex.Age.UI.LiveCasino.Master
{
    using System.Web.UI;

    /// <summary>
    /// Confirm agent auto pt on member
    /// </summary>
    public partial class ConfirmAgentAutoPT_Page : TplPage
    {
        /// <summary>
        /// Render UI
        /// </summary>
        /// <param name="writer">Request params</param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            var confirm = CasinoUI.ConfirmEditAutoPT;

            confirm.information = Resources.Lang.lblConfirmEditAgent;

            writer.Write(confirm);
        }

        /// <summary>
        /// Invoke data
        /// </summary>
        /// <param name="p">Request params</param>
        /// <returns>Null value</returns>
        public override object InvokeData(System.Collections.Hashtable p)
        {
            return null;
        }

        /// <summary>
        /// Process request
        /// </summary>
        /// <param name="p">Request params</param>
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {            
        }

        /// <summary>
        /// Validate request
        /// </summary>
        /// <param name="p">Request params</param>
        /// <returns>Null value</returns>
        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }
    }
}