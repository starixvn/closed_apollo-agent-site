namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Data;
    using Sunplus.Agent.UI.BetList.Excel;
    using Sunplus.Agent.Utilities;

    /// <summary>
    /// Win/Place
    /// </summary>
    public class BetType33 : BetType31
    {
        public delegate DataTable WinPlaceDelegate(long transid, string culture);

        public override object GetOdds(BetTypeExcelData excelData)
        {
            if (MatchedRows != null && MatchedRows.Length > 1)
            {
                string Odds_win = string.Empty;
                string Odds_place = string.Empty;
                for (int i = 0; i < MatchedRows.Length; i++)
                {
                    if (Convert.ToInt32(MatchedRows[i]["bettype"]) == 31) //win
                    {
                        switch ((MatchedRows[i]["status"]).ToString().ToLower())
                        {
                            case BetStatus.Won:
                                Odds_win = Formater.FormatNumber20(MatchedRows[i]["odds"], null);
                                break;
                            default:
                                Odds_win = "0.00";
                                break;
                        }
                    }
                    else //Place
                    {
                        switch ((MatchedRows[i]["status"]).ToString().ToLower())
                        {
                            case BetStatus.Won:
                                Odds_place = Formater.FormatNumber20(MatchedRows[i]["odds"], null);
                                break;
                            default:
                                Odds_place = "0.00";
                                break;
                        }
                    }
                }

                excelData.RtfTextRender.AddText(Resources.BetList.win + ": ");
                excelData.RtfTextRender.AddText(Odds_win, excelData.PosFont);
                excelData.RtfTextRender.AddText("\n" + Resources.BetList.place + ": ");
                excelData.RtfTextRender.AddText(Odds_place, excelData.PosFont);
                var rtfValue = excelData.RtfTextRender.Render();
                excelData.RtfTextRender.Clear();
                return rtfValue;
            }

            return "-";
        }

        public override object GetStatus(BetTypeExcelData excelData)
        {
            if (MatchedRows != null && MatchedRows.Length > 1)
            {
                string Status_win = string.Empty;
                string Status_place = string.Empty;
                for (int i = 0; i < MatchedRows.Length; i++)
                {
                    if (Convert.ToInt32(MatchedRows[i]["bettype"]) == 31) //win
                    {
                        switch ((MatchedRows[i]["status"]).ToString().ToLower())
                        {
                            case BetStatus.Won:
                                Status_win = Resources.BetList.won;
                                break;
                            default:
                                Status_win = Resources.BetList.lose;
                                break;
                        }
                    }
                    else //Place
                    {
                        switch ((MatchedRows[i]["status"]).ToString().ToLower())
                        {
                            case BetStatus.Won:
                                Status_place = Resources.BetList.won;
                                break;
                            default:
                                Status_place = Resources.BetList.lose;
                                break;
                        }
                    }
                }

                excelData.RtfTextRender.AddText(Resources.BetList.win + ": ");
                excelData.RtfTextRender.AddText(Status_win, excelData.PosFont);
                excelData.RtfTextRender.AddText("\n" + Resources.BetList.place + ": ");
                excelData.RtfTextRender.AddText(Status_place, excelData.PosFont);
            }

            excelData.RtfTextRender.AddText("\n" + this.Status.ShowIPVN.betIp);
            var rtfValue = excelData.RtfTextRender.Render();
            excelData.RtfTextRender.Clear();
            return rtfValue;
        }

        public override object GetSuperPT_Comm(BetTypeExcelData excelData)
        {
            if (MatchedRows != null && MatchedRows.Length > 1)
            {
                for (int i = 0; i < MatchedRows.Length; i++)
                {
                    if (this.Super.SuperPT != null && this.Super.SuperPT.Visible)
                    {
                        decimal spos_win = 0;
                        decimal spos_place = 0;
                        decimal value = 0;

                        if (IsNotEmpty(MatchedRows[i], "spositiontaking"))
                        {
                            value = Cast.AsDecimal(MatchedRows[i]["spositiontaking"]) * 100;
                        }
                        if (Convert.ToInt32(MatchedRows[i]["bettype"]) == 31) //win
                        {
                            spos_win = value;
                        }
                        else //Place
                        {
                            spos_place = value;
                        }

                        this.Super.SuperPT.superPT_setting = Resources.BetList.win + ": " + Formater.FormatNumber2(spos_win, null) + "%\n" + Resources.BetList.place + ": " + Formater.FormatNumber2(spos_place, null) + "%";
                    }
                }
            }
            return base.GetSuperPT_Comm(excelData);
        }

        public override object GetMasterPT_Comm(BetTypeExcelData excelData)
        {
            if (MatchedRows != null && MatchedRows.Length > 1)
            {
                for (int i = 0; i < MatchedRows.Length; i++)
                {
                    if (this.Master.MasterPT != null && this.Master.MasterPT.Visible)
                    {
                        decimal mpos_win = 0;
                        decimal mpos_place = 0;
                        decimal value = 0;

                        if (IsNotEmpty(MatchedRows[i], "mpositiontaking"))
                        {
                            value = Cast.AsDecimal(MatchedRows[i]["mpositiontaking"]) * 100;
                        }
                        if (Convert.ToInt32(MatchedRows[i]["bettype"]) == 31) //win
                        {
                            mpos_win = value;
                        }
                        else //Place
                        {
                            mpos_place = value;
                        }

                        this.Master.MasterPT.masterPT_setting = Resources.BetList.win + ": " + Formater.FormatNumber2(mpos_win, null) + "%\n" + Resources.BetList.place + ": " + Formater.FormatNumber2(mpos_place, null) + "%";
                    }
                }
            }
            return base.GetMasterPT_Comm(excelData);
        }

        public override object GetAgentPT_Comm(BetTypeExcelData excelData)
        {
            if (MatchedRows != null && MatchedRows.Length > 1)
            {
                for (int i = 0; i < MatchedRows.Length; i++)
                {
                    if (this.Agent.AgentPT != null && this.Agent.AgentPT.Visible)
                    {
                        decimal apos_win = 0;
                        decimal apos_place = 0;
                        decimal value = 0;

                        if (IsNotEmpty(MatchedRows[i], "apositiontaking"))
                        {
                            value = Cast.AsDecimal(MatchedRows[i]["apositiontaking"]) * 100;
                        }
                        if (Convert.ToInt32(MatchedRows[i]["bettype"]) == 31) //win
                        {
                            apos_win = value;
                        }
                        else //Place
                        {
                            apos_place = value;
                        }

                        this.Agent.AgentPT.agentPT_setting = Resources.BetList.win + ": " + Formater.FormatNumber2(apos_win, null) + "%\n" + Resources.BetList.place + ": " + Formater.FormatNumber2(apos_place, null) + "%";
                    }
                }
            }
            return base.GetAgentPT_Comm(excelData);
        }

        public override void Assign(int index, System.Data.DataRow row)
        {
            base.Assign(index, row);

            string status = GetValueFromDataRow(row, "status", "null").ToString().ToLower(); // need use status value to check
            //set winloss (comm) is empty when status is "void", "reject", "refund"
            if (status.Equals(BetStatus.Void) || status.Equals(BetStatus.Reject) || status.Equals(BetStatus.Refund))
            {
                if (this.Winlost != null)
                {
                    this.Winlost.comm = string.Empty;
                }
            }

            if (base.OutDataTable != null && base.OutDataTable.Rows.Count > 0)
            {
                #region declare params

                string Odds_win = string.Empty;
                string Odds_place = string.Empty;

                string Status_win = string.Empty;
                string Status_place = string.Empty;

                decimal apos_win = 0;
                decimal mpos_win = 0;
                decimal spos_win = 0;

                decimal apos_place = 0;
                decimal mpos_place = 0;
                decimal spos_place = 0;

                #endregion

                DataRow[] foundRows = base.OutDataTable.Select("refno=" + base.TransTime.refNo);

                if (foundRows != null && foundRows.Length > 1)
                {
                    #region Win/Place

                    for (int i = 0; i < foundRows.Length; i++)
                    {
                        if (int.Parse(foundRows[i]["bettype"].ToString()) == 31) //win
                        {
                            GetWin(ref Odds_win, ref Status_win, foundRows, i);
                            //Get PT
                            GetPT(ref apos_win, ref mpos_win, ref spos_win, foundRows, i);
                        }
                        else //Place
                        {
                            GetPlace(ref Odds_place, ref Status_place, foundRows, i);
                            //Get PT
                            GetPT(ref apos_place, ref mpos_place, ref spos_place, foundRows, i);
                        }
                    }

                    #endregion

                    SetOdds(row, ref Odds_win, ref Odds_place);

                    SetStatus(row, ref Status_win, ref Status_place);
                }
                else
                {
                    this.odds = "-";
                }
                SetPT(apos_win, mpos_win, spos_win, apos_place, mpos_place, spos_place);
            }
        }

        private void SetStatus(System.Data.DataRow row, ref string Status_win, ref string Status_place)
        {
            switch (row["status"].ToString().ToLower())
            {
                case BetStatus.Running:
                case BetStatus.Waiting:
                case BetStatus.Void:
                case BetStatus.Reject:
                case BetStatus.Refund: break;
                default:
                    Status_win = "<span class='blue' style='font-weight:normal;'>" + Status_win + "</span>";
                    Status_place = "<span class='' style='font-weight:normal;'>" + Status_place + "</span>";
                    if (this.Status != null)
                    {
                        this.Status.result = Status_win + "<br>" + Status_place;
                    } break;
            }
        }

        private void SetOdds(System.Data.DataRow row, ref string Odds_win, ref string Odds_place)
        {
            string status = row["status"].ToString().ToLower();
            if (status.Equals(BetStatus.Won) || status.Equals(BetStatus.Lose))
            {
                Odds_win = "<span class='blue' style='font-weight:normal;'>" + Odds_win + "</span>";
                Odds_place = "<span class='' style='font-weight:normal;'>" + Odds_place + "</span>";
                this.odds = Odds_win + "<br>" + Odds_place;
            }
            else
            {
                this.odds = "-";
            }
        }

        private void SetPT(decimal apos_win, decimal mpos_win, decimal spos_win, decimal apos_place, decimal mpos_place, decimal spos_place)
        {
            if (base.Super != null && base.Super.SuperPT != null)
            {
                base.Super.SuperPT.superPT_setting = string.Join("", new string[] { "<span class='blue' style='font-weight:normal;'>"
                                                                      , Resources.BetList.win + ": "
                                                                      , "<b>",Formater.FormatNumber2(spos_win, null),"%</b>"
                                                                      , "</span><br>"
                                                                      , "<span class='' style='font-weight:normal;'>"
                                                                      , Resources.BetList.place + ": "
                                                                      , "<b>", Formater.FormatNumber2(spos_place, null), "%</b>"
                                                                      , "</span>"});
            }

            if (base.Master != null && base.Master.MasterPT != null)
            {
                base.Master.MasterPT.masterPT_setting = string.Join("", new string[]{ "<span class='blue' style='font-weight:normal;'>"
                                                                      ,Resources.BetList.win + ": "
                                                                      ,"<b>",Formater.FormatNumber2(mpos_win, null),"%</b>"
                                                                      ,"</span><br>"
                                                                      ,"<span class='' style='font-weight:normal;'>"
                                                                      ,Resources.BetList.place + ": "
                                                                      ,"<b>",Formater.FormatNumber2(mpos_place, null),"%</b>"
                                                                      ,"</span>"});
            }

            if (base.Agent != null && base.Agent.AgentPT != null)
            {
                base.Agent.AgentPT.agentPT_setting = string.Join("", new string[]{ "<span class='blue' style='font-weight:normal;'>"
                                                                      ,Resources.BetList.win + ": "
                                                                      ,"<b>",Formater.FormatNumber2(apos_win, null),"%</b>"
                                                                      ,"</span><br>"
                                                                      ,"<span class='' style='font-weight:normal;'>"
                                                                      ,Resources.BetList.place + ": "
                                                                      ,"<b>",Formater.FormatNumber2(apos_place, null),"%</b>"
                                                                      ,"</span>"});
            }
        }

        private static void GetPlace(ref string Odds_place, ref string Status_place, DataRow[] foundRows, int i)
        {
            switch ((foundRows[i]["status"]).ToString().ToLower())
            {
                case BetStatus.Won:
                    Odds_place += string.Join(null, new string[]{Resources.BetList.place,
                                                                          ": ",
                                                                          "<b>",
                                                                          Formater.FormatNumber20(foundRows[i]["odds"], null),
                                                                          "</b>"});

                    Status_place += string.Join(null, new string[]{Resources.BetList.place,
                                                                          ": ",
                                                                          "<b>",
                                                                          Resources.BetList.won,
                                                                          "</b>"});
                    break;
                default:
                    Odds_place += string.Join(null, new string[]{Resources.BetList.place,
                                                                          ": ",
                                                                          "<b>",
                                                                          "0.00",
                                                                          "</b>"});

                    Status_place += string.Join(null, new string[]{Resources.BetList.place,
                                                                          ": ",
                                                                          "<b>",
                                                                          Resources.BetList.lose,
                                                                          "</b>"});
                    break;
            }
        }

        private static void GetWin(ref string Odds_win, ref string Status_win, DataRow[] foundRows, int i)
        {
            switch ((foundRows[i]["status"]).ToString().ToLower())
            {
                case BetStatus.Won:
                    Odds_win += string.Join(null, new string[]{Resources.BetList.win,
                                                                          ": ",
                                                                          "<b>",
                                                                          Formater.FormatNumber20(foundRows[i]["odds"], null),
                                                                          "</b>"});

                    Status_win += string.Join(null, new string[]{Resources.BetList.win,
                                                                          ": ",
                                                                          "<b>",
                                                                          Resources.BetList.won,
                                                                          "</b>"});

                    break;
                default:
                    Odds_win += string.Join(null, new string[]{Resources.BetList.win,
                                                                          ": ",
                                                                          "<b>",
                                                                          "0.00",
                                                                          "</b>"});

                    Status_win += string.Join(null, new string[]{Resources.BetList.win,
                                                                          ": ",
                                                                          "<b>",
                                                                          Resources.BetList.lose,
                                                                          "</b>"});

                    break;
            }
        }

        private void GetPT(ref decimal apos_win, ref decimal mpos_win, ref decimal spos_win, DataRow[] foundRows, int i)
        {
            if (base.Super != null && base.Super.SuperPT != null)
            {
                if (IsNotEmpty(foundRows[i], "spositiontaking"))
                {
                    spos_win = Cast.AsDecimal(foundRows[i]["spositiontaking"]) * 100;
                }
            }
            if (base.Master != null && base.Master.MasterPT != null)
            {
                if (IsNotEmpty(foundRows[i], "mpositiontaking"))
                {
                    mpos_win = Cast.AsDecimal(foundRows[i]["mpositiontaking"]) * 100;
                }
            }
            if (base.Agent != null && base.Agent.AgentPT != null)
            {
                if (IsNotEmpty(foundRows[i], "apositiontaking"))
                {
                    apos_win = Cast.AsDecimal(foundRows[i]["apositiontaking"]) * 100;
                }
            }
        }
    }
}
