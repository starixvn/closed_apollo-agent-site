using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using System.Diagnostics;

namespace Fanex.Age.Utility.NPOI
{
    public static class TextFormatRunner
    {
        public static IRichTextString Run(RtfTextRender renderer, ITextFormat value)
        {
            Debug.Assert(renderer != null, "renderer must be not null");
            Debug.Assert(value != null, "value must be not null");
            return null;
        }
    }
}
