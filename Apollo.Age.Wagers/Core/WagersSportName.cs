// --------------------------------------------------------------------------
// <copyright file="WagersSportName.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.6.29">Created</change>
// </history>

namespace Apollo.Age.Wagers.Core
{
    using System.Collections.Generic;
    using Apollo.Age.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class WagersSportName
    {
        public const string Basketball = "basketball";

        public const string Baseball = "baseball";

        public const string Others = "others";

        public const string Soccer = "soccer";

        public const string Tennis = "tennis";

        public const string Volleyball = "volleyball";

        /// <summary>
        /// Get sport id by sport name.
        /// </summary>
        /// <returns>Dictionary of sport id.</returns>
        public static Dictionary<string, string> WagerSportGroup()
        {
            var listOfSportType = new Dictionary<string, string>();

            listOfSportType.Add(Soccer, SportGroup.Soccer);
            listOfSportType.Add(Basketball, SportGroup.Basketball_Football_Rugy);
            listOfSportType.Add(Volleyball, SportGroup.Volleyball_Badminton_TableTennis);
            listOfSportType.Add(Tennis, SportGroup.Tennis);
            listOfSportType.Add(Baseball, SportGroup.BaseBall);
            listOfSportType.Add(Others, SportGroup.Others);

            return listOfSportType;
        }
    }
}