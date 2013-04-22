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
                    Value = bettype.Id.ToString(),
                    Text = CacheRef.GetBettype(bettype.Id)
                }).ToList<SelectListItem>();

                return selectItemList;
            }
        }
    }
}
