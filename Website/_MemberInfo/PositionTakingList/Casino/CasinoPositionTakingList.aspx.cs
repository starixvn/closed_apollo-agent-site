/*
 * Created 20111120@Simon - Render Casino Position Taking List
 */
namespace Sunplus.Agent.UI._MemberInfo.PositionTakingList.Casino
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using CasinoLibrary.SBAgent;    
    using Fanex.Age.UI.Reports.Components;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.UI._MemberInfo.PositionTakingList.Casino;
    using Sunplus.Agent.Utilities;

    public partial class CasinoPositionTakingList_Page : ContextMenuColumnsBase
    {
        #region Variable       
        private const string CasinoPtList = "CasinoPositionTakingList";
        private int custId = 0;
        private bool error = false;
        private string messageError = "Not casino's Customer";
        private string type = string.Empty, viewType = string.Empty;
        private string[] wildcard = new string[] { "a" };        
        #endregion

        #region Override
        public override void RenderContent(HtmlTextWriter writer)
        {
            writer.Write(this.GetCasinoPositionTaking());
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.type = (Request.QueryString["type"] == null) ? string.Empty : Request.QueryString["type"].ToString();
            this.custId = (Request.QueryString["custid"] == null) ? CurrentUserSession.CustId : Convert.ToInt32(Request.QueryString["custid"]);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            Dictionary<string, DataTable> view = new Dictionary<string, DataTable>();
            DataTable positionTaking = null;
            Report report = new Report();
            try
            {
                if (CurrentUserSession.RoleId == (int)UserTypes.Agent)
                {
                    this.type = "agent";
                    positionTaking = report.PTByAgent(this.custId);
                    this.wildcard = new string[] { "a" };
                }
                else
                {
                    if (CurrentUserSession.RoleId == (int)UserTypes.Master)
                    {
                        if (this.type == string.Empty)
                        {
                            this.wildcard = new string[] { "m", "a" };
                            positionTaking = report.PTByMaster(this.custId);
                            this.viewType = "agent";
                        }
                        else if (this.type == "agent")
                        {
                            this.wildcard = new string[] { "m", "a" };
                            positionTaking = report.PTByAgent(this.custId);
                        }
                    }
                    else if (CurrentUserSession.RoleId == (int)UserTypes.Super)
                    {
                        if (this.type == string.Empty)
                        {
                            this.wildcard = new string[] { "s" };
                            positionTaking = report.PTBySuper(this.custId);
                            this.viewType = "master";
                        }
                        else if (this.type == "master")
                        {
                            this.wildcard = new string[] { "m", "a" };
                            positionTaking = report.PTByMaster(this.custId);
                            this.viewType = "agent";
                        }
                        else if (this.type == "agent")
                        {
                            this.wildcard = new string[] { "s", "m", "a" };
                            positionTaking = report.PTByAgent(this.custId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.messageError = ex.Message;
                this.error = true;
            }

            view.Add(CasinoPtList, positionTaking);
            return view;
        }
        #endregion

        private string GetCasinoPositionTaking()
        {
            DataTable dtbPositionTaking = (View as Dictionary<string, DataTable>)[CasinoPtList];
            CasinoPositionTakingList_Template tpl = new CasinoPositionTakingList_Template();

            RegisterJavascriptObject("error", this.error);
            RegisterJavascriptObject("messageError", this.messageError);

            string content = string.Empty;
            int count = 1;
            string valueCustId = "-1";            

            int colCount = 0;
            colCount = this.RenderGameKindGameType(tpl, colCount);

            if (dtbPositionTaking != null && dtbPositionTaking.Rows.Count > 0)
            {
                StringBuilder stbResult = new StringBuilder();
                var row1 = string.Empty;
                var row2 = string.Empty;
                var row3 = string.Empty;

                string closedBg = Fanex.Age.UI.Common.RowCssClosed;
                string suspendedBg = Fanex.Age.UI.Common.RowCssSuspended;
                string evenBg = Fanex.Age.UI.Common.RowCssEven;
                string oddBg = Fanex.Age.UI.Common.RowCssOdd;
                int countPTList = dtbPositionTaking.Rows.Count;

                for (int i = 0; i < countPTList; i++)
                {
                    if (valueCustId != this.CheckValueNull(dtbPositionTaking.Rows[i]["ibccustid"]))
                    {
                        stbResult.Append(row1);
                        if (this.wildcard.Length > 1)
                        {
                            stbResult.Append(row2);
                            if (this.wildcard.Length == 3)
                            {
                                stbResult.Append(row3);
                            }
                        }

                        valueCustId = this.CheckValueNull(dtbPositionTaking.Rows[i]["ibccustid"]);

                        // Copy Block Row
                        var tplRow = tpl.Rows;

                        if (Convert.ToInt32(dtbPositionTaking.Rows[i]["closed"]) != 0)
                        {
                            tplRow.BgRow = closedBg;
                        }
                        else if (Convert.ToInt32(dtbPositionTaking.Rows[i]["suspended"]) != 0)
                        {
                            tplRow.BgRow = suspendedBg;
                        }
                        else if (count % 2 != 0)
                        {
                            tplRow.BgRow = evenBg;
                        }
                        else
                        {
                            tplRow.BgRow = oddBg;
                        }

                        // Edit User position taking
                        if (this.type == "agent")
                        {
                            tplRow.NonRepeat.EditMember.custid_value = tplRow.NonRepeat1.EditMember1.custid_value = valueCustId;
                            tplRow.NonRepeat.EditMember.UserName = tplRow.NonRepeat1.EditMember1.UserName = this.CheckValueNull(dtbPositionTaking.Rows[i]["UserName"]);
                        }
                        else
                        {
                            tplRow.NonRepeat.ViewDownLine.custid_value = tplRow.NonRepeat1.ViewDownLine1.custid_value = valueCustId;
                            tplRow.NonRepeat.ViewDownLine.type = tplRow.NonRepeat1.ViewDownLine1.type = this.viewType;
                            tplRow.NonRepeat.ViewDownLine.UserName = tplRow.NonRepeat1.ViewDownLine1.UserName = this.CheckValueNull(dtbPositionTaking.Rows[i]["UserName"]);
                        }

                        // Apply data for first row
                        tplRow.NonRepeat.No = count.ToString();
                        tplRow.NonRepeat.rowspan = tplRow.NonRepeat1.rowspan = "rowspan='" + this.wildcard.Length.ToString() + "'";

                        if (this.wildcard.Length > 1)
                        {
                            tplRow.Role.Role = tplRow.Role1.Role = this.wildcard[0].ToUpperInvariant();
                        }

                        tplRow.NonRepeat.Show();
                        tplRow.NonRepeat1.Show();
                        row1 = tplRow;

                        // Assign row 2 & 3.
                        if (this.wildcard.Length > 1)
                        {
                            tplRow.NonRepeat.Hide();
                            tplRow.NonRepeat1.Hide();

                            tplRow.Role.Role = tplRow.Role1.Role = this.wildcard[1].ToUpperInvariant();
                            row2 = tplRow;

                            if (this.wildcard.Length == 3)
                            {
                                tplRow.Role.Role = tplRow.Role1.Role = this.wildcard[2].ToUpperInvariant();
                                row3 = tplRow;
                            }
                        }

                        count++;
                    }

                    row1 = this.SetGameKindGameType(dtbPositionTaking, row1, i, 0); 

                    if (this.wildcard.Length > 1)
                    {
                        row2 = this.SetGameKindGameType(dtbPositionTaking, row2, i, 1); 
                    }

                    if (this.wildcard.Length == 3)
                    {
                        row3 = this.SetGameKindGameType(dtbPositionTaking, row3, i, 2); 
                    }
                }

                // for end of row --> append data to string builder : if (i == dataTable.Rows.Count - 1)
                if (countPTList > 0)
                {
                    stbResult.Append(row1);
                    if (this.wildcard.Length > 1)
                    {
                        stbResult.Append(row2);
                        if (this.wildcard.Length == 3)
                        {
                            stbResult.Append(row3);
                        }
                    }
                }

                content = stbResult.ToString();
            }
            else
            {
                NoInfo_Template noInfoRow = new NoInfo_Template();
                if (this.wildcard.Length > 1)
                {
                    colCount = colCount + 5;
                }
                else
                {
                    colCount = colCount + 4;
                }

                noInfoRow.NumCols = colCount.ToString();
                content = noInfoRow;
                tpl.ContextMenu = null;
            }

            tpl.Rows.SetValue(content);
            return tpl;
        }

        private int RenderGameKindGameType(CasinoPositionTakingList_Template tpl, int colCount)
        {
            BasicInfo bi = new BasicInfo();
            DataTable tblGameKind = bi.GetGameKind("01");
            if (tblGameKind != null && tblGameKind.Rows.Count != 0)
            {
                StringBuilder gameKind = new StringBuilder();
                StringBuilder gameType = new StringBuilder();
                StringBuilder rowData = new StringBuilder();

                CasinoPositionTakingList_header_GameKind_Block tplGameKind = new CasinoPositionTakingList_header_GameKind_Block();
                CasinoPositionTakingList_header_GameType_Block tplGameType = new CasinoPositionTakingList_header_GameType_Block();
                CasinoPositionTakingList_Rows_RowData_Block tplRowData = new CasinoPositionTakingList_Rows_RowData_Block();

                int codeId = 0;
                string txtGameKind = string.Empty;
                ArrayList arrTextShowHide = new ArrayList();
                ArrayList arrCssShowHide = new ArrayList();
                DataRowCollection drcGameKind = tblGameKind.Rows;

                foreach (DataRow dr in drcGameKind)
                {
                    codeId = dr["CodeID"] != null ? Convert.ToInt32(dr["CodeID"]) : 0;
                    DataTable tblGameType = bi.GetGameType(codeId);
                    if (tblGameType != null && tblGameType.Rows.Count != 0)
                    {
                        txtGameKind = dr[DAL.APIClient.APICasinoProvider.Lang("CodeName")].ToString();
                        arrTextShowHide.Add(txtGameKind);
                        arrCssShowHide.Add("sh" + codeId);

                        tplGameKind.csShowHide = tplGameType.csShowHide = "sh" + codeId;
                        DataRowCollection drcGameType = tblGameType.Rows;
                        foreach (DataRow dr2 in drcGameType)
                        {
                            tplGameType.GameTypeClass = "class='colHdr'";
                            tplGameType.lblGameType = dr2[DAL.APIClient.APICasinoProvider.Lang("GameTypeName")].ToString();
                            gameType.Append(tplGameType);

                            tplRowData.RowData = "{" + codeId + "_" + (dr2["GameTypeID"] != null ? dr2["GameTypeID"].ToString() : "0") + "}";
                            tplRowData.csShowHide = "sh" + codeId;
                            rowData.Append(tplRowData);

                            colCount++;
                        }

                        tplGameKind.GameKindColSpan = "colspan='" + tblGameType.Rows.Count.ToString() + "'";
                        tplGameKind.lblGameKind = txtGameKind;
                        gameKind.Append(tplGameKind);
                    }
                }

                tpl.header.GameKind.SetValue(gameKind.ToString());
                tpl.header.GameType.SetValue(gameType.ToString());
                tpl.Rows.RowData.SetValue(rowData.ToString());

                // For Show Hide Column
                string[] strArrayText = (string[])arrTextShowHide.ToArray(typeof(string));
                string[] strArrayCss = (string[])arrCssShowHide.ToArray(typeof(string));
                RegisterCommonScriptObjects("tblMain", new string[] { "headerTop", "headerBottom" }, "ctxContainer", this);
                tpl.ContextMenu = RenderContextMenu(strArrayText, strArrayCss);
                tpl.ShowHideColumnClasses = RenderTableClasses(strArrayCss);
            }
            else
            {
                tpl.header.Hide();
            }

            // Should remove Role when wildcard.length = 1
            if (this.wildcard.Length == 1)
            {
                tpl.header.AdditionalRow = null;
                tpl.header.AdditionalRow1 = null;
                tpl.Rows.Role = null;
                tpl.Rows.Role1 = null;
                tpl.header.colNum = "2";
                tpl.header.colNum2 = "1";
            }
            else
            {
                tpl.header.colNum = "3";
                tpl.header.colNum2 = "2";
            }

            // Provide link to edit member if in agent level
            if (this.type == "agent")
            {
                tpl.Rows.NonRepeat.ViewDownLine = null;
                tpl.Rows.NonRepeat1.ViewDownLine1 = null;
            }
            else
            {
                tpl.Rows.NonRepeat.EditMember = null;
                tpl.Rows.NonRepeat1.EditMember1 = null;
            }

            return colCount;
        }

        private string SetGameKindGameType(DataTable dtbPositionTaking, string row1, int i, int index)
        {
            string gameKindGameType = string.Empty;
            if (this.wildcard[index].ToUpperInvariant() == "S")
            {
                gameKindGameType = Formater.DecFormat(dtbPositionTaking.Rows[i]["SPositionTaking"], 2, "0");
            }
            else if (this.wildcard[index].ToUpperInvariant() == "M")
            {
                gameKindGameType = Formater.DecFormat(dtbPositionTaking.Rows[i]["MPositionTaking"], 2, "0");
            }
            else
            {
                gameKindGameType = Formater.DecFormat(dtbPositionTaking.Rows[i]["APositionTaking"], 2, "0");
            }

            return row1.Replace("{" + this.CheckValueNull(dtbPositionTaking.Rows[i]["GameKind"]) + "_" + this.CheckValueNull(dtbPositionTaking.Rows[i]["GameTypeID"]) + "}",  gameKindGameType);
        }

        private string CheckValueNull(object value)
        { 
            return value != null ? value.ToString() : string.Empty;
        }
    }
}