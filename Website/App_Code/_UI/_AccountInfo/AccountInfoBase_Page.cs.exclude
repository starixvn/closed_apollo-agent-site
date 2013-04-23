namespace Sunplus.Agent.UI._AccountInfo.AccountInfoBase
{
    using System;
    using System.Data;
    using Fanex.Age.UI;
    using Fanex.Agent.UI._AccountInfo;
    

    public abstract class AccountInfoBase_Page : TplPage
    {
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {

        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            return null;
        }

        #region Utility

        /// <summary>
        /// Get DB boolean value
        /// If null, return false
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool GetDBBooleanValue(object data)
        {
            if (Convert.IsDBNull(data))
            {
                return false;
            }

            return (bool)data;
        }

        /// <summary>
        /// Get DB disable boolean value
        /// If null, return false
        /// If data = true return false
        /// else return true
        /// Use for disable value:
        ///         disable HR
        ///         disable Outright
        ///         .............
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool GetDBEnabledBooleanValue(object data)
        {
            if (Convert.IsDBNull(data))
            {
                return false;
            }

            return (bool)data ? false : true;
        }

        /// <summary>
        /// Get int value from database
        /// return 0 if the object is DBNull
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int GetDBIntValue(object data)
        {
            if (Convert.IsDBNull(data))
            {
                return 0;
            }

            return (int)data;
        }

        /// <summary>
        /// Get decimal value from database
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public decimal GetDBDecimalValue(object data)
        {
            if (Convert.IsDBNull(data))
            {
                return 0;
            }

            return (decimal)data;
        }

        /// <summary>
        /// Get string value from database
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetDBStringValue(object data)
        {
            if (Convert.IsDBNull(data))
            {
                return string.Empty;
            }

            return data.ToString();
        }

        #endregion Utility
    }
}
