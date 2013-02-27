/// Handicap bet type. !!! NEVER CHANGE THIS CLASS!!!
namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Collections.Generic;
    using Fanex.Age.Utility.NPOI;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.UI.BetList.Excel;
    using Sunplus.Agent.UI.BetList.Utils;
    using Sunplus.Agent.Utilities;
    using Sunplus.Core.Common.IPs;

    /// <summary>
    /// Handicap bet type
    /// Show or hide columns base on BetListType
    /// </summary>
    public class BetType1 : BetTypeBase
    {        
        #region Properties
        public const string BL_UNDER_DOG = "bl_underdog";
        public const string BL_FAVORITE = "bl_favorite";
        private string home = string.Empty;
        private string away = string.Empty;
        private bool isLive = false;

        public BetListType CurrentBetType { get; set; }

        public object MatchID { get; set; }

        public object LeagueID { get; set; }

        public object BetTypeID { get; set; }

        public object SportTypeID { get; set; }        

        protected bool IsLive
        {
            get { return this.isLive; }
            set { this.isLive = value; }
        }        

        protected string Home
        {
            get { return this.home; }
            set { this.home = value; }
        }

        protected string Away
        {
            get { return this.away; }
            set { this.away = value; }
        }        
        #endregion

        public override void ToString(ref System.Text.StringBuilder bd)
        {
            base.ToString(ref bd);
        }

        #region IBetItem Members

        public override string Render()
        {
            return ToString();
        }

        public override void HideLinkedResources()
        {
            throw new NotImplementedException();
        }

        public override void Assign(int index, System.Data.DataRow row, BetListWorkbook workbook)
        {
            Assign(index, row);

            if (OutDataTable != null && OutDataTable.Rows.Count > 0)
            {
                MatchedRows = OutDataTable.Select("refno=" + TransTime.refNo);
            }

            var render = new RtfTextRender(workbook.NormalFont);
            var excelData = new BetTypeExcelData()
            {
                DataRow = row,
                NormalFont = workbook.NormalFont,
                PosFont = workbook.PosFont,
                NegFont = workbook.NegFont,
                RtfTextRender = render,
            };
            var numberTextRender = new NumberTextRender(render, workbook.PosFont, workbook.NegFont);

            var choiceData = this.GetChoice(excelData);
            List<IRichTextString> newChoiceData;
            if (choiceData is List<IRichTextString>)
            {
                newChoiceData = (List<IRichTextString>)choiceData;
            }
            else
            {
                newChoiceData = new List<IRichTextString>() { (IRichTextString)choiceData };
            }

            for (int i = 0; i < newChoiceData.Count; i++)
            {
                var item = newChoiceData[i];
                short maxRowHeight = workbook.GetRowHeight(item);
                var values = new List<object>();
                var excelRow = workbook.CopyRow(index);
                bool resolve = i == 0;

                // No
                if (resolve)
                {
                    values.Add(index);
                }
                else
                {
                    values.Add(string.Empty);
                }

                // Member
                if (this.Member != null && this.Member.Visible)
                {
                    if (resolve)
                    {
                        values.Add(this.Member.userName);
                    }
                    else
                    {
                        values.Add(string.Empty);
                    }
                }

                // Trans. Time - show TransTimeHyperLink or TransTime
                if (true)
                {
                    if (resolve)
                    {
                        string transTime = Resources.BetList.refno + ": " + this.TransTime.refNo;
                        transTime += "\n" + this.transTime.ToString();
                        var transTimeRtf = new HSSFRichTextString(transTime);
                        values.Add(transTimeRtf);
                    }
                    else
                    {
                        values.Add(string.Empty);
                    }
                }

                // Choice
                values.Add(item);

                // Odds
                if (resolve)
                {
                    values.Add(this.GetOdds(excelData));
                }
                else
                {
                    values.Add(string.Empty);
                }

                // Stake
                if (resolve)
                {
                    string newText = this.stake.Replace("<br/>&nbsp;", "\n");
                    values.Add(newText);
                }
                else
                {
                    values.Add(string.Empty);
                }

                // Win Loss
                if (this.Winlost != null && this.Winlost.Visible)
                {
                    if (resolve)
                    {
                        string winloss = this.Winlost.winloss;
                        numberTextRender.Render(winloss);
                        if (!string.IsNullOrEmpty(this.Winlost.comm))
                        {
                            winloss = "\n" + this.Winlost.comm;
                            render.AddText(winloss, workbook.PosFont);
                        }

                        var winlossRtf = render.Render();
                        render.Clear();
                        values.Add(winlossRtf);
                    }
                    else
                    {
                        values.Add(string.Empty);
                    }
                }

                // Status
                if (this.Status != null && this.Status.Visible)
                {
                    if (resolve)
                    {
                        values.Add(this.GetStatus(excelData));
                    }
                    else
                    {
                        values.Add(string.Empty);
                    }
                }

                // SuperPT/Comm
                if (this.Super != null && this.Super.Visible)
                {
                    if (resolve)
                    {
                        var superPtCommRtf = this.GetSuperPT_Comm(excelData);
                        maxRowHeight = Math.Max(maxRowHeight, workbook.GetRowHeight(superPtCommRtf));
                        values.Add(superPtCommRtf);
                    }
                    else
                    {
                        values.Add(string.Empty);
                    }
                }

                // MasterPT/Comm
                if (this.Master != null && this.Master.Visible)
                {
                    if (resolve)
                    {
                        var masterPtCommRtf = this.GetMasterPT_Comm(excelData);
                        maxRowHeight = Math.Max(maxRowHeight, workbook.GetRowHeight(masterPtCommRtf));
                        values.Add(masterPtCommRtf);
                    }
                    else
                    {
                        values.Add(string.Empty);
                    }
                }

                // AgentPT/Comm
                if (this.Agent != null && this.Agent.Visible)
                {
                    if (resolve)
                    {
                        var agentPtCommRtf = this.GetAgentPT_Comm(excelData);
                        maxRowHeight = Math.Max(maxRowHeight, workbook.GetRowHeight(agentPtCommRtf));
                        values.Add(agentPtCommRtf);
                    }
                    else
                    {
                        values.Add(string.Empty);
                    }
                }

                excelRow.WriteCells(0, values);

                excelRow.Height = maxRowHeight;
            }

            // merge cells
            if (newChoiceData.Count > 1)
            {
                int total = newChoiceData.Count;
                int firstRow = workbook.ContentRowIndex - total;
                int lastRow = workbook.ContentRowIndex - 1;
                var columns = workbook.ShownColumns;
                foreach (var column in columns)
                {
                    if (column != workbook.Columns.Choice)
                    {
                        workbook.ResultSheet.MergeCells(firstRow, column, lastRow, column);
                    }
                }
            }
        }

        public override void RenderBetTeam(BetTypeExcelData excelData)
        {
            this.Event.betTeam = this.Event.betTeam.Replace("<span class=\"bl_favorite\">", string.Empty);
            this.Event.betTeam = this.Event.betTeam.Replace("</span>", string.Empty);
            this.Event.betTeam = this.Event.betTeam.Replace("&nbsp;", " ");
        }

        public override object GetChoice(BetTypeExcelData excelData)
        {
            return GetChoice(excelData, false);
        }

        public override object GetChoice(BetTypeExcelData excelData, bool isMixParlay)
        {
            // the 1st line
            this.RenderBetTeam(excelData);
            string choice = this.Event.betTeam;
            if (this.Event.Handicap != null && this.Event.Handicap.Visible)
            {
                choice += " " + this.Event.Handicap.handicap;
            }

            if (this.Event.Score != null && this.Event.Score.Visible)
            {
                choice += " [" + this.Event.Score.homeScore + "-" + this.Event.Score.awayScore + "]";
            }

            if (isMixParlay)
            {
                choice += " @ " + Formater.FormatNumber3(excelData.DataRow["odds"], null);
            }

            if (!string.IsNullOrEmpty(choice))
            {
                excelData.RtfTextRender.AddText(choice, excelData.PosFont);
                choice = "\n";
            }

            // the 2nd line
            if (!string.IsNullOrEmpty(this.Event.betTypeName))
            {
                choice += this.Event.betTypeName;
            }

            if (!string.IsNullOrEmpty(choice))
            {
                excelData.RtfTextRender.AddText(choice);
                choice = "\n";
            }

            // the 3rd line
            bool isAddNewLine = false;
            if (!string.IsNullOrEmpty(this.Event.homeTeam))
            {
                choice += this.Event.homeTeam;
            }

            if (!string.IsNullOrEmpty(choice))
            {
                isAddNewLine = true;
                excelData.RtfTextRender.AddText(choice);
            }

            if (excelData.DataRow.Table.Columns.Contains("betstatus"))
            {
                this.Event.home_firstGoal_lastGoal = string.Empty;
                this.Event.away_firstGoal_lastGoal = string.Empty;
                this.CalculateFG_LG_Excel(excelData.DataRow["betstatus"] as string, ref this.Event.home_firstGoal_lastGoal, ref this.Event.away_firstGoal_lastGoal);
            }

            choice = string.Empty;
            if (!string.IsNullOrEmpty(this.Event.home_firstGoal_lastGoal))
            {
                choice = " " + this.Event.home_firstGoal_lastGoal;
            }

            if (!string.IsNullOrEmpty(choice))
            {
                isAddNewLine = true;
                excelData.RtfTextRender.AddText(choice, excelData.PosFont);
            }

            choice = string.Empty;
            if (this.Event.VS != null && this.Event.VS.Visible)
            {
                choice = "- vs -";
            }

            if (!string.IsNullOrEmpty(this.Event.awayTeam))
            {
                choice += " " + this.Event.awayTeam;
            }

            if (!string.IsNullOrEmpty(choice))
            {
                isAddNewLine = true;
                excelData.RtfTextRender.AddText(choice);
            }

            choice = string.Empty;
            if (!string.IsNullOrEmpty(this.Event.away_firstGoal_lastGoal))
            {
                choice = " " + this.Event.away_firstGoal_lastGoal;
            }

            if (!string.IsNullOrEmpty(choice) || isAddNewLine)
            {
                excelData.RtfTextRender.AddText(choice, excelData.PosFont);
                choice = "\n";
            }

            // the 4th line
            if (!string.IsNullOrEmpty(this.Event.sportTypeName))
            {
                choice += this.Event.sportTypeName;
            }

            if (!string.IsNullOrEmpty(this.Event.LeagueName.leagueName))
            {
                if (!string.IsNullOrEmpty(choice))
                {
                    choice += " ";
                }

                choice += this.Event.LeagueName.leagueName;
            }

            if (!string.IsNullOrEmpty(choice))
            {
                excelData.RtfTextRender.AddText(choice);
            }

            var choiceRtf = excelData.RtfTextRender.Render();
            excelData.RtfTextRender.Clear();
            return choiceRtf;
        }

        public override object GetOdds(BetTypeExcelData excelData)
        {
            string odds = this.odds;
            var numberTextRender = new NumberTextRender(excelData.RtfTextRender, excelData.PosFont, excelData.NegFont);
            numberTextRender.Render(odds);
            if (!string.IsNullOrEmpty(this.oddType))
            {
                odds = "\n" + this.oddType;
                excelData.RtfTextRender.AddText(odds, excelData.PosFont);
            }

            var oddsRtf = excelData.RtfTextRender.Render();
            excelData.RtfTextRender.Clear();
            return oddsRtf;
        }

        public override object GetStatus(BetTypeExcelData excelData)
        {
            string status = this.Status.result;
            excelData.RtfTextRender.AddText(status, excelData.PosFont);
            if (this.Status.ShowIPVN.Visible)
            {
                status = "\n" + this.Status.ShowIPVN.betIp;
                excelData.RtfTextRender.AddText(status);
            }

            var statusRtf = excelData.RtfTextRender.Render();
            excelData.RtfTextRender.Clear();
            return statusRtf;
        }

        public override object GetSuperPT_Comm(BetTypeExcelData excelData)
        {
            var numberTextRender = new NumberTextRender(excelData.RtfTextRender, excelData.PosFont, excelData.NegFont);
            if (this.Super.SuperPT != null && this.Super.SuperPT.Visible)
            {
                string superPtComm = this.Super.SuperPT.superPT_setting + "\n";
                excelData.RtfTextRender.AddText(superPtComm, excelData.PosFont);
            }

            if (this.Super.SuperPTValue != null && this.Super.SuperPTValue.Visible)
            {
                string superPtComm = this.Super.SuperPTValue.superPT_value + "\n";
                numberTextRender.Render(superPtComm);
            }

            if (this.Super.SuperComm != null && this.Super.SuperComm.Visible)
            {
                string superPtComm = this.Super.SuperComm.superComm_setting + "\n";
                excelData.RtfTextRender.AddText(superPtComm);
            }

            if (this.Super.SuperCommValue != null && this.Super.SuperCommValue.Visible)
            {
                string superPtComm = this.Super.SuperCommValue.superComm_value;
                excelData.RtfTextRender.AddText(superPtComm);
            }

            var superPtCommRtf = excelData.RtfTextRender.Render();
            excelData.RtfTextRender.Clear();
            return superPtCommRtf;
        }

        public override object GetMasterPT_Comm(BetTypeExcelData excelData)
        {
            var numberTextRender = new NumberTextRender(excelData.RtfTextRender, excelData.PosFont, excelData.NegFont);
            if (this.Master.MasterPT != null && this.Master.MasterPT.Visible)
            {
                string masterPtComm = this.Master.MasterPT.masterPT_setting + "\n";
                excelData.RtfTextRender.AddText(masterPtComm, excelData.PosFont);
            }

            if (this.Master.MasterPTValue != null && this.Master.MasterPTValue.Visible)
            {
                string masterPtComm = this.Master.MasterPTValue.masterPT_value + "\n";
                numberTextRender.Render(masterPtComm);
            }

            if (this.Master.MasterComm != null && this.Master.MasterComm.Visible)
            {
                string masterPtComm = this.Master.MasterComm.masterComm_setting + "\n";
                excelData.RtfTextRender.AddText(masterPtComm);
            }

            if (this.Master.MasterCommValue != null && this.Master.MasterCommValue.Visible)
            {
                string masterPtComm = this.Master.MasterCommValue.masterComm_value;
                excelData.RtfTextRender.AddText(masterPtComm);
            }

            var masterPtCommRtf = excelData.RtfTextRender.Render();
            excelData.RtfTextRender.Clear();
            return masterPtCommRtf;
        }

        public override object GetAgentPT_Comm(BetTypeExcelData excelData)
        {
            var numberTextRender = new NumberTextRender(excelData.RtfTextRender, excelData.PosFont, excelData.NegFont);
            if (this.Agent.AgentPT != null && this.Agent.AgentPT.Visible)
            {
                string agentPtComm = this.Agent.AgentPT.agentPT_setting + "\n";
                excelData.RtfTextRender.AddText(agentPtComm, excelData.PosFont);
            }

            if (this.Agent.AgentPTValue != null && this.Agent.AgentPTValue.Visible)
            {
                string agentPtComm = this.Agent.AgentPTValue.agentPT_value + "\n";
                numberTextRender.Render(agentPtComm);
            }

            if (this.Agent.AgentComm != null && this.Agent.AgentComm.Visible)
            {
                string agentPtComm = this.Agent.AgentComm.agentComm_setting + "\n";
                excelData.RtfTextRender.AddText(agentPtComm);
            }

            if (this.Agent.AgentCommValue != null && this.Agent.AgentCommValue.Visible)
            {
                string agentPtComm = this.Agent.AgentCommValue.agentComm_value;
                excelData.RtfTextRender.AddText(agentPtComm);
            }

            var agentPtCommRtf = excelData.RtfTextRender.Render();
            excelData.RtfTextRender.Clear();
            return agentPtCommRtf;
        }

        public override void Assign(int index, System.Data.DataRow row)
        {
            // Set values
            this.LeagueID = this.GetValueFromDataRow(row, "leagueId", 0);
            this.MatchID = this.GetValueFromDataRow(row, "matchId", 0);
            this.BetTypeID = this.GetValueFromDataRow(row, "betType", 0);
            this.SportTypeID = this.GetValueFromDataRow(row, "sportType", 0);

            // Assign data here, all column name in lower case
            this.TransTimeHyperLink.refNo = this.betId = GetValueFromDataRow(row, "transid").ToString();
            this.TransTime.refNo = this.TransTimeHyperLink.refNo;

            if (this.Member != null)
            {
                this.Member.userName = GetValueFromDataRow(row, "username").ToString();
            }

            this.TransTimeHyperLink.Show();
            this.TransTime.Hide();

            this.transTime = this.IsNotEmpty(row, "transdate") ? Convert.ToDateTime(row["transdate"]).ToString() : string.Empty;

            this.TransTimeHyperLink.winlostDate = this.IsNotEmpty(row, "winlostdate") ? Formater.FullDateFormat(row["winlostdate"]) : Formater.FullDateFormat(DateTime.Today);

            this.TransTimeHyperLink.league = this.LeagueID.ToString();

            this.odds = ConvertByBetType.Odds(this.GetValueFromDataRow(row, "odds", 0), this.BetTypeID, this.GetValueFromDataRow(row, "oddstype", 0));

            this.oddType = CacheRef.GetOddsTypeName(ConvertByBetType.OddsType(this.BetTypeID, this.GetValueFromDataRow(row, "oddstype", 0)));

            this.stake = ConvertByBetType.Stake(this.GetValueFromDataRow(row, "stake"));

            if (this.Winlost != null)
            {
                this.Winlost.winloss = ConvertByBetType.Winloss(this.GetValueFromDataRow(row, "winlost", 0));

                // (change "commission" into "playercomm" )
                this.Winlost.comm = ConvertByBetType.Comm(this.GetValueFromDataRow(row, "playercomm"));
            }

            this.No.orderNo = index.ToString();

            string status = this.GetValueFromDataRow(row, "status", "null").ToString().ToLower(); // need use status value to check

            this.AssignStatus(row, status);

            this.AssignPtComm(row, status);

            this.AssignEvent(row);

            this.AssignScoreMapIcon(row, status);
        }

        public override void CustomizeColumns(UserTypes userType, BetListType betListType)
        {
            this.CurrentBetType = betListType;

            if ((betListType & BetListType.Canceled) == BetListType.Canceled)
            {
                this.Winlost = null;
                this.Super = null;
                this.Master = null;
                this.Agent = null;
            }
            else
            {
                if ((betListType & BetListType.Winlost) != BetListType.Winlost)
                {
                    this.Winlost = null;
                }

                if ((betListType & BetListType.Member) != BetListType.Member)
                {
                    this.Member = null;
                }

                if ((betListType & BetListType.DailyStatement) == BetListType.DailyStatement)
                {
                    this.Super = null;
                    this.Master = null;
                    this.Agent = null;
                }

                switch (userType)
                {
                    case UserTypes.Super:
                        break;
                    case UserTypes.Master:
                        this.Super = null;
                        break;
                    case UserTypes.Agent:
                        this.Super = null;
                        this.Master = null;
                        break;
                    default: // Hide all position taking
                        this.Super = null;
                        this.Master = null;
                        this.Agent = null;
                        break;
                }
            }

            // Hide "Result" for Cancel BetList
            if ((betListType & BetListType.Canceled) == BetListType.Canceled)
            {
                Status.StatusResult.Visible = false;
            }
        }

        /// <summary>
        /// Asign bet choice.
        /// </summary>
        /// <param name="row"></param>
        public override void AssignEvent(System.Data.DataRow row)
        {
            System.Data.DataColumnCollection columns = row.Table.Columns;

            // Set values
            if (this.LeagueID == null)
            {
                this.LeagueID = this.GetValueFromDataRow(row, "leagueId", 0);
            }

            if (this.BetTypeID == null)
            {
                this.BetTypeID = this.GetValueFromDataRow(row, "betType", 0);
            }

            if (this.SportTypeID == null)
            {
                this.SportTypeID = this.GetValueFromDataRow(row, "sportType", 0);
            }

            string betTeam = this.GetValueFromDataRow(row, "betteam").ToString();
            bool betHomeTeam = betTeam == "h";
            decimal hdp1 = columns.Contains("hdp1") ? Convert.ToDecimal(row["hdp1"]) : 0;
            decimal hdp2 = columns.Contains("hdp2") ? Convert.ToDecimal(row["hdp2"]) : 0;
            decimal handicap = hdp2 > hdp1 ? hdp2 : hdp1;

            bool isNeutral = columns.Contains("isneutral") ? (bool)row["isneutral"] : false;

            this.home = CacheRef.GetTeamName(this.GetValueFromDataRow(row, "homeid"));
            this.away = CacheRef.GetTeamName(this.GetValueFromDataRow(row, "awayid"));

            this.Event.betTeam = betHomeTeam ? this.home : this.away;
            this.Event.betTeamClassName = this.Event.Score.scoreClassName = betHomeTeam ? (hdp2 >= hdp1 ? BL_UNDER_DOG : BL_FAVORITE) : (hdp2 <= hdp1 ? BL_UNDER_DOG : BL_FAVORITE);
            this.Event.Handicap.handicap = ConvertByBetType.Hdp((this.Event.betTeamClassName == BL_FAVORITE) ? -handicap : handicap);

            this.Event.Score.homeScore = this.GetValueFromDataRow(row, "livehomescore").ToString();
            this.Event.Score.awayScore = this.GetValueFromDataRow(row, "liveawayscore").ToString();

            this.isLive = Convert.ToBoolean(row["liveindicator"]);

            // For special cases
            this.Event.Score.Visible = this.isLive;

            this.Event.betTypeName = CacheRef.GetBettype(Convert.ToInt32(this.BetTypeID));

            this.Event.homeTeam = this.home + (isNeutral ? " (N)" : string.Empty);
            this.Event.home_firstGoal_lastGoal = string.Empty;
            this.Event.awayTeam = this.away;
            this.Event.away_firstGoal_lastGoal = string.Empty;
            this.Event.strikeThrough = string.Empty;

            if (columns.Contains("betstatus"))
            {
                this.CalculateFG_LG(row["betstatus"] as string, ref this.Event.home_firstGoal_lastGoal, ref this.Event.away_firstGoal_lastGoal);
            }

            this.Event.sportTypeName = CacheRef.GetSportName(Convert.ToInt32(this.SportTypeID));
            this.Event.LeagueName.leagueName = CacheRef.GetLeagueName(Convert.ToInt32(this.LeagueID));
        }

        protected bool IsNotEmpty(System.Data.DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName))
            {
                if (string.IsNullOrEmpty(row[columnName].ToString()))
                {
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        protected object GetValueFromDataRow(System.Data.DataRow row, string columnName, object defaultValue)
        {
            object returnValue = row.Table.Columns.Contains(columnName) ? row[columnName] : defaultValue;

            if (string.IsNullOrEmpty(returnValue.ToString()))
            {
                returnValue = defaultValue;
            }

            return returnValue;
        }

        protected object GetValueFromDataRow(System.Data.DataRow row, string columnName)
        {
            return this.GetValueFromDataRow(row, columnName, "null");
        }        

        #endregion

        protected virtual void CalculateFG_LG_Excel(string betStatus, ref string homeFGLG, ref string awayFGLG)
        {
            if (betStatus == null)
            {
                return;
            }

            // Format: FL:1-2
            string[] args = betStatus.Split(new char[] { ':', '-' });

            if (args.Length != 3)
            {
                return;
            }

            if (args[1] == "1")
            {
                homeFGLG += "(F)";
            }
            else if (args[1] == "2")
            {
                awayFGLG += "(F)";
            }

            if (args[2] == "1")
            {
                homeFGLG += "(L)";
            }
            else if (args[2] == "2")
            {
                awayFGLG += "(L)";
            }
        }

        /// <summary>
        /// Return a string indicate FG LG status of Home team and Away team.
        /// </summary>
        /// <param name="betStatus"></param>
        /// <returns></returns>
        protected virtual void CalculateFG_LG(string betStatus, ref string homeFGLG, ref string awayFGLG)
        {
            if (betStatus == null)
            {
                return;
            }

            // Format: FL:1-2
            string[] args = betStatus.Split(new char[] { ':', '-' });

            if (args.Length != 3)
            {
                return;
            }

            if (ExportExcel)
            {
                if (args[1] == "1")
                {
                    homeFGLG += "<b>(F)</b>";
                }
                else if (args[1] == "2")
                {
                    awayFGLG += "<b>(F)</b>";
                }

                if (args[2] == "1")
                {
                    homeFGLG += "<b>(L)</b>";
                }
                else if (args[2] == "2")
                {
                    awayFGLG += "<b>(L)</b>";
                }
            }
            else
            {
                if (args[1] == "1")
                {
                    homeFGLG += "<div class=\"firstGoal\">&nbsp;</div>";
                }
                else if (args[1] == "2")
                {
                    awayFGLG += "<div class=\"firstGoal\">&nbsp;</div>";
                }

                if (args[2] == "1")
                {
                    homeFGLG += "<div class=\"lastGoal\">&nbsp;</div>";
                }
                else if (args[2] == "2")
                {
                    awayFGLG += "<div class=\"lastGoal\">&nbsp;</div>";
                }
            }
        }

        #region ScoreMapIcon
        private void AssignScoreMapIcon(System.Data.DataRow row, string status)
        {
            List<int> list = this.ScoreMapBetTypes;
            if (list.Contains(Convert.ToInt32(this.BetTypeID)) && status.Equals(BetStatus.Running) && ((this.CurrentBetType & BetListType.SmartPhone) != BetListType.SmartPhone))
            {
                this.Event.ScoreMapIcon.matchId = this.MatchID.ToString();
                this.Event.ScoreMapIcon.betTypeId = this.BetTypeID.ToString();
                this.Event.ScoreMapIcon.liveindicator = Convert.ToBoolean(row["liveindicator"]) ? "1" : "0";
            }
            else
            {
                this.Event.ScoreMapIcon = null;
            }
        }
        #endregion

        private void AssignPtComm(System.Data.DataRow row, string status)
        {
            if (this.Super != null)
            {
                this.Super.SuperPT.superPT_setting = ConvertByBetType.Pt(this.GetValueFromDataRow(row, "spositiontaking", 0)) + "%";
                if (status.Equals(BetStatus.Running) || status.Equals(BetStatus.Waiting))
                {
                    this.Super.SuperPTValue.Visible = false;
                    this.Super.SuperCommValue.Visible = false;
                }
                else
                {
                    this.Super.SuperPTValue.superPT_value = ConvertByBetType.PtValue(this.GetValueFromDataRow(row, "swinlost", 0));
                    this.Super.SuperCommValue.superComm_value = ConvertByBetType.Comm(this.GetValueFromDataRow(row, "scomm", 0));
                }

                this.Super.SuperComm.superComm_setting = (this.GetValueFromDataRow(row, "comstatus", "0").ToString() == "1" ? "2x " + ConvertByBetType.Comm_Percentage(this.GetValueFromDataRow(row, "sdiscount", 0)) : ConvertByBetType.Comm_Percentage(this.GetValueFromDataRow(row, "sdiscount", 0))) + "%";
            }

            if (this.Master != null)
            {
                this.Master.MasterPT.masterPT_setting = ConvertByBetType.Pt(this.GetValueFromDataRow(row, "mpositiontaking", 0)) + "%";
                if (status.Equals(BetStatus.Running) || status.Equals(BetStatus.Waiting))
                {
                    this.Master.MasterPTValue.Visible = false;
                    this.Master.MasterCommValue.Visible = false;
                }
                else
                {
                    this.Master.MasterPTValue.masterPT_value = ConvertByBetType.PtValue(this.GetValueFromDataRow(row, "mwinlost", 0));
                    this.Master.MasterCommValue.masterComm_value = ConvertByBetType.Comm(this.GetValueFromDataRow(row, "acomm", 0));
                }

                this.Master.MasterComm.masterComm_setting = (this.GetValueFromDataRow(row, "comstatus", "0").ToString() == "1" ? "2x " + ConvertByBetType.Comm_Percentage(this.GetValueFromDataRow(row, "mdiscount", 0)) : ConvertByBetType.Comm_Percentage(this.GetValueFromDataRow(row, "mdiscount", 0))) + "%";
            }

            if (this.Agent != null)
            {
                this.Agent.AgentPT.agentPT_setting = ConvertByBetType.Pt(this.GetValueFromDataRow(row, "apositiontaking", 0)) + "%";
                if (status.Equals(BetStatus.Running) || status.Equals(BetStatus.Waiting))
                {
                    this.Agent.AgentPTValue.Visible = false;
                    this.Agent.AgentCommValue.Visible = false;
                }
                else
                {
                    this.Agent.AgentPTValue.agentPT_value = ConvertByBetType.PtValue(this.GetValueFromDataRow(row, "awinlost", 0));
                    this.Agent.AgentCommValue.agentComm_value = ConvertByBetType.Comm(this.GetValueFromDataRow(row, "comm", 0));
                }

                this.Agent.AgentComm.agentComm_setting = (this.GetValueFromDataRow(row, "comstatus", "0").ToString() == "1" ? "2x " + ConvertByBetType.Comm_Percentage(this.GetValueFromDataRow(row, "adiscount", 0)) : ConvertByBetType.Comm_Percentage(this.GetValueFromDataRow(row, "adiscount", 0))) + "%";
            }
        }

        private void AssignStatus(System.Data.DataRow row, string status)
        {
            this.Status.result = status;
            if (status.Equals(BetStatus.Running))
            {
                this.Status.result = Resources.BetList.running;
            }
            else if (status.Equals(BetStatus.Reject))
            {
                this.Status.result = Resources.BetList.reject;
            }
            else if (status.Equals(BetStatus.Won))
            {
                this.Status.result = Resources.BetList.won;
            }
            else if (status.Equals(BetStatus.Lose))
            {
                this.Status.result = Resources.BetList.lose;
            }
            else if (status.Equals(BetStatus.Draw))
            {
                this.Status.result = Resources.BetList.draw;
            }
            else if (status.Equals(BetStatus.Waiting))
            {
                this.Status.result = Resources.BetList.waiting;
            }
            else if (status.Equals(BetStatus.Void))
            {
                this.Status.result = Resources.BetList.voided;
            }
            else if (status.Equals(BetStatus.Refund))
            {
                this.Status.result = Resources.BetList.refund;
            }

            // Check status
            if (status.Equals(BetStatus.Running) || status.Equals(BetStatus.Waiting))
            {
                if (this.Winlost != null)
                {
                    this.Winlost.winloss = "-";
                    this.Winlost.comm = string.Empty;
                }

                this.TransTimeHyperLink.isRunning = "1";
            }
            else
            {
                this.TransTimeHyperLink.isRunning = "0";
            }

            // Do NOT show transtime hyperlink when status is "void", "reject", "refund"
            if (status.Equals(BetStatus.Void) || status.Equals(BetStatus.Reject) || status.Equals(BetStatus.Refund))
            {
                this.TransTimeHyperLink.Hide();
                this.TransTime.Show();
            }

            // Hide "Result" when status is "running", "waiting", "void", "reject"
            if (status.Equals(BetStatus.Running) || status.Equals(BetStatus.Waiting)
                || status.Equals(BetStatus.Void) || status.Equals(BetStatus.Reject))
            {
                this.Status.StatusResult.Visible = false;
            }

            this.Status.StatusResult.matchId = this.MatchID.ToString();

            this.Status.StatusResult.betType = this.BetTypeID.ToString();

            this.Status.StatusResult.race = this.GetValueFromDataRow(row, "race", 0).ToString();

            this.Status.StatusResult.refNo = this.TransTime.refNo;

            this.Status.StatusResult.sportType = this.SportTypeID.ToString();

            this.Status.StatusResult.league = this.LeagueID.ToString();

            this.Status.StatusResult.userName = this.GetValueFromDataRow(row, "username", "null").ToString();

            this.Status.StatusResult.winlostDate = this.TransTimeHyperLink.winlostDate;

            // Show/Hide IP
            string ip = this.GetValueFromDataRow(row, "ip", string.Empty).ToString();

            bool isShowIpLink = this.IsAdminCanViewVNIP;

            if (!isShowIpLink)
            {
                if (IpUtils.IsVnIp(ip))
                {
                    this.Status.ShowIPVN.Visible = false;
                }
                else
                {
                    this.Status.ShowIPVN.betIp = ip;
                }
            }
            else
            {
                this.Status.ShowIPVN.betIp = ip;
            }
        }
    }
}
