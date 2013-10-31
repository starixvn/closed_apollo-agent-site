/*
 * Created 20091211@Lee - ERROR object in UI
 * Revision: 
 * 20091211@Lee - Initializing
 * 
 * !!!CAUTION: ANY REVISION MUST BE APPROVED BEFORE PROCEEDING!!!
 * 
 */

namespace Fanex.Age.UI
{
    public class JSONError : JSONData
    {
        // Error code convention:
        // -1 : Unknown error
        // 0 :Success
        // > 0 : Error code, retrieve error message from errMsg field
        public const string ERROR_CODE = "errCode";
        public const string ERROR_MSG = "errMsg";

        public JSONError(int errorCode, string errorMsg)
        {
            Add(ERROR_CODE, errorCode);
            Add(ERROR_MSG, errorMsg);
        }

        public JSONError(ErrorType errorType, string errorMsg)
        {
            Add(ERROR_CODE, (int)errorType);
            Add(ERROR_MSG, errorMsg);
        }
    }

    public enum ErrorType
    {
        Success = 0,
        Unknown = -1,
        AccessDenied = 1,
        AccountClosed = 2,
        KickedOut = 3,
        UnderMaintenance = 4
    }

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