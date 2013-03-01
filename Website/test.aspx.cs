using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fanex.Age.Utility;

public partial class test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write(Sunplus.Agent.Utilities.Encryption.CfsEnCrypt("@pldb@"));
        Response.Write("</br>");
        Response.Write(Sunplus.Agent.Utilities.Encryption.CfsDeCrypt("TBAw8F+PqCx3u382anlnOVRxKIJYMsMwXVn1hJpeyKk="));
        Response.Write("</br>");
        Response.Write(Sunplus.Agent.Utilities.Encryption.CfsDeCrypt("QYN7k+nZC5A="));
        //TBAw8F+PqCx3u382anlnOVRxKIJYMsMwXVn1hJpeyKk=
    }
}