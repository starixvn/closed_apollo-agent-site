/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Excel.Reports.Result
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Age.Utility.NPOI;
    using Fanex.Age.Utility.String;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    public static class Common
    {
        public const string NA = "N/A";
        public const string RefundHt = "Refund HT";
        public const string RefundFt = "Refund FT";
        public const string Refund = "Refund";

        // Define list key for eventstatus
        private static Dictionary<string, int> eventDic = new Dictionary<string, int>()
        {
            { "running", Convert.ToInt32("0000000", 2) },
            { "completed", Convert.ToInt32("0000001", 2) },
            { "closed", Convert.ToInt32("0000010", 2) },
            { "postponed", Convert.ToInt32("0000011", 2) },
            { "refund", Convert.ToInt32("0000100", 2) },
            { "refund ft", Convert.ToInt32("0000101", 2) },
            { "refund ht", Convert.ToInt32("0000110", 2) },
            { "won", Convert.ToInt32("0000111", 2) },
            { "internal", Convert.ToInt32("0001000", 2) }
        };

        // Define list key for Result: isPending(1 bit) isHTCal(1 bit) calculate(1 bit) enventStatus(4 bit)
        private static Dictionary<int, string> result = new Dictionary<int, string>()
        {
            { Convert.ToInt32("0000010", 2), "closed" },
            { Convert.ToInt32("0100010", 2), "closed" },
            { Convert.ToInt32("0000000", 2), "running" },
            { Convert.ToInt32("0100000", 2), "running" },
            { Convert.ToInt32("1000000", 2), "pending" },
            { Convert.ToInt32("1010010", 2), "pending" },
            { Convert.ToInt32("1100010", 2), "pending" },
            { Convert.ToInt32("0110001", 2), "completed" },
            { Convert.ToInt32("1000010", 2), "pending" },
            { Convert.ToInt32("0000100", 2), "refund" },
            { Convert.ToInt32("1100000", 2), "pending" },
            { Convert.ToInt32("1010000", 2), "pending" },
            { Convert.ToInt32("0000101", 2), "refundft" },
            { Convert.ToInt32("0010101", 2), "refundft" },
            { Convert.ToInt32("0010110", 2), "refundht" },
            { Convert.ToInt32("0000110", 2), "refundht" },
            { Convert.ToInt32("0010111", 2), "lblWon" },
            { Convert.ToInt32("0000111", 2), "lblWon" },
            { Convert.ToInt32("0010011", 2), "postponed" },
            { Convert.ToInt32("0000011", 2), "postponed" },
            { Convert.ToInt32("0011000", 2), "Internal" },
            { Convert.ToInt32("0001000", 2), "Internal" }
        };

        public static string GetHorseRacingPlaceString(int placeNo)
        {
            switch (placeNo)
            {
                case 1:
                    return Resources.Lang.No1;
                case 2:
                    return Resources.Lang.No2;
                case 3:
                    return Resources.Lang.No3;
                case 4:
                    return Resources.Lang.No4;
                default:
                    return placeNo + Resources.Lang.noothers;
            }
        }

        public static string MapEventStatusString(Fanex.Age.DTO.Reports.Result.OutrightMatch match)
        {
            return MapEventStatusString(match.Status, match.IsPending == null ? false : match.IsPending == true, false, false);
        }

        public static string MapEventStatusString(Fanex.Age.DTO.Reports.Result.SportMatch3 match)
        {
            return MapEventStatusString(match.EventStatus, match.IsPending == null ? false : match.IsPending == true, false, false);
        }

        public static string MapEventStatusString(Fanex.Age.DTO.Reports.Result.SportMatch1 match)
        {
            return MapEventStatusString(match.EventStatus, match.IsPending, match.IsHTCal, match.Calculate);
        }

        public static string MapEventStatusString(Fanex.Age.DTO.Reports.Result.NumberGameMatch match)
        {
            return MapEventStatusString(match.EventStatus, match.IsPending == null ? false : match.IsPending == true, false, false);
        }

        public static string MapEventStatusString(string eventstatus, bool isPending, bool isHTcal, bool calculate)
        {
            int resultKey = 0x0000000;
            int pending = Convert.ToInt32(isPending);
            int htcal = Convert.ToInt32(isHTcal);
            int cal = Convert.ToInt32(calculate);

            pending <<= 6; // Shift right 6 bit for pending status
            htcal <<= 5; // Shift right 5 bit for haft time calculate status
            cal <<= 4; // Shift right 4 bit for calculate status

            resultKey = pending | htcal | cal;

            if (eventDic.ContainsKey(eventstatus.ToLowerInvariant()))
            {
                resultKey |= eventDic[eventstatus.ToLowerInvariant()]; // Or with event status
                if (result.ContainsKey(resultKey))
                {
                    return Resources.Lang.ResourceManager.GetString(result[resultKey]);

                    // return "Completed";
                }
            }

            return eventstatus.UppercaseFirst();
        }

        public static void GetFormatText(Fanex.Age.DTO.Reports.Result.SportMatch1 match, HSSFWorkbook hssfWorkbook, out IRichTextString homeRtf, out IRichTextString awayRtf)
        {
            string homeName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetTeamName(match.HomeId) + (match.IsNeutral ? Resources.Lang.IsNeutral : string.Empty);
            homeName = homeName.Trim();
            string awayName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetTeamName(match.AwayId);
            awayName = awayName.Trim();

            RtfFontFormatter fontFormatter = new RtfFontFormatter("Tahoma", 9);
            IFont normalFont = hssfWorkbook.CreateFont();
            IFont boldFont = hssfWorkbook.CreateFont();
            boldFont.Boldweight = (short)FontBoldWeight.BOLD;
            fontFormatter.Format(normalFont);
            fontFormatter.Format(boldFont);

            RtfTextRender homeRtfTextRender = new RtfTextRender(normalFont);
            RtfTextRender awayRtfTextRender = new RtfTextRender(normalFont);
            if (match.FTHomeScore > match.FTAwayScore)
            {
                homeRtfTextRender = new RtfTextRender(boldFont);
            }
            else if (match.FTHomeScore < match.FTAwayScore)
            {
                awayRtfTextRender = new RtfTextRender(boldFont);
            }

            homeRtfTextRender.AddText(homeName);
            awayRtfTextRender.AddText(awayName);

            if (!string.IsNullOrEmpty(match.BetStatus) && match.BetStatus.Contains(':'))
            {
                string[] fglgArray = match.BetStatus.Split(':');
                if (fglgArray.Length > 1 && string.Compare(fglgArray[0], "FL", true) == 0)
                {
                    string firstGoal = fglgArray[1].Split('-')[0];
                    string lastGoal = fglgArray[1].Split('-')[1];
                    if (firstGoal == "1")
                    {
                        homeRtfTextRender.AddText("(F)", boldFont);
                    }
                    else if (firstGoal == "2")
                    {
                        awayRtfTextRender.AddText("(F)", boldFont);
                    }

                    if (lastGoal == "1")
                    {
                        homeRtfTextRender.AddText("(L)", boldFont);
                    }
                    else if (lastGoal == "2")
                    {
                        awayRtfTextRender.AddText("(L)", boldFont);
                    }
                }
            }

            homeRtf = homeRtfTextRender.Render();
            awayRtf = awayRtfTextRender.Render();
        }
    }
}