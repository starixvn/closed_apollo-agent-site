namespace Apollo.Age.Reports.Services
{
    using System.Collections.Generic;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.ViewModels;
    using Fanex.NAP.Framework.Data;
    using NPOI.HSSF.UserModel;

    public interface ICreditReportService
    {
        List<TransactionCreditModel> CreditReportTransactions(ICriteria criteria);

        HSSFWorkbook ExportCreditReport(CreditReportViewModel model, IEnumerable<TransactionCreditModel> listCreditTrans);
    }
}
