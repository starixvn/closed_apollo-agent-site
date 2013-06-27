// --------------------------------------------------------------------------
// <copyright file="CustomerSettingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.15">Created</change>
//   <change who="Quinn.Vo" date="2013.4.22">Add Action: AgentShare, MemberShare, GetBetType</change>
//   <change who="Ricky.Nguyen" date="2013.4.29">Update AgentId, Member Id, Shadow Account edit setting</change>
// </history>

namespace Apollo.Age.Account.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Account.Services;
    using Apollo.Age.Account.ViewModels;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Filters;
    using Fanex.Common;
    using Fanex.CustSetting.Models;
    using Fanex.CustSetting.Service;
    using Fanex.CustSetting.ViewData;
    using Newtonsoft.Json;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CustomerSettingController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerSettingService _customerSettingService;
        private readonly ISportSettingService _sportService;

        #region Ctors

        public CustomerSettingController()
            : this(null, null, null) 
        { 
        }

        public CustomerSettingController(
            IAccountService accountService, 
            ICustomerSettingService customerSettingService, 
            ISportSettingService sportService)
        {
            _accountService = accountService ?? new AccountService();
            _customerSettingService = customerSettingService ?? new CustomerSettingService();
            _sportService = sportService ?? new SportSettingService();
        }

        #endregion

        #region Bet Setting

        //public ActionResult EditBetSetting(int custId, string userName, int? agentLevel)
        //{
        //    ViewBag.CustId = custId;
        //    ViewBag.UserName = userName;
        //    ViewBag.AgentLevel = agentLevel;
        //    return View("~/Views/Account/EditBetSetting.cshtml");
        //}

        //public ActionResult GetBetLimit(int targetCustId, int? agentId, int? agentLevel)
        //{
        //    SettingContext settingContext = this.GetCurrentSettingContext(targetCustId, agentId, agentLevel, FormSettingType.BetLimit);
        //    IList<BettingLimit> betLimits = _customerSettingService.GetBettingLimitSettingForUpdating(settingContext);
        //    this.SetCurrentLanguage(base.CurrentSession.Language);
        //    this.SetBetLimitLanguage(settingContext, betLimits);

        //    JsonSerializerSettings jsonSetting = this.GetJsonSetting();
        //    string jsString = string.Format("var betLimits = {0} ;\n var settingContext = {1} ;",
        //        JsonConvert.SerializeObject(betLimits, jsonSetting),
        //        JsonConvert.SerializeObject(settingContext, jsonSetting));

        //    return JavaScript(jsString);
        //}

        //[HttpPost]
        //public ActionResult UpdateBetLimit(SettingContext settingContext, IList<BettingLimit> betLimits)
        //{
        //    Result result = _customerSettingService.UpdateBettingLimitSetting(settingContext, betLimits);
        //    return ReturnResult(result);
        //}

        #endregion

        #region Commission Setting

        /// <summary>
        /// Views commission from logged-in agent to selected (searched) agent/member
        /// </summary>
        /// <param name="id">id of selected agent/member</param>
        /// <param name="isMember">indicates whether the customer is agent or member</param>
        public ActionResult ViewCommission(ViewCommissionViewModel model)
        {
            if (model.Id == 0 && string.IsNullOrWhiteSpace(model.Username))
            {
                model.Id = this.CurrentSession.AgentId;
                model.Username = this.CurrentSession.AgentName;
            }
            else
            {
                if (model.Id == 0 && !string.IsNullOrWhiteSpace(model.Username))
                {
                    model.Id = -1; // force search by username only
                }
            }
            
            CustomerInfoModel customerInfo = model.IsMember
                ? _accountService.GetMemberInfo(new GetMemberInfoCriteria
                {
                    ActorAgentId = CurrentSession.AgentId,
                    CustId = model.Id,
                    Username = model.Username
                })
                : _accountService.GetAgentInfo(new GetAgentInfoCriteria
                {
                    ActorAgentId = CurrentSession.AgentId,
                    AgentId = model.Id,
                    Username = model.Username,
                    ExcludeActor = false
                });

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

        //public ActionResult EditCommissionSetting(int custId, string userName, int? agentLevel, string userGroup)
        //{
        //    ViewBag.CustId = custId;
        //    ViewBag.UserName = userName;
        //    ViewBag.AgentLevel = agentLevel;
        //    ViewBag.UserGroup = userGroup;
        //    return View("~/Views/Account/EditCommission.cshtml");
        //}

        //public ActionResult GetCommissionSetting(int targetCustId, int? agentId, int? agentLevel, string ugroup)
        //{
        //    SettingContext settingContext = this.GetCurrentSettingContext(targetCustId, agentId, agentLevel, FormSettingType.Commission);
        //    CommissionSetting commissionSetting = _customerSettingService.GetCommissionSettingForUpdating(settingContext);
        //    if (ugroup != null)
        //    {
        //        commissionSetting.UserGroup = ugroup;
        //    }

        //    JsonSerializerSettings jsonSetting = this.GetJsonSetting();
        //    string jsString = string.Format("var commissionSetting = {0} ;\n var settingContext = {1} ;",
        //        JsonConvert.SerializeObject(commissionSetting, jsonSetting),
        //        JsonConvert.SerializeObject(settingContext, jsonSetting));

        //    return JavaScript(jsString);
        //}

        //[HttpPost]
        //public ActionResult UpdateCommissionSetting(SettingContext settingContext, CommissionSetting commissionSetting)
        //{
        //    Result result = _customerSettingService.UpdateCommissionSetting(settingContext, commissionSetting);
        //    return ReturnResult(result);
        //}

        #endregion

        #region Share [Position Taking]

        /// <summary>
        /// Views "share info" from logged-in agent to selected (searched) agent
        /// </summary>
        /// <param name="username">selected agent username</param>
        public ActionResult AgentShare(int? agentId, string userName, short? sportType, short? betType, bool? isLive, bool? isMixSportParlay)
        {
            var sportId = sportType.HasValue ? sportType.Value : Converter.To<short>(1);
            var betTypeId = betType.HasValue ? betType.Value : Converter.To<short>(1);
            var live = isLive.HasValue ? isLive.Value : false;
            var isMixSport = isMixSportParlay.HasValue ? isMixSportParlay.Value : false;

            if (!agentId.HasValue && string.IsNullOrWhiteSpace(userName))
            {
                userName = this.CurrentSession.AgentName;
                agentId = this.CurrentSession.AgentId;
            }

            var sportList = _sportService.GetAllSportType();
            foreach (var sportItem in sportList)
            {
                sportItem.Name = base.GetResourceData(sportItem.ResourceKey);
            }

            if (SportSettingService.SportList == null)
            {   
                SportSettingService.SportList = sportList;
            }

            var betTypeList = _sportService.GetBetTypeBySport(sportId, isMixSport);
            foreach (var betTypeItem in betTypeList)
            {
                betTypeItem.Name = base.GetResourceData(betTypeItem.ResourceKey);
            }

            var agentShareViewModel = new AgentShareViewModel()
            {
                IsMember = false,
                Username = userName,
                SportType = sportId,
                BetType = betTypeId,
                IsLive = live,
                SportList = sportList,
                BetTypeList = betTypeList
            };

            AgentShareCriteria criteria = new AgentShareCriteria()
            {
                ActorAgentId = this.CurrentSession.AgentId,
                AgentId = agentId.Value,
                Username = userName,
                SportType = sportId,
                BetType = betTypeId,
                IsLive = live
            };

            var agentShareModelList = _accountService.GetAgentShare(criteria);
            agentShareViewModel.AgentShareModelList = agentShareModelList;

            return View("~/Views/Account/AgentShare.cshtml", agentShareViewModel);
        }

        /// <summary>
        /// Views "share info" from logged-in agent to selected (searched) member
        /// </summary>
        /// <param name="username">selected member username</param>
        public ActionResult MemberShare(int? memberId, string userName, short? sportType, short? betType, bool? isLive, bool? isMixSportParlay)
        {
            var sportId = sportType.HasValue ? sportType.Value : Converter.To<short>(1);
            var betTypeId = betType.HasValue ? betType.Value : Converter.To<short>(1);
            var live = isLive.HasValue ? isLive.Value : false;
            var isMixSport = isMixSportParlay.HasValue ? isMixSportParlay.Value : false;

            var sportList = _sportService.GetAllSportType();
            var betTypeList = _sportService.GetBetTypeBySport(sportId, isMixSport);

            foreach (var sportItem in sportList)
            {
                sportItem.Name = base.GetResourceData(sportItem.ResourceKey);
            }

            foreach (var betTypeItem in betTypeList)
            {
                betTypeItem.Name = base.GetResourceData(betTypeItem.ResourceKey);
            }

            var criteria = new MemberShareCriteria()
            {
                ActorAgentId = this.CurrentSession.AgentId,
                CustId = memberId.HasValue ? memberId.Value : -1,
                Username = userName,
                SportType = sportId,
                BetType = betTypeId,
                IsLive = live
            };

            var memberShareModelList = _accountService.GetMemberShare(criteria);

            var memberShareViewModel = new MemberShareViewModel()
            {
                IsMember = true,
                Username = userName,
                SportType = criteria.SportType,
                BetType = criteria.BetType,
                IsLive = criteria.IsLive,
                SportList = sportList,
                BetTypeList = betTypeList,
                MemberShareModelList = memberShareModelList
            };

            return View("~/Views/Account/MemberShare.cshtml", memberShareViewModel);
        }

        public JsonResult GetBetType(short sportId, bool isMixSportParlay)
        {
            var view = new AgentShareViewModel();

            var betTypeList = _sportService.GetBetTypeBySport(sportId, isMixSportParlay);

            foreach (var betTypeItem in betTypeList)
            {
                betTypeItem.Name = base.GetResourceData(betTypeItem.ResourceKey);
            }

            view.BetTypeList = betTypeList;

            return Json(view.BetTypeSelectListItem, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult EditPositionTaking(int custId, string userName, int? agentLevel)
        //{
        //    ViewBag.CustId = custId;
        //    ViewBag.UserName = userName;
        //    ViewBag.AgentLevel = agentLevel;
        //    return View("~/Views/Account/EditPositionTaking.cshtml");
        //}

        //public ActionResult GetPositionTaking(int targetCustId, int? agentId, int? agentLevel)
        //{
        //    SettingContext settingContext = this.GetCurrentSettingContext(targetCustId, agentId, agentLevel, FormSettingType.PositionTaking);
        //    PositionTakingSetting positionTakingSetting = _customerSettingService.GetPositionTakingSettingForUpdating(settingContext);
        //    this.SetCurrentLanguage(base.CurrentSession.Language);
        //    this.SetPositionTakingLanguage(positionTakingSetting);

        //    JsonSerializerSettings jsonSetting = this.GetJsonSetting();
        //    string jsString = string.Format("var positionTakingSetting = {0} ;\n var settingContext = {1} ;",
        //        JsonConvert.SerializeObject(positionTakingSetting, jsonSetting),
        //        JsonConvert.SerializeObject(settingContext, jsonSetting));

        //    return JavaScript(jsString);
        //}

        //[HttpPost]
        //public ActionResult UpdatePositionTaking(SettingContext settingContext, PositionTakingSetting positionTakingSetting)
        //{
        //    Result result = _customerSettingService.UpdatePositionTakingSetting(settingContext, positionTakingSetting);
        //    return ReturnResult(result);
        //}

        #endregion

        #region Setting

        public ActionResult EditAllSetting(int custId, string userName, int? agentId, int? agentLevel, string userGroup, bool showHeader = false)
        {
            ViewBag.CustId = custId;
            ViewBag.UserName = userName;
            ViewBag.AgentLevel = agentLevel;
            ViewBag.UserGroup = userGroup;
            ViewBag.CurentParentAgentId = agentId;
            ViewBag.ShowHeader = showHeader;
            return View("~/Views/Account/EditSetting.cshtml");
        }

        public ActionResult EditMemberSetting(int memberId, string userName, string userGroup, int? agentId, bool showHeader = false, bool isSimpleMode = true)
        {
            ViewBag.MemberId = memberId;
            ViewBag.UserName = userName;
            ViewBag.UserGroup = userGroup;
            ViewBag.CurentParentAgentId = agentId;
            ViewBag.IsSimpleMode = isSimpleMode;
            if (showHeader)
            {
                ViewBag.IsShowHeader = true;
            }
            ViewBag.IsMaster = base.CurrentSession.IsMaster;
            return View("~/Views/Account/EditMemberSetting.cshtml");
        }

        public ActionResult EditAgentSetting(int agentId, int agentLevel, string userName, bool showHeader = false, bool isSimpleMode = true)
        {
            ViewBag.AgentId = agentId;
            ViewBag.UserName = userName;
            ViewBag.AgentLevel = agentLevel;
            ViewBag.IsSimpleMode = isSimpleMode;
            if (showHeader)
            {
                ViewBag.IsShowHeader = true;
            }
            ViewBag.IsMaster = base.CurrentSession.IsMaster;
            return View("~/Views/Account/EditAgentSetting.cshtml");
        }

        public ActionResult GetMemberSetting(int memberId, int? agentId)
        {
            string jsString = this.GetSettingScript(memberId, agentId, null, _customerSettingService.GetMemberGroup(memberId));
            return JavaScript(jsString);
        }

        public ActionResult GetAgentSetting(int agentId, int agentLevel)
        {
            string jsString = this.GetSettingScript(agentId, null, (int?) agentLevel, null);
            return JavaScript(jsString);
        }

        [HttpPost, CustomValidateAntiForgeryToken]
        public ActionResult UpdateMemberSetting(int memberId, SettingContext settingContext, CommissionSetting commissionSetting, IList<BettingLimit> betLimits, PositionTakingSetting positionTakingSetting)
        {
            return UpdateAllSetting(settingContext, commissionSetting, betLimits, positionTakingSetting);
        }

        [HttpPost, CustomValidateAntiForgeryToken]
        public ActionResult UpdateAgentSetting(int agentId, SettingContext settingContext, CommissionSetting commissionSetting, IList<BettingLimit> betLimits, PositionTakingSetting positionTakingSetting)
        {
            return UpdateAllSetting(settingContext, commissionSetting, betLimits, positionTakingSetting);
        }

        public ActionResult MySetting()
        {
            ViewBag.CustId = CurrentSession.AgentId;
            ViewBag.UserName = CurrentSession.Username;
            ViewBag.AgentLevel = CurrentSession.AgentLevel;
            ViewBag.UserGroup = string.Empty;
            ViewBag.CurentParentAgentId = CurrentSession.AgentId;
            ViewBag.IsShowHeader = true;
            ViewBag.IsMaster = false;
            return View("~/Views/Account/MySetting.cshtml");
        }

        public ActionResult ViewAgentDetails(int agentId, int agentLevel, string userName, bool showHeader = false, bool isSimpleMode = true)
        {
            ViewBag.UserId = agentId;
            ViewBag.UserName = userName;
            ViewBag.IsViewOnMember = false;
            SettingContext settingContext = this.GetCurrentSettingContext(agentId, base.CurrentSession.AgentId, agentLevel, FormSettingType.All);
            return ViewAccountDetails(settingContext);
        }

        public ActionResult ViewMemberDetails(int memberId, string userName, int? agentId, bool showHeader = false, bool isSimpleMode = true)
        {
            ViewBag.UserId = memberId;
            ViewBag.UserName = userName;
            ViewBag.IsViewOnMember = true;
            SettingContext settingContext = this.GetCurrentSettingContext(memberId, agentId, null, FormSettingType.All);
            return ViewAccountDetails(settingContext);
        }

        private ViewResult ViewAccountDetails(SettingContext settingContext)
        {
            ViewBag.CommissionSetting = this.GetCommissionSetting(settingContext);
            ViewBag.BetSetting = this.GetBetSetting(settingContext);
            return View("~/Views/Account/ViewAccountDetails.cshtml");
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
            SettingContext settingContext = new SettingContext
            {
                CustId = base.CurrentSession.AgentId,
                TargetCustId = targetCustId,
                ActorCustId = base.CurrentSession.AgentId,
                RoleId = this.GetRoleFromAgentLevel(base.CurrentSession.AgentLevel),
                TargetRoleId = this.GetTargetRoleIdFromAgenLevel(agentLevel),
                FormSetting = (int)formSettingType,
                FormId = FormType.EditPT,
                Language = base.CurrentSession.Language
            };

            //// If current Cust Edited Role is Member, then set Edit Role is Agent
            if (settingContext.TargetRoleId == (int)Role.Member)
            {
                settingContext.RoleId = (int)Role.Agent;
            }

            //// If current Agent difference with Direct Upline Agent, then set Form to Edit Non Direct Member
            if (agentId != null && agentId.Value != base.CurrentSession.AgentId)
            {
                settingContext.FormId = FormType.EditMemberNonDirect;
            }

            //// View Current Agent Setting
            if (base.CurrentSession.AgentId == targetCustId && settingContext.TargetRoleId != (int)Role.Member)
            {
                settingContext.FormId = FormType.ViewAllSetting;
            }
            //// If Current Login Agent is not Agent Master User, then set form to form view
            else if (!base.CurrentSession.IsMaster)
            {
                settingContext.FormId = FormType.ShadowViewSetting;
                if (base.CurrentSession.IsSuper && settingContext.TargetRoleId == (int)Role.Member)
                {
                    settingContext.CustId = agentId.Value;
                }
            }
            else if (base.CurrentSession.IsSuper
            && settingContext.TargetRoleId == (int)Role.Member
            && agentId != null)
            {
                settingContext.FormId = FormType.EditPT;
                settingContext.CustId = agentId.Value;
            }

            return settingContext;
        }

        private string GetSettingScript(int targetCustId, int? agentId, int? agentLevel, string ugroup)
        {
            SettingContext settingContext = this.GetCurrentSettingContext(targetCustId, agentId, agentLevel, FormSettingType.All);

            CommissionSetting commissionSetting = _customerSettingService.GetCommissionSettingForUpdating(settingContext);
            if (ugroup != null)
            {
                commissionSetting.UserGroup = ugroup;
            }

            IList<BettingLimit> betLimits = _customerSettingService.GetBettingLimitSettingForUpdating(settingContext);
            this.SetCurrentLanguage(base.CurrentSession.Language);
            this.SetBetLimitLanguage(settingContext, betLimits);

            PositionTakingSetting positionTakingSetting = _customerSettingService.GetPositionTakingSettingForUpdating(settingContext);
            this.SetPositionTakingLanguage(positionTakingSetting);

            JsonSerializerSettings jsonSetting = this.GetJsonSetting();
            string jsString = string.Format("var commissionSetting = {0} ;\n var betLimits = {1} ;\n var positionTakingSetting = {2} ;\n var settingContext = {3} ;",
                JsonConvert.SerializeObject(commissionSetting, jsonSetting),
                JsonConvert.SerializeObject(betLimits, jsonSetting),
                JsonConvert.SerializeObject(positionTakingSetting, jsonSetting),
                JsonConvert.SerializeObject(settingContext, jsonSetting));

            return jsString;
        }

        private CommissionSetting GetCommissionSetting(SettingContext settingContext)
        {
            CommissionSetting commissionSetting = _customerSettingService.GetCommissionSettingForUpdating(settingContext);
            if (settingContext.TargetRoleId == (int)Role.Member)
            {
                commissionSetting.UserGroup = _customerSettingService.GetMemberGroup(settingContext.TargetCustId);
            }
            return commissionSetting;
        }

        private IList<BettingLimit> GetBetSetting(SettingContext settingContext)
        {
            IList<BettingLimit> betLimits = _customerSettingService.GetBettingLimitSettingForUpdating(settingContext);
            this.SetCurrentLanguage(base.CurrentSession.Language);
            this.SetBetLimitLanguage(settingContext, betLimits);

            return betLimits;
        }

        private PositionTakingSetting GetPositionTakingSetting(SettingContext settingContext)
        {
            PositionTakingSetting positionTakingSetting = _customerSettingService.GetPositionTakingSettingForUpdating(settingContext);
            this.SetPositionTakingLanguage(positionTakingSetting);
            return positionTakingSetting;
        }

        [NonAction]
        public ActionResult UpdateAllSetting(SettingContext settingContext, CommissionSetting commissionSetting, IList<BettingLimit> betLimits, PositionTakingSetting positionTakingSetting)
        {
            if (commissionSetting == null && betLimits == null && positionTakingSetting == null)
            {
                return ReturnResult(new Result { 
                    ErrCode = 0,
                    ErrMsg = Apollo.Age.Resources.Lang.noChangeToUpdate
                });
            }
            Result result = _customerSettingService.UpdateSetting(settingContext, commissionSetting, betLimits, positionTakingSetting);
            return ReturnResult(result);
        }

        #endregion
    }
}
