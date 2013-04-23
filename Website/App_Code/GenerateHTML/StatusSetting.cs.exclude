using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using DNAide.Web;
using System.Text;
/// <summary>
/// Summary description for StatusSetting
/// </summary>
public class StatusSetting
{
    public static string StatusDisable(string colspan, Hashtable htbDis, string myTemplate)
    {
        //if (colspan == "") colspan = "<td colspan='2'><td>";                   
        StringBuilder TemplateFina = new StringBuilder();
        TemplateFina.Append("<tr>"); 
            List<string> ArrTemplate = new List<string>();
            IDictionaryEnumerator myEn = htbDis.GetEnumerator();
            while (myEn.MoveNext())
            {
                if ((bool)myEn.Value == true)
                {
                    ArrTemplate.Add(TemplateHandler.CopyBlock(myTemplate, myEn.Key.ToString()));
                }
            }
            for (int i = 1; i <= ArrTemplate.Count; i++)
            {                
                TemplateFina.Append(ArrTemplate[i - 1]);
                if (i % 2 == 0)
                {                   
                    TemplateFina.Append("</tr><tr>");
                }
            }
            if (ArrTemplate.Count % 2 != 0)
            {
                TemplateFina.Append(colspan);
            }
            return TemplateFina.ToString().Substring(0, TemplateFina.ToString().Length - 4);                
                
    }
    public static string GenSubMenu(string[] arrUrl, string[] arrText,string[]arrDis,string[] arrHd)
    {
        string strTpl = TemplateHandler.LoadTemplate("MemberInfo/HeaderHRFICS.html");        
        string strMenuItem = TemplateHandler.CopyBlock(strTpl, "menu");
        StringBuilder strBuild = new StringBuilder();
        int counter = 0;
        for (int i = 0; i < arrText.Length; i++)
        {
            if (arrDis[i] == "0")
            {
                counter++;
                string strTmp = strMenuItem;
                TemplateHandler.Assign(ref strTmp, "url", arrUrl[i]);
                TemplateHandler.Assign(ref strTmp, "text", arrText[i]);
                TemplateHandler.Assign(ref strTmp, "curr", (counter == 1 ? "current" : ""));
                TemplateHandler.Assign(ref strTmp, "id", "IdCurr" + Convert.ToString((counter - 1)));
                TemplateHandler.Assign(ref strTmp, "hdisEdit", arrHd[i]);
                strBuild.AppendLine(strTmp);
            }
        }
        
        TemplateHandler.ReplaceBlock(ref strTpl, "menu", strBuild.ToString());
        TemplateHandler.Assign(ref strTpl, "length", counter.ToString());
        return strTpl;
    }
}
