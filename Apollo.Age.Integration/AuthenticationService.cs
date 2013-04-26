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

        private readonly OAuthSection _config;

        public AuthenticationService()
        {
            _config = (OAuthSection)ConfigurationManager.GetSection("OAuthSection");
        }

        private JsonResponseModel SubmitAuthCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException("code");
            }

            JsonResponseModel jsonResponseModel;

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
                
            }
            catch (AuthenticationException authException)
            {

            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return null;
        }
    }
}
