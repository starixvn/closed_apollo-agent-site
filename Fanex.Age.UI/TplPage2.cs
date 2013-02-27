using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Collections;

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
    /// <summary>
    /// Use this class only when need to handle post back event.
    /// </summary>
    public abstract class TplPage2 : Fanex.Age.UI.TplPage
    {
         /// <summary>
        /// New IsPostBack.
        /// </summary>
        public new bool IsPostBack
        {
            get
            {
                return Request.Form.Count > 0;
            }
        }

        /// <summary>
        /// Processing post back event. It was called before any routine. Do process it and redirect to dedicated page.
        /// </summary>
        /// <param name="request"></param>
        public abstract void OnPostBack();

        /// <summary>
        /// Don't override this method in instance class.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (IsPostBack)
            {
                OnPostBack();
            }

            _Parameters = new Hashtable();

            ProcessRequest(ref _Parameters);

            object error = ValidateRequest(Parameters);

            // Remove end html tag at the end and render later to fix bug xml is not welformed in Opera browser
            TplHtmlTextWriter writer2 = new TplHtmlTextWriter(writer);
            bool removedEndHtmlTag = false;

            if (null != error)
            {
                writer2.Write(error);
            }
            else
            {
                _View = InvokeData(Parameters);

                RenderContent(writer2);
                removedEndHtmlTag = writer2.RemovedEndHtmlTag;
            }

            if (_JsData.Count > 0)
            {
                // Render javascript variables
                WriteJavascriptObjects(writer2);
            }

            if (removedEndHtmlTag)
            {
                // Render end html tag
                writer2.Write(TplHtmlTextWriter.END_HTML_TAG);
            }
        }
    }
}
