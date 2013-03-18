namespace Sunplus.Agent.UI._MemberInfo.Commission
{

    using System;
    using System.Data;
    using System.Text;
    using CasinoLibrary.SBAgent;
    using Fanex.Agent.UI._MemberInfo.Commission;
    using Resources;
    using Sunplus.Agent.Model.BL.MemberInfo.MemberCommission;
    using Sunplus.Agent.UI._MemberCommision;
    using Sunplus.Agent.Utilities;

    public partial class MemberCommissionCasino_Page : MemberCommision_Page
    {
        private int idMaster = 0, idAgent = 0, idFirstAgent = 0, idFirstMaster = 0;
        private MemberCommissionBO bo = null;
        private MemberCommissionCasino_Template tpl = new MemberCommissionCasino_Template();
        private Report rptContent = new Report();
        private string[] wildcard = new string[] { "P", "A" };

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            int count = 1;
            string custId = "-1";

            this.tpl.CommonGroup = GroupOptionMemberCommission("CA");

            // Load Game Kind, Game Type
            int colCount;
            this.RenderGameKindGameType(out colCount);

            // Load DataTable        
            DataTable dataTable = View as DataTable;
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                string evenBg = Fanex.Age.UI.Common.RowCssEven;
                string oddBg = Fanex.Age.UI.Common.RowCssOdd;

                StringBuilder result = new StringBuilder();
                var row1 = string.Empty;
                var row2 = string.Empty;
                var row3 = string.Empty;

                DataRowCollection dtc = dataTable.Rows;
                foreach (DataRow dr in dtc)
                {
                    if (custId != this.CheckValueNull(dr["custid"]))
                    {
                        if (count > 1)
                        {
                            result.Append(row1);
                            result.Append(row2);
                            if (this.wildcard.Length == 3)
                            {
                                result.Append(row3);
                            }
                        }

                        // Copy Block Row
                        var tplRow = this.tpl.Rows;

                        custId = this.CheckValueNull(dr["custid"]);
                        if (count % 2 != 0)
                        {
                            tplRow.BgRow = evenBg;
                        }
                        else
                        {
                            tplRow.BgRow = oddBg;
                        }

                        tplRow.NonRepeat.No = count.ToString();
                        tplRow.NonRepeat.UserName = tplRow.NonRepeat1.UserName = this.CheckValueNull(dr["UserName"]);
                        tplRow.NonRepeat.RowSpan = tplRow.NonRepeat1.RowSpan = "rowspan='" + this.wildcard.Length.ToString() + "'";
                        tplRow.Role.Role = tplRow.Role1.Role = this.wildcard[0].ToUpperInvariant();

                        tplRow.NonRepeat.Show();
                        tplRow.NonRepeat1.Show();
                        row1 = tplRow;

                        // Assign row 2 & 3.
                        tplRow.NonRepeat.Hide();
                        tplRow.NonRepeat1.Hide();

                        tplRow.Role.Role = tplRow.Role1.Role = this.wildcard[1].ToUpperInvariant();
                        row2 = tplRow;

                        if (this.wildcard.Length == 3)
                        {
                            tplRow.Role.Role = tplRow.Role1.Role = this.wildcard[2].ToUpperInvariant();
                            row3 = tplRow;
                        }

                        count++;
                    }

                    // Apply position taking data from API
                    row1 = row1.Replace("{" + this.CheckValueNull(dr["GameKind"]) + "_" + this.CheckValueNull(dr["GameTypeID"]) + "}", Formater.DecFormat(dr["MemberRollingRate"], 4, "0"));
                    row2 = row2.Replace("{" + this.CheckValueNull(dr["GameKind"]) + "_" + this.CheckValueNull(dr["GameTypeID"]) + "}", Formater.DecFormat(dr["AgentRollingRate"], 4, "0"));
                    if (this.wildcard.Length == 3)
                    {
                        row3 = row3.Replace("{" + this.CheckValueNull(dr["GameKind"]) + "_" + this.CheckValueNull(dr["GameTypeID"]) + "}", Formater.DecFormat(dr["MAgentRollingRate"], 4, "0"));
                    }
                }

                // for end of row --> append data to string builder : if (i == dataTable.Rows.Count - 1)
                if (dataTable.Rows.Count > 0)
                {
                    result.Append(row1);
                    result.Append(row2);
                    if (this.wildcard.Length == 3)
                    {
                        result.Append(row3);
                    }
                }

                this.tpl.Rows.SetValue(result.ToString());
                this.tpl.NoRowInfo.Clear();
            }
            else
            {
                if (this.wildcard.Length > 1)
                {
                    colCount = colCount + 5;
                }
                else
                {
                    colCount = colCount + 4;
                }

                this.tpl.Rows.Clear();
                this.tpl.NoRowInfo.NoInfoColspan = colCount.ToString();
                this.tpl.NoRowInfo.NoInfo = Lang.noInfo;
            }

            writer.Write(this.tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            if (CurrentUserSession.RoleId == 2)
            {
                this.tpl.CommToAllTypes = Lang.lblA + ", " + Lang.lblMe;
                this.tpl.GroupAgent.Clear();
                this.wildcard = new string[] { "P", "A" };
                return this.rptContent.MemberCommission(-1, -1, this.CheckValueNullInt(CurrentUserSession.CustId));
            }
            else if (CurrentUserSession.RoleId == 3)
            {
                this.tpl.CommToAllTypes = Lang.lblM + ", " + Lang.lblA + ", " + Lang.lblMe;
                this.tpl.GroupAgent.GroupMaster.Clear();
                this.tpl.GroupAgent.ComboAgent = GetComboCustomer(false, this.bo.GetCasinoAgentList(CurrentUserSession.CustId), CurrentUserSession.CustId, this.idAgent, ref this.idFirstAgent);

                this.wildcard = new string[] { "P", "A", "M" };
                if (this.idAgent == 0)
                {
                    return this.rptContent.MemberCommission(-1, this.CheckValueNullInt(CurrentUserSession.CustId), this.CheckValueNullInt(this.idFirstAgent));
                }
                else
                {
                    return this.rptContent.MemberCommission(-1, this.CheckValueNullInt(CurrentUserSession.CustId), this.CheckValueNullInt(this.idAgent));
                }
            }
            else
            {
                this.tpl.CommToAllTypes = Lang.lblM + ", " + Lang.lblA + ", " + Lang.lblMe;
                this.tpl.GroupAgent.GroupMaster.ComboMaster = GetComboCustomer(false, this.bo.GetCasinoMasterList(CurrentUserSession.CustId), CurrentUserSession.CustId, this.idMaster, ref this.idFirstMaster);
                this.wildcard = new string[] { "P", "A", "M" };

                if (this.idMaster != 0)
                {
                    this.tpl.GroupAgent.ComboAgent = GetComboCustomer(true, this.bo.GetCasinoAgentList(this.idMaster), this.idMaster, this.idAgent, ref this.idFirstAgent);
                    if (this.idMaster != this.idAgent && this.idAgent != 0)
                    {
                        return this.rptContent.MemberCommission(this.CheckValueNullInt(CurrentUserSession.CustId), this.CheckValueNullInt(this.idMaster), this.CheckValueNullInt(this.idAgent));
                    }
                    else
                    {
                        return this.rptContent.MemberCommission(this.CheckValueNullInt(CurrentUserSession.CustId), this.CheckValueNullInt(this.idMaster), -1);
                    }
                }
                else
                {
                    this.tpl.GroupAgent.ComboAgent = GetComboCustomer(true, this.bo.GetCasinoAgentList(this.idFirstMaster), this.idFirstMaster, this.idAgent, ref this.idFirstAgent);
                    return this.rptContent.MemberCommission(this.CheckValueNullInt(CurrentUserSession.CustId), this.CheckValueNullInt(this.idFirstMaster), -1);
                }
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.idMaster = GetIntRequest("idMaster", 0);
            this.idAgent = GetIntRequest("idAgent", 0);

            this.bo = this.bo == null ? new MemberCommissionBO(CurrentUserSession.NetSessionId) : this.bo;
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        private void RenderGameKindGameType(out int colCount)
        {
            MemberCommissionCasino_GameKind_Block gameKind = new MemberCommissionCasino_GameKind_Block();
            MemberCommissionCasino_GameType_Block gameType = new MemberCommissionCasino_GameType_Block();
            MemberCommissionCasino_Rows_RowData_Block rowData = new MemberCommissionCasino_Rows_RowData_Block();
            StringBuilder stbGameKind = new StringBuilder();
            StringBuilder stbGameType = new StringBuilder();
            StringBuilder stbRowData = new StringBuilder();
            colCount = 0;
            BasicInfo bi = new BasicInfo();
            DataTable tblGameKind = bi.GetGameKind("01");
            if (tblGameKind != null && tblGameKind.Rows.Count != 0)
            {
                // Load Game Type
                foreach (DataRow dr in tblGameKind.Rows)
                {
                    DataTable tblGameType = bi.GetGameType(Convert.ToInt32(dr["CodeID"]));
                    if (tblGameType != null && tblGameType.Rows.Count != 0)
                    {
                        foreach (DataRow dr2 in tblGameType.Rows)
                        {
                            gameType.GameTypeClass = "class='colHdr'";
                            gameType.lblGameType = dr2[DAL.APIClient.APICasinoProvider.Lang("GameTypeName")].ToString();
                            stbGameType.Append(gameType);

                            rowData.RowData = "{" + this.CheckValueNullInt(dr["CodeID"]).ToString() + "_" + this.CheckValueNullInt(dr2["GameTypeID"]).ToString() + "}";
                            stbRowData.Append(rowData);

                            // count column data
                            colCount++;
                        }

                        gameKind.GameKindColSpan = "colspan='" + tblGameType.Rows.Count.ToString() + "'";
                        gameKind.lblGameKind = dr[DAL.APIClient.APICasinoProvider.Lang("CodeName")].ToString();
                        stbGameKind.Append(gameKind);
                    }
                }

                this.tpl.GameKind.SetValue(stbGameKind.ToString());
                this.tpl.GameType.SetValue(stbGameType.ToString());
                this.tpl.Rows.RowData.SetValue(stbRowData.ToString());
            }
        }

        private string CheckValueNull(object value)
        {
            return value != null ? value.ToString() : string.Empty;
        }

        private int CheckValueNullInt(object value)
        {
            return value != null ? Convert.ToInt32(value) : 0;
        }
    }

}