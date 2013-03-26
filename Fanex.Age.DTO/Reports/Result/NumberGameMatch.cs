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

    public class NumberGameMatch
    {
        private IList<byte?> ballList = null;

        public virtual int LeagueId { get; set; }

        public virtual DateTime KickOffTime { get; set; }

        public virtual int MatchId { get; set; }

        public virtual string MatchCode { get; set; }

        public virtual byte? Seq { get; set; }

        public virtual string EventStatus { get; set; }

        public virtual bool? IsPending { get; set; }

        public virtual IList<byte?> BallList
        {
            get
            {
                return this.ballList;
            }

            set
            {
                if (this.ballList == value)
                {
                    return;
                }

                this.ballList = value;
                if (this.BallList == null)
                {
                    return;
                }

                if (this.BallList.Count > 0)
                {
                    this.FirstBall = this.BallList.First();
                    if (this.BallList.Count > 1)
                    {
                        this.LastBall = this.BallList.Last();
                    }
                }

                this.BallSum = 0;
                foreach (var ball in this.BallList)
                {
                    this.BallSum += ball == null ? 0 : (int)ball;
                }
            }
        }

        public byte? FirstBall { get; private set; }

        public byte? LastBall { get; private set; }

        public int BallSum { get; private set; }
    }
}