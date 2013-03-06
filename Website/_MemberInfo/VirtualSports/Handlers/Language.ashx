<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.VirtualSports.Language_Handler" %>

namespace Sunplus.Agent.UI._MemberInfo.VirtualSports
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

            context.Response.ContentType = "text/plain";

            string language = this.SetLanguageAddNewMaster();
            context.Response.Write("(function (){\n");
            context.Response.Write(language);
            context.Response.Write("})();");
        }

        #region Set Language from Add New Master
        /// <summary>
        /// Set Language from Position Taking
        /// </summary>
        /// <returns></returns>
        private string SetLanguageAddNewMaster()
        {
            StringBuilder language = new StringBuilder();

            language.AppendLine(this.SetValue("VIRTUAL_SPORTS", Resources.Lang.VirtualSports));
            language.AppendLine(this.SetValue("MIN_BET", Resources.Lang.lblMinBet));
            language.AppendLine(this.SetValue("MAX_BET", Resources.Lang.lblMaxBet));
            language.AppendLine(this.SetValue("MAX_PER_MATCH", Resources.Lang.lblMaxPerMatch));
            language.AppendLine(this.SetValue("SUPER_PT", Resources.Lang.superPT));
            language.AppendLine(this.SetValue("MASTER_PT", Resources.Lang.masterPT));
            language.AppendLine(this.SetValue("AGENT_PT", Resources.Lang.agentPT));
            language.AppendLine(this.SetValue("IS_AUTO", Resources.Lang.isAuto));
            language.AppendLine(this.SetValue("MASTER_MAX_PT", Resources.Lang.isAuto));
            language.AppendLine(this.SetValue("MASTER_MIN_PT", Resources.Lang.isAuto));
            language.AppendLine(this.SetValue("AGENT_MAX_PT", Resources.Lang.isAuto));
            language.AppendLine(this.SetValue("AGENT_MIN_PT", Resources.Lang.isAuto));
            language.AppendLine(this.SetValue("MASTER_MAX_PT", Resources.Lang.masterMaxPT));
            language.AppendLine(this.SetValue("MASTER_MIN_PT", Resources.Lang.masterMinPT));
            language.AppendLine(this.SetValue("AGENT_MAX_PT", Resources.Lang.agentMaxPT));
            language.AppendLine(this.SetValue("AGENT_MIN_PT", Resources.Lang.agentMinPT));

            language.AppendLine(this.SetValue("NOINFO", Resources.Lang.noInfo));
            language.AppendLine(this.SetValue("BETTING_LIMIT", Resources.Lang.lblBetSetting));
            language.AppendLine(this.SetValue("INCORRECT_MIN_BET", Resources.Lang.alertIncorrectMinBet));
            language.AppendLine(this.SetValue("INCORRECT_MAX_BET", Resources.Lang.alertIncorrectMaxBet));
            language.AppendLine(this.SetValue("INCORRECT_MAX_PER_MATCH", Resources.Lang.alertIncorrectMaxPerMatch));
            language.AppendLine(this.SetValue("MIN_BET_GREATER_THAN_MIN_MIN_BET", string.Format("{0} {1}", Resources.Lang.lblalertMinbetCompare, "{0}")));
            language.AppendLine(this.SetValue("MAX_BET_LOWER_THAN_MAX_MAX_BET", string.Format("{0} {1}", Resources.Lang.lblalerMaxbetCompare, "{0}")));
            language.AppendLine(this.SetValue("MAX_PER_MATCH_LOWER_THAN_MAX_PER_MATCH_MAX_BET", Resources.Lang.lblalertMaxPerMatchCompare.Replace("{1}", "{0}")));
            language.AppendLine(this.SetValue("MIN_BET_LOWER_THAN_MIN_MAX_BET", Resources.Lang.minBetShouldBeLessThanOrEqualToMaxBet));
            language.AppendLine(this.SetValue("MIN_BET_LOWER_THAN_MIN_MAX_PER_MATCH_BET", Resources.Lang.minBetShouldBeLessThanOrEqualToMaxPerMatch));
            language.AppendLine(this.SetValue("MAX_BET_LOWER_THAN_MIN_MAX_PER_MATCH_BET", Resources.Lang.maxBetShouldBeLessThanOrEqualToMaxPerMatch));
            language.AppendLine(this.SetValue("CONFIRM_UPDATE_POSITION_TAKING", Resources.Lang.titleConfirmEditPT));
            language.AppendLine(this.SetValue("WANT_TO_UPDATE_ALL_MEMBER_POSITION_TAKING", Resources.Lang.msgUpdateAllMember));
            language.AppendLine(this.SetValue("CONFIRM_UPDATE_MEMBERS_PT", Resources.Lang.confirmUpdateMembers));
            language.AppendLine(this.SetValue("YES", Resources.Lang.lblYes));
            language.AppendLine(this.SetValue("NO", Resources.Lang.lblNo));
            language.AppendLine(this.SetValue("BASKETBALL", Resources.Lang.Basketball));
            language.AppendLine(this.SetValue("SOCCER", Resources.Lang.soccer));
            
            //language.AppendLine(this.SetValue("AGENT_POSITION", Resources.Lang.agent));
            //language.AppendLine(this.SetValue("MASTER_AUTO", Resources.Lang.lblMasterAuto));
            //language.AppendLine(this.SetValue("SUPER_AUTO", Resources.Lang.lblSuperAuto));
            //language.AppendLine(this.SetValue("DEAD_BALL", Resources.Lang.lblDeadball));
            //language.AppendLine(this.SetValue("LIVE", Resources.Lang.lblLive));
            //language.AppendLine(this.SetValue("EDIT", Resources.Lang.edit));
            //language.AppendLine(this.SetValue("SUPER", Resources.Lang.super));
            //language.AppendLine(this.SetValue("MASTER", Resources.Lang.master));
            //language.AppendLine(this.SetValue("AGENT", Resources.Lang.agent));
            //language.AppendLine(this.SetValue("MIN_POSITION", Resources.Lang.labelMinPosition));
            //language.AppendLine(this.SetValue("COPY_FROM", Resources.Lang.copyFrom));
            //language.AppendLine(this.SetValue("OTHER_BET_TYPE_OF", Resources.Lang.otherBetTypesOf));
            //language.AppendLine(this.SetValue("COPY_ALL_HINT", Resources.Lang.copyAllHint));
            //language.AppendLine(this.SetValue("COPY_ALL_OTHER_POSITION_TAKING_FROM", Resources.Lang.copyAllPositionTakingFrom));
            //language.AppendLine(this.SetValue("COPY_ALL_POSITION_TAKING_FROM", Resources.Lang.copyPositionTakingFrom));
            //language.AppendLine(this.SetValue("COPY_ALL_BETTING_LIMIT_FROM", Resources.Lang.copyAllBetSettingFromExcludingNG));
            
            //// Set language to validation value on form.
            //IUserSession userSession = UserSession.Current;
            //language.AppendLine(this.SetValue("ALERT_CREDIT", Resources.Lang.alertIncorrectAmaxcredit));
            //language.AppendLine(this.SetValue("ALERT_PASSWORD_REQUIRE", string.Format("<span class='nowrap'>{0}</span>", Resources.Lang.lblPasswordRequire)));
            //language.AppendLine(this.SetValue("ALERT_INCORRECT_MASTER_CREDIT", Resources.Lang.alertMasterCredit));
            //language.AppendLine(this.SetValue("ALERT_CHOOSE_DAILY", Resources.Lang.choonsedaily));
            //language.AppendLine(this.SetValue("ALERT_CHOOSE_ONE_DAY", Resources.Lang.chooseoneday));
            //language.AppendLine(this.SetValue("ALERT_TRANSFER_CONDITION_DOWNLINE", Resources.Lang.confirmtranscondition));

            //string creditInvalid = string.Empty, incorrectCredit = string.Empty;
            //if (userSession.IsSuper)
            //{
            //    creditInvalid = Resources.Lang.lblCreditInvalid.Replace("{0}", Resources.Lang.lblMasterMaxCredit);
            //    incorrectCredit = Resources.Lang.alertMasterCredit;
            //}
            //else if (userSession.IsMaster)
            //{
            //    creditInvalid = Resources.Lang.lblCreditInvalid.Replace("{0}", Resources.Lang.lblAgentMaxCredit);
            //    incorrectCredit = Resources.Lang.alertAgentCredit;
            //}
            //else if (userSession.IsAgent)
            //{
            //    creditInvalid = Resources.Lang.lblCreditInvalid.Replace("{0}", Resources.Lang.lblMemberMaxCredit);
            //    incorrectCredit = Resources.Lang.creditInvalid;
            //}

            //language.AppendLine(this.SetValue("ALERT_CREDIT_INCORRECT", incorrectCredit));
            //language.AppendLine(this.SetValue("ALERT_CREDIT_INVALID", creditInvalid));
            //language.AppendLine(this.SetValue("ALERT_INCORRECT_MEMBER_MAX_CREDIT", Resources.Lang.alertMemberCredit));
            //language.AppendLine(this.SetValue("ALERT_INVALID_MAX_CREDIT", string.Format(Resources.Lang.invalid, " " + Resources.Lang.lblMasterMaxCredit)));

            //string lblMemberCreditInvalid = Resources.Lang.lblCreditInvalid.Replace("{0}", Resources.Lang.lblMemberMaxCredit);
            //language.AppendLine(this.SetValue("ALERT_MEMBER_CREDIT_INVALID", lblMemberCreditInvalid));

            //// Confirm Status
            //language.AppendLine(this.SetValue("ALERT_CONFIRM_SUSPEND", Resources.Lang.confirmSuspendDownline));
            //language.AppendLine(this.SetValue("ALERT_CONFIRM_UNSUSPEND", Resources.Lang.confirmUnSuspended));
            //language.AppendLine(this.SetValue("ALERT_CONFIRM_CLOSE", Resources.Lang.confirmCloseDownline));
            //language.AppendLine(this.SetValue("ALERT_CONFIRM_OPEN", Resources.Lang.confirmCloseMem));

            //language.AppendLine(this.SetValue("ALERT_CONFIRM_SUSPEND_MEMBER", Resources.Lang.confirmSuspendMem));
            //language.AppendLine(this.SetValue("ALERT_CONFIRM_UNSUSPEND_MEMBER", Resources.Lang.confirmUnSuspendMem));
            //language.AppendLine(this.SetValue("ALERT_CONFIRM_CLOSE_MEMBER", Resources.Lang.confirmCloseMem));

            //language.AppendLine(this.SetValue("REQUIRE_VALIDATION_CODE", Resources.Lang.errRequireValidationCode));
            
            return language.ToString();
        }

        private string SetValue(string key, string value)
        {
            return string.Format("Language.{0} = \"{1}\";", key, value);
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