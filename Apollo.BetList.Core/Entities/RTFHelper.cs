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

        public IFont PosFontTrike { get; set; }

        public IFont NegFont { get; set; }

        public IFont NegFontTrike { get; set; }

        public IFont RedAndBold { get; set; }

        public IFont RedNoBold { get; set; }

        public IFont RedAndBoldTrike { get; set; }

        public IFont RedNoBoldTrike { get; set; }

        public IFont BlueAndBold { get; set; }

        public IFont BlueNoBold { get; set; }

        public IFont BlueAndBoldTrike { get; set; }

        public IFont BlueNoBoldTrike { get; set; }

        public IFont GreenAndBold { get; set; }

        public IFont GreenNoBold { get; set; }

        public IFont GreenAndBoldTrike { get; set; }

        public IFont GreenNoBoldTrike { get; set; }

        public IFont WhiteColor { get; set; }

        public IFont ScoreFont { get; set; }

        public IFont ScoreFontTrike { get; set; }

        public IFont NormalFontCrossed { get; set; }

        public IFont PosFontCrossed { get; set; }

        public IFont NegFontCrossed { get; set; }

        public IFont TrikeFont { get; set; }

        public RtfTextRender RTFRenderer { get; set; }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>RTF Helper.</returns>
        public RTFHelper Clone()
        {
            RTFHelper tmp = new RTFHelper();

            tmp.PosFont = this.PosFont;
            tmp.NegFont = this.NegFont;
            tmp.NormalFont = this.NormalFont;

            tmp.NormalFontCrossed = this.NormalFontCrossed;
            tmp.PosFontCrossed = this.PosFontCrossed;
            tmp.NegFontCrossed = this.NegFontCrossed;

            tmp.RTFRenderer = new RtfTextRender();

            return tmp;
        }
    }
}