// --------------------------------------------------------------------------
// <copyright file="CustomerSettingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.15">Created</change>
//   <change who="Quinn.Vo" date="2013.4.22">Add Action: AgentShare, MemberShare, GetBetType</change>
//   <change who="Ricky.Nguyen" date="2013.4.29">Update AgentId, Member Id, Shadow Account edit setting</change>
//   <change who="Ricky.Nguyen" date="2013.7.1">Updated Edit Share Setting, Commission Setting, Bet Limit with Singbet requirements</change>
// </history>

namespace Apollo.Age.Account.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Entities;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Account.Services;
    using Apollo.Age.Account.ViewModels;
    using Apollo.Age.Core;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Filters;
    using Apollo.Age.Web.Web.Attribute;
    using Fanex.Common;
    using Apollo.CustSetting.Models;
    using Apollo.CustSetting.Service;
    using Apollo.CustSetting.ViewData;
    using Newtonsoft.Json;

    /// <summary>
    /// Customer Setting Controller.
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
    public class CustomerSettingController : BaseController
    {
        private static int _maxBetLimit = 0;
        private readonly IAccountService _accountService;
        private readonly ICustomerSettingService _customerSettingService;
        private readonly ISportSettingService _sportService;
        private readonly IUserService _userService;

        #region Ctors

        public CustomerSettingController()
            : this(null, null, null, null)
        {
        }

        public CustomerSettingController(
            IAccountService accountService,
            ICustomerSettingService customerSettingService,
            ISportSettingService sportService,
            IUserService userService)
        {
            _accountService = accountService ?? new AccountService();
            _customerSettingService = customerSettingService ?? new CustomerSettingService();
            _sportService = sportService ?? new SportSettingService();
            _userService = userService ?? new UserService();
        }

        #endregion Ctors

        #region Commission Setting

        /// <summary>
        /// Views commission from logged-in agent to selected (searched) agent/member.
        /// </summary>
        /// <param name="model">Username: text which user input to search, IsMember: is agent or member.</param>
        /// <returns>View Commission.</returns>
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Query)]
        public ActionResult ViewCommission(ViewCommissionViewModel model)
        {
            var hasDownlineDetailsPermission = _userService.GetUserPolicy(this.CurrentSession.UserId, ApolloFunctionKeys.DownlineDetails) >= (byte)Policy.Query;

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
                : _accountService.GetAgentInfo(new GetAgentAllInfoCriteria
                {
                    ActorAgentId = CurrentSession.AgentId,
                    AgentId = model.Id,
                    Username = model.Username,
                    ExcludeActor = false
                });

            if (customerInfo != null)
            {
                model.Username = customerInfo.Username;
                model.Group = customerInfo.Group != null ? RevertCommGroupName(customerInfo.Group) : customerInfo.Group;

                IEnumerable<CustomerCommissionModel> commissionList = null;

                if (model.IsMember)
                {
                    // Only search if have downline details permission
                    if (hasDownlineDetailsPermission)
                    {
                        commissionList = _accountService.GetCommissionToMember(
                            new GetCommissionToMemberCriteria
                            {
                                ActorAgentId = CurrentSession.AgentId,
                                CustId = customerInfo.CustId,
                                Username = customerInfo.Username
                            });
                    }
                }
                else
                {
                    /* Only search himself if not have downline details permission
                     */
                    if (string.IsNullOrWhiteSpace(customerInfo.Username)
                       || string.Equals(customerInfo.Username, this.CurrentSession.AgentName)
                       || hasDownlineDetailsPermission)
                    {
                        commissionList = _accountService.GetCommissionToAgent(
                            new GetCommissionToAgentCriteria
                            {
                                ActorAgentId = CurrentSession.AgentId,
                                AgentId = customerInfo.CustId,
                                Username = customerInfo.Username
                            });
                    }
                }

                var commissionModelList = new List<CustomerCommissionModel>();
                if (commissionList != null && commissionList.Any())
                {
                    commissionList.ToList().ForEach(t => commissionModelList.Add(RevertCommGroupValue(t)));
                }

                model.CommissionList = commissionModelList;
            }

            return View("~/Views/Account/ViewCommission.cshtml", model);
        }

        #endregion Commission Setting

        #region Share [Position Taking]

        /// <summary>
        /// Views "share info" from logged-in agent to selected (searched) agent.
        /// </summary>
        /// <param name="agentId">The agent ID.</param>
        /// <param name="userName">Selected agent username.</param>
        /// <param name="sportType">The sport type.</param>
        /// <param name="betType">The bet type.</param>
        /// <param name="isLive">Is live or not.</param>
        /// <param name="isMixSportParlay">Is mix sport parlay or not.</param>
        /// <returns>Action result.</returns>
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Query)]
        public ActionResult AgentShare(
            int? agentId,
            string userName,
            short? sportType,
            short? betType,
            bool? isLive,
            bool? isMixSportParlay)
        {
            var sportId = sportType.HasValue ? sportType.Value : Converter.To<short>(1);
            var betTypeId = betType.HasValue ? betType.Value : Converter.To<short>(1);
            var live = isLive.HasValue ? isLive.Value : false;
            var isMixSport = isMixSportParlay.HasValue ? isMixSportParlay.Value : false;
            List<SportSetting> sportList = null;

            if (!agentId.HasValue && string.IsNullOrWhiteSpace(userName))
            {
                userName = this.CurrentSession.AgentName;
                agentId = this.CurrentSession.AgentId;
            }

            if (SportSettingService.SportList == null)
            {
                sportList = _sportService.GetAllSportType();
                SportSettingService.SportList = sportList;
            }
            else
            {
                sportList = SportSettingService.SportList;
            }

            foreach (var sportItem in sportList)
            {
                sportItem.Name = GetResourceData(sportItem.ResourceKey);
            }

            var betTypeList = _sportService.GetBetTypeBySport(sportId, isMixSport);
            foreach (var betTypeItem in betTypeList)
            {
                betTypeItem.Name = GetResourceData(betTypeItem.ResourceKey);
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

            var hasDownlineDetailsPermission = _userService.GetUserPolicy(this.CurrentSession.UserId, ApolloFunctionKeys.DownlineDetails) >= (byte)Policy.Query;

            if (!string.IsNullOrWhiteSpace(userName)
                && !string.Equals(userName, this.CurrentSession.AgentName)
                && !hasDownlineDetailsPermission)
            {
                return View("~/Views/Account/AgentShare.cshtml", agentShareViewModel);
            }

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
        /// Views "share info" from logged-in agent to selected (searched) member.
        /// </summary>
        /// <param name="memberId">The member ID.</param>
        /// <param name="userName">Selected member username.</param>
        /// <param name="sportType">The sport type.</param>
        /// <param name="betType">The bet type.</param>
        /// <param name="isLive">Is live or not.</param>
        /// <param name="isMixSportParlay">Is mix sport parlay or not.</param>
        /// <returns>Action result.</returns>
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Query)]
        public ActionResult MemberShare(
            int? memberId,
            string userName,
            short? sportType,
            short? betType,
            bool? isLive,
            bool? isMixSportParlay)
        {
            var sportId = sportType.HasValue ? sportType.Value : Converter.To<short>(1);
            var betTypeId = betType.HasValue ? betType.Value : Converter.To<short>(1);
            var live = isLive.HasValue ? isLive.Value : false;
            var isMixSport = isMixSportParlay.HasValue ? isMixSportParlay.Value : false;
            List<SportSetting> sportList = null;

            if (SportSettingService.SportList == null)
            {
                sportList = _sportService.GetAllSportType();
                SportSettingService.SportList = sportList;
            }
            else
            {
                sportList = SportSettingService.SportList;
            }

            foreach (var sportItem in sportList)
            {
                sportItem.Name = GetResourceData(sportItem.ResourceKey);
            }

            var betTypeList = _sportService.GetBetTypeBySport(sportId, isMixSport);

            foreach (var betTypeItem in betTypeList)
            {
                betTypeItem.Name = GetResourceData(betTypeItem.ResourceKey);
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

            var memberShareViewModel = new MemberShareViewModel()
            {
                IsMember = true,
                Username = userName,
                SportType = criteria.SportType,
                BetType = criteria.BetType,
                IsLive = criteria.IsLive,
                SportList = sportList,
                BetTypeList = betTypeList
            };

            var hasDownlineDetailsPermission = _userService.GetUserPolicy(this.CurrentSession.UserId, ApolloFunctionKeys.DownlineDetails) >= (byte)Policy.Query;

            if (!string.IsNullOrWhiteSpace(userName)
                && !string.Equals(userName, this.CurrentSession.AgentName)
                && !hasDownlineDetailsPermission)
            {
                return View("~/Views/Account/MemberShare.cshtml", memberShareViewModel);
            }

            var memberShareModelList = _accountService.GetMemberShare(criteria);

            memberShareViewModel.MemberShareModelList = memberShareModelList;

            return View("~/Views/Account/MemberShare.cshtml", memberShareViewModel);
        }

        /// <summary>
        /// Get bet type by sport.
        /// </summary>
        /// <param name="sportId">Parameter sport id.</param>
        /// <param name="isMixSportParlay">Is mix sport parlay or no.</param>
        /// <returns>Bet type list.</returns>
        public JsonResult GetBetType(short sportId, bool isMixSportParlay)
        {
            var view = new AgentShareViewModel();

            var betTypeList = _sportService.GetBetTypeBySport(sportId, isMixSportParlay);

            foreach (var betTypeItem in betTypeList)
            {
                betTypeItem.Name = GetResourceData(betTypeItem.ResourceKey);
            }

            view.BetTypeList = betTypeList;

            return Json(view.BetTypeSelectListItem, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Edit Agent Position Taking.
        /// </summary>
        /// <param name="agentId">Agent ID need be edited.</param>
        /// <param name="userName">Agent username.</param>
        /// <param name="agentLevel">Agent level.</param>
        /// <param name="reload">After edit, reload page or not.</param>
        /// <param name="isSimpleMode">Current mode is simple mode or not.</param>
        /// <param name="backLink">Back link for after editing.</param>
        /// <param name="fromLeftMenu">Link from left menu or not.</param>
        /// <param name="parentUsername">The parent username.</param>
        /// <returns>Action result.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Query)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        public ActionResult EditAgentPositionTaking(
            int agentId,
            string userName,
            int? agentLevel,
            bool reload = false,
            bool isSimpleMode = true,
            string backLink = "",
            bool fromLeftMenu = false,
            string parentUsername = "")
        {
            var custSettingPolicy = _userService.GetUserPolicy(CurrentSession.UserId, ApolloFunctionKeys.CustomerSetting);

            ViewBag.CustId = agentId;
            ViewBag.UserName = userName;
            ViewBag.AgentLevel = agentLevel;
            ViewBag.Reload = reload;
            ViewBag.BackLink = backLink;
            ViewBag.FromLeftMenu = fromLeftMenu;
            ViewBag.IsEditAgent = true;
            ViewBag.IsSimpleMode = isSimpleMode;
            ViewBag.IsMaster = CurrentSession.IsMaster && custSettingPolicy == (byte)Policy.Edit;
            ViewBag.ParentUsername = parentUsername;
            ViewBag.AllowEdit = agentLevel != null ? !_accountService.IsInPeakHours(agentLevel.Value) : true;

            return View("~/Views/Account/EditPositionTaking.cshtml");
        }

        [HttpPost]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Edit)]
        public ActionResult CalculateUpdateAgentPositionTakingTime(SettingContext settingContext, PositionTakingSetting positionTakingSetting)
        {
            var totalSeconds = _accountService.CalculateUpdateAgentPositionTakingTime(settingContext, positionTakingSetting);
            return Json(totalSeconds);
        }

        /// <summary>
        /// Edit all member share of agent.
        /// </summary>
        /// <param name="agentId">The agent ID.</param>
        /// <param name="agentUserName">The agent username.</param>
        /// <param name="reload">Reload or not.</param>
        /// <param name="isSimpleMode">Simple mode or not.</param>
        /// <param name="backLink">The back link URL.</param>
        /// <param name="fromLeftMenu">From left menu or not.</param>
        /// <returns>Action result.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Query)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        public ActionResult EditAllMemberShare(
            int agentId,
            string agentUserName,
            bool reload = false,
            bool isSimpleMode = true,
            string backLink = "",
            bool fromLeftMenu = false)
        {
            if (_accountService.HasMember(agentId))
            {
                return EditMemberPositionTaking(
                    agentId,
                    agentUserName,
                    reload,
                    isSimpleMode,
                    backLink,
                    fromLeftMenu,
                    string.Empty);
            }

            return View("~/Views/Account/EditAllMemberShare.cshtml");
        }

        /// <summary>
        /// Edit member position taking.
        /// </summary>
        /// <param name="agentId">Cross edit agent.</param>
        /// <param name="agentUsername">Agent user name.</param>
        /// <param name="reload">Reload page or not.</param>
        /// <param name="isSimpleMode">Current mode is simple mode or not.</param>
        /// <param name="backLink">Back link after editing.</param>
        /// <param name="fromLeftMenu">From left menu.</param>
        /// <param name="memberIds">List of member ID.</param>
        /// <returns>Action result.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Query)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        public ActionResult EditMemberPositionTaking(
            int agentId,
            string agentUsername = "",
            bool reload = false,
            bool isSimpleMode = true,
            string backLink = "",
            bool fromLeftMenu = false,
            string memberIds = "")
        {
            var custSettingPolicy = _userService.GetUserPolicy(CurrentSession.UserId, ApolloFunctionKeys.CustomerSetting);

            ViewBag.CustId = 0;
            ViewBag.UserName = string.Empty;
            ViewBag.AgentId = agentId;
            ViewBag.BackLink = backLink;
            ViewBag.FromLeftMenu = fromLeftMenu;
            ViewBag.Reload = reload;
            ViewBag.IsSimpleMode = isSimpleMode;
            ViewBag.IsEditAgent = false;
            ViewBag.IsMaster = CurrentSession.IsMaster && custSettingPolicy == (byte)Policy.Edit;
            ViewBag.AgentUsername = agentUsername;
            ViewBag.MemberIds = memberIds;

            return View("~/Views/Account/EditPositionTaking.cshtml");
        }

        /// <summary>
        /// Get Position taking result.
        /// </summary>
        /// <param name="targetCustId">Target customer id is member or agent.</param>
        /// <param name="agentId">Agent id for edit cross.</param>
        /// <param name="agentLevel">Agent level.</param>
        /// <param name="targetCustIds">List of target customer ID.</param>
        /// <returns>Action result.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Query)]
        public ActionResult GetPositionTaking(
           int targetCustId,
           int? agentId,
           int? agentLevel,
           string targetCustIds = "")
        {
            SettingContext settingContext = this.GetCurrentSettingContext(targetCustId, agentId, agentLevel, FormSettingType.PositionTaking, targetCustIds);
            PositionTakingSetting positionTakingSetting = _customerSettingService.GetPositionTakingSettingForUpdating(settingContext);
            this.SetCurrentLanguage(CurrentSession.Language);
            this.SetPositionTakingLanguage(positionTakingSetting);

            JsonSerializerSettings jsonSetting = this.GetJsonSetting();
            string javascriptString = string.Format(
                "var positionTakingSetting = {0} ;\n var settingContext = {1} ;",
                JsonConvert.SerializeObject(positionTakingSetting, jsonSetting),
                JsonConvert.SerializeObject(settingContext, jsonSetting));

            return JavaScript(javascriptString);
        }

        /// <summary>
        /// Update position taking setting.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="positionTakingSetting">Position taking update.</param>
        /// <returns>Action result.</returns>
        [HttpPost, AllowUpdateChecking]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Edit)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        public ActionResult UpdatePositionTaking(SettingContext settingContext, PositionTakingSetting positionTakingSetting)
        {
            Result result = _customerSettingService.UpdatePositionTakingSetting(settingContext, positionTakingSetting);
            return ReturnResult(result);
        }

        #endregion Share [Position Taking]

        #region Setting

        /// <summary>
        /// Edit All Setting of customer.
        /// </summary>
        /// <param name="custId">Customer ID need be edited.</param>
        /// <param name="userName">User name.</param>
        /// <param name="agentId">Agent id for edit cross level.</param>
        /// <param name="agentLevel">Agent level.</param>
        /// <param name="userGroup">User group for edit member.</param>
        /// <param name="showHeader">Show header or not.</param>
        /// <returns>Action result.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Edit)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        public ActionResult EditAllSetting(
            int custId,
            string userName,
            int? agentId,
            int? agentLevel,
            string userGroup,
            bool showHeader = false)
        {
            ViewBag.CustId = custId;
            ViewBag.UserName = userName;
            ViewBag.AgentLevel = agentLevel;
            ViewBag.UserGroup = userGroup;
            ViewBag.CurentParentAgentId = agentId;
            ViewBag.ShowHeader = showHeader;
            return View("~/Views/Account/EditSetting.cshtml");
        }

        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Edit)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        public ActionResult EditMemberSetting(
            int memberId,
            string userName,
            int? agentId,
            bool fromLeftMenu = false,
            string memberIds = "")
        {
            ViewBag.MemberId = memberId;
            ViewBag.UserName = userName;
            ViewBag.CurentParentAgentId = agentId;
            ViewBag.IsMaster = CurrentSession.IsMaster;
            ViewBag.FromLeftMenu = fromLeftMenu;
            ViewBag.MemberIds = memberIds;
            ViewBag.MaxBetLimit = _maxBetLimit;
            return View("~/Views/Account/EditMemberSetting.cshtml");
        }

        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Edit)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        public ActionResult EditAgentSetting(
            int agentId,
            int agentLevel,
            string userName,
            bool fromLeftMenu = false)
        {
            ViewBag.AgentId = agentId;
            ViewBag.UserName = userName;
            ViewBag.AgentLevel = agentLevel;
            ViewBag.IsMaster = CurrentSession.IsMaster;
            ViewBag.FromLeftMenu = fromLeftMenu;
            ViewBag.MaxBetLimit = _maxBetLimit;

            return View("~/Views/Account/EditAgentSetting.cshtml");
        }

        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Query)]
        public ActionResult GetMemberSetting(int memberId, int? agentId, string memberIds = "")
        {
            var memberInfo = _customerSettingService.GetMemberInfo(memberId);
            string javascriptString = this.GetSettingScript(memberId, agentId, null, memberInfo != null ? memberInfo.UGroup : string.Empty, memberIds);
            return JavaScript(javascriptString);
        }

        public ActionResult GetAgentSetting(int agentId, int agentLevel)
        {
            string javascriptString = this.GetSettingScript(agentId, null, (int?)agentLevel, null, null);
            return JavaScript(javascriptString);
        }

        [HttpPost, AllowUpdateChecking]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Edit)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        public ActionResult UpdateMemberSetting(int memberId, SettingContext settingContext, CommissionSetting commissionSetting, IList<BettingLimit> betLimits)
        {
            return UpdateAllSetting(settingContext, commissionSetting, betLimits);
        }

        [HttpPost, AllowUpdateChecking]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Edit)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        public ActionResult UpdateAgentSetting(int agentId, SettingContext settingContext, CommissionSetting commissionSetting, IList<BettingLimit> betLimits)
        {
            return UpdateAllSetting(settingContext, commissionSetting, betLimits);
        }

        /// <summary>
        /// Get my setting.
        /// </summary>
        /// <returns>Action result.</returns>
        public ActionResult MySetting()
        {
            ViewBag.CustId = CurrentSession.AgentId;
            ViewBag.UserName = CurrentSession.AgentName;
            ViewBag.AgentLevel = CurrentSession.AgentLevel;
            ViewBag.IsViewOnMember = false;
            ViewBag.CanEdit = CurrentSession.IsMaster;
            ViewBag.IsViewMySetting = true;
            SettingContext settingContext = this.GetCurrentSettingContext(
                CurrentSession.AgentId,
                CurrentSession.AgentId,
                CurrentSession.AgentLevel,
                FormSettingType.All,
                string.Empty);

            return ViewAccountDetails(settingContext);
        }

        /// <summary>
        /// View agent details.
        /// </summary>
        /// <param name="agentId">The agent ID.</param>
        /// <param name="agentLevel">The agent level.</param>
        /// <param name="userName">The username.</param>
        /// <param name="backLink">The back link URL.</param>
        /// <param name="fromLeftMenu">From left menu or not.</param>
        /// <returns>Action result.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Query)]
        public ActionResult ViewAgentDetails(
            int agentId,
            int agentLevel,
            string userName,
            string backLink = "",
            bool fromLeftMenu = false)
        {
            var custSettingPolicy = _userService.GetUserPolicy(CurrentSession.UserId, ApolloFunctionKeys.CustomerSetting);
            if (!PermissionDownlineDetail(agentId))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            ViewBag.CustId = agentId;
            ViewBag.UserName = userName;
            ViewBag.AgentLevel = agentLevel;
            ViewBag.IsViewOnMember = false;
            ViewBag.CanEdit = CurrentSession.IsMaster && custSettingPolicy == (byte)Policy.Edit;
            ViewBag.IsViewMySetting = CurrentSession.AgentId == agentId;
            ViewBag.BackLink = backLink;
            ViewBag.FromLeftMenu = fromLeftMenu;
            SettingContext settingContext = this.GetCurrentSettingContext(agentId, CurrentSession.AgentId, agentLevel, FormSettingType.All, string.Empty);

            return ViewAccountDetails(settingContext);
        }

        /// <summary>
        /// View member details.
        /// </summary>
        /// <param name="memberId">The member ID.</param>
        /// <param name="userName">The username.</param>
        /// <param name="agentId">The agent ID.</param>
        /// <param name="backLink">The back link URL.</param>
        /// <param name="fromLeftMenu">From left menu or not.</param>
        /// <param name="memberIds">List of member ID.</param>
        /// <returns>Action result.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Query)]
        public ActionResult ViewMemberDetails(
            int memberId,
            string userName,
            int? agentId,
            string backLink = "",
            bool fromLeftMenu = false,
            string memberIds = "")
        {
            var custSettingPolicy = _userService.GetUserPolicy(CurrentSession.UserId, ApolloFunctionKeys.CustomerSetting);

            ViewBag.CustId = memberId;
            ViewBag.UserName = userName;
            ViewBag.AgentId = agentId;
            ViewBag.IsViewOnMember = true;
            ViewBag.IsViewMySetting = false;
            ViewBag.BackLink = backLink;
            ViewBag.FromLeftMenu = fromLeftMenu;
            ViewBag.MemberIds = memberIds;
            ViewBag.CanEdit = CurrentSession.IsMaster && custSettingPolicy == (byte)Policy.Edit;
            SettingContext settingContext = this.GetCurrentSettingContext(memberId, agentId, null, FormSettingType.All, memberIds);

            return ViewAccountDetails(settingContext);
        }

        #endregion Setting

        #region Helpers

        /// <summary>
        /// Update all setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="commissionSetting">Commission setting.</param>
        /// <param name="betLimits">List of bet limit.</param>
        /// <returns>
        /// Action result.
        /// </returns>
        [NonAction]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Edit)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        public ActionResult UpdateAllSetting(
            SettingContext settingContext,
            CommissionSetting commissionSetting,
            IList<BettingLimit> betLimits)
        {
            if (commissionSetting == null && betLimits == null)
            {
                return ReturnResult(new Result
                {
                    ErrCode = 0,
                    ErrMsg = Apollo.Age.Resources.Lang.noChangeToUpdate
                });
            }

            if (commissionSetting != null)
            {
                if (commissionSetting.CommissionItems != null
                    && commissionSetting.CommissionItems.Any()
                    && commissionSetting.CommissionItems[0].SportId == (int)SportTypes.Soccer
                    && commissionSetting.CommissionItems[0].BetTypeId == (int)BetTypes.Handicap)
                {
                    RevertCommGroupValue(commissionSetting.CommissionItems[0]);
                }

                if (!string.IsNullOrEmpty(commissionSetting.UserGroup))
                {
                    commissionSetting.UserGroup = RevertCommGroupName(commissionSetting.UserGroup);
                }
            }

            Result result = _customerSettingService.UpdateSetting(settingContext, commissionSetting, betLimits, null);

            return ReturnResult(result);
        }

        [ApolloAuth(FunctionKey = ApolloFunctionKeys.CustomerSetting, Policy = Policy.Query)]
        private ViewResult ViewAccountDetails(SettingContext settingContext)
        {
            ViewBag.CommissionSetting = this.GetCommissionSetting(settingContext);
            ViewBag.BetSetting = this.GetBetSetting(settingContext);

            return View("~/Views/Account/ViewAccountDetails.cshtml");
        }

        /// <summary>
        /// Get role from agent level.
        /// </summary>
        /// <param name="agentLevel">The agent level.</param>
        /// <returns>Agent role.</returns>
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

        /// <summary>
        /// Set bet limit language.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="betLimits">List of bet limit.</param>
        private void SetBetLimitLanguage(SettingContext settingContext, IList<BettingLimit> betLimits)
        {
            foreach (Apollo.CustSetting.ViewData.BettingLimit bettingLimitItem in betLimits)
            {
                bettingLimitItem.Name = GetResourceData(bettingLimitItem.ResourceKey);
            }
        }

        /// <summary>
        /// Set current language.
        /// </summary>
        /// <param name="language">The language.</param>
        private void SetCurrentLanguage(string language)
        {
            CultureInfo def = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentCulture.DateTimeFormat = def.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = def.NumberFormat;
        }

        /// <summary>
        /// Get JSON setting.
        /// </summary>
        /// <returns>JSON setting.</returns>
        private JsonSerializerSettings GetJsonSetting()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();

            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;

            return settings;
        }

        /// <summary>
        /// Get target role ID from agent level.
        /// </summary>
        /// <param name="agentLevel">The agent level.</param>
        /// <returns>Target role ID.</returns>
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

        /// <summary>
        /// Set position taking language.
        /// </summary>
        /// <param name="positionTakingSetting">The position taking setting.</param>
        private void SetPositionTakingLanguage(PositionTakingSetting positionTakingSetting)
        {
            // Set language for SportType Name on SportTypes
            string betTypeName = string.Empty;
            string bettypeTitle = string.Empty;

            foreach (Apollo.CustSetting.ViewData.SportType sportTypeItem in positionTakingSetting.SportTypes)
            {
                sportTypeItem.Name = this.GetResourceData(sportTypeItem.ResourceKey);
                foreach (Apollo.CustSetting.ViewData.BetTypeGroup betTypeGroupItem in sportTypeItem.BetTypeGroups)
                {
                    foreach (Apollo.CustSetting.ViewData.BetType betTypeItem in betTypeGroupItem.BetTypes)
                    {
                        betTypeName = bettypeTitle = GetResourceData(betTypeItem.ResourceKey);
                        betTypeItem.Name = betTypeName;
                        betTypeItem.Title = bettypeTitle;
                    }
                }
            }
        }

        /// <summary>
        /// Return result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>JSON result.</returns>
        private JsonResult ReturnResult(Result result)
        {
            if (result.ErrCode != 0 && result.ErrCode != Apollo.CustSetting.Core.Constant.WARNING_CODE_DEFAULT)
            {
                return Json(new Result(result.ErrCode, result.ErrMsg));
            }
            else
            {
                return Json(new Result(0, Resources.Lang.updatesuccessfull));
            }
        }

        /// <summary>
        /// Get current setting context.
        /// </summary>
        /// <param name="targetCustId">The target customer ID.</param>
        /// <param name="agentId">The agent ID.</param>
        /// <param name="agentLevel">The agent level.</param>
        /// <param name="formSettingType">Form setting type.</param>
        /// <param name="memberIds">List of member ID.</param>]
        /// <returns>Setting context.</returns>
        private SettingContext GetCurrentSettingContext(
            int targetCustId,
            int? agentId,
            int? agentLevel,
            FormSettingType formSettingType,
            string memberIds)
        {
            SettingContext settingContext = new SettingContext
            {
                CustId = CurrentSession.AgentId,
                TargetCustId = targetCustId,
                ActorCustId = CurrentSession.AgentId,
                RoleId = this.GetRoleFromAgentLevel(CurrentSession.AgentLevel),
                TargetRoleId = this.GetTargetRoleIdFromAgenLevel(agentLevel),
                FormSetting = (int)formSettingType,
                FormId = FormType.EditPT,
                Language = CurrentSession.Language,
                AdminId = CurrentSession.AdminId,
                TargetCustIds = memberIds
            };

            var isEditMember = settingContext.TargetRoleId == (int)Role.Member;

            //// If Current is Share Holder, then constrain min
            if (agentLevel != null && agentLevel.Value == 2)
            {
                settingContext.HasMinConstrain = true;
            }

            //// If current Cust Edited Role is Member, then set Edit Role is Agent
            if (isEditMember)
            {
                settingContext.RoleId = (int)Role.Agent;
            }

            //// If current Agent difference with Direct Upline Agent, then set Form to Edit Non Direct Member
            if (agentId != null && agentId.Value != CurrentSession.AgentId)
            {
                settingContext.FormId = FormType.EditMemberNonDirect;
            }

            //// View Current Agent Setting
            if (CurrentSession.AgentId == targetCustId && settingContext.TargetRoleId != (int)Role.Member)
            {
                settingContext.FormId = FormType.ViewAllSetting;
            }

            if (isEditMember && agentId != null)
            {
                settingContext.FormId = FormType.EditPT;
                settingContext.CustId = agentId.Value;
            }

            if (!CurrentSession.IsMaster)
            {
                //// If Current Login Agent is not Agent Master User, then set form to form view
                settingContext.FormId = FormType.ShadowViewSetting;
                if (CurrentSession.IsSuper && settingContext.TargetRoleId == (int)Role.Member)
                {
                    settingContext.CustId = agentId.Value;
                }
            }

            if (!string.IsNullOrEmpty(memberIds))
            {
                settingContext.FormId = FormType.EditMultiple;
            }

            var custSettingPolicy = _userService.GetUserPolicy(CurrentSession.UserId, ApolloFunctionKeys.CustomerSetting);

            if (custSettingPolicy != (byte)Policy.Edit)
            {
                //// If Current Login Agent is not have edit customer setting, then set form to form view
                settingContext.FormId = FormType.ShadowViewSetting;
                if (CurrentSession.IsSuper && settingContext.TargetRoleId == (int)Role.Member)
                {
                    settingContext.CustId = agentId.Value;
                }
            }

            return settingContext;
        }

        /// <summary>
        /// Get setting script.
        /// </summary>
        /// <param name="targetCustId">The target customer ID.</param>
        /// <param name="agentId">The agent ID.</param>
        /// <param name="agentLevel">The agent level.</param>
        /// <param name="ugroup">The user group commission.</param>
        /// <param name="targetCustIds">List of target customer ID.</param>
        /// <returns>Setting script.</returns>
        private string GetSettingScript(
            int targetCustId,
            int? agentId,
            int? agentLevel,
            string ugroup,
            string targetCustIds)
        {
            SettingContext settingContext = this.GetCurrentSettingContext(targetCustId, agentId, agentLevel, FormSettingType.All, targetCustIds);

            CommissionSetting commissionSetting = GetCommissionSetting(settingContext);
                        
            this.SetCurrentLanguage(CurrentSession.Language);
            IList<BettingLimit> betLimits = _customerSettingService.GetBettingLimitSettingForUpdating(settingContext, ref _maxBetLimit);

            this.SetBetLimitLanguage(settingContext, betLimits);

            JsonSerializerSettings jsonSetting = this.GetJsonSetting();

            string javasciptString = string.Format(
                "var commissionSetting = {0} ;\n var betLimits = {1} ;\n var settingContext = {2} ;\n var maxBetLimit = {3} ;",
                JsonConvert.SerializeObject(commissionSetting, jsonSetting),
                JsonConvert.SerializeObject(betLimits, jsonSetting),
                JsonConvert.SerializeObject(settingContext, jsonSetting),
                _maxBetLimit);

            return javasciptString;
        }

        /// <summary>
        /// Get commission setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Commission setting.</returns>
        private CommissionSetting GetCommissionSetting(SettingContext settingContext)
        {
            CommissionSetting commissionSetting = _customerSettingService.GetCommissionSettingForUpdating(settingContext);

            if (commissionSetting.CommissionItems.Any())
            {
                RevertCommGroupValue(commissionSetting.CommissionItems[0]);
                RevertCommGroupValue(commissionSetting.CommissionItems[0].Upline);
            }

            if (settingContext.TargetRoleId == (int)Role.Member)
            {
                var memberInfo = _customerSettingService.GetMemberInfo(settingContext.TargetCustId);
                if (memberInfo != null)
                {
                    commissionSetting.UserGroup = RevertCommGroupName(memberInfo.UGroup);
                    settingContext.CanEditComGroup = settingContext.AdminId > 0 || CurrentSession.IsSuper || !memberInfo.IsUGroupChanged;
                }
            }

            return commissionSetting;
        }

        /// <summary>
        /// Get bet setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>List of bet limit.</returns>
        private IList<BettingLimit> GetBetSetting(SettingContext settingContext)
        {
            IList<BettingLimit> betLimits = _customerSettingService.GetBettingLimitSettingForUpdating(settingContext, ref _maxBetLimit);

            this.SetCurrentLanguage(CurrentSession.Language);
            this.SetBetLimitLanguage(settingContext, betLimits);

            return betLimits;
        }

        /// <summary>
        /// Get position taking setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Position taking setting.</returns>
        private PositionTakingSetting GetPositionTakingSetting(SettingContext settingContext)
        {
            PositionTakingSetting positionTakingSetting = _customerSettingService.GetPositionTakingSettingForUpdating(settingContext);
            this.SetPositionTakingLanguage(positionTakingSetting);

            return positionTakingSetting;
        }

        /// <summary>
        /// Revert commission group value.
        /// </summary>
        /// <param name="commissionItem">Commission item.</param>
        private void RevertCommGroupValue(CommissionItem commissionItem)
        {
            var tempDiscountA = commissionItem.DiscountA;
            var tempDiscountB = commissionItem.DiscountB;
            var tempDiscountC = commissionItem.DiscountC;
            var tempDiscountD = commissionItem.DiscountD;

            commissionItem.DiscountA = tempDiscountD;
            commissionItem.DiscountB = tempDiscountC;
            commissionItem.DiscountC = tempDiscountB;
            commissionItem.DiscountD = tempDiscountA;
        }

        /// <summary>
        /// Revert commission group name.
        /// </summary>
        /// <param name="groupName">The group name.</param>
        /// <returns>Reverted commission group name.</returns>
        private string RevertCommGroupName(string groupName)
        {
            switch (groupName.ToLower())
            {
                case "a":
                    return "d";
                case "b":
                    return "c";
                case "c":
                    return "b";
                case "d":
                    return "a";
            }

            return string.Empty;
        }

        /// <summary>
        /// Revert commission group value.
        /// </summary>
        /// <param name="commissionModel">Commission model.</param>
        /// <returns>Customer commission model.</returns>
        private CustomerCommissionModel RevertCommGroupValue(CustomerCommissionModel commissionModel)
        {
            var tempDiscountA = commissionModel.DiscountA;
            var tempDiscountB = commissionModel.DiscountB;
            var tempDiscountC = commissionModel.DiscountC;
            var tempDiscountD = commissionModel.DiscountD;

            commissionModel.DiscountA = tempDiscountD;
            commissionModel.DiscountB = tempDiscountC;
            commissionModel.DiscountC = tempDiscountB;
            commissionModel.DiscountD = tempDiscountA;

            return commissionModel;
        }

        #endregion Helpers
    }
}