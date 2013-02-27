namespace Sunplus.Agent.UI.BetList.Excel
{
    using System.Data;
    using Fanex.Age.Utility.NPOI;
    using NPOI.SS.UserModel;

    public class BetTypeExcelData
    {
        public BetTypeExcelData()
        {
        }

        public DataRow DataRow { get; set; }

        public IFont NormalFont { get; set; }

        public IFont PosFont { get; set; }

        public IFont NegFont { get; set; }

        public RtfTextRender RtfTextRender { get; set; }        
    }
}