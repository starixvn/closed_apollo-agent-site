/*
 * Created 20091224@Lee - Data exception
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.Model.DL
{
    using System.Collections;
    using System.Text;

    public class DataException : System.Exception
    {
        private string message = null;

        public DataException(string stpName, Hashtable parameters)
        {
            StringBuilder error = new StringBuilder(100);

            error.Append("Unknown error when execute ");

            error.Append(stpName);

            if (null == parameters)
            {
                this.message = error.ToString();
                return;
            }

            int c = parameters.Count;

            object[] keys = new object[c];
            parameters.Keys.CopyTo(keys, 0);

            object[] values = new object[c];
            parameters.Values.CopyTo(values, 0);

            for (int i = 0; i < c; i++)
            {
                error.Append(" @" + keys[i] + "=" + values[i]);
            }

            this.message = error.ToString();
        }

        public override string Message
        {
            get
            {
                if (null != this.message)
                {
                    return this.message;
                }

                return base.Message;
            }
        }
    }
}
