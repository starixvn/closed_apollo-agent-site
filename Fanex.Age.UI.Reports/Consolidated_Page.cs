using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.Definitions;
using Fanex.Age.UI.Reports.Consolidated;

/// <summary>
/// Summary description for Consolidated_Page
/// </summary>
/// 
namespace Fanex.Age.UI.Reports
{
    public abstract class Consolidated_Page : WinlossReports_Page
    {
        public Consolidated_Page()
        {

        }

        public static string GetSportName(object sportId)
        {
            switch (Convert.ToInt32(sportId))
            {
                case 0:
                    return Resources.Lang.sportBook;
                case 161:
                    return Resources.Lang.numbergame;
                case 162:
                    return Resources.Lang.LiveCasino;
                case 180:
                    return Resources.Lang.VirtualSports;
                case 251:
                    return Resources.Lang.casino;
                case 151:
                    return Resources.Lang.Racebook;
                case 252:
                    return Resources.Lang.bingo;
                case 253:
                    return Resources.Lang.P2P;
            }
            return string.Empty;
        }

        public static string FormatDateTime(string DateTimeString)
        {
            int year = Convert.ToInt32(DateTimeString.Substring(0, 4));
            int month = Convert.ToInt32(DateTimeString.Substring(4, 2));

            DateTime dt = new DateTime(year, month, 1);
            return dt.Month + "/" + dt.Year;
        }

        public static string FormatDateTimeforDaily(string DateTimeString)
        {
            int year = Convert.ToInt32(DateTimeString.Substring(0, 4));
            int month = Convert.ToInt32(DateTimeString.Substring(4, 2));
            int day = Convert.ToInt32(DateTimeString.Substring(6, 2));
            DateTime dt = new DateTime(year, month, day);
            return dt.Month + "/" + dt.Day + "/" + dt.Year;
        }

        public static void RenderGroupContent(int currencyId, DataRow[] Rows, Consolidated_ExcelContents_Block excelBlock, bool isBySport, bool isByDaily)
        {
            Consolidated_ExcelContents_Content_Block contentBlock = new Consolidated_ExcelContents_Content_Block();
            int numRow = Rows.Count();
            bool flag = false;
            string prefix = string.Empty;
            switch (currencyId)
            {
                case -2:
                    prefix = "RM";
                    break;
                case -3:
                    prefix = "USD";
                    break;
                default:
                    prefix = string.Empty;
                    break;
            }
            foreach (DataRow row in Rows)
            {
                contentBlock.ColorClass = (numRow % 2) == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
                numRow--;
                if (!flag)
                {
                    contentBlock.RemoveTd.rowspan = Rows.Count().ToString();
                    flag = true;
                    if (!isBySport)
                        if (!isByDaily)
                            contentBlock.RemoveTd.option = FormatDateTime(row["breakdown"].ToString());
                        else
                            contentBlock.RemoveTd.option = FormatDateTimeforDaily(row["breakdown"].ToString());

                    else
                        contentBlock.RemoveTd.option = GetSportName(row["breakdown"].ToString());
                }
                else
                    contentBlock.RemoveTd = null;

                contentBlock.turnover = Formater.DecFormat(row[prefix + "turnover"], 2);
                contentBlock.username = row["username"] as string;
                contentBlock.currency = RemotingRef.RefData.GetExchangeData(Convert.ToInt32(row["currency"])).Currency;

                contentBlock.mem_winloss = Formater.DecFormat(row[prefix + "member_winlost"], 2);
                contentBlock.mem_comm = Formater.DecFormat(row[prefix + "member_comm"], 2);
                contentBlock.mem_total = Formater.DecFormat(row[prefix + "member_total"], 2);

                contentBlock.owner_winloss = Formater.DecFormat(row[prefix + "owner_winlost"], 2);
                contentBlock.owner_comm = Formater.DecFormat(row[prefix + "owner_comm"], 2);
                contentBlock.owner_total = Formater.DecFormat(row[prefix + "owner_total"], 2);

                contentBlock.company = Formater.DecFormat(row[prefix + "company"], 2);
                contentBlock.betcount = Formater.DecFormat(row["betcount"], 0);

                excelBlock.Content.Append(contentBlock);
            }
        }



