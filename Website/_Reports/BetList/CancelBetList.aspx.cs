// ---------------------------------------------------------------------------------
// <copyright file="CancelBetList.aspx.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Thai" date="2010.10.30">Create</change>
//     <change who="Marc.Bui" date="2013.02.28">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.Age.UI.Reports.BetList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using Fanex.Age.UI.Components;
    using Fanex.BetList.Age.BetListCmd;
    using Fanex.BetList.Age.BetListExcelBuilderExt;
    using Fanex.BetList.Age.BetListHTMLBuilderExt;
    using Fanex.BetList.Age.DTO;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using NPOI.SS.UserModel;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;

    public partial class CancelBetList : TplPage
    {
        #region Variables

        private int masterId = 0;
        private int agentId = 0;
        private int memberId = 0;

        private string option = string.Empty;
        private string title = string.Empty;

        private CancelBetList_Template tpl = new CancelBetList_Template();
        private IBetListCmd command = null;

        #endregion Variables

        public override void RenderContent(HtmlTextWriter writer)
        {
            #region INIT

            Dictionary<string, DataTable> results = View as Dictionary<string, DataTable>;
            UserTypes userType = UserTypes.Unknown;

            DataTable dtDeletedBets = results["dtDeletedBets"];
            IList<ITicket> tickets = new List<ITicket>();

            if (dtDeletedBets != null)
            {
                tickets = TicketTransformer.Transform(dtDeletedBets);
                dtDeletedBets.Dispose();
            }

            Show_HideComboBox(results, userType);
            DataTable betTransm = command.OutDataTable();
            List<ITicketData> ticketData = new List<ITicketData>();

            if (betTransm != null)
            {
                ticketData = TicketDataTransformer.Transform(betTransm);
                betTransm.Dispose();
            }

            int index = 0;
            bool isAdminCanViewVNIP = UserSession.Current.IsAdminCanViewVNIP;
            GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;
            ITicketHelper ticketHelper = new TicketHelper();

            #endregion INIT

            #region RENDER HTML

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();

            StringBuilder content = new StringBuilder();

            Fanex.BetList.Age.ExtTemplate.OpenBetList_Template betlistOpenTag = new Fanex.BetList.Age.ExtTemplate.OpenBetList_Template();
            Header1_Template betlistHeader = new Header1_Template();
            betlistHeader.Winloss.Hide();

            content.Append(betlistOpenTag);
            content.Append(betlistHeader);
            content.Append("<tbody>");

            foreach (ITicket ticket in tickets)
            {
                index++;
                ticketHelper.Index = index;
                builder.Append(string.Format("<tr id='{0}'>", ticket.TransId))
                       .AddNo(index, "<td class='w-order'>", "</td>")
                       .AddMember(ticket, "<td class='c'>", "</td>")
                       .AddTrans(ticket, "<td class='c'>", "</td>")
                       .AddChoice(ticket, ticketHelper, ticketData, "<td class='r bl_evt'><div>", "</div></td>", false)
                       .AddOdds(ticket, ticketData, "<td class='bl_underdog'>", "</td>", funcGetOddsTypeName)
                       .AddStake(ticket, "<td class='bl_underdog'>", "</td>")
                       .AddStatus(ticket, ticketHelper, ticketData, "<td class='c'>", "</td>", isAdminCanViewVNIP)
                       .Append("</tr>");
            }

            content.Append(builder.GetBetList());
            content.Append("</tbody></table>");

            tpl.content = content.ToString();

            writer.Write(tpl);

            #endregion RENDER HTML
        }

        private void Show_HideComboBox(Dictionary<string, DataTable> results, UserTypes userType)
        {
            //if (results["dtCombo1"] != null && results["dtCombo1"].Rows.Count > 0)
            //{
            //        tpl.Options.Master.Visible = false;
            //        tpl.Options.Agent.Visible = false;
            //        userType = UserTypes.Agent;

            //        // modify tables=> new datatable has 2 columns "custid" & "text" (text=username+betcount+Resources.Lang.bet)
            //        results["dtCombo1"] = HtmlGenerator.ModifyDataTable(results["dtCombo1"], string.Empty, "custid", "username", "betcount", Resources.Lang.bet);

            //        tpl.Options.Member.option_member = HtmlGenerator.ComboBox("memberId", memberId, results["dtCombo1"], "custid", "text", "selected_Option_member(this)");
            //}
            //else // has no item of cancelBet
            //{
            //    tpl.Options.Master.Visible = false;
            //    tpl.Options.Agent.Visible = false;
            //    tpl.Options.Member.Visible = false;
            //}
        }

        public override object InvokeData(Hashtable hash)
        {
            command = BetListFactory.CreateBetList("CancelBetList", CurrentUserSession.NetSessionId, null);

            if (command != null)
            {
                hash.Add("memberId", memberId);
                hash.Add("agentId", agentId);
                hash.Add("masterId", masterId);

                command.SetParams(hash);
                return command.Execute();
            }

            return null;
        }

        public override void ProcessRequest(ref Hashtable hash)
        {
            int.TryParse(Request["masterId"] + string.Empty, out masterId);
            int.TryParse(Request["agentId"] + string.Empty, out agentId);
            int.TryParse(Request["memberId"] + string.Empty, out memberId);
        }

        public override object ValidateRequest(Hashtable hash)
        {
            if (memberId < 0 || agentId < 0 || masterId < 0)
            {
                return new ErrorPage_Page(Resources.Lang.strNotAccessPage);
            }

            return null;
        }
    }
}