using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for PageUrl
/// </summary>
public class PageUrl
{
    public string Url;
    public int Id;
    public string Funtion;
    public string SubFuntion;
    public RolePermissionPair[] RoleRules;
}
public class RolePermissionPair
{
    public byte RoleId;
    public string Permission;
}
