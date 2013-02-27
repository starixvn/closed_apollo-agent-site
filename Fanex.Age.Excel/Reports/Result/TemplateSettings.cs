using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.Excel.Reports.Result
{
    public class TemplateSettings
    {
        private string _dir;

        public TemplateSettings()
        {
            _dir = HttpContext.Current.Server.MapPath("~") + "\\ExcelTemplates\\_Reports\\Result";
        }

        public string GetOutrightTemplateFileName(int sportTypeId)
        {
            return "Outright.xls";
        }

        public string GetOutrightTemplatePath(int sportTypeId)
        {
            return _dir + "\\" + GetOutrightTemplateFileName(sportTypeId);
        }

        public string GetTemplateFileName(int sportTypeId)
        {
            string filename = "";
            switch (sportTypeId)
            {
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Soccer:
                    filename = "Soccer.xls";
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.BasketBall:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Netball:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.FootBall:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Rugby:
                    filename = "BasketBall.xls";
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.IceHockey:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.FieldHockey:
                    filename = "Hockey.xls";
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Tennis:
                    filename = "Tennis.xls";
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.VolleyBall:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Badminton:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Squash:
                    filename = "Badminton.xls";
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.BaseBall:
                    filename = "BaseBall.xls";
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Golf:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Motorsports:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Billard:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Winter:
                    filename = "Billiard.xls";
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.HorseRacing:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Greyhound:
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Harness:
                    filename = "HorseRacing.xls";
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.NumberGame:
                    filename = "NumberGame.xls";
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.SportType.LiveCasino:
                    filename = "LiveCasino.xls";
                    break;
                default:
                    filename = "Other.xls";
                    break;
            }
            return filename;
        }

        public string GetTemplatePath(int sportTypeId)
        {
            return _dir + "\\" + GetTemplateFileName(sportTypeId);
        }
    }
}
