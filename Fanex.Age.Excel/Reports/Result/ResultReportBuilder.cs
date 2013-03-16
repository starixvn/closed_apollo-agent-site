using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Web;
using System.Resources;
using Fanex.Age.DTO.Reports.Result;
using Fanex.Age.Utility;
using Fanex.Age.Business.Reports.Result;

/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.Excel.Reports.Result
{
    public class ResultReportBuilder : BaseResultReportBuilder
    {
        public object Data { get; private set; }

        public ResultReportBuilder(int sportTypeId, int leagueId, int betType, DataTable data, DateTime fromDate, DateTime toDate)
        {
            SportTypeId = sportTypeId;
            LeagueId = leagueId;
            BetType = betType;
            FromDate = fromDate;
            ToDate = toDate;
            IsOutright = false;
            var business = new ResultBusiness(sportTypeId);
            Data = business.ToList(data);
        }

        public MemoryStream Build()
        {
            TemplateReport report = null;

            TemplateSettings settings = new TemplateSettings();
            string filePath = settings.GetTemplatePath(SportTypeId);
            switch (SportTypeId)
            {
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Soccer:
                    report = new ResultSoccerReport(filePath, this);
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.BasketBall:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Netball:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.FootBall:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Rugby:
                    report = new ResultBasketBallReport(filePath, this);
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.IceHockey:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.FieldHockey:
                    report = new ResultHockeyReport(filePath, this);
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Tennis:
                    report = new ResultTennisReport(filePath, this);
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.VolleyBall:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Badminton:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Squash:
                    report = new ResultBadmintonReport(filePath, this);
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.BaseBall:
                    report = new ResultBaseBallReport(filePath, this);
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Golf:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Motorsports:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Billard:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Winter:
                    report = new ResultBilliardReport(filePath, this);
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.HorseRacing:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Greyhound:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Harness:
                    report = new ResultHorseRacingReport(filePath, this);
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.NumberGame:
                    report = new ResultNumberGameReport(filePath, this);
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.LiveCasino:
                    report = new ResultLiveCasinoReport(filePath, this);
                    break;
                default:
                    report = new ResultOtherSportReport(filePath, this);
                    break;
            }

            if (report != null)
            {
                if (report is IReportBuilder)
                {
                    MemoryStream mem = new MemoryStream();
                    HSSFWorkbook hssfWorkbook = ((IReportBuilder)report).Build();
                    if (hssfWorkbook != null)
                    {
                        hssfWorkbook.Write(mem);
                        return mem;
                    }
                }
                report.Dispose();
            }

            return null;
        }
    }
}
