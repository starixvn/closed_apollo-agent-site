using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/*
 * Created 20091211@Lee - JSON processing
 * Revision: 
 * 20091211@Lee - Initializing
 * 
 * !!!CAUTION: ANY REVISION MUST BE APPROVED BEFORE PROCEEDING!!!
 * 
 */

namespace Fanex.Age.UI
{
    public static class JSON
    {
        private const string NULL = "null";

        /// <summary>
        /// Convert a list of DictionaryEntry to JSON format.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string Render(JSONData data)
        {
            if (data == null)
            {
                return NULL;
            }

            int count = data.Count;
            var builder = new StringBuilder();

            builder.Append('{');

            for (int i = 0; i < count; i++)
            {
                builder.Append(ObjectToJSON(data[i].Key, data[i].Value));
                if (i < count - 1) builder.Append(',');
            }

            builder.Append('}');

            return builder.ToString();

            #region old code
            //if (null == d) return "null";
            //int c = d.Count;

            //StringBuilder builder = new StringBuilder();
            //builder.Append('{');


            //for (int i = 0; i < c; i++)
            //{
            //    builder.Append(ObjectToJSON(d[i].Key, d[i].Value));
            //    if (i < c - 1) builder.Append(',');
            //}

            //builder.Append('}');
            //return builder.ToString(); 
            #endregion
        }

        /// <summary>
        /// Declares an object as JSON format.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ObjectToJSON(object name, object value)
        {
            return name == null ? null
                : string.Format("\"{0}\":{1}", name, ToJSON(value));
        }

        /// <summary>
        /// Convert an object to JSON format.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ToJSON(object value)
        {
            if (value == null) return NULL;

            string text = value.ToString();

            if (value is string)
                return string.Format("\"{0}\"", text.Replace("\"", "\\\""));
            else if (value is JSONData)
                return Render((JSONData)(value));
            else if (value is bool)
                return text.ToLower();
            else if (value is Array)
                return ArrayToJSON((object[])(value));

            return text;
        }

        [Obsolete]
        private static string FormatString(string text)
        {
            return "'" + text.Replace("\'", "\\\'") + "'";
        }

        private static string ArrayToJSON(object[] values)
        {
            if (values == null) return NULL;

            int count = values.Length;
            var builder = new StringBuilder();

            builder.Append('[');

            for (int i = 0; i < count; i++)
            {
                builder.Append(ToJSON(values[i]));
                if (i < count - 1) builder.Append(',');
            }

            builder.Append(']');

            return builder.ToString();
        }
    }

    /// <summary>
    /// Represent .net data in JSON format.
    /// </summary>
    public class JSONData : List<DictionaryEntry>
    {
        public JSONData() { }

        public JSONData(int capacity)
        {
            Capacity = capacity;
        }

        public void Add(object name, object value)
        {
            Add(new DictionaryEntry(name, value));
        }

        /// <summary>
        /// Return an error.
        /// </summary>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        public void AsError(int errCode, string errMsg)
        {
            this.Clear();
            this.Add(new DictionaryEntry(JSONError.ERROR_CODE, errCode));
            this.Add(new DictionaryEntry(JSONError.ERROR_MSG, errMsg));
        }

        /// <summary>
        /// Return an error.
        /// </summary>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        public void AsError(ErrorType errType, string errMsg)
        {
            this.Clear();
            this.Add(new DictionaryEntry(JSONError.ERROR_CODE, (int)(errType)));
            this.Add(new DictionaryEntry(JSONError.ERROR_MSG, errMsg));
        }

        /// <summary>
        /// Return a result.
        /// </summary>
        /// <param name="result"></param>
        public void AsResult(Result result)
        {
            this.Clear();
            this.Add(new DictionaryEntry(JSONError.ERROR_CODE, result.ErrCode));

            if (result.ErrCode != 0)
            {
                Add(new DictionaryEntry(JSONError.ERROR_MSG, result.ErrMsg));
            }
        }

        /// <summary>
        /// Return a result.
        /// </summary>
        /// <param name="result"></param>
        public void AsResult(Result result, bool isMsgSuccess)
        {
            this.Clear();
            this.Add(new DictionaryEntry(JSONError.ERROR_CODE, result.ErrCode));

            if (isMsgSuccess)
            {
                this.Add(new DictionaryEntry(JSONError.ERROR_MSG, result.ErrMsg));
            }
        }

        /// <summary>
        /// Return a success result.
        /// </summary>
        public void AsSuccessResult()
        {
            this.Clear();
            this.Add(new DictionaryEntry(JSONError.ERROR_CODE, (int)(ErrorType.Success)));
        }
    }
}