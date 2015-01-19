namespace Apollo.Age.Entities
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
            this.ErrorCode = errorCode;
            this.ErrorMsg = errorMessage;
        }

        public int ErrorCode
        {
            get;
            set;
        }

        public string ErrorMsg
        {
            get;
            set;
        }
    }
}