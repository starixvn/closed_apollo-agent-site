// --------------------------------------------------------------------------
// <copyright file="JSON.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Age.UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    public static class JSON
    {
        private const string NULL = "null";

        /// <summary>
        /// Convert a list of DictionaryEntry to JSON format.
        /// </summary>
        /// <param name="data">The data.</param>
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
                if (i < count - 1)
                {
                    builder.Append(',');
                }
            }

            builder.Append('}');

            return builder.ToString();          
        }

        /// <summary>
        /// Declares an object as JSON format.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>JSON string.</returns>
        private static string ObjectToJSON(object name, object value)
        {
            return name == null ? null : string.Format("\"{0}\":{1}", name, ToJSON(value));
        }

        /// <summary>
        /// Convert an object to JSON format.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ToJSON(object value)
        {
            if (value == null)
            {
                return NULL;
            }

            string text = value.ToString();

            if (value is string)
            {
                return string.Format("\"{0}\"", text.Replace("\"", "\\\""));
            }
            else if (value is JSONData)
            {
                return Render((JSONData)(value));
            }
            else if (value is bool)
            {
                return text.ToLower();
            }
            else if (value is Array)
            {
                return ArrayToJSON((object[])(value));
            }

            return text;
        }

        [Obsolete]
        private static string FormatString(string text)
        {
            return "'" + text.Replace("\'", "\\\'") + "'";
        }

        private static string ArrayToJSON(object[] values)
        {
            if (values == null)
            {
                return NULL;
            }

            int count = values.Length;
            var builder = new StringBuilder();

            builder.Append('[');

            for (int i = 0; i < count; i++)
            {
                builder.Append(ToJSON(values[i]));
                if (i < count - 1)
                {
                    builder.Append(',');
                }
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
        /// <param name="errCode">The error code.</param>
        /// <param name="errMsg">The error message.</param>
        public void AsError(int errCode, string errMsg)
        {
            this.Clear();
            this.Add(new DictionaryEntry(JSONError.ErrorCode, errCode));
            this.Add(new DictionaryEntry(JSONError.ErrorMessage, errMsg));
        }

        /// <summary>
        /// Return an error.
        /// </summary>
        /// <param name="errCode">The error code.</param>
        /// <param name="errMsg">The error message.</param>
        public void AsError(ErrorType errType, string errMsg)
        {
            this.Clear();
            this.Add(new DictionaryEntry(JSONError.ErrorCode, (int)(errType)));
            this.Add(new DictionaryEntry(JSONError.ErrorMessage, errMsg));
        }

        /// <summary>
        /// Return a result.
        /// </summary>
        /// <param name="result"></param>
        public void AsResult(Result result)
        {
            this.Clear();
            this.Add(new DictionaryEntry(JSONError.ErrorCode, result.ErrCode));

            if (result.ErrCode != 0)
            {
                Add(new DictionaryEntry(JSONError.ErrorMessage, result.ErrMsg));
            }
        }

        /// <summary>
        /// Return a result.
        /// </summary>
        /// <param name="result"></param>
        public void AsResult(Result result, bool isMsgSuccess)
        {
            this.Clear();
            this.Add(new DictionaryEntry(JSONError.ErrorCode, result.ErrCode));

            if (isMsgSuccess)
            {
                this.Add(new DictionaryEntry(JSONError.ErrorMessage, result.ErrMsg));
            }
        }

        /// <summary>
        /// Return a success result.
        /// </summary>
        public void AsSuccessResult()
        {
            this.Clear();
            this.Add(new DictionaryEntry(JSONError.ErrorCode, (int)(ErrorType.Success)));
        }
    }
}