/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Excel
{
    using System;
    using System.IO;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    public abstract class TemplateReport : IDisposable
    {
        protected HSSFWorkbook hssfWorkbook;

        public HSSFWorkbook Workbook
        {
            get
            {
                return hssfWorkbook;
            }
        }

        public TemplateReport(string filePath)
        {
            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            hssfWorkbook = new HSSFWorkbook(file);
        }

        public void Dispose()
        {
            if (hssfWorkbook != null)
            {
                hssfWorkbook.Dispose();
            }
        }

        public void BoldAllFonts()
        {
            for (short i = 0; i < hssfWorkbook.NumberOfFonts; i++)
            {
                var font = hssfWorkbook.GetFontAt(i);
                font.Boldweight = (short)FontBoldWeight.BOLD;
            }
        }
    }
}