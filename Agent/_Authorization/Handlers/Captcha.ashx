<%@ WebHandler Language="C#" Class="LoginCaptcha" %>

using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Utils;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Model.BL;
using Fanex.Age.Utility.CaptchaUtil;

public class LoginCaptcha : IHttpHandler, IRequiresSessionState
{
    private Random _random = new Random();
    private int _choice = 1;

    public LoginCaptcha()
    {
        _choice = _random.Next(2);
    }

    public void ProcessRequest(HttpContext context)
    {
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        context.Response.Cache.SetNoStore();

        #region Captcha
        IUserSession session = UserSessionService.GetUserSessionInstance(context.Session.SessionID);

        string code = "";
        string value = "";
        string ip = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP();
        var captchaUtil = new CaptchaImage(ip, session.UserName, CaptchaType.Text);
        var bitmap = captchaUtil.Generate(out code, out value);

        session.CaptchaCode = value;

        // Set the image.
        // Change the response headers to output a JPEG image.
        context.Response.Clear();
        context.Response.ContentType = "image/Jpeg";

        // Write the image to the response stream in JPEG format.
        bitmap.Save(context.Response.OutputStream, ImageFormat.Jpeg);

        // Dispose of the CAPTCHA image object.
        bitmap.Dispose();
        #endregion
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}