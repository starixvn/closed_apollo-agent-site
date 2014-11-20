namespace Apollo.Age.Reports.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Apollo.Age.Configurations;
    using Apollo.Age.Core;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.Core;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Web.Attribute;

    /// <summary>
    /// Reports Controller.
    /// </summary>
    //[SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
    public partial class ReportsController : BaseController
    {
        /// <summary>
        /// Credits the report.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ViewResult CreditReport(CreditReportViewModel model)
        {
            // Get user permission
            var userPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);
            var hasPermission = userPermission[ApolloFunctionKeys.CreditReport] >= (byte)Policy.Query;
            ViewBag.allFunctionPermission = userPermission;

            if (string.IsNullOrEmpty(model.Username))
            {
                return View(new CreditReportViewModel
                {
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now,
                    Username = string.Empty,
                    IsAgent = true,
                    TransactionList = new List<TransactionCreditModel>()
                });
            }

            //if (!hasPermission || (agentId != null && !hasDownlineDetailsPermission))
            if (!hasPermission)
            {
                return View("ReportAccessDenied");
            }

            model.TransactionList = SearchTransaction(model);

            var sessionKey = string.Format(
                model.IsAgent ? Constants.SessionKeyAgentCreditReport : Constants.SessionKeyMemberCreditReport,
                this.CurrentSession.UserId);

            HttpContext.Session[sessionKey] = model.TransactionList;

            return this.View(model);
        }

        /// <summary>
        /// Exports the credit report.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ActionResult ExportCreditReport(CreditReportViewModel model)
        {
            // Get user permission.
            var userPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);

            var hasPermission = userPermission[ApolloFunctionKeys.CreditReport] >= (byte)Policy.Query;
            // var hasDownlineDetailsPermission = userPermission[ApolloFunctionKeys.DownlineDetails] >= (byte)Policy.Query;

            ViewBag.allFunctionPermission = userPermission;

            if (!hasPermission)
            {
                return View("ReportAccessDenied");
            }

            var sessionKey = string.Format(
                model.IsAgent ? Constants.SessionKeyAgentCreditReport : Constants.SessionKeyMemberCreditReport,
                this.CurrentSession.UserId);

            var listCreditTrans = (List<TransactionCreditModel>)HttpContext.Session[sessionKey];

            if (listCreditTrans == null)
            {
                listCreditTrans = SearchTransaction(model);
            }

            var excelResult = this._creditReportService.ExportCreditReport(model, listCreditTrans);

            return ExcelResult(excelResult, string.Format("CreditReport_{0}_{1}_{2}", model.Username, this._creditReportService.ShortDateFormat(model.FromDate), this._creditReportService.ShortDateFormat(model.ToDate)));
        }

        /// <summary>
        /// Searches the transaction.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        private List<TransactionCreditModel> SearchTransaction(CreditReportViewModel model)
        {
            var custId = _userService.GetUserIdByUsername(new Age.Criterias.UserIdByUsernameCriteria
            {
                IsAgent = model.IsAgent,
                LoggedId = CurrentSession.AgentId,
                Username = model.Username
            });

            return _creditReportService.CreditReportTransactions(new CreditReportCriteria
            {
                AgentId = model.IsAgent ? custId : -1,
                BalanceUpDay = _creditReportService.GetBalanceUpDay(),
                CustId = model.IsAgent ? -1 : custId,
                FromDate = model.FromDate,
                ToDate = model.ToDate
            });
        }
    }
}