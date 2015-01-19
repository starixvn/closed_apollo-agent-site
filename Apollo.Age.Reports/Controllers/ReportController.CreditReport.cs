namespace Apollo.Age.Reports.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Apollo.Age.Core;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Web;

    /// <summary>
    /// Reports Controller.
    /// </summary>
    public partial class ReportsController : BaseController
    {
        /// <summary>
        /// Credits the report.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>View Result.</returns>
        public ViewResult CreditReport(CreditReportViewModel model)
        {
            // Get user permission
            var userPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);
            var hasPermission = userPermission[ApolloFunctionKeys.CreditReport] >= (byte)Policy.Query;
            model.UserPermission = userPermission;

            if (!hasPermission)
            {
                return View("ReportAccessDenied");
            }

            if (string.IsNullOrEmpty(model.Username))
            {
                return View(new CreditReportViewModel
                {
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now,
                    Username = string.Empty,
                    IsAgent = true,
                    TransactionList = new List<TransactionCreditModel>(),
                    UserPermission = userPermission
                });
            }

            model.TransactionList = SearchTransaction(model);

            return this.View(model);
        }

        /// <summary>
        /// Exports the credit report.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Action Result.</returns>
        public ActionResult ExportCreditReport(CreditReportViewModel model)
        {
            // Get user permission.
            var userPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);
            var hasPermission = userPermission[ApolloFunctionKeys.CreditReport] >= (byte)Policy.Query;

            model.UserPermission = userPermission;

            if (!hasPermission)
            {
                return View("ReportAccessDenied");
            }

            var listCreditTrans = SearchTransaction(model);

            var excelResult = this._creditReportService.ExportCreditReport(model, listCreditTrans);
            var fileName = string.Format(
                "CreditReport_{0}_{1}_{2}",
                model.Username,
                _creditReportService.ShortDateFormat(model.FromDate),
                _creditReportService.ShortDateFormat(model.ToDate));
            return ExcelResult(excelResult, fileName);
        }

        /// <summary>
        /// Searches the transaction.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>List Transactions.</returns>
        private List<TransactionCreditModel> SearchTransaction(CreditReportViewModel model)
        {
            var custId = _userService.GetUserIdByUsername(new Age.Criterias.UserIdByUsernameCriteria
            {
                IsAgent = model.IsAgent,
                LoggedId = CurrentSession.AgentId,
                Username = model.Username
            });

            var listTrans = _creditReportService.CreditReportTransactions(new CreditReportCriteria
            {
                AgentId = model.IsAgent ? custId : -1,
                BalanceUpDay = _creditReportService.GetBalanceUpDay(),
                CustId = model.IsAgent ? -1 : custId,
                FromDate = model.FromDate,
                ToDate = model.ToDate
            });

            if (model.FromDate < _creditReportService.GetBalanceUpDay())
            {
                var listOldTrans = _creditReportService.CreditReportTransactions(new CreditReportOldCriteria
                {
                    AgentId = model.IsAgent ? custId : -1,
                    BalanceUpDay = _creditReportService.GetBalanceUpDay(),
                    CustId = model.IsAgent ? -1 : custId,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate
                });

                listTrans = listTrans.Union(listOldTrans).ToList();
            }

            return listTrans;
        }
    }
}