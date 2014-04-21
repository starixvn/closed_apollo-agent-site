// --------------------------------------------------------------------------
// <copyright file="AuthenticationService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.26">Created</change>
// </history>

namespace Apollo.Age.Integration.Services
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web.Helpers;
    using Apollo.Age.Integration.Configurations;
    using Apollo.Age.Integration.Models;
    using Apollo.Age.Resources;

    /// <summary>
    /// Authentication service.
    /// </summary>
    public class AuthenticationService
    {
        private const string FormParams = "grant_type=authorization_code&code={0}&redirect_uri={1}&client_id={2}&client_secret={3}";

        public AuthenticationService()
        {
        }

        /// <summary>
        ///  Translate error message key returned from authentication service to understandable message.
        /// </summary>
        /// <param name="errorCode">Server_error is default error code.</param>
        /// <returns>Return exactly language.</returns>
        public static string TranslateErrorMsg(string errorCode = ErrorCode.ServerError)
        {
            switch (errorCode)
            {
                case ErrorCode.InvalidRequest:
                    return Lang.lblInvalidRequest;

                case ErrorCode.UnauthorizedClient:
                    return Lang.lblUnauthorizedClient;

                case ErrorCode.AccessDenied:
                    return Lang.lblAccessDenied;

                case ErrorCode.UnsupportedResponseType:
                    return Lang.lblUnsupportedResponseType;

                case ErrorCode.ServerError:
                    return Lang.lblServerError;

                case ErrorCode.ComingSoon:
                    return Lang.comingsoon;

                case ErrorCode.TemporarilyUnavailable:
                default:
                    return Lang.lblTemporarilyUnavailable;
            }
        }

        /// <summary>
        /// Authentication model.
        /// </summary>
        /// <param name="authorizationCode">Authorization code returned from authorization server.</param>
        /// <returns>Return a model that authenticated.</returns>
        public AuthModel GetAuthInfo(string authorizationCode)
        {
            if (string.IsNullOrWhiteSpace(authorizationCode))
            {
                throw new ArgumentNullException("authorizationCode");
            }

            var authModel = new AuthModel(GetSessionToken(authorizationCode));

            return authModel;
        }

        /// <summary>
        ///  Convert language code returned from authentication service to Apollo agent language code.
        ///  <see cref="OAuthController" >Default is Chinese traditional.</see>
        ///  <seealso cref="member">text</seealso>
        /// </summary>
        /// <param name="sungameLanguage">Input a sun game language.</param>
        /// <returns>Language code.</returns>
        public string ConvertToApolloLanguage(string sungameLanguage)
        {
            switch (sungameLanguage)
            {
                case "zh_TW":
                    return "zh-TW";
                case "ja_JP":
                    return "zh-CN";
                case "th_TH":
                    return "zh-CN";
                case "vm_VM":
                    return "zh-CN";
                case "en_US":
                    return "en-US";
                case "zh_GB":
                default:
                    return "zh-CN";
            }
        }

        /// <summary>
        /// Resource server contact with authorization server to get access token
        /// and authentication info.
        /// </summary>
        /// <param name="authenticationCode">Code returned from TGP Service.</param>
        /// <returns><see cref="JsonResponseModel"/></returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        internal JsonResponseModel GetSessionToken(string authenticationCode)
        {
            var jsonResponseModel = new JsonResponseModel();

            // Compose post data to make a web request to authorization server.
            var postData = string.Format(
                FormParams,
                authenticationCode,
                OAuthSettings.Settings.RedirectUri,
                OAuthSettings.Settings.AppId,
                OAuthSettings.Settings.AppSecret);

            byte[] requestData = new ASCIIEncoding().GetBytes(postData);
            var request = (HttpWebRequest)WebRequest.Create(OAuthSettings.Settings.TokenUrl);

            request.KeepAlive = false;
            request.Method = WebRequestMethods.Http.Post; // It is required.
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = requestData.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(requestData, 0, requestData.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                // read response stream and parse to JsonResponseModel
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
                {
                    string jsonString = reader.ReadToEnd();
                    jsonResponseModel = Json.Decode<JsonResponseModel>(jsonString);
                }

                response.Close();
            }

            return jsonResponseModel;
        }
    }
}