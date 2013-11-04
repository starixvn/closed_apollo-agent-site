// --------------------------------------------------------------------------
// <copyright file="Error.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.11.1">Created</change>
// </history>

namespace Fanex.Age.UI
{
    /// <summary>
    /// Error type.
    /// </summary>
    public enum ErrorType
    {
        Success = 0,
        Unknown = -1,
        AccessDenied = 1,
        AccountClosed = 2,
        KickedOut = 3,
        UnderMaintenance = 4
    }

    /// <summary>
    /// JSON error class.
    /// </summary>
    public class JSONError : JSONData
    {
        // Error code convention:
        // -1 : Unknown error
        // 0 :Success
        // > 0 : Error code, retrieve error message from errMsg field
        public const string ErrorCode = "errCode";

        public const string ErrorMessage = "errMsg";

        public JSONError(int errorCode, string errorMsg)
        {
            Add(ErrorCode, errorCode);
            Add(ErrorMessage, errorMsg);
        }

        public JSONError(ErrorType errorType, string errorMsg)
        {
            Add(ErrorCode, (int)errorType);
            Add(ErrorMessage, errorMsg);
        }
    }

    /// <summary>
    /// Result class.
    /// </summary>
    public class Result
    {
        public int ErrCode;
        public string ErrMsg;

        public Result(int code, string msg)
        {
            ErrCode = code;
            ErrMsg = msg;
        }

        public Result(ErrorType type, string msg)
        {
            ErrCode = (int)type;
            ErrMsg = msg;
        }
    }
}