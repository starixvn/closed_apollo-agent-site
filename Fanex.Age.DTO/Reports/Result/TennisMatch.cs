/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.DTO.Reports.Result
{
    using System;
    using System.Collections.Generic;

    public class TennisMatch : SportMatch1
    {
        public override string BetStatus
        {
            get
            {
                return base.BetStatus;
            }

            set
            {
                if (base.BetStatus == value)
                {
                    return;
                }

                base.BetStatus = value;
                this.TotalGamesHome = 0;
                this.TotalGamesAway = 0;
                var homeSet = new string[] { "-", "-", "-", "-", "-" };
                var awaySet = new string[] { "-", "-", "-", "-", "-" };
                if (this.BetStatus.Contains(","))
                {
                    var betStatusList = this.BetStatus.Split(',');
                    for (int j = 0; j < betStatusList.Length && j < 5; j++)
                    {
                        homeSet[j] = betStatusList[j].Trim().Split('-')[0].Trim();
                        awaySet[j] = betStatusList[j].Trim().Split('-')[1].Trim();
                        this.TotalGamesHome += Convert.ToInt32(homeSet[j]);
                        this.TotalGamesAway += Convert.ToInt32(awaySet[j]);
                    }
                }

                this.S1Home = homeSet[0];
                this.S2Home = homeSet[1];
                this.S3Home = homeSet[2];
                this.S4Home = homeSet[3];
                this.S5Home = homeSet[4];

                this.S1Away = awaySet[0];
                this.S2Away = awaySet[1];
                this.S3Away = awaySet[2];
                this.S4Away = awaySet[3];
                this.S5Away = awaySet[4];
            }
        }

        public string S1Home { get; private set; }

        public string S2Home { get; private set; }

        public string S3Home { get; private set; }

        public string S4Home { get; private set; }

        public string S5Home { get; private set; }

        public int TotalGamesHome { get; private set; }

        public string S1Away { get; private set; }

        public string S2Away { get; private set; }

        public string S3Away { get; private set; }

        public string S4Away { get; private set; }

        public string S5Away { get; private set; }

        public int TotalGamesAway { get; private set; }
    }
}