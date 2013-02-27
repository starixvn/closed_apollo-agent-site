namespace Fanex.Age.Excel.Reports.Result
{
    using Fanex.Age.Utility.NPOI;
    using NPOI.SS.UserModel;
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.Utilities;

    public abstract class BaseResultReport : TemplateReport
    {
        public BaseResultReport(string filePath)
            : base(filePath)
        {
        }

        public virtual void WriteTitle(ISheet resultSheet, ref int rowId, ref int colId, BaseResultReportBuilder builder)
        {
            string title1 = string.Empty;
            if (builder.BetType == (int)Sunplus.Agent.Model.BL.Definitions.BetType.Mix_Parlay)
            {
                title1 = Resources.Lang.lblMixParlay;
            }
            else if (builder.BetType == (int)Sunplus.Agent.Model.BL.Definitions.BetType.Combine_Mix_Parlay)
            {
                title1 = Resources.Lang.lblSystemParlay;
            }
            else
            {
                if (builder.IsOutright)
                {
                    title1 = string.Format(
                        "{0} - {1} -> {2}",
                        Resources.Lang.lblOutrightResult.ToUpper(),
                        Formater.ShortDateFormat(builder.FromDate),
                        Formater.ShortDateFormat(builder.ToDate));
                }
                else
                {
                    title1 = string.Format(
                        "{0} - {1}",
                        Resources.Lang.result.ToUpper(),
                        Formater.ShortDateFormat(builder.FromDate));
                }
            }

            resultSheet.GetCell(rowId, colId).SetCellValue(title1);
            rowId++;

            string title2 = CacheRef.GetSportName(builder.SportTypeId) + " - ";
            if (builder.LeagueId == 0)
            {
                title2 += "Show All Leagues";
            }
            else
            {
                title2 += CacheRef.GetLeagueName(builder.LeagueId);
            }

            resultSheet.GetCell(rowId, colId).SetCellValue(title2);
            rowId++;
        }
    }
}