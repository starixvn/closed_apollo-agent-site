using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using CasinoLibrary.SBAgent;
using DNAide.Core;
using Fanex.Data.DNAide;
using DNAide.Utils;
using DNAide.Web;
using Sunplus.Agent.Model.Entities.Permission;
using Microsoft.Practices.Unity;
using Sunplus.Agent.Model.Entities;

/// <summary>
/// Summary description for MemberInfo
/// </summary>
/// 
public class MemberInfo_Downline
{

    #region Bingo: Member Commission list
    public static string memberCommissionBingo(string idmaster, string idagent)
    {
        #region Define variable
        //IResult resultCustomer = new Sunplus.Bingo.Entities.Result();
        //IResult resultGameKind = new Sunplus.Bingo.Entities.Result();
        //IResult resultGameType = new Sunplus.Bingo.Entities.Result();
        string main = TemplateHandler.LoadTemplate("MemberInfo/MemberCommissionBingo.html");
        string notfound = TemplateHandler.LoadTemplate("TotalBets/NotFound.html");
        string idfirstagent = "";
        string idfirstmaster = "";
        //Create template
        string[] wildcard = new string[] { "P", "A" };
        Report rptContent = new Report();
        DataTable dataTable = null;
        string content = "";
        int count = 1;
        string _custid = "-1";
        string _username = "";
        //for regular expression
        Regex re = new Regex(@"\{\d{1,2}_\d{1,2}\}");
        MatchCollection matches;
        #endregion

        TemplateHandler.Assign(ref main, "lblCommission", Resources.Lang.lblMemberCommission);
        TemplateHandler.Assign(ref main, "SportBook", Resources.Lang.sportBook);
        TemplateHandler.Assign(ref main, "Racing", Resources.Lang.Racebook);
        TemplateHandler.Assign(ref main, "Bingo", Resources.Bingo.bingo);
        //Render Casino
        if (UserPermission.IsShowCasino)
            TemplateHandler.Assign(ref main, "Casino", Resources.Lang.casino);
        else
            TemplateHandler.RemoveBlock(ref main, "p2pCasino");

        TemplateHandler.Assign(ref main, "lblUserName", Resources.Lang.username);

        //apply class name for Role
        TemplateHandler.Assign(ref main, "firstCol", "firstCol");
        TemplateHandler.Assign(ref main, "overcolor", AppSettings.RowBgOver);

        if (UserPermission.IsShowP2P)
            TemplateHandler.Assign(ref main, "P2PText", Resources.Lang.P2P);
        else
            TemplateHandler.RemoveBlock(ref main, "P2P");

        if (UserPermission.IsShowLiveCasino)
            TemplateHandler.Assign(ref main, "LiveCasinoText", Resources.Lang.LiveCasino);
        else
            TemplateHandler.RemoveBlock(ref main, "LiveCasino");

        //Load Game Kind
        #region Load Game Kind
        string gameKind = TemplateHandler.CopyBlock(main, "GameKind");
        string gameType = TemplateHandler.CopyBlock(main, "GameType");
        string rowData = TemplateHandler.CopyBlock(main, "rowdata");
        StringBuilder _gameKind = new StringBuilder();
        StringBuilder _gameType = new StringBuilder();
        StringBuilder _rowData = new StringBuilder();
        int colCount = 0;

        //ICustomerService customerService = BingoContainer.Current.Resolve<ICustomerService>();
        //DataTable tblGameKind = customerService.GetGameKindDataTable(BingoSettings.BingoGame.CodeGameNo, ref resultGameKind);
        //if (tblGameKind != null && tblGameKind.Rows.Count != 0)
        //{
        //    //Load Game Type
        //    #region Load Game Type
        //    foreach (DataRow drGameKind in tblGameKind.Rows)
        //    {

        //        DataTable tableGameType = customerService.GetGameTypeDataTable(CastAide.AsInt32(drGameKind["GameGroupID"]), ref resultGameType);
        //        if (tableGameType != null && tableGameType.Rows.Count != 0)
        //        {
        //            foreach (DataRow drGameType in tableGameType.Rows)
        //            {
        //                string gameTypeTpl = gameType;
        //                TemplateHandler.Assign(ref gameTypeTpl, "GameTypeClass", "class='colHdr'");
        //                TemplateHandler.Assign(ref gameTypeTpl, "lblGameType", drGameType[DAL.APIClient.APICasinoProvider.Lang("GameTypeName")]);
        //                _gameType.Append(gameTypeTpl);

        //                string rowDataTpl = rowData;
        //                TemplateHandler.Assign(ref rowDataTpl, "rowdata", "{" + CastAide.AsInt32(drGameKind["GameGroupID"]).ToString() + "_" + CastAide.AsInt32(drGameType["GameTypeID"]).ToString() + "}");
        //                TemplateHandler.Assign(ref rowDataTpl, "normalCol", "#424242");
        //                _rowData.Append(rowDataTpl);

        //                //count column data
        //                colCount++;
        //            }
        //            string gameKindTpl = gameKind;
        //            TemplateHandler.Assign(ref gameKindTpl, "GameKindColSpan", "colspan='" + tableGameType.Rows.Count.ToString() + "'");
        //            TemplateHandler.Assign(ref gameKindTpl, "lblGameKind", drGameKind[DAL.APIClient.APICasinoProvider.Lang("GameGroupName")]);
        //            _gameKind.Append(gameKindTpl);
        //        }
        //    }
        //    #endregion

        //    TemplateHandler.ReplaceBlock(ref main, "GameKind", _gameKind.ToString());
        //    TemplateHandler.ReplaceBlock(ref main, "GameType", _gameType.ToString());
        //    TemplateHandler.ReplaceBlock(ref main, "rowdata", _rowData.ToString());
        //}
        //else
        //{
            //remove header
            TemplateHandler.RemoveBlock(ref main, "header");
        //}
        #endregion

        #region Load Data Row

        //IReportService reportService = BingoContainer.Current.Resolve<IReportService>();
        if (AppSettings.IsAgentSite)
        {
            TemplateHandler.Assign(ref main, "appliesallbettype", Resources.Lang.lblA + ", " + Resources.Lang.lblMe);
            TemplateHandler.RemoveBlock(ref main, "limit-agent");
            wildcard = new string[] { "P", "A" };
            //dataTable = reportService.GetMemberCommDataTable(null, null, Sunplus.Agent.Model.Entities.UserSession.Current.CustId, ref resultCustomer);
        }
        else if (AppSettings.IsMasterSite)
        {
            TemplateHandler.Assign(ref main, "appliesallbettype", Resources.Lang.lblM + ", " + Resources.Lang.lblA + ", " + Resources.Lang.lblMe);
            TemplateHandler.RemoveBlock(ref main, "limit-master");
            TemplateHandler.Assign(ref main, "selagent", Resources.Lang.selectagent);
            string optionAgent = GetOptionAgentBingo(Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString(), idagent, ref idfirstagent);
            if (idfirstagent == "")
                TemplateHandler.RemoveBlock(ref main, "limit-agent");
            else
            {
                TemplateHandler.Assign(ref main, "option_agent", optionAgent);
                wildcard = new string[] { "P", "A", "M" };
                //if (idagent == "")
                //    dataTable = reportService.GetMemberCommDataTable(null, CastAide.AsInt32(Sunplus.Agent.Model.Entities.UserSession.Current.CustId), CastAide.AsInt32(idfirstagent), ref resultCustomer);    
                //else
                //    dataTable = reportService.GetMemberCommDataTable(null, CastAide.AsInt32(Sunplus.Agent.Model.Entities.UserSession.Current.CustId), CastAide.AsInt32(idagent), ref resultCustomer);    
            }
        }
        else
        {
            TemplateHandler.Assign(ref main, "appliesallbettype", Resources.Lang.lblM + ", " + Resources.Lang.lblA + ", " + Resources.Lang.lblMe);
            TemplateHandler.Assign(ref main, "selmaster", Resources.Lang.selectmaster);
            TemplateHandler.Assign(ref main, "selagent", Resources.Lang.selectagent);
            string optionMaster = GetOptionMasterBingo(Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString(), ref idfirstmaster, idmaster);
            string optionAgent = "";

            if (idfirstmaster == "")
            {
                TemplateHandler.RemoveBlock(ref main, "limit-agent");
            }
            else
            {
                if (idmaster != "")
                    optionAgent = GetOptionAgentBingo(idmaster, idagent, ref idfirstagent);
                else
                    optionAgent = GetOptionAgentBingo(idfirstmaster, idagent, ref idfirstagent);

                TemplateHandler.Assign(ref main, "option_agent", optionAgent);
                TemplateHandler.Assign(ref main, "option_master", optionMaster);

                wildcard = new string[] { "P", "A", "M" };
                //if (idmaster != "")
                //{
                //    if (idmaster != idagent && idagent != "")
                //        dataTable = reportService.GetMemberCommDataTable(Sunplus.Agent.Model.Entities.UserSession.Current.CustId, CastAide.AsInt32(idmaster), CastAide.AsInt32(idagent), ref resultCustomer);
                //    else
                //        dataTable = reportService.GetMemberCommDataTable(Sunplus.Agent.Model.Entities.UserSession.Current.CustId, CastAide.AsInt32(idmaster), null, ref resultCustomer);
                //}
                //else
                //    dataTable = reportService.GetMemberCommDataTable(Sunplus.Agent.Model.Entities.UserSession.Current.CustId, CastAide.AsInt32(idfirstmaster), null, ref resultCustomer);
            }
        }

        // Load DataTable
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            string rowTpl = TemplateHandler.CopyBlock(main, "row");
            StringBuilder _s = new StringBuilder();
            string _row = string.Empty;
            string _row2 = string.Empty;
            string _row3 = string.Empty;

            #region Apply Comm from API
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (_custid != CastAide.AsString(dataTable.Rows[i]["custid"]))
                {
                    //begin another user
                    if (re.IsMatch(_row))
                    {
                        matches = re.Matches(_row);
                        foreach (Match match in matches)
                        {
                            TemplateHandler.Assign(ref _row, match.Value, "-");
                        }

                    }
                    _s.Append(_row);


                    if (re.IsMatch(_row2))
                    {
                        matches = re.Matches(_row2);
                        foreach (Match match in matches)
                        {
                            TemplateHandler.Assign(ref _row2, match.Value, "-");
                        }

                    }
                    _s.Append(_row2);

                    if (wildcard.Length == 3)
                    {
                        if (re.IsMatch(_row3))
                        {
                            matches = re.Matches(_row3);
                            foreach (Match match in matches)
                            {
                                TemplateHandler.Assign(ref _row3, match.Value, "-");
                            }

                        }

                        _s.Append(_row3);
                    }
                    _username = CastAide.AsString(dataTable.Rows[i]["UserName"]);
                    _custid = CastAide.AsString(dataTable.Rows[i]["custid"]);

                    _row = rowTpl;
                    if (count % 2 != 0) //Apply Odd-Even Row
                        TemplateHandler.Assign(ref _row, "color", AppSettings.RowBgEven);
                    else
                        TemplateHandler.Assign(ref _row, "color", AppSettings.RowBgOdd);

                    //apply data for first row
                    TemplateHandler.Assign(ref _row, "No", count);
                    count++;
                    TemplateHandler.Assign(ref _row, "UserName_vl", CastAide.AsString(_username));
                    TemplateHandler.Assign(ref _row, "rowspan", "rowspan='" + wildcard.Length.ToString() + "'");

                    _row2 = _row;
                    //Remove NonRepeat td for second row
                    TemplateHandler.RemoveBlock(ref _row2, "NonRepeat");
                    if (wildcard.Length == 3)
                    {
                        _row3 = _row;
                        TemplateHandler.RemoveBlock(ref _row3, "NonRepeat");
                        TemplateHandler.Assign(ref _row3, "Role", wildcard[2].ToUpperInvariant());
                    }
                    TemplateHandler.Assign(ref _row, "Role", wildcard[0].ToUpperInvariant());
                    TemplateHandler.Assign(ref _row2, "Role", wildcard[1].ToUpperInvariant());
                }

                //Apply position taking data from API
                //First Row
                TemplateHandler.Assign(ref _row, CastAide.AsString(dataTable.Rows[i]["GameGroupID"]) + "_" + CastAide.AsString(dataTable.Rows[i]["GameTypeID"]), NumberFormat.DecFormat(dataTable.Rows[i]["MemberRollingRate"], 4));
                //second Row
                TemplateHandler.Assign(ref _row2, CastAide.AsString(dataTable.Rows[i]["GameGroupID"]) + "_" + CastAide.AsString(dataTable.Rows[i]["GameTypeID"]), NumberFormat.DecFormat(dataTable.Rows[i]["AgentRollingRate"], 4));
                if (AppSettings.IsSuperSite || AppSettings.IsMasterSite)
                {
                    //third Row
                    TemplateHandler.Assign(ref _row3, CastAide.AsString(dataTable.Rows[i]["GameGroupID"]) + "_" + CastAide.AsString(dataTable.Rows[i]["GameTypeID"]), NumberFormat.DecFormat(dataTable.Rows[i]["MAgentRollingRate"], 4));
                }
            }
            //for end of row --> append data to string builder : if (i == dataTable.Rows.Count - 1)
            if (dataTable.Rows.Count > 0)
            {
                if (re.IsMatch(_row))
                {
                    matches = re.Matches(_row);
                    foreach (Match match in matches)
                    {
                        TemplateHandler.Assign(ref _row, match.Value, "-");
                    }

                }
                _s.Append(_row);


                if (re.IsMatch(_row2))
                {
                    matches = re.Matches(_row2);
                    foreach (Match match in matches)
                    {
                        TemplateHandler.Assign(ref _row2, match.Value, "-");
                    }

                }
                _s.Append(_row2);


                if (wildcard.Length == 3)
                {
                    if (re.IsMatch(_row3))
                    {
                        matches = re.Matches(_row3);
                        foreach (Match match in matches)
                        {
                            TemplateHandler.Assign(ref _row3, match.Value, "-");
                        }
                    }
                    _s.Append(_row3);
                }
            }

