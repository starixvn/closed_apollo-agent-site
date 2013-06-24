// ---------------------------------------------------------------------------------
// <copyright file="CombinationMixParlayDetail.aspx.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Kaden.Trinh" date="2013.05.02">Apply CoreBetList</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Sunplus.Agent.UI._BetList.MixParlay
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Web.UI;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Services;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Services;
    using Fanex.Age.UI;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Apollo.Age.WebApp;
    using Apollo.Age.WebApp._BetList.MixParlay;

    public partial class CombinationMixParlayDetail_Page : TplPage
    {
        #region Variable

        long _refno = 0;
        decimal _outStanding = 0;
        long _transid = 0;
        long _betid = 0;
        int _custid = 0;

        #endregion Variable

        public override void RenderContent(HtmlTextWriter writer)
        {
            CombinationMixParlayDetail_Template tpl = new CombinationMixParlayDetail_Template();

            var combMixParlayData = View as SystemParlayData;

            tpl.title = Apollo.Age.Resources.Lang.lblSystemParlay;

            _outStanding = combMixParlayData.Outstanding;
            RenderReportContent(combMixParlayData, tpl.Content);

            writer.Write(tpl);
        }

        public override object InvokeData(Hashtable hash)
        {
            var reportService = new ReportService();
            var betListService = new BetListService();
            var criteria = new SystemParlayDetailCriteria
            {
                RefNo = _refno,
                CustId = _custid,
                MoveBetsDay = reportService.MoveBetsDay(),
                BalanceUpDay = reportService.GetBalanceUpDay(),
            };
            return betListService.GetCombinationMixParlayDetail(criteria);
        }

        public override void ProcessRequest(ref Hashtable hash)
        {
            string temp = Request["refno"];
            long.TryParse(temp, out _refno);

            _transid = Convert.ToInt64(Request["transid"]);
            _betid = Convert.ToInt64(Request["betid"]);
            _custid = Convert.ToInt32(Request["custid"]);
        }

        public override object ValidateRequest(Hashtable p)
        {
            return null;
        }

        private void RenderReportContent(SystemParlayData systemParlay, CombinationMixParlayDetail_Content_Block contentBlock)
        {
            CombinationMixParlayRow_Template template = new CombinationMixParlayRow_Template();
            CombinationMixParlayRow_RowData_Block rowBlock = new CombinationMixParlayRow_RowData_Block();
            CombinationMixParlayRow_Summary_Block sumBlock = new CombinationMixParlayRow_Summary_Block();
            template.RowData.Clear();
            template.SubSummary.Clear();

            long serial = 0;
            string even = string.Empty;
            decimal winlostSum = 0;
            decimal subWinlostSum = 0;
            string winlostdate = string.Empty;
            string winlostNextDate = string.Empty;
            var ticketList = systemParlay.TicketList;
            int numRow = ticketList.Count;
            var i = 0;
            if (numRow > 0)
            {
                #region details

                foreach (var ticket in ticketList)
                {
                    rowBlock.stake = ConvertByBetType.Stake(ticket.Stake);
                    rowBlock.winlost = ConvertByBetType.Winloss(ticket.WinLost);
                    rowBlock.comm = ConvertByBetType.Comm(ticket.PlayerComm);
                    winlostSum += ticket.WinLost + ticket.PlayerComm;

                    // status
                    string status = ticket.Status.ToLower();

                    if (status.Equals(BetStatus.Running)) rowBlock.status = Apollo.Age.Resources.BetList.running;
                    else if (status.Equals(BetStatus.Reject)) rowBlock.status = Apollo.Age.Resources.BetList.reject;
                    else if (status.Equals(BetStatus.Won)) rowBlock.status = Apollo.Age.Resources.BetList.won;
                    else if (status.Equals(BetStatus.Lose)) rowBlock.status = Apollo.Age.Resources.BetList.lose;
                    else if (status.Equals(BetStatus.Draw)) rowBlock.status = Apollo.Age.Resources.BetList.draw;
                    else if (status.Equals(BetStatus.Waiting)) rowBlock.status = Apollo.Age.Resources.BetList.waiting;
                    else if (status.Equals(BetStatus.Void)) rowBlock.status = Apollo.Age.Resources.BetList.voided;
                    else if (status.Equals(BetStatus.Refund)) rowBlock.status = Apollo.Age.Resources.BetList.refund;
                    else rowBlock.status = string.Empty;

                    if (status.Equals(BetStatus.Lose))
                    {
                        rowBlock.odds = "-";
                        rowBlock.even = Apollo.Age.Resources.BetList.losingtickets;
                    }
                    else
                    {
                        // even
                        serial = ticket.Serial;
                        var serials = systemParlay.SerialList.Where(s => s.Serial == serial);
                        even = string.Empty;
                        foreach (var item in serials)
                        {
                            even += CacheRef.GetTeamName(item.HomeId) + " - vs - " + CacheRef.GetTeamName(item.AwayId);
                            even += "<br/>";
                        }
                        rowBlock.even = even;
                        // end even

                        if (ticket.MOdds > 0)
                        {
                            rowBlock.odds = ticket.MOdds.ToString("##0.###");
                        }
                        else
                        {
                            rowBlock.odds = ConvertByBetType.Odds(ticket.MOdds);
                        }
                    }

                    if (winlostdate != ticket.WinLostDate.ToString("yyyy-mm-dd"))
                    {
                        var row = ticketList.Where(s => s.WinLostDate == ticket.WinLostDate);

                        int numRowSpan = row.Count();
                        if (numRowSpan > 1)
                        {
                            rowBlock.WinlostDate.rowSpan = "rowspan='" + numRowSpan.ToString() + "'";
                        }
                        else
                        {
                            rowBlock.WinlostDate.rowSpan = string.Empty;
                        }
                        rowBlock.WinlostDate.date = ticket.WinLostDate.ToString("yyyy-mm-dd"); ;
                        rowBlock.WinlostDate.Show();
                    }
                    else
                    {
                        rowBlock.WinlostDate.Hide();
                    }

                    subWinlostSum += ticket.WinLost + ticket.PlayerComm;
                    template.RowData.Append(rowBlock);
                    winlostdate = ticket.WinLostDate.ToString("yyyy-mm-dd");

                    //Get Sub Total by Day
                    if (i + 1 == numRow)
                    {
                        winlostNextDate = string.Empty;
                    }
                    else
                    {
                        var drTemp = ticketList[i + 1];
                        winlostNextDate = drTemp.WinLostDate.ToString("yyyy-mm-dd");
                    }

                    this.GetSubTotalByDay(template, ref subWinlostSum, ticket.WinLostDate.ToString("yyyy-mm-dd"), winlostNextDate);
                    i++;
                }

                #endregion details

                template.Summary.total = ConvertByBetType.Winloss(winlostSum);
            }
            else
            {
                template.RowData.Visible = false;
                template.Summary.Visible = false;
                template.SubSummary.Visible = false;
            }

            template.OutStanding.outstanding = ConvertByBetType.Stake(_outStanding);
            contentBlock.Append(template);
        }

        #region Get SubTotal

        private void GetSubTotalByDay(CombinationMixParlayRow_Template template, ref decimal subTotal, string currentDate, string nextDate)
        {
            if (currentDate != nextDate)
            {
                CombinationMixParlayRow_SubSummary_Block subTotalBlock = new CombinationMixParlayRow_SubSummary_Block();
                subTotalBlock.subTotal = ConvertByBetType.Winloss(subTotal);
                template.RowData.Append(subTotalBlock);
                subTotal = 0;
            }
            else
            {
                template.SubSummary.Visible = false;
            }
        }

        #endregion Get SubTotal
    }
}