/// Mix parlay inherit from Handicap
namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Data;
    using System.Text;
    using Sunplus.Agent.Model.BL.BetList;
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.UI.BetList.Factories;

    /// <summary>
    /// Mix Parlay
    /// </summary>
    public class BetType9 : BetType1
    {
        public delegate DataTable MixParlayDelegate(long transid, string language);

        public override void Assign(int index, System.Data.DataRow row)
        {
            base.Assign(index, row);

            // modify    
            this.Status.StatusResult.refNo_MixParlay = GetValueFromDataRow(row, "refno", 0).ToString();

            TransTime.Show();
            TransTimeHyperLink.Hide();
        }

        public override object GetChoice(Excel.BetTypeExcelData excelData)
        {
            int betTypeId = Convert.ToInt32(excelData.DataRow["bettype"]);
            string betteam = Convert.ToString(excelData.DataRow["betteam"]);
            string betTypeName = CacheRef.GetBettype(betTypeId);

            MixParlayDelegate mixParlayDelegate = new MixParlayDelegate(BetListBO.GetMixParlayDetail);
            DataTable betListData = mixParlayDelegate(long.Parse(TransTimeHyperLink.refNo), CurrentUserSession.LangKey);

            string mixParlay = betTypeName + "\n\n";
            excelData.RtfTextRender.AddText(mixParlay);
            return ExcelFactory.RenderMixParlay(CurrentBetType, ref betListData, excelData);
        }

        public override void AssignEvent(System.Data.DataRow row)
        {
            int betTypeId = Convert.ToInt32(row["bettype"]);
            
            string betTypeName = CacheRef.GetBettype(betTypeId);
            
            if (this.ExportExcel)
            {
                MixParlayDelegate mixParlayDelegate = new MixParlayDelegate(BetListBO.GetMixParlayDetail);

                DataTable betListData = mixParlayDelegate(long.Parse(TransTimeHyperLink.refNo), CurrentUserSession.LangKey);

                string mixParlay = betTypeName + "<br/></br/><br/>";
                StringBuilder mixParlayContent = RenderingFactory.RenderMixParlay(CurrentBetType, ref betListData, this.ExportExcel);
                this.Event.SetValue(mixParlay + mixParlayContent.ToString());
            }
            else
            {
                string mixParlay = string.Empty;

                // Only use for SmartPhone
                if ((CurrentBetType & BetListType.SmartPhone) == BetListType.SmartPhone)
                {
                    mixParlay = "<a data-rel='dialog' class='ui-link' href='MixParlay.aspx?transid=" + TransTimeHyperLink.refNo + "'>" + betTypeName + "</a>";                                     
                }
                else
                {
                    mixParlay = string.Join(null, new string[] { "<a id=\"hidden0\" href=\"javascript:showMP(&quot;" + TransTimeHyperLink.refNo + "&quot;);\">" + betTypeName + "</a>", "<br/><br/>", "<div id=\"divEvent_" + TransTimeHyperLink.refNo + "\" style=\"display: none\" ></div>" });
                }

                this.Event.SetValue(mixParlay);
            }                                    
        }
    }
}
