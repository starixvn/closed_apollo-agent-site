using System;
using System.Web;
using System.Xml.Xsl;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

using Fanex.Caching;
using Sunplus.Shared.ViewLogLib;

using Sunplus.Shared.ViewLogLib.Services;
using Sunplus.Shared.ViewLogLib.Entities;
using Sunplus.Agent.UI._SubAcc;
using Sunplus.Agent.Model.Entities;
using Resources;

namespace Sunplus.Agent.UI._ViewLog
{
    public partial class LogCustomerSetting_Page : ViewLogPageBase
    {
        private const string XsltPath = "~/_ViewLog/Resources/XsltTemplate";

        protected override void PageLoadEvent(object sender, EventArgs e)
        {

            #region Check ViewLogDebug key to clear cache xslt transform object
            if (ConfigurationManager.AppSettings["ViewLogDebug"] != null
                && ConfigurationManager.AppSettings["ViewLogDebug"].ToString() == "1")
            {
                ICache cache = CacheService.CreateInstance(CacheType.MemoryCache);
                cache.Remove(Constants.CacheKeyTransformObject);
            }
            #endregion

            DataControl = rptDataLog;
            NoInfoLabel = ltrNoInfo;
            CurrentUrl = "LogCustomerSetting.aspx";
        }

        protected override IList GetLogData(out int totalPage)
        {
            totalPage = 0;

            IRequestViewLogContext requestViewLogContext = this.RequestViewLogContext;
            List<LogRunSp> entityList = null;

            if (requestViewLogContext != null)
            {                
                var permissions = Permissions.GetPermissionsDescription();
                var stakes = new Dictionary<string, string>();

                stakes.Add("stakelevel1", Resources.Lang.playForFun);
                stakes.Add("stakelevel2", Resources.Lang.low);
                stakes.Add("stakelevel3", Resources.Lang.medium);
                stakes.Add("stakelevel4", Resources.Lang.high);
                stakes.Add("stakelevel5", Resources.Lang.vip);
             
                requestViewLogContext.ActionTypeId = LogActionTypeId;
                entityList = (new LogService<LogRunSp>()).GetPaged(requestViewLogContext, this.PageIndex, this.PageSize, out totalPage, permissions, stakes);
            }

            return entityList;
        }

        private int LogActionTypeId
        {
            get
            {
                int actionType = -1;
                if (!Int32.TryParse(GetQueryStringValue("actiontype", "-1"), out actionType)) actionType = -1;
                return actionType;
            }
        }

        /// <summary>
        /// Transform xml to html using xslt
        /// </summary>
        /// <param name="xml">xml content</param>
        /// <param name="actionId">actionid to get approriated xslt file</param>
        /// <returns>html string</returns>
        protected string GetContentHtml(object xml, object actionId, object logId)
        {
            string html = string.Empty;

            XsltLogFormat xslLogFormat = new XsltLogFormat();
            XsltArgumentList args = new XsltArgumentList();
            args.AddExtensionObject("urn:LogFormat", xslLogFormat);

            html = string.IsNullOrEmpty(xml as string) 
                ? 
                string.Empty 
                : 
                Utility.GetHtmlContent(xml.ToString(), Convert.ToInt32(actionId), XsltPath, args);

            return (HttpUtility.HtmlDecode(html));
        }
    }
}
