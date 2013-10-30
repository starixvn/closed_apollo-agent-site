/*
 * Created 20091211@Lee - Base ashx page, use Template method pattern http://sourcemaking.com/design_patterns/template_method
 * Revision: 
 * 20091211@Lee - Initializing
 * 20100227@Lee - Update security: validating http input
 * 
 * !!!CAUTION: ANY REVISION MUST BE APPROVED BEFORE PROCEEDING!!!
 * 
 */

namespace Fanex.Age.UI
{
    using System.Collections;
    using System.Web;
    using System.Web.SessionState;

    /// <summary>
    /// Implement IHttpHandler, process request and return in JSON format.
    /// All ashx must be inherited form this handler.
    /// </summary>
    public abstract class TplHandler : TplBase, IHttpHandler, IRequiresSessionState
    {
        private HttpContext _Context;

        public new bool IsReusable
        {
            get { return true; }
        }

        public new HttpResponse Response
        {
            get 
            {
                return _Context.Response;
            }
        }

        public new HttpRequest Request
        {
            get 
            {
                return _Context.Request;
            }
        }

        protected bool IsValidateRequest = true; 

        public override void ProcessRequest(HttpContext context)
        {
            // ContentType always be json
            context.Response.ContentType = "text/plain";

            _Context = context;
            // Check potentially dangerous request
            try
            {
                if (IsValidateRequest) context.Request.ValidateInput();

                // Process requested param
                _Parameters = new Hashtable();
                ProcessRequest(ref _Parameters);
            }
            catch (HttpRequestValidationException e)
            {
                Result error = new Result(e.ErrorCode, "Invalid parameter value.");
                _Result.AsResult(error);
                Response.Write(JSON.Render(_Result));
                return;
            }

            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

           // SetLanguage(CurrentUserSession.Language);

            // Validate request
            object obj = ValidateRequest(Parameters);

            // Return error
            if (null != obj)
            {
                Result result = obj as Result;
                if (null == result)
                {
                    result = new Result(ErrorType.Unknown, obj as string);
                }

                _Result.AsResult(result as Result);
            }
            else
            {
                // Doing data5m5
                _View = InvokeData(Parameters);

                // Render result
                RenderResult(ref _Result);
            }

            Response.Write(JSON.Render(_Result));

            // End
        }

        /// <summary>
        /// List of returning results.
        /// </summary>
        private JSONData _Result = new JSONData(10);

        /// <summary>
        /// Process and return a list of result.
        /// </summary>
        /// <returns></returns>
        public abstract void RenderResult(ref JSONData data);
    }
}