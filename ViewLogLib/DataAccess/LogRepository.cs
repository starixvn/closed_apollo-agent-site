using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Sunplus.Shared.ViewLogLib.Entities;
using Sunplus.Agent.Configurations;
using Fanex.Data.Configuration;

namespace Sunplus.Shared.ViewLogLib.DataAccess
{
    public class LogRepository<T> : RepositoryBase, ILogRepository<T> where T : LogBase
    {
        #region ILogRepository<T> Members

        public List<T> GetPaged(IRequestViewLogContext requestContext
            , int pageIndex, int pageSize, out int totalRecords)
        {
            var entityList = new List<T>();
            totalRecords = 0;

            if (entityList is List<LogRunSp>)
            {
                entityList = GetLogRunSp(requestContext, pageIndex, pageSize) as List<T>;
                totalRecords = GetRunSpTotalRecords(requestContext);
            }
            else if (entityList is List<LogCredit>)
            {
                entityList = GetLogCredit(requestContext, pageIndex, pageSize) as List<T>;
                totalRecords = GetLogCreditTotalRecords(requestContext);
            }
            else if (entityList is List<LogLogin>)
            {
                entityList = GetLogLogin(requestContext, pageIndex, pageSize) as List<T>;
                totalRecords = GetLogLoginTotalRecords(requestContext);
            }

            return entityList;
        }

        #endregion

        #region Log RunSp
        private List<LogRunSp> GetLogRunSp(IRequestViewLogContext context
            , int pageIndex, int pageSize)
        {
            var entityList = new List<LogRunSp>();

            Database db = Databsse;

            var dbSetting = DbSettingProviderManager.Current.GetSetting("Acc_UserLog_RunSPSel");
            string realSpName = dbSetting.SpName;

            using (var dbCommand = db.GetStoredProcCommand(realSpName))
            {
                db.AddInParameter(dbCommand, "CustId", DbType.Int32, context.CustomerId);
                db.AddInParameter(dbCommand, "CustName", DbType.String, context.CustomerName);
                db.AddInParameter(dbCommand, "ActionTypeId", DbType.Int32, context.ActionTypeId);
                db.AddInParameter(dbCommand, "FromDate", DbType.DateTime, context.FromDate);
                db.AddInParameter(dbCommand, "ToDate", DbType.DateTime, context.ToDate);
                db.AddInParameter(dbCommand, "LowerActionId", DbType.Int32, context.LowerActionId);
                db.AddInParameter(dbCommand, "UpperActionId", DbType.Int32, context.UpperActionId);
                db.AddInParameter(dbCommand, "IsInternal", DbType.Byte, context.IsInternal);
                db.AddInParameter(dbCommand, "PageIndex ", DbType.Int32, pageIndex);
                db.AddInParameter(dbCommand, "PageSize ", DbType.Int32, pageSize);

                using (IDataReader reader = db.ExecuteReader(dbCommand))
                {
                    if (reader != null)
                    {

                        bool hasTrackingValueOld = false;
                        try
                        {
                            hasTrackingValueOld = reader.GetOrdinal("TrackingValueOld") != -1;
                        }
                        catch
                        {

                        }


                        while (reader.Read())
                        {
                            var entity = new LogRunSp
                             {
                                 LogID = (long)reader["LogID"],
                                 UserID = Convert.ToInt32(reader["UserID"]),
                                 UserName = reader["Username"] as string,
                                 TrackingValueOld = !hasTrackingValueOld ? string.Empty : reader["TrackingValueOld"] as string,
                                 TrackingValue = reader["TrackingValue"] as string,
                                 LogDate = (DateTime)(reader["LogDate"]),
                                 ActionID = Convert.ToInt32(reader["ActionID"]),
                                 ActionTypeID = Convert.IsDBNull(reader["ActionTypeID"]) ? 0 : Convert.ToInt32(reader["ActionTypeID"]),
                                 SPName = reader["SPName"] as string,
                                 IsInternal = (byte)reader["IsInternal"],
                                 CustID = Convert.ToInt32(reader["CustID"]),
                                 CustName = reader["CustName"] as string,
                                 ActionName = reader["ActionName"] as string
                             };

                            entityList.Add(entity);
                        }
                    }
                }
            }

            return entityList;
        }

