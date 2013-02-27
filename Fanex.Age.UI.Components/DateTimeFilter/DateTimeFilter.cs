using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;

using TplCore.Element;

/// <summary>
/// Summary description for DateTimeFilter
/// </summary>
/// 
namespace Fanex.Age.UI.Components
{
    using DateTimeFilter;
    public class DateTimeFilter_Control : IComponent
    {
        DateTimeFilter_Template tpl = new DateTimeFilter_Template();
        /// <summary>
        ///    Resources.Lang.pleaseselect: "0"
        ///    Resources.Lang.today: "1"
        ///    Resources.Lang.yesterday: "2"
        ///    Resources.Lang.currentweek: "3"
        ///    Resources.Lang.lastweek: "4"
        ///    Resources.Lang.currentmonth: "5"
        ///    Resources.Lang.lastmonth: "6"
        ///    Resources.Lang.sincelastmonth: "7"
        ///    Resources.Lang.specificdate: "8"
        /// </summary>
        /// <param name="currentSelectd"></param>
        public DateTimeFilter_Control(int currentSelectd)
        {
            GenerateControl(currentSelectd);
        }
        public static implicit operator string(DateTimeFilter_Control value)
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
        private void GenerateControl(int Selected)
        {
            tpl.date = Resources.Lang.date;
            //RangeDate.Add(Resources.Lang.pleaseselect, "0");
            tpl.pleaseSelect = Resources.Lang.pleaseselect;

            Dictionary<string, string> RangeDate = new Dictionary<string, string>();            
            RangeDate.Add(Resources.Lang.today, "1");
            RangeDate.Add(Resources.Lang.yesterday, "2");
            RangeDate.Add(Resources.Lang.currentweek, "3");
            RangeDate.Add(Resources.Lang.lastweek, "4");
            RangeDate.Add(Resources.Lang.currentmonth, "5");
            RangeDate.Add(Resources.Lang.lastmonth, "6");
            RangeDate.Add(Resources.Lang.sincelastmonth, "7");
            RangeDate.Add(Resources.Lang.specificdate, "8");

            DateTimeFilter_options_Block tplOption = new DateTimeFilter_options_Block();
            tpl.options.Clear();
            if (RangeDate.Count > 0)
            {               
                int counter = RangeDate.Count;
                IDictionaryEnumerator en = RangeDate.GetEnumerator(); 
                while(en.MoveNext())
                {

                    tplOption.OptionText = en.Key.ToString();
                    tplOption.OptionValue = en.Value.ToString();
                    if (en.Value.ToString().CompareTo(Selected.ToString()) == 0)
                        tplOption.selected = "Selected";
                    else
                        tplOption.selected = string.Empty;
                    tpl.options.Append(tplOption);
                }
            }
            else
            {
                tpl.pleaseSelect = Resources.Lang.pleaseselect;
                tpl.options.selected = "Selected";
                tpl.options = tplOption;
            }
        }
    }
}