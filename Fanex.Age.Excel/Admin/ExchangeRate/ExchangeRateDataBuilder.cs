/*
 * Created 20130318@Lewis
 * Revision:
 *
 */

namespace Fanex.Age.Excel.Admin.ExchangeRate
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Resources;
    using System.Web;
    using Fanex.Age.Business.Admin.ExchangeRate;
    using Fanex.Age.DTO.Admin.ExchangeRate;

    public class ExchangeRateDataBuilder
    {
        public ExchangeRateDataBuilder(DataTable data, ResourceManager resMgr)
        {
            var parser = new ExchangeRateBusiness();
            this.Data = parser.ToList(data);
        }

        public IList<ExchRate> Data { get; private set; }

        public MemoryStream Build()
        {
            string tplPath = string.Format("{0}\\ExcelTemplates\\_Admin\\ExchangeRate\\ExchangeRate.xls",
                HttpContext.Current.Server.MapPath("~"));
            var data = new ExchangeRateList(tplPath, this);

            if (data != null && data is IReportBuilder)
            {
                var stream = new MemoryStream();
                var hssfWorkbook = ((IReportBuilder)data).Build();

                if (hssfWorkbook != null)
                {
                    hssfWorkbook.Write(stream);
                    return stream;
                }

                data.Dispose();
            }

            return null;
        }
    }
}