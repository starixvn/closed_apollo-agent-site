namespace Fanex.Age.UI.Admin
{
    using System.Collections;
    using System.Data;
    using System.IO;
    using System.Resources;
    using Fanex.Age.UI;
    using Sunplus.Agent.Model.BL.Admin;

    public abstract class AdminBasePage : TplPage
    {
        #region Const

        protected const string EMPTY_STR = "";
        protected const string EVEN_ROW_CLASS = "even";
        protected const string ODD_ROW_CLASS = "odd";
        protected const string EXPORT_EXCEL = "ExportExcel.x";

        #endregion

        protected string _fileName = EMPTY_STR;
        protected bool _exportToExcel = false;

        private AdminBO _adminBO;
        protected AdminBO AdminBO
        {
            get
            {
                if (_adminBO == null)
                {
                    _adminBO = new AdminBO(CurrentUserSession.NetSessionId);
                }

                return _adminBO;
            }
        }

        public override void ProcessRequest(ref Hashtable p)
        {
            _exportToExcel = Request[EXPORT_EXCEL] != null;
        }

        public override object ValidateRequest(Hashtable p)
        {
            return null;
        }

        protected void ExportToExcel(MemoryStream excelStream, string fileName)
        {
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
            Response.Clear();
            Response.BinaryWrite(excelStream.GetBuffer());
            Response.End();
        }

        #region Helpers
        
        protected DataTable GetInvokedData()
        {
            return View as DataTable;
        }

        private ResourceManager _resourceManager = null;
        static object locker = new object();
        protected ResourceManager GetLangResourceManager()
        {
            if (_resourceManager == null)
            {
                lock (locker)
                {
                    if (_resourceManager == null)
                    {
                        _resourceManager = new ResourceManager(typeof(Resources.Lang));
                        _resourceManager.IgnoreCase = true;
                    }
                }
            }

            return _resourceManager;
        }

        #endregion
    }
}