        public static void RenderTotalBaseOnCurrency(DataTable dtb, Consolidated_Template tpl)
        {
            Consolidated_ExcelContents_ContentTotalBaseCurrency_Block tplBlock = new Consolidated_ExcelContents_ContentTotalBaseCurrency_Block();
            tplBlock.basecurrency = Resources.Lang.basecurrency + " " + RemotingRef.RefData.GetExchangeData(2).Currency;
            tplBlock.turnover = Formater.DecFormat(dtb.Compute("SUM(rmturnover)", ""), 2);
            tplBlock.mem_winloss = Formater.DecFormat(dtb.Compute("SUM(rmmember_winlost)", ""), 2);
            tplBlock.mem_comm = Formater.DecFormat(dtb.Compute("SUM(rmmember_comm)", ""), 2);
            tplBlock.mem_total = Formater.DecFormat(dtb.Compute("SUM(rmmember_total)", ""), 2);

            tplBlock.owner_winloss = Formater.DecFormat(dtb.Compute("SUM(rmowner_winlost)", ""), 2);
            tplBlock.owner_comm = Formater.DecFormat(dtb.Compute("SUM(rmowner_comm)", ""), 2);
            tplBlock.owner_total = Formater.DecFormat(dtb.Compute("SUM(rmowner_total)", ""), 2);

            tplBlock.company = Formater.DecFormat(dtb.Compute("SUM(rmcompany)", ""), 2);
            tplBlock.betcount = Formater.DecFormat(dtb.Compute("SUM(betcount)", ""), 0);
            tpl.ExcelContents.ContentTotalBaseCurrency.Append(tplBlock);

            tplBlock.basecurrency = Resources.Lang.basecurrency + " " + RemotingRef.RefData.GetExchangeData(3).Currency;
            tplBlock.turnover = Formater.DecFormat(dtb.Compute("SUM(usdturnover)", ""), 2);
            tplBlock.mem_winloss = Formater.DecFormat(dtb.Compute("SUM(usdmember_winlost)", ""), 2);
            tplBlock.mem_comm = Formater.DecFormat(dtb.Compute("SUM(usdmember_comm)", ""), 2);
            tplBlock.mem_total = Formater.DecFormat(dtb.Compute("SUM(usdmember_total)", ""), 2);

            tplBlock.owner_winloss = Formater.DecFormat(dtb.Compute("SUM(usdowner_winlost)", ""), 2);
            tplBlock.owner_comm = Formater.DecFormat(dtb.Compute("SUM(usdowner_comm)", ""), 2);
            tplBlock.owner_total = Formater.DecFormat(dtb.Compute("SUM(usdowner_total)", ""), 2);

            tplBlock.company = Formater.DecFormat(dtb.Compute("SUM(usdcompany)", ""), 2);
            tplBlock.betcount = Formater.DecFormat(dtb.Compute("SUM(betcount)", ""), 0);
            tpl.ExcelContents.ContentTotalBaseCurrency.Append(tplBlock);

        }

