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

    public class TextBlock
    {
        private string _text;
        private IFont _font;

        public string Text
        {
            get { return _text; }
        }

        public IFont Font
        {
            get { return _font; }
        }

        public TextBlock(string text)
        {
            _text = text;
        }

        public TextBlock(string text, IFont font)
        {
            _text = text;
            _font = font;
        }
    }
}