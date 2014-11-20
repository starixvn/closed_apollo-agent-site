namespace Apollo.Age.Reports.Services
{
    using System.Collections.Generic;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.ViewModels;
    using NPOI.HSSF.UserModel;

    public interface ICreditReportService
    {
        List<TransactionCreditModel> CreditReportTransactions(CreditReportCriteria criteria);

        HSSFWorkbook ExportCreditReport(CreditReportViewModel model, IEnumerable<TransactionCreditModel> listCreditTrans);
    }
}
