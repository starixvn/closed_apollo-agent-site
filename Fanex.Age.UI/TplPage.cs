/*
 * Created 20091211@Lee - Base aspx page, use Template method pattern http://sourcemaking.com/design_patterns/template_method
 * Revision: 
 * 20091211@Lee - Initializing
 * 
 * !!!CAUTION: ANY REVISION MUST BE APPROVED BEFORE PROCEEDING!!!
 * 
 */

namespace Fanex.Age.UI
{
    using System.Collections;
    using System.Web.UI;

    /// <summary>
    /// Base IBC page. All aspx pages should be inherited from TplPage or TplPage2.
    /// </summary>
    public abstract class TplPage : TplBase
    {
        /// <summary>
        /// Root namespace of JSON data returned.
        /// </summary>
        private const string JS_DATA = "_page";

        /// <summary>
        /// List of values will be rendered to browser as JSON format.
        /// </summary>
        protected JSONData _JsData = new JSONData(10);

        /// <summary>
        /// Register an object as JSON format. Access object in client site by _page.objName.
        /// </summary>
        /// <param name="name">Name of the object.</param>
        /// <param name="value">Value of the object. It could be a JSONData object.</param>
        public void RegisterJavascriptObject(object name, object value)
        {
            _JsData.Add(name, value);
        }

        /// <summary>
        /// Register an error as JSON format.
        /// </summary>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        public void RegisterJSONError(int errCode, string errMsg)
        {
            _JsData.Add(JSONError.ERROR_CODE, errCode);
            _JsData.Add(JSONError.ERROR_MSG, errMsg);
        }

        /// <summary>
        /// Register an error as JSON format.
        /// </summary>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        public void RegisterJSONError(ErrorType type, string errMsg)
        {
            _JsData.Add(JSONError.ERROR_CODE, (int)(type));
            _JsData.Add(JSONError.ERROR_MSG, errMsg);
        }

        /// <summary>
        /// Render content section of html page.
        /// </summary>
        /// <param name="writer"></param>
        public abstract void RenderContent(HtmlTextWriter writer);

        /// <summary>
        /// Don't override this method in instance class.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            // Hot fix for Opera browser (IIS does sniffing user agent)
            Response.ContentType = "text/html";

            _Parameters = new Hashtable();

            ProcessRequest(ref _Parameters);

            object error = ValidateRequest(Parameters);

            if (null != error)
            {
                writer.Write(error);
            }
            else
            {
                if (!IsPostBack)
                {
                    _View = InvokeData(Parameters);
                }

                RenderContent(writer);
            }

            if (_JsData.Count > 0)
            {
                // Render javascript variables
                WriteJavascriptObjects(writer);
            }
        }

        protected void WriteJavascriptObjects(HtmlTextWriter writer)
        {
            writer.Write("<script type=\"text/javascript\">var ");
            writer.Write(JS_DATA);
            writer.Write(" = ");
            writer.Write(JSON.Render(_JsData));
            writer.Write(";</script>");
        }
    }
}
