using System.Collections.Generic;
using NPOI.SS.UserModel;

namespace Fanex.Age.Utility.NPOI
{
    public interface ITextFormat
    {
        Dictionary<string, IFont> Fonts { get; }
        string Text { get; }
    }
}
