namespace Fanex.Age.Business.Reports.Commission
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Age.DTO.Reports.Commission;

    public class CommissionByBetTypeBusiness
    {
        public IList<CommissionByBetTypeItem> ToList(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = (from c in dt.AsEnumerable()
                          select new CommissionByBetTypeItem
                          {
                              Username = c.Field<string>("username"),
                              Handicap = c.Field<decimal>("hdp_comm"),
                              OverUnder = c.Field<decimal>("ou_comm"),
                              FirstHalfHandicap = c.Field<decimal>("hdp1st_comm"),
                              FirstHalfOverUnder = c.Field<decimal>("ou1st_comm"),
                              OddEven = c.Field<decimal>("oe_comm"),
                              CSheet = c.Field<decimal>("csh_comm"),
                              DChance = c.Field<decimal>("dc_comm"),
                              DNB = c.Field<decimal>("dnb_comm"),
                              BON = c.Field<decimal>("bon_comm"),
                              TWinTNill = c.Field<decimal>("wn_comm"),
                              ThirdWayHdp = c.Field<decimal>("hdp3_comm"),
                              OneXTwo = c.Field<decimal>("x12_comm"),
                              CorrectScore = c.Field<decimal>("cs_comm"),
                              TotalGoal = c.Field<decimal>("tg_comm"),
                              MixParlay = c.Field<decimal>("mp_comm"),
                              CombinationMixParlay = c.Field<decimal>("sp_comm"),
                              Outright = c.Field<decimal>("outr_comm"),
                              FGLG = c.Field<decimal>("fglg_comm"),
                              HTFT = c.Field<decimal>("htft_comm"),
                              MoneyLineFullTime = c.Field<decimal>("moneyline_comm"),
                              MoneyLineHalfTime = c.Field<decimal>("moneyline1st_comm"),
                              Casino = c.Field<decimal>("Casino_comm"),
                              LiveCasino = c.Field<decimal>("LiveCasino_comm"),
                              NumberGame = c.Field<decimal>("NumberGame_comm"),
                              //Bingo = c.Field<decimal>("Bingo_comm"),
                              P2P = c.Field<decimal>("P2P_comm"),
                              Total1 = c.Field<decimal>("TotalComm_1"),
                              Total2 = c.Field<decimal>("TotalComm_2"),
                              OneXTwoComm = c.Field<decimal>("x12_comm"),
                              First1X2Comm = c.Field<decimal>("f_x12_comm"),
                              Others = c.Field<decimal>("others_comm"),
                              HNB = c.Field<decimal>("hnb_comm"),
                              ANB = c.Field<decimal>("anb_comm"),
                              DND = c.Field<decimal>("dnd_comm"),
                              FirstCS = c.Field<decimal>("f_cs_comm")
                          }).ToList();

            return result;
        }
    }
}