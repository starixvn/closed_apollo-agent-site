using System;

namespace Sunplus.Agent.UI._ViewLog
{
    using Sunplus.Shared.ViewLogLib;
    using Sunplus.Shared.ViewLogLib.Services;
    using Sunplus.Shared.ViewLogLib.Entities;

    using Sunplus.Agent.Model.BL.Authorization;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.Model.Common;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class ViewLogPageBase : PageBase
    {
        public bool IsSubAaccountAndCannotViewLog
        {
            get
            {
                bool isSubAccountLogin = (CurrentUserSession.SubAccId > 0);
                bool isSubAccountCanViewLog = false;
                if (CurrentUserSession.SubPageAccess != null)
                {
                    isSubAccountCanViewLog = CurrentUserSession.SubPageAccess.ViewLog;
                }

                return (isSubAccountLogin && !isSubAccountCanViewLog);
            }
        }

        public IUserSession CurrentUserSession
        {
            get
            {
                return UserSession.Current;
            }
        }

        public static List<List<object>> FilterActionList = null;

        /// <summary>
        /// List action id and permission.
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        private bool IsShowActionId(int actionId)
        {
            if (FilterActionList == null)
            {

                List<object> _151_31 = new List<object>(new object[] { UserPermission.IsShowRacing_151_31, 1129, 339, 338, 340, 337 });
                List<object> _151_41 = new List<object>(new object[] { UserPermission.IsShowRacing_151_41, 1130, 343, 342, 344, 341 });
                List<object> _151_51 = new List<object>(new object[] { UserPermission.IsShowRacing_151_51, 1125, 322, 324, 321, 323 });

                List<object> _152_41 = new List<object>(new object[] { UserPermission.IsShowRacing_152_41, 1126, 327, 326, 328, 325 });
                List<object> _152_51 = new List<object>(new object[] { UserPermission.IsShowRacing_152_51, 1127, 330, 332, 329, 331 });

                List<object> _153_51 = new List<object>(new object[] { UserPermission.IsShowRacing_153_51, 1128, 334, 336, 333, 335 });

                // Add more here

                FilterActionList = new List<List<object>>();
                FilterActionList.AddRange(new List<object>[] { _151_31, _151_41, _151_51, _152_41, _152_51, _153_51 });
            }

            foreach (List<object> list in FilterActionList)
            {
                if (!list.Contains(actionId)) continue;
                return (bool)(list[0]);
            }

            return true;
        }

        protected override void BindDataByPageIndex(IList entityList, int totalRecords)
        {
            if (entityList == null) return;

            // Filter to hide some ActionId for Racing
            List<object> removedObjList = new List<object>();
            int newTotalRecords = totalRecords;

            foreach (object entity in entityList)
            {
                if (entity is LogBase)
                {
                    LogBase log = entity as LogBase;
                    if (!IsShowActionId(log.ActionID))
                    {
                        removedObjList.Add(log);
                        newTotalRecords--;
                    }
                }
            }

            foreach (object removedObj in removedObjList)
            {
                entityList.Remove(removedObj);
            }

            Paging = new Fanex.Age.UI.Components.Paging_Control(null, "paging", PageSize, Request.RawUrl, newTotalRecords);

            base.BindDataByPageIndex(entityList, newTotalRecords);
        }

        protected override void OnInit(EventArgs e)
        {
            //redirect user to login page
            //when user clear cookie id on url
            if (Session.IsNewSession)
            {
                Response.Redirect("~/Index.aspx");
            }

            if (IsSubAaccountAndCannotViewLog
                && Request.Url.ToString().IndexOf("LogLogin.aspx") < 0)
            {
                string custname = string.IsNullOrEmpty(Request["custName"]) ? "" : Request["custName"].ToString();
                Response.Redirect("LogLogin.aspx?tabid=Login&custname=" + custname);
            }

            // Remove white space
            Response.Filter = new TrimStream(Response.Filter);

            base.OnInit(e);
        }

        Fanex.Age.UI.Components.Paging_Control Paging = null;

        public override void RenderPager(int rowCount)
        {
            Pager = Paging;
        }

        protected override bool ShowOldValue
        {
            get
            {
                bool showOldValue = true;
                var cookie = Request.Cookies.Get("showOldValue");
                if (cookie != null)
                {
                    bool.TryParse(cookie.Value, out showOldValue);
                }
                return showOldValue;
            }
            set
            {
                base.ShowOldValue = value;
            }
        }

        /// <summary>
        /// Convert page index starting from 1 to 0
        /// </summary>
        public override int PageIndex
        {
            get
            {
                int pageIndex = 0;
                int.TryParse(Request["pageIndex"], out pageIndex);
                return pageIndex > 0 ? pageIndex - 1 : 0;
            }
        }

        const int DEFAULT_PAGESIZE = 50;
        public override int PageSize
        {
            get
            {
                int pageSize = DEFAULT_PAGESIZE;
                int.TryParse(Request["pageSize"], out pageSize);
                return pageSize > 0 ? pageSize : DEFAULT_PAGESIZE;
            }
            set
            {
                base.PageSize = value;
            }
        }

        /// <summary>
        /// Create request viewlog context
        /// </summary>
        /// <returns>IRequestViewLogContext</returns>

        public override IRequestViewLogContext CreateRequestViewLogContext()
        {

            var isSubAccount = false;
            var customerId = -1;

            AuthorizationBO bo = new AuthorizationBO(Session.SessionID);

            if (string.IsNullOrEmpty(this.CustomerName))
            {
                return null;
            }

            bool isValidDownline = bo.IsUsernameBelong(
                this.CustomerName,
                UserSession.Current.CustId,
                out customerId, 
                out isSubAccount
                );


            if (!isValidDownline)
            {
                return null;
            }

            IRequestViewLogContext requestViewLogContext = new RequestViewLogContext
            {
                ShowOldValue = this.ShowOldValue,
                IsInternal = CurrentUserSession.IsInternal,
                CustomerName = this.CustomerName,
                FromDate = this.FromDate,
                ToDate = this.ToDate,
                CustomerId = customerId,
                IsSubCust = isSubAccount,
                LanguageCode = ToLanguageCode(CurrentUserSession.LangKey)
            };

            return requestViewLogContext;
        }

        public override string GetCurrentLanguage()
        {
            return CurrentUserSession.Language;
        }
    }

