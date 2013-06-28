// --------------------------------------------------------------------------
// <copyright file="AgentShareViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.17">Created</change>
// </history>

namespace Apollo.Age.Account.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Apollo.Age.Account.Entities;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Enumerations;

    public class AgentShareViewModel
    {
        public bool IsMember { get; set; }

        public string Username { get; set; }

        public short SportType { get; set; }

        public short BetType { get; set; }

        public bool IsLive { get; set; }

        public string SportTypeSelected
        {
            get
            {
                // hard code with Mix Sports Parlay: SportType == 99 && BetType == 9
                return (this.SportType == (short)SportTypes.OtherSports && this.BetType == (short)BetTypes.MixParlay) 
                    ? ("1-" + this.SportType.ToString()) : ("0-" + this.SportType.ToString());
            }
        }

        public string BetTypeSelected
        {
            get
            {
                return this.IsLive ? ("1-" + this.BetType.ToString()) : ("0-" + this.BetType.ToString());
            }
        }

        public List<SportSetting> SportList { get; set; }

        public List<BetTypeSetting> BetTypeList { get; set; }

        public List<AgentShareModel> AgentShareModelList { get; set; }

        public IEnumerable<SelectListItem> SportSelectListItem
        {
            get
            {
                var selectItemList = this.SportList.Select(sport => new SelectListItem
                {
                    Value = sport.IsMixSportParlay ? ("1-" + sport.Id.ToString()) : ("0-" + sport.Id.ToString()),
                    Text = sport.Name
                }).ToList<SelectListItem>();

                return selectItemList;
            }
        }

        public IEnumerable<SelectListItem> BetTypeSelectListItem
        {
            get
            {
                var selectItemList = this.BetTypeList.Select(bettype => new SelectListItem
                {
                    Value = bettype.IsLive ? ("1-" + bettype.Id.ToString()) : ("0-" + bettype.Id.ToString()),
                    Text = bettype.IsLive ? (Apollo.Age.Resources.Lang.lblLiveBall + " " + bettype.Name)
                        : (bettype.IsDead ? (Apollo.Age.Resources.Lang.lblDeadball + " " + bettype.Name)
                        : bettype.Name)
                }).ToList<SelectListItem>();

                return selectItemList;
            }
        }
    }
}
