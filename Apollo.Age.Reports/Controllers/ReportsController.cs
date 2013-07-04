// --------------------------------------------------------------------------
// <copyright file="ReportsController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.29">Created</change>
//   <change who="Terry.Nguyen" date="2013.04.07">Implement</change>
//   <change who="Terry.Nguyen" date="2013.04.09">Get AgentID from BaseController</change>
//   <change who="Quinn.Vo" date="2013.04.16">Add Action: MatchEntered, AgentWinLoss, MemberWinLoss</change>
// </history>

namespace Apollo.Age.Reports.Controllers
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;
	using System.Web.SessionState;
	using Apollo.Age.Entities;
	using Apollo.Age.Reports.Criterias;
	using Apollo.Age.Reports.Models;
	using Apollo.Age.Reports.Services;
	using Apollo.Age.Reports.ViewModels;
	using Apollo.Age.Services.Impl;
	using Apollo.Age.Web;

	[SessionState(SessionStateBehavior.ReadOnly)]
	public class ReportsController : BaseController
	{
		private readonly OutstandingService _reportService;
		private readonly IWinLossService _winLossService;
		private int _currentAgentId;

		public ReportsController()
			: this(null)
		{
		}

		public ReportsController(OutstandingService reportService = null)
		{
			_reportService = reportService ?? new OutstandingService();
			_winLossService = _winLossService ?? new WinLossService();
		}

		public int? CurrentAgentId
		{
			get
			{
				return _currentAgentId;
			}

			set
			{
				_currentAgentId = value.HasValue ? value.Value : this.CurrentSession.AgentId;
			}
		}

		#region Outstanding Report

		public ViewResult Outstanding(int? agentId, string sporttypes, string bettypes)
		{
			CurrentAgentId = agentId;
			var mainShareHolder = new Apollo.Age.Entities.Agent();

			var outstandingViewModel = new OutstandingViewModel()
			{
				CurrentAgentId = CurrentAgentId.Value,
				MainShareHolder = mainShareHolder,
				Sporttypes = sporttypes,
				Bettypes = bettypes
			};

			if (sporttypes != null)
			{
				IEnumerable<Agent> modelAgentDownLine = AgentDownlinePath(CurrentAgentId.Value);

				if (CurrentSession.IsSuper)
				{
					if (modelAgentDownLine.Count() > 1)
					{
						mainShareHolder = modelAgentDownLine.ToList()[1];
					}
				}

				IEnumerable<OutstandingModel> models = _reportService.GetData<OutstandingModel>(new OutstandingCriteria
				{
					AgentId = CurrentAgentId.Value,
					Sporttypes = sporttypes,
					Bettypes = bettypes
				});

				outstandingViewModel.ListOutStanding = models;
				outstandingViewModel.DownlinePath = modelAgentDownLine;
				outstandingViewModel.AgentLevel = modelAgentDownLine.LastOrDefault().Level;
			}

			outstandingViewModel.SportList = SingbetSportService.GetSport(isOther: false);
			outstandingViewModel.BettypeList = _reportService.GetBettype();

			return View(outstandingViewModel);
		}

		[HttpPost]
		public ActionResult MemberOutstanding(int? agentId, string sporttypes, string bettypes, int mainShareHolderId, string mainShareHolderUsername)
		{
			CurrentAgentId = agentId;
			//// call service to get report data here
			IEnumerable<OutstandingModel> models = _reportService.GetData<OutstandingModel>(new MemberOutstandingCriteria
			{
				AgentId = CurrentAgentId.Value,
				Sporttypes = sporttypes,
				Bettypes = bettypes
			});

			//// Sum total amount
			OutstandingModel totalModels = new OutstandingModel();
			if (models != null && models.Any())
			{
				foreach (OutstandingModel item in models)
				{
					totalModels.Outstanding += item.Outstanding;
					totalModels.ShareAmount += item.ShareAmount;
					totalModels.GameOutstanding += item.GameOutstanding;
					totalModels.GameShareAmount += item.GameShareAmount;
				}
			}
			//// return object
			return Json(new
			{
				ListOutstanding = models,
				Total = totalModels,
				MainShareHolderId = mainShareHolderId,
				MainShareHolderUsername = mainShareHolderUsername
			});
		}

		#endregion Outstanding Report

		#region Agent Win Loss Report

		public ViewResult MatchEntered()
		{
			var matchEnteredViewModel = new MatchEnteredViewModel();
			matchEnteredViewModel.BalanceUpDay = _reportService.GetBalanceUpDay();
			matchEnteredViewModel.SportList = SingbetSportService.GetSport(isOther: false);
			matchEnteredViewModel.BettypeList = _reportService.GetBettype();
			matchEnteredViewModel.matchEnteredModelList = _winLossService.GetMatchEntered();

			return View(matchEnteredViewModel);
		}

		public ViewResult AgentWinLoss(AgentWinLossViewModel model)
		{
			model.AgentId = model.AgentId == 0 ? this.CurrentSession.AgentId : model.AgentId;
			var agentName = string.IsNullOrWhiteSpace(model.AgentName) ? this.CurrentSession.AgentName : model.AgentName;
			var isSuper = model.AgentId == this.CurrentSession.AgentId ? this.CurrentSession.IsSuper : false;

			var criteria = new AgentWinLossCriteria
			{
				AgentId = model.AgentId,
				Sporttypes = model.Sporttypes,
				Bettypes = model.Bettypes,
				FromDate = model.FromDate,
				ToDate = model.ToDate,
				BalanceUpDay = _reportService.GetBalanceUpDay(),
				MoveBetDay = _reportService.MoveBetsDay()
			};

			var agentWinLossList = _winLossService.GetAgentWinLoss(criteria);
			var modelAgentDownLine = AgentDownlinePath(model.AgentId).ToList();

			model.IsSuper = isSuper;
			model.AgentName = agentName;
			model.AgentWinLossList = agentWinLossList;
			model.DownlinePath = modelAgentDownLine;
			model.SportList = SingbetSportService.GetSport(isOther: false);
			model.BettypeList = _reportService.GetBettype();
			model.BalanceUpDay = _reportService.GetBalanceUpDay();

			return View(model);
		}

		public ViewResult MemberWinLoss(MemberWinLossViewModel model)
		{
			var criteria = new MemberWinLossCriteria
			{
				AgentId = model.AgentId,
				Sporttypes = model.Sporttypes,
				Bettypes = model.Bettypes,
				FromDate = model.FromDate,
				ToDate = model.ToDate,
				BalanceUpDay = _reportService.GetBalanceUpDay(),
				MoveBetDay = _reportService.MoveBetsDay()
			};

			var memberWinLossList = _winLossService.GetMemberWinLoss(criteria);
			var modelAgentDownLine = AgentDownlinePath(model.AgentId).ToList();

			model.MemberWinLossList = memberWinLossList;
			model.DownlinePath = modelAgentDownLine;
			model.SportList = SingbetSportService.GetSport(isOther: false);
			model.BettypeList = _reportService.GetBettype();
			model.BalanceUpDay = _reportService.GetBalanceUpDay();

			return View(model);
		}

		#endregion Agent Win Loss Report

		private IEnumerable<Agent> AgentDownlinePath(int agentId)
		{
			IEnumerable<Agent> models = _reportService.GetData<Agent>(new AgentDownlinePathCriteria
			{
				ParentId = CurrentSession.AgentId,
				AgentId = agentId
			});

			return models;
		}
	}
}