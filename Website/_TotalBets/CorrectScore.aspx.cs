using System.Collections;
using System.Web.UI;

/*
 * Created 20100507@Jacob - Render Financial page
 * Revision:
 * ?@? - ... 
 * 
 */
namespace Fanex.Age.UI.TotalBets
{
    public partial class CorrectScore_Page : TotalBets_Page
    {
        public override void RenderContent(HtmlTextWriter writer)
        {
        }

        public override object ValidateRequest(Hashtable p)
        {
            return null;
        }

        public override object InvokeData(Hashtable p)
        {
            return _TotalBetsBO.GetFinancial();
        }
    }
}
