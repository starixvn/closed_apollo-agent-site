/// Combination Mix parlay inherit from Handicap
namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web;
    using Sunplus.Agent.Model.BL.BetList;
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.UI.BetList.Factories;
    using Sunplus.Agent.UI.BetList.Utils;
    using Sunplus.Agent.Utilities;

    /// <summary>
    /// Combination Mix Parlay
    /// </summary>
    public class BetType29 : BetType1
    {
        public delegate DataTable MixParlayDelegate(long transid, string language);

        public override void Assign(int index, System.Data.DataRow row)
        {
            base.Assign(index, row);

            TransTime.Show();
            TransTimeHyperLink.Hide();

            this.Status.StatusResult.refNo_MixParlay = GetValueFromDataRow(row, "refno", 0).ToString();            

            //odds
            string _status = base.Status.result.ToLower();
            if (_status.Equals(BetStatus.Running))
            {
                base.odds = "-";                
            }        
            else
            {
                base.odds = ConvertByBetType.MOdds4(GetValueFromDataRow(row, "modds", 0));
            }

            //stake            
            object mpStake = GetValueFromDataRow(row, "stake", 0);
            object mpActualStake = GetValueFromDataRow(row, "actual_stake", 0);

            base.stake = (Cast.AsDecimal(mpStake) == 0)
                          ? ConvertByBetType.Stake(mpActualStake)
                          : string.Join(null, new string[] { ConvertByBetType.Stake(mpActualStake), "<br/>", "&nbsp;(", ConvertByBetType.Stake(mpStake), ")" });
        }

        public override object GetChoice(Excel.BetTypeExcelData excelData)
        {
            int betTypeId = Convert.ToInt32(excelData.DataRow["bettype"]);
            string betteam = Convert.ToString(excelData.DataRow["betteam"]);
            string betTypeName = CacheRef.GetBettype(betTypeId);

            MixParlayDelegate mixParlayDelegate = new MixParlayDelegate(BetListBO.GetMixParlayDetail);
            DataTable betListData = mixParlayDelegate(long.Parse(base.TransTimeHyperLink.refNo), CurrentUserSession.LangKey);

            string mixParlay = betTypeName + " - " + GetCombinationOfferName(betteam) + "\n\n";
            excelData.RtfTextRender.AddText(mixParlay);
            return ExcelFactory.RenderMixParlay(CurrentBetType, ref betListData, excelData);
        }

        public override void AssignEvent(System.Data.DataRow row)
        {
            int betTypeId = Convert.ToInt32(row["bettype"]);
            string betteam=Convert.ToString(row["betteam"]);
            
            string betTypeName = CacheRef.GetBettype(betTypeId);
            
            if (this.ExportExcel)
            {
                MixParlayDelegate mixParlayDelegate = new MixParlayDelegate(BetListBO.GetMixParlayDetail);

                DataTable betListData = mixParlayDelegate(long.Parse(base.TransTimeHyperLink.refNo), CurrentUserSession.LangKey);

                string mixParlay = betTypeName + " - " + GetCombinationOfferName(betteam) + "<br/></br/><br/>";
                StringBuilder mixParlayContent = RenderingFactory.RenderMixParlay(CurrentBetType, ref betListData, this.ExportExcel);
                this.Event.SetValue(mixParlay + mixParlayContent.ToString());
            }
            else
            {
                string mixParlay = string.Empty, custid = string.Empty;
                if (HttpContext.Current.Request["custid"] != null)
                {
                    custid = HttpContext.Current.Request["custid"];
                }
                else
                { 
                   custid = GetValueFromDataRow(row, "custid", 0).ToString();
                }

                //Only use for SmartPhone
                if ((CurrentBetType & BetListType.SmartPhone) == BetListType.SmartPhone)
                {
                    mixParlay = String.Join(null, new string[]{
                                "<div><a data-rel='dialog' href='CombinationMixParlay.aspx?transid=", TransTimeHyperLink.refNo, "'>",
                                betTypeName,
                                "</a>", " - " + GetCombinationOfferName(betteam) +"</div>"
                                , "<div class='mt5'><a data-rel='dialog' href='CombinationMixParlayDetail.aspx?refno=", GetValueFromDataRow(row, "refno", 0).ToString(), 
                                                                                                            "&transid=", GetValueFromDataRow(row,"transid",0).ToString(), 
                                                                                                            "&betid=", GetValueFromDataRow(row,"betid",0).ToString(), 
                                                                                                            "&custid=", custid, 
                                                                                                            "'>", Resources.BetList.details,"</a><div>"
                                });
                }
                else
                {                
                    mixParlay = String.Join(null, new string[]{
                                "<div class=\"combinationLink\"><a id=\"hidden0\" href=\"javascript:showCombMP(&quot;",
                                base.TransTimeHyperLink.refNo, "&quot;,&quot;" , base.No.orderNo ,"&quot;);\">",
                                betTypeName,"</a>", " - ",
                                
                                "<span style=\"color:#3a3a3a\">",GetCombinationOfferName(betteam),"</span>",
                                
                                "</div>",
                                
                                "<div id=\"divEvent_" + base.TransTimeHyperLink.refNo + base.No.orderNo + "\" style=\"display: none\" ></div>",
                                
                                "<div class=\"detail\" onclick=\"showCombMPDetail(&quot;",GetValueFromDataRow(row, "refno", 0).ToString(),"&quot;",
                                                                                    ",&quot;",base.TransTimeHyperLink.winlostDate,"&quot;",
                                                                                    ",&quot;",GetValueFromDataRow(row,"transid",0).ToString(),"&quot;",
                                                                                    ",&quot;",GetValueFromDataRow(row,"betid",0).ToString(),"&quot;",
                                                                                    ",&quot;", custid,"&quot;);\">",                                
                                "<a style=\"color:#755200;font-weight:bold\" href=\"javascript:void(&quot;&quot;);\">",Resources.BetList.details,"</a>",
                                
                                "</div>"
                                });
                }

                this.Event.SetValue(mixParlay);
            }                                    
        }

        private static Dictionary<string, string> _combinationOfferName = new Dictionary<string, string> 
        { 
            {"110","Trixie"},
            {"1110","Yankee"},
            {"11110","Canadian"},
            {"111110","Heinz"},
            {"1111110","Super Heinz"},
            {"11111110","Goliath"}
        };

        private static string GetCombinationOfferName(string betteam)
        {
            betteam = betteam.Trim();
            return _combinationOfferName.ContainsKey(betteam) ? _combinationOfferName[betteam] : "";
        }

    }
}
