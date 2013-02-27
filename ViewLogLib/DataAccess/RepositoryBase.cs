using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Sunplus.Shared.ViewLogLib.DataAccess
{
    using Fanex.Data;

    public abstract class RepositoryBase 
    {
        private static string ConnectionString
        {
            get
            {
                //get from cache
                var connectionString = ConnectionStringGetter.GetConnectionString("CustomerLogEntities");

                return connectionString;
            }
        }

        private SqlDatabase _db;
        protected SqlDatabase Databsse
        {
            get
            {
                
                _db = new SqlDatabase(ConnectionString);
                return _db;
            }
        }
    }
}
