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

    public class BaseBallMatch : SportMatch1
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
                var homeSet = new string[] { "-", "-", "-", "-", "-", "-", "-", "-", "-", "-1" };
                var awaySet = new string[] { "-", "-", "-", "-", "-", "-", "-", "-", "-", "-1" };
                if (this.BetStatus.Contains(","))
                {
                    var betStatusList = this.BetStatus.Split(',');
                    for (int j = 0; j < betStatusList.Length && j < 10; j++)
                    {
                        homeSet[j] = betStatusList[j].Trim().Split('-')[0].Trim();
                        awaySet[j] = betStatusList[j].Trim().Split('-')[1].Trim();
                    }
                }

                this.Home1 = homeSet[0];
                this.Home2 = homeSet[1];
                this.Home3 = homeSet[2];
                this.Home4 = homeSet[3];
                this.Home5 = homeSet[4];
                this.Home6 = homeSet[5];
                this.Home7 = homeSet[6];
                this.Home8 = homeSet[7];
                this.Home9 = homeSet[8];
                this.EIHome = Convert.ToInt32(homeSet[9]);

                this.Away1 = awaySet[0];
                this.Away2 = awaySet[1];
                this.Away3 = awaySet[2];
                this.Away4 = awaySet[3];
                this.Away5 = awaySet[4];
                this.Away6 = awaySet[5];
                this.Away7 = awaySet[6];
                this.Away8 = awaySet[7];
                this.Away9 = awaySet[8];
                this.EIAway = Convert.ToInt32(awaySet[9]);
            }
        }

        public string Home1 { get; private set; }

        public string Home2 { get; private set; }

        public string Home3 { get; private set; }

        public string Home4 { get; private set; }

        public string Home5 { get; private set; }

        public string Home6 { get; private set; }

        public string Home7 { get; private set; }

        public string Home8 { get; private set; }

        public string Home9 { get; private set; }

        public int EIHome { get; private set; }

        public string Away1 { get; private set; }

        public string Away2 { get; private set; }

        public string Away3 { get; private set; }

        public string Away4 { get; private set; }

        public string Away5 { get; private set; }

        public string Away6 { get; private set; }

        public string Away7 { get; private set; }

        public string Away8 { get; private set; }

        public string Away9 { get; private set; }

        public int EIAway { get; private set; }
    }
}