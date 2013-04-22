// --------------------------------------------------------------------------
// <copyright file="MemberShareViewModel.cs" company="Nexcel Solutions Vietnam">
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
    using Apollo.Age.Account.Models;
    using Apollo.Age.Entities;
    using Apollo.Age.Services;

    public class MemberShareViewModel
    {
        public bool IsMember { get; set; }

        public string Username { get; set; }

        public short SportType { get; set; }

        public short BetType { get; set; }

        public bool IsLive { get; set; }

        public string BetTypeSelected
        {
            get
            {
                return this.IsLive ? ("1-" + BetType.ToString()) : ("0-" + BetType.ToString());
            }
        }

        public List<Sport> SportList { get; set; }

        public List<BetType> BetTypeList { get; set; }

        public List<MemberShareModel> MemberShareModelList { get; set; }

        public IEnumerable<SelectListItem> SportSelectListItem
        {
            get
            {
                var selectItemList = SportList.Select(sport => new SelectListItem
                {
                    Value = sport.Id.ToString(),
                    Text = CacheRef.GetSportName(sport.Id)
                }).ToList<SelectListItem>();

                return selectItemList;
            }
        }

        public IEnumerable<SelectListItem> BetTypeSelectListItem
        {
            get
            {
                var selectItemList = BetTypeList.Select(bettype => new SelectListItem
                {
                    Value = bettype.IsLive ? ("1-" + bettype.Id.ToString()) : ("0-" + bettype.Id.ToString()),
                    Text = bettype.IsLive ? (Resources.Lang.lblLiveBall + " " + CacheRef.GetBettype(bettype.Id))
                        : (bettype.IsDead ? (Resources.Lang.lblDeadball + " " + CacheRef.GetBettype(bettype.Id))
                        : CacheRef.GetBettype(bettype.Id))
                }).ToList<SelectListItem>().OrderByDescending(t=>t.Value);

                return selectItemList;
            }
        }
    }
}
