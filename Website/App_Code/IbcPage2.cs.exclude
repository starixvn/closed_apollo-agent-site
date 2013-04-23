using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DNAide.Web;

[Obsolete]
public class IbcPage2 : System.Web.UI.Page
{
    private string _locale = string.Empty;
    private string _vers = string.Empty;
    protected string ThemePath;
    protected override void InitializeCulture()
    {
        System.Globalization.CultureInfo def = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Sunplus.Agent.Model.Entities.UserSession.Current.Language);
        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(Sunplus.Agent.Model.Entities.UserSession.Current.Language);

        System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat = def.DateTimeFormat;

        base.InitializeCulture();     
    }

    public bool IsIbcPagePostBack
    {
        get 
        {
            return (Request.Form.Count > 0);
        }
    }

    public string GetText(string key)
    {
        object text = GetGlobalResourceObject("Lang", key);
        if (text == null) return key;
        return text.ToString();
    }

    public string GetText(string culture, string key)
    {
        if (culture == _locale) return GetText(key);
        this.Culture = culture;
        this.UICulture = culture;
        string result = GetText(key);
        this.Culture = _locale;
        this.UICulture = _locale;
        return result;
    }
    public string Locale
    {
        get
        {
            return _locale;
        }
    }
    protected string RelativePath
    {
        get
        {
            string prePath = string.Empty;
            string fPath = Request.AppRelativeCurrentExecutionFilePath.Replace("~/", string.Empty);
            while (true)
            {
                int pos = fPath.IndexOf("/");
                if (pos == -1) break;
                fPath = fPath.Remove(0, pos + 1);
                prePath += "../";
            }
            return prePath;
        }
    }
  
    protected string CssLinks
    {
        get
        {
            string link = "<link href='{RelPath}' type='text/css' rel='stylesheet' />";
            string links = "\n";
            string[] cssFiles = AppSettings.CssFiles;
            for (int i = 0; i < cssFiles.Length; ++i)
            {
                links += link.Replace("{RelPath}", RelativePath + cssFiles[i]);
            }
            links += "\n";
            return links;
        }
    }

    protected override void Render(HtmlTextWriter writer)
    {
        // Hot fix for Opera browser (IIS does sniffing user agent)
        Response.ContentType = "text/html";

        base.Render(writer);
        WriteJsVariables(writer);
    }

    private System.Text.StringBuilder _jsVariables = new System.Text.StringBuilder(5000);
    public void AddJsRawVariable(string strName, string value)
    {
        _jsVariables.Append(strName + "=" + value.ToString() + ";\r\n");
    }

    public void AddJsVariable(string strName, string value)
    {
        _jsVariables.Append(strName + "='" + value.ToString().Replace("'", "\\'") + "';\r\n");
    }

    private void WriteJsVariables(HtmlTextWriter writer)
    {
        if (_jsVariables.Length == 0) return;
        // write javascript variables
        writer.WriteLine("<script type=\"text/javascript\">\r\n");
        writer.WriteLine(_jsVariables.ToString());
        writer.WriteLine("</script>\r\n");
    }
}
