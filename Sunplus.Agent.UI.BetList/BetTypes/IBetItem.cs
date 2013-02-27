namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.UI.BetList.Excel;

    public interface IBetItem : TplCore.Element.IComponent
    {
        bool ExportExcel { get; set; }

        DataTable OutDataTable { get; set; }

        bool IsAdminCanViewVNIP { get; set; }

        List<int> ScoreMapBetTypes { get; set; }

        string Render();

        void ToString(ref StringBuilder bd);

        void Assign(int index, DataRow row);

        void Assign(int index, DataRow row, BetListWorkbook workbook);

        void CustomizeColumns(UserTypes userType, BetListType betListType);

        void CustomizeColumns(UserTypes userType, BetListType betListType, BetListWorkbook workbook);
    }
}
