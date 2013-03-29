using System;
using System.Collections;
using Sunplus.Agent.Model.BL.TotalBets;

/*
 * Created 20100527@Jacob - Support render full mode or body mode
 * Revision:
 * ?@? - ... 
 * 
 */
/// </summary>
namespace Fanex.Age.UI.TotalBets
{
    public abstract class TotalBets_Page : TplPage
    {
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
                if (_totalBetsBO == null) 
                    _totalBetsBO = new TotalBetsBO(CurrentUserSession.NetSessionId);

                return _totalBetsBO;
            }
        }

        public override void ProcessRequest(ref Hashtable p)
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
    }
}