        private int GetRunSpTotalRecords(IRequestViewLogContext context)
        {
            int totalRecord;
            Database db = Databsse;

            var dbSetting = DbSettingProviderManager.Current.GetSetting("Acc_UserLog_RunSPSelTotalRecords");
            string realSpName = dbSetting.SpName;

            using (var dbCommand = db.GetStoredProcCommand(realSpName))
            {
                db.AddInParameter(dbCommand, "CustId", DbType.Int32, context.CustomerId);
                db.AddInParameter(dbCommand, "ActionTypeId", DbType.Int32, context.ActionTypeId);
                db.AddInParameter(dbCommand, "FromDate", DbType.DateTime, context.FromDate);
                db.AddInParameter(dbCommand, "ToDate", DbType.DateTime, context.ToDate);
                db.AddInParameter(dbCommand, "LowerActionId", DbType.Int32, context.LowerActionId);
                db.AddInParameter(dbCommand, "UpperActionId", DbType.Int32, context.UpperActionId);
                db.AddInParameter(dbCommand, "IsInternal", DbType.Byte, context.IsInternal);

                totalRecord = Convert.ToInt32(db.ExecuteScalar(dbCommand));
            }

            return totalRecord;
        }
        #endregion

        #region LogCredit

        private string DecFormat(object value, byte decDigits)
        {
            if (Convert.IsDBNull(value)) return null;

            decimal val = Convert.ToDecimal(value);
            return val.ToString("N" + decDigits);
        }

