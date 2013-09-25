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
    using Apollo.Age.Enumerations;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Filters;
    using Fanex.Common;
    using Fanex.CustSetting.Models;
    using Fanex.CustSetting.Service;
    using Fanex.CustSetting.ViewData;
    using Newtonsoft.Json;
    using Apollo.Age.Configurations;

    /// <summary>
    /// Customer Setting Controller
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
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

        #endregion Ctors

        #region Commission Setting

        /// <summary>
        /// Views commission from logged-in agent to selected (searched) agent/member.
        /// </summary>
        /// <param name="model">Username: text which user input to search, IsMember: is agent or member.</param>
        /// <returns>View Commission</returns>
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
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
                var commissionList = model.IsMember
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

                var commissionModelList = new List<CustomerCommissionModel>();
                if(commissionList != null && commissionList.Any())
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
        /// <param name="username">selected agent username.</param>
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
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
                sportItem.Name = base.GetResourceData(sportItem.ResourceKey);
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
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
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
                sportItem.Name = base.GetResourceData(sportItem.ResourceKey);
            }

            var betTypeList = _sportService.GetBetTypeBySport(sportId, isMixSport);

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
                betTypeItem.Name = base.GetResourceData(betTypeItem.ResourceKey);
            }

            view.BetTypeList = betTypeList;

            return Json(view.BetTypeSelectListItem, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Edit Agent Position Taking.
        /// </summary>
        /// <param name="agentId">Agent Id need be edited.</param>
        /// <param name="userName">Agent username.</param>
        /// <param name="agentLevel">Agent level.</param>
        /// <param name="reload">After edit, reload page or not.</param>
        /// <param name="isSimpleMode">Current mode is simple mode or not</param>
        /// <param name="backLink">Back link for after editing</param>
        /// <param name="fromLeftMenu">Link from left menu or not</param>
        /// <returns>Action result.</returns>
        public ActionResult EditAgentPositionTaking(int agentId, string userName, int? agentLevel, bool reload = false, bool isSimpleMode = true, string backLink = "", bool fromLeftMenu = false, string parentUsername = "")
        {
            ViewBag.CustId = agentId;
            ViewBag.UserName = userName;
            ViewBag.AgentLevel = agentLevel;
            ViewBag.Reload = reload;
            ViewBag.BackLink = backLink;
            ViewBag.FromLeftMenu = fromLeftMenu;
            ViewBag.IsEditAgent = true;
            ViewBag.IsSimpleMode = isSimpleMode;
            ViewBag.IsMaster = base.CurrentSession.IsMaster;// && _accountService.IsDirectAgent(base.CurrentSession.AgentId, agentId, base.CurrentSession.IsSuper);
            ViewBag.ParentUsername = parentUsername;
            return View("~/Views/Account/EditPositionTaking.cshtml");
        }

        [HttpPost]
        public ActionResult CalculateUpdateAgentPositionTakingTime(SettingContext settingContext, PositionTakingSetting positionTakingSetting)
        {
            var totalSeconds = _accountService.CalculateUpdateAgentPositionTakingTime(settingContext, positionTakingSetting);
            return Json(totalSeconds);
        }
                
        /// <summary>
        /// Edit all member share of agent.
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="agentUserName"></param>
        /// <returns></returns>
        public ActionResult EditAllMemberShare(int agentId, 
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
        /// Edit member position taking
        /// </summary>
        /// <param name="memberId">Member id need be edited</param>
        /// <param name="userName">Member user name.</param>
        /// <param name="agentId">Cross edit agent.</param>
        /// <param name="agentUsername">Agent user name.</param>
        /// <param name="reload">Reload page or not</param>
        /// <param name="isSimpleMode">Current mode is simple mode or not</param>
        /// <param name="backLink">Back link after editing.</param>
          /// <param name="fromLeftMenu">From left menu.</param>
        /// <returns>Action result</returns>
        public ActionResult EditMemberPositionTaking(
            int agentId, 
            string agentUsername = "", 
            bool reload = false, 
            bool isSimpleMode = true, 
            string backLink = "", 
            bool fromLeftMenu = false,
            string memberIds = "")
      
        {
            //if (AppSettings.DisableEditOnMember)
            //{
            //    return Redirect("~/Error?err=404");
            //}
            ViewBag.CustId = 0;
            ViewBag.UserName = string.Empty;
            ViewBag.AgentId = agentId;
            ViewBag.BackLink = backLink;
            ViewBag.FromLeftMenu = fromLeftMenu;
            ViewBag.Reload = reload;
            ViewBag.IsSimpleMode = isSimpleMode;
            ViewBag.IsEditAgent = false;
            ViewBag.IsMaster = base.CurrentSession.IsMaster;
            ViewBag.AgentUsername = agentUsername;
            ViewBag.MemberIds = memberIds;
            return View("~/Views/Account/EditPositionTaking.cshtml");
        }

        /// <summary>
        /// Get Position taking result.
        /// </summary>
        /// <param name="targetCustId">Target cust id is member or agent.</param>
        /// <param name="agentId">Agent id for edit cross.</param>
       
        /// <param name="agentLevel">Agent level.</param>
        /// <returns>Action result.</returns>
         public ActionResult GetPositionTaking(
            int targetCustId, 
            int? agentId, 
            int? agentLevel,
            string targetCustIds = "")

        {
            SettingContext settingContext = this.GetCurrentSettingContext(targetCustId, agentId, agentLevel, FormSettingType.PositionTaking, targetCustIds);
            PositionTakingSetting positionTakingSetting = _customerSettingService.GetPositionTakingSettingForUpdating(settingContext);
            this.SetCurrentLanguage(base.CurrentSession.Language);
            this.SetPositionTakingLanguage(positionTakingSetting);

            JsonSerializerSettings jsonSetting = this.GetJsonSetting();
            string javascriptString = string.Format("var positionTakingSetting = {0} ;\n var settingContext = {1} ;",
                JsonConvert.SerializeObject(positionTakingSetting, jsonSetting),
                JsonConvert.SerializeObject(settingContext, jsonSetting));

            return JavaScript(javascriptString);
        }

        /// <summary>
        /// Update postion taking setting.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="positionTakingSetting">Position taking update.</param>
        /// <returns>Action result.</returns>
        [HttpPost, AllowUpdateChecking]
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
        /// <param name="custId">Cust id need be edited</param>
        /// <param name="userName">User name.</param>
        /// <param name="agentId">Agent id for edit cross level.</param>
        /// <param name="agentLevel">Agent level.</param>
        /// <param name="userGroup">User group for edit member.</param>
        /// <param name="showHeader">Show header or not.</param>
        /// <returns>Action result.</returns>
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
            ViewBag.IsMaster = base.CurrentSession.IsMaster;
            ViewBag.FromLeftMenu = fromLeftMenu;
            ViewBag.MemberIds = memberIds;
            return View("~/Views/Account/EditMemberSetting.cshtml");
        }

        public ActionResult EditAgentSetting(
            int agentId, 
            int agentLevel, 
            string userName, 
            bool fromLeftMenu = false)
        {
            ViewBag.AgentId = agentId;
            ViewBag.UserName = userName;
            ViewBag.AgentLevel = agentLevel;
            ViewBag.IsMaster = base.CurrentSession.IsMaster;
            ViewBag.FromLeftMenu = fromLeftMenu;
            return View("~/Views/Account/EditAgentSetting.cshtml");
        }

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
        public ActionResult UpdateMemberSetting(int memberId, SettingContext settingContext, CommissionSetting commissionSetting, IList<BettingLimit> betLimits)
        {
            return UpdateAllSetting(settingContext, commissionSetting, betLimits);
        }

        [HttpPost, AllowUpdateChecking]
        public ActionResult UpdateAgentSetting(int agentId, SettingContext settingContext, CommissionSetting commissionSetting, IList<BettingLimit> betLimits)
        {
            return UpdateAllSetting(settingContext, commissionSetting, betLimits);
        }

        public ActionResult MySetting()
        {
            ViewBag.CustId = CurrentSession.AgentId;
            ViewBag.UserName = CurrentSession.AgentName;
            ViewBag.AgentLevel = CurrentSession.AgentLevel;
            ViewBag.IsViewOnMember = false;
            ViewBag.CanEdit = base.CurrentSession.IsMaster;
            ViewBag.IsViewMySetting = true;
            SettingContext settingContext = this.GetCurrentSettingContext(CurrentSession.AgentId, base.CurrentSession.AgentId, CurrentSession.AgentLevel, FormSettingType.All, string.Empty);
            return ViewAccountDetails(settingContext);
        }

        public ActionResult ViewAgentDetails(
            int agentId, 
            int agentLevel, 
            string userName, 
            string backLink = "", 
            bool fromLeftMenu = false)
        {
            ViewBag.CustId = agentId;
            ViewBag.UserName = userName;
            ViewBag.AgentLevel = agentLevel;
            ViewBag.IsViewOnMember = false;
            ViewBag.CanEdit = base.CurrentSession.IsMaster;
            ViewBag.IsViewMySetting = base.CurrentSession.AgentId == agentId;
            ViewBag.BackLink = backLink;
            ViewBag.FromLeftMenu = fromLeftMenu;
            SettingContext settingContext = this.GetCurrentSettingContext(agentId, base.CurrentSession.AgentId, agentLevel, FormSettingType.All, string.Empty);
            return ViewAccountDetails(settingContext);
        }

        public ActionResult ViewMemberDetails(
            int memberId, 
            string userName, 
            int? agentId, 
            string backLink = "", 
            bool fromLeftMenu = false,
            string memberIds = "")
        {
            ViewBag.CustId = memberId;
            ViewBag.UserName = userName;
            ViewBag.AgentId = agentId;
            ViewBag.IsViewOnMember = true;
            ViewBag.IsViewMySetting = false;
            ViewBag.BackLink = backLink;
            ViewBag.FromLeftMenu = fromLeftMenu;
            ViewBag.MemberIds = memberIds;
            ViewBag.CanEdit = base.CurrentSession.IsMaster;//&& ((agentId != null && base.CurrentSession.AgentId == agentId.Value) || base.CurrentSession.IsSuper);
            SettingContext settingContext = this.GetCurrentSettingContext(memberId, agentId, null, FormSettingType.All, memberIds);
            //if (agentId != null && agentId.Value != base.CurrentSession.AgentId)
            //{
            //    settingContext.FormId = FormType.EditPT;
            //}
            return ViewAccountDetails(settingContext);
        }

        private ViewResult ViewAccountDetails(SettingContext settingContext)
        {
            ViewBag.CommissionSetting = this.GetCommissionSetting(settingContext);
            ViewBag.BetSetting = this.GetBetSetting(settingContext);
            return View("~/Views/Account/ViewAccountDetails.cshtml");
        }

        #endregion Setting

        #region Helpers
        [NonAction]
        public ActionResult UpdateAllSetting(SettingContext settingContext, CommissionSetting commissionSetting, IList<BettingLimit> betLimits)
        {
            if (commissionSetting == null && betLimits == null)
            {
                return ReturnResult(new Result
                {
                    ErrCode = 0,
                    ErrMsg = Apollo.Age.Resources.Lang.noChangeToUpdate
                });
            }

            if (commissionSetting != null )
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

        private SettingContext GetCurrentSettingContext(
            int targetCustId, 
            int? agentId, 
            int? agentLevel, 
            FormSettingType formSettingType,
            string memberIds)
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
                Language = base.CurrentSession.Language,
                AdminId = base.CurrentSession.AdminId,
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
            if (agentId != null && agentId.Value != base.CurrentSession.AgentId)
            {
                settingContext.FormId = FormType.EditMemberNonDirect;
            }

            //// View Current Agent Setting
            if (base.CurrentSession.AgentId == targetCustId && settingContext.TargetRoleId != (int)Role.Member)
            {
                settingContext.FormId = FormType.ViewAllSetting;
            }

            if (isEditMember && agentId != null)
            {
                settingContext.FormId = FormType.EditPT;
                settingContext.CustId = agentId.Value;
            }

            if (!base.CurrentSession.IsMaster)
            {
                //// If Current Login Agent is not Agent Master User, then set form to form view
                settingContext.FormId = FormType.ShadowViewSetting;
                if (base.CurrentSession.IsSuper && settingContext.TargetRoleId == (int)Role.Member)
                {
                    settingContext.CustId = agentId.Value;
                }
            }

            if (!string.IsNullOrEmpty(memberIds))
            {
                settingContext.FormId = FormType.EditMultiple;
            }

            return settingContext;
        }

        private string GetSettingScript(
            int targetCustId, 
            int? agentId, 
            int? agentLevel, 
            string ugroup,
            string targetCustIds)
        {
            SettingContext settingContext = this.GetCurrentSettingContext(targetCustId, agentId, agentLevel, FormSettingType.All, targetCustIds);

            CommissionSetting commissionSetting = GetCommissionSetting(settingContext);

            IList<BettingLimit> betLimits = _customerSettingService.GetBettingLimitSettingForUpdating(settingContext);
            this.SetCurrentLanguage(base.CurrentSession.Language);
            this.SetBetLimitLanguage(settingContext, betLimits);

            JsonSerializerSettings jsonSetting = this.GetJsonSetting();
            string javasciptString = string.Format("var commissionSetting = {0} ;\n var betLimits = {1} ;\n var settingContext = {2} ;",
                JsonConvert.SerializeObject(commissionSetting, jsonSetting),
                JsonConvert.SerializeObject(betLimits, jsonSetting),
                JsonConvert.SerializeObject(settingContext, jsonSetting));

            return javasciptString;
        }

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

        private void RevertCommGroupValue(
            CommissionItem commissionItem)
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