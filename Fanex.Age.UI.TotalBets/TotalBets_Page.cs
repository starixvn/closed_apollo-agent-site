/*
 * Created 20100527@Jacob - Support render full mode or body mode
 * Revision:
 *      Lewis@20130329 - added constants
 * 
 */

namespace Fanex.Age.UI.TotalBets
{
    using System;
    using Sunplus.Agent.Model.BL.TotalBets;

    public abstract class TotalBets_Page : TplPage
    {
        #region Const
        protected const string LIVE = "live";
        protected const string ALT = "alt";
        #endregion

        #region Variable
        /// <summary>
        /// If fullMode=false, only render the body content
        /// Else render full page
        /// </summary>
        protected bool _fullMode = true;
        private TotalBetsBO _totalBetsBO = null;
        protected TotalBetsBO _TotalBetsBO
        {
            get
            {
                if (_totalBetsBO == null) _totalBetsBO = new TotalBetsBO(CurrentUserSession.NetSessionId);
                return _totalBetsBO;
            }
        }

        #endregion

        #region Override

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            if (Request["full_mode"] != null)
            {
                int fullMode = 0;
                if (Int32.TryParse(Request["full_mode"], out fullMode))
                {
                    _fullMode = fullMode > 0 ? true : false;
                }
            }
        }
        #endregion
    }
}