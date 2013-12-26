// --------------------------------------------------------------------------
// <copyright file="Result.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.1">Created</change>
// </history>

namespace Apollo.CustSetting.Models
{
    using System;

    /// <summary>
    /// Result information.
    /// </summary>
    [Serializable]
    public class Result
    {
        public Result()
            : this(0, string.Empty)
        {
        }

        public Result(int errorCode, string errorMessage)
        {
            this.ErrCode = errorCode;
            this.ErrMsg = errorMessage;
        }

        public int ErrCode
        {
            get;
            set;
        }

        public string ErrMsg
        {
            get;
            set;
        }
    }
}