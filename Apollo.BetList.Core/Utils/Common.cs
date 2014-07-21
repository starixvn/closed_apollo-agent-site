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
            switch (betFrom.ToLower())
            {
                case "s":
                    {
                        return string.Format(betFromFormat, BetList.smartdevice);
                    }

                case "a":
                    {
                        return string.Format(betFromFormat, BetList.minigame);
                    }

                default:
                    {
                        return string.Empty;
                    }
            }
        }
    }
}