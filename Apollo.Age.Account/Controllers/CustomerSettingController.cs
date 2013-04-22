// --------------------------------------------------------------------------
// <copyright file="CustomerSettingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.15">Created</change>
// </history>


namespace Apollo.Age.Account.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Services;
    using Apollo.Age.Account.ViewModels;
    using Apollo.Age.Web;
    using Fanex.CustSetting.Models;
    using Fanex.CustSetting.Service;
    using Fanex.CustSetting.ViewData;
    using Newtonsoft.Json;

    public class CustomerSettingController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerSettingService _customerSettingService;

        #region Ctors

        public CustomerSettingController()
            : this(null, null) { }

        public CustomerSettingController(IAccountService accountService,
            ICustomerSettingService customerSettingService)
        {
            _accountService = accountService ?? new AccountService();
            _customerSettingService = customerSettingService ?? new CustomerSettingService();
        }

        #endregion

        #region Bet Setting

        public ActionResult EditBetSetting(int custId, string userName, int? agentLevel)
        {
            ViewBag.CustId = custId;
            ViewBag.UserName = userName;
            ViewBag.AgentLevel = agentLevel;
            return View("~/Views/Account/EditBetSetting.cshtml");
        }

        public ActionResult GetBetLimit(int targetCustId, int? agentId, int? agentLevel)
        {
            SettingContext settingContext = this.GetCurrentSettingContext(targetCustId, agentId, agentLevel, FormSettingType.BetLimit);
            IList<BettingLimit> betLimits = _customerSettingService.GetBettingLimitSettingForUpdating(settingContext);
            this.SetCurrentLanguage(base.CurrentSession.Language);
            this.SetBetLimitLanguage(settingContext, betLimits);

            JsonSerializerSettings jsonSetting = this.GetJsonSetting();
            string jsString = string.Format("var betLimits = {0} ;\n var settingContext = {1} ;",
                JsonConvert.SerializeObject(betLimits, jsonSetting),
                JsonConvert.SerializeObject(settingContext, jsonSetting));

            return JavaScript(jsString);
        }

        [HttpPost]
        public ActionResult UpdateBetLimit(SettingContext settingContext, IList<BettingLimit> betLimits)
        {
            Result result = _customerSettingService.UpdateBettingLimitSetting(settingContext, betLimits);
            return ReturnResult(result);
        }

        #endregion

        #region Commission Setting

        /// <summary>
        /// Views commission from logged-in agent to selected (searched) agent/member
        /// </summary>
        /// <param name="id">id of selected agent/member</param>
        /// <param name="isMember">indicates whether the customer is agent or member</param>
        public ActionResult ViewCommission(ViewCommissionViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Username))
            {
                // force search by username only
                model.Id = -1;
            }

            var customerInfo = _accountService
                .GetCustomerInfo(model.IsMember, model.Id, model.Username, CurrentSession.AgentId);

            if (customerInfo != null)
            {
                model.Username = customerInfo.Username;
                model.Group = customerInfo.Group;
                model.CommissionList = model.IsMember
                    ? _accountService.GetCommissionToMember(
                    new GetCommissionToMemberCriteria
                    {
                        ActorAgentId = CurrentSession.AgentId,
                        CustId = customerInfo.CustId,
                        Username = customerInfo.Username
                    })
                    : _accountService.GetCommissionToAgent(
                    new GetCommissionToAgentCriteria
                    {
                        ActorAgentId = CurrentSession.AgentId,
                        AgentId = customerInfo.CustId,
                        Username = customerInfo.Username
                    });
            }

            return View("~/Views/Account/ViewCommission.cshtml", model);
        }

        public ActionResult EditCommissionSetting(int custId, string userName, int? agentLevel, string userGroup)
        {
            ViewBag.CustId = custId;
            ViewBag.UserName = userName;
            ViewBag.AgentLevel = agentLevel;
            ViewBag.UserGroup = userGroup;
            return View("~/Views/Account/EditCommission.cshtml");
        }

        public ActionResult GetCommissionSetting(int targetCustId, int? agentId, int? agentLevel, string ugroup)
        {
            SettingContext settingContext = this.GetCurrentSettingContext(targetCustId, agentId, agentLevel, FormSettingType.Commission);
            CommissionSetting commissionSetting = _customerSettingService.GetCommissionSettingForUpdating(settingContext);
            if (ugroup != null)
            {
                commissionSetting.UserGroup = ugroup;
            }

            JsonSerializerSettings jsonSetting = this.GetJsonSetting();
            string jsString = string.Format("var commissionSetting = {0} ;\n var settingContext = {1} ;",
                JsonConvert.SerializeObject(commissionSetting, jsonSetting),
                JsonConvert.SerializeObject(settingContext, jsonSetting));

            return JavaScript(jsString);
        }

        [HttpPost]
        public ActionResult UpdateCommissionSetting(SettingContext settingContext, CommissionSetting commissionSetting)
        {
            Result result = _customerSettingService.UpdateCommissionSetting(settingContext, commissionSetting);
            return ReturnResult(result);
        }

        #endregion

        #region Share [Position Taking]

        /// <summary>
        /// Views "share info" from logged-in agent to selected (searched) agent/member
        /// </summary>
        /// <param name="username">selected agent/member username</param>
        public ActionResult AgentShare(int? agentId, string userName, short sportType, short betType, bool isLive)
        {
            var agentShareModelList = _accountService.GetAgentShare(
                this.CurrentSession.AgentId,
                agentId.HasValue ? agentId.Value : -1,
                userName,
                sportType,
                betType,
                isLive);

            var sportList = _accountService.GetAllSportType();
            var betTypeList = _accountService.GetBetTypeBySport(sportType);

            var agentShareViewModel = new AgentShareViewModel()
            {
                IsMember = false,
                Username = userName,
                SportType = sportType,
                BetType = betType,
                SportList = sportList,
                BetTypeList = betTypeList,
                AgentShareModelList = agentShareModelList
            };

            return View("~/Views/Account/AgentShare.cshtml", agentShareViewModel);
        }

        public ActionResult MemberShare(int? memberId, string userName, short sportType, short betType, bool isLive)
        {
            var memberShareModelList = _accountService.GetMemberShare(
                this.CurrentSession.AgentId,
                memberId.HasValue ? memberId.Value : -1,
                userName,
                sportType,
                betType,
                isLive);

            var sportList = _accountService.GetAllSportType();
            var betTypeList = _accountService.GetBetTypeBySport(sportType);

            var memberShareViewModel = new MemberShareViewModel()
            {
                IsMember = true,
                Username = userName,
                SportType = sportType,
                BetType = betType,
                SportList = sportList,
                BetTypeList = betTypeList,
                MemberShareModelList = memberShareModelList
            };

            return View("~/Views/Account/MemberShare.cshtml", memberShareViewModel);
        }

        public ActionResult ViewShare(string username)
        {
            ViewBag.UserId = CurrentSession.UserId;
            ViewBag.AgentLevel = CurrentSession.AgentLevel;

            return View("~/Views/Account/ViewShare.cshtml");
        }

        public ActionResult EditPositionTaking(int custId, string userName, int? agentLevel)
        {
            ViewBag.CustId = custId;
            ViewBag.UserName = userName;
            ViewBag.AgentLevel = agentLevel;
            return View("~/Views/Account/EditPositionTaking.cshtml");
        }

        public ActionResult GetPositionTaking(int targetCustId, int? agentId, int? agentLevel)
        {
            SettingContext settingContext = this.GetCurrentSettingContext(targetCustId, agentId, agentLevel, FormSettingType.PositionTaking);
            PositionTakingSetting positionTakingSetting = _customerSettingService.GetPositionTakingSettingForUpdating(settingContext);
            this.SetCurrentLanguage(base.CurrentSession.Language);
            this.SetPositionTakingLanguage(positionTakingSetting);

            JsonSerializerSettings jsonSetting = this.GetJsonSetting();
            string jsString = string.Format("var positionTakingSetting = {0} ;\n var settingContext = {1} ;",
                JsonConvert.SerializeObject(positionTakingSetting, jsonSetting),
                JsonConvert.SerializeObject(settingContext, jsonSetting));

            return JavaScript(jsString);
        }

        [HttpPost]
        public ActionResult UpdatePositionTaking(SettingContext settingContext, PositionTakingSetting positionTakingSetting)
        {
            Result result = _customerSettingService.UpdatePositionTakingSetting(settingContext, positionTakingSetting);
            return ReturnResult(result);
        }

        #endregion

        #region Setting

        public ActionResult EditAllSetting(int custId, string userName, int? agentId, int? agentLevel, string userGroup)
        {
            ViewBag.CustId = custId;
            ViewBag.UserName = userName;
            ViewBag.AgentLevel = agentLevel;
            ViewBag.UserGroup = userGroup;
            ViewBag.CurentParentAgentId = agentId;
            return View("~/Views/Account/EditSetting.cshtml");
        }

        [HttpPost]
        public ActionResult UpdateAllSetting(SettingContext settingContext, CommissionSetting commissionSetting, IList<BettingLimit> betLimits, PositionTakingSetting positionTakingSetting)
        {
            Result result = _customerSettingService.UpdateSetting(settingContext, commissionSetting, betLimits, positionTakingSetting);
            return ReturnResult(result);
        }

        #endregion

        #region Helpers

        private int GetRoleFromAgentLevel(int agentLevel)
        {
            switch (agentLevel)
            {
                case 0:
                    return (int)Role.Super;
                case 1:
                    return (int)Role.MainShareHolder;
                default:
                    return (int)Role.Agent;
            }
        }

        private void SetBetLimitLanguage(SettingContext settingContext, IList<BettingLimit> betLimits)
        {
            foreach (Fanex.CustSetting.ViewData.BettingLimit bettingLimitItem in betLimits)
            {
                bettingLimitItem.Name = base.GetResourceData(bettingLimitItem.ResourceKey);
            }
        }

        private void SetCurrentLanguage(string language)
        {
            CultureInfo def = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentCulture.DateTimeFormat = def.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = def.NumberFormat;
        }

        private JsonSerializerSettings GetJsonSetting()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            return settings;
        }

        private int GetTargetRoleIdFromAgenLevel(int? agentLevel)
        {
            if (agentLevel != null)
            {
                return this.GetRoleFromAgentLevel(agentLevel.Value);
            }
            else
            {
                return (int)Role.Member;
            }
        }

        private void SetPositionTakingLanguage(PositionTakingSetting positionTakingSetting)
        {
            // Set language for SportType Name on SportTypes
            string betTypeName = string.Empty;
            string bettypeTitle = string.Empty;
            foreach (Fanex.CustSetting.ViewData.SportType sportTypeItem in positionTakingSetting.SportTypes)
            {
                sportTypeItem.Name = this.GetResourceData(sportTypeItem.ResourceKey);
                foreach (Fanex.CustSetting.ViewData.BetTypeGroup betTypeGroupItem in sportTypeItem.BetTypeGroups)
                {
                    foreach (Fanex.CustSetting.ViewData.BetType betTypeItem in betTypeGroupItem.BetTypes)
                    {
                        betTypeName = bettypeTitle = base.GetResourceData(betTypeItem.ResourceKey);
                        betTypeItem.Name = betTypeName;
                        betTypeItem.Title = bettypeTitle;
                    }
                }
            }
        }

        private JsonResult ReturnResult(Result result)
        {
            if (result.ErrCode != 0 && result.ErrCode != Fanex.CustSetting.Core.Constant.WARNING_CODE_DEFAULT)
            {
                return Json(new Result(result.ErrCode, result.ErrMsg));
            }
            else
            {
                return Json(new Result(0, Resources.Lang.updatesuccessfull));
            }
        }

        private SettingContext GetCurrentSettingContext(int targetCustId, int? agentId, int? agentLevel, FormSettingType formSettingType)
        {
            SettingContext settingContex = new SettingContext
            {
                CustId = base.CurrentSession.AgentId,
                TargetCustId = targetCustId,
                ActorCustId = base.CurrentSession.AgentId,
                RoleId = this.GetRoleFromAgentLevel(base.CurrentSession.AgentLevel),
                TargetRoleId = this.GetTargetRoleIdFromAgenLevel(agentLevel),
                FormSetting = (int)formSettingType,
                FormId = FormType.EditPT
            };

            if (settingContex.TargetRoleId == (int)Role.Member)
            {
                settingContex.RoleId = (int)Role.Agent;
            }

            if (agentId != null && agentId.Value != base.CurrentSession.AgentId)
            {
                settingContex.FormId = FormType.EditMemberNonDirect;
            }

            return settingContex;
        }


        #endregion
    }
}
