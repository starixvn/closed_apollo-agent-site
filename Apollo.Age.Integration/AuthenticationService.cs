// --------------------------------------------------------------------------
// <copyright file="AuthenticationService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.26">Created</change>
// </history>


namespace Apollo.Age.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Integration.Configurations;
    using System.Configuration;
    using System.Net;
    using System.IO;
    using System.Web.Script.Serialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AuthenticationService
    {
        private const string FormParams = "grant_type=authorization_code&code={0}&redirect_uri={1}&client_id={2}&client_secret={3}";

        // shoule apply singleton pattern?
        private readonly OAuthSection _config;

        public AuthenticationService()
        {
            _config = (OAuthSection)ConfigurationManager.GetSection("OAuthSection");
        }

        /// <summary>
        /// Resource server contact with authorization server to get access token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private JsonResponseModel SubmitAuthCode(string code)
        {
          
            JsonResponseModel jsonResponseModel = new JsonResponseModel();

            // make a web request to authorization server
            var postData = string.Format(FormParams, code, _config.RedirectUri, _config.AppId, _config.AppSecret);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_config.TokenUrl);
            request.Method = WebRequestMethods.Http.Post; // it is required
            request.ContentType = "application/x-www-form-urlencoded";

            byte[] requestData = new ASCIIEncoding().GetBytes(postData);
            request.ContentLength = requestData.Length;
            request.GetRequestStream().Write(requestData, 0, requestData.Length);

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // read response stream and parse to JsonResponseModel
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
                    {
                        string jsonString = reader.ReadToEnd();
                        jsonResponseModel = new JavaScriptSerializer().Deserialize<JsonResponseModel>(jsonString);
                    }
                }
            }
            catch (WebException webException)
            {
                //writelog
            }
            catch (AuthenticationException authException)
            {
                //writelog
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return jsonResponseModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">see Integration Document</param>
        /// <returns></returns>
        public AuthModel GetAuthInfo(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException("code");
            }

            AuthModel authModel = new AuthModel(SubmitAuthCode(code));

            return authModel;
        }

        private readonly string AuthScriptTemplate = "<script src=\"{0}\" type=\"text/javascript\"></script>" +
            "<script type=\"text/javascript\">" +
            "TGP.authorize_endpoint = \"{1}\"" +
            "TGP.authorize({client_id=\"{2}\", redirect_uri=\"{3}\",state: \"active\"})";

        public string RenderAuthScriptSection()
        {
            string scriptSection = string.Format(AuthScriptTemplate,
                _config.TgpScriptSrc,
                _config.AuthorizeUrl,
                _config.AppId,
                _config.RedirectUri,
                "active");

            return scriptSection;
        }
    }
}
