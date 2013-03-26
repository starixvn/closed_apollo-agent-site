using TplCore.Element;
/// <summary>
/// Summary description for AnalysisReportFilterOnChart
/// </summary>
/// 
namespace Fanex.Age.UI.Components
{
    using AnalysisReportFilter;
    public class AnalysisReportFilterOnChart_Control : IComponent
    {
        AnalysisReportFilterOnChart_Template tpl = new AnalysisReportFilterOnChart_Template();
        public AnalysisReportFilterOnChart_Control(IElement parent, string[] optionId, string[] optionVal, string[] onClick, bool[] isChecked)
        {
            tpl.monthlyId = optionId[0] as string;
            tpl.productId = optionId[1] as string;

            tpl.monthlyVal = optionVal[0] as string;
            tpl.productVal = optionVal[1] as string;

            tpl.monthlyonClick = onClick[0] as string;
            tpl.productonClick = onClick[1] as string;

            tpl.monthlyischecked = isChecked[0] ? "Checked" : string.Empty;
            tpl.productischecked = isChecked[1] ? "Checked" : string.Empty;

            tpl.monthly = Resources.Lang.monthly;
            tpl.product = Resources.Lang.product;
            tpl.reportby = Resources.Lang.reportby;
        }

        public static implicit operator string(AnalysisReportFilterOnChart_Control value)
        {
            return value.ToString();
        }
        public void HideLinkedResources()
        {

        }
        public override string ToString()
        {
            return tpl.ToString();
        }
    }
}
