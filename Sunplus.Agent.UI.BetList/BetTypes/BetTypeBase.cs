namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System.Collections.Generic;
    using System.Data;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.UI.BetList.Excel;

    /// <summary>
    /// Base Bettype
    /// </summary>
    public abstract class BetTypeBase : BetType1_Template, IBetItem
    {
        #region IBetItem Members

        public bool IsAdminCanViewVNIP { get; set; }

        public List<int> ScoreMapBetTypes { get; set; }

        public bool ExportExcel
        {
            get;
            set;
        }

        public DataTable OutDataTable
        {
            get;
            set;
        }

        public DataRow[] MatchedRows
        {
            get;
            protected set;
        }

        #endregion

        public IUserSession CurrentUserSession 
        {
            get 
            {
                return UserSession.Current;
            }
        }

        public virtual void AssignEvent(DataRow row) 
        { 
        }

        #region IBetItem Members

        public abstract string Render();

        public abstract void Assign(int index, DataRow row);

        public abstract void Assign(int index, DataRow row, BetListWorkbook workbook);

        public abstract void RenderBetTeam(BetTypeExcelData excelData);

        public abstract object GetChoice(BetTypeExcelData excelData);

        public abstract object GetChoice(BetTypeExcelData excelData, bool isMixParlay);

        public abstract object GetOdds(BetTypeExcelData excelData);

        public abstract object GetStatus(BetTypeExcelData excelData);

        public abstract object GetSuperPT_Comm(BetTypeExcelData excelData);

        public abstract object GetMasterPT_Comm(BetTypeExcelData excelData);

        public abstract object GetAgentPT_Comm(BetTypeExcelData excelData);

        public void CustomizeColumns(UserTypes userType, BetListType betListType, BetListWorkbook workbook)
        {
            CustomizeColumns(userType, betListType);
        }

        public abstract void CustomizeColumns(Sunplus.Agent.Model.BL.Definitions.UserTypes userType, BetListType betListType);

        #endregion

        #region IComponent Members

        public abstract void HideLinkedResources();

        #endregion        
    }
}
