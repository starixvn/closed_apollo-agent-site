// ---------------------------------------------------------------------------------
// <copyright file="RTFNumber.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.10">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Utils
{
    using NPOI.SS.UserModel;

    /// <summary>
    /// Rich text number class.
    /// </summary>
    public class RTFNumber
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RTFNumber" /> class.
        /// </summary>
        /// <param name="render">The render.</param>
        /// <param name="posFont">The positive font.</param>
        /// <param name="negFont">The negative font.</param>
        public RTFNumber(RtfTextRender render, IFont posFont, IFont negFont)
        {
            this.RenderContent = render;
            this.PosFontText = posFont;
            this.NegFontText = negFont;
        }

        private IFont PosFontText { get; set; }

        private IFont NegFontText { get; set; }

        private RtfTextRender RenderContent { get; set; }

        /// <summary>
        /// Renders the specified text.
        /// </summary>
        /// <param name="text">The string text.</param>
        public void Render(string text)
        {
            string removeText = text.Replace(ConvertByBetType.FontOpenTag, string.Empty).Replace(ConvertByBetType.FontCloseTag, string.Empty);
            if (text.Length == removeText.Length)
            {
                this.RenderContent.AddText(removeText, this.PosFontText);
            }
            else
            {
                this.RenderContent.AddText(removeText, this.NegFontText);
            }
        }
    }
}