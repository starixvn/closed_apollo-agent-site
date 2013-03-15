// ---------------------------------------------------------------------------------
// <copyright file="RtfTextRender.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.10">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Utils
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Renderer for RTF
    /// </summary>
    public class RtfTextRender
    {
        private IFont _font;
        private List<TextBlock> _blocks = new List<TextBlock>();

        public RtfTextRender()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RtfTextRender" /> class.
        /// </summary>
        /// <param name="font">The font.</param>
        public RtfTextRender(IFont font)
            : base()
        {
            _font = font;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            _blocks.Clear();
        }

        /// <summary>
        /// Renders this instance.
        /// </summary>
        /// <returns></returns>
        public IRichTextString Render()
        {
            string s = "";
            foreach (var block in _blocks)
            {
                s += block.Text;
            }

            IRichTextString result = new HSSFRichTextString(s);
            int textIndex = 0;
            foreach (var block in _blocks)
            {
                IFont font = _font;
                if (font == null)
                {
                    font = block.Font;
                }
                else
                {
                    if (block.Font != null)
                    {
                        font = block.Font;
                    }
                }
                if (font != null)
                {
                    int startIndex = textIndex;
                    int endIndex = textIndex + block.Text.Length;
                    result.ApplyFont(textIndex, endIndex, font);
                }
                textIndex += block.Text.Length;
            }

            return result;
        }

        /// <summary>
        /// Adds the text.
        /// </summary>
        /// <param name="block">The block.</param>
        public void AddText(TextBlock block)
        {
            _blocks.Add(block);
        }

        /// <summary>
        /// Adds the text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void AddText(string text)
        {
            _blocks.Add(new TextBlock(text));
        }

        /// <summary>
        /// Adds the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        public void AddText(string text, IFont font)
        {
            _blocks.Add(new TextBlock(text, font));
        }
    }
}