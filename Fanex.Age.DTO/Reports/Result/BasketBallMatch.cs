/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.DTO.Reports.Result
{
    using System;
    using System.Linq;

    public class BasketBallMatch : SportMatch1
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
                var homeSet = new string[] { "-", "-", "-", "-", "-" };
                var awaySet = new string[] { "-", "-", "-", "-", "-" };
                if (this.BetStatus.Contains(','))
                {
                    var betStatusList = this.BetStatus.Split(',');
                    for (int j = 0; j < betStatusList.Length && j < 5; j++)
                    {
                        homeSet[j] = betStatusList[j].Trim().Split('-')[0].Trim();
                        awaySet[j] = betStatusList[j].Trim().Split('-')[1].Trim();

                        if (j == 4)
                        {
                            this.OTHome += Convert.ToInt32(homeSet[j]);
                            this.OTAway += Convert.ToInt32(awaySet[j]);
                        }
                    }
                }
                else
                {
                    ////HTHomeScore = null;
                    ////HTAwayScore = null;
                    ////FTHomeScore = null;
                    ////FTAwayScore = null;
                }

                this.Q1Home = homeSet[0];
                this.Q2Home = homeSet[1];
                this.Q3Home = homeSet[2];
                this.Q4Home = homeSet[3];

                this.Q1Away = awaySet[0];
                this.Q2Away = awaySet[1];
                this.Q3Away = awaySet[2];
                this.Q4Away = awaySet[3];
            }
        }

        public string Q1Home { get; private set; }

        public string Q2Home { get; private set; }

        public string Q3Home { get; private set; }

        public string Q4Home { get; private set; }

        public int OTHome { get; private set; }

        public string Q1Away { get; private set; }

        public string Q2Away { get; private set; }

        public string Q3Away { get; private set; }

        public string Q4Away { get; private set; }

        public int OTAway { get; private set; }
    }
}