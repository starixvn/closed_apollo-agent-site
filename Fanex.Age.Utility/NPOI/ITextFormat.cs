using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;

namespace Fanex.Age.Utility.NPOI
{
    public interface ITextFormat
    {
        Dictionary<string, IFont> Fonts { get; }
        string Text { get; }
    }
}
