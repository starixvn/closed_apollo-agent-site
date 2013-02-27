namespace Fanex.Age.Excel.BetList
{
    using System.Web;

    public class TemplateSettings
    {
        private string dirPath;

        public TemplateSettings()
        {
            this.dirPath = HttpContext.Current.Server.MapPath("~") + "\\ExcelTemplates\\_BetList";
        }

        public string GetBetListPath()
        {
            return this.dirPath + "\\" + "BetList.xls";
        }
    }
}