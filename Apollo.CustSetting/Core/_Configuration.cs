// --------------------------------------------------------------------------
// <copyright file="_Configuration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.5">Created</change>
// </history>

namespace Apollo.CustSetting.Core
{
    using System.Web;
    using System.Xml.Linq;

    /// <summary>
    /// Configuration Class, read Xml file and save XDocument object.
    /// </summary>
    public static class _Configuration
    {
        static _Configuration()
        {
            PositionTakingGridConfig = XDocument.Load(HttpContext.Current.Server.MapPath(Constant.PT_GRID_CONFIG));
            BettingLimitSportNameConfig = XDocument.Load(HttpContext.Current.Server.MapPath(Constant.BETTING_LIMIT_SPORT_NAMES_FILE));
        }

        public static XDocument PositionTakingGridConfig { get; set; }

        public static XDocument BettingLimitSportNameConfig { get; set; }
    }
}