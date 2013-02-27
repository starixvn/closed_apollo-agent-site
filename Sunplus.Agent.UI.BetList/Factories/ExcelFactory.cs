namespace Sunplus.Agent.UI.BetList.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Age.Utility.NPOI;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.BL.Forecast;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.UI.BetList.BetTypes;
    using Sunplus.Agent.UI.BetList.Excel;

    public class ExcelFactory
    {
        public static void Render(BetListType betListType, ref DataTable betListData, UserTypes userType, HSSFWorkbook workbook, string title, DataTable betTransm)
        {
            var betlistWorkbook = new BetListWorkbook(workbook, title);

            // Append header
            HeaderRender(betListType, userType, betlistWorkbook);

            DataRowCollection rows = betListData.Rows;
            int c = rows.Count;
            
            // Append content
            ContentRender(betListType, userType, betlistWorkbook, rows, c, betTransm);

            // Append footer
            FooterRender(betListType, betListData, userType, betlistWorkbook);

            betlistWorkbook.RemoveTemplateSheet();
        }

        public static object RenderMixParlay(BetListType betListType, ref DataTable betListData, Excel.BetTypeExcelData excelData)
        {
            DataRowCollection rows = betListData.Rows;
            int c = rows.Count;
            var result = new List<IRichTextString>();

            // Render row collection
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    DataRow row = rows[i];
                    IBetItem item = null;

                    if (row.Table.Columns.Contains("bettype"))
                    {
                        if (row["bettype"] == null)
                        {
                            throw new NullReferenceException("row[\"bettype\"] is not reference to an object.");
                        }

                        int betType = Convert.ToInt32(row["bettype"]);
                        item = BetItemFactory.CreateBetItem(betType);
                    }
                    else
                    {
                        item = BetItemFactory.CreateBetItem();
                    }

                    item.ExportExcel = true;
                    (item as BetTypeBase).AssignEvent(row);
                    excelData.DataRow = row;
                    var value = (IRichTextString)(item as BetTypeBase).GetChoice(excelData, true);
                    result.Add(value);
                }
            }
            else
            {
                result.Add(excelData.RtfTextRender.Render());
                excelData.RtfTextRender.Clear();
            }

            return result;
        }

        private static void HeaderRender(BetListType betListType, UserTypes userType, BetListWorkbook workbook)
        {
            if (string.IsNullOrEmpty(workbook.Title))
            {
                workbook.ResultSheet.RemoveRow(0);
                workbook.ResultSheet.ShiftRows(1, 2, -1);
                for (int i = workbook.ResultSheet.NumMergedRegions - 1; i >= 0; i--)
                {
                    workbook.ResultSheet.RemoveMergedRegion(i);
                }
            }
            else
            {
                workbook.ResultSheet.GetRow(workbook.TitleHeaderRowIndex).GetCell(0).SetCellValue(workbook.Title);
            }

            IBetItem header = BetItemFactory.CreateHeader(betListType);
            header.CustomizeColumns(userType, betListType, workbook);
        }

        private static void ContentRender(BetListType betListType, UserTypes userType, BetListWorkbook workbook, DataRowCollection rows, int c, DataTable racingData)
        {
            bool isAdminCanViewVNIP = UserSession.Current.IsAdminCanViewVNIP;
            ForecastBO bo = new ForecastBO(UserSession.Current.NetSessionId);
            List<int> scoreMapBetTypes = bo.GetListBetTypeScoreMap();

            // Render row collection
            for (int i = 0; i < c; i++)
            {
                DataRow row = rows[i];
                IBetItem item = null;

                if (row.Table.Columns.Contains("bettype"))
                {
                    if (row["bettype"] == null)
                    {
                        throw new NullReferenceException("row[\"bettype\"] is not reference to an object.");
                    }

                    int betType = Convert.ToInt32(row["bettype"]);
                    item = BetItemFactory.CreateBetItem(betType);
                }
                else
                {
                    item = BetItemFactory.CreateBetItem();
                }

                item.ScoreMapBetTypes = scoreMapBetTypes;
                item.IsAdminCanViewVNIP = isAdminCanViewVNIP;

                item.CustomizeColumns(userType, betListType, workbook);
                item.ExportExcel = true;
                item.OutDataTable = racingData;
                item.Assign(i + 1, row, workbook);
            }
        }

        private static void FooterRender(BetListType betListType, DataTable betListData, UserTypes userType, BetListWorkbook workbook)
        {
            IBetItem footer = null;
            DataRow totalRow = null;

            if (betListData.Rows.Count == 0)
            {
                // Append no information row            
                footer = BetItemFactory.CreateFooter(FooterType.None, betListType);
            }
            else 
            {
                // Add total row only in case Winlost bet list type        
                if ((betListType & BetListType.Winlost) == BetListType.Winlost)
                {
                    totalRow = betListData.NewRow();

                    if (betListData.Columns.Contains("winlost") && betListData.Columns.Contains("playercomm"))
                    {
                        totalRow["winlost"] = betListData.Compute("sum(winlost)", "status <> 'running'");
                        totalRow["playercomm"] = betListData.Compute("sum(playercomm)", "status <> 'running'");
                    }

                    // Append total row            
                    if (totalRow["winlost"].ToString() != string.Empty && totalRow["playercomm"].ToString() != string.Empty)
                    {
                        betListData.Rows.Add(totalRow);
                        footer = BetItemFactory.CreateFooter(FooterType.Total, betListType);
                    }
                }
            }

            if (footer != null)
            {
                footer.CustomizeColumns(userType, betListType);
                if (totalRow != null)
                {
                    footer.Assign(betListData.Rows.Count, totalRow, workbook);
                }
            }
        }        
    }
}