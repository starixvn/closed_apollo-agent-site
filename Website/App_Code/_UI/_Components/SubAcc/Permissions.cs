/*
 * Created 20111228@Andy - Defines permissions for sub-account
 * Revision:
 * ?@? - ...
 *
 */

using System.Collections.Generic;
using Resources;
using Sunplus.Agent.Model.Entities;

namespace Sunplus.Agent.UI._SubAcc
{
    /// <summary>
    /// Defines permissions for sub-account
    /// </summary>
    public class Permissions
    {
        public Permissions()
        {
        }

        //public static IList<IExtObject<string, string>> GetPermissions()
        //{
        //    // If one more permission is added, only add one item to perm
        //    var perm = new List<IExtObject<string, string>>()
        //    {
        //        new ExtObject<string, string>(Permissions.TotalBets, Lang.totalbets + "/" + Lang.lblForecast),
        //        new ExtObject<string, string>(Permissions.Reports, Lang.reports),
        //        new ExtObject<string, string>(Permissions.CreateAccount, Lang.createaccount),
        //        new ExtObject<string, string>(Permissions.UpdateAccount, Lang.updateaccount),
        //        new ExtObject<string, string>(Permissions.ViewMemberInfo, Lang.view),
        //        new ExtObject<string, string>(Permissions.Transfer, Lang.transfer),
        //        new ExtObject<string, string>(Permissions.ViewLog, Lang.viewlog)
        //    };

        //    return perm;
        //}

        //public static IList<IExtObject<string, bool>> GetPageAccess(PageAccess access)
        //{
        //    var pageAccess = new List<IExtObject<string, bool>>()
        //    {
        //        new ExtObject<string, bool>(PageAccessType.ViewTotalBets, access.ViewTotalBets),
        //        new ExtObject<string, bool>(PageAccessType.ViewReports, access.ViewReports),
        //        new ExtObject<string, bool>(PageAccessType.CreateAccount, access.CreateAccount),
        //        new ExtObject<string, bool>(PageAccessType.UpdateAccount, access.UpdateAccount),
        //        new ExtObject<string, bool>(PageAccessType.ViewMemberInfo, access.ViewMemberInfo),
        //        new ExtObject<string, bool>(PageAccessType.Transfer, access.Transfer),
        //        new ExtObject<string, bool>(PageAccessType.ViewLog, access.ViewLog)
        //    };

        //    return pageAccess;
        //}

        //public static IList<IExtObject<string, string>> GetPermissionsDescription()
        //{
        //    var permissions = new List<IExtObject<string, string>>()
        //    {
        //        new ExtObject<string, string>(PageAccessType.ViewTotalBets, Lang.totalbets + "/" + Lang.lblForecast),
        //        new ExtObject<string, string>(PageAccessType.ViewReports, Lang.reports),
        //        new ExtObject<string, string>(PageAccessType.CreateAccount, Lang.memberinfo + ": " + Lang.FullControl),
        //        new ExtObject<string, string>(PageAccessType.UpdateAccount, Lang.memberinfo + ": " + Lang.update),
        //        new ExtObject<string, string>(PageAccessType.ViewMemberInfo, Lang.memberinfo + ": " + Lang.view),
        //        new ExtObject<string, string>(PageAccessType.Transfer, Lang.transfer),
        //        new ExtObject<string, string>(PageAccessType.ViewLog, Lang.viewlog)
        //    };

        //    return permissions;
        //}

        //public const string TotalBets = "chkTotalBets";
        //public const string Reports = "chkReports";
        //public const string CreateAccount = "chkCreateAcc";
        //public const string UpdateAccount = "chkUpdateAcc";
        //public const string Transfer = "chkTransfer";
        //public const string ViewLog = "chkViewLog";
        //public const string ViewMemberInfo = "chkMemberInfo";
    }
}