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

        public AuthenticationService()
        {

        }

        /// <summary>
        /// Resource server contact with authorization server to get access token
        /// and authentication info
        /// </summary>
        /// <param name="code">code returned from TGP Service</param>
        /// <returns><see cref="JsonResponseModel"/></returns>
        private JsonResponseModel SubmitAuthCode(string code)
        {

            JsonResponseModel jsonResponseModel = new JsonResponseModel();

            // make a web request to authorization server
            var postData = string.Format(FormParams,
                code,
                OAuthSettings.Settings.RedirectUri,
                OAuthSettings.Settings.AppId,
                OAuthSettings.Settings.AppSecret);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(OAuthSettings.Settings.TokenUrl);
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
                Console.WriteLine(webException.ToString());
            }
            catch (AuthenticationException authException)
            {
                //writelog
                Console.WriteLine(authException.ToString());
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
        /// <param name="authorizationCode">authorization code returned from authorization server</param>
        /// <returns></returns>
        public AuthModel GetAuthInfo(string authorizationCode)
        {
            if (string.IsNullOrWhiteSpace(authorizationCode))
            {
                throw new ArgumentNullException("code");
            }

            AuthModel authModel = new AuthModel(SubmitAuthCode(authorizationCode));

            return authModel;
        }       
    }
}
