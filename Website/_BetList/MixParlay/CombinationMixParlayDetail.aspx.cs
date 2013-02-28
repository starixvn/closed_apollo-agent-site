// ---------------------------------------------------------------------------------
// <copyright file="CombinationMixParlayDetail.aspx.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2013.02.28">Apply CoreBetList</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Sunplus.Agent.UI._BetList.MixParlay
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using Fanex.Age.UI;
    using Fanex.BetList.Age.BetListCmd;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Fanex.Data.DNAide;

    public partial class CombinationMixParlayDetail_Page : TplPage
    {
        #region Variable
        protected const string EVEN_ROW_CLASS = Fanex.Age.UI.Common.RowBgEven;
        protected const string ODD_ROW_CLASS = Fanex.Age.UI.Common.RowBgOdd;

        long _refno = 0;
        decimal _outStanding = 0;
        long _transid = 0;
        long _betid = 0;
        int _custid = 0;

        #endregion

        public override void RenderContent(HtmlTextWriter writer)
        {
            CombinationMixParlayDetail_Template tpl = new CombinationMixParlayDetail_Template();

            DbResult combMixParlayData = View as DbResult;

            tpl.title = Resources.Lang.lblSystemParlay;

            if(combMixParlayData.ResultSet.Tables.Count>1)
            {
                _outStanding = Cast.AsDecimal(combMixParlayData.Outputs["outstanding"]);
                RenderReportContent(combMixParlayData.ResultSet.Tables[0],combMixParlayData.ResultSet.Tables[1], tpl.Content);
            }
            writer.Write(tpl);
        }

        public override object InvokeData(Hashtable hash)
        {
            DbResult dtResult = null;
            IBetListCmd command = new CombMixParlayBetListDetailCmd(CurrentUserSession.NetSessionId);
            if (command != null)
            {
                hash.Add("refno", _refno);
                hash.Add("transid", _transid);
                hash.Add("betid", _betid);
                hash.Add("custid", _custid);
                command.SetParams(hash);

                dtResult = command.Execute() as DbResult;
            }
            return dtResult;
        }

        public override void ProcessRequest(ref Hashtable hash)
        {
            string temp = Request["refno"];
            temp = temp.Replace("'", null);
            long.TryParse(temp, out _refno);

            _transid = Convert.ToInt64(Request["transid"]);
            _betid = Convert.ToInt64(Request["betid"]);
            _custid = Convert.ToInt32(Request["custid"]);
            //long.TryParse(Request["refno"], out _refno);            
            //winLostDate = Cast.AsDateTime(Request["winlostdate"]);
        }

        public override object ValidateRequest(Hashtable p)
        {
            return null;
        }

        private void RenderReportContent(DataTable dt, DataTable dtDetail, CombinationMixParlayDetail_Content_Block contentBlock)
        {                        
            CombinationMixParlayRow_Template template = new CombinationMixParlayRow_Template();
            CombinationMixParlayRow_RowData_Block rowBlock = new CombinationMixParlayRow_RowData_Block();
            CombinationMixParlayRow_Summary_Block sumBlock = new CombinationMixParlayRow_Summary_Block();
            template.RowData.Clear();
            template.SubSummary.Clear();

            int serial = 0;
            string even = string.Empty;
            decimal winlostSum = 0;
            decimal subWinlostSum = 0;
            string winlostdate = string.Empty;
            string winlostNextDate = string.Empty;

            int numRow = dt.Rows.Count;
            if(numRow>0)
            {
                #region details
                
                for (int i=0;i<numRow;i++)
                {                    
                    DataRow dr = dt.Rows[i];

                    rowBlock.stake = ConvertByBetType.Stake(dr["stake"]);
                    rowBlock.winlost = ConvertByBetType.Winloss(dr["winlost"]);
                    rowBlock.comm = ConvertByBetType.Comm(dr["playercomm"]);
                    winlostSum += Cast.AsDecimal(dr["winlost"]) + Cast.AsDecimal(dr["playercomm"]);

                    //status
                    string status = dr["status"].ToString().ToLower();

                    if (status.Equals(BetStatus.Running)) rowBlock.status = Resources.BetList.running;
                    else if (status.Equals(BetStatus.Reject)) rowBlock.status = Resources.BetList.reject;
                    else if (status.Equals(BetStatus.Won)) rowBlock.status = Resources.BetList.won;
                    else if (status.Equals(BetStatus.Lose)) rowBlock.status = Resources.BetList.lose;
                    else if (status.Equals(BetStatus.Draw)) rowBlock.status = Resources.BetList.draw;
                    else if (status.Equals(BetStatus.Waiting)) rowBlock.status = Resources.BetList.waiting;
                    else if (status.Equals(BetStatus.Void)) rowBlock.status = Resources.BetList.voided;
                    else if (status.Equals(BetStatus.Refund)) rowBlock.status = Resources.BetList.refund;
                    else rowBlock.status = string.Empty;

                    if (status.Equals(BetStatus.Lose))
                    {
                        rowBlock.odds = "-";
                        rowBlock.even = Resources.BetList.losingtickets;
                    }
                    else
                    {
                        //even
                        serial = Sunplus.Agent.Utilities.Cast.AsInt32(dr["serial"]);
                        DataRow[] rows = dtDetail.Select("serial=" + serial);
                        even = string.Empty;
                        foreach (DataRow row in rows)
                        {
                            even += CacheRef.GetTeamName(row["homeid"]) + " - vs - " + CacheRef.GetTeamName(row["awayid"]);
                            even += "<br/>";
                        }
                        rowBlock.even = even;
                        //end even         

                        if (Cast.AsDecimal(dr["modds"]) > 0)
                        {
                            rowBlock.odds = float.Parse(dr["modds"].ToString()).ToString("##0.###");
                        }
                        else
                        {
                            rowBlock.odds = ConvertByBetType.Odds(dr["modds"]);
                        }
                    }

                    winlostdate = Formatter.ShortDateFormat(dr["winlostdate"]);
                    if (i == 0)
                    {
                        IEnumerable<DataRow> row = dt.AsEnumerable().Where(t => t.Field<DateTime>("winlostdate") == Convert.ToDateTime(winlostdate));
                        int numRowSpan = row.CopyToDataTable().Rows.Count;
                        if (numRowSpan > 1)
                            rowBlock.WinlostDate.rowSpan = "rowspan='" + numRowSpan.ToString() + "'";
                        else
                            rowBlock.WinlostDate.rowSpan = string.Empty;
                        rowBlock.WinlostDate.date = winlostdate;
                        rowBlock.WinlostDate.Show();
                    }
                    else
                    {
                        rowBlock.WinlostDate.Hide();
                    }                   

                    subWinlostSum += Cast.AsDecimal(dr["winlost"]) + Cast.AsDecimal(dr["playercomm"]);
                    rowBlock.ColorClass = i % 2 == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
                    template.RowData.Append(rowBlock);

                    //Get Sub Total by Day
                    if (i + 1 == numRow)
                    {
                        winlostNextDate = string.Empty;
                    }
                    else
                    {
                        DataRow drTemp = dt.Rows[i + 1];
                        winlostNextDate = Formatter.ShortDateFormat(drTemp["winlostdate"]);
                    }

                    GetSubTotalByDay(template, ref subWinlostSum, winlostdate, winlostNextDate);

                }
                #endregion

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
        #endregion

    }

}