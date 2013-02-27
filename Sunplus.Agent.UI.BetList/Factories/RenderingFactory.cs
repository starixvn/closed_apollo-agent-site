// <copyright file="RenderingFactory.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved. 
// </copyright>
//
// <summary>
//  Render Factory
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.01.18">Created</change>
//  <change who="Kaden" date="2012.01.19">Updated</change>
// </history>
namespace Sunplus.Agent.UI.BetList.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using BetTypes;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.BL.Forecast;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;

    public class RenderingFactory
    {
        /// <summary>
        /// Render bet list follow bet list type and bet list data.
        /// </summary>
        /// <param name="betListType"></param>
        /// <param name="betListData"></param>
        /// <param name="userType"></param>
        /// <param name="exportExcel"></param>
        /// <param name="racingData"></param>
        /// <param name="liveCasinoData"></param>
        /// <returns></returns>
        public static StringBuilder Render(BetListType betListType, ref DataTable betListData, UserTypes userType, bool exportExcel, DataTable dataTable)
        {
            StringBuilder content = new StringBuilder();

            Render(betListType, ref betListData, userType, content, exportExcel, dataTable);

            return content;
        }

        /// <summary>
        /// Render bet list follow bet list type and bet list data.
        /// </summary>
        /// <param name="betListType"></param>
        /// <param name="betListData"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static StringBuilder Render(BetListType betListType, ref DataTable betListData, UserTypes userType)
        {
            return Render(betListType, ref betListData, userType, false, null);
        }

        /// <summary>
        /// Render bet list follow bet list type and bet list data.
        /// </summary>
        /// <param name="betListType"></param>
        /// <param name="betListData"></param>
        /// <param name="userType"></param>
        /// <param name="content"></param>
        public static void Render(BetListType betListType, ref DataTable betListData, UserTypes userType, ref StringBuilder content)
        {
            Render(betListType, ref betListData, userType, content, false, null);
        }

        /// <summary>
        /// Render bet list follow bet list type and bet list data.
        /// </summary>
        /// <param name="betListType"></param>
        /// <param name="betListData"></param>
        /// <returns></returns>
        public static void Render(BetListType betListType, ref DataTable betListData, UserTypes userType, StringBuilder content, bool exportExcel, DataTable dataTable)
        {
            // Append header
            HeaderRender(betListType, userType, content);

            DataRowCollection rows = betListData.Rows;
            int c = rows.Count;

            // Lowercase column name
            LowercaseColumnName(ref betListData);

            // Append content
            ContentRender(betListType, userType, content, rows, c, exportExcel, dataTable);

            // Append footer
            FooterRender(betListType, betListData, userType, content);
        }

        #region MixParlay

        /// <summary>
        /// RenderMixParlay bet list follow bet list data.
        /// </summary>
        /// <param name="betListType"></param>
        /// <param name="betListData"></param>
        /// <returns></returns>
        public static StringBuilder RenderMixParlay(ref DataTable betListData, bool exportExcel)
        {
            StringBuilder content = new StringBuilder();
            DataRowCollection rows = betListData.Rows;
            int c = rows.Count;

            // Append content
            MixParlayContentRender(BetListType.Default, content, rows, c, exportExcel);

            return content;
        }

        public static StringBuilder RenderMixParlay(BetListType betListType, ref DataTable betListData, bool exportExcel)
        {
            StringBuilder content = new StringBuilder();

            DataRowCollection rows = betListData.Rows;
            int c = rows.Count;

            // Append content
            MixParlayContentRender(betListType, content, rows, c, exportExcel);

            return content;
        }

        private static void MixParlayContentRender(BetListType betListType, StringBuilder content, DataRowCollection rows, int c, bool exportExcel)
        {
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

                item.ExportExcel = exportExcel;
                (item as BetTypeBase).AssignEvent(row);

                string oddMixParlay = "<span class='bl_num'><b> @ " + Formater.FormatNumber3(row["odds"], null) + "</b></span>";
                (item as BetTypeBase).Event.OddMixParlay.SetValue(oddMixParlay);
                if (i < c - 1)
                {
                    (item as BetTypeBase).Event.Append("<div class='line'></div>");
                }

                if ((betListType & BetListType.SmartPhone) == BetListType.SmartPhone && ((item as BetTypeBase).Event.homeTeam != null && (item as BetTypeBase).Event.awayTeam != null))
                {
                    (item as BetTypeBase).Event.LeagueName = null;
                }

                if (row["statusid"] != null && Convert.ToInt32(row["statusid"]) == 102)
                {
                    (item as BetTypeBase).Event.strikeThrough = "strike_through";
                }
                else
                {
                    (item as BetTypeBase).Event.strikeThrough = null;
                }

                (item as BetTypeBase).Event.ScoreMapIcon = null;
                (item as BetTypeBase).Event.ToString(ref content);
            }
        }

        #endregion

        #region Utilities

        private static void LowercaseColumnName(ref DataTable dt)
        {
            int c = dt.Columns.Count;

            for (int i = 0; i < c; i++)
            {
                dt.Columns[i].ColumnName = dt.Columns[i].ColumnName.ToLower();
            }
        }

        #endregion

        private static void ContentRender(BetListType betListType, UserTypes userType, StringBuilder content, DataRowCollection rows, int c, bool exportExcel, DataTable betTypeData)
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
                item.CustomizeColumns(userType, betListType);
                item.ExportExcel = exportExcel;
                item.OutDataTable = betTypeData;
                item.Assign(i + 1, row);

                // Hide Status: When view on SmartPhone
                if ((betListType & BetListType.SmartPhone) == BetListType.SmartPhone)
                {
                    (item as BetTypeBase).No = null;
                    (item as BetTypeBase).Status = null;

                    if ((item as BetTypeBase).Event.homeTeam != null && (item as BetTypeBase).Event.awayTeam != null && (((int)SportType.LiveCasino) != Convert.ToInt32(row["sporttype"])))
                    {
                        (item as BetTypeBase).Event.LeagueName = null;
                    }

                    switch (userType)
                    {
                        case UserTypes.Super: // super 
                            (item as BetTypeBase).Master = null;
                            (item as BetTypeBase).Agent = null;
                            break;
                        case UserTypes.Master: // master
                            (item as BetTypeBase).Super = null;
                            (item as BetTypeBase).Agent = null;
                            break;
                        case UserTypes.Agent: // agent
                            (item as BetTypeBase).Super = null;
                            (item as BetTypeBase).Master = null;
                            break;
                        default: // Hide all position taking
                            (item as BetTypeBase).Super = null;
                            (item as BetTypeBase).Master = null;
                            (item as BetTypeBase).Agent = null;
                            break;
                    }
                }

                item.ToString(ref content);
            }
        }

        private static void FooterRender(BetListType betListType, DataTable betListData, UserTypes userType, StringBuilder content)
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
                    footer.Assign(0, totalRow);
                }

                footer.ToString(ref content);
            }

            // Close bet list
            CloseBetList_Template close = new CloseBetList_Template();
            close.ToString(ref content);
        }

        private static void HeaderRender(BetListType betListType, UserTypes userType, StringBuilder content)
        {
            OpenBetList_Template open = new OpenBetList_Template();
            open.ToString(ref content);

            IBetItem header = BetItemFactory.CreateHeader(betListType);
            header.CustomizeColumns(userType, betListType);

            header.ToString(ref content);
        }
    }
}
