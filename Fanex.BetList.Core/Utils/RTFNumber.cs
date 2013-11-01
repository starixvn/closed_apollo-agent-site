// ---------------------------------------------------------------------------------
// <copyright file="RTFNumber.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.10">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Utils
{
    using NPOI.SS.UserModel;

    /// <summary>
    /// Rich text number class.
    /// </summary>
    public class RTFNumber
    {
        private IFont posFontText;
        private IFont negFontText;
        private RtfTextRender renderContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="RTFNumber" /> class.
        /// </summary>
        /// <param name="render">The render.</param>
        /// <param name="posFont">The pos font.</param>
        /// <param name="negFont">The neg font.</param>
        public RTFNumber(RtfTextRender render, IFont posFont, IFont negFont)
        {
            this.renderContent = render;
            this.posFontText = posFont;
            this.negFontText = negFont;
        }

        /// <summary>
        /// Renders the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
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