        public static void RenderTotalCurrencyBaseOnCurrency(DataTable dtb, int currencyId, Consolidated_Template tpl)
        {
            string prefix = string.Empty;
            switch (currencyId)
            {
                case -2:
                    prefix = "RM";
                    break;
                case -3:
                    prefix = "USD";
                    break;
            }

            Consolidated_ExcelContents_ContentTotalCurrency_Block tplBlock = new Consolidated_ExcelContents_ContentTotalCurrency_Block();
            var query = from rawdata in dtb.AsEnumerable()
                        group rawdata by rawdata.Field<byte>("currency") into grp
                        orderby grp.Key
                        select new
                        {
                            currency = grp.Key,
                            totalTurnOver = grp.Sum(r => r.Field<double>(prefix + "turnover")),

                            totalMemberWinlost = grp.Sum(r => r.Field<double>(prefix + "member_winlost")),
                            totalMemberComm = grp.Sum(r => r.Field<double>(prefix + "member_comm")),
                            totalMemberTotal = grp.Sum(r => r.Field<double>(prefix + "member_total")),

                            totalOwnerWinlost = grp.Sum(r => r.Field<double>(prefix + "owner_winlost")),
                            totalOwnerComm = grp.Sum(r => r.Field<double>(prefix + "owner_comm")),
                            totalOwnerTotal = grp.Sum(r => r.Field<double>(prefix + "owner_total")),

                            totalCompany = grp.Sum(r => r.Field<double>(prefix + "company")),
                            totalBetCount = grp.Sum(r => r.Field<int>("betcount")),

                        };

            tpl.ExcelContents.ContentTotalCurrency.Clear();
            bool flag = false;
            foreach (var grp in query)
            {
                if (!flag)
                {
                    tplBlock.RemoveTd.rowspan = query.ToList().Count.ToString();
                    flag = true;
                }
                else
                    tplBlock.RemoveTd = null;

                tplBlock.currency = RemotingRef.RefData.GetExchangeData(Convert.ToInt32(grp.currency)).Currency;
                tplBlock.turnover = Formater.DecFormat(grp.totalTurnOver, 2);

                tplBlock.mem_winloss = Formater.DecFormat(grp.totalMemberWinlost, 2);
                tplBlock.mem_comm = Formater.DecFormat(grp.totalMemberComm, 2);
                tplBlock.mem_total = Formater.DecFormat(grp.totalMemberTotal, 2);

                tplBlock.owner_winloss = Formater.DecFormat(grp.totalOwnerWinlost, 2);
                tplBlock.owner_comm = Formater.DecFormat(grp.totalOwnerComm, 2);
                tplBlock.owner_total = Formater.DecFormat(grp.totalOwnerTotal, 2);

                tplBlock.company = Formater.DecFormat(grp.totalCompany, 2);
                tplBlock.betcount = Formater.DecFormat(grp.totalBetCount, 0);

                tpl.ExcelContents.ContentTotalCurrency.Append(tplBlock);

            }
        }

        public static void RenderTotalCurrency(DataTable dtb, Consolidated_Template tpl)
        {

            Consolidated_ExcelContents_ContentTotalCurrency_Block tplBlock = new Consolidated_ExcelContents_ContentTotalCurrency_Block();
            var query = from rawdata in dtb.AsEnumerable()
                        group rawdata by rawdata.Field<byte>("currency") into grp
                        orderby grp.Key
                        select new
                        {
                            currency = grp.Key,
                            totalTurnOver = grp.Sum(r => r.Field<decimal>("turnover")),

                            totalMemberWinlost = grp.Sum(r => r.Field<decimal>("member_winlost")),
                            totalMemberComm = grp.Sum(r => r.Field<decimal>("member_comm")),
                            totalMemberTotal = grp.Sum(r => r.Field<decimal>("member_total")),

                            totalOwnerWinlost = grp.Sum(r => r.Field<decimal>("owner_winlost")),
                            totalOwnerComm = grp.Sum(r => r.Field<decimal>("owner_comm")),
                            totalOwnerTotal = grp.Sum(r => r.Field<decimal>("owner_total")),

                            totalCompany = grp.Sum(r => r.Field<decimal>("company")),
                            totalBetCount = grp.Sum(r => r.Field<int>("betcount")),

                        };

            tpl.ExcelContents.ContentTotalCurrency.Clear();
            bool flag = false;
            foreach (var grp in query)
            {
                if (!flag)
                {
                    tplBlock.RemoveTd.rowspan = query.ToList().Count.ToString();
                    flag = true;
                }
                else
                    tplBlock.RemoveTd = null;

                tplBlock.currency = RemotingRef.RefData.GetExchangeData(Convert.ToInt32(grp.currency)).Currency;
                tplBlock.turnover = Formater.DecFormat(grp.totalTurnOver, 2);

                tplBlock.mem_winloss = Formater.DecFormat(grp.totalMemberWinlost, 2);
                tplBlock.mem_comm = Formater.DecFormat(grp.totalMemberComm, 2);
                tplBlock.mem_total = Formater.DecFormat(grp.totalMemberTotal, 2);

                tplBlock.owner_winloss = Formater.DecFormat(grp.totalOwnerWinlost, 2);
                tplBlock.owner_comm = Formater.DecFormat(grp.totalOwnerComm, 2);
                tplBlock.owner_total = Formater.DecFormat(grp.totalOwnerTotal, 2);

                tplBlock.company = Formater.DecFormat(grp.totalCompany, 2);
                tplBlock.betcount = Formater.DecFormat(grp.totalBetCount, 0);

                tpl.ExcelContents.ContentTotalCurrency.Append(tplBlock);

            }
        }

    }
}