// --------------------------------------------------------------------------
// <copyright file="OutStandingViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.29">Created</change>
// </history>

namespace Apollo.Age.Reports.ViewModels
{
	using System.Collections.Generic;
	using System.Linq;
	using Apollo.Age.Entities;
	using Apollo.Age.Reports.Models;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class OutstandingViewModel
	{
		public int CurrentAgentId { get; set; }

        public string CurrentUserName { get; set; }

		public Agent MainShareHolder { get; set; }

		public int AgentLevel { get; set; }

		public string Sporttypes { get; set; }

		public string Bettypes { get; set; }

		public IEnumerable<OutstandingModel> ListOutStanding { get; set; }

		public IEnumerable<Agent> DownlinePath { get; set; }

		public List<SingbetSport> SportList { get; set; }

		public List<SingbetBettype> BettypeList { get; set; }

		public string SportName
		{
			get
			{
				var sport = SportList.Where(t => t.Id == this.Sporttypes).SingleOrDefault();

				if (sport != null)
				{
					return sport.Name;
				}
				else
				{
					return string.Empty;
				}
			}
		}

		public string BettypeName
		{
			get
			{
				var bettype = BettypeList.Where(t => t.Id == this.Bettypes).SingleOrDefault();

				if (bettype != null)
				{
					return bettype.Name;
				}
				else
				{
					return string.Empty;
				}
			}
		}
	}
}