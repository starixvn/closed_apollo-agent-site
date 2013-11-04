// --------------------------------------------------------------------------
// <copyright file="TicketHelper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.11.4">Created</change>
// </history>

namespace Fanex.BetList.Core.TicketHelper
{
    using System;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// Utilities for resolving Names from IDs.
    /// </summary>
    public class TicketHelper : ITicketHelper
    {
        public int Index { get; set; }

        public string CurrentLanguage { get; set; }

        public bool IsShowOddsWithChoice { get; set; }

        public string GetBetTypeNameById(object bettypeId)
        {
            return CacheRef.GetBettype(bettypeId);
        }

        public string GetCasinoBetTeamNameById(object betteamId)
        {
            return CacheRef.GetCasinoBetItem(betteamId);
        }

        public string GetLeagueNameById(object leagueId)
        {
            return CacheRef.GetLeagueName(leagueId);
        }

        public string GetSportNameById(object sportId)
        {
            return CacheRef.GetSportName(sportId);
        }

        public string GetTeamNameById(object teamId)
        {
            return CacheRef.GetTeamName(teamId);
        }

        public virtual string GetHorseTeamNameById(object homeId, object awayId)
        {
            return CacheRef.GetHorseTeamName(homeId);
        }

        public string GetBetTypeNameById(object bettypeId, object betId)
        {
            return CacheRef.GetBettype(bettypeId, betId);
        }

        public string GetRouletteBetItemNameById(object itemId)
        {
            return CacheRef.GetRouletteBetItem(itemId);
        }

        public bool IsVnIp(string ip)
        {
            throw new NotImplementedException();
        }
    }
}