namespace Sunplus.Agent.UI.BetList.Excel
{
    using System.Collections.Generic;
    using Fanex.Age.Utility.NPOI;
    using Fanex.Age.Utility.String;
    using NPOI.SS.UserModel;

    public class BetListWorkbook
    {
        public readonly short DefaultVerSpacing = 72;
        public readonly short DefaultTextHeight = 216;

        public BetListWorkbook(IWorkbook workbook, string title)
        {
            this.Workbook = workbook;
            this.Columns = new ColumnIndex();
            this.ResultSheet = this.Workbook.GetSheet("BetList");
            this.TemplateSheet = this.Workbook.GetSheet("Template");

            this.Title = title;
            if (string.IsNullOrEmpty(title))
            {
                this.Workbook.SetSheetName(this.Workbook.GetSheetIndex(this.ResultSheet), "CancelBetList");
                this.TitleHeaderRowIndex = -1;
                this.ColumnHeaderRowIndex = 0;
                this.ContentRowIndex = 1;
            }
            else
            {
                this.TitleHeaderRowIndex = 0;
                this.ColumnHeaderRowIndex = 1;
                this.ContentRowIndex = 2;
            }

            this.NormalFont = workbook.CreateFont();
            this.PosFont = workbook.CreateFont();
            this.PosFont.Boldweight = (short)FontBoldWeight.BOLD;
            this.NegFont = workbook.CreateFont();
            this.NegFont.Boldweight = (short)FontBoldWeight.BOLD;
            this.NegFont.Color = IndexedColors.RED.Index;
            RtfFontFormatter fontFormatter = new RtfFontFormatter("Tahoma", 9);
            fontFormatter.Format(this.NormalFont);
            fontFormatter.Format(this.PosFont);
            fontFormatter.Format(this.NegFont);
        }

        public IWorkbook Workbook { get; private set; }

        public string Title { get; private set; }

        public ISheet ResultSheet { get; private set; }

        public ISheet TemplateSheet { get; private set; }

        public IFont NormalFont { get; private set; }

        public IFont PosFont { get; private set; }

        public IFont NegFont { get; private set; }

        public ColumnIndex Columns { get; private set; }

        public int TitleHeaderRowIndex { get; private set; }

        public int ColumnHeaderRowIndex { get; private set; }

        public int ContentRowIndex { get; private set; }        

        public int FooterRowIndex
        {
            get
            {
                return this.ContentRowIndex;
            }
        }

        public List<int> ShownColumns
        {
            get
            {
                var result = new List<int>();
                foreach (var property in this.Columns.GetType().GetProperties())
                {
                    short value = (short)property.GetValue(this.Columns, null);
                    if (value != -1)
                    {
                        result.Add(value);
                    }
                }

                return result;
            }
        }

        public void RemoveColumns(List<int> columnList)
        {
            this.Columns.RemoveColumns(columnList);
            this.ResultSheet.RemoveColumns(columnList);
            this.TemplateSheet.RemoveColumns(columnList);
        }

        public void RemoveTemplateSheet()
        {
            this.Workbook.RemoveSheetAt(this.Workbook.GetSheetIndex(this.TemplateSheet));
        }

        public IRow CopyRow(int index)
        {
            var rowId = this.ContentRowIndex++;
            if (index % 2 == 0)
            {
                this.ResultSheet.CopyRow(this.TemplateSheet, 1, rowId);
            }
            else
            {
                this.ResultSheet.CopyRow(this.TemplateSheet, 3, rowId);
            }

            return this.ResultSheet.GetRow(rowId);
        }

        public short GetRowHeight(IRichTextString rtfText)
        {
            return (short)((rtfText.String.GetLineBreakTotal() * this.DefaultTextHeight) + this.DefaultVerSpacing);
        }

        public short GetRowHeight(object value)
        {
            string s = string.Empty;
            if (value is IRichTextString)
            {
                s = ((IRichTextString)value).String;
            }
            else
            {
                s = value.ToString();
            }

            return (short)((s.GetLineBreakTotal() * this.DefaultTextHeight) + this.DefaultVerSpacing);
        }

        public class ColumnIndex
        {
            public ColumnIndex()
            {
                short id = 0;
                foreach (var property in this.GetType().GetProperties())
                {
                    property.SetValue(this, id++, null);
                }
            }

            public short No { get; private set; }

            public short Member { get; private set; }

            public short TransTime { get; private set; }

            public short Choice { get; private set; }

            public short Odds { get; private set; }

            public short Stack { get; private set; }

            public short WinLoss { get; private set; }

            public short Status { get; private set; }

            public short Super { get; private set; }

            public short Master { get; private set; }

            public short Agent { get; private set; }            

            public void RemoveColumns(List<int> columns)
            {
                short defaultValue = -1;

                foreach (var property in this.GetType().GetProperties())
                {
                    short value = (short)property.GetValue(this, null);
                    if (columns.Contains(value))
                    {
                        property.SetValue(this, defaultValue, null);
                    }
                }

                short id = 0;
                foreach (var property in this.GetType().GetProperties())
                {
                    short value = (short)property.GetValue(this, null);
                    if (value != defaultValue)
                    {
                        property.SetValue(this, id++, null);
                    }
                }
            }
        }
    }
}