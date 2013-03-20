<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.CustomerSetting.Handlers.Language_Handler" %>

namespace Sunplus.Agent.UI._MemberInfo.CustomerSetting.Handlers
{
    using System;
    using System.Web;
    using System.Web.SessionState;
    using System.Collections.Generic;
    using System.Resources;
    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Threading;
    using Sunplus.Agent.Model.Entities;
    using System.Text;

    public class Language_Handler : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            // use english date time format
            CultureInfo def = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(UserSession.Current.Language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(UserSession.Current.Language);
            System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat = def.DateTimeFormat;
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat = def.NumberFormat;

            context.Response.ContentType = "text/javascript";

            string language = this.SetLanguageAddNewMaster();
            context.Response.Write("var Language = {");
            context.Response.Write(language);
            context.Response.Write("};");
        }

        #region Set Language from Add New Master
        /// <summary>
        /// Set Language from Position Taking
        /// </summary>
        /// <returns></returns>
        private string SetLanguageAddNewMaster()
        {
            StringBuilder language = new StringBuilder();
            bool isThai = Thread.CurrentThread.CurrentCulture.Name == "th-TH";
            bool isVN = Thread.CurrentThread.CurrentCulture.Name == "vi-VN";
            // Set language to form Position Taking
            
            language.AppendLine(this.SetValue("MIN_BET", Resources.Lang.lblMinBet));
            language.AppendLine(this.SetValue("PRESET_POSITION", "Preset Position"));
            language.AppendLine(this.SetValue("POSITION_TAKING", Resources.Lang.positiontaking));
            language.AppendLine(this.SetValue("MAX_POSITION", "Max Position"));
            language.AppendLine(this.SetValue("MAX_BET", Resources.Lang.lblMaxBet));
            language.AppendLine(this.SetValue("MAX_PER_MATCH", Resources.Lang.lblMaxPerMatch));
            language.AppendLine(this.SetValue("MAX_PER_BALL", Resources.Lang.maxperball));
            language.AppendLine(this.SetValue("BETTING_LIMIT", "Bet Limit"));//"Bet Setting"
            language.AppendLine(this.SetValue("DEAD_BALL", isVN ? "Dead Ball" : Resources.Lang.lblDeadball));
            language.AppendLine(this.SetValue("LIVE", isVN ? "Live Ball" : Resources.Lang.lblLive));
            language.AppendLine(this.SetValue("DEAD_BALL_VN", Resources.Lang.lblDeadball));
            language.AppendLine(this.SetValue("LIVE_VN", Resources.Lang.lblLive));
            language.AppendLine(this.SetValue("EDIT", Resources.Lang.edit));
            language.AppendLine(this.SetValue("AGENT", Resources.Lang.agent));
            language.AppendLine(this.SetValue("INCORRECT_MIN_BET", Resources.Lang.alertIncorrectMinBet));
            language.AppendLine(this.SetValue("INCORRECT_MAX_BET", Resources.Lang.alertIncorrectMaxBet));
            language.AppendLine(this.SetValue("INCORRECT_MAX_PER_MATCH", Resources.Lang.alertIncorrectMaxPerMatch));
            language.AppendLine(this.SetValue("INCORRECT_MAX_PER_BALL", Resources.Lang.alertIncorrectMaxPerBall));
            language.AppendLine(this.SetValue("MIN_BET_GREATER_THAN_MIN_MIN_BET", string.Format("{0} {1}", Resources.Lang.lblalertMinbetCompare, "{0}")));
            language.AppendLine(this.SetValue("MAX_BET_LOWER_THAN_MAX_MAX_BET", string.Format("{0} {1}", Resources.Lang.lblalerMaxbetCompare, "{0}")));
            language.AppendLine(this.SetValue("MAX_PER_MATCH_LOWER_THAN_MAX_PER_MATCH_MAX_BET", Resources.Lang.lblalertMaxPerMatchCompare.Replace("{1}", "{0}")));
            language.AppendLine(this.SetValue("MAX_PER_BALL_LOWER_THAN_MAX_PER_BALL_MAX_BET", Resources.Lang.lblalertMaxPerBallCompare.Replace("{1}", "{0}")));
            language.AppendLine(this.SetValue("MIN_POSITION", Resources.Lang.labelMinPosition));
            language.AppendLine(this.SetValue("COPY_FROM", Resources.Lang.copyFrom));
            language.AppendLine(this.SetValue("OTHER_BET_TYPE_OF", Resources.Lang.otherBetTypesOf));
            language.AppendLine(this.SetValue("COPY_ALL_HINT", Resources.Lang.copyAllHint));
            language.AppendLine(this.SetValue("COPY_ALL_OTHER_POSITION_TAKING_FROM", Resources.Lang.copyAllPositionTakingFrom));
            language.AppendLine(this.SetValue("COPY_ALL_POSITION_TAKING_FROM", Resources.Lang.copyPositionTakingFrom));
            language.AppendLine(this.SetValue("COPY_ALL_BETTING_LIMIT_FROM", Resources.Lang.copyAllBetSettingFromExcludingNG));
            language.AppendLine(this.SetValue("MIN_BET_LOWER_THAN_MIN_MAX_BET", Resources.Lang.minBetShouldBeLessThanOrEqualToMaxBet));
            language.AppendLine(this.SetValue("MIN_BET_LOWER_THAN_MIN_MAX_PER_MATCH_BET", Resources.Lang.minBetShouldBeLessThanOrEqualToMaxPerMatch));
            language.AppendLine(this.SetValue("MIN_BET_LOWER_THAN_MIN_MAX_PER_BALL", Resources.Lang.minBetShouldBeLessThanOrEqualToMaxPerBall));
            language.AppendLine(this.SetValue("MAX_BET_LOWER_THAN_MIN_MAX_PER_MATCH_BET", Resources.Lang.maxBetShouldBeLessThanOrEqualToMaxPerMatch));
            
            // Set language to validation value on form.
            //IUserSession userSession = UserSession.Current;

            language.AppendLine(this.SetValue("YES", Resources.Lang.lblYes));
            language.AppendLine(this.SetValue("NO", Resources.Lang.lblNo));
            //language.AppendLine(this.SetValue("REQUIRE_VALIDATION_CODE", Resources.Lang.errRequireValidationCode));
            language.AppendLine(this.SetValue("BASKETBALL", Resources.Lang.Basketball));
            language.AppendLine(this.SetValue("SOCCER", Resources.Lang.soccer));
            language.AppendLine(this.SetValue("POSITION_SETTING_HEADER", Resources.Lang.lblPositionTaking));
            language.AppendLine(this.SetValue("BET_SETTING_HEADER", Resources.Lang.lblBetSetting));
            language.AppendLine(this.SetValue("COMMISSION_SETTING_HEADER", Resources.Lang.commission));
            language.AppendLine(this.SetValue("GROUP_A", Resources.Lang.lblGroupa));
            language.AppendLine(this.SetValue("GROUP_B", Resources.Lang.lblGroupb));
            language.AppendLine(this.SetValue("GROUP_C", Resources.Lang.lblGroupc));
            language.AppendLine(this.SetValue("GROUP_D", Resources.Lang.lblGroupd));
            language.AppendLine(this.SetValue("_1X2", Resources.Lang.lbl1x2));
            language.AppendLine(this.SetValue("OTHERS_COM", Resources.Lang.other));
            language.AppendLine(this.SetValue("NUMBER_GAME", Resources.Lang.numbergame));
            language.AppendLine(this.SetValue("ASIAN_HDC_OU_OE", Resources.Lang.lblAsianHDC_OU_OE));
            language.AppendLine(this.SetValue("MEMBER_GROUP", "Member Group"));
            language.AppendLine(this.SetValue("COMM_FOR_AGENT", "Comm for Agent"));
            language.AppendLine(this.SetValue("COMM_FOR_MEMBER", "Comm for Member"));
            language.AppendLine(this.SetValue("GROUP", Resources.Lang.group));
            
            return language.ToString();
        }

        private string SetValue(string key, string value)
        {
            return string.Format("{0} : \"{1}\",", key, value.Replace("\"", "\\\""));
        }
        
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}