        private List<LogCredit> GetLogCredit(IRequestViewLogContext context
            , int pageIndex, int pageSize)
        {
            var entityList = new List<LogCredit>();

            Database db = Databsse;


            var dbSetting = DbSettingProviderManager.Current.GetSetting("Acc_UserLog_CreditSel");
            string realSpName = dbSetting.SpName;

            using (var dbCommand = db.GetStoredProcCommand(realSpName))
            {
                db.AddInParameter(dbCommand, "CustID", DbType.Int32, context.CustomerId);
                db.AddInParameter(dbCommand, "CustName", DbType.String, context.CustomerName);
                db.AddInParameter(dbCommand, "FromDate", DbType.DateTime, context.FromDate);
                db.AddInParameter(dbCommand, "ToDate", DbType.DateTime, context.ToDate);
                db.AddInParameter(dbCommand, "IsInternal", DbType.Byte, context.IsInternal);
                db.AddInParameter(dbCommand, "PageIndex ", DbType.Int32, pageIndex);
                db.AddInParameter(dbCommand, "PageSize ", DbType.Int32, pageSize);

                using (IDataReader reader = db.ExecuteReader(dbCommand))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var entity = new LogCredit
                             {
                                 LogID = (long)reader["LogID"],
                                 UserID = (int)reader["UserID"],
                                 CustID = (int)reader["CustID"],
                                 LogDate = (DateTime)(reader["LogDate"]),
                                 OldValue = DecFormat(reader["OldValue"], 2),
                                 NewValue = DecFormat(reader["NewValue"], 2),
                                 IP = reader["IP"] as string,
                                 Doing = reader["Doing"] as string,
                                 ActionID = Convert.ToInt32(reader["ActionID"]),
                                 IsInternal = (byte)reader["IsInternal"],
                                 ActionTypeID = Convert.IsDBNull(reader["ActionTypeID"]) ? 0 : Convert.ToInt32(reader["ActionTypeID"]),
                                 SPName = reader["SPName"] as string,
                                 UserName = reader["Username"] as string,
                                 CustName = reader["ActionName"] as string
                             };

                            entityList.Add(entity);
                        }
                    }
                }
            }

            return entityList;
        }

        private int GetLogCreditTotalRecords(IRequestViewLogContext context)
        {
            int totalRecord;
            Database db = Databsse;

            var dbSetting = DbSettingProviderManager.Current.GetSetting("Acc_UserLog_CreditSelTotalRecords");

            string realSpName = dbSetting.SpName;

            using (var dbCommand = db.GetStoredProcCommand(realSpName))
            {
                db.AddInParameter(dbCommand, "CustID", DbType.Int32, context.CustomerId);
                db.AddInParameter(dbCommand, "FromDate", DbType.DateTime, context.FromDate);
                db.AddInParameter(dbCommand, "ToDate", DbType.DateTime, context.ToDate);
                db.AddInParameter(dbCommand, "IsInternal", DbType.Byte, context.IsInternal);

                totalRecord = Convert.ToInt32(db.ExecuteScalar(dbCommand));
            }

            return totalRecord;
        }
        #endregion

        #region Log Login
        private List<LogLogin> GetLogLogin(IRequestViewLogContext context
            , int pageIndex, int pageSize)
        {
            var entityList = new List<LogLogin>();

            Database db = Databsse;

            var dbSetting = DbSettingProviderManager.Current.GetSetting("Acc_UserLog_LoginSel");
            string realSpName = dbSetting.SpName;

            using (var dbCommand = db.GetStoredProcCommand(realSpName))
            {
                db.AddInParameter(dbCommand, "UserId", DbType.Int32, context.CustomerId);
                db.AddInParameter(dbCommand, "UserName", DbType.String, context.CustomerName);
                db.AddInParameter(dbCommand, "FromDate", DbType.DateTime, context.FromDate);
                db.AddInParameter(dbCommand, "ToDate", DbType.DateTime, context.ToDate);
                db.AddInParameter(dbCommand, "IsAdminAccess", DbType.Byte, context.IsInternal);
                db.AddInParameter(dbCommand, "PageIndex ", DbType.Int32, pageIndex);
                db.AddInParameter(dbCommand, "PageSize ", DbType.Int32, pageSize);

                using (IDataReader reader = db.ExecuteReader(dbCommand))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var entity = new LogLogin
                            {
                                LogID = (long)reader["LogID"],
                                UserID = (int)reader["UserID"],
                                UserName = reader["Username"] as string,
                                Doing = reader["Doing"] as string,
                                LogDate = (DateTime)(reader["LogDate"]),
                                ActionID = (int)reader["ActionID"],
                                IsInternal = (byte)reader["IsInternal"],
                                ActionName = reader["ActionName"] as string,
                                SPName = reader["SPName"] as string,
                                IP = reader["IP"] as string,
                                ServerId = (int)reader["ServerId"],
                                Browser = reader["Browser"] as string,
                                Domain = reader["Domain"] as string,
                                Platform = reader["Platform"] as string,
                                CountryCode = reader["CountryCode"] as string
                            };

                            entityList.Add(entity);
                        }
                    }
                }
            }

            return entityList;
        }

        private int GetLogLoginTotalRecords(IRequestViewLogContext context)
        {
            int totalRecord;
            Database db = Databsse;

            var dbSetting = DbSettingProviderManager.Current.GetSetting("Acc_UserLog_LoginSelTotalRecords");
            string realSpName = dbSetting.SpName;

            using (var dbCommand = db.GetStoredProcCommand(realSpName))
            {
                db.AddInParameter(dbCommand, "UserId", DbType.Int32, context.CustomerId);
                db.AddInParameter(dbCommand, "FromDate", DbType.DateTime, context.FromDate);
                db.AddInParameter(dbCommand, "ToDate", DbType.DateTime, context.ToDate);
                db.AddInParameter(dbCommand, "IsAdminAccess", DbType.Byte, context.IsInternal);

                totalRecord = Convert.ToInt32(db.ExecuteScalar(dbCommand));
            }

            return totalRecord;
        }
        #endregion
    }
}
