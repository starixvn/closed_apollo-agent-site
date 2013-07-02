// --------------------------------------------------------------------------
// <copyright file="MatchEnteredViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.15">Created</change>
// </history>

namespace Apollo.Age.Reports.ViewModels
{
	using System;
	using System.Collections.Generic;
	using Apollo.Age.Entities;
	using Apollo.Age.Reports.Models;

	public class MatchEnteredViewModel
	{
		public DateTime BalanceUpDay { get; set; }

		public List<MatchEnteredModel> matchEnteredModelList { get; set; }

		public List<SingbetSport> SportList { get; set; }

		public List<SingbetBettype> BettypeList { get; set; }

		public bool CheckIsCompleted(bool isToday)
		{
			if (matchEnteredModelList != null)
			{
				if (isToday)
				{
					foreach (var item in matchEnteredModelList)
					{
						if (item.TodayNonEntered != 0)
						{
							return false;
						}
					}
				}
				else
				{
					foreach (var item in matchEnteredModelList)
					{
						if (item.YesterdayNonEntered != 0)
						{
							return false;
						}
					}
				}
			}

			return true;
		}
	}
}
