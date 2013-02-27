namespace Sunplus.Agent.UI.BetList.Excel
{
    using Fanex.Age.Utility.NPOI;
    using NPOI.SS.UserModel;
    using Sunplus.Agent.UI.BetList.Utils;

    public class NumberTextRender
    {
        private IFont posFontText;
        private IFont negFontText;
        private RtfTextRender renderContent;

        public NumberTextRender(RtfTextRender render, IFont posFont, IFont negFont)
        {
            this.renderContent = render;
            this.posFontText = posFont;
            this.negFontText = negFont;
        }

        public void Render(string text)
        {
            string removeText = text.Replace(ConvertByBetType.FontOpenTag, string.Empty).Replace(ConvertByBetType.FontCloseTag, string.Empty);
            if (text.Length == removeText.Length)
            {
                this.renderContent.AddText(removeText, this.posFontText);
            }
            else
            {
                this.renderContent.AddText(removeText, this.negFontText);
            }
        }
    }
}