            content = _s.ToString();
            #endregion
        }
        else
        {
            if (wildcard.Length > 1)
                colCount = colCount + 5;
            else
                colCount = colCount + 4;
            TemplateHandler.Assign(ref notfound, "notfound", Resources.Lang.noInfo);
            TemplateHandler.Assign(ref notfound, "numcolumn", colCount);
            content = notfound;
        }
        #endregion

        TemplateHandler.ReplaceBlock(ref main, "row", content);
        return main;
    }
    #endregion

    #region Casino: Member Commission list
    public static string memberCommissionCasino(string idmaster, string idagent)
    {
        string main = TemplateHandler.LoadTemplate("MemberInfo/MemberCommissionCasino.html");
        string notfound = TemplateHandler.LoadTemplate("TotalBets/NotFound.html");

        string idfirstagent = "";
        string idfirstmaster = "";

        //for regular expression
        Regex re = new Regex(@"\{\d{1,2}_\d{1,2}\}");
        MatchCollection matches;

        TemplateHandler.Assign(ref main, "lblCommission", Resources.Lang.lblMemberCommission);
        TemplateHandler.Assign(ref main, "SportBook", Resources.Lang.sportBook);
        TemplateHandler.Assign(ref main, "Racing", Resources.Lang.Racebook);
        TemplateHandler.Assign(ref main, "Casino", Resources.Lang.casino);
        //Render Bingo
        //if (UserPermission.IsShowBingo && !Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoCommission)
        //{
        //    TemplateHandler.Assign(ref main, "Bingo", Resources.Bingo.bingo);
        //}
        //else
        //{
            TemplateHandler.RemoveBlock(ref main, "p2pBingo");
        //}

        if (UserPermission.IsShowP2P)
            TemplateHandler.Assign(ref main, "P2PText", Resources.Lang.P2P);
        else
            TemplateHandler.RemoveBlock(ref main, "P2P");

        if (UserPermission.IsShowLiveCasino)
            TemplateHandler.Assign(ref main, "LiveCasinoText", Resources.Lang.LiveCasino);
        else
            TemplateHandler.RemoveBlock(ref main, "LiveCasino");

        //Create template
        string[] wildcard = new string[] { "P", "A" };
        Report rptContent = new Report();
        DataTable dataTable = null;
        string content = "";
        int count = 1;
        string _custid = "-1";
        string _username = "";

        TemplateHandler.Assign(ref main, "lblUserName", Resources.Lang.username);

        //apply class name for Role
        TemplateHandler.Assign(ref main, "firstCol", "firstCol");
        TemplateHandler.Assign(ref main, "overcolor", AppSettings.RowBgOver);

        //Load Game Kind
        #region Load Game Kind
        string gameKind = TemplateHandler.CopyBlock(main, "GameKind");
        string gameType = TemplateHandler.CopyBlock(main, "GameType");
        string rowData = TemplateHandler.CopyBlock(main, "rowdata");
        StringBuilder _gameKind = new StringBuilder();
        StringBuilder _gameType = new StringBuilder();
        StringBuilder _rowData = new StringBuilder();
        int colCount = 0;
        BasicInfo bi = new BasicInfo();
        DataTable tblGameKind = bi.GetGameKind("01");
        if (tblGameKind != null && tblGameKind.Rows.Count != 0)
        {
            //Load Game Type
            #region Load Game Type
            foreach (DataRow dr in tblGameKind.Rows)
            {
                DataTable tblGameType = bi.GetGameType(CastAide.AsInt32(dr["CodeID"]));
                if (tblGameType != null && tblGameType.Rows.Count != 0)
                {
                    foreach (DataRow dr2 in tblGameType.Rows)
                    {
                        string gameTypeTpl = gameType;
                        TemplateHandler.Assign(ref gameTypeTpl, "GameTypeClass", "class='colHdr'");
                        TemplateHandler.Assign(ref gameTypeTpl, "lblGameType", dr2[DAL.APIClient.APICasinoProvider.Lang("GameTypeName")]);
                        _gameType.Append(gameTypeTpl);

                        string rowDataTpl = rowData;
                        TemplateHandler.Assign(ref rowDataTpl, "rowdata", "{" + CastAide.AsInt32(dr["CodeID"]).ToString() + "_" + CastAide.AsInt32(dr2["GameTypeID"]).ToString() + "}");
                        TemplateHandler.Assign(ref rowDataTpl, "normalCol", "#424242");
                        _rowData.Append(rowDataTpl);

                        //count column data
                        colCount++;
                    }
                    string gameKindTpl = gameKind;
                    TemplateHandler.Assign(ref gameKindTpl, "GameKindColSpan", "colspan='" + tblGameType.Rows.Count.ToString() + "'");
                    TemplateHandler.Assign(ref gameKindTpl, "lblGameKind", dr[DAL.APIClient.APICasinoProvider.Lang("CodeName")]);
                    _gameKind.Append(gameKindTpl);
                }
            }
            #endregion

            TemplateHandler.ReplaceBlock(ref main, "GameKind", _gameKind.ToString());
            TemplateHandler.ReplaceBlock(ref main, "GameType", _gameType.ToString());
            TemplateHandler.ReplaceBlock(ref main, "rowdata", _rowData.ToString());
        }
        else
        {
            //remove header
            TemplateHandler.RemoveBlock(ref main, "header");
        }
        #endregion

        #region Load Data Row
        // Get DataTable
        if (AppSettings.IsAgentSite)
        {
            TemplateHandler.Assign(ref main, "appliesallbettype", Resources.Lang.lblA + ", " + Resources.Lang.lblMe);
            TemplateHandler.RemoveBlock(ref main, "limit-agent");
            wildcard = new string[] { "P", "A" };
            dataTable = rptContent.MemberCommission(-1, -1, CastAide.AsInt32(Sunplus.Agent.Model.Entities.UserSession.Current.CustId));
        }
        else if (AppSettings.IsMasterSite)
        {
            TemplateHandler.Assign(ref main, "appliesallbettype", Resources.Lang.lblM + ", " + Resources.Lang.lblA + ", " + Resources.Lang.lblMe);
            TemplateHandler.RemoveBlock(ref main, "limit-master");
            TemplateHandler.Assign(ref main, "selagent", Resources.Lang.selectagent);
            string optionAgent = GetOptionAgentCasino(Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString(), idagent, ref idfirstagent);
            if (idfirstagent == "")
                TemplateHandler.RemoveBlock(ref main, "limit-agent");
            else
            {
                TemplateHandler.Assign(ref main, "option_agent", optionAgent);
                wildcard = new string[] { "P", "A", "M" };
                if (idagent == "")
                    dataTable = rptContent.MemberCommission(-1, CastAide.AsInt32(Sunplus.Agent.Model.Entities.UserSession.Current.CustId), CastAide.AsInt32(idfirstagent));
                else
                    dataTable = rptContent.MemberCommission(-1, CastAide.AsInt32(Sunplus.Agent.Model.Entities.UserSession.Current.CustId), CastAide.AsInt32(idagent));
            }


        }
        else
        {
            TemplateHandler.Assign(ref main, "appliesallbettype", Resources.Lang.lblM + ", " + Resources.Lang.lblA + ", " + Resources.Lang.lblMe);
            TemplateHandler.Assign(ref main, "selmaster", Resources.Lang.selectmaster);
            TemplateHandler.Assign(ref main, "selagent", Resources.Lang.selectagent);
            string optionMaster = GetOptionMasterCasino(Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString(), ref idfirstmaster, idmaster);
            string optionAgent = "";

            if (idfirstmaster == "")
            {
                TemplateHandler.RemoveBlock(ref main, "limit-agent");
            }
            else
            {
                if (idmaster != "")
                    optionAgent = GetOptionAgentCasino(idmaster, idagent, ref idfirstagent);
                else
                    optionAgent = GetOptionAgentCasino(idfirstmaster, idagent, ref idfirstagent);

                TemplateHandler.Assign(ref main, "option_agent", optionAgent);
                TemplateHandler.Assign(ref main, "option_master", optionMaster);

                wildcard = new string[] { "P", "A", "M" };
                if (idmaster != "")
                {
                    if (idmaster != idagent && idagent != "")
                        dataTable = rptContent.MemberCommission(CastAide.AsInt32(Sunplus.Agent.Model.Entities.UserSession.Current.CustId), CastAide.AsInt32(idmaster), CastAide.AsInt32(idagent));
                    else
                        dataTable = rptContent.MemberCommission(CastAide.AsInt32(Sunplus.Agent.Model.Entities.UserSession.Current.CustId), CastAide.AsInt32(idmaster), -1);
                }
                else
                    dataTable = rptContent.MemberCommission(CastAide.AsInt32(Sunplus.Agent.Model.Entities.UserSession.Current.CustId), CastAide.AsInt32(idfirstmaster), -1);
            }


        }

        // Load DataTable
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            string rowTpl = TemplateHandler.CopyBlock(main, "row");
            StringBuilder _s = new StringBuilder();
            string _row = string.Empty;
            string _row2 = string.Empty;
            string _row3 = string.Empty;

            #region Apply Comm from API
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (_custid != CastAide.AsString(dataTable.Rows[i]["custid"]))
                {
                    //begin another user
                    if (re.IsMatch(_row))
                    {
                        matches = re.Matches(_row);
                        foreach (Match match in matches)
                        {
                            TemplateHandler.Assign(ref _row, match.Value, "-");
                        }

                    }
                    _s.Append(_row);


                    if (re.IsMatch(_row2))
                    {
                        matches = re.Matches(_row2);
                        foreach (Match match in matches)
                        {
                            TemplateHandler.Assign(ref _row2, match.Value, "-");
                        }

                    }
                    _s.Append(_row2);

                    if (wildcard.Length == 3)
                    {
                        if (re.IsMatch(_row3))
                        {
                            matches = re.Matches(_row3);
                            foreach (Match match in matches)
                            {
                                TemplateHandler.Assign(ref _row3, match.Value, "-");
                            }

                        }

                        _s.Append(_row3);
                    }

                    _username = CastAide.AsString(dataTable.Rows[i]["UserName"]);
                    _custid = CastAide.AsString(dataTable.Rows[i]["custid"]);

                    _row = rowTpl;
                    if (count % 2 != 0) //Apply Odd-Even Row
                        TemplateHandler.Assign(ref _row, "color", AppSettings.RowBgEven);
                    else
                        TemplateHandler.Assign(ref _row, "color", AppSettings.RowBgOdd);

                    //apply data for first row
                    TemplateHandler.Assign(ref _row, "No", count);
                    count++;
                    TemplateHandler.Assign(ref _row, "UserName_vl", CastAide.AsString(_username));
                    TemplateHandler.Assign(ref _row, "rowspan", "rowspan='" + wildcard.Length.ToString() + "'");

                    _row2 = _row;
                    //Remove NonRepeat td for second row
                    TemplateHandler.RemoveBlock(ref _row2, "NonRepeat");
                    if (wildcard.Length == 3)
                    {
                        _row3 = _row;
                        TemplateHandler.RemoveBlock(ref _row3, "NonRepeat");
                        TemplateHandler.Assign(ref _row3, "Role", wildcard[2].ToUpperInvariant());
                    }
                    TemplateHandler.Assign(ref _row, "Role", wildcard[0].ToUpperInvariant());
                    TemplateHandler.Assign(ref _row2, "Role", wildcard[1].ToUpperInvariant());
                }

                //Apply position taking data from API
                //First Row
                TemplateHandler.Assign(ref _row, CastAide.AsString(dataTable.Rows[i]["GameKind"]) + "_" + CastAide.AsString(dataTable.Rows[i]["GameTypeID"]), NumberFormat.DecFormat(dataTable.Rows[i]["MemberRollingRate"], 4));
                //second Row
                TemplateHandler.Assign(ref _row2, CastAide.AsString(dataTable.Rows[i]["GameKind"]) + "_" + CastAide.AsString(dataTable.Rows[i]["GameTypeID"]), NumberFormat.DecFormat(dataTable.Rows[i]["AgentRollingRate"], 4));
                if (AppSettings.IsSuperSite || AppSettings.IsMasterSite)
                {
                    //third Row
                    TemplateHandler.Assign(ref _row3, CastAide.AsString(dataTable.Rows[i]["GameKind"]) + "_" + CastAide.AsString(dataTable.Rows[i]["GameTypeID"]), NumberFormat.DecFormat(dataTable.Rows[i]["MAgentRollingRate"], 4));
                }
            }
            //for end of row --> append data to string builder : if (i == dataTable.Rows.Count - 1)
            if (dataTable.Rows.Count > 0)
            {
                if (re.IsMatch(_row))
                {
                    matches = re.Matches(_row);
                    foreach (Match match in matches)
                    {
                        TemplateHandler.Assign(ref _row, match.Value, "-");
                    }

                }
                _s.Append(_row);


                if (re.IsMatch(_row2))
                {
                    matches = re.Matches(_row2);
                    foreach (Match match in matches)
                    {
                        TemplateHandler.Assign(ref _row2, match.Value, "-");
                    }

                }
                _s.Append(_row2);


                if (wildcard.Length == 3)
                {
                    if (re.IsMatch(_row3))
                    {
                        matches = re.Matches(_row3);
                        foreach (Match match in matches)
                        {
                            TemplateHandler.Assign(ref _row3, match.Value, "-");
                        }

                    }
                    _s.Append(_row3);
                }
            }

            content = _s.ToString();
            #endregion
        }
        else
        {
            if (wildcard.Length > 1)
                colCount = colCount + 5;
            else
                colCount = colCount + 4;
            TemplateHandler.Assign(ref notfound, "notfound", Resources.Lang.noInfo);
            TemplateHandler.Assign(ref notfound, "numcolumn", colCount);
            content = notfound;
        }
        #endregion

        TemplateHandler.ReplaceBlock(ref main, "row", content);

        return main;
    }
    #endregion

    #region Member Commission List
    public static string memberCommission(string idmaster, string idagent)
    {
        string strmain = TemplateHandler.LoadTemplate("MemberInfo/MemberCommission.html");        
        string idfirstagent = "";
        string idfirstmaster = "";
        int rowcount = 0;
        int colspancomm = 3;
        string strExplainComm = "";
        strExplainComm = "<li>" + Resources.Lang.lblM + ", " + Resources.Lang.lblA + ", " + Resources.Lang.lblMe + "</li>";
        TemplateHandler.Assign(ref strmain, "lblCommission", Resources.Lang.lblMemberCommission);

        //Simon : Render Casino
        if (UserPermission.IsShowCasino)
            TemplateHandler.Assign(ref strmain, "Casino", Resources.Lang.casino);
        else
            TemplateHandler.RemoveBlock(ref strmain, "p2pCasino");
        
        //Render Bingo
        //if (UserPermission.IsShowBingo && !Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoCommission)
        //    TemplateHandler.Assign(ref strmain, "Bingo", Resources.Bingo.bingo);
        //else
        TemplateHandler.RemoveBlock(ref strmain, "p2pBingo");

        TemplateHandler.Assign(ref strmain, "SportBook", Resources.Lang.sportBook);
        TemplateHandler.Assign(ref strmain, "Racing", Resources.Lang.Racebook);

        //If set enable number commission on webconfig
        if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGame)
            TemplateHandler.RemoveBlock(ref strmain, "number-comm");
        else if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission)
            TemplateHandler.RemoveBlock(ref strmain, "number-comm");
        else
            TemplateHandler.Assign(ref strmain, "lblNumberComm", Resources.Lang.numbergamecomm);
        //phuong.nguyen, render HR

        if (UserPermission.IsShowP2P)
            TemplateHandler.Assign(ref strmain, "P2PText", Resources.Lang.P2P);
        else
            TemplateHandler.RemoveBlock(ref strmain, "P2P");

        if (UserPermission.IsShowLiveCasino)
            TemplateHandler.Assign(ref strmain, "LiveCasinoText", Resources.Lang.LiveCasino);
        else
            TemplateHandler.RemoveBlock(ref strmain, "LiveCasino");

        TemplateHandler.Assign(ref strmain, "lblComm", Resources.Lang.com);        
        if (AppSettings.IsAgentSite)
        {
            colspancomm = 2;
            TemplateHandler.RemoveBlock(ref strmain, "mcomm1");
            TemplateHandler.RemoveBlock(ref strmain, "mcomm2");

            TemplateHandler.RemoveBlock(ref strmain, "limit-agent");
            TemplateHandler.ReplaceBlock(ref strmain, "limit-content", GetDataMemberCommissionAgent(Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString(), ref rowcount));

            strExplainComm = "<li>" + Resources.Lang.lblA + ", " + Resources.Lang.lblMe + "</li>";
        }
        else if (AppSettings.IsMasterSite)
        {
            TemplateHandler.Assign(ref strmain, "lblM", Resources.Lang.lblM);
            TemplateHandler.RemoveBlock(ref strmain, "limit-master");
            TemplateHandler.Assign(ref strmain, "selagent", Resources.Lang.selectagent);
            string optionAgent = GetOptionAgent(Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString(), idagent, ref idfirstagent);
            TemplateHandler.Assign(ref strmain, "option_agent", optionAgent);
            if (idagent == "")
            {
                TemplateHandler.ReplaceBlock(ref strmain, "limit-content", GetDataMemberCommissionAgent(idfirstagent, ref rowcount));
            }
            else
            {
                TemplateHandler.ReplaceBlock(ref strmain, "limit-content", GetDataMemberCommissionAgent(idagent, ref rowcount));
            }
        }
        else
        {
            TemplateHandler.Assign(ref strmain, "lblM", Resources.Lang.lblM);
            TemplateHandler.Assign(ref strmain, "selmaster", Resources.Lang.selectmaster);
            TemplateHandler.Assign(ref strmain, "selagent", Resources.Lang.selectagent);

            string optionMaster = GetOptionMaster(Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString(), ref idfirstmaster, idmaster);
            string optionAgent = "";
            if (idmaster != "")
            {
                optionAgent = GetOptionAgent(idmaster, idagent, ref idfirstagent);
            }
            else
            {
                optionAgent = GetOptionAgent(idfirstmaster, idagent, ref idfirstagent);
            }
            TemplateHandler.Assign(ref strmain, "option_agent", optionAgent);
            TemplateHandler.Assign(ref strmain, "option_master", optionMaster);
            if (idmaster != idagent)
            {
                if (idagent == "")
                {
                    TemplateHandler.ReplaceBlock(ref strmain, "limit-content", GetDataMemberCommissionAgent(idfirstagent, ref rowcount));
                }
                else
                {
                    TemplateHandler.ReplaceBlock(ref strmain, "limit-content", GetDataMemberCommissionAgent(idagent, ref rowcount));
                }
            }
            else
            {
                if (idagent == "")
                {
                    TemplateHandler.ReplaceBlock(ref strmain, "limit-content", GetDataMemberCommissionMaster(idfirstmaster, ref rowcount));
                }
                else
                {
                    TemplateHandler.ReplaceBlock(ref strmain, "limit-content", GetDataMemberCommissionMaster(idagent, ref rowcount));
                }
            }
        }
        TemplateHandler.Assign(ref strmain, "lblA", Resources.Lang.lblA);
        TemplateHandler.Assign(ref strmain, "lblMe", Resources.Lang.lblMe);

        strExplainComm = Resources.Lang.appliesallbettype + strExplainComm;
        TemplateHandler.Assign(ref strmain, "appliesallbettype", strExplainComm);
        TemplateHandler.Assign(ref strmain, "view", Resources.Lang.viewgroupcommission);
        TemplateHandler.Assign(ref strmain, "no", Resources.Lang.no);
        TemplateHandler.Assign(ref strmain, "username", Resources.Lang.username);
        TemplateHandler.Assign(ref strmain, "firstname", Resources.Lang.firstname);
        TemplateHandler.Assign(ref strmain, "lastname", Resources.Lang.lastname);
        TemplateHandler.Assign(ref strmain, "group", Resources.Lang.group);
        TemplateHandler.Assign(ref strmain, "credit", Resources.Lang.credit);
        TemplateHandler.Assign(ref strmain, "colspancomm", colspancomm.ToString());
        TemplateHandler.Assign(ref strmain, "rowcount", rowcount);

        return strmain;
    }
    #endregion

    #region Racing: Member Commission List
    public static string MemberCommissionForRacing(string idmaster, string idagent)
    {
        string strmain = TemplateHandler.LoadTemplate("MemberInfo/MemberCommissionRacing.html");
        if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowHRCurrency)
            TemplateHandler.RemoveBlock(ref strmain, "SAHorse");
        string idfirstagent = "";
        string idfirstmaster = "";
        int rowcount = 0;
        int colspancomm = 3;
        string strExplainComm = "";
        strExplainComm = "<li>" + Resources.Lang.lblM + ", " + Resources.Lang.lblA + ", " + Resources.Lang.lblMe + "</li>";
        TemplateHandler.Assign(ref strmain, "lblCommission", Resources.Lang.lblMemberCommission);

        TemplateHandler.Assign(ref strmain, "lblCommission", Resources.Lang.lblMemberCommission);
        TemplateHandler.Assign(ref strmain, "SportBook", Resources.Lang.sportBook);
        TemplateHandler.Assign(ref strmain, "Racing", Resources.Lang.Racebook);

        // Render Casino
        if (UserPermission.IsShowCasino)
            TemplateHandler.Assign(ref strmain, "Casino", Resources.Lang.casino);
        else
            TemplateHandler.RemoveBlock(ref strmain, "p2pCasino");
        
        // Render Bingo
        //if (UserPermission.IsShowBingo && !Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoCommission)
        //    TemplateHandler.Assign(ref strmain, "Bingo", Resources.Bingo.bingo);
        //else
        TemplateHandler.RemoveBlock(ref strmain, "p2pBingo");

        if (UserPermission.IsShowP2P)
            TemplateHandler.Assign(ref strmain, "P2PText", Resources.Lang.P2P);
        else
            TemplateHandler.RemoveBlock(ref strmain, "P2P");

        if (UserPermission.IsShowLiveCasino)
            TemplateHandler.Assign(ref strmain, "LiveCasinoText", Resources.Lang.LiveCasino);
        else
            TemplateHandler.RemoveBlock(ref strmain, "LiveCasino");

        TemplateHandler.Assign(ref strmain, "lblComm", Resources.Lang.com);
        TemplateHandler.Assign(ref strmain, "lblSAHorse", Resources.Racing.hrSaTote);
        TemplateHandler.Assign(ref strmain, "lblUKHorse", Resources.Racing.hrUkTote);
        TemplateHandler.Assign(ref strmain, "lblAUHorse", Resources.Racing.hrAUTote);
        TemplateHandler.Assign(ref strmain, "lblUKGreyhound", Resources.Racing.GreyhoundUK);
        TemplateHandler.Assign(ref strmain, "lblAUGreyhound", Resources.Racing.GreyhoundAU);
        TemplateHandler.Assign(ref strmain, "lblAUHarness", Resources.Racing.HarnessAU);
        if (AppSettings.IsAgentSite)
        {
            colspancomm = 2;
            TemplateHandler.RemoveBlock(ref strmain, "IsAgentSite");
            TemplateHandler.RemoveBlock(ref strmain, "limit-agent");
            TemplateHandler.ReplaceBlock(ref strmain, "limit-content", GetDataMemberCommissionAgentForRacing(Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString(), ref rowcount));
            strExplainComm = "<li>" + Resources.Lang.lblA + ", " + Resources.Lang.lblMe + "</li>";
        }
        else if (AppSettings.IsMasterSite)
        {
            TemplateHandler.Assign(ref strmain, "lblM", Resources.Lang.lblM);
            TemplateHandler.RemoveBlock(ref strmain, "limit-master");
            TemplateHandler.Assign(ref strmain, "selagent", Resources.Lang.selectagent);
            string optionAgent = GetOptionAgent(Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString(), idagent, ref idfirstagent);
            TemplateHandler.Assign(ref strmain, "option_agent", optionAgent);
            if (idagent == "")
            {
                TemplateHandler.ReplaceBlock(ref strmain, "limit-content", GetDataMemberCommissionAgentForRacing(idfirstagent, ref rowcount));
            }
            else
            {
                TemplateHandler.ReplaceBlock(ref strmain, "limit-content", GetDataMemberCommissionAgentForRacing(idagent, ref rowcount));
            }
        }
        else
        {
            TemplateHandler.Assign(ref strmain, "lblM", Resources.Lang.lblM);
            TemplateHandler.Assign(ref strmain, "selmaster", Resources.Lang.selectmaster);
            TemplateHandler.Assign(ref strmain, "selagent", Resources.Lang.selectagent);

            string optionMaster = GetOptionMaster(Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString(), ref idfirstmaster, idmaster);
            string optionAgent = "";
            if (idmaster != "")
            {
                optionAgent = GetOptionAgent(idmaster, idagent, ref idfirstagent);
            }
            else
            {
                optionAgent = GetOptionAgent(idfirstmaster, idagent, ref idfirstagent);
            }
            TemplateHandler.Assign(ref strmain, "option_agent", optionAgent);
            TemplateHandler.Assign(ref strmain, "option_master", optionMaster);
            if (idmaster != idagent)
            {
                if (idagent == "")
                {
                    TemplateHandler.ReplaceBlock(ref strmain, "limit-content", GetDataMemberCommissionAgentForRacing(idfirstagent, ref rowcount));
                }
                else
                {
                    TemplateHandler.ReplaceBlock(ref strmain, "limit-content", GetDataMemberCommissionAgentForRacing(idagent, ref rowcount));
                }
            }
            else
            {
                if (idagent == "")
                {
                    TemplateHandler.ReplaceBlock(ref strmain, "limit-content", GetDataMemberCommissionMasterForRacing(idfirstmaster, ref rowcount));
                }
                else
                {
                    TemplateHandler.ReplaceBlock(ref strmain, "limit-content", GetDataMemberCommissionMasterForRacing(idagent, ref rowcount));
                }
            }
        }
        TemplateHandler.Assign(ref strmain, "lblA", Resources.Lang.lblA);
        TemplateHandler.Assign(ref strmain, "lblMe", Resources.Lang.lblMe);

        TemplateHandler.Assign(ref strmain, "appliesallbettype", strExplainComm);
        TemplateHandler.Assign(ref strmain, "view", Resources.Lang.viewgroupcommission);
        TemplateHandler.Assign(ref strmain, "no", Resources.Lang.no);
        TemplateHandler.Assign(ref strmain, "username", Resources.Lang.username);
        TemplateHandler.Assign(ref strmain, "firstname", Resources.Lang.firstname);
        TemplateHandler.Assign(ref strmain, "lastname", Resources.Lang.lastname);
        TemplateHandler.Assign(ref strmain, "credit", Resources.Lang.credit);
        TemplateHandler.Assign(ref strmain, "colspancomm", colspancomm.ToString());
        TemplateHandler.Assign(ref strmain, "rowcount", rowcount);

        return strmain;
    }
    #endregion

    #region Generate Member Commission by Agent
    public static string GetDataMemberCommissionAgent(string id, ref int rowcount)
    {
        string strmain = TemplateHandler.LoadTemplate("MemberInfo/MemberCommission.html");        
        if (AppSettings.IsAgentSite)
        {
            TemplateHandler.RemoveBlock(ref strmain, "mcomm1_value");
            TemplateHandler.RemoveBlock(ref strmain, "mcomm2_value");
            TemplateHandler.RemoveBlock(ref strmain, "mcomm3_value");
            TemplateHandler.RemoveBlock(ref strmain, "mnumber_comm_value");
        }
        string notfound = TemplateHandler.LoadTemplate("NoInfo.html");
        string copystrmain = TemplateHandler.CopyBlock(strmain, "limit-content");
        string resstrmain = "";
        StringBuilder result = new StringBuilder();
        int counter = 0;
        
        //If set enable number commission on webconfig
        if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission)
        {
            TemplateHandler.RemoveBlock(ref copystrmain, "number-comm");
        }

        TemplateHandler.Assign(ref copystrmain, "lblComm", Resources.Lang.com);

        if (id != "")
        {
            DbResult dbr = MemberInfo.GetDataAgent_WithAgent_MemberComm(id);
            DataTable dtb = dbr.ResultSet.Tables[0];

            TemplateHandler.Assign(ref copystrmain, "overcolor", AppSettings.RowBgOver);
            string OddEven = AppSettings.RowBgEven;
            if (dtb.Rows.Count > 0)
            {
                //for (int i = 0; i < dtb.Rows.Count; i++)                
                foreach (DataRow dr in dtb.Rows)
                {
                    counter++;
                    resstrmain = copystrmain;
                    TemplateHandler.Assign(ref resstrmain, "overcolor", AppSettings.RowBgOver);
                    if (Convert.ToInt32(dr["enabled"]) == 1)
                    {
                        if (Convert.ToInt32(dr["closed"]) != 0)
                            TemplateHandler.Assign(ref resstrmain, "color", AppSettings.RowBgClosed);
                        else if (Convert.ToInt32(dr["suspended"]) != 0)
                            TemplateHandler.Assign(ref resstrmain, "color", AppSettings.RowBgSuspended);
                        else
                        {
                            OddEven = (OddEven == AppSettings.RowBgEven) ? AppSettings.RowBgOdd : AppSettings.RowBgEven;
                            TemplateHandler.Assign(ref resstrmain, "color", OddEven);
                        }
                    }
                    else
                    {
                        TemplateHandler.Assign(ref resstrmain, "color", AppSettings.RowBgDisabled);
                    }
                    TemplateHandler.Assign(ref resstrmain, "no_value", counter.ToString());
                    TemplateHandler.Assign(ref resstrmain, "username_value", dr["username"].ToString());
                    TemplateHandler.Assign(ref resstrmain, "firstname_value", dr["firstname"].ToString());
                    TemplateHandler.Assign(ref resstrmain, "lastname_value", dr["lastname"].ToString());
                    TemplateHandler.Assign(ref resstrmain, "group_value", dr["ugroup"].ToString());
                    TemplateHandler.Assign(ref resstrmain, "mcomm1_value", NumberFormat.DecFormat(dr["adiscount"], 2));
                    TemplateHandler.Assign(ref resstrmain, "acomm1_value", NumberFormat.DecFormat(dr["discount"], 2));
                    TemplateHandler.Assign(ref resstrmain, "pcomm1_value", NumberFormat.DecFormat(dr["playerdiscount"], 2));
                    TemplateHandler.Assign(ref resstrmain, "mcomm2_value", NumberFormat.DecFormat(dr["adiscount1x2"], 2));
                    TemplateHandler.Assign(ref resstrmain, "acomm2_value", NumberFormat.DecFormat(dr["discount1x2"], 2));
                    TemplateHandler.Assign(ref resstrmain, "pcomm2_value", NumberFormat.DecFormat(dr["playerdiscount1x2"], 2));
                    TemplateHandler.Assign(ref resstrmain, "mcomm3_value", NumberFormat.DecFormat(dr["adiscountcs"], 2));
                    TemplateHandler.Assign(ref resstrmain, "acomm3_value", NumberFormat.DecFormat(dr["discountcs"], 2));
                    TemplateHandler.Assign(ref resstrmain, "pcomm3_value", NumberFormat.DecFormat(dr["playerdiscountcs"], 2));

                    TemplateHandler.Assign(ref resstrmain, "credit_value", NumberFormat.DecFormat(dr["credit"], 2));

                    //Bingo
                    if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission)
                    {
                        TemplateHandler.Assign(ref resstrmain, "mnumber_comm_value", NumberFormat.DecFormat(dr["adiscount_numbergame"], 2));
                        TemplateHandler.Assign(ref resstrmain, "anumber_comm_value", NumberFormat.DecFormat(dr["discount_numbergame"], 2));
                        TemplateHandler.Assign(ref resstrmain, "pnumber_comm_value", NumberFormat.DecFormat(dr["playerdiscount_numbergame"], 2));
                    }
                    //End

                    result.Append(resstrmain);
                }
            }
            else
            {
                int numcols = 22;                
                if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission) numcols += 3;

                TemplateHandler.Assign(ref notfound, "noinfo", Resources.Lang.noInfo);
                TemplateHandler.Assign(ref notfound, "num-cols", numcols);
                result.Append(notfound);
            }
        }
        else
        {
            int numcols = 22;            
            if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission) numcols += 3;

            TemplateHandler.Assign(ref notfound, "noinfo", Resources.Lang.noInfo);
            TemplateHandler.Assign(ref notfound, "num-cols", numcols);
            result.Append(notfound);
        }
        rowcount = counter;
        return result.ToString();
    }
    #endregion

    #region Racing: Generate Member Commission by Agent
    public static string GetDataMemberCommissionAgentForRacing(string id, ref int rowcount)
    {
        string strmain = TemplateHandler.LoadTemplate("MemberInfo/MemberCommissionRacing.html");
        if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowHRCurrency)
            TemplateHandler.RemoveBlock(ref strmain, "SAHorse");
        if (AppSettings.IsAgentSite)
        {
            TemplateHandler.RemoveBlock(ref strmain, "IsAgentSite");
        }
        string notfound = TemplateHandler.LoadTemplate("NoInfo.html");
        string copystrmain = TemplateHandler.CopyBlock(strmain, "limit-content");
        string resstrmain = "";
        StringBuilder result = new StringBuilder();
        int counter = 0;
        
        TemplateHandler.Assign(ref copystrmain, "lblComm", Resources.Lang.com);
        TemplateHandler.Assign(ref strmain, "lblSAHorse", Resources.Racing.hrSaTote);
        TemplateHandler.Assign(ref strmain, "lblUKHorse", Resources.Racing.hrUkTote);
        TemplateHandler.Assign(ref strmain, "lblAUHorse", Resources.Racing.hrAUTote);
        TemplateHandler.Assign(ref strmain, "lblUKGreyhound", Resources.Racing.GreyhoundUK);
        TemplateHandler.Assign(ref strmain, "lblAUGreyhound", Resources.Racing.GreyhoundAU);
        TemplateHandler.Assign(ref strmain, "lblAUHarness", Resources.Racing.HarnessAU);

        if (id != "")
        {
            DbResult dbr = MemberInfo.GetDataAgent_WithAgent_MemberCommForRacing(id);
            DataTable dtb = dbr.ResultSet.Tables[0];

            TemplateHandler.Assign(ref copystrmain, "overcolor", AppSettings.RowBgOver);
            string OddEven = AppSettings.RowBgEven;
            if (dtb.Rows.Count > 0)
            {
                //for (int i = 0; i < dtb.Rows.Count; i++)                
                foreach (DataRow dr in dtb.Rows)
                {
                    counter++;
                    resstrmain = copystrmain;
                    TemplateHandler.Assign(ref resstrmain, "overcolor", AppSettings.RowBgOver);
                    if (Convert.ToInt32(dr["enabled"]) == 1)
                    {
                        if (Convert.ToInt32(dr["closed"]) != 0)
                            TemplateHandler.Assign(ref resstrmain, "color", AppSettings.RowBgClosed);
                        else if (Convert.ToInt32(dr["suspended"]) != 0)
                            TemplateHandler.Assign(ref resstrmain, "color", AppSettings.RowBgSuspended);
                        else
                        {
                            OddEven = (OddEven == AppSettings.RowBgEven) ? AppSettings.RowBgOdd : AppSettings.RowBgEven;
                            TemplateHandler.Assign(ref resstrmain, "color", OddEven);
                        }
                    }
                    else
                    {
                        TemplateHandler.Assign(ref resstrmain, "color", AppSettings.RowBgDisabled);
                    }
                    TemplateHandler.Assign(ref resstrmain, "no_value", counter.ToString());
                    TemplateHandler.Assign(ref resstrmain, "username_value", dr["username"].ToString());
                    TemplateHandler.Assign(ref resstrmain, "firstname_value", dr["firstname"].ToString());
                    TemplateHandler.Assign(ref resstrmain, "lastname_value", dr["lastname"].ToString());
                    TemplateHandler.Assign(ref resstrmain, "group_value", dr["ugroup"].ToString());
                    TemplateHandler.Assign(ref resstrmain, "credit_value", NumberFormat.DecFormat(dr["credit"], 2));

                    // SA Horse
                    if (Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowHRCurrency)
                    {
                        TemplateHandler.Assign(ref resstrmain, "v151adiscount_31", NumberFormat.DecFormat(dr["v151adiscount_31"], 2));
                        TemplateHandler.Assign(ref resstrmain, "v151discount_31", NumberFormat.DecFormat(dr["v151discount_31"], 2));
                        TemplateHandler.Assign(ref resstrmain, "v151playerdiscount_31", NumberFormat.DecFormat(dr["v151playerdiscount_31"], 2));
                    }

                    // UK Horse
                    TemplateHandler.Assign(ref resstrmain, "v151adiscount_41", NumberFormat.DecFormat(dr["v151adiscount_41"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v151discount_41", NumberFormat.DecFormat(dr["v151discount_41"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v151playerdiscount_41", NumberFormat.DecFormat(dr["v151playerdiscount_41"], 2));

                    // AU Horse
                    TemplateHandler.Assign(ref resstrmain, "v151adiscount_51", NumberFormat.DecFormat(dr["v151adiscount_51"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v151discount_51", NumberFormat.DecFormat(dr["v151discount_51"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v151playerdiscount_51", NumberFormat.DecFormat(dr["v151playerdiscount_51"], 2));

                    // UK Greyhound
                    TemplateHandler.Assign(ref resstrmain, "v152adiscount_41", NumberFormat.DecFormat(dr["v152adiscount_41"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v152discount_41", NumberFormat.DecFormat(dr["v152discount_41"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v152playerdiscount_41", NumberFormat.DecFormat(dr["v152playerdiscount_41"], 2));

                    // AU Greyhound
                    TemplateHandler.Assign(ref resstrmain, "v152adiscount_51", NumberFormat.DecFormat(dr["v152adiscount_51"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v152discount_51", NumberFormat.DecFormat(dr["v152discount_51"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v152playerdiscount_51", NumberFormat.DecFormat(dr["v152playerdiscount_51"], 2));

                    // AU Harness
                    TemplateHandler.Assign(ref resstrmain, "v153adiscount_51", NumberFormat.DecFormat(dr["v153adiscount_51"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v153discount_51", NumberFormat.DecFormat(dr["v153discount_51"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v153playerdiscount_51", NumberFormat.DecFormat(dr["v153playerdiscount_51"], 2));

                    result.Append(resstrmain);
                }
            }
            else
            {
                int numcols = 22;
                if (Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowHRCurrency)
                {
                    numcols += 3;
                }
                TemplateHandler.Assign(ref notfound, "noinfo", Resources.Lang.noInfo);
                TemplateHandler.Assign(ref notfound, "num-cols", numcols);
                result.Append(notfound);
            }
        }
        else
        {
            int numcols = 22;
            if (Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowHRCurrency)
            {
                numcols += 3;
            }
            TemplateHandler.Assign(ref notfound, "noinfo", Resources.Lang.noInfo);
            TemplateHandler.Assign(ref notfound, "num-cols", numcols);
            result.Append(notfound);
        }
        rowcount = counter;
        return result.ToString();
    }
    #endregion

    #region Generate Member Commission by Master
    public static string GetDataMemberCommissionMaster(string id, ref int rowcount)
    {
        string strmain = TemplateHandler.LoadTemplate("MemberInfo/MemberCommission.html");        
        string copystrmain = TemplateHandler.CopyBlock(strmain, "limit-content");
        string notfound = TemplateHandler.LoadTemplate("NoInfo.html");
        string resstrmain = "";
        string result = "";
        int counter = 0;
        string OddEven = AppSettings.RowBgEven;

        //If set enable number commission on webconfig
        if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission)
        {
            TemplateHandler.RemoveBlock(ref copystrmain, "number-comm");
        }

        TemplateHandler.Assign(ref copystrmain, "lblComm", Resources.Lang.com);
        if (id != "")
        {
            DbResult dbr = MemberInfo.GetDataAgent_WithMaster_MemberComm(id);
            DataTable dtb = dbr.ResultSet.Tables[0];
            TemplateHandler.Assign(ref copystrmain, "overcolor", AppSettings.RowBgOver);
            if (dtb.Rows.Count > 0)
            {
                for (int i = 0; i < dtb.Rows.Count; i++)
                {
                    counter++;
                    resstrmain = copystrmain;
                    if (Convert.ToInt32(dtb.Rows[i]["enabled"]) == 1)
                    {
                        if (Convert.ToInt32(dtb.Rows[i]["closed"]) != 0)
                            TemplateHandler.Assign(ref resstrmain, "color", AppSettings.RowBgClosed);
                        else if (Convert.ToInt32(dtb.Rows[i]["suspended"]) != 0)
                            TemplateHandler.Assign(ref resstrmain, "color", AppSettings.RowBgSuspended);
                        else
                        {
                            OddEven = (OddEven == AppSettings.RowBgOdd) ? AppSettings.RowBgEven : AppSettings.RowBgOdd;
                            TemplateHandler.Assign(ref resstrmain, "color", OddEven);
                        }
                    }
                    else
                    {
                        TemplateHandler.Assign(ref resstrmain, "color", AppSettings.RowBgDisabled);
                    }
                    TemplateHandler.Assign(ref resstrmain, "no_value", counter.ToString());
                    TemplateHandler.Assign(ref resstrmain, "username_value", dtb.Rows[i]["username"].ToString());
                    TemplateHandler.Assign(ref resstrmain, "firstname_value", dtb.Rows[i]["firstname"].ToString());
                    TemplateHandler.Assign(ref resstrmain, "lastname_value", dtb.Rows[i]["lastname"].ToString());
                    TemplateHandler.Assign(ref resstrmain, "group_value", dtb.Rows[i]["ugroup"].ToString());
                    TemplateHandler.Assign(ref resstrmain, "mcomm1_value", NumberFormat.DecFormat(dtb.Rows[i]["adiscount"], 2));
                    TemplateHandler.Assign(ref resstrmain, "acomm1_value", NumberFormat.DecFormat(dtb.Rows[i]["discount"], 2));
                    TemplateHandler.Assign(ref resstrmain, "pcomm1_value", NumberFormat.DecFormat(dtb.Rows[i]["playerdiscount"], 2));
                    TemplateHandler.Assign(ref resstrmain, "mcomm2_value", NumberFormat.DecFormat(dtb.Rows[i]["adiscount1x2"], 2));
                    TemplateHandler.Assign(ref resstrmain, "acomm2_value", NumberFormat.DecFormat(dtb.Rows[i]["discount1x2"], 2));
                    TemplateHandler.Assign(ref resstrmain, "pcomm2_value", NumberFormat.DecFormat(dtb.Rows[i]["playerdiscount1x2"], 2));
                    TemplateHandler.Assign(ref resstrmain, "mcomm3_value", NumberFormat.DecFormat(dtb.Rows[i]["adiscountcs"], 2));
                    TemplateHandler.Assign(ref resstrmain, "acomm3_value", NumberFormat.DecFormat(dtb.Rows[i]["discountcs"], 2));
                    TemplateHandler.Assign(ref resstrmain, "pcomm3_value", NumberFormat.DecFormat(dtb.Rows[i]["playerdiscountcs"], 2));
                    TemplateHandler.Assign(ref resstrmain, "credit_value", NumberFormat.DecFormat(dtb.Rows[i]["credit"], 2));

                    //Bingo
                    if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission)
                    {
                        TemplateHandler.Assign(ref resstrmain, "mnumber_comm_value", NumberFormat.DecFormat(dtb.Rows[i]["adiscount_numbergame"], 2));
                        TemplateHandler.Assign(ref resstrmain, "anumber_comm_value", NumberFormat.DecFormat(dtb.Rows[i]["discount_numbergame"], 2));
                        TemplateHandler.Assign(ref resstrmain, "pnumber_comm_value", NumberFormat.DecFormat(dtb.Rows[i]["playerdiscount_numbergame"], 2));
                    }
                    //End
                    result += resstrmain;
                }
            }
            else
            {
                int numcols = 22;
                if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission) numcols += 3;
              
                TemplateHandler.Assign(ref notfound, "noinfo", Resources.Lang.noInfo);
                TemplateHandler.Assign(ref notfound, "num-cols", numcols);
                result += notfound;
            }
        }
        else
        {
            int numcols = 22;
            if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission) numcols += 3;
            TemplateHandler.Assign(ref notfound, "noinfo", Resources.Lang.noInfo);
            TemplateHandler.Assign(ref notfound, "num-cols", numcols);
            result += notfound;
        }
        rowcount = counter;
        return result;
    }
    #endregion

    #region Racing: Generate Member Commission by Master
    public static string GetDataMemberCommissionMasterForRacing(string id, ref int rowcount)
    {
        string strmain = TemplateHandler.LoadTemplate("MemberInfo/MemberCommissionRacing.html");
        if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowHRCurrency)
            TemplateHandler.RemoveBlock(ref strmain, "SAHorse");
        string copystrmain = TemplateHandler.CopyBlock(strmain, "limit-content");
        string notfound = TemplateHandler.LoadTemplate("NoInfo.html");
        string resstrmain = "";
        string result = "";
        int counter = 0;
        string OddEven = AppSettings.RowBgEven;

        TemplateHandler.Assign(ref copystrmain, "lblComm", Resources.Lang.com);
        TemplateHandler.Assign(ref strmain, "lblSAHorse", Resources.Racing.hrSaTote);
        TemplateHandler.Assign(ref strmain, "lblUKHorse", Resources.Racing.hrUkTote);
        TemplateHandler.Assign(ref strmain, "lblAUHorse", Resources.Racing.hrAUTote);
        TemplateHandler.Assign(ref strmain, "lblUKGreyhound", Resources.Racing.GreyhoundUK);
        TemplateHandler.Assign(ref strmain, "lblAUGreyhound", Resources.Racing.GreyhoundAU);
        TemplateHandler.Assign(ref strmain, "lblAUHarness", Resources.Racing.HarnessAU);

        if (id != "")
        {
            DbResult dbr = MemberInfo.GetDataAgent_WithMaster_MemberCommForRacing(id);
            DataTable dtb = dbr.ResultSet.Tables[0];
            TemplateHandler.Assign(ref copystrmain, "overcolor", AppSettings.RowBgOver);
            if (dtb.Rows.Count > 0)
            {
                //for (int i = 0; i < dtb.Rows.Count; i++)
                foreach (DataRow dr in dtb.Rows)
                {
                    counter++;
                    resstrmain = copystrmain;
                    if (Convert.ToInt32(dr["enabled"]) == 1)
                    {
                        if (Convert.ToInt32(dr["closed"]) != 0)
                            TemplateHandler.Assign(ref resstrmain, "color", AppSettings.RowBgClosed);
                        else if (Convert.ToInt32(dr["suspended"]) != 0)
                            TemplateHandler.Assign(ref resstrmain, "color", AppSettings.RowBgSuspended);
                        else
                        {
                            OddEven = (OddEven == AppSettings.RowBgOdd) ? AppSettings.RowBgEven : AppSettings.RowBgOdd;
                            TemplateHandler.Assign(ref resstrmain, "color", OddEven);
                        }
                    }
                    else
                    {
                        TemplateHandler.Assign(ref resstrmain, "color", AppSettings.RowBgDisabled);
                    }
                    TemplateHandler.Assign(ref resstrmain, "no_value", counter.ToString());
                    TemplateHandler.Assign(ref resstrmain, "username_value", dr["username"].ToString());
                    TemplateHandler.Assign(ref resstrmain, "firstname_value", dr["firstname"].ToString());
                    TemplateHandler.Assign(ref resstrmain, "lastname_value", dr["lastname"].ToString());
                    TemplateHandler.Assign(ref resstrmain, "group_value", dr["ugroup"].ToString());
                    TemplateHandler.Assign(ref resstrmain, "credit_value", NumberFormat.DecFormat(dr["credit"], 2));

                    // SA Horse
                    if (Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowHRCurrency)
                    {
                        TemplateHandler.Assign(ref resstrmain, "v151adiscount_31", NumberFormat.DecFormat(dr["v151adiscount_31"], 2));
                        TemplateHandler.Assign(ref resstrmain, "v151discount_31", NumberFormat.DecFormat(dr["v151discount_31"], 2));
                        TemplateHandler.Assign(ref resstrmain, "v151playerdiscount_31", NumberFormat.DecFormat(dr["v151playerdiscount_31"], 2));
                    }

                    // UK Horse
                    TemplateHandler.Assign(ref resstrmain, "v151adiscount_41", NumberFormat.DecFormat(dr["v151adiscount_41"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v151discount_41", NumberFormat.DecFormat(dr["v151discount_41"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v151playerdiscount_41", NumberFormat.DecFormat(dr["v151playerdiscount_41"], 2));

                    // AU Horse
                    TemplateHandler.Assign(ref resstrmain, "v151adiscount_51", NumberFormat.DecFormat(dr["v151adiscount_51"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v151discount_51", NumberFormat.DecFormat(dr["v151discount_51"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v151playerdiscount_51", NumberFormat.DecFormat(dr["v151playerdiscount_51"], 2));

                    // UK Greyhound
                    TemplateHandler.Assign(ref resstrmain, "v152adiscount_41", NumberFormat.DecFormat(dr["v152adiscount_41"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v152discount_41", NumberFormat.DecFormat(dr["v152discount_41"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v152playerdiscount_41", NumberFormat.DecFormat(dr["v152playerdiscount_41"], 2));

                    // AU Greyhound
                    TemplateHandler.Assign(ref resstrmain, "v152adiscount_51", NumberFormat.DecFormat(dr["v152adiscount_51"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v152discount_51", NumberFormat.DecFormat(dr["v152discount_51"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v152playerdiscount_51", NumberFormat.DecFormat(dr["v152playerdiscount_51"], 2));

                    // AU Harness
                    TemplateHandler.Assign(ref resstrmain, "v153adiscount_51", NumberFormat.DecFormat(dr["v153adiscount_51"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v153discount_51", NumberFormat.DecFormat(dr["v153discount_51"], 2));
                    TemplateHandler.Assign(ref resstrmain, "v153playerdiscount_51", NumberFormat.DecFormat(dr["v153playerdiscount_51"], 2));

                    result += resstrmain;
                }
            }
            else
            {
                int numcols = 22;
                if (Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowHRCurrency)
                {
                    numcols += 3;
                }
                TemplateHandler.Assign(ref notfound, "noinfo", Resources.Lang.noInfo);
                TemplateHandler.Assign(ref notfound, "num-cols", numcols);
                result += notfound;
            }
        }
        else
        {
            int numcols = 22;
            if (Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowHRCurrency)
            {
                numcols += 3;
            }
            TemplateHandler.Assign(ref notfound, "noinfo", Resources.Lang.noInfo);
            TemplateHandler.Assign(ref notfound, "num-cols", numcols);
            result += notfound;
        }
        rowcount = counter;
        return result;
    }
    #endregion


    #region Generate Select Master, Agent, Member
    public static string GetComboMaster(DbResult dbr, string idsu, ref string idfirstmaster, string idmaster)
    {
        string option = "<option value='{optvalue}' {selected}>{optname}</option>";
        string resoption = "";
        string result = "";
        DataTable dtb = dbr.ResultSet.Tables[0];
        if (dtb.Rows.Count > 0)
        {
            for (int i = 0; i < dtb.Rows.Count; i++)
            {
                resoption = option;
                if (idmaster == dtb.Rows[i]["custid"].ToString())
                {
                    TemplateHandler.Assign(ref resoption, "optvalue", dtb.Rows[i]["custid"]);
                    TemplateHandler.Assign(ref resoption, "optname", dtb.Rows[i]["username"]);
                    TemplateHandler.Assign(ref resoption, "selected", "selected");
                }
                else
                {
                    TemplateHandler.Assign(ref resoption, "optvalue", dtb.Rows[i]["custid"]);
                    TemplateHandler.Assign(ref resoption, "optname", dtb.Rows[i]["username"]);
                    TemplateHandler.Assign(ref resoption, "selected", string.Empty);
                }
                result += resoption;
            }
            idfirstmaster = dtb.Rows[0]["custid"].ToString();
        }
        else
        {
            idfirstmaster = "";
            result = "<option value=0>" + Resources.Lang.noInfo + "</option>";
        }
        return result;
    }
    public static string GetOptionMaster(string idsu, ref string idfirstmaster, string idmaster)
    {
        DbResult dbr = MemberInfo.GetSeLMaster_MemberComm(idsu);
        return GetComboMaster(dbr, idsu, ref idfirstmaster, idmaster);
    }
    public static string GetOptionMasterCasino(string idsu, ref string idfirstmaster, string idmaster)
    {
        DbResult dbr = MemberInfo.GetSeLMaster_MemberCommCasino(idsu);
        return GetComboMaster(dbr, idsu, ref idfirstmaster, idmaster);
    }
    public static string GetOptionMasterBingo(string idsu, ref string idfirstmaster, string idmaster)
    {
        DbResult dbr = MemberInfo.GetSeLMaster_MemberCommBingo(idsu);
        return GetComboMaster(dbr, idsu, ref idfirstmaster, idmaster);
    }
    public static string GetComboAgent(DbResult dbr, string idmas, string idagent, ref string idfistagent)
    {
        string alloption = "<option value=\"" + idmas + "\" {selected}>" + Resources.Lang.allagent + "</option>";
        string option = "<option value=\"{optvalue}\" {selected}>{optname}</option>";
        string resoption = "";
        string result = "";
        DataTable dtb = dbr.ResultSet.Tables[0];
        if (dtb.Rows.Count > 0)
        {
            if (AppSettings.IsSuperSite)
            {
                result = alloption;
            }
            for (int i = 0; i < dtb.Rows.Count; i++)
            {
                resoption = option;
                if (idagent == dtb.Rows[i]["custid"].ToString())
                {
                    TemplateHandler.Assign(ref resoption, "optvalue", dtb.Rows[i]["custid"]);
                    TemplateHandler.Assign(ref resoption, "optname", dtb.Rows[i]["username"]);
                    TemplateHandler.Assign(ref resoption, "selected", "selected");
                }
                else
                {
                    TemplateHandler.Assign(ref resoption, "optvalue", dtb.Rows[i]["custid"]);
                    TemplateHandler.Assign(ref resoption, "optname", dtb.Rows[i]["username"]);
                    TemplateHandler.Assign(ref resoption, "selected", string.Empty);
                }
                result += resoption;
            }
            idfistagent = dtb.Rows[0]["custid"].ToString();
        }
        else
        {
            idfistagent = "";
            result = "<option value=0>" + Resources.Lang.noInfo + "</option>";
        }
        return result;
    }
    public static string GetOptionAgent(string idmas, string idagent, ref string idfistagent)
    {
        DbResult dbr = MemberInfo.GetSeLAgent_MemberComm(idmas);
        return GetComboAgent(dbr, idmas, idagent, ref idfistagent);
    }
    public static string GetOptionAgentCasino(string idmas, string idagent, ref string idfistagent)
    {
        DbResult dbr = MemberInfo.GetSeLAgent_MemberCommCasino(idmas);
        return GetComboAgent(dbr, idmas, idagent, ref idfistagent);
    }
    public static string GetOptionAgentBingo(string idmas, string idagent, ref string idfistagent)
    {
        DbResult dbr = MemberInfo.GetSeLAgent_MemberCommBingo(idmas);
        return GetComboAgent(dbr, idmas, idagent, ref idfistagent);
    }
    #endregion

    #region Generate Position Taking list
    public static string GetPositionTaking(string row, string type, string custid, ref int rowcount)
    {
        string content = string.Empty;
        string result = string.Empty;
        string[] wildcard = new string[] { "a" };
        DbResult dbr = null;
        StringBuilder _s = new StringBuilder();
        int count = 0;
        string url = "";

        bool IsEditMember = false;

        if (AppSettings.IsAgentSite)
        {
            IsEditMember = true;
            wildcard = new string[] { "a" };
            dbr = MemberInfo.GetData_PositionTaking(custid);
            url = "../UserAgent/EditMember.aspx?m=0";
        }
        else
        {
            if (AppSettings.IsMasterSite)
            {
                wildcard = new string[] { "m", "a" };
                if (type == "")
                {
                    IsEditMember = false;
                    dbr = MemberInfo.GetData_Upline_PositionTaking(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId.ToString(), custid);
                    url = "javascript:viewPT('1','agent'";
                }
                else if (type == "agent")
                {
                    IsEditMember = true;
                    dbr = MemberInfo.GetData_PositionTaking(custid);
                    url = "../UserAgent/EditMember.aspx?m=0";
                }
            }
            else if (AppSettings.IsSuperSite)
            {
                if (type == "")
                {
                    IsEditMember = false;
                    wildcard = new string[] { "s" };
                    dbr = MemberInfo.GetData_Upline_PositionTaking(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId.ToString(), custid);
                    url = "javascript:viewPT('1','master'";
                }
                else if (type == "master")
                {
                    IsEditMember = false;
                    wildcard = new string[] { "m", "a" };
                    dbr = MemberInfo.GetData_Upline_PositionTaking("3", custid);
                    url = "javascript:viewPT('1','agent'";
                }
                else if (type == "agent")
                {
                    IsEditMember = true;
                    wildcard = new string[] { "s", "m", "a" };
                    dbr = MemberInfo.GetData_PositionTaking(custid);
                    url = "../UserAgent/EditMember.aspx?m=0";
                }
            }
        }

        //Agent only
        if (wildcard.Length == 1)
        {
            TemplateHandler.RemoveBlock(ref row, "Role");
        }

        if (IsEditMember)
        {
            TemplateHandler.RemoveBlock(ref row, "link");
        }
        else
        {
            TemplateHandler.RemoveBlock(ref row, "editmember");
        }

        string notfound = TemplateHandler.LoadTemplate("TotalBets/NotFound.html");
        if (dbr.ResultSet.Tables.Count > 0 && dbr.ResultSet.Tables[0].Rows.Count > 0)
        {
            DataTable dtb = dbr.ResultSet.Tables[0];
            rowcount = dtb.Rows.Count;
            count = 1;
            string _custid = "";
            string _username = "";
            string _url = "";
            foreach (DataRow r in dtb.Rows)
            {
                _custid = CastAide.AsString(r["custid"]);
                _username = CastAide.AsString(r["username"]);

                _url = url;
                //_url = _url + "&custid=" + _custid + "&USER=" + _username;
                _url += String.Format(",{0},'{1}')", _custid, _username);

                for (int i = 0; i < wildcard.Length; i++)
                {
                    content = row;
                    if (Convert.ToInt32(r["enabled"]) == 1)
                    {
                        if (Convert.ToInt32(r["closed"]) != 0)
                        {
                            TemplateHandler.Assign(ref content, "color", AppSettings.RowBgClosed);

                        }
                        else if (Convert.ToInt32(r["suspended"]) != 0)
                        {
                            TemplateHandler.Assign(ref content, "color", AppSettings.RowBgSuspended);

                        }
                        //else
                        //    TemplateHandler.Assign(ref content, "color", string.Empty);
                    }
                    else
                    {
                            TemplateHandler.Assign(ref content, "color", AppSettings.RowBgDisabled);
                    }
                    if (count % 2 != 0)
                    {
                        TemplateHandler.Assign(ref content, "color", AppSettings.RowBgEven);
                    }
                    else
                    {
                        TemplateHandler.Assign(ref content, "color", AppSettings.RowBgOdd);
                    }

                    TemplateHandler.Assign(ref content, "custid_value", _custid);
                    TemplateHandler.Assign(ref content, "username_value", _username);
                    TemplateHandler.Assign(ref content, "custid_upline", custid);
                    /* @Dung Fix because new interface don't have the corect site name*/
                    TemplateHandler.Assign(ref content, "Customer", Common.GetLevel(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId));

                    if (i == 0)
                    {
                        TemplateHandler.Assign(ref content, "No", count);
                        //Need to edit link
                        TemplateHandler.Assign(ref content, "url", _url);
                        TemplateHandler.Assign(ref content, "UserName_vl", CastAide.AsString(r["username"]));
                        TemplateHandler.Assign(ref content, "rowspan", "rowspan='" + wildcard.Length.ToString() + "'");
                    }
                    else
                    {
                        TemplateHandler.RemoveBlock(ref content, "NonRepeat");
                    }
                    if (wildcard.Length > 1)
                    {
                        TemplateHandler.Assign(ref content, "Role", wildcard[i].ToUpperInvariant());
                    }
                    //Soccer
                    TemplateHandler.Assign(ref content, "Soccer_hdp", NumberFormat.DecFormat(r[wildcard[i] + "positiontaking_1"], 2));
                    TemplateHandler.Assign(ref content, "Soccer_ou", NumberFormat.DecFormat(r[wildcard[i] + "positiontaking_3"], 2));
                    TemplateHandler.Assign(ref content, "Soccer_1sthdp", NumberFormat.DecFormat(r[wildcard[i] + "positiontaking_7"], 2));
                    TemplateHandler.Assign(ref content, "Soccer_1stou", NumberFormat.DecFormat(r[wildcard[i] + "positiontaking_8"], 2));
                    TemplateHandler.Assign(ref content, "Soccer_oe", NumberFormat.DecFormat(r[wildcard[i] + "positiontaking_2"], 2));
                    TemplateHandler.Assign(ref content, "Soccer_1x2", NumberFormat.DecFormat(r[wildcard[i] + "positiontaking_5"], 2));
                    TemplateHandler.Assign(ref content, "Soccer_cs", NumberFormat.DecFormat(r[wildcard[i] + "positiontaking_4"], 2));
                    TemplateHandler.Assign(ref content, "Soccer_tg", NumberFormat.DecFormat(r[wildcard[i] + "positiontaking_6"], 2));
                    TemplateHandler.Assign(ref content, "Soccer_mp", NumberFormat.DecFormat(r[wildcard[i] + "positiontaking_9"], 2));
                    TemplateHandler.Assign(ref content, "Soccer_outright", NumberFormat.DecFormat(r[wildcard[i] + "positiontaking_10"], 2));

                    //Soccer Live
                    TemplateHandler.Assign(ref content, "SoccerLive_hdp", NumberFormat.DecFormat(r[wildcard[i] + "livepositiontaking_1"], 2));
                    TemplateHandler.Assign(ref content, "SoccerLive_ou", NumberFormat.DecFormat(r[wildcard[i] + "livepositiontaking_3"], 2));
                    TemplateHandler.Assign(ref content, "SoccerLive_1sthdp", NumberFormat.DecFormat(r[wildcard[i] + "livepositiontaking_7"], 2));
                    TemplateHandler.Assign(ref content, "SoccerLive_1stou", NumberFormat.DecFormat(r[wildcard[i] + "livepositiontaking_8"], 2));

                    //baseball
                    TemplateHandler.Assign(ref content, "Basketball_hdp", NumberFormat.DecFormat(r["v2" + wildcard[i] + "positiontaking_1"], 2));
                    TemplateHandler.Assign(ref content, "Basketball_ou", NumberFormat.DecFormat(r["v2" + wildcard[i] + "positiontaking_3"], 2));
                    TemplateHandler.Assign(ref content, "Basketball_oe", NumberFormat.DecFormat(r["v2" + wildcard[i] + "positiontaking_2"], 2));
                    TemplateHandler.Assign(ref content, "Basketball_ml", NumberFormat.DecFormat(r["v2" + wildcard[i] + "positiontaking_20"], 2));

                    //football
                    TemplateHandler.Assign(ref content, "Football_hdp", NumberFormat.DecFormat(r["v3" + wildcard[i] + "positiontaking_1"], 2));
                    TemplateHandler.Assign(ref content, "Football_ou", NumberFormat.DecFormat(r["v3" + wildcard[i] + "positiontaking_3"], 2));
                    TemplateHandler.Assign(ref content, "Football_oe", NumberFormat.DecFormat(r["v3" + wildcard[i] + "positiontaking_2"], 2));
                    TemplateHandler.Assign(ref content, "Football_ml", NumberFormat.DecFormat(r["v3" + wildcard[i] + "positiontaking_20"], 2));

                    //tennis
                    TemplateHandler.Assign(ref content, "Tennis_hdp", NumberFormat.DecFormat(r["v5" + wildcard[i] + "positiontaking_1"], 2));
                    TemplateHandler.Assign(ref content, "Tennis_ou", NumberFormat.DecFormat(r["v5" + wildcard[i] + "positiontaking_3"], 2));
                    TemplateHandler.Assign(ref content, "Tennis_oe", NumberFormat.DecFormat(r["v5" + wildcard[i] + "positiontaking_2"], 2));
                    TemplateHandler.Assign(ref content, "Tennis_ml", NumberFormat.DecFormat(r["v5" + wildcard[i] + "positiontaking_20"], 2));

                    //baseball
                    TemplateHandler.Assign(ref content, "Baseball_hdp", NumberFormat.DecFormat(r["v8" + wildcard[i] + "positiontaking_1"], 2));
                    TemplateHandler.Assign(ref content, "Baseball_ou", NumberFormat.DecFormat(r["v8" + wildcard[i] + "positiontaking_3"], 2));
                    TemplateHandler.Assign(ref content, "Baseball_ml", NumberFormat.DecFormat(r["v8" + wildcard[i] + "positiontaking_20"], 2));

                    //golf
                    TemplateHandler.Assign(ref content, "Golf_hdp", NumberFormat.DecFormat(r["v10" + wildcard[i] + "positiontaking_1"], 2));
                    TemplateHandler.Assign(ref content, "Golf_ou", NumberFormat.DecFormat(r["v10" + wildcard[i] + "positiontaking_3"], 2));
                    TemplateHandler.Assign(ref content, "Golf_oe", NumberFormat.DecFormat(r["v10" + wildcard[i] + "positiontaking_2"], 2));
                    TemplateHandler.Assign(ref content, "Golf_ml", NumberFormat.DecFormat(r["v10" + wildcard[i] + "positiontaking_20"], 2));

                    //motor sports
                    TemplateHandler.Assign(ref content, "MotorSport_hdp", NumberFormat.DecFormat(r["v11" + wildcard[i] + "positiontaking_1"], 2));
                    TemplateHandler.Assign(ref content, "MotorSport_ml", NumberFormat.DecFormat(r["v11" + wildcard[i] + "positiontaking_20"], 2));

                    //other sports
                    TemplateHandler.Assign(ref content, "OtherSports_hdp", NumberFormat.DecFormat(r["v99" + wildcard[i] + "positiontaking_1"], 2));
                    TemplateHandler.Assign(ref content, "OtherSports_ou", NumberFormat.DecFormat(r["v99" + wildcard[i] + "positiontaking_3"], 2));
                    TemplateHandler.Assign(ref content, "OtherSports_oe", NumberFormat.DecFormat(r["v99" + wildcard[i] + "positiontaking_2"], 2));
                    TemplateHandler.Assign(ref content, "OtherSports_ml", NumberFormat.DecFormat(r["v99" + wildcard[i] + "positiontaking_20"], 2));
                    
                    //Parlay Mixed Sports
                    if(UserPermission.IsShowMixedSport)
                        TemplateHandler.Assign(ref content, "ParlayMixedSports_ml", NumberFormat.DecFormat(r["v99" + wildcard[i] + "positiontaking_9"], 2));

                    _s.Append(content);
                }
                count++;
            }
            result = _s.ToString();
        }
        else
        {
            if (wildcard.Length > 1)
            {
                int colno = 48;
                TemplateHandler.Assign(ref notfound, "numcolumn", colno);
            }
            else
            {
                int colno = 47;
                TemplateHandler.Assign(ref notfound, "numcolumn", colno.ToString());
            }

            TemplateHandler.Assign(ref notfound, "notfound", Resources.Lang.noInfo);
            result = notfound;
        }
        return result;
    }
    #endregion

    #region Generate Position Taking Racing list
    public static string GetPositionTakingRacing(string row, string type, string custid, ref int rowcount)
    {
        string content = string.Empty;
        string result = string.Empty;
        string[] wildcard = new string[] { "a" };
        DbResult dbr = null;
        StringBuilder _s = new StringBuilder();
        int count = 0;
        string url = "";

        bool IsEditMember = false;

        if (AppSettings.IsAgentSite)
        {
            IsEditMember = true;
            wildcard = new string[] { "a" };
            dbr = MemberInfo.GetData_PositionTakingRacing(custid);
            url = "../UserAgent/EditMember.aspx?m=0";
        }
        else
        {
            if (AppSettings.IsMasterSite)
            {
                wildcard = new string[] { "m", "a" };
                if (type == "")
                {
                    IsEditMember = false;
                    dbr = MemberInfo.GetData_Upline_PositionTakingRacing(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId.ToString(), custid);
                    url = "javascript:viewPT('2','agent'";
                }
                else if (type == "agent")
                {
                    IsEditMember = true;
                    dbr = MemberInfo.GetData_PositionTakingRacing(custid);
                    url = "../UserAgent/EditMember.aspx?m=0";
                }
            }
            else if (AppSettings.IsSuperSite)
            {
                if (type == "")
                {
                    IsEditMember = false;
                    wildcard = new string[] { "s" };
                    dbr = MemberInfo.GetData_Upline_PositionTakingRacing(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId.ToString(), custid);
                    url = "javascript:viewPT('2','master'";
                }
                else if (type == "master")
                {
                    IsEditMember = false;
                    wildcard = new string[] { "m", "a" };
                    dbr = MemberInfo.GetData_Upline_PositionTakingRacing("3", custid);
                    url = "javascript:viewPT('2','agent'";
                }
                else if (type == "agent")
                {
                    IsEditMember = true;
                    wildcard = new string[] { "s", "m", "a" };
                    dbr = MemberInfo.GetData_PositionTakingRacing(custid);
                    url = "../UserAgent/EditMember.aspx?m=0";
                }
            }
        }

        //Agent only
        if (wildcard.Length == 1)
        {
            TemplateHandler.RemoveBlock(ref row, "Role");
        }

        if (IsEditMember)
        {
            TemplateHandler.RemoveBlock(ref row, "link");
        }
        else
        {
            TemplateHandler.RemoveBlock(ref row, "editmember");
        }

        string notfound = TemplateHandler.LoadTemplate("TotalBets/NotFound.html");
        if (dbr.ResultSet.Tables.Count > 0 && dbr.ResultSet.Tables[0].Rows.Count > 0)
        {
            DataTable dtb = dbr.ResultSet.Tables[0];
            rowcount = dtb.Rows.Count;
            count = 1;
            string _custid = "";
            string _username = "";
            string _url = "";
            foreach (DataRow r in dtb.Rows)
            {
                _custid = CastAide.AsString(r["custid"]);
                _username = CastAide.AsString(r["username"]);

                _url = url;
                //_url = _url + "&custid=" + _custid + "&USER=" + _username;
                _url += String.Format(",{0},'{1}')", _custid, _username);

                for (int i = 0; i < wildcard.Length; i++)
                {
                    content = row;
                    if (Convert.ToInt32(r["enabled"]) == 1)
                    {
                        if (Convert.ToInt32(r["closed"]) != 0)
                        {
                            TemplateHandler.Assign(ref content, "color", AppSettings.RowBgClosed);

                        }
                        else if (Convert.ToInt32(r["suspended"]) != 0)
                        {
                            TemplateHandler.Assign(ref content, "color", AppSettings.RowBgSuspended);

                        }
                        //else
                        //    TemplateHandler.Assign(ref content, "color", string.Empty);
                    }
                    else
                    {
                        TemplateHandler.Assign(ref content, "color", AppSettings.RowBgDisabled);
                    }
                    if (count % 2 != 0)
                    {
                        TemplateHandler.Assign(ref content, "color", AppSettings.RowBgEven);
                    }
                    else
                    {
                        TemplateHandler.Assign(ref content, "color", AppSettings.RowBgOdd);
                    }

                    TemplateHandler.Assign(ref content, "custid_value", _custid);
                    TemplateHandler.Assign(ref content, "username_value", _username);
                    TemplateHandler.Assign(ref content, "custid_upline", custid);
                    /* @Dung Fix because new interface don't have the corect site name*/
                    TemplateHandler.Assign(ref content, "Customer", Common.GetLevel(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId));

                    if (i == 0)
                    {
                        TemplateHandler.Assign(ref content, "No", count);
                        //Need to edit link
                        TemplateHandler.Assign(ref content, "url", _url);
                        TemplateHandler.Assign(ref content, "UserName_vl", CastAide.AsString(r["username"]));
                        TemplateHandler.Assign(ref content, "rowspan", "rowspan='" + wildcard.Length.ToString() + "'");
                    }
                    else
                    {
                        TemplateHandler.RemoveBlock(ref content, "NonRepeat");
                    }
                    if (wildcard.Length > 1)
                    {
                        TemplateHandler.Assign(ref content, "Role", wildcard[i].ToUpperInvariant());
                    }

                    // Horse
                    TemplateHandler.Assign(ref content, "UK_Win_Horse", NumberFormat.DecFormat(r["v151" + wildcard[i] + "positiontaking_41"], 2));
                    TemplateHandler.Assign(ref content, "UK_Place_Horse", NumberFormat.DecFormat(r["v151" + wildcard[i] + "positiontaking_42"], 2));
                    TemplateHandler.Assign(ref content, "AU_win_Horse", NumberFormat.DecFormat(r["v151" + wildcard[i] + "positiontaking_51"], 2));
                    TemplateHandler.Assign(ref content, "AU_place_Horse", NumberFormat.DecFormat(r["v151" + wildcard[i] + "positiontaking_52"], 2));

                    // Greyhound
                    TemplateHandler.Assign(ref content, "UK_Win_Greyhound", NumberFormat.DecFormat(r["v152" + wildcard[i] + "positiontaking_41"], 2));
                    TemplateHandler.Assign(ref content, "UK_Place_Greyhound", NumberFormat.DecFormat(r["v152" + wildcard[i] + "positiontaking_42"], 2));
                    TemplateHandler.Assign(ref content, "AU_win_Greyhound", NumberFormat.DecFormat(r["v152" + wildcard[i] + "positiontaking_51"], 2));
                    TemplateHandler.Assign(ref content, "AU_place_Greyhound", NumberFormat.DecFormat(r["v152" + wildcard[i] + "positiontaking_52"], 2));

                    // Harness
                    TemplateHandler.Assign(ref content, "AU_win_Harness", NumberFormat.DecFormat(r["v153" + wildcard[i] + "positiontaking_51"], 2));
                    TemplateHandler.Assign(ref content, "AU_place_Harness", NumberFormat.DecFormat(r["v153" + wildcard[i] + "positiontaking_52"], 2));

                    if (Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowHRCurrency)
                    {
                        TemplateHandler.Assign(ref content, "hr_win", NumberFormat.DecFormat(r["v151" + wildcard[i] + "positiontaking_31"], 2));
                        TemplateHandler.Assign(ref content, "hr_place", NumberFormat.DecFormat(r["v151" + wildcard[i] + "positiontaking_32"], 2));
                    }
                    
                    _s.Append(content);
                }
                count++;
            }
            result = _s.ToString();
        }
        else
        {
            if (wildcard.Length > 1)
            {
                int colno = 50;
                if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowHRCurrency)
                    colno -= 2;
                TemplateHandler.Assign(ref notfound, "numcolumn", colno);
            }
            else
            {
                int colno = 49;
                if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowHRCurrency)
                    colno -= 2;
                TemplateHandler.Assign(ref notfound, "numcolumn", colno.ToString());
            }

            TemplateHandler.Assign(ref notfound, "notfound", Resources.Lang.noInfo);
            result = notfound;
        }
        return result;
    }
    #endregion

    #region Generate Position Taking NumberGame list
    public static string GetPositionTakingNumberGame(string row, string type, string custid, ref int rowcount)
    {
        string content = string.Empty;
        string result = string.Empty;
        string[] wildcard = new string[] { "a" };
        DbResult dbr = null;
        StringBuilder _s = new StringBuilder();
        int count = 0;
        string url = "";

        bool IsEditMember = false;

        if (AppSettings.IsAgentSite)
        {
            IsEditMember = true;
            wildcard = new string[] { "a" };
            dbr = MemberInfo.GetData_PositionTakingNumberGame(custid);
            url = "../UserAgent/EditMember.aspx?m=0";
        }
        else
        {
            if (AppSettings.IsMasterSite)
            {
                wildcard = new string[] { "m", "a" };
                if (type == "")
                {
                    IsEditMember = false;
                    dbr = MemberInfo.GetData_Upline_PositionTakingNumberGame(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId.ToString(), custid);
                    url = "javascript:viewPT('3','agent'";
                }
                else if (type == "agent")
                {
                    IsEditMember = true;
                    dbr = MemberInfo.GetData_PositionTakingNumberGame(custid);
                    url = "../UserAgent/EditMember.aspx?m=0";
                }
            }
            else if (AppSettings.IsSuperSite)
            {
                if (type == "")
                {
                    IsEditMember = false;
                    wildcard = new string[] { "s" };
                    dbr = MemberInfo.GetData_Upline_PositionTakingNumberGame(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId.ToString(), custid);
                    url = "javascript:viewPT('3','master'";
                }
                else if (type == "master")
                {
                    IsEditMember = false;
                    wildcard = new string[] { "m", "a" };
                    dbr = MemberInfo.GetData_Upline_PositionTakingNumberGame("3", custid);
                    url = "javascript:viewPT('3','agent'";
                }
                else if (type == "agent")
                {
                    IsEditMember = true;
                    wildcard = new string[] { "s", "m", "a" };
                    dbr = MemberInfo.GetData_PositionTakingNumberGame(custid);
                    url = "../UserAgent/EditMember.aspx?m=0";
                }
            }
        }

        //Agent only
        if (wildcard.Length == 1)
        {
            TemplateHandler.RemoveBlock(ref row, "Role");
        }

        if (IsEditMember)
        {
            TemplateHandler.RemoveBlock(ref row, "link");
        }
        else
        {
            TemplateHandler.RemoveBlock(ref row, "editmember");
        }

        string notfound = TemplateHandler.LoadTemplate("TotalBets/NotFound.html");
        if (dbr.ResultSet.Tables.Count > 0 && dbr.ResultSet.Tables[0].Rows.Count > 0)
        {
            DataTable dtb = dbr.ResultSet.Tables[0];
            rowcount = dtb.Rows.Count;
            count = 1;
            string _custid = "";
            string _username = "";
            string _url = "";
            foreach (DataRow r in dtb.Rows)
            {
                _custid = CastAide.AsString(r["custid"]);
                _username = CastAide.AsString(r["username"]);

                _url = url;
                //_url = _url + "&custid=" + _custid + "&USER=" + _username;
                _url += String.Format(",{0},'{1}')", _custid, _username);

                for (int i = 0; i < wildcard.Length; i++)
                {
                    content = row;
                    if (Convert.ToInt32(r["enabled"]) == 1)
                    {
                        if (Convert.ToInt32(r["closed"]) != 0)
                        {
                            TemplateHandler.Assign(ref content, "color", AppSettings.RowBgClosed);

                        }
                        else if (Convert.ToInt32(r["suspended"]) != 0)
                        {
                            TemplateHandler.Assign(ref content, "color", AppSettings.RowBgSuspended);

                        }                        
                    }
                    else
                    {
                        TemplateHandler.Assign(ref content, "color", AppSettings.RowBgDisabled);
                    }

                    if (count % 2 != 0)
                    {
                        TemplateHandler.Assign(ref content, "color", AppSettings.RowBgEven);
                    }
                    else
                    {
                        TemplateHandler.Assign(ref content, "color", AppSettings.RowBgOdd);
                    }
                    TemplateHandler.Assign(ref content, "custid_value", _custid);
                    TemplateHandler.Assign(ref content, "username_value", _username);
                    TemplateHandler.Assign(ref content, "custid_upline", custid);
                    /* @Dung Fix because new interface don't have the corect site name*/
                    TemplateHandler.Assign(ref content, "Customer", Common.GetLevel(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId));

                    if (i == 0)
                    {
                        TemplateHandler.Assign(ref content, "No", count);
                        //Need to edit link
                        TemplateHandler.Assign(ref content, "url", _url);
                        TemplateHandler.Assign(ref content, "UserName_vl", CastAide.AsString(r["username"]));
                        TemplateHandler.Assign(ref content, "rowspan", "rowspan='" + wildcard.Length.ToString() + "'");
                    }
                    else
                    {
                        TemplateHandler.RemoveBlock(ref content, "NonRepeat");
                    }
                    if (wildcard.Length > 1)
                    {
                        TemplateHandler.Assign(ref content, "Role", wildcard[i].ToUpperInvariant());
                    }
                    //DeadBall NumberGame
                    TemplateHandler.Assign(ref content, "DeadBall_1stLastOU", NumberFormat.DecFormat(r["v161" + wildcard[i] + "positiontaking_81"], 2));
                    TemplateHandler.Assign(ref content, "DeadBall_1stLastOE", NumberFormat.DecFormat(r["v161" + wildcard[i] + "positiontaking_83"], 2));
                    TemplateHandler.Assign(ref content, "DeadBall_Warrior", NumberFormat.DecFormat(r["v161" + wildcard[i] + "positiontaking_88"], 2));
                    TemplateHandler.Assign(ref content, "DeadBall_OE", NumberFormat.DecFormat(r["v161" + wildcard[i] + "positiontaking_86"], 2));
                    //LiveBall NumberGame
                    TemplateHandler.Assign(ref content, "LiveBall_OU", NumberFormat.DecFormat(r["v161" + wildcard[i] + "livepositiontaking_85"], 2));
                    TemplateHandler.Assign(ref content, "LiveBall_OE", NumberFormat.DecFormat(r["v161" + wildcard[i] + "livepositiontaking_86"], 2));
                    TemplateHandler.Assign(ref content, "LiveBall_HL", NumberFormat.DecFormat(r["v161" + wildcard[i] + "livepositiontaking_87"], 2));
                    TemplateHandler.Assign(ref content, "LiveBall_NextCombo", NumberFormat.DecFormat(r["v161" + wildcard[i] + "livepositiontaking_89"], 2));
                    if (UserPermission.IsShowNumberGroup) 
                        TemplateHandler.Assign(ref content, "LiveBall_NumberGroup", NumberFormat.DecFormat(r["v161" + wildcard[i] + "livepositiontaking_90"], 2));
                    
                    _s.Append(content);
                }
                count++;

            }
            result = _s.ToString();
        }
        else
        {
            int colno;
            if (wildcard.Length > 1)
                colno = 12;
            else
                colno = 11;
            TemplateHandler.Assign(ref notfound, "numcolumn", colno.ToString());
            TemplateHandler.Assign(ref notfound, "notfound", Resources.Lang.noInfo);
            result = notfound;
        }
        return result;
    }
    #endregion

    #region Generate Transfer Condition
    public static string TransferCondition(string custid)
    {
        string strTransferTime = "";
        string strday = "";
        string[] Arrday = { Resources.Lang.sunday, Resources.Lang.monday, Resources.Lang.tuesday, Resources.Lang.wednesday, Resources.Lang.thursday, Resources.Lang.friday, Resources.Lang.saturday };
        DbResult dbr = MemberInfo.GetOptionTransferCondition(custid);
        if (dbr.Outputs != null && dbr.Outputs.ToString() != "")
        {
            strTransferTime += Resources.Lang.allowtransfer;
            char[] arrchar = CastAide.AsString(dbr.Outputs["transferoption"]).ToCharArray();
            if (arrchar.Length > 0)
            {
                for (int i = 1; i < arrchar.Length - 1; i++)
                {
                    if (CastAide.AsString(arrchar[i]).Equals("1") == true)
                    {
                        strday += " - " + Arrday[i].ToString();
                    }
                }
                if (CastAide.AsString(arrchar[0]).Equals("1") == true)
                {
                    strday += " - " + Arrday[0].ToString();
                }
                if (strday.Length > 0)
                {
                    strday = strday.Substring(3);
                }
                if (CastAide.AsString(arrchar[7]).Equals("1") == true)
                {
                    strTransferTime += " " + strday;
                }
                else
                {
                    strTransferTime += " " + Resources.Lang.daily;
                }
            }
            else
            {
                strTransferTime += " ";
            }
        }
        return strTransferTime;
    }
    #endregion

    #region Generate Credit Balance list
    public static string CreditBalance(string custid, string site_list, int pageId, string search, string username, int status, int totalPages)
    {
        //Terry
        int pageSize = 10;
        
        string _content = "";
        string _main = "";
        string _result = "";
        string _copycontent = "";
        StringBuilder rescontent = new StringBuilder();
        string _noinfo = TemplateHandler.LoadTemplate("NoInfo.html");
        _main = TemplateHandler.LoadTemplate("MemberInfo/CreditBalance.html");

        //Common.SetGUIByLang(ref _main);

        TemplateHandler.Assign(ref _main, "no", Resources.Lang.no);
        TemplateHandler.Assign(ref _main, "status", Resources.Lang.status);
        TemplateHandler.Assign(ref _main, "username", Resources.Lang.username);
        TemplateHandler.Assign(ref _main, "firstname", Resources.Lang.firstname);
        TemplateHandler.Assign(ref _main, "lastname", Resources.Lang.lastname);
        TemplateHandler.Assign(ref _main, "credit", Resources.Lang.credit);
        TemplateHandler.Assign(ref _main, "balance", Resources.Lang.balance);
        TemplateHandler.Assign(ref _main, "yesbalance", Resources.Lang.yesterday + " " + Resources.Lang.balance);
        TemplateHandler.Assign(ref _main, "betcredit", Resources.Lang.betcredit);
        TemplateHandler.Assign(ref _main, "outtxn", Resources.Lang.outstanding);
        TemplateHandler.Assign(ref _main, "memberturnover", Resources.Lang.memberturnover);
        TemplateHandler.Assign(ref _main, "lastlogin", Resources.Lang.lastlogin);
        TemplateHandler.Assign(ref _main, "lastloginip", Resources.Lang.lastloginIP);
        TemplateHandler.Assign(ref _main, "cus", custid);
        #region search head section
        TemplateHandler.Assign(ref _main, "lbl_username", Resources.Lang.username);
        TemplateHandler.Assign(ref _main, "search", search);
        TemplateHandler.Assign(ref _main, "previous_value", (username == "" || username == Resources.Lang.enterusername) ? Resources.Lang.enterusername : username);
        TemplateHandler.Assign(ref _main, "enterusername", Resources.Lang.enterusername);
        TemplateHandler.Assign(ref _main, "classStyle", (username == "" || username == Resources.Lang.enterusername) ? "text_f1" : "text_f");

        TemplateHandler.Assign(ref _main, "lbl_status", Resources.Lang.status);
        TemplateHandler.Assign(ref _main, "label_submit", Resources.Lang.submit);
        string strSearchAction = "searchByUsername('CreditBalance.aspx','" + site_list + "')";
        TemplateHandler.Assign(ref _main, "search_by_username", strSearchAction);

        TemplateHandler.Assign(ref _main, "all", Resources.Lang.all);
        TemplateHandler.Assign(ref _main, "active", Resources.Lang.open);
        TemplateHandler.Assign(ref _main, "closed", Resources.Lang.closed);
        TemplateHandler.Assign(ref _main, "suspended", Resources.Lang.suspended);
        TemplateHandler.Assign(ref _main, "disabled", Resources.Lang.disabled);
        if (status == 0)
        {
            TemplateHandler.Assign(ref _main, "active-select", "");
            TemplateHandler.Assign(ref _main, "all-select", "selected='selected'");
            TemplateHandler.Assign(ref _main, "closed-select", "");
            TemplateHandler.Assign(ref _main, "suspended-select", "");
            TemplateHandler.Assign(ref _main, "disabled-select", "");
        }
        else if (status == 1)
        {
            TemplateHandler.Assign(ref _main, "active-select", "selected='selected'");
            TemplateHandler.Assign(ref _main, "all-select", "");
            TemplateHandler.Assign(ref _main, "closed-select", "");
            TemplateHandler.Assign(ref _main, "suspended-select", "");
            TemplateHandler.Assign(ref _main, "disabled-select", "");
        }
        else if (status == 2)
        {
            TemplateHandler.Assign(ref _main, "active-select", "");
            TemplateHandler.Assign(ref _main, "all-select", "");
            TemplateHandler.Assign(ref _main, "closed-select", "");
            TemplateHandler.Assign(ref _main, "suspended-select", "selected='selected'");
            TemplateHandler.Assign(ref _main, "disabled-select", "");
        }
        else if (status == 3)
        {
            TemplateHandler.Assign(ref _main, "active-select", "");
            TemplateHandler.Assign(ref _main, "all-select", "");
            TemplateHandler.Assign(ref _main, "closed-select", "selected='selected'");
            TemplateHandler.Assign(ref _main, "suspended-select", "");
            TemplateHandler.Assign(ref _main, "disabled-select", "");
        }
        else
        {
            TemplateHandler.Assign(ref _main, "active-select", "");
            TemplateHandler.Assign(ref _main, "all-select", "");
            TemplateHandler.Assign(ref _main, "closed-select", "");
            TemplateHandler.Assign(ref _main, "suspended-select", "");
            TemplateHandler.Assign(ref _main, "disabled-select", "selected='selected'");
        }

        #endregion

        if (AppSettings.IsAgentSite)
        {
            TemplateHandler.Assign(ref _main, "customerlist", Resources.Lang.customerlist);
            TemplateHandler.ReplaceBlock(ref _main, "limit-creditbalance", Resources.Lang.creditbalance);
            TemplateHandler.Assign(ref _main, "url-list", "../_MemberInfo/CustomerList/Agent/MemberList.aspx");
            TemplateHandler.RemoveBlock(ref _main, "limit-position-taking");
            TemplateHandler.RemoveBlock(ref _main, "limit-agentlist");
            //--------------- Limit transfer  condition
            TemplateHandler.Assign(ref _main, "allowtransfer", TransferCondition(custid));
            //--------------------------

            //Hiep added on 02/10/08
            //for freeze headline
            TemplateHandler.Assign(ref _main, "isAgentSite", "1");
        }
        else if (AppSettings.IsMasterSite)
        {
            //Hiep added on 02/10/08
            //for freeze headline
            TemplateHandler.Assign(ref _main, "isAgentSite", "0");

            if (site_list == "MemberList")
            {
                TemplateHandler.Assign(ref _main, "customerlist", Resources.Lang.customerlist);
                TemplateHandler.ReplaceBlock(ref _main, "limit-creditbalance", Resources.Lang.creditbalance);
                TemplateHandler.Assign(ref _main, "url-list", "../_MemberInfo/CustomerList/Master/MemberList.aspx?custid=" + custid);
                TemplateHandler.Assign(ref _main, "positiontaking", Resources.Lang.positiontaking);
                TemplateHandler.Assign(ref _main, "url-position", "../_MemberInfo/PositionTakingList/Sportbook/SportbookPositionTakingList.aspx?type=agent&custid=" + custid);

                TemplateHandler.Assign(ref _main, "agentlist", Resources.Lang.lblAgentList);
                TemplateHandler.Assign(ref _main, "url-agentlist", "DownlineList.aspx");
                TemplateHandler.RemoveBlock(ref _main, "limit-position-taking");
            }
            else
            {
                TemplateHandler.Assign(ref _main, "customerlist", Resources.Lang.lblAgentList);
                TemplateHandler.ReplaceBlock(ref _main, "limit-creditbalance", Resources.Lang.creditbalance);
                TemplateHandler.Assign(ref _main, "url-list", "../_MemberInfo/CustomerList/Master/AgentList.aspx");
                TemplateHandler.RemoveBlock(ref _main, "limit-position-taking");
                TemplateHandler.RemoveBlock(ref _main, "limit-agentlist");
            }
            //--------------- Limit transfer  condition            
            TemplateHandler.RemoveBlock(ref _main, "allowtransfer-section");
        }
        else
        {
            //Hiep added on 02/10/08
            //for freeze headline
            TemplateHandler.Assign(ref _main, "isAgentSite", "0");

            if (site_list == "AgentList")
            {
                TemplateHandler.Assign(ref _main, "customerlist", Resources.Lang.lblAgentList);
                TemplateHandler.ReplaceBlock(ref _main, "limit-creditbalance", Resources.Lang.creditbalance);
                TemplateHandler.RemoveBlock(ref _main, "limit-position-taking");
                TemplateHandler.Assign(ref _main, "url-list", "../_MemberInfo/CustomerList/Super/AgentList.aspx?custid=" + custid);
                TemplateHandler.RemoveBlock(ref _main, "limit-agentlist");
            }
            else if (site_list == "MemberList")
            {
                TemplateHandler.Assign(ref _main, "customerlist", Resources.Lang.customerlist);
                TemplateHandler.ReplaceBlock(ref _main, "limit-creditbalance", Resources.Lang.creditbalance);
                TemplateHandler.Assign(ref _main, "url-list", "../_MemberInfo/CustomerList/Super/MemberList.aspx?custid=" + custid);
                TemplateHandler.Assign(ref _main, "positiontaking", Resources.Lang.positiontaking);
                TemplateHandler.Assign(ref _main, "url-position", "../_MemberInfo/PositionTakingList/Sportbook/SportbookPositionTakingList.aspx?type=agent&custid=" + custid);
                TemplateHandler.RemoveBlock(ref _main, "limit-agentlist");
            }
            else
            {
                TemplateHandler.Assign(ref _main, "customerlist", Resources.Lang.lblMasterList);
                TemplateHandler.ReplaceBlock(ref _main, "limit-creditbalance", Resources.Lang.creditbalance);
                TemplateHandler.Assign(ref _main, "url-list", "../_MemberInfo/CustomerList/Super/MasterList.aspx?custid=" + custid);
                TemplateHandler.RemoveBlock(ref _main, "limit-position-taking");
                TemplateHandler.RemoveBlock(ref _main, "limit-agentlist");
            }
            //--------------- Limit transfer  condition
            //TemplateHandler.RemoveBlock(ref _main, "limit-allowtransfer");
            TemplateHandler.RemoveBlock(ref _main, "allowtransfer-section");
        }
        _content = TemplateHandler.CopyBlock(_main, "limit-content");
        TemplateHandler.Assign(ref _content, "overcolor", AppSettings.RowBgOver);
        DbResult dbr = MemberInfo.GetDataCreditBalance(custid, site_list, pageSize, pageId, ref totalPages, (username.ToLower().Equals("all") ? string.Empty : username), status);
        TemplateHandler.Assign(ref _main, "totalpages", totalPages);
        DataTable dtb = dbr.ResultSet.Tables[0];
        string color = AppSettings.RowBgEven;
        int isclose = 0;
        int issuspend = 0;
        int isEnabled = 0;
        string url = "";
        string url_outst = "";
        string isbalance = "";
        string isYesbalance = "";
        int i = 0;
        int rowCount = 0;
        string pageList = string.Empty;
        string GotoPageHtml = string.Empty;
        if (dtb.Rows.Count > 0)
        {
            rowCount = dtb.Rows.Count;
            foreach (DataRow dr in dtb.Rows)
            {
                i++;               
                url = "javascript:viewBal('{act}', {_custid})";
                TemplateHandler.Assign(ref url, "_custid", dr["custid"]);
                url_outst = String.Format("javascript:viewOutSt({0},'{1}');", dr["custid"], dr["username"]);

                _copycontent = _content;
                isclose = Convert.ToInt32(dr["closed"]);
                issuspend = Convert.ToInt32(dr["suspended"]);
                isEnabled = CastAide.AsInt32(dr["enabled"]);

                isbalance = FormatHandler.FormatNumber2(dr["balance"], Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
                isYesbalance = FormatHandler.FormatNumber2(dr["yesterday_balance"], Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
                if (isEnabled == 1)
                {
                    if (isclose != 0)
                    {
                        TemplateHandler.Assign(ref _copycontent, "color", AppSettings.RowBgClosed);
                    }
                    else if (issuspend != 0)
                    {
                        TemplateHandler.Assign(ref _copycontent, "color", AppSettings.RowBgSuspended);
                    }
                    else
                    {
                        TemplateHandler.Assign(ref _copycontent, "color", color = (color == AppSettings.RowBgEven) ? AppSettings.RowBgOdd : AppSettings.RowBgEven);
                    }
                }
                else
                {
                    TemplateHandler.Assign(ref _copycontent, "color", AppSettings.RowBgDisabled); 
                }                
                TemplateHandler.Assign(ref _copycontent, "no_value", CastAide.AsString(dr["RowNumber"]));
                if (isEnabled == 1)
                {
                    if (isclose == 0)
                    {
                        if (issuspend == 0)
                            TemplateHandler.Assign(ref _copycontent, "status_value", Resources.Lang.open);
                        else
                            TemplateHandler.Assign(ref _copycontent, "status_value", Resources.Lang.suspend);
                    }
                    else
                    {
                        TemplateHandler.Assign(ref _copycontent, "status_value", Resources.Lang.close);
                    }
                }
                else
                {
                    TemplateHandler.Assign(ref _copycontent, "status_value", Resources.Lang.disabled);
                }

                if (AppSettings.IsSuperSite || AppSettings.IsMasterSite)
                {
                    if (site_list == "MemberList")
                    {
                        //TemplateHandler.RemoveBlock(ref  _copycontent, "limit-url-agent");
                        TemplateHandler.ReplaceBlock(ref _copycontent, "limit-url-agent", dr["username"].ToString());
                        TemplateHandler.RemoveBlock(ref  _copycontent, "limit-url-master-super");
                    }
                    else if (site_list == "AgentList")
                    {
                        TemplateHandler.RemoveBlock(ref  _copycontent, "limit-url-master-super");
                        TemplateHandler.Assign(ref url, "act", "MemberList");
                        TemplateHandler.Assign(ref _copycontent, "username_value", dr["username"]);
                    }
                    else
                    {
                        TemplateHandler.RemoveBlock(ref  _copycontent, "limit-url-master-super");
                        TemplateHandler.Assign(ref url, "act", "AgentList");
                        TemplateHandler.Assign(ref _copycontent, "username_value", dr["username"]);
                        
                    }
                    TemplateHandler.Assign(ref _copycontent, "url_value", url);
                    //------------
                    if (CastAide.AsDecimal(isYesbalance) == 0)
                    {
                        if (AppSettings.IsAgentSite)
                        {
                            TemplateHandler.ReplaceBlock(ref _copycontent, "limit-yes-agent", "<font color='#1e1eeb'>" + isYesbalance + "</font>");
                        }
                        else
                        {
                            TemplateHandler.ReplaceBlock(ref _copycontent, "limit-yes-agent", isYesbalance);
                        }
                    }
                    else if (CastAide.AsDecimal(isYesbalance) < 0)
                    {
                        TemplateHandler.ReplaceBlock(ref _copycontent, "limit-yes-agent", "<font color='red'>" + isYesbalance + "</font>");
                    }
                    else
                    {
                        TemplateHandler.ReplaceBlock(ref _copycontent, "limit-yes-agent", isYesbalance);
                    }
                    //-------------
                    if (site_list != "MemberList" || !UserSession.Current.SubPageAccess.ViewReports)
                    {
                        TemplateHandler.RemoveBlock(ref _copycontent, "limit-url-link-out");
                    }
                    else
                    {
                        TemplateHandler.RemoveBlock(ref _copycontent, "limit-url-out");
                        TemplateHandler.Assign(ref _copycontent, "url_outtxn", url_outst);
                    }
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref  _copycontent, "limit-url-agent");
                    //------------
                    if (CastAide.AsDecimal(isYesbalance) == 0)
                    {
                        string strTemp = (Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowedTotalBalanceTransfer ? isYesbalance : "<font color='#1e1eeb'>" + isYesbalance + "<font>");
                        TemplateHandler.ReplaceBlock(ref _copycontent, "limit-yes-agent", strTemp);                        
                    }
                    else
                    {
                        string strTemp = (CastAide.AsDecimal(isYesbalance) < 0 ?
                            "<font color='red'>" + isYesbalance + "</font>" : isYesbalance);
                        if (Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowedTotalBalanceTransfer)
                        {
                            TemplateHandler.ReplaceBlock(ref _copycontent, "limit-yes-agent", strTemp);
                        }
                        else
                        {
                            TemplateHandler.Assign(ref _copycontent, "yesbalance_value", strTemp);
                        }
                    }
                    //------------
                    if (!UserSession.Current.SubPageAccess.ViewReports)
                        TemplateHandler.RemoveBlock(ref _copycontent, "limit-url-link-out");
                    else
                        TemplateHandler.RemoveBlock(ref _copycontent, "limit-url-out");

                    TemplateHandler.Assign(ref _copycontent, "url_outtxn", url_outst);
                    TemplateHandler.Assign(ref _copycontent, "username_value", dr["username"]);
                }
                //TemplateHandler.Assign(ref _copycontent, "username_value", dr["username"]);
                TemplateHandler.Assign(ref _copycontent, "firstname_value", dr["firstname"]);
                TemplateHandler.Assign(ref _copycontent, "lastname_value", dr["lastname"]);

                // For Column Credit
                TemplateHandler.Assign(ref _copycontent, "credit_value", FormatHandler.FormatNumber2(dr["credit"], Sunplus.Agent.Model.Entities.UserSession.Current.LangKey));
                if (UserSession.Current.SubPageAccess.UpdateAccount)
                    TemplateHandler.RemoveBlock(ref _copycontent, "permission-credit");
                else
                    TemplateHandler.RemoveBlock(ref _copycontent, "permission-url-credit");

                TemplateHandler.Assign(ref _copycontent, "custid", dr["custid"]);
                TemplateHandler.Assign(ref _copycontent, "usn", dr["username"]);
                TemplateHandler.Assign(ref _copycontent, "sitelist", site_list);

                if (CastAide.AsDecimal(isbalance) == 0)
                {
                    string strTemp = string.Empty;
                    strTemp = (Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowedTotalBalanceTransfer && AppSettings.IsAgentSite ? "<font color='#1e1eeb'>" + isbalance + "<font>" : isbalance);
                    TemplateHandler.ReplaceBlock(ref _copycontent, "limit-link-agent", strTemp);
                }
                else
                {
                    string strTemp = (CastAide.AsDecimal(isbalance) < 0 ? "<font color='red'>" + isbalance + "</font>" : isbalance);
                    if (Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowedTotalBalanceTransfer && AppSettings.IsAgentSite)
                    {
                        TemplateHandler.Assign(ref _copycontent, "balance_value", strTemp);
                    }
                    else
                    {
                        TemplateHandler.ReplaceBlock(ref _copycontent, "limit-link-agent", strTemp);
                    }
                }

                TemplateHandler.Assign(ref _copycontent, "betcredit_value", FormatHandler.FormatNumber2(CastAide.AsDouble(dr["betcredit"]), Sunplus.Agent.Model.Entities.UserSession.Current.LangKey));
                TemplateHandler.Assign(ref _copycontent, "outtxn_value", FormatHandler.FormatNumber2(CastAide.AsDouble(dr["outstanding"]), Sunplus.Agent.Model.Entities.UserSession.Current.LangKey));

                TemplateHandler.Assign(ref _copycontent, "memberturnover_value", FormatHandler.FormatNumber2(dr["turnover"], Sunplus.Agent.Model.Entities.UserSession.Current.LangKey));
                TemplateHandler.Assign(ref _copycontent, "lastlogin_value", string.IsNullOrEmpty(dr["lastlogged"] + string.Empty) ? "-" : FormatHandler.FormatDateTimeEn12(dr["lastlogged"]));
                TemplateHandler.Assign(ref _copycontent, "lastloginip_value", string.IsNullOrEmpty(dr["iplogin"] + string.Empty) ? "-" : dr["iplogin"]);
                rescontent.Append(_copycontent);
            }
            _result = rescontent.ToString();

            #region Generate Paging Section if yes
            StringBuilder pagingUrl = new StringBuilder();
            pagingUrl.Append("CreditBalance.aspx?");
            pagingUrl.Append("custid=" + custid);
            pagingUrl.Append("&search=" + search);
            pagingUrl.Append("&username=" + username);
            pagingUrl.Append("&status=" + status);
            pagingUrl.Append("&total=" + totalPages);


            pageList = Common.GenPagingFooter(pagingUrl.ToString(), totalPages, pageId, 14, site_list);

            GotoPageHtml = TemplateHandler.LoadTemplate("GotoPage.html");

            TemplateHandler.Assign(ref GotoPageHtml, "textgotopage", Resources.Lang.gotopage);
            TemplateHandler.Assign(ref GotoPageHtml, "textgo", Resources.Lang.go);
            TemplateHandler.Assign(ref pageList, "pagetext", Resources.Lang.tpage);

            TemplateHandler.Assign(ref GotoPageHtml, "Url", pagingUrl.ToString());
            TemplateHandler.Assign(ref GotoPageHtml, "Site", site_list);
            #endregion
        }
        else
        {
            TemplateHandler.Assign(ref _noinfo, "num-cols", "14");
            TemplateHandler.Assign(ref _noinfo, "noinfo", Resources.Lang.noInfo);
            _result = _noinfo;
        }
        TemplateHandler.Assign(ref _main, "rowcount", rowCount);

        TemplateHandler.ReplaceBlock(ref _main, "limit-content", _result);

        if (pageList.Length > 0)
        {
            TemplateHandler.Assign(ref _main, "pagenevigator", pageList);
            _main += GotoPageHtml;
        }
        else
        {
            TemplateHandler.RemoveBlock(ref _main, "limit-footer");
        }

        return _main;
    }
    #endregion

    public static string Customerlist(string custid, string masterid, string act_list, string username, int pageindex, int _status)
    {
        string main = TemplateHandler.LoadTemplate("MemberInfo/CustomerList.html");
        TemplateHandler.Assign(ref main, "Customer", Common.GetLevel(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId));

        TemplateHandler.Assign(ref main, "lblprint", Resources.Lang.print);

        TemplateHandler.Assign(ref main, "masterid", masterid);
        TemplateHandler.Assign(ref main, "custid_upline", custid);
        string _noinfo = TemplateHandler.LoadTemplate("NoInfo.html");
        TemplateHandler.Assign(ref main, "no", Resources.Lang.no);
        TemplateHandler.Assign(ref main, "status", Resources.Lang.status);
        TemplateHandler.Assign(ref main, "username", Resources.Lang.username);
        TemplateHandler.Assign(ref main, "nickname", Resources.Lang.nickname);
        TemplateHandler.Assign(ref main, "lastname", Resources.Lang.lastname);
        TemplateHandler.Assign(ref main, "firstname", Resources.Lang.firstname);
        TemplateHandler.Assign(ref main, "group", Resources.Lang.group);

        TemplateHandler.Assign(ref main, "comma1", Resources.Lang.acomm1);
        TemplateHandler.Assign(ref main, "comma1x2", Resources.Lang.acomm2);
        TemplateHandler.Assign(ref main, "comma2", Resources.Lang.acomm3);        
        TemplateHandler.Assign(ref main, "commp1", Resources.Lang.pcomm1);
        TemplateHandler.Assign(ref main, "commp1x2", Resources.Lang.pcomm2);
        TemplateHandler.Assign(ref main, "commp2", Resources.Lang.pcomm3);
        
        TemplateHandler.Assign(ref main, "lastloginip", Resources.Lang.lastloginIP);
        TemplateHandler.Assign(ref main, "createdate", Resources.Lang.CreatedDate);
        //Render Horseracing
        int roleid = 1;
        if (act_list == "MemberList")
        {
            roleid = 1;
        }
        
        if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing)
        {
            TemplateHandler.Assign(ref main, "horseracing", Resources.Lang.hr);
        }
        else
        {
            TemplateHandler.RemoveBlock(ref main, "limit-horseracing");
            TemplateHandler.RemoveBlock(ref main, "limit-horseracing-value");
        }

        if (UserPermission.IsShowCasino)
        {
            TemplateHandler.Assign(ref main, "lblCasino", Resources.Lang.casino);
        }
        else
        {
            TemplateHandler.RemoveBlock(ref main, "casino");
        }
        //end
        //Render p2p
        if (UserPermission.IsShowP2P)
        {
            TemplateHandler.Assign(ref main, "lblP2P", Resources.Lang.P2P);
        }
        else
        {
            TemplateHandler.RemoveBlock(ref main, "no-p2p");
            TemplateHandler.RemoveBlock(ref main, "edit-P2P");
        }

        
        if (act_list == "MemberList")
        {
            TemplateHandler.Assign(ref main, "password", Resources.Lang.edit);
            TemplateHandler.RemoveBlock(ref main, "limit-tr-custlist-normal");
            TemplateHandler.Assign(ref main, "positiontaking", Resources.Lang.positiontaking);
            TemplateHandler.Assign(ref main, "creditbalance", Resources.Lang.creditbalance);
            TemplateHandler.ReplaceBlock(ref main, "limit-customerlist", Resources.Lang.customerlist);

            TemplateHandler.Assign(ref main, "url-credit", "CreditBalance.aspx?act=" + act_list + "&custid=" + custid);
            TemplateHandler.Assign(ref main, "url-position", "../_MemberInfo/PositionTakingList/Sportbook/SportbookPositionTakingList.aspx?type=agent&custid=" + custid);

            TemplateHandler.Assign(ref main, "positiontaking", Resources.Lang.positiontaking);
            if (AppSettings.IsAgentSite)
            {
                //Render Horseracing
                int colspan4 = 8;
                if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing)
                {
                    colspan4 = 9;
                }
                if (UserPermission.IsShowCasino)
                    colspan4++;

                //end
                TemplateHandler.Assign(ref main, "agentlist", Resources.Lang.lblAgentList);
                TemplateHandler.Assign(ref main, "url-agentlist", "DownlineList.aspx");
                TemplateHandler.Assign(ref main, "colspan4", "2");
                TemplateHandler.Assign(ref main, "colspan2", "1");
            }
            else if (AppSettings.IsMasterSite)
            {
                //Render Horseracing
                int colspan4 = 8;
                if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing)
                {
                    colspan4 = 9;
                }
                if (UserPermission.IsShowCasino)
                    colspan4++;
                //end
                TemplateHandler.Assign(ref main, "agentlist", Resources.Lang.lblAgentList);
                TemplateHandler.Assign(ref main, "url-agentlist", "DownlineList.aspx");
                TemplateHandler.Assign(ref main, "colspan4", colspan4.ToString());
                TemplateHandler.RemoveBlock(ref main, "limit-space");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref main, "limit-agentlist");
                //Render Horseracing
                int colspan4 = 8;
                if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing)
                {
                    colspan4 = 9;
                }
                if (UserPermission.IsShowCasino)
                    colspan4++;
                //end
                TemplateHandler.Assign(ref main, "colspan4", colspan4.ToString());
                TemplateHandler.RemoveBlock(ref main, "limit-space");
            }
        }
        else
        {
            TemplateHandler.Assign(ref main, "password", Resources.Lang.password);
            TemplateHandler.RemoveBlock(ref main, "limit-tr-custlist");
            TemplateHandler.Assign(ref main, "lblcustomerlist", Resources.Lang.customerlist);
            //Render Horseracing
            int colspan4 = 5;
            if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing)
            {
                colspan4 = 6;
            }
            if (UserPermission.IsShowCasino)
                colspan4++;

            //end
            TemplateHandler.Assign(ref main, "colspan4", colspan4.ToString());
            TemplateHandler.Assign(ref main, "colspan2", "3");
        }

        TemplateHandler.Assign(ref main, "commission", Resources.Lang.commission);
        TemplateHandler.Assign(ref main, "edit", Resources.Lang.edit);

        string _content = TemplateHandler.CopyBlock(main, "limit-content");
        TemplateHandler.Assign(ref _content, "overcolor", AppSettings.RowBgOver);
        string _copycontent = "";
        string _resContent = "";
        string status = "";
        
        int totalpages =0;
        DbResult dbr = MemberInfo.GetDataCustomerList(custid, username, ref  totalpages, pageindex, _status);
        int counter = ((pageindex - 1) * totalpages) + 1;
        DataTable dtb = dbr.ResultSet.Tables[0];
        if (dtb.Rows.Count > 0)
        {
            StringBuilder strBuilder = new StringBuilder(50000);
            for (int i = 0; i < dtb.Rows.Count; i++)
            {
                _copycontent = _content;
                counter++;
                bool isClosed = CastAide.AsBoolean(dtb.Rows[i]["closed"]);
                bool isSuspended = CastAide.AsBoolean(dtb.Rows[i]["suspended"]);

                if (isClosed)
                    status = Resources.Lang.close;
                else
                {
                    if (isSuspended)
                        status = Resources.Lang.suspend;
                    else
                        status = Resources.Lang.open;
                }
                if (isClosed)
                {
                    TemplateHandler.Assign(ref _copycontent, "color", AppSettings.RowBgClosed);
                    TemplateHandler.Assign(ref _copycontent, "overcolor", AppSettings.RowBgOver);
                }
                else if (counter % 2 == 0)
                {
                    if (isSuspended)
                    {
                        TemplateHandler.Assign(ref _copycontent, "color", AppSettings.RowBgSuspended);
                    }
                    else
                    {
                        TemplateHandler.Assign(ref _copycontent, "color", AppSettings.RowBgEven);
                    }
                }
                else
                {
                    if (isSuspended)
                    {
                        TemplateHandler.Assign(ref _copycontent, "color", AppSettings.RowBgSuspended);
                    }
                    else
                    {
                        TemplateHandler.Assign(ref _copycontent, "color", AppSettings.RowBgOdd);
                    }
                }


                if (UserPermission.IsShowCasino)
                {
                    if (!CastAide.AsBoolean(dtb.Rows[i]["synccasino"]))
                    {
                        if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId - roleid == 1)
                        {
                            TemplateHandler.RemoveBlock(ref _copycontent, "no-casino");
                            // Edit/Sync Casino button
                            TemplateHandler.Assign(ref _copycontent, "divOp", "divSync");
                            TemplateHandler.Assign(ref _copycontent, "editCS", Resources.Lang.sync);
                        }
                        else
                        {
                            TemplateHandler.RemoveBlock(ref _copycontent, "casino");
                            TemplateHandler.RemoveBlock(ref _copycontent, "edit-CS");

                            TemplateHandler.Assign(ref _copycontent, "divOp", "divDisable");
                            TemplateHandler.Assign(ref _copycontent, "editCS", "");
                        }

                    }
                    else
                    {
                        TemplateHandler.RemoveBlock(ref _copycontent, "no-casino");
                        if ((Sunplus.Agent.Model.Entities.UserSession.Current.RoleId - roleid == 1) || (roleid == 1))
                        {


                            if (CastAide.AsBoolean(dtb.Rows[i]["disablecasino"]))
                            {
                                // Edit/Sync Casino button
                                TemplateHandler.Assign(ref _copycontent, "divOp", "divEditDisabled");
                            }
                            else
                            {
                                // Edit/Sync Casino button
                                TemplateHandler.Assign(ref _copycontent, "divOp", "divEdit");
                            }
                            TemplateHandler.Assign(ref _copycontent, "editCS", Resources.Lang.edit);
                        }
                        else
                        {
                            // Edit/Sync Casino button
                            TemplateHandler.Assign(ref _copycontent, "divOp", "divViewCS");
                            TemplateHandler.Assign(ref _copycontent, "editCS", Resources.Lang.view);
                        }
                    }
                }
                GenBingoForMemberList(ref _copycontent, dtb.Columns.Contains("syncbingo") ? dtb.Rows[i]["syncbingo"] : "0", roleid);

                TemplateHandler.Assign(ref _copycontent, "isClosed", isClosed.ToString().ToLower());
                TemplateHandler.Assign(ref _copycontent, "isSuspended", isSuspended.ToString().ToLower());
                TemplateHandler.Assign(ref _copycontent, "custid_value", dtb.Rows[i]["custid"].ToString());
                TemplateHandler.Assign(ref _copycontent, "roleid_value", roleid); // separate Sps

                TemplateHandler.Assign(ref _copycontent, "no_value", counter.ToString());
                TemplateHandler.RemoveBlock(ref _copycontent, "limit-super");
                TemplateHandler.Assign(ref _copycontent, "url_link", String.Format("javascript:viewCustOutSt('{0}', {1});", dtb.Rows[i]["username"].ToString(), dtb.Rows[i]["custid"].ToString()));


                //Statement.aspx
                if (act_list == "MemberList")
                {
                    TemplateHandler.Assign(ref _copycontent, "status_value", status);
                    TemplateHandler.ReplaceBlock(ref _copycontent, "limit-status", status);
                }
                else
                {
                    TemplateHandler.Assign(ref _copycontent, "status_value", status);
                }

                TemplateHandler.Assign(ref _copycontent, "username_value", dtb.Rows[i]["username"]);
                string nickname = CastAide.AsString(dtb.Rows[i]["nickname"], string.Empty);
                TemplateHandler.Assign(ref _copycontent, "nickname-value", nickname);
                TemplateHandler.Assign(ref _copycontent, "lastname_value", dtb.Rows[i]["lastname"]);
                TemplateHandler.Assign(ref _copycontent, "firstname_value", dtb.Rows[i]["firstname"]);
                TemplateHandler.Assign(ref _copycontent, "group_value", dtb.Rows[i]["ugroup"]);
                TemplateHandler.Assign(ref _copycontent, "comma1_value", FormatHandler.FormatNumber2(dtb.Rows[i]["discount"], Sunplus.Agent.Model.Entities.UserSession.Current.LangKey));
                TemplateHandler.Assign(ref _copycontent, "comma2_value", FormatHandler.FormatNumber2(dtb.Rows[i]["discountcs"], Sunplus.Agent.Model.Entities.UserSession.Current.LangKey));
                TemplateHandler.Assign(ref _copycontent, "comma1x2_value", FormatHandler.FormatNumber2(dtb.Rows[i]["discount1x2"], Sunplus.Agent.Model.Entities.UserSession.Current.LangKey));
                TemplateHandler.Assign(ref _copycontent, "commp1_value", FormatHandler.FormatNumber2(dtb.Rows[i]["playerdiscount"], Sunplus.Agent.Model.Entities.UserSession.Current.LangKey));
                TemplateHandler.Assign(ref _copycontent, "commp2_value", FormatHandler.FormatNumber2(dtb.Rows[i]["playerdiscountcs"], Sunplus.Agent.Model.Entities.UserSession.Current.LangKey));
                TemplateHandler.Assign(ref _copycontent, "commp1x2_value", FormatHandler.FormatNumber2(dtb.Rows[i]["playerdiscount1x2"], Sunplus.Agent.Model.Entities.UserSession.Current.LangKey));
                TemplateHandler.Assign(ref _copycontent, "createdate_value", CastAide.AsString(dtb.Rows[i]["createdate"]));
                TemplateHandler.Assign(ref _copycontent, "lastloginip_value", dtb.Rows[i]["lastloginip"]);
                TemplateHandler.Assign(ref _copycontent, "chkid_value", dtb.Rows[i]["custid"] + ";" + dtb.Rows[i]["username"]);
                if (act_list == "MemberList")
                {
                    TemplateHandler.Assign(ref _copycontent, "edit", Resources.Lang.edit + " " + Resources.Lang.member);
                    TemplateHandler.RemoveBlock(ref _copycontent, "limit-reset-pass");
                }
                else
                {
                    TemplateHandler.Assign(ref _copycontent, "edit", Resources.Lang.edit);
                    TemplateHandler.RemoveBlock(ref _copycontent, "limit-reset-edit-member");
                }
                TemplateHandler.Assign(ref _copycontent, "custid_value", dtb.Rows[i]["custid"]);
                TemplateHandler.Assign(ref _copycontent, "custidsuper", dtb.Rows[i]["custid"]);
                //render Horseracing
                if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing)
                {
                    TemplateHandler.Assign(ref _copycontent, "horseracing_value", Resources.Lang.edit);
                }
                strBuilder .Append(_copycontent);
            }
            _resContent = strBuilder.ToString();
        }
        else
        {
            //Render Horseracing
            int numcols = 18; //22May09 - 16
            if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing)
            {
                numcols = 19; //22May09 - 17
            }
            if (UserPermission.IsShowCasino)
                numcols++;
            //end
            TemplateHandler.Assign(ref _noinfo, "num-cols", numcols.ToString());
            TemplateHandler.Assign(ref _noinfo, "noinfo", Resources.Lang.noInfo);
            _resContent = _noinfo;
        }
        TemplateHandler.ReplaceBlock(ref main, "limit-content", _resContent);
        return main;
    }

    public static string MasterAgentList(string custid, string masterid, bool isSuper, string actlist, string username, int pageindex, int _status, string search, int totalpages) //for super, master
    {
        string main = TemplateHandler.LoadTemplate("MemberInfo\\MasterAgentList.html");

        // Format language 
        TemplateHandler.Assign(ref main, "lblprint", Resources.Lang.print);
        TemplateHandler.Assign(ref main, "commission", Resources.Lang.commission);
        TemplateHandler.Assign(ref main, "no", Resources.Lang.no);
        TemplateHandler.Assign(ref main, "status", Resources.Lang.status);
        TemplateHandler.Assign(ref main, "commission", Resources.Lang.commission);
        TemplateHandler.Assign(ref main, "minposition", Resources.Lang.sbminpt);
        TemplateHandler.Assign(ref main, "username", Resources.Lang.username);
        TemplateHandler.Assign(ref main, "firstname", Resources.Lang.firstname);
        TemplateHandler.Assign(ref main, "lastname", Resources.Lang.lastname);
        TemplateHandler.Assign(ref main, "member", Resources.Lang.member);
        TemplateHandler.Assign(ref main, "group", Resources.Lang.group);
        TemplateHandler.Assign(ref main, "1x2group", Resources.Lang.lbl1x2);
        TemplateHandler.Assign(ref main, "other", Resources.Lang.other);
        TemplateHandler.Assign(ref main, "lastloginip", Resources.Lang.lastloginIP);
        TemplateHandler.Assign(ref main, "transfer", Resources.Lang.transfer);
        TemplateHandler.Assign(ref main, "password", Resources.Lang.password);
        TemplateHandler.Assign(ref main, "custid_upline", custid);
        TemplateHandler.Assign(ref main, "createdate", Resources.Lang.CreatedDate);
        if (UserPermission.IsShowRacing)
        {
            TemplateHandler.Assign(ref main, "horseracing", Resources.Lang.hr);
        }
        //If set enable number game on webconfgi
        GenHeaderNumberGame(ref main, 7);
        
        GenerateSearch(ref main, actlist, _status, username, search);
        TemplateHandler.Assign(ref main, "cus", custid);

        int roleid = 2;
        
        //end

        //Render casino
        if (UserPermission.IsShowCasino)
        {
            TemplateHandler.Assign(ref main, "lblCasino", Resources.Lang.casino);
        }
        else
        {
            // Edit/Sync Casino button
            TemplateHandler.RemoveBlock(ref main, "no-casino");
            TemplateHandler.RemoveBlock(ref main, "casino");
            TemplateHandler.RemoveBlock(ref main, "edit-CS");
        }
        //END

        //Render p2p
        if (UserPermission.IsShowP2P)
        {
            TemplateHandler.Assign(ref main, "lblP2P", Resources.Lang.P2P);
        }
        else
        {
            TemplateHandler.RemoveBlock(ref main, "no-p2p");
            TemplateHandler.RemoveBlock(ref main, "edit-P2P");
        }

        //Render Bingo
        //if (!UserPermission.IsShowBingo)
        //{
        //    // Edit/Sync Bingo button
        //    TemplateHandler.RemoveBlock(ref main, "bingo");
        //    TemplateHandler.RemoveBlock(ref main, "no-bingo");
        //    TemplateHandler.RemoveBlock(ref main, "edit-BG");
        //}
        //else
        //{
            TemplateHandler.Assign(ref main, "lblBingo", Resources.Bingo.bingo);
        //}

        if (AppSettings.IsSuperSite)
        {
            if (actlist == "AgentList")
            {
                roleid = 2;
            }
            else
            {
                roleid = 3;
            }
        }
                
        bool isDirectDL = UserSession.Current.RoleId - roleid == 1;

        TemplateHandler.Assign(ref main, "isdisablep2p", isDirectDL && UserPermission.IsShowP2P ? "0" : "1");
        if (AppSettings.IsSuperSite)
        {
            if (isSuper)
            {
                TemplateHandler.Assign(ref main, "function_name_single", "EditMaster_Single");
                TemplateHandler.Assign(ref main, "function_name_multi", "EditMaster_Multi");
            }
            else
            {
                if (actlist == "AgentList")
                {

                    TemplateHandler.RemoveBlock(ref main, "limit-edit");
                    TemplateHandler.RemoveBlock(ref main, "limit-transfer");
                    
                }
                TemplateHandler.Assign(ref main, "function_name_single", "EditAgent_Single");
                TemplateHandler.Assign(ref main, "function_name_multi", "EditAgent_Multi");
                
            }
        }
        else if (AppSettings.IsMasterSite)
        {
            TemplateHandler.Assign(ref main, "function_name_single", "EditAgent_Single");
            TemplateHandler.Assign(ref main, "function_name_multi", "EditAgent_Multi");
            string conditiontime = MemberInfo.GetDataTransferCondition(Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString());
            if (conditiontime.IndexOf("1") != -1)
            {
                TemplateHandler.RemoveBlock(ref main, "limit-transfer");
            }
            
        }
        else
        {
            TemplateHandler.Assign(ref main, "function_name_single", "EditAgent_Single");
            TemplateHandler.Assign(ref main, "function_name_multi", "EditAgent_Multi");
            TemplateHandler.RemoveBlock(ref main, "limit-transfer");
            
        }
        TemplateHandler.Assign(ref main, "lblMasterList", isSuper ? Resources.Lang.lblMasterList : Resources.Lang.lblAgentList);
        TemplateHandler.Assign(ref main, "noinfo", Resources.Lang.noInfo);

        DataTable dt = new DataTable();
        // Get data from DB
        DbResult dbr = null;
        //int totalpages =0;       
        string pageList = string.Empty;
        string GotoPageHtml = string.Empty;
        if (isSuper)
        {
            dbr = MemberInfo.GetDataSagentList(custid, username, ref totalpages, pageindex, _status);
        }
        else
        {
            dbr = MemberInfo.GetDataMagentList(custid, username, ref totalpages, pageindex, _status);
        }
        dt = dbr.ResultSet.Tables[0];

        // Format data
        string block_content = TemplateHandler.CopyBlock(main, "Limit-Content");

        if (dt.Rows.Count == 0) // no data
        {
            TemplateHandler.RemoveBlock(ref main, "Limit-Content");
            if (AppSettings.IsMasterSite)
            {
                string conditiontime = MemberInfo.GetDataTransferCondition(Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString());
                int colspan2 = 6;
                int numcol = 18;
                if (conditiontime.IndexOf("1") == -1)
                {
                    if (UserPermission.IsShowCasino)
                    {
                        colspan2++;
                        numcol++;
                    }
                }
                else
                {
                    colspan2 = 5;
                    numcol = 17;
                    if (UserPermission.IsShowCasino)
                    {
                        colspan2++;
                        numcol++;
                    }
                }

                if (UserPermission.IsShowP2P)
                {
                    colspan2++;
                    numcol++;
                }

                TemplateHandler.Assign(ref main, "colspan1", 3);
                TemplateHandler.Assign(ref main, "colspan2", colspan2.ToString());
                TemplateHandler.Assign(ref main, "numcol", numcol.ToString());
            }
            else
            {
                if (actlist == "AgentList")
                {
                    //render hr
                    int colspan1 = 8;
                    int colspan2 = 3;
                    int numcol = 20;
                    string disabledCheckAll = "disabled='disabled'";
                    TemplateHandler.Assign(ref main, "disabledCheckAll", disabledCheckAll);

                    if (UserPermission.IsShowCasino)
                    {
                        colspan1++;
                    }

                    if (UserPermission.IsShowP2P)
                    {
                        colspan1++;
                    }
                    
                    TemplateHandler.RemoveBlock(ref main, "limit");
                    TemplateHandler.Assign(ref main, "colspan1", colspan1.ToString());
                    TemplateHandler.Assign(ref main, "colspan2", colspan2.ToString());
                    TemplateHandler.Assign(ref main, "numcol", numcol);
                }
                else
                {
                    int colspan2 = 6;
                    if (UserPermission.IsShowCasino)
                    {
                        colspan2++;
                    }

                    if (UserPermission.IsShowP2P)
                    {
                        colspan2++;                     
                    }
                    TemplateHandler.Assign(ref main, "colspan1", 3);
                    TemplateHandler.Assign(ref main, "colspan2", colspan2.ToString());
                }
            }
            TemplateHandler.RemoveBlock(ref main, "limit-footer");
            return main;
        }
        else
        {
            TemplateHandler.RemoveBlock(ref main, "Limit-Not-Found");
            
        }

        StringBuilder rows = new StringBuilder(50000);
        string status = string.Empty, temp_content = string.Empty, url_link = string.Empty, odd_even_row = "even_row";
        int i = ((pageindex - 1) * AppSettings.PageSize);
        //AppSettings.PageSize
        if (AppSettings.IsMasterSite)
            url_link = "javascript:viewStatement({custid}, 'Agent');";
        else
        {
            if (actlist == "AgentList")
                url_link = "javascript:viewStatement({custid}, 'Agent');";
            else
                url_link = "javascript:viewStatement({custid}, 'Master');";
        }

        if (AppSettings.IsSuperSite)
        {
            if (actlist == "AgentList")
            {

                TemplateHandler.Assign(ref block_content, "status_value", "{status}");
                TemplateHandler.ReplaceBlock(ref block_content, "limit-open-status", "{status}");
                TemplateHandler.Assign(ref block_content, "url_view", "javascript:viewDownlineList('MemberList', {custid}," + masterid + ")");

                TemplateHandler.RemoveBlock(ref block_content, "limit-edit");
                TemplateHandler.RemoveBlock(ref block_content, "limit-transfer");
                //render hr
                int colspan1 = 7;
                string disabledCheckAll = "disabled='disabled'";
                TemplateHandler.Assign(ref main, "disabledCheckAll", disabledCheckAll);

                if (UserPermission.IsShowRacing) colspan1++;
                if (UserPermission.IsShowCasino) colspan1++;
                //if (UserPermission.IsShowBingo) colspan1++;
                if (UserPermission.IsShowP2P) colspan1++;

                TemplateHandler.Assign(ref main, "colspan1", colspan1.ToString());
                TemplateHandler.RemoveBlock(ref main, "limit");

            }
            else
            {
                TemplateHandler.Assign(ref block_content, "status_value", "{status}");
                TemplateHandler.Assign(ref block_content, "url_view", "javascript:viewDownlineList('AgentList', {custid}, {custid})");

                //render hr
                int colspan2 = 7;
                string disabledCheckAll = "disabled='disabled'";
                TemplateHandler.Assign(ref main, "disabledCheckAll", disabledCheckAll);
                if (UserPermission.IsShowCasino) colspan2++;
                
                TemplateHandler.Assign(ref main, "colspan1", "3");
                TemplateHandler.Assign(ref main, "colspan2", colspan2.ToString());
            }
        }
        else
        {
            TemplateHandler.Assign(ref block_content, "status_value", "{status}");
            TemplateHandler.Assign(ref block_content, "url_view", "javascript:viewDownlineList('MemberList', {custid})");

            // render HR
            string conditiontime = MemberInfo.GetDataTransferCondition(Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString());
            int colspan2 = 6;
            if (conditiontime.IndexOf("1") == -1)
            {
                if (UserPermission.IsShowCasino) colspan2++;
            }
            else
            {
                if (UserPermission.IsShowCasino)
                    colspan2++;
                else
                    colspan2--;

            }
            TemplateHandler.Assign(ref main, "colspan1", "3");
            TemplateHandler.Assign(ref main, "colspan2", colspan2.ToString());
        }

        TemplateHandler.Assign(ref block_content, "edit", Resources.Lang.edit);
        GenStatementLink(ref block_content, url_link);
        TemplateHandler.Assign(ref block_content, "view", Resources.Lang.view);
        TemplateHandler.Assign(ref block_content, "overcolor", AppSettings.RowBgOver);
        TemplateHandler.Assign(ref main, "totalpages", totalpages.ToString());
        foreach (DataRow dr in dt.Rows)
        {
            i++;
            odd_even_row = (odd_even_row == "even_row") ? "odd_row" : "even_row";
            temp_content = block_content.Clone().ToString();
            bool isClosed = CastAide.AsBoolean(dr["closed"]);
            bool isSuspended = CastAide.AsBoolean(dr["suspended"]);
            bool isDisableOutright = CastAide.AsBoolean(dr["DisableOutright"]);
            bool isEnable = CastAide.AsBoolean(dr["enabled"]);
            bool isDisableNumber = dr.Table.Columns.Contains("disablenumber") ? CastAide.AsBoolean(dr["disablenumber"]) : true;

            TemplateHandler.Assign(ref temp_content, "isClosed", isClosed.ToString().ToLower());
            TemplateHandler.Assign(ref temp_content, "isSuspended", isSuspended.ToString().ToLower());
            TemplateHandler.Assign(ref temp_content, "isOutright", (!isDisableOutright).ToString().ToLower());
            TemplateHandler.Assign(ref temp_content, "isAllowNumber", (!isDisableNumber).ToString().ToLower());

            if (isEnable)
            {
                if (isClosed)
                {
                    status = Resources.Lang.close;
                    TemplateHandler.Assign(ref temp_content, "color", AppSettings.RowBgClosed);
                }
                else if (isSuspended)
                {
                    status = Resources.Lang.suspend;
                    TemplateHandler.Assign(ref temp_content, "color", AppSettings.RowBgSuspended);
                }              
                else
                {
                    status = Resources.Lang.open;
                    TemplateHandler.Assign(ref temp_content, "color", AppSettings.RowBgOdd);
                }
            }
            else
            {
                status = Resources.Lang.disabled;
                TemplateHandler.Assign(ref temp_content, "color", AppSettings.RowBgDisabled);
            }

            
            //Render Casino
            GenCasinoForAgentList(ref temp_content, dr["synccasino"], roleid);

            //Render p2p
            if (UserPermission.IsShowP2P)
            {
                GenP2P(ref temp_content, dr, roleid);
            }
            
            //Render Bingo
            //if (UserPermission.IsShowBingo)
            //{
            //    GenBingoForAgentList(ref temp_content, dt.Columns.Contains("syncbingo") ? dr["syncbingo"] : "0", roleid);
            //}
            //else
            //{
                TemplateHandler.RemoveBlock(ref temp_content, "bingo");
                TemplateHandler.RemoveBlock(ref temp_content, "no-bingo");
            //}

            TemplateHandler.Assign(ref temp_content, "no_value", i);
            TemplateHandler.Assign(ref temp_content, "status", status);
            TemplateHandler.Assign(ref temp_content, "custid", dr["custid"]);
            TemplateHandler.Assign(ref temp_content, "roleid_value", roleid); // separate SPs 
            TemplateHandler.Assign(ref temp_content, "username_value", dr["username"].ToString());
            TemplateHandler.Assign(ref temp_content, "firstname_value", dr["firstname"].ToString());
            TemplateHandler.Assign(ref temp_content, "lastname_value", dr["lastname"].ToString());
            TemplateHandler.Assign(ref temp_content, "custid_value", dr["custid"].ToString());
            TemplateHandler.Assign(ref temp_content, "actionOn", (actlist == "AgentList") ? "ViewAgent" : "Master");
            TemplateHandler.Assign(ref temp_content, "chkid_value", dr["custid"].ToString() + ";" + dr["username"].ToString());

            TemplateHandler.Assign(ref temp_content, "groupA_value", FormatHandler.FormatNumber4(dr["groupa"], FormatHandler.CULT_NAME_ENG));
            TemplateHandler.Assign(ref temp_content, "groupB_value", FormatHandler.FormatNumber4(dr["groupb"], FormatHandler.CULT_NAME_ENG));
            TemplateHandler.Assign(ref temp_content, "groupC_value", FormatHandler.FormatNumber4(dr["groupc"], FormatHandler.CULT_NAME_ENG));
            TemplateHandler.Assign(ref temp_content, "groupD_value", FormatHandler.FormatNumber4(dr["groupd"], FormatHandler.CULT_NAME_ENG));
            TemplateHandler.Assign(ref temp_content, "group1x2_value", FormatHandler.FormatNumber4(dr["discount1x2"], FormatHandler.CULT_NAME_ENG));
            TemplateHandler.Assign(ref temp_content, "other_value", FormatHandler.FormatNumber4(dr["discountcs"], FormatHandler.CULT_NAME_ENG));
            TemplateHandler.Assign(ref temp_content, "createdate_value", CastAide.AsString(dr["createdate"]));
            TemplateHandler.Assign(ref temp_content, "lastloginip_value", dr["lastloginip"].ToString());
            TemplateHandler.Assign(ref temp_content, "edit", Resources.Lang.edit);
            TemplateHandler.Assign(ref temp_content, "password", Resources.Lang.password);
            TemplateHandler.Assign(ref temp_content, "custid_value", dr["custid"].ToString());
            TemplateHandler.Assign(ref temp_content, "row-over", "live_row");
            TemplateHandler.Assign(ref temp_content, "divHR", "divViewCS");
            //If enable number game on webconfig
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission)
                TemplateHandler.RemoveBlock(ref temp_content, "disable-number-game");
            else
                TemplateHandler.Assign(ref temp_content, "numbergame_value", FormatHandler.FormatNumber4(dr["discountnumber"], FormatHandler.CULT_NAME_ENG));
                

            rows.Append(temp_content);
        }
        TemplateHandler.ReplaceBlock(ref main, "Limit-Content", rows.ToString());


        if (dt.Rows.Count > 0)
        {
            #region Generate Paging Section if yes
            StringBuilder url = new StringBuilder();
            url.Append("DownlineList.aspx?");
            url.Append("custid=" + custid);
            url.Append("&search=" + search);
            url.Append("&username=" + username);
            url.Append("&status=" + status);
            url.Append("&total=" + totalpages);
            int cols = 16;
            if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsCasinoDisabled)
                cols++;

            if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsDisabledFI)
                cols++;

            if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsDisabledHR)
                cols++;

            pageList = Common.GenPagingFooter(url.ToString(), totalpages, pageindex, cols, actlist);
            GotoPageHtml = TemplateHandler.LoadTemplate("GotoPage.html");

            TemplateHandler.Assign(ref GotoPageHtml, "textgotopage", Resources.Lang.gotopage);
            TemplateHandler.Assign(ref GotoPageHtml, "textgo", Resources.Lang.go);
            TemplateHandler.Assign(ref pageList, "pagetext", Resources.Lang.tpage);

            TemplateHandler.Assign(ref GotoPageHtml, "Url", url.ToString());
            TemplateHandler.Assign(ref GotoPageHtml, "Site", actlist);
            if (pageList.Length > 0)
            {
                TemplateHandler.Assign(ref main, "pagenevigator", pageList);
                main += GotoPageHtml;
            }
            else
            {
                TemplateHandler.RemoveBlock(ref main, "limit-footer");
            }
            #endregion
        }
        if (!Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing)
        {
            TemplateHandler.RemoveBlock(ref main, "edit-HR");
        }
        return main;
    }

    #region Sub-Account List
    public static string SubAccountList(string custId)
    {
        string strMain = TemplateHandler.LoadTemplate("MemberInfo\\SubAccList.html");
        //int j = 0;
        string strContent = string.Empty;
        int c = 0;
        TemplateHandler.Assign(ref strMain, "no", Resources.Lang.no);
        TemplateHandler.Assign(ref strMain, "edit", Resources.Lang.edit);
        TemplateHandler.Assign(ref strMain, "username", Resources.Lang.username);
        TemplateHandler.Assign(ref strMain, "firstname", Resources.Lang.firstname);
        TemplateHandler.Assign(ref strMain, "lastname", Resources.Lang.lastname);
        TemplateHandler.Assign(ref strMain, "totalbets", Resources.Lang.totalbets);
        TemplateHandler.Assign(ref strMain, "reports", Resources.Lang.reports);
        TemplateHandler.Assign(ref strMain, "createaccount", Resources.Lang.createaccount);
        TemplateHandler.Assign(ref strMain, "updateaccount", Resources.Lang.updateaccount);
        TemplateHandler.Assign(ref strMain, "transfer", Resources.Lang.transfer);
        TemplateHandler.Assign(ref strMain, "password", Resources.Lang.password);
        TemplateHandler.Assign(ref strMain, "noinfo", Resources.Lang.noInfo);
        TemplateHandler.Assign(ref strMain, "createdate", Resources.Lang.CreatedDate);

        DataTable dt = new DataTable();
        // Get data from DB
        DbResult dbr = MemberInfo.GetCustSubSel(custId);
        dt = dbr.ResultSet.Tables[0];
        c = dt.Rows.Count;

        if (c == 0)
        {
            TemplateHandler.RemoveBlock(ref strMain, "Limit-Content");
            return strMain;
        }
        else
        {
            TemplateHandler.RemoveBlock(ref strMain, "Limit-Not-Found");
        }

        string strTemp = TemplateHandler.CopyBlock(strMain, "Limit-Content");
        TemplateHandler.Assign(ref strTemp, "overcolor", AppSettings.RowBgOver);
        for (int i = 0; i < c; i++)
        {
            string strTempBlock = strTemp;
            TemplateHandler.Assign(ref strTempBlock, "no_value", (i + 1));
            TemplateHandler.Assign(ref strTempBlock, "username_value", dt.Rows[i]["username"]);
            TemplateHandler.Assign(ref strTempBlock, "firstname_value", dt.Rows[i]["firstname"]);
            TemplateHandler.Assign(ref strTempBlock, "lastname_value", dt.Rows[i]["lastname"]);
            TemplateHandler.Assign(ref strTempBlock, "createdate_value", CastAide.AsString(dt.Rows[i]["createdate"]));
            TemplateHandler.Assign(ref strTempBlock, "id_value", dt.Rows[i]["subaccid"]);

            string strPageAccess = dt.Rows[i]["PageAccess"].ToString();
            if (strPageAccess.IndexOf("A") != -1)
                TemplateHandler.Assign(ref strTempBlock, "checkedA", " checked");
            else
                TemplateHandler.Assign(ref strTempBlock, "checkedA", "");
            if (strPageAccess.IndexOf("B") != -1)
                TemplateHandler.Assign(ref strTempBlock, "checkedB", " checked");
            else
                TemplateHandler.Assign(ref strTempBlock, "checkedB", "");
            if (strPageAccess.IndexOf("C") != -1)
                TemplateHandler.Assign(ref strTempBlock, "checkedC", " checked");
            else
                TemplateHandler.Assign(ref strTempBlock, "checkedC", "");
            if (strPageAccess.IndexOf("D") != -1)
                TemplateHandler.Assign(ref strTempBlock, "checkedD", " checked");
            else
                TemplateHandler.Assign(ref strTempBlock, "checkedD", "");
            if (strPageAccess.IndexOf("E") != -1)
                TemplateHandler.Assign(ref strTempBlock, "checkedE", " checked");
            else
                TemplateHandler.Assign(ref strTempBlock, "checkedE", "");

            if (Convert.ToInt32(dt.Rows[i]["closed"]) != 0)
            {
                TemplateHandler.Assign(ref strTempBlock, "color", AppSettings.RowBgClosed);
            }
            else if (i % 2 == 0)
            {
                TemplateHandler.Assign(ref strTempBlock, "color", AppSettings.RowBgEven);
            }
            else
            {
                TemplateHandler.Assign(ref strTempBlock, "color", AppSettings.RowBgOdd);
            }
            strContent += strTempBlock;
        }
        TemplateHandler.ReplaceBlock(ref strMain, "Limit-Content", strContent);

        return strMain;
    }
    #endregion

    #region Casino Position Taking list
    public static string getCasinoPositionTaking(string type, string custid)
    {
        string content = string.Empty;
        string[] wildcard = new string[] { "a" };
        bool IsEditMember = false;
        Report report = new Report();
        DataTable dataTable = null;
        int count = 1;
        string _custid = "-1";
        string _username = "";
        string _url = "";
        string url = "";
        string viewType = "";
        int intCustID = CastAide.AsInt32(custid);
        string notfound = TemplateHandler.LoadTemplate("NoInfo.html");
        string main = TemplateHandler.LoadTemplate("MemberInfo/CasinoPositionTaking.html");
        string gameKind = TemplateHandler.CopyBlock(main, "GameKind");
        string gameType = TemplateHandler.CopyBlock(main, "GameType");
        string rowData = TemplateHandler.CopyBlock(main, "rowdata");
        StringBuilder _gameKind = new StringBuilder();
        StringBuilder _gameType = new StringBuilder();
        StringBuilder _rowData = new StringBuilder();

        //for regular expression
        Regex re = new Regex(@"\{\d{1,2}_\d{1,2}\}");
        MatchCollection matches ;



        #region Get Header

        TemplateHandler.Assign(ref main, "lblPositionTaking", Resources.Lang.casinoPositionTaking);
        TemplateHandler.Assign(ref main, "lblUserName", Resources.Lang.username);

        //apply class name for Role
        TemplateHandler.Assign(ref main, "firstCol", "firstCol");

        //apply over color
        TemplateHandler.Assign(ref main, "overcolor", AppSettings.RowBgOver);


        //Create template
        //Load Game Kind
        #region Load Game Kind
        int colCount = 0;
        BasicInfo bi = new BasicInfo();
        DataTable tblGameKind = bi.GetGameKind("01");
        if (tblGameKind != null && tblGameKind.Rows.Count != 0)
        {
            //Load Game Type
            #region Load Game Type
            foreach (DataRow dr in tblGameKind.Rows)
            {
                DataTable tblGameType = bi.GetGameType(CastAide.AsInt32(dr["CodeID"]));
                if (tblGameType != null && tblGameType.Rows.Count != 0)
                {
                    foreach (DataRow dr2 in tblGameType.Rows)
                    {
                        string gameTypeTpl = gameType;
                        TemplateHandler.Assign(ref gameTypeTpl, "GameTypeClass", "class='colHdr'");
                        TemplateHandler.Assign(ref gameTypeTpl, "lblGameType", dr2[DAL.APIClient.APICasinoProvider.Lang("GameTypeName")]);
                        _gameType.Append(gameTypeTpl);

                        string rowDataTpl = rowData;
                        TemplateHandler.Assign(ref rowDataTpl, "rowdata", "{" + CastAide.AsInt32(dr["CodeID"]).ToString() + "_" + CastAide.AsInt32(dr2["GameTypeID"]).ToString() + "}");
                        TemplateHandler.Assign(ref rowDataTpl, "normalCol", "#424242");
                        _rowData.Append(rowDataTpl);

                        //count column data
                        colCount++;
                    }
                    string gameKindTpl = gameKind;
                    TemplateHandler.Assign(ref gameKindTpl, "GameKindColSpan", "colspan='" + tblGameType.Rows.Count.ToString() + "'");
                    TemplateHandler.Assign(ref gameKindTpl, "lblGameKind", dr[DAL.APIClient.APICasinoProvider.Lang("CodeName")]);
                    _gameKind.Append(gameKindTpl);
                }
            }
            #endregion

            TemplateHandler.ReplaceBlock(ref main, "GameKind", _gameKind.ToString());
            TemplateHandler.ReplaceBlock(ref main, "GameType", _gameType.ToString());
            TemplateHandler.ReplaceBlock(ref main, "rowdata", _rowData.ToString());
        }
        else
        {
            //remove header
            TemplateHandler.RemoveBlock(ref main, "header");

        }
        #endregion
        #endregion

        #region Get Content
        if (AppSettings.IsAgentSite)
        {
            IsEditMember = true;
            dataTable = report.PTByAgent(intCustID);
            wildcard = new string[] { "a" };
        }
        else
        {
            if (AppSettings.IsMasterSite)
            {
                if (type == "")
                {
                    IsEditMember = false;
                    wildcard = new string[] { "m", "a" };
                    dataTable = report.PTByMaster(intCustID);
                    viewType = "agent";
                }
                else if (type == "agent")
                {
                    IsEditMember = true;
                    wildcard = new string[] { "m", "a" };
                    dataTable = report.PTByAgent(intCustID);
                }
            }
            else if (AppSettings.IsSuperSite)
            {
                if (type == "")
                {
                    IsEditMember = false;
                    wildcard = new string[] { "s" };
                    dataTable = report.PTBySuper(intCustID);
                    viewType = "master";
                }
                else if (type == "master")
                {
                    IsEditMember = false;
                    wildcard = new string[] { "m", "a" };
                    dataTable = report.PTByMaster(intCustID);
                    viewType = "agent";
                }
                else if (type == "agent")
                {
                    IsEditMember = true;
                    wildcard = new string[] { "s", "m", "a" };
                    dataTable = report.PTByAgent(intCustID);
                }
            }
        }

        //should remove Role when wildcard.length = 1
        if (wildcard.Length == 1)
        {
            TemplateHandler.RemoveBlock(ref main, "AdditionalRow");
            TemplateHandler.RemoveBlock(ref main, "Role");
            TemplateHandler.Assign(ref main, "colNum", "2");
            TemplateHandler.Assign(ref main, "colNum2", "1");
        }
        else
        {
            TemplateHandler.Assign(ref main, "colNum", "3");
            TemplateHandler.Assign(ref main, "colNum2", "2");
        }

        //copy block row
        string row = TemplateHandler.CopyBlock(main, "row");
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            StringBuilder _s = new StringBuilder();
            string _row = string.Empty;
            string _row2 = string.Empty;
            string _row3 = string.Empty;
            

            //provide link to edit member if in agent level
            if (IsEditMember)
            {
                TemplateHandler.RemoveBlock(ref row, "link");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref row, "editmember");
            }


            #region Apply PT from API
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (_custid != CastAide.AsString(dataTable.Rows[i]["ibccustid"]))
                {
                    if (re.IsMatch(_row))
                    {
                        matches = re.Matches(_row);
                        foreach (Match match in matches)
                        {
                            TemplateHandler.Assign(ref _row, match.Value, "-");
                        }
                            
                    }

                    _s.Append(_row);
                    if (wildcard.Length > 1)
                    {
                        if (re.IsMatch(_row2))
                        {
                            matches = re.Matches(_row2);
                            foreach (Match match in matches)
                            {
                                TemplateHandler.Assign(ref _row2, match.Value, "-");
                            }

                        }
                        _s.Append(_row2);
                        if (wildcard.Length == 3)
                        {
                            if (re.IsMatch(_row3))
                            {
                                matches = re.Matches(_row3);
                                foreach (Match match in matches)
                                {
                                    TemplateHandler.Assign(ref _row3, match.Value, "-");
                                }

                            }

                            _s.Append(_row3);
                        }
                    }


                    _username = CastAide.AsString(dataTable.Rows[i]["UserName"]);
                    _custid = CastAide.AsString(dataTable.Rows[i]["ibccustid"]);

                    _row = row;
                    if (Convert.ToInt32(dataTable.Rows[i]["closed"]) != 0)
                        TemplateHandler.Assign(ref _row, "color", AppSettings.RowBgClosed);
                    else if (Convert.ToInt32(dataTable.Rows[i]["suspended"]) != 0)
                        TemplateHandler.Assign(ref _row, "color", AppSettings.RowBgSuspended);
                    else if (count % 2 != 0) //Apply Odd-Even Row
                        TemplateHandler.Assign(ref _row, "color", AppSettings.RowBgEven);
                    else
                        TemplateHandler.Assign(ref _row, "color", AppSettings.RowBgOdd);

                    //Edit User position taking
                    TemplateHandler.Assign(ref _row, "custid_value", _custid);

                    //apply data for first row
                    TemplateHandler.Assign(ref _row, "No", count);
                    TemplateHandler.Assign(ref _row, "type", viewType);
                    TemplateHandler.Assign(ref _row, "UserName_vl", CastAide.AsString(_username));
                    TemplateHandler.Assign(ref _row, "rowspan", "rowspan='" + wildcard.Length.ToString() + "'");

                    if (wildcard.Length > 1)
                    {
                        _row2 = _row;
                        //Remove NonRepeat td for second row
                        TemplateHandler.RemoveBlock(ref _row2, "NonRepeat");

                        if (wildcard.Length == 3)
                        {
                            _row3 = _row;
                            TemplateHandler.RemoveBlock(ref _row3, "NonRepeat");
                            TemplateHandler.Assign(ref _row3, "Role", wildcard[2].ToUpperInvariant());
                        }

                        //Apply Role
                        TemplateHandler.Assign(ref _row, "Role", wildcard[0].ToUpperInvariant());
                        TemplateHandler.Assign(ref _row2, "Role", wildcard[1].ToUpperInvariant());

                    }
                    count++;
                }


                //Apply position taking data from API
                //First Row
                TemplateHandler.Assign(ref _row, CastAide.AsString(dataTable.Rows[i]["GameKind"]) + "_" + CastAide.AsString(dataTable.Rows[i]["GameTypeID"]), (wildcard[0].ToUpperInvariant() == "S") ? NumberFormat.DecFormat(dataTable.Rows[i]["SPositionTaking"], 2) : ((wildcard[0].ToUpperInvariant() == "M") ? NumberFormat.DecFormat(dataTable.Rows[i]["MPositionTaking"], 2) : NumberFormat.DecFormat(dataTable.Rows[i]["APositionTaking"], 2)));

                //second Row
                if (wildcard.Length > 1)
                    TemplateHandler.Assign(ref _row2, CastAide.AsString(dataTable.Rows[i]["GameKind"]) + "_" + CastAide.AsString(dataTable.Rows[i]["GameTypeID"]), (wildcard[1].ToUpperInvariant() == "S") ? NumberFormat.DecFormat(dataTable.Rows[i]["SPositionTaking"], 2) : ((wildcard[1].ToUpperInvariant() == "M") ? NumberFormat.DecFormat(dataTable.Rows[i]["MPositionTaking"], 2) : NumberFormat.DecFormat(dataTable.Rows[i]["APositionTaking"], 2)));

                //third Row
                if (wildcard.Length == 3)
                    TemplateHandler.Assign(ref _row3, CastAide.AsString(dataTable.Rows[i]["GameKind"]) + "_" + CastAide.AsString(dataTable.Rows[i]["GameTypeID"]), (wildcard[2].ToUpperInvariant() == "S") ? NumberFormat.DecFormat(dataTable.Rows[i]["SPositionTaking"], 2) : ((wildcard[2].ToUpperInvariant() == "M") ? NumberFormat.DecFormat(dataTable.Rows[i]["MPositionTaking"], 2) : NumberFormat.DecFormat(dataTable.Rows[i]["APositionTaking"], 2)));

                //for end of row --> append data to string builder
                if (i == dataTable.Rows.Count - 1)
                {
                    if (re.IsMatch(_row))
                    {
                        matches = re.Matches(_row);
                        foreach (Match match in matches)
                        {
                            TemplateHandler.Assign(ref _row, match.Value, "-");
                        }

                    }
                    _s.Append(_row);
                    if (wildcard.Length > 1)
                    {
                        if (re.IsMatch(_row2))
                        {
                            matches = re.Matches(_row2);
                            foreach (Match match in matches)
                            {
                                TemplateHandler.Assign(ref _row2, match.Value, "-");
                            }

                        }
                        _s.Append(_row2);
                        if (wildcard.Length == 3)
                        {
                            if (re.IsMatch(_row3))
                            {
                                matches = re.Matches(_row3);
                                foreach (Match match in matches)
                                {
                                    TemplateHandler.Assign(ref _row3, match.Value, "-");
                                }

                            }

                            _s.Append(_row3);
                        }
                    }
                }
            }

            content = _s.ToString();
            #endregion

        }
        else
        {
            if (wildcard.Length > 1)
                colCount = colCount + 5;
            else
                colCount = colCount + 4;
            TemplateHandler.Assign(ref notfound, "noinfo", Resources.Lang.noInfo);
            TemplateHandler.Assign(ref notfound, "num-cols", colCount);
            content = notfound;
        }
        #endregion

        TemplateHandler.ReplaceBlock(ref main, "row", content);
        return main;
    }
    #endregion

    #region Bingo Position Taking List
    public static string GetBingoPositionTaking(string type, string custid)
    {
        #region Define variable
        //IResult resultCustomer = new Sunplus.Bingo.Entities.Result();
        //IResult resultGameKind = new Sunplus.Bingo.Entities.Result();
        //IResult resultGameType = new Sunplus.Bingo.Entities.Result();

        string content = string.Empty;
        string[] wildcard = new string[] { "a" };
        bool IsEditMember = false;
        Report report = new Report();
        DataTable dataTable = null;
        int count = 1;
        string _custid = "-1";
        string _username = "";
        string viewType = "";
        int intCustID = CastAide.AsInt32(custid);
        string notfound = TemplateHandler.LoadTemplate("NoInfo.html");
        string main = TemplateHandler.LoadTemplate("MemberInfo/BingoPositionTaking.html");
        string gameKind = TemplateHandler.CopyBlock(main, "GameKind");
        string gameType = TemplateHandler.CopyBlock(main, "GameType");
        string rowData = TemplateHandler.CopyBlock(main, "rowdata");
        StringBuilder _gameKind = new StringBuilder();
        StringBuilder _gameType = new StringBuilder();
        StringBuilder _rowData = new StringBuilder();

        //for regular expression
        Regex re = new Regex(@"\{\d{1,2}_\d{1,2}\}");
        MatchCollection matches;
        #endregion

        #region Get Header

        TemplateHandler.Assign(ref main, "lblPositionTaking", Resources.Lang.bingoPositionTaking);
        TemplateHandler.Assign(ref main, "lblUserName", Resources.Lang.username);

        //apply class name for Role
        TemplateHandler.Assign(ref main, "firstCol", "firstCol");
        //apply over color
        TemplateHandler.Assign(ref main, "overcolor", AppSettings.RowBgOver);


        //Create template
        //Load Game Kind
        #region Load Game Kind
        int colCount = 0;
        //ICustomerService customerService = BingoContainer.Current.Resolve<ICustomerService>();
        //DataTable tblGameKind = customerService.GetGameKindDataTable(BingoSettings.BingoGame.CodeGameNo, ref resultGameKind);
        //if (tblGameKind != null && tblGameKind.Rows.Count != 0)
        //{
        //    //Load Game Type
        //    #region Load Game Type
        //    foreach (DataRow drGameKind in tblGameKind.Rows)
        //    {
        //        DataTable tblGameType = customerService.GetGameTypeDataTable(CastAide.AsInt32(drGameKind["GameGroupID"]), ref resultGameType);
        //        if (tblGameType != null && tblGameType.Rows.Count != 0)
        //        {
        //            foreach (DataRow drGameType in tblGameType.Rows)
        //            {
        //                string gameTypeTpl = gameType;
        //                TemplateHandler.Assign(ref gameTypeTpl, "GameTypeClass", "class='colHdr'");
        //                TemplateHandler.Assign(ref gameTypeTpl, "lblGameType", drGameType[DAL.APIClient.APICasinoProvider.Lang("GameTypeName")]);
        //                _gameType.Append(gameTypeTpl);

        //                string rowDataTpl = rowData;
        //                TemplateHandler.Assign(ref rowDataTpl, "rowdata", "{" + CastAide.AsInt32(drGameKind["GameGroupID"]).ToString() + "_" + CastAide.AsInt32(drGameType["GameTypeID"]).ToString() + "}");
        //                TemplateHandler.Assign(ref rowDataTpl, "normalCol", "#424242");
        //                _rowData.Append(rowDataTpl);

        //                //count column data
        //                colCount++;
        //            }
        //            string gameKindTpl = gameKind;
        //            TemplateHandler.Assign(ref gameKindTpl, "GameKindColSpan", "colspan='" + tblGameType.Rows.Count.ToString() + "'");
        //            TemplateHandler.Assign(ref gameKindTpl, "lblGameKind", drGameKind[DAL.APIClient.APICasinoProvider.Lang("GameGroupName")]);
        //            _gameKind.Append(gameKindTpl);
        //        }
        //    }
        //    #endregion

        //    TemplateHandler.ReplaceBlock(ref main, "GameKind", _gameKind.ToString());
        //    TemplateHandler.ReplaceBlock(ref main, "GameType", _gameType.ToString());
        //    TemplateHandler.ReplaceBlock(ref main, "rowdata", _rowData.ToString());
        //}
        //else
        //{
            //remove header
            TemplateHandler.RemoveBlock(ref main, "header");
        //}
        #endregion
        #endregion

        #region Load Data Row
        //IReportService reportService = BingoContainer.Current.Resolve<IReportService>();
        //if (AppSettings.IsAgentSite)
        //{
        //    IsEditMember = true;
        //    dataTable = reportService.GetPositionTakingDataTable(null, null,intCustID, Sunplus.Agent.Model.Entities.UserSession.Current.RoleId-1 , ref resultCustomer);
        //    wildcard = new string[] { "a" };
        //}
        //else
        //{
        //    if (AppSettings.IsMasterSite)
        //    {
        //        if (type == "")
        //        {
        //            IsEditMember = false;
        //            wildcard = new string[] { "m", "a" };
        //            dataTable = reportService.GetPositionTakingDataTable(null,intCustID, null,Sunplus.Agent.Model.Entities.UserSession.Current.RoleId-1, ref resultCustomer);
        //            viewType = "agent";
        //        }
        //        else if (type == "agent")
        //        {
        //            IsEditMember = true;
        //            wildcard = new string[] { "m", "a" };
        //            dataTable = reportService.GetPositionTakingDataTable(null, null, intCustID, Sunplus.Agent.Model.Entities.UserSession.Current.RoleId-2, ref resultCustomer);
        //        }
        //    }
        //    else if (AppSettings.IsSuperSite)
        //    {
        //        if (type == "")
        //        {
        //            IsEditMember = false;
        //            wildcard = new string[] { "s" };
        //            dataTable = reportService.GetPositionTakingDataTable(intCustID, null, null, Sunplus.Agent.Model.Entities.UserSession.Current.RoleId-1, ref resultCustomer);
        //            viewType = "master";
        //        }
        //        else if (type == "master")
        //        {
        //            IsEditMember = false;
        //            wildcard = new string[] { "m", "a" };
        //            dataTable = reportService.GetPositionTakingDataTable(null, intCustID, null, Sunplus.Agent.Model.Entities.UserSession.Current.RoleId-2, ref resultCustomer);
        //            viewType = "agent";
        //        }
        //        else if (type == "agent")
        //        {
        //            IsEditMember = true;
        //            wildcard = new string[] { "s", "m", "a" };
        //            dataTable = reportService.GetPositionTakingDataTable(null, null, intCustID, Sunplus.Agent.Model.Entities.UserSession.Current.RoleId-3, ref resultCustomer);
        //        }
        //    }
        //}

        //should remove Role when wildcard.length = 1
        if (wildcard.Length == 1)
        {
            TemplateHandler.RemoveBlock(ref main, "AdditionalRow");
            TemplateHandler.RemoveBlock(ref main, "Role");
            TemplateHandler.Assign(ref main, "colNum", "2");
            TemplateHandler.Assign(ref main, "colNum2", "1");
        }
        else
        {
            TemplateHandler.Assign(ref main, "colNum", "3");
            TemplateHandler.Assign(ref main, "colNum2", "2");
        }

        //copy block row
        string row = TemplateHandler.CopyBlock(main, "row");
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            StringBuilder _s = new StringBuilder();
            string _row = string.Empty;
            string _row2 = string.Empty;
            string _row3 = string.Empty;

            //provide link to edit member if in agent level
            if (IsEditMember)
            {
                TemplateHandler.RemoveBlock(ref row, "link");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref row, "editmember");
            }


            #region Apply PT from API
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (_custid != CastAide.AsString(dataTable.Rows[i]["custid"]))
                {
                    if (re.IsMatch(_row))
                    {
                        matches = re.Matches(_row);
                        foreach (Match match in matches)
                        {
                            TemplateHandler.Assign(ref _row, match.Value, "-");
                        }

                    }

                    _s.Append(_row);
                    if (wildcard.Length > 1)
                    {
                        if (re.IsMatch(_row2))
                        {
                            matches = re.Matches(_row2);
                            foreach (Match match in matches)
                            {
                                TemplateHandler.Assign(ref _row2, match.Value, "-");
                            }

                        }
                        _s.Append(_row2);
                        if (wildcard.Length == 3)
                        {
                            if (re.IsMatch(_row3))
                            {
                                matches = re.Matches(_row3);
                                foreach (Match match in matches)
                                {
                                    TemplateHandler.Assign(ref _row3, match.Value, "-");
                                }

                            }

                            _s.Append(_row3);
                        }
                    }


                    _username = CastAide.AsString(dataTable.Rows[i]["UserName"]);
                    _custid = CastAide.AsString(dataTable.Rows[i]["custid"]);

                    _row = row;
                    if (Convert.ToInt32(dataTable.Rows[i]["closed"]) != 0)
                        TemplateHandler.Assign(ref _row, "color", AppSettings.RowBgClosed);
                    else if (dataTable.Rows[i]["reject"].ToString()!="" && Convert.ToInt32(dataTable.Rows[i]["reject"]) != 0)
                        TemplateHandler.Assign(ref _row, "color", AppSettings.RowBgSuspended);
                    else if (count % 2 != 0) //Apply Odd-Even Row
                        TemplateHandler.Assign(ref _row, "color", AppSettings.RowBgEven);
                    else
                        TemplateHandler.Assign(ref _row, "color", AppSettings.RowBgOdd);

                    //Edit User position taking
                    TemplateHandler.Assign(ref _row, "custid_value", _custid);

                    //apply data for first row
                    TemplateHandler.Assign(ref _row, "No", count);
                    TemplateHandler.Assign(ref _row, "type", viewType);
                    TemplateHandler.Assign(ref _row, "UserName_vl", CastAide.AsString(_username));
                    TemplateHandler.Assign(ref _row, "rowspan", "rowspan='" + wildcard.Length.ToString() + "'");

                    if (wildcard.Length > 1)
                    {
                        _row2 = _row;
                        //Remove NonRepeat td for second row
                        TemplateHandler.RemoveBlock(ref _row2, "NonRepeat");

                        if (wildcard.Length == 3)
                        {
                            _row3 = _row;
                            TemplateHandler.RemoveBlock(ref _row3, "NonRepeat");
                            TemplateHandler.Assign(ref _row3, "Role", wildcard[2].ToUpperInvariant());
                        }

                        //Apply Role
                        TemplateHandler.Assign(ref _row, "Role", wildcard[0].ToUpperInvariant());
                        TemplateHandler.Assign(ref _row2, "Role", wildcard[1].ToUpperInvariant());

                    }
                    count++;
                }


                //Apply position taking data from API
                //First Row
                TemplateHandler.Assign(ref _row, CastAide.AsString(dataTable.Rows[i]["GameGroupID"]) + "_" + CastAide.AsString(dataTable.Rows[i]["GameTypeID"]), (wildcard[0].ToUpperInvariant() == "S") ? NumberFormat.DecFormat(dataTable.Rows[i]["SPositionTaking"], 2) : ((wildcard[0].ToUpperInvariant() == "M") ? NumberFormat.DecFormat(dataTable.Rows[i]["MPositionTaking"], 2) : NumberFormat.DecFormat(dataTable.Rows[i]["APositionTaking"], 2)));

                //second Row
                if (wildcard.Length > 1)
                    TemplateHandler.Assign(ref _row2, CastAide.AsString(dataTable.Rows[i]["GameGroupID"]) + "_" + CastAide.AsString(dataTable.Rows[i]["GameTypeID"]), (wildcard[1].ToUpperInvariant() == "S") ? NumberFormat.DecFormat(dataTable.Rows[i]["SPositionTaking"], 2) : ((wildcard[1].ToUpperInvariant() == "M") ? NumberFormat.DecFormat(dataTable.Rows[i]["MPositionTaking"], 2) : NumberFormat.DecFormat(dataTable.Rows[i]["APositionTaking"], 2)));

                //third Row
                if (wildcard.Length == 3)
                    TemplateHandler.Assign(ref _row3, CastAide.AsString(dataTable.Rows[i]["GameGroupID"]) + "_" + CastAide.AsString(dataTable.Rows[i]["GameTypeID"]), (wildcard[2].ToUpperInvariant() == "S") ? NumberFormat.DecFormat(dataTable.Rows[i]["SPositionTaking"], 2) : ((wildcard[2].ToUpperInvariant() == "M") ? NumberFormat.DecFormat(dataTable.Rows[i]["MPositionTaking"], 2) : NumberFormat.DecFormat(dataTable.Rows[i]["APositionTaking"], 2)));

                //for end of row --> append data to string builder
                if (i == dataTable.Rows.Count - 1)
                {
                    if (re.IsMatch(_row))
                    {
                        matches = re.Matches(_row);
                        foreach (Match match in matches)
                        {
                            TemplateHandler.Assign(ref _row, match.Value, "-");
                        }

                    }
                    _s.Append(_row);
                    if (wildcard.Length > 1)
                    {
                        if (re.IsMatch(_row2))
                        {
                            matches = re.Matches(_row2);
                            foreach (Match match in matches)
                            {
                                TemplateHandler.Assign(ref _row2, match.Value, "-");
                            }

                        }
                        _s.Append(_row2);
                        if (wildcard.Length == 3)
                        {
                            if (re.IsMatch(_row3))
                            {
                                matches = re.Matches(_row3);
                                foreach (Match match in matches)
                                {
                                    TemplateHandler.Assign(ref _row3, match.Value, "-");
                                }

                            }

                            _s.Append(_row3);
                        }
                    }
                }
            }

            content = _s.ToString();
            #endregion

        }
        else
        {
            if (wildcard.Length > 1)
                colCount = colCount + 5;
            else
                colCount = colCount + 4;
            TemplateHandler.Assign(ref notfound, "noinfo", Resources.Lang.noInfo);
            TemplateHandler.Assign(ref notfound, "num-cols", colCount);
            content = notfound;
        }
        #endregion

        TemplateHandler.ReplaceBlock(ref main, "row", content);
        return main;
    }
    #endregion

    #region Generate Downline list view by Super or Master
    public static string DirectDownlineList(int custid, string masterid, int level, string username, int pageindex, int status, string act_list, string search, int totalpages)
    {
        string main = TemplateHandler.LoadTemplate("MemberInfo\\MasterList.html");
        int cols = 8;

        //Render casino
        if (!UserPermission.IsShowCasino)
        {
            // Edit/Sync Casino button
            TemplateHandler.RemoveBlock(ref main, "no-casino");
            TemplateHandler.RemoveBlock(ref main, "casino");
            TemplateHandler.RemoveBlock(ref main, "edit-CS");
        }
        //END

        //Render Bingo
        //if (!UserPermission.IsShowBingo)
        //{
        //    // Edit/Sync Bingo button
        //    TemplateHandler.RemoveBlock(ref main, "bingo");
        //    TemplateHandler.RemoveBlock(ref main, "no-bingo");
        //    TemplateHandler.RemoveBlock(ref main, "edit-BG");
        //}

        
        GenDownlineListTpl(ref main, ref cols, level,7);
        GenDownlineListTitle(ref main, level);
        GenEditMult(ref  main, custid);
        GenerateSearch(ref main, act_list, status, username, search);

        bool isDirectDL = UserSession.Current.RoleId - level == 1;

        TemplateHandler.Assign(ref main, "isdisablep2p", isDirectDL && UserPermission.IsShowP2P ? "0" : "1");

        TemplateHandler.Assign(ref main, "cus", custid);
        string blockContent = TemplateHandler.CopyBlock(main, "Content");
        string rowTpl = "";
        StringBuilder rows = new StringBuilder(50000);              
       
        TemplateHandler.ReplaceBlock(ref main, "Header", GenDownlineListHeader(main, level));
        DataTable dt = new DataTable();
        // Get data from DB
        DbResult dbr = null;
        //int totalpages =0;
        string pageList = string.Empty;
        string GotoPageHtml = string.Empty;
        if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 4)
        {
            dbr = MemberInfo.GetDataSagentList(custid.ToString(), username, ref totalpages, pageindex, status);
        }
        else if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 3)
        {
            dbr = MemberInfo.GetDataMagentList(custid.ToString(), username, ref totalpages, pageindex, status);
        }

        int countrow = ((pageindex - 1) * AppSettings.PageSize);
        dt = dbr.ResultSet.Tables[0];
        if (dt != null)
        {
            if (dt.Rows.Count == 0)
            {
                cols = cols + 15;
                rows.Append(Common.NoInfo(cols));
                TemplateHandler.Assign(ref main, "pagenevigator", string.Empty);
            }
            else
            {
                TemplateHandler.Assign(ref main, "totalpages", totalpages.ToString());
                string conditiontime = MemberInfo.GetDataTransferCondition(Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString());
                if (conditiontime.IndexOf("1") == -1)
                {
                    TemplateHandler.Assign(ref main, "istransfer", "1");
                }
                else
                {
                    TemplateHandler.Assign(ref main, "istransfer", "0");
                }

                foreach (DataRow row in dt.Rows)
                {
                    countrow++;

                    rowTpl = blockContent;
                    TemplateHandler.Assign(ref rowTpl, "no_value", countrow.ToString());
                    TemplateHandler.Assign(ref rowTpl, "username_value", CastAide.AsString(row["username"]));
                    TemplateHandler.Assign(ref rowTpl, "firstname_value", CastAide.AsString(row["firstname"]));
                    TemplateHandler.Assign(ref rowTpl, "lastname_value", CastAide.AsString(row["lastname"]));
                    TemplateHandler.Assign(ref rowTpl, "groupA_value", FormatHandler.FormatNumber4(row["groupa"], FormatHandler.CULT_NAME_ENG));
                    TemplateHandler.Assign(ref rowTpl, "groupB_value", FormatHandler.FormatNumber4(row["groupb"], FormatHandler.CULT_NAME_ENG));
                    TemplateHandler.Assign(ref rowTpl, "groupC_value", FormatHandler.FormatNumber4(row["groupc"], FormatHandler.CULT_NAME_ENG));
                    TemplateHandler.Assign(ref rowTpl, "groupD_value", FormatHandler.FormatNumber4(row["groupd"], FormatHandler.CULT_NAME_ENG));
                    TemplateHandler.Assign(ref rowTpl, "1x2_value", FormatHandler.FormatNumber4(row["discount1x2"], FormatHandler.CULT_NAME_ENG));
                    TemplateHandler.Assign(ref rowTpl, "other_value", FormatHandler.FormatNumber4(row["discountcs"], FormatHandler.CULT_NAME_ENG));
                    TemplateHandler.Assign(ref rowTpl, "createdate_value", CastAide.AsString(row["createdate"]));
                    TemplateHandler.Assign(ref rowTpl, "lastloginip_value", row["lastloginip"].ToString());
                    TemplateHandler.Assign(ref rowTpl, "edit", Resources.Lang.edit);
                    TemplateHandler.Assign(ref rowTpl, "password", Resources.Lang.password);
                    TemplateHandler.Assign(ref rowTpl, "custid_value", row["custid"].ToString());
                    TemplateHandler.Assign(ref rowTpl, "chkid_value", row["custid"].ToString() + ";" + row["username"].ToString());
                    TemplateHandler.Assign(ref rowTpl, "roleid_value", (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId - 1));
                    if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 4)
                    {
                        TemplateHandler.Assign(ref rowTpl, "isAllowAutoPT", CastAide.AsBoolean(row["DisableAutoMPT"]) ? "false" : "true");
                    }
                    else
                        TemplateHandler.Assign(ref rowTpl, "isAllowAutoPT", "false");

                    TemplateHandler.Assign(ref rowTpl, "isAllowOutright", CastAide.AsBoolean(row["DisableOutright"]) ? "false" : "true");

                    TemplateHandler.Assign(ref rowTpl, "overcolor", AppSettings.RowBgOver);

                    TemplateHandler.Assign(ref rowTpl, "divHR", GetHRIconStyle(row));
                    TemplateHandler.Assign(ref rowTpl, "divFI", CastAide.AsBoolean(row["disableFI"]) ? "divEditDisabled" : "divEdit");

                    //If enable number game on webconfig
                    if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission)
                        TemplateHandler.RemoveBlock(ref rowTpl, "disable-number-game");
                    else
                        TemplateHandler.Assign(ref rowTpl, "numbergame_value", FormatHandler.FormatNumber4(row["discountnumber"], FormatHandler.CULT_NAME_ENG));    
                        

                    GenStatus(ref rowTpl, row);
                    GenViewStatement(ref rowTpl, row, masterid);
                    GenCasino(ref rowTpl, row, level);
                    GenBingo(ref rowTpl, row, level);
                    GenP2P(ref rowTpl, row, level);
                    rows.Append(rowTpl);
                }                
                
                #region Generate Paging Section if yes
                StringBuilder url = new StringBuilder();
                url.Append("DownlineList.aspx?");
                url.Append("custid=" + custid);
                url.Append("&search=" + search);
                url.Append("&username=" + username);
                url.Append("&status=" + status);
                url.Append("&total=" + totalpages);

                int cols1 = 18;
                
                if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsCasinoDisabled)
                    cols1++;

                if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsDisabledFI)
                    cols1++;

                if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsDisabledHR)
                    cols1++;

                pageList = Common.GenPagingFooter(url.ToString(), totalpages, pageindex, cols1, act_list);
                GotoPageHtml = TemplateHandler.LoadTemplate("GotoPage.html");

                TemplateHandler.Assign(ref GotoPageHtml, "textgotopage", Resources.Lang.gotopage);
                TemplateHandler.Assign(ref GotoPageHtml, "textgo", Resources.Lang.go);
                TemplateHandler.Assign(ref pageList, "pagetext", Resources.Lang.tpage);

                TemplateHandler.Assign(ref GotoPageHtml, "Url", url.ToString());
                TemplateHandler.Assign(ref GotoPageHtml, "Site", act_list);
                if (pageList.Length > 0)
                {
                    TemplateHandler.Assign(ref main, "pagenevigator", pageList);
                    main += GotoPageHtml;
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref main, "limit-footer");
                }
                #endregion

            }
        }
        TemplateHandler.ReplaceBlock(ref main, "Content", rows.ToString());

        if (!Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing)
        {
            TemplateHandler.RemoveBlock(ref main, "edit-HR"); 
        }

        return main;
    }
    #endregion

    #region Generate Customer List
    public static string MemberList(int custid, string masterid, int level,string username,int pageindex,int status,string act_list,string search,int totalpages)
    {
        string main = TemplateHandler.LoadTemplate("MemberInfo\\MemberList.html");
        int cols = 9;

        //Render casino
        if (!UserPermission.IsShowCasino)
        {
            // Edit/Sync Casino button
            TemplateHandler.RemoveBlock(ref main, "no-casino");
            TemplateHandler.RemoveBlock(ref main, "casino");
            TemplateHandler.RemoveBlock(ref main, "edit-CS");
        }
        //END

        //Render p2p
        if (UserPermission.IsShowP2P)
        {
            TemplateHandler.Assign(ref main, "lblP2P", Resources.Lang.P2P);
            cols = 10;
        }
        else
        {
            TemplateHandler.RemoveBlock(ref main, "no-p2p");
            TemplateHandler.RemoveBlock(ref main, "edit-P2P");
            cols = 10;
        }
        
        //Render Bingo
        //if (!UserPermission.IsShowBingo)
        //{

        //    // Edit/Sync Bingo button
        //    TemplateHandler.RemoveBlock(ref main, "bingo");
        //    TemplateHandler.RemoveBlock(ref main, "no-bingo");
        //    TemplateHandler.RemoveBlock(ref main, "edit-BG");
            
        //}

        int roleid = Sunplus.Agent.Model.Entities.UserSession.Current.RoleId;
        bool isDirectDL = roleid - level == 1;

        TemplateHandler.Assign(ref main, "isdisablep2p", isDirectDL && UserPermission.IsShowP2P ? "0" : "1");

        GenDownlineListTpl(ref main, ref cols, level,6);
        GenDownlineListTitle(ref main, level);
        GenCustomerListTitle(ref main, Sunplus.Agent.Model.Entities.UserSession.Current.CustId);
        GenEditMult(ref main, custid);
        GenerateSearch(ref main, act_list, status, username, search);
        TemplateHandler.Assign(ref main, "masterid", masterid);
        TemplateHandler.Assign(ref main, "cus", custid);
        string blockContent = TemplateHandler.CopyBlock(main, "Content");
        string rowTpl = "";
        StringBuilder rows = new StringBuilder(50000);

        TemplateHandler.ReplaceBlock(ref main, "Header", GenDownlineListHeader(main, level));
        DataTable dt = new DataTable();
        // Get data from DB    
       // int totalpages = 0;
        string pageList = string.Empty;
        string GotoPageHtml = string.Empty;
        DbResult dbr = MemberInfo.GetDataCustomerList(custid.ToString(), username, ref totalpages, pageindex, status);
        int countrow = ((pageindex - 1) * AppSettings.PageSize);
        TemplateHandler.Assign(ref main, "totalpages", totalpages.ToString());
        dt = dbr.ResultSet.Tables[0];
        if (dt != null)
        {
            if (dt.Rows.Count == 0)
            {
                cols = cols + 15;
                rows.Append(Common.NoInfo(cols));
            }
            else
            {
                foreach (DataRow row in dt.Rows)
                {
                    countrow++;

                    rowTpl = blockContent;
                    if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId != 2) TemplateHandler.RemoveBlock(ref rowTpl, "reset-pass");
                    TemplateHandler.Assign(ref rowTpl, "no_value", countrow.ToString());
                    TemplateHandler.Assign(ref rowTpl, "username_value", CastAide.AsString(row["username"]));
                    string nickname = CastAide.AsString(CastAide.AsString(row["username2"]), string.Empty);
                    TemplateHandler.Assign(ref rowTpl, "nickname-value", nickname);
                    TemplateHandler.Assign(ref rowTpl, "firstname_value", CastAide.AsString(row["firstname"]));
                    TemplateHandler.Assign(ref rowTpl, "lastname_value", CastAide.AsString(row["lastname"]));
                    TemplateHandler.Assign(ref rowTpl, "group_value", row["ugroup"]);
                    TemplateHandler.Assign(ref rowTpl, "comma1_value", FormatHandler.FormatNumber4(row["discount"], FormatHandler.CULT_NAME_ENG));
                    TemplateHandler.Assign(ref rowTpl, "comma2_value", FormatHandler.FormatNumber4(row["discountcs"], FormatHandler.CULT_NAME_ENG));
                    TemplateHandler.Assign(ref rowTpl, "comma1x2_value", FormatHandler.FormatNumber4(row["discount1x2"], FormatHandler.CULT_NAME_ENG));
                    TemplateHandler.Assign(ref rowTpl, "commp1_value", FormatHandler.FormatNumber4(row["playerdiscount"], FormatHandler.CULT_NAME_ENG));
                    TemplateHandler.Assign(ref rowTpl, "commp2_value", FormatHandler.FormatNumber4(row["playerdiscountcs"], FormatHandler.CULT_NAME_ENG));
                    TemplateHandler.Assign(ref rowTpl, "commp1x2_value", FormatHandler.FormatNumber4(row["playerdiscount1x2"], FormatHandler.CULT_NAME_ENG));
                    TemplateHandler.Assign(ref rowTpl, "createdate_value", CastAide.AsString(row["createdate"]));
                    TemplateHandler.Assign(ref rowTpl, "lastloginip_value", Sunplus.Agent.Model.Common.AgentIPFormat.FormatIP(row["lastloginip"].ToString()));
                    TemplateHandler.Assign(ref rowTpl, "edit", Resources.Lang.edit);
                    TemplateHandler.Assign(ref rowTpl, "password", Resources.Lang.password);
                    TemplateHandler.Assign(ref rowTpl, "custid_value", row["custid"].ToString());
                    TemplateHandler.Assign(ref rowTpl, "chkid_value", row["custid"].ToString() + ";" + row["username"].ToString());
                    TemplateHandler.Assign(ref rowTpl, "roleid_value", "1");

                    TemplateHandler.Assign(ref rowTpl, "overcolor", AppSettings.RowBgOver);

                    TemplateHandler.Assign(ref rowTpl, "divHR", GetHRIconStyle(row));

                    TemplateHandler.Assign(ref rowTpl, "divFI", CastAide.AsBoolean(row["disableFI"]) ? "divEditDisabled" : "divEdit");
                    
                    //If enable number game on webconfig
                    if (UserPermission.IsShowNumberGame)
                        TemplateHandler.Assign(ref rowTpl, "numbergame_value", FormatHandler.FormatNumber4(row["discountnumber"], FormatHandler.CULT_NAME_ENG));
                    else
                        TemplateHandler.RemoveBlock(ref rowTpl, "disable-number-game");

                    GenStatus(ref rowTpl, row);
                    GenViewMemberStatement(ref rowTpl, row);
                    GenCasino(ref rowTpl, row, level);
                    GenBingo(ref rowTpl, row, level);
                    GenP2P(ref rowTpl, row, level);
                    rows.Append(rowTpl);
                }
                #region Generate Paging Section if yes
                StringBuilder url = new StringBuilder();
                url.Append("DownlineList.aspx?");
                url.Append("custid=" + custid);
                url.Append("&search=" + search);
                url.Append("&username=" + username);
                url.Append("&status=" + status);
                url.Append("&total=" + totalpages);

                int cols1 = 18;
                if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsCasinoDisabled)
                    cols1++;

                if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsDisabledFI)
                    cols1++;

                if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsDisabledHR)
                    cols1++;

                pageList = Common.GenPagingFooter(url.ToString(), totalpages, pageindex, cols1, act_list);
                GotoPageHtml = TemplateHandler.LoadTemplate("GotoPage.html");

                TemplateHandler.Assign(ref GotoPageHtml, "textgotopage", Resources.Lang.gotopage);
                TemplateHandler.Assign(ref GotoPageHtml, "textgo", Resources.Lang.go);
                TemplateHandler.Assign(ref pageList, "pagetext", Resources.Lang.tpage);

                TemplateHandler.Assign(ref GotoPageHtml, "Url", url.ToString());
                TemplateHandler.Assign(ref GotoPageHtml, "Site", act_list);
                #endregion
            }
            TemplateHandler.ReplaceBlock(ref main, "Content", rows.ToString());
            if (pageList.Length > 0)
            {
                TemplateHandler.Assign(ref main, "pagenevigator", pageList);
                main += GotoPageHtml;
            }
            else
            {
                TemplateHandler.RemoveBlock(ref main, "limit-footer");
            }
        }
        
        //Terry
        if (!Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing)
        {
            TemplateHandler.RemoveBlock(ref main, "edit-HR");
        }
        return main;
    }
    #endregion

    #region Generate Search for Customer List
    private static void GenerateSearch(ref string main, string act_list, int status, string username, string search)
    {
        TemplateHandler.Assign(ref main, "lbl_username", Resources.Lang.username);
        TemplateHandler.Assign(ref main, "search", search);
        TemplateHandler.Assign(ref main, "previous_value", (username == "" || username == Resources.Lang.enterusername) ? Resources.Lang.enterusername : username);
        TemplateHandler.Assign(ref main, "enterusername", Resources.Lang.enterusername);
        TemplateHandler.Assign(ref main, "classStyle", (username == "" || username == Resources.Lang.enterusername) ? "text_f1" : "text_f");
        TemplateHandler.Assign(ref main, "lbl_status", Resources.Lang.status);
        TemplateHandler.Assign(ref main, "label_submit", Resources.Lang.submit);
        

        TemplateHandler.Assign(ref main, "all", Resources.Lang.all);
        TemplateHandler.Assign(ref main, "active", Resources.Lang.open);
        TemplateHandler.Assign(ref main, "closed", Resources.Lang.closed);
        TemplateHandler.Assign(ref main, "suspended", Resources.Lang.suspended);
        TemplateHandler.Assign(ref main, "disabled", Resources.Lang.disabled);

        string strSearchAction = "searchByUsername('DownlineList.aspx','" + act_list + "')";
        TemplateHandler.Assign(ref main, "search_by_username", strSearchAction);

        if (status == 0)
        {
            TemplateHandler.Assign(ref main, "active-select", "");
            TemplateHandler.Assign(ref main, "all-select", "selected='selected'");
            TemplateHandler.Assign(ref main, "closed-select", "");
            TemplateHandler.Assign(ref main, "suspended-select", "");
            TemplateHandler.Assign(ref main, "disabled-select", "");
        }
        else if (status == 1)
        {
            TemplateHandler.Assign(ref main, "active-select", "selected='selected'");
            TemplateHandler.Assign(ref main, "all-select", "");
            TemplateHandler.Assign(ref main, "closed-select", "");
            TemplateHandler.Assign(ref main, "suspended-select", "");
            TemplateHandler.Assign(ref main, "disabled-select", "");
        }
        else if (status == 2)
        {
            TemplateHandler.Assign(ref main, "active-select", "");
            TemplateHandler.Assign(ref main, "all-select", "");
            TemplateHandler.Assign(ref main, "closed-select", "");
            TemplateHandler.Assign(ref main, "suspended-select", "selected='selected'");
            TemplateHandler.Assign(ref main, "disabled-select", "");
        }
        else if (status == 3)
        {
            TemplateHandler.Assign(ref main, "active-select", "");
            TemplateHandler.Assign(ref main, "all-select", "");
            TemplateHandler.Assign(ref main, "closed-select", "selected='selected'");
            TemplateHandler.Assign(ref main, "suspended-select", "");
            TemplateHandler.Assign(ref main, "disabled-select", "");
        }
        else
        {
            TemplateHandler.Assign(ref main, "active-select", "");
            TemplateHandler.Assign(ref main, "all-select", "");
            TemplateHandler.Assign(ref main, "closed-select", "");
            TemplateHandler.Assign(ref main, "suspended-select", "");
            TemplateHandler.Assign(ref main, "disabled-select", "selected='selected'");
        }
    }
    #endregion

    #region Generate header of table
    private static string GenDownlineListHeader(string tpl, int level)
    {
        string header = TemplateHandler.CopyBlock(tpl, "Header");
        TemplateHandler.Assign(ref header, "commission", Resources.Lang.commission);
        TemplateHandler.Assign(ref header, "no", Resources.Lang.no);
        TemplateHandler.Assign(ref header, "username", Resources.Lang.username);
        TemplateHandler.Assign(ref header, "status", Resources.Lang.status);
        TemplateHandler.Assign(ref header, "minposition", Resources.Lang.minposition);
        TemplateHandler.Assign(ref header, "firstname", Resources.Lang.firstname);
        TemplateHandler.Assign(ref header, "lastname", Resources.Lang.lastname);
        TemplateHandler.Assign(ref header, "member", Resources.Lang.member);
        TemplateHandler.Assign(ref header, "group", Resources.Lang.group);
        TemplateHandler.Assign(ref header, "1x2group", Resources.Lang.lbl1x2);
        TemplateHandler.Assign(ref header, "other", Resources.Lang.other);
        TemplateHandler.Assign(ref header, "lastloginip", Resources.Lang.lastloginIP);
        TemplateHandler.Assign(ref header, "transfer", Resources.Lang.transfer);
        TemplateHandler.Assign(ref header, "password", Resources.Lang.password);
        TemplateHandler.Assign(ref header, "createdate", Resources.Lang.CreatedDate);
        if (level == 1)
        {
            TemplateHandler.Assign(ref header, "nickname", Resources.Lang.nickname);
            TemplateHandler.Assign(ref header, "comma1", Resources.Lang.acomm1);
            TemplateHandler.Assign(ref header, "comma1x2", Resources.Lang.acomm2);
            TemplateHandler.Assign(ref header, "comma2", Resources.Lang.acomm3);
            TemplateHandler.Assign(ref header, "commp1", Resources.Lang.pcomm1);
            TemplateHandler.Assign(ref header, "commp1x2", Resources.Lang.pcomm2);
            TemplateHandler.Assign(ref header, "commp2", Resources.Lang.pcomm3);            

            //tooltips
            TemplateHandler.Assign(ref header, "tipcomm1", Resources.Lang.tipcomm1);
            TemplateHandler.Assign(ref header, "tipcomm2", Resources.Lang.tipcomm2);
            TemplateHandler.Assign(ref header, "tipcomm3", Resources.Lang.tipcomm3);
        }
        return header;
    }

    private static void GenHeaderNumberGame(ref string main, int colspan)
    {
        //If set enable number game on webconfgi
        if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission)
        {
            TemplateHandler.RemoveBlock(ref main, "disable-number-game");
            TemplateHandler.Assign(ref main, "colbeforeCreate", colspan-1);
        }
        else
        {
            TemplateHandler.Assign(ref main, "colbeforeCreate", colspan);
            TemplateHandler.Assign(ref main, "lblGame", Resources.Lang.game);
        }
    }
    #endregion

    #region Generate Template by HR, FI, Casino, Bingo
    private static void GenDownlineListTpl(ref string template, ref int colspan, int level, int colbeforeCreate)
    {
        int colbeforeComm = colspan;
        if (!UserPermission.IsShowRacing)
        {
            colbeforeComm--;
        }
        else
        {
            TemplateHandler.Assign(ref template, "horseracing", Resources.Lang.hr);
        }
        if (!UserPermission.IsShowCasino)
        {
            // Edit/Sync Casino button
            TemplateHandler.Assign(ref template, "divOp", "divEdit");
            TemplateHandler.RemoveBlock(ref template, "edit-CS");
            colbeforeComm--;
        }
        else
        {
            TemplateHandler.Assign(ref template, "lblCasino", Resources.Lang.casino);
        }

        //Render p2p
        if (UserPermission.IsShowP2P)
        {
            if (level != 1)
                colbeforeComm++;
            else
            {
                colbeforeComm--;
                colbeforeCreate++;
            }
            TemplateHandler.Assign(ref template, "lblP2P", Resources.Lang.P2P);
        }
        else
        {
            if (level != 1)
            {
                //colbeforeComm++;
                //colbeforeCreate--;
            }
            else
            {
                colbeforeComm = colbeforeComm - 2;
                colbeforeCreate++;
            }
            TemplateHandler.RemoveBlock(ref template, "no-p2p");
            TemplateHandler.RemoveBlock(ref template, "edit-P2P");
        }
                
        //if (!UserPermission.IsShowBingo)
        //{
        //    // Edit/Sync Bingo button
        //    TemplateHandler.RemoveBlock(ref template, "edit-BG");
        //    TemplateHandler.RemoveBlock(ref template, "no-bingo");
        //    TemplateHandler.RemoveBlock(ref template, "bingo");
        //    colbeforeComm--;
        //}
        //else
        //{
            TemplateHandler.Assign(ref template, "lblBingo", Resources.Bingo.bingo);
            // Disable Bingo
            //colbeforeComm--;
        //}
        
        if (!UserPermission.IsShowFinance)
        {
            TemplateHandler.RemoveBlock(ref template, "edit-FI");
            colbeforeComm--;
        }
        else
        {
            TemplateHandler.Assign(ref template, "finance", Resources.Lang.Financial);
        }

        //If set enable number game on webconfgi
        if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission)
        {
            TemplateHandler.RemoveBlock(ref template, "disable-number-game");
            TemplateHandler.Assign(ref template, "colbeforeCreate", colbeforeCreate - 1);
        }
        else
        {
            TemplateHandler.Assign(ref template, "colbeforeCreate", colbeforeCreate);
            TemplateHandler.Assign(ref template, "lblGame", Resources.Lang.game);
        }

        if (level == 1 && Sunplus.Agent.Model.Entities.UserSession.Current.RoleId != 2)
        {
           TemplateHandler.RemoveBlock(ref template, "reset-pass");
           //colbeforeComm--;
        }
        if (level == 1 && Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 2)
            colbeforeComm++;
        TemplateHandler.Assign(ref template, "colbeforeComm", colbeforeComm.ToString());
        colspan = colbeforeComm;
    }
    #endregion

    #region Gennerate Status Column
    private static void GenStatus(ref string rowTpl, DataRow dr)
    {
        string odd_even_row = "";
        string status = "";
        odd_even_row = (odd_even_row == "even_row") ? "odd_row" : "even_row";

        bool isClosed = CastAide.AsBoolean(dr["closed"]);
        bool isSuspended = CastAide.AsBoolean(dr["suspended"]);
        bool isDisableOutright = CastAide.AsBoolean(dr["DisableOutright"]);
        bool isEnabled = CastAide.AsBoolean(dr["enabled"]);

        bool isDisableNumber = dr.Table.Columns.Contains("disablenumber") ? CastAide.AsBoolean(dr["disablenumber"]) : true;
        
        

        TemplateHandler.Assign(ref rowTpl, "isClosed", isClosed.ToString().ToLower());
        TemplateHandler.Assign(ref rowTpl, "isSuspended", isSuspended.ToString().ToLower());
        TemplateHandler.Assign(ref rowTpl, "isOutright", (!isDisableOutright).ToString().ToLower());
        TemplateHandler.Assign(ref rowTpl, "isAllowNumber", (!isDisableNumber).ToString().ToLower());
        
        if (isEnabled)
        {
            if (isClosed)
            {
                status = Resources.Lang.close;
                TemplateHandler.Assign(ref rowTpl, "color", AppSettings.RowBgClosed);
            }
            else if (isSuspended)
            {
                status = Resources.Lang.suspend;
                TemplateHandler.Assign(ref rowTpl, "color", AppSettings.RowBgSuspended);
            }            
            else
            {
                status = Resources.Lang.open;
                TemplateHandler.Assign(ref rowTpl, "color", AppSettings.RowBgOdd);
            }
        }
        else
        {
            status = Resources.Lang.disabled;
            TemplateHandler.Assign(ref rowTpl, "color", AppSettings.RowBgDisabled);
        }
        TemplateHandler.Assign(ref rowTpl, "status_value", status);

        if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 4)
        {
            TemplateHandler.Assign(ref rowTpl, "function_name_single", "EditMaster_Single");
            TemplateHandler.Assign(ref rowTpl, "custid_upline", Sunplus.Agent.Model.Entities.UserSession.Current.CustId);

            
        }
        else if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 3)
        {
            TemplateHandler.Assign(ref rowTpl, "function_name_single", "EditAgent_Single");
        }
        else if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 2)
        {
            TemplateHandler.Assign(ref rowTpl, "function_name_single", "EditMember_Single");
        }

    }
    #endregion

    #region Gennerate Edit Multiple Tpl
    private static void GenEditMult(ref string rowTpl, int custid)
    {
        if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 4)
        {
            TemplateHandler.Assign(ref rowTpl, "function_name_multi", "EditMaster_Multi");
            TemplateHandler.Assign(ref rowTpl, "custid_upline", custid);
            TemplateHandler.Assign(ref rowTpl, "Customer", Common.GetLevel(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId));
        }
        else if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 3)
        {
            TemplateHandler.Assign(ref rowTpl, "function_name_multi", "EditAgent_Multi");
            TemplateHandler.Assign(ref rowTpl, "custid_upline", custid);
            TemplateHandler.Assign(ref rowTpl, "Customer", Common.GetLevel(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId));
        }
        else if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 2)
        {
            TemplateHandler.Assign(ref rowTpl, "function_name_multi", "EditMember_Multi");
            TemplateHandler.Assign(ref rowTpl, "custid_upline", custid);
            TemplateHandler.Assign(ref rowTpl, "Customer", Common.GetLevel(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId));
        }
    }
    #endregion

    #region Generate View Statement and view Downline
    private static void GenViewStatement(ref string rowTpl, DataRow dr, string masterid)
    {
        string url_link = "";
        string url_downline = "";
        if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 4)
        {
            url_link = "javascript:viewStatement(" + dr["custid"] + ", 'Master');";
            url_downline = "javascript:viewDownlineList('AgentList'," + dr["custid"] + "," + dr["custid"] + ")";
        }
        else
        {
            url_link = "javascript:viewStatement(" + dr["custid"] + ", 'Agent');";
            url_downline = "javascript:viewDownlineList('MemberList'," + dr["custid"] + "," + masterid + ")";
        }
        if (HasReport)
        {
            TemplateHandler.Assign(ref rowTpl, "url_link", url_link);
            TemplateHandler.RemoveBlock(ref rowTpl, "no-link");
        }
        else
        {
            TemplateHandler.RemoveBlock(ref rowTpl, "limit-master");
        }
        TemplateHandler.Assign(ref rowTpl, "url_view", url_downline);
        TemplateHandler.Assign(ref rowTpl, "view", Resources.Lang.view);
    }

    private static void GenViewMemberStatement(ref string rowTpl, DataRow dr)
    {
        if (HasReport)
        {
            string url_link = "";
            url_link = "javascript:viewCustOutSt('" + dr["username"] + "', " + dr["custid"] + ");";
            TemplateHandler.RemoveBlock(ref rowTpl, "no-link");
            TemplateHandler.Assign(ref rowTpl, "url_link", url_link);
        }
        else
        {
            TemplateHandler.RemoveBlock(ref rowTpl, "view-statement");
        }
    }
    private static bool HasReport
    {
        get
        {
            return (Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.ViewReports);
        }
    }
    private static void GenStatementLink(ref string block_content, string url)
    {
        if (HasReport)
        {
            TemplateHandler.Assign(ref block_content, "url_link", url);
            TemplateHandler.RemoveBlock(ref block_content, "no-link");
        }
        else
        {
            TemplateHandler.RemoveBlock(ref block_content, "limit-master");
        }
    }
    #endregion

    #region Generate Horse Racing Icon Tyle 
    private static String GetHRIconStyle(DataRow dr)
    {
        string strTyle = "divEdit";

        bool disable151_41 = CastAide.AsBoolean(dr["disable151_41"]) || !UserPermission.IsShowRacing_151_41;
        bool disable151_31 = CastAide.AsBoolean(dr["disable151_31"]) || !UserPermission.IsShowRacing_151_31;
        bool disable151_51 = CastAide.AsBoolean(dr["disable151_51"]) || !UserPermission.IsShowRacing_151_51;
        bool disable152_41 = CastAide.AsBoolean(dr["disable152_41"]) || !UserPermission.IsShowRacing_152_41;
        bool disable152_51 = CastAide.AsBoolean(dr["disable151_51"]) || !UserPermission.IsShowRacing_152_51;
        bool disable153_51 = CastAide.AsBoolean(dr["disable153_51"]) || !UserPermission.IsShowRacing_153_51;

        bool disable151 = CastAide.AsBoolean(dr["Disable151"]);

        disable151 = disable151_41 
            & disable151_31
            & disable151_51
            & disable152_41
            & disable152_51
            & disable153_51
            ;

        if (disable151)
        {
            strTyle = "divEditDisabled";
        }

        return strTyle;
    }
    #endregion

    #region Generate Bingo Column

    private static void GenBingoForMemberList(ref string _copycontent, object drBingo, int roleid)
    {
        if (!CastAide.AsBoolean(drBingo))
        {
            if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId - roleid == 1)
            {
                TemplateHandler.RemoveBlock(ref _copycontent, "no-bingo");
                TemplateHandler.Assign(ref _copycontent, "divOpBG", "divSync");
                TemplateHandler.Assign(ref _copycontent, "editBG", Resources.Lang.sync);
            }
            else
            {
                TemplateHandler.RemoveBlock(ref _copycontent, "edit-BG");
                TemplateHandler.Assign(ref _copycontent, "divOpBG", "divDisable");
                TemplateHandler.Assign(ref _copycontent, "editBG", "");
            }

        }
        else
        {
            if ((Sunplus.Agent.Model.Entities.UserSession.Current.RoleId - roleid == 1) || (roleid == 1))
            {
                if (CastAide.AsBoolean(drBingo))
                {
                    TemplateHandler.RemoveBlock(ref _copycontent, "edit-BG");
                    TemplateHandler.Assign(ref _copycontent, "divOpBG", "divEditDisabled");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _copycontent, "no-bingo");
                    TemplateHandler.Assign(ref _copycontent, "divOpBG", "divEdit");
                }

                TemplateHandler.Assign(ref _copycontent, "editBG", Resources.Lang.edit);
            }
            else
            {
                TemplateHandler.RemoveBlock(ref _copycontent, "no-bingo");
                TemplateHandler.Assign(ref _copycontent, "divOpBG", "divViewCS");
                TemplateHandler.Assign(ref _copycontent, "editBG", Resources.Lang.view);
            }
        }
    }

    private static void GenBingoForAgentList(ref string temp_content, object synBingo, int roleid )
    {
        if (!CastAide.AsBoolean(synBingo))
        {
            if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId - roleid == 1)
            {
                TemplateHandler.RemoveBlock(ref temp_content, "no-bingo");
                TemplateHandler.Assign(ref temp_content, "divOpBG", "divSync");
                TemplateHandler.Assign(ref temp_content, "editBG", Resources.Lang.sync);
                TemplateHandler.Assign(ref temp_content, "statusBG", "0"); // Not syn bingo
            }
            else
            {
                TemplateHandler.RemoveBlock(ref temp_content, "bingo");
                TemplateHandler.Assign(ref temp_content, "divOpBG", "divDisable");
                TemplateHandler.Assign(ref temp_content, "editBG", "");
                TemplateHandler.Assign(ref temp_content, "statusBG", "-1"); // Can not take any action
            }
        }
        else
        {
            if ((Sunplus.Agent.Model.Entities.UserSession.Current.RoleId - roleid == 1) || (roleid == 1))
            {
                if (CastAide.AsBoolean(synBingo))
                {
                    TemplateHandler.RemoveBlock(ref temp_content, "bingo");
                    TemplateHandler.Assign(ref temp_content, "divOpBG", "divEditDisabled");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref temp_content, "no-bingo");
                    TemplateHandler.Assign(ref temp_content, "divOpBG", "divEdit");
                }

                TemplateHandler.Assign(ref temp_content, "editBG", Resources.Lang.edit);
            }
            else
            {
                TemplateHandler.RemoveBlock(ref temp_content, "no-bingo");
                TemplateHandler.Assign(ref temp_content, "divOpBG", "divViewCS");
                TemplateHandler.Assign(ref temp_content, "editBG", Resources.Lang.view);
            }

            TemplateHandler.Assign(ref temp_content, "statusBG", "1");
        }
    }

    private static void GenBingo(ref string rowTpl, DataRow dr, int roleidDL)
    {   
        int roleid = Sunplus.Agent.Model.Entities.UserSession.Current.RoleId;
        bool isDirectDL = roleid - roleidDL == 1;
        if (dr.Table.Columns.Contains("disablenumber") ? !CastAide.AsBoolean(dr["syncbingo"]) : false)
        {
            if (isDirectDL)
            {
                TemplateHandler.RemoveBlock(ref rowTpl, "no-bingo");

                TemplateHandler.Assign(ref rowTpl, "divOpBG", "divSync");
                TemplateHandler.Assign(ref rowTpl, "editBG", Resources.Lang.sync);
                TemplateHandler.Assign(ref rowTpl, "statusBG", "0"); // Not syn Bingo
            }
            else
            {
                TemplateHandler.RemoveBlock(ref rowTpl, "bingo");
                TemplateHandler.RemoveBlock(ref rowTpl, "edit-BG");
                
                TemplateHandler.Assign(ref rowTpl, "divOpBG", "divDisable");
                TemplateHandler.Assign(ref rowTpl, "editBG", "");
                TemplateHandler.Assign(ref rowTpl, "statusBG", "-1"); // Can not take any action
            }
        }
        else
        {
            if (isDirectDL || (roleid == 4 && roleidDL == 1) || (roleid == 3 && roleidDL == 1))
            {
                if (dr.Table.Columns.Contains("disablebingo"))
                {
                    TemplateHandler.RemoveBlock(ref rowTpl, "no-bingo");
                    if (CastAide.AsBoolean(dr["disablebingo"]))
                    {
                        TemplateHandler.Assign(ref rowTpl, "divOpBG", "divEditDisabled");
                    }
                    else
                    {
                        TemplateHandler.Assign(ref rowTpl, "divOpBG", "divEdit");
                    }
                }
                TemplateHandler.Assign(ref rowTpl, "editBG", Resources.Lang.edit);
            }
            else
            {
                TemplateHandler.Assign(ref rowTpl, "divOpBG", "divViewCS");
                TemplateHandler.Assign(ref rowTpl, "editBG", Resources.Lang.view);
            }
            TemplateHandler.Assign(ref rowTpl, "statusBG", "1");
        }
    } 
    #endregion

    #region Generate Casino Column
    private static void GenCasinoForAgentList(ref string temp_content, object drCasino, int roleid)
    {
        if (!UserPermission.IsShowCasino)
        {
            TemplateHandler.Assign(ref temp_content, "statusCS", "-1"); // Can not take any action
            return;
        }

        if (!CastAide.AsBoolean(drCasino))
        {
            if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId - roleid == 1)
            {
                TemplateHandler.RemoveBlock(ref temp_content, "no-casino");
                // Edit/Sync Casino button
                TemplateHandler.Assign(ref temp_content, "divOp", "divSync");
                TemplateHandler.Assign(ref temp_content, "editCS", Resources.Lang.sync);

                TemplateHandler.Assign(ref temp_content, "statusCS", "0"); // Not syn
            }
            else
            {
                TemplateHandler.RemoveBlock(ref temp_content, "casino");
                TemplateHandler.RemoveBlock(ref temp_content, "edit-CS");

                TemplateHandler.Assign(ref temp_content, "divOp", "divDisable");
                TemplateHandler.Assign(ref temp_content, "editCS", "");

                TemplateHandler.Assign(ref temp_content, "statusCS", "-1"); // Can not take any action
            }


        }
        else
        {
            TemplateHandler.RemoveBlock(ref temp_content, "no-casino");
            if ((Sunplus.Agent.Model.Entities.UserSession.Current.RoleId - roleid == 1) || (roleid == 1))
            {
                if (CastAide.AsBoolean(drCasino))
                {
                    // Edit/Sync Casino button
                    TemplateHandler.Assign(ref temp_content, "divOp", "divEditDisabled");
                }
                else
                {
                    // Edit/Sync Casino button
                    TemplateHandler.Assign(ref temp_content, "divOp", "divEdit");
                }
                TemplateHandler.Assign(ref temp_content, "editCS", Resources.Lang.edit);
            }
            else
            {
                // Edit/Sync Casino button
                TemplateHandler.Assign(ref temp_content, "divOp", "divViewCS");
                TemplateHandler.Assign(ref temp_content, "editCS", Resources.Lang.view);
            }

            TemplateHandler.Assign(ref temp_content, "statusCS", "1");
        }
    }
    private static void GenCasino(ref string rowTpl, DataRow dr, int roleidDL)
    {
        if (!UserPermission.IsShowCasino)
        {
            TemplateHandler.Assign(ref rowTpl, "statusCS", "-1"); // Can not take any action
            return;
        }

        int roleid = Sunplus.Agent.Model.Entities.UserSession.Current.RoleId;
        bool isDirectDL = roleid - roleidDL == 1;

        if (!CastAide.AsBoolean(dr["synccasino"]))
        {
            if (isDirectDL)
            {
                TemplateHandler.RemoveBlock(ref rowTpl, "no-casino");
                // Edit/Sync Casino button
                TemplateHandler.Assign(ref rowTpl, "divOp", "divSync");
                TemplateHandler.Assign(ref rowTpl, "editCS", Resources.Lang.sync);
                
                TemplateHandler.Assign(ref rowTpl, "statusCS", "0"); // Not syn
            }
            else
            {
                TemplateHandler.RemoveBlock(ref rowTpl, "casino");
                TemplateHandler.RemoveBlock(ref rowTpl, "edit-CS");
                
                TemplateHandler.Assign(ref rowTpl, "divOp", "divDisable");
                TemplateHandler.Assign(ref rowTpl, "editCS", "");

                TemplateHandler.Assign(ref rowTpl, "statusCS", "-1"); // Can not take any action
            }

            
        }
        else
        {
            TemplateHandler.RemoveBlock(ref rowTpl, "no-casino");
            if (isDirectDL || (roleid == 4 && roleidDL == 1) || (roleid == 3 && roleidDL == 1))
            {
                if (CastAide.AsBoolean(dr["disablecasino"]))
                {
                    // Edit/Sync Casino button
                    TemplateHandler.Assign(ref rowTpl, "divOp", "divEditDisabled");
                }
                else
                {
                    // Edit/Sync Casino button
                    TemplateHandler.Assign(ref rowTpl, "divOp", "divEdit");
                }
                TemplateHandler.Assign(ref rowTpl, "editCS", Resources.Lang.edit);
            }
            else
            {
                // Edit/Sync Casino button
                TemplateHandler.Assign(ref rowTpl, "divOp", "divViewCS");
                TemplateHandler.Assign(ref rowTpl, "editCS", Resources.Lang.view);
            }

            TemplateHandler.Assign(ref rowTpl, "statusCS", "1");
        }
    }
    #endregion

    #region Gen P2P
    private static void GenP2P(ref string rowTpl, DataRow dr, int roleidDL)
    {
        if (!UserPermission.IsShowP2P)
        {
            TemplateHandler.Assign(ref rowTpl, "statusP2P", "-1"); // Can not take any action
            return;
        }

        int roleid = Sunplus.Agent.Model.Entities.UserSession.Current.RoleId;
        bool isDirectDL = roleid - roleidDL == 1;

        //if (isDirectDL)
        //{
            //TemplateHandler.RemoveBlock(ref rowTpl, "no-p2p");
            // Edit/Sync Casino button
            TemplateHandler.Assign(ref rowTpl, "divOpP2P", "divEdit");
            TemplateHandler.Assign(ref rowTpl, "editP2P", Resources.Lang.edit);

            TemplateHandler.Assign(ref rowTpl, "statusP2P", "0"); // Not syn
            TemplateHandler.Assign(ref rowTpl, "custid", dr["custid"]);
            TemplateHandler.Assign(ref rowTpl, "roleid", roleidDL);
            TemplateHandler.Assign(ref rowTpl, "username", dr["username"]);
        //}
        //else
        //{
        //    TemplateHandler.RemoveBlock(ref rowTpl, "no-p2p");
        //    TemplateHandler.RemoveBlock(ref rowTpl, "edit-P2P");

        //    TemplateHandler.Assign(ref rowTpl, "divOpP2P", "divDisable");
        //    TemplateHandler.Assign(ref rowTpl, "editP2P", "");
            
        //    TemplateHandler.Assign(ref rowTpl, "statusP2P", "-1"); // Can not take any action
        //}

    }
    #endregion

    #region Generate Title of Downline List
    private static void GenDownlineListTitle(ref string mainTpl, int level)
    {
        switch (level)
        {
            case 3:
                TemplateHandler.Assign(ref mainTpl, "lblMasterList", Resources.Lang.lblMasterList);
                break;
            case 2:
                TemplateHandler.Assign(ref mainTpl, "lblMasterList", Resources.Lang.lblAgentList);
                break;
            case 1:
                TemplateHandler.Assign(ref mainTpl, "lblcustomerlist", Resources.Lang.customerlist);
                TemplateHandler.RemoveBlock(ref mainTpl, "limit-tr-custlist");
                break;
        }
    }
    #endregion

    #region Generate Title links of Customer list
    private static void GenCustomerListTitle(ref string mainTpl, int custid)
    {
        if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId != 2)
        {
            TemplateHandler.Assign(ref mainTpl, "agentlist", Resources.Lang.lblAgentList);
            TemplateHandler.Assign(ref mainTpl, "url-agentlist", "DownlineList.aspx");
            TemplateHandler.Assign(ref mainTpl, "positiontaking", Resources.Lang.positiontaking);
            TemplateHandler.Assign(ref mainTpl, "creditbalance", Resources.Lang.creditbalance);
            TemplateHandler.ReplaceBlock(ref mainTpl, "limit-customerlist", Resources.Lang.customerlist);
            TemplateHandler.Assign(ref mainTpl, "url-credit", "CreditBalance.aspx?act=Memberlist&custid=" + custid);
            TemplateHandler.Assign(ref mainTpl, "url-position", "../_MemberInfo/PositionTakingList/Sportbook/SportbookPositionTakingList.aspx?type=agent&custid=" + custid);
        }
    }
    #endregion
}