    #region ResponseWhitespaceFilter
    // http://www.codeproject.com/KB/aspnet/RemovingWhiteSpacesAspNet.aspx

    public class TrimStream : Stream
    {
        private Stream stream;
        private StreamWriter streamWriter;

        private Decoder dec;

        public TrimStream(Stream stm)
        {
            stream = stm;
            streamWriter = new StreamWriter(stream, System.Text.Encoding.UTF8);

            dec = Encoding.UTF8.GetDecoder();
        }

        /// <summary>

        /// Flag - write '\n' before next line

        /// </summary>

        private bool bNewLine = false;
        /// <summary>

        /// Flag - lash non-blank char in line was '>'

        /// </summary>

        private bool bLastCharGT = false;
        /// <summary>

        /// Array holding white chars from end of last line between Write() calls

        /// </summary>

        private char[] arBlanks = null;

        public override void Write(byte[] buffer, int offset, int count)
        {
            int nCharCnt = dec.GetCharCount(buffer, offset, count);
            char[] result = new char[nCharCnt];
            int nDecoded = dec.GetChars(buffer, offset, count, result, 0);

            if (nDecoded <= 0)
                return;

            int nFirstNonBlank = -1; //position of first non-black line char

            int nLastNonBlank = -1; //position of last non-black line char

            int nFirstLineChar = 0; //position of first line char (any)


            bool bFirstLine = true;

            for (int nPos = 0; nPos <= nDecoded; ++nPos)
            {
                bool bLastLine = nPos == nDecoded;

                char c = (nPos < nDecoded) ? result[nPos] : '\n';
                if (c == '\n') //handle new line
                {
                    //first line, and we have important 

                    //white chars from previous Write() call

                    if (bFirstLine && (arBlanks != null))
                    {
                        //current line contains non-blank chars 

                        //- write white chars from previous call

                        if (nFirstNonBlank >= 0)
                        {
                            if (arBlanks.Length > 0)
                                streamWriter.Write(arBlanks, 0, arBlanks.Length);

                            arBlanks = null;
                            nFirstNonBlank = 0;
                            bNewLine = false;
                        }
                    }
                    bFirstLine = false;

                    //current line contains any non-white chars - write them

                    if (nFirstNonBlank >= 0)
                    {
                        if (bNewLine && (result[nFirstNonBlank] != '<'))
                            //write new line char ?

                            streamWriter.WriteLine();

                        //write current line (trimmed)

                        streamWriter.Write(result, nFirstNonBlank,
                              nLastNonBlank - nFirstNonBlank + 1);

                        //setting variables...

                        if (!bLastLine)
                        {
                            nFirstNonBlank = -1;
                            nLastNonBlank = -1;
                            nFirstLineChar = nPos + 1;
                        }
                        bNewLine = !bLastCharGT;
                        bLastCharGT = false;
                    }

                    if (bLastLine)
                    //last line - optionally remember white chars from its end
                    {
                        if ((arBlanks == null) && (nFirstNonBlank < 0))
                        {
                            //empty line and we don't have any 

                            //white chars from previous call - nothing to do

                        }
                        else if (nLastNonBlank < nDecoded - 1)
                        //there was white chars at end of this line
                        {
                            int nNumBlanks, nFirstBlank;
                            if (nLastNonBlank < 0)
                            {
                                nNumBlanks = nDecoded - nFirstLineChar;
                                nFirstBlank = nFirstLineChar;
                            }
                            else
                            {
                                nNumBlanks = nDecoded - nLastNonBlank - 1;
                                nFirstBlank = nLastNonBlank + 1;
                            }

                            if ((arBlanks == null) || (arBlanks.Length <= 0))
                            //create new array?
                            {
                                arBlanks = new char[nNumBlanks];
                                Array.Copy(result, nFirstBlank,
                                           arBlanks, 0, nNumBlanks);
                            }
                            else //append at end of existsing array
                            {
                                char[] ar = new char[arBlanks.Length + nNumBlanks];
                                arBlanks.CopyTo(ar, 0);
                                Array.Copy(result, nFirstBlank, ar,
                                           arBlanks.Length, nNumBlanks);
                                arBlanks = ar;
                            }
                        }
                        else
                        //line without white-chars at end 

                        //- mark this using zero-sized array
                        {
                            arBlanks = new char[0];
                        }

                        //set variable...

                        bNewLine = false;
                    }
                }
                else if (!Char.IsWhiteSpace(c)) //handle non-white chars
                {
                    if (nFirstNonBlank < 0)
                        nFirstNonBlank = nPos;
                    nLastNonBlank = nPos;
                    bLastCharGT = (c == '>');
                }
            }

            streamWriter.Flush();
        }

        /* other overrided Stream functions and properties goes here */

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            streamWriter.Flush();
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
