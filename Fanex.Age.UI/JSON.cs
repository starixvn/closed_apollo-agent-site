using System;
using System.Collections.Generic;
using System.Collections;
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
        /// <summary>
        /// Convert a list of DictionaryEntry to JSON format.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string Render(JSONData d)
        {
            if (null == d) return "null";
            int c = d.Count;

            StringBuilder builder = new StringBuilder();
            builder.Append('{');


            for (int i = 0; i < c; i++)
            {
                builder.Append(ObjectToJSON(d[i].Key, d[i].Value));
                if (i < c - 1) builder.Append(',');
            }

            builder.Append('}');
            return builder.ToString();
        }

        /// <summary>
        /// Declare an object as JSON format.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ObjectToJSON(object name, object value)
        {
            if (null == name) return null;
            return "'" + name + "':" + ToJSON(value);
        }

        /// <summary>
        /// Convert an object to JSON format.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ToJSON(object value)
        {
            if (null == value) return "null";

            string text = value.ToString();

            if (value is string)
                return FormatString(text);
            else if (value is JSONData)
                return Render((JSONData)(value));
            else if (value is bool)
                return text.ToLower();
            else if (value is Array)
                return ArrayToJSON((object[])(value));
            
            // Return default ToString() method
            return text;
        }

        private static string FormatString(string text)
        {
            return "'" + text.Replace("\'", "\\\'") + "'";
        }

        private static string ArrayToJSON(object[] values)
        {
            if (null == values) return "null";
            int c = values.Length;

            StringBuilder builder = new StringBuilder();
            builder.Append('[');

            for (int i = 0; i < c; i++)
            {
                builder.Append(ToJSON(values[i]));
                if (i < c - 1) builder.Append(',');
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
        public JSONData()
        { 

        }

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
            Clear();
            Add(new DictionaryEntry(JSONError.ERROR_CODE, errCode));
            Add(new DictionaryEntry(JSONError.ERROR_MSG, errMsg));
        }

        /// <summary>
        /// Return an error.
        /// </summary>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        public void AsError(ErrorType errType, string errMsg)
        {
            Clear();
            Add(new DictionaryEntry(JSONError.ERROR_CODE, (int)(errType)));
            Add(new DictionaryEntry(JSONError.ERROR_MSG, errMsg));
        }

        /// <summary>
        /// Return a result.
        /// </summary>
        /// <param name="result"></param>
        public void AsResult(Result r)
        {
            Clear();
            Add(new DictionaryEntry(JSONError.ERROR_CODE, r.ErrCode));
            if (r.ErrCode != 0)
            {
                Add(new DictionaryEntry(JSONError.ERROR_MSG, r.ErrMsg));
            }
        }

        /// <summary>
        /// Return a result.
        /// </summary>
        /// <param name="result"></param>
        public void AsResult(Result r, bool isMsgSuccess)
        {
            Clear();
            Add(new DictionaryEntry(JSONError.ERROR_CODE, r.ErrCode));
            if (isMsgSuccess)
            {
                Add(new DictionaryEntry(JSONError.ERROR_MSG, r.ErrMsg));
            }
        }

        /// <summary>
        /// Return a success result.
        /// </summary>
        public void AsSuccessResult()
        {
            Clear();
            Add(new DictionaryEntry(JSONError.ERROR_CODE, (int)(ErrorType.Success)));
        }
    }
}