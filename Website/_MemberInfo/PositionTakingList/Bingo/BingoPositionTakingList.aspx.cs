/*
 * Created 20111120@Simon - Render Bingo Position Taking List
 */
namespace Sunplus.Agent.UI._MemberInfo.PositionTakingList.Bingo
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using Fanex.Age.UI.Reports.Components;
    using Microsoft.Practices.Unity;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.UI._MemberInfo.PositionTakingList.Bingo;
    using Sunplus.Agent.Utilities;
    using Sunplus.Bingo.Core;
    using Sunplus.Bingo.Entities;
    using Sunplus.Bingo.Services;
    using Fanex.Agent.UI._MemberInfo.PositionTakingList.Bingo;

    public partial class BingoPositionTakingList_Page : ContextMenuColumnsBase
    {
        #region Variable        
        private const string BingoPtList = "BingoPositionTakingList";
        private int custId = 0;
        private string type = string.Empty, viewType = string.Empty;
        private string[] wildcard = new string[] { "a" };

        private IResult resultCustomer = new Sunplus.Bingo.Entities.Result();
        private IResult resultGameKind = new Sunplus.Bingo.Entities.Result();
        private IResult resultGameType = new Sunplus.Bingo.Entities.Result();
        #endregion

        #region Override
        public override void RenderContent(HtmlTextWriter writer)
        {
            writer.Write(this.GetBingoPositionTaking());
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
            IReportService reportService = BingoContainer.Current.Resolve<IReportService>();

            if (CurrentUserSession.RoleId == (int)UserTypes.Agent)
            {
                this.type = "agent";
                positionTaking = reportService.GetPositionTakingDataTable(null, null, this.custId, CurrentUserSession.RoleId - 1, ref this.resultCustomer);
                this.wildcard = new string[] { "a" };
            }
            else
            {
                if (CurrentUserSession.RoleId == (int)UserTypes.Master)
                {
                    if (this.type == string.Empty)
                    {
                        this.wildcard = new string[] { "m", "a" };
                        positionTaking = reportService.GetPositionTakingDataTable(null, this.custId, null, CurrentUserSession.RoleId - 1, ref this.resultCustomer);
                        this.viewType = "agent";
                    }
                    else if (this.type == "agent")
                    {
                        this.wildcard = new string[] { "m", "a" };
                        positionTaking = reportService.GetPositionTakingDataTable(null, null, this.custId, CurrentUserSession.RoleId - 2, ref this.resultCustomer);
                    }
                }
                else if (CurrentUserSession.RoleId == (int)UserTypes.Super)
                {
                    if (this.type == string.Empty)
                    {
                        this.wildcard = new string[] { "s" };
                        positionTaking = reportService.GetPositionTakingDataTable(this.custId, null, null, CurrentUserSession.RoleId - 1, ref this.resultCustomer);
                        this.viewType = "master";
                    }
                    else if (this.type == "master")
                    {
                        this.wildcard = new string[] { "m", "a" };
                        positionTaking = reportService.GetPositionTakingDataTable(null, this.custId, null, CurrentUserSession.RoleId - 2, ref this.resultCustomer);
                        this.viewType = "agent";
                    }
                    else if (this.type == "agent")
                    {
                        this.wildcard = new string[] { "s", "m", "a" };
                        positionTaking = reportService.GetPositionTakingDataTable(null, null, this.custId, CurrentUserSession.RoleId - 3, ref this.resultCustomer);
                    }
                }
            }

            view.Add(BingoPtList, positionTaking);
            return view;
        }
        #endregion

        public string GetBingoPositionTaking()
        {
            DataTable dtbPositionTaking = (View as Dictionary<string, DataTable>)[BingoPtList];
            BingoPositionTakingList_Template tpl = new BingoPositionTakingList_Template();            

            string content = string.Empty;
            int count = 1;
            string valueCustId = "-1";
            string userName = string.Empty;

            int colCount = 0;
            ICustomerService customerService = BingoContainer.Current.Resolve<ICustomerService>();
            DataTable tblGameKind = customerService.GetGameKindDataTable(BingoSettings.BingoGame.CodeGameNo, ref this.resultGameKind);
            if (tblGameKind != null && tblGameKind.Rows.Count != 0)
            {
                StringBuilder gameKind = new StringBuilder();
                StringBuilder gameType = new StringBuilder();
                StringBuilder rowData = new StringBuilder();

                BingoPositionTakingList_header_GameKind_Block tplGameKind = new BingoPositionTakingList_header_GameKind_Block();
                BingoPositionTakingList_header_GameType_Block tplGameType = new BingoPositionTakingList_header_GameType_Block();
                BingoPositionTakingList_Rows_RowData_Block tplRowData = new BingoPositionTakingList_Rows_RowData_Block();            

                int codeId = 0;
                string txtGameKind = string.Empty;
                ArrayList arrTextShowHide = new ArrayList();
                ArrayList arrCssShowHide = new ArrayList();
                DataRowCollection drcGameKind = tblGameKind.Rows;

                foreach (DataRow dr in drcGameKind)
                {
                    codeId = dr["GameGroupID"] != null ? Convert.ToInt32(dr["GameGroupID"]) : 0;
                    DataTable tblGameType = customerService.GetGameTypeDataTable(codeId, ref this.resultGameType);
                    if (tblGameType != null && tblGameType.Rows.Count != 0)
                    {
                        txtGameKind = dr[DAL.APIClient.APICasinoProvider.Lang("GameGroupName")].ToString();
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
                tpl.header = null;
            }

            // Should remove Role when wildcard.length = 1
            if (this.wildcard.Length == 1)
            {
                tpl.header.AdditionalRow = null;
                tpl.Rows.Role = null;
                tpl.header.colNum = "2";
            }
            else
            {
                tpl.header.colNum = "3";
            }

            // Provide link to edit member if in agent level
            if (this.type == "agent")
            {
                tpl.Rows.NonRepeat.ViewDownLine = null;
            }
            else
            {
                tpl.Rows.NonRepeat.EditMember = null;
            }

            if (dtbPositionTaking != null && dtbPositionTaking.Rows.Count > 0)
            {
                StringBuilder stbResult = new StringBuilder();
                string row1 = string.Empty;
                string row2 = string.Empty;
                string row3 = string.Empty;                

                string closedBg = Fanex.Age.UI.Common.RowCssClosed;
                string suspendedBg = Fanex.Age.UI.Common.RowCssSuspended;
                string evenBg = Fanex.Age.UI.Common.RowCssEven;
                string oddBg = Fanex.Age.UI.Common.RowCssOdd;
                int countPTList = dtbPositionTaking.Rows.Count;

                for (int i = 0; i < countPTList; i++)
                {
                    if (valueCustId != this.CheckValueNull(dtbPositionTaking.Rows[i]["custid"]))
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

                        userName = this.CheckValueNull(dtbPositionTaking.Rows[i]["UserName"]);
                        valueCustId = this.CheckValueNull(dtbPositionTaking.Rows[i]["custid"]);

                        // Copy Block Row
                        BingoPositionTakingList_Rows_Block tplRow = tpl.Rows;

                        if (Convert.ToInt32(dtbPositionTaking.Rows[i]["closed"]) != 0)
                        {
                            tplRow.BgRow = closedBg;
                        }
                        else if (dtbPositionTaking.Rows[i]["reject"].ToString() != string.Empty && Convert.ToInt32(dtbPositionTaking.Rows[i]["reject"]) != 0)
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
                            tplRow.NonRepeat.EditMember.custid_value = valueCustId;
                            tplRow.NonRepeat.EditMember.UserName = userName;
                        }
                        else
                        {
                            tplRow.NonRepeat.ViewDownLine.custid_value = valueCustId;
                            tplRow.NonRepeat.ViewDownLine.type = this.viewType;
                            tplRow.NonRepeat.ViewDownLine.UserName = userName;
                        }

                        // Apply data for first row
                        tplRow.NonRepeat.No = count.ToString();
                        tplRow.NonRepeat.rowspan = "rowspan='" + this.wildcard.Length.ToString() + "'";

                        if (this.wildcard.Length > 1)
                        {
                            tplRow.Role.Role = this.wildcard[0].ToUpperInvariant();
                        }

                        tplRow.NonRepeat.Show();
                        row1 = tplRow;

                        // Assign row 2 & 3.
                        if (this.wildcard.Length > 1)
                        {
                            tplRow.NonRepeat.Hide();

                            tplRow.Role.Role = this.wildcard[1].ToUpperInvariant();
                            row2 = tplRow;

                            if (this.wildcard.Length == 3)
                            {
                                tplRow.Role.Role = this.wildcard[2].ToUpperInvariant();
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
            }

            tpl.Rows.SetValue(content);
            return tpl;
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

            return row1.Replace("{" + this.CheckValueNull(dtbPositionTaking.Rows[i]["GameGroupID"]) + "_" + this.CheckValueNull(dtbPositionTaking.Rows[i]["GameTypeID"]) + "}", gameKindGameType);
        }

        private string CheckValueNull(object value)
        {
            return value != null ? value.ToString() : string.Empty;
        }
    }
}