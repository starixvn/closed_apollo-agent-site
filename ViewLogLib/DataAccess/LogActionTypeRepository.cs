using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Sunplus.Shared.ViewLogLib.Entities;

namespace Sunplus.Shared.ViewLogLib.DataAccess
{
    public class LogActionTypeRepository : RepositoryBase, ILogActionTypeRepository
    {
        #region IRepository<LogActionType> Members

        public IEnumerable<LogActionType> GetAll()
        {
            var entityList = new List<LogActionType>();
            Database db = base.Databsse;
            using (var dbCommand = db.GetStoredProcCommand("Acc_UserLog_LogActionTypeSel"))
            {
                using (var reader = db.ExecuteReader(dbCommand))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var entity = new LogActionType
                            {
                                ActionTypeID =
                                    reader.GetInt32(reader.GetOrdinal("ActionTypeID")),
                                ActionTypeNameEN =
                                    (reader.IsDBNull(reader.GetOrdinal("ActionTypeNameEN")))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("ActionTypeNameEN")),
                                ActionTypeNameCH =
                                    (reader.IsDBNull(reader.GetOrdinal("ActionTypeNameCH")))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("ActionTypeNameCH")),
                                ActionTypeNameCS =
                                    (reader.IsDBNull(reader.GetOrdinal("ActionTypeNameCS")))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("ActionTypeNameCS")),
                                ActionTypeNameJP =
                                    (reader.IsDBNull(reader.GetOrdinal("ActionTypeNameJP")))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("ActionTypeNameJP")),
                                ActionTypeNameIT =
                                    (reader.IsDBNull(reader.GetOrdinal("ActionTypeNameIT")))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("ActionTypeNameIT")),
                                ActionTypeNameTH =
                                    (reader.IsDBNull(reader.GetOrdinal("ActionTypeNameTH")))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("ActionTypeNameTH")),
                                ActionTypeNameKO =
                                    (reader.IsDBNull(reader.GetOrdinal("ActionTypeNameKO")))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("ActionTypeNameKO"))
                            };

                            entityList.Add(entity);
                        }
                    }
                }
            }

            return entityList;
        }

        public bool Update(LogActionType entity)
        {
            bool isUpdateSuccessful;
            Database db = Databsse;

            using (var dbCommand = db.GetStoredProcCommand("Acc_LogActionUpd"))
            {
                db.AddInParameter(dbCommand, "ActionTypeId", DbType.Int32, entity.ActionTypeID);
                db.AddInParameter(dbCommand, "ActionTypeNameEN", DbType.String, entity.ActionTypeNameEN);
                db.AddInParameter(dbCommand, "ActionTypeNameCS", DbType.String, entity.ActionTypeNameCS);
                db.AddInParameter(dbCommand, "ActionTypeNameIT", DbType.String, entity.ActionTypeNameIT);
                db.AddInParameter(dbCommand, "ActionTypeNameJP", DbType.String, entity.ActionTypeNameJP);
                db.AddInParameter(dbCommand, "ActionTypeNameKO", DbType.String, entity.ActionTypeNameKO);
                db.AddInParameter(dbCommand, "ActionTypeNameTH", DbType.String, entity.ActionTypeNameTH);
                

                int iRecordAffected = Convert.ToInt32(db.ExecuteScalar(dbCommand));

                isUpdateSuccessful = (iRecordAffected > 0);
            }

            return isUpdateSuccessful;
        }

        #endregion
    }
}
