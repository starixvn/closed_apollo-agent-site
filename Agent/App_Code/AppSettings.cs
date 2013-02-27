using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.IO;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using System.Web.Configuration;

using Sunplus.Agent.Utilities;


[Obsolete]
public partial class AppSettings
{

    public enum MySite
    {
        Member = 1,
        Agent = 2,
        Master = 3,
        Super = 4
    };


    static AppSettings()
    {

    }

    public static string[] CssFiles
    {
        get
        {
            return new string[] { };
        }
    }

    public static void InitThemeInfo()
    {

    }

}
