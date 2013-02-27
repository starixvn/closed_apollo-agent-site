// --------------------------------------------------------------------------
// <copyright file="HomeDrawAwayNoBet.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <create who="Jeff.Nguyen" date="2012.10.09"></create>
//   <change who="" date=""></change>
// </history>

namespace Fanex.Age.UI.TotalBets
{
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Text;
    using System.Collections;
    using Sunplus.Agent.Utilities;
    using Sunplus.Agent.Model.BL.TotalBets;
    using Sunplus.Agent.UI.BetList;
    using Sunplus.Agent.UI._BetList;
    using Sunplus.Agent.UI.BetList.Factories;
    using Fanex.Age.Excel.BetList;
    using Sunplus.Agent.Model.BL.Definitions;

    public partial class OtherBettypes_Page : Fanex.Age.UI.Reports.WinlossReports_Page
    {
        private bool exportExcel = false;
        private IBetListCmd command = null;

        public override void RenderContent(HtmlTextWriter writer)
        {
            DataTable betListData = View as DataTable;
            BetList_Template2 tpl = new BetList_Template2();
            tpl.title = Resources.Lang.lblOtherTypes ;
            UserTypes userType = UserTypes.Unknown;
            if (CurrentUserSession.IsSuper)
            {
                userType = UserTypes.Super;
            }
            else if (CurrentUserSession.IsMaster)
            {
                userType = UserTypes.Master;
            }
            else if (CurrentUserSession.IsAgent)
            {
                userType = UserTypes.Agent;
            }

            this.command = BetListFactory.CreateBetList("Bettype", CurrentUserSession.NetSessionId, _permission);
            DataTable betTransm = this.command.OutDataTable();
            
            if (this.exportExcel)
            {
                var resMan = GetLangResourceManager();
                var builder = new BetListReportBuilder(BetListType.Member, betListData, userType, tpl.title.ToUpper(), betTransm);
                var excelStream = builder.Build();
                if (excelStream != null)
                {
                    ExportToExcel(excelStream, "BetList.xls");
                    excelStream.Dispose();

                    return;
                }
            }
            else
            {
                // Estimate size of the content
                StringBuilder content = new StringBuilder(1024 * 2 * (betListData.Rows.Count + 3));
                RenderingFactory.Render(BetListType.Member, ref betListData, userType, content, exportExcel, betTransm);
                tpl.content = content.ToString();

                RenderFilterElementNames();
                tpl.ToString(writer);
            }
        }

        public override void ProcessRequest(ref Hashtable hash)
        {
            base.ProcessRequest(ref hash);
            exportExcel = Request["exporttoexcel.x"] != null ? true : false;
        }

        public override object ValidateRequest(Hashtable p)
        {
            return null;
        }

        public override object InvokeData(Hashtable p)
        {
            TotalBetsBO _TotalBetsBO = new TotalBetsBO(CurrentUserSession.NetSessionId);

            return _TotalBetsBO.GetOtherBettypes();
        }
    }
}