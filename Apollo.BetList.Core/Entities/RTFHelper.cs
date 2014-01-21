// ---------------------------------------------------------------------------------
// <copyright file="RTFHelper.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.5">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Entities
{
    using Apollo.BetList.Core.Utils;
    using NPOI.SS.UserModel;

    public struct RTFHelper
    {
        public IFont NormalFont { get; set; }

        public IFont PosFont { get; set; }

        public IFont NegFont { get; set; }

        public IFont RedAndBold { get; set; }

        public IFont RedNoBold { get; set; }

        public IFont BlueAndBold { get; set; }

        public IFont BlueNoBold { get; set; }

        public IFont ScoreFont { get; set; }

        public RtfTextRender RTFRenderer { get; set; }
    }
}