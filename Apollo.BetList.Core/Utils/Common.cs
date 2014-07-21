namespace Apollo.BetList.Core.Utils
{
    using Apollo.Age.Resources;

    public static class Common
    {
        /// <summary>
        /// Bets from format.
        /// </summary>
        /// <param name="betFrom">The bet from.</param>
        /// <returns>Formatted bet from.</returns>
        public static string BetFromFormat(string betFrom)
        {
            string betFromFormat = "</br><span class='red-color'>{0}</span>";

            return string.Format(betFromFormat, IndicatedBetFrom(betFrom));
        }

        /// <summary>
        /// Indicateds the bet from.
        /// </summary>
        /// <param name="betFrom">The bet from.</param>
        /// <returns>Indicated bet from.</returns>
        public static string IndicatedBetFrom(string betFrom)
        {
            switch (betFrom.ToLower())
            {
                case "s":
                    {
                        return BetList.smartdevice;
                    }

                case "a":
                    {
                        return BetList.minigame;
                    }

                default:
                    {
                        return string.Empty;
                    }
            }
        }
    }
}