using System.Data;

/*
 * Created 20100115@Lee - Data access base class
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.Model.DL.Base
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Data.Configuration;
    using Fanex.Data.DNAide;
    using Sunplus.Agent.Model.DL.Definitions;
    using Sunplus.Agent.Model.Services;
    using Fanex.PTEngine.BusinessObjects;
    using Fanex.Data;
    using Fanex.Age.Utility;

    public class DAOBase
    {
        private const int DEFAULT_TIME_OUT = 600;
        private const string NO_SP = "-";

        #region ExecSp to DbResult

        public virtual Result GetResult(ObjectDb db)
        {
            string errCode = db.GetParameterValue("errcode").ToString();
            string errMsg = db.GetParameterValue("errmsg").ToString();
            return new Result(errCode, errMsg);
        }

        public virtual IList<DbResult> ExecSp(string name, IList<Hashtable> parameters, int timeOut, bool hasTransaction)
        {
            var results = new List<DbResult>();

            for (int i = 0; i < parameters.Count; i++)
            {
                results.Add(DbHelper.ExecuteDbResult(name, parameters[i], DEFAULT_TIME_OUT));    
            }

            return results;
        }

        public virtual IList<DbResult> ExecSp(IList<string> names, IList<Hashtable> parameters, int timeOut, bool hasTransaction)
        {
            var results = new List<DbResult>();

            for (int i = 0; i < names.Count; i++)
            {
                results.Add(DbHelper.ExecuteDbResult(names[i], parameters[i], DEFAULT_TIME_OUT));    
            }

            return results;
        }

        public virtual IList<DbResult> ExecSp(string name, IList<Hashtable> parameters, bool hasTransaction)
        {
            var results = new List<DbResult>();

            for (int i = 0; i < parameters.Count; i++)
            {
                results.Add(DbHelper.ExecuteDbResult(name, parameters[i], DEFAULT_TIME_OUT));    
            }

            return results;
        }

        public virtual IList<DbResult> ExecSp(IList<string> names, IList<Hashtable> parameters, bool hasTransaction)
        {
            var results = new List<DbResult>();

            for (int i = 0; i < names.Count; i++)
            {
                results.Add(DbHelper.ExecuteDbResult(names[i], parameters[i], DEFAULT_TIME_OUT));    
            }

            return results;
        }

        /// <summary>
        /// Base method.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="dbType"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public virtual DbResult ExecSp(string name, Hashtable parameters, int timeOut)
        {
            DbResult dtbResult = null;

            var dtbSetting = DbSettingProviderManager.Current.GetSetting(name);
            string sp = dtbSetting.SpName;
            string asType = dtbSetting.DbName;

            if (sp == NO_SP)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                return new DbResult(new Hashtable(), ds);
            }

            bool useLiveDb = Configurations.AppSettings.Configuration.Get("UseLiveDb") == "1";
            bool useLiveMainDb = Configurations.AppSettings.Configuration.Get("UseLiveMainDb") == "1";

            if (useLiveDb && (
                (asType == DbTypes.MAIN && useLiveMainDb) ||
                asType == DbTypes.ARCHIVE ||
                asType == DbTypes.ARCHIVE5 ||
                asType == DbTypes.ARCHIVE52 ||
                asType == DbTypes.ARCHIVE53 ||
                asType == DbTypes.ARCHIVE6 ||
                asType == DbTypes.WAREHOUSE))
            {
                LiveDataService service = new LiveDataService();
                List<string> parameterNames = new List<string>();
                List<object> parameterValues = new List<object>();

                if (parameters != null)
                {
                    foreach (object key in parameters.Keys)
                    {
                        parameterNames.Add(key as string);
                        parameterValues.Add(parameters[key]);
                    }
                }

                dtbResult = (DbResult)service.Invoke(name, parameterNames.ToArray(), parameterValues.ToArray(), asType);
            }
            else
            {
                dtbResult = DbHelper.ExecuteDbResult(name, parameters, timeOut);
            }

            return dtbResult;
        }

        public DbResult ExecSp(string name, Hashtable parameters)
        {
            return ExecSp(name, parameters, DEFAULT_TIME_OUT);
        }

        #endregion

        #region ExecSp to DataTable
        /// <summary>
        /// Execute use store procedure Return data table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual DataTable ExecSpTable(string name, Hashtable parameters)
        {
            return ExecSpTable(name, parameters, DEFAULT_TIME_OUT);
        }

        /// <summary>
        /// Execute use store procedure Return data table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="dbType"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public virtual DataTable ExecSpTable(string name, Hashtable parameters, int timeOut)
        {
            return ExecSpTable(name, parameters, timeOut, true);
        }

        /// <summary>
        /// Execute use store procedure Return data table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="dbType"></param>
        /// <param name="timeOut"></param>
        /// <param name="isVerifyingReturnData"></param>
        /// <returns></returns>
        public virtual DataTable ExecSpTable(string name, Hashtable parameters, int timeOut, bool isVerifyingReturnData)
        {
            DbResult dtbResult = ExecSp(name, parameters);

            // Check exist a DataTable in DataSet
            if (dtbResult != null && dtbResult.ResultSet.Tables.Count > 0)
            {
                return dtbResult.ResultSet.Tables[0];
            }
            else
            {
                if (isVerifyingReturnData)
                {
                    // Throw DataException and log the SP calling
                    DataException error = new DataException(name, parameters);
                    Logs.Write(error.Message);
                    throw error;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region ExecSp To DataSet
        /// <summary>
        /// Execute use store procedure Return dataset
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public virtual DataSet ExecSpDataset(string name, Hashtable parameters)
        {
            return ExecSpDataset(name, parameters, DEFAULT_TIME_OUT);
        }

        /// <summary>
        /// Execute use store procedure Return dataset
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="dbType"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public virtual DataSet ExecSpDataset(string name, Hashtable parameters, int timeOut)
        {
            DbResult dtbResult = ExecSp(name, parameters);

            // Check exist a DataTable in DataSet
            if (dtbResult != null)
            {
                return dtbResult.ResultSet;
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException(name, parameters);
                Logs.Write(error.Message);
                throw error;
            }
        }

        /// <summary>
        /// Execute use store procedure Return dataset
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public virtual DataSet ExecSp(string name, ref Hashtable parameters)
        {
            DataSet dataSet = null;
            DbResult dtbResult = ExecSp(name, parameters, DEFAULT_TIME_OUT);

            // Check exist a DataTable in DataSet
            if (dtbResult != null)
            {
                Hashtable outputs = dtbResult.Outputs;

                foreach (string paramName in outputs.Keys)
                {
                    if (parameters.ContainsKey(paramName))
                    {
                        parameters[paramName] = outputs[paramName];
                    }
                    else
                    {
                        parameters.Add(paramName, outputs[paramName]);
                    }
                }

                dataSet = dtbResult.ResultSet;
            }

            return dataSet;
        }

        #endregion

        #region ExecSp To Hashtable

        /// <summary>
        /// Execute use store procedure Return hash table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public virtual Hashtable ExecSpHashtable(string name, Hashtable parameters)
        {
            return ExecSpHashtable(name, parameters, DEFAULT_TIME_OUT);
        }

        /// <summary>
        /// Execute use store procedure Return hash table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="dbType"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public virtual Hashtable ExecSpHashtable(string name, Hashtable parameters, int timeOut)
        {
            DbResult dtbResult = ExecSp(name, parameters);

            // Check exist a DataTable in DataSet
            if (dtbResult != null && dtbResult.Outputs != null && dtbResult.Outputs.Count > 0)
            {
                return dtbResult.Outputs;
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException(name, parameters);
                Logs.Write(error.Message);
                throw error;
            }
        }

        #endregion
    }
}
