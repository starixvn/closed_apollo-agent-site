// ---------------------------------------------------------------------------------
// <copyright file="TextBlock.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.10">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Entities
{
    using NPOI.SS.UserModel;

    /// <summary>
    /// Text Block.
    /// </summary>
    public class TextBlock
    {
        private string _text;
        private IFont _font;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBlock"/> class.
        /// </summary>
        /// <param name="text">Input The text.</param>
        public TextBlock(string text)
        {
            _text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBlock"/> class.
        /// </summary>
        /// <param name="text">Input The text.</param>
        /// <param name="font">Input The font.</param>
        public TextBlock(string text, IFont font)
        {
            _text = text;
            _font = font;
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <value>
        /// Input The text.
        /// </value>
        public string Text
        {
            get { return _text; }
        }

        /// <summary>
        /// Gets the font.
        /// </summary>
        /// <value>
        /// Input The font.
        /// </value>
        public IFont Font
        {
            get { return _font; }
        }
    }
}