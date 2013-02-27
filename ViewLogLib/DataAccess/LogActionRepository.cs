using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Sunplus.Shared.ViewLogLib.Entities;

namespace Sunplus.Shared.ViewLogLib.DataAccess
{
    public class LogActionRepository : RepositoryBase, ILogActionRepository
    {
        #region IRepository<LogAction> Members

        public IEnumerable<LogAction> GetAll()
        {
            var entityList = new List<LogAction>();
            Database db = base.Databsse;
            using (var dbCommand = db.GetStoredProcCommand("Acc_UserLog_LogActionSel"))
            {
                #region Mapping data from reader
                using (var reader = db.ExecuteReader(dbCommand))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            #region Mapping
                            var entity
                                = new LogAction
                                {
                                    ActionID = reader.GetInt32(reader.GetOrdinal("ActionID")),
                                    ActionTypeID =
                                        (reader.IsDBNull(reader.GetOrdinal("ActionTypeID")))
                                            ? 0
                                            : reader.GetInt32(reader.GetOrdinal("ActionTypeID")),
                                    ActionNameEN =
                                        (reader.IsDBNull(reader.GetOrdinal("ActionNameEN")))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("ActionNameEN")),
                                    ActionNameCH =
                                        (reader.IsDBNull(reader.GetOrdinal("ActionNameCH")))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("ActionNameCH")),
                                    ActionNameCS =
                                        (reader.IsDBNull(reader.GetOrdinal("ActionNameCS")))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("ActionNameCS")),
                                    ActionNameJP =
                                        (reader.IsDBNull(reader.GetOrdinal("ActionNameJP")))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("ActionNameJP")),
                                    ActionNameIT =
                                        (reader.IsDBNull(reader.GetOrdinal("ActionNameIT")))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("ActionNameIT")),
                                    ActionNameTH =
                                        (reader.IsDBNull(reader.GetOrdinal("ActionNameTH")))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("ActionNameTH")),
                                    ActionNameKO =
                                        (reader.IsDBNull(reader.GetOrdinal("ActionNameKO")))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("ActionNameKO")),
                                    IsInternal =
                                        (reader.IsDBNull(reader.GetOrdinal("IsInternal")))
                                            ? false
                                            : reader.GetByte(reader.GetOrdinal("IsInternal")) == 1,
                                    SPName =
                                        (reader.IsDBNull(reader.GetOrdinal("SPName")))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("SPName"))
                                };
                            #endregion
                            entityList.Add(entity);
                        }
                    }
                }

                #endregion
            }

            return entityList;
        }

        public bool Update(LogAction entity)
        {
            bool isUpdateSuccessful;
            Database db = Databsse;

            using (var dbCommand = db.GetStoredProcCommand("Acc_LogActionUpd"))
            {
                db.AddInParameter(dbCommand, "ActionId", DbType.Int32, entity.ActionID);
                db.AddInParameter(dbCommand, "ActionTypeId", DbType.Int32, entity.ActionTypeID);
                db.AddInParameter(dbCommand, "SPName", DbType.String, entity.SPName);
                db.AddInParameter(dbCommand, "ActionNameEN", DbType.String, entity.ActionNameEN);
                db.AddInParameter(dbCommand, "ActionNameCS", DbType.String, entity.ActionNameCS);
                db.AddInParameter(dbCommand, "ActionNameIT", DbType.String, entity.ActionNameIT);
                db.AddInParameter(dbCommand, "ActionNameJP", DbType.String, entity.ActionNameJP);
                db.AddInParameter(dbCommand, "ActionNameKO", DbType.String, entity.ActionNameKO);
                db.AddInParameter(dbCommand, "ActionNameTH", DbType.String, entity.ActionNameTH);
                db.AddInParameter(dbCommand, "IsInternal", DbType.Int16, entity.IsInternal);

                int iRecordAffected = Convert.ToInt32(db.ExecuteScalar(dbCommand));
                
                isUpdateSuccessful = (iRecordAffected > 0);
            }

            return isUpdateSuccessful;
        }

        #endregion
    }
}
