// --------------------------------------------------------------------------
// <copyright file="IResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2014.1.16">Created</change>
// </history>

namespace Apollo.BetList.Core.Builder.ResultBuilder
{
    using Apollo.BetList.Core.Entities;
    using NPOI.SS.UserModel;

    public interface IResult
    {
        IRichTextString RenderRTF(ITicket ticket, BetListPage page, RTFHelper rtfHelper);
    }
}