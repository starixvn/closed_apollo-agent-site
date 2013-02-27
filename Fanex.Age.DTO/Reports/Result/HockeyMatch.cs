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
    using System.Linq;
    using System.Text;

    public class HockeyMatch : SportMatch1
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
                this.OTHome = 0;
                this.OTAway = 0;
                var homeSet = new string[] { "-", "-", "-", "-" };
                var awaySet = new string[] { "-", "-", "-", "-" };
                if (this.BetStatus.Contains(','))
                {
                    var betStatusList = this.BetStatus.Split(',');
                    for (int j = 0; j < betStatusList.Length && j < 4; j++)
                    {
                        homeSet[j] = betStatusList[j].Trim().Split('-')[0].Trim();
                        awaySet[j] = betStatusList[j].Trim().Split('-')[1].Trim();
                        if (j == 3)
                        {
                            this.OTHome = Convert.ToInt32(homeSet[j]);
                            this.OTAway = Convert.ToInt32(awaySet[j]);
                        }
                    }
                }
                else
                {
                    ////FTHomeScore = null;
                    ////FTAwayScore = null;
                }

                this.P1Home = homeSet[0];
                this.P2Home = homeSet[1];
                this.P3Home = homeSet[2];

                this.P1Away = awaySet[0];
                this.P2Away = awaySet[1];
                this.P3Away = awaySet[2];
            }
        }

        public string P1Home { get; private set; }

        public string P2Home { get; private set; }

        public string P3Home { get; private set; }

        public int OTHome { get; private set; }

        public string P1Away { get; private set; }

        public string P2Away { get; private set; }

        public string P3Away { get; private set; }

        public int OTAway { get; private set; }
    }
}