// --------------------------------------------------------------------------
// <copyright file="GetAllMainShareHolderCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.25">Created</change>
// </history>

namespace Apollo.Age.Criterias
{
    using Apollo.Age.Enumerations;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get all main shareholder.
    /// </summary>
    public class GetAllMainShareHolderCriteria : CriteriaBase
    {
        private string _betType;

        private BetTypes _betTypeEnum;

        public GetAllMainShareHolderCriteria()
        {
        }

        public GetAllMainShareHolderCriteria(BetTypes betType)
        {
            _betTypeEnum = betType;
        }

        public string Bettypes
        {
            get
            {
                if (!string.IsNullOrEmpty(_betType))
                {
                    return _betType;
                }

                switch (_betTypeEnum)
                {
                    case BetTypes.Handicap:
                        return "1,3,7,8";
                    case BetTypes.Total_Goal:
                        return "6,126";
                    case BetTypes.Money_Line:
                        return "20,21";
                    case BetTypes.Odd_Even:
                        return "2,5,12,15,123";
                    case BetTypes.HT_FT:
                        return "16";
                    case BetTypes.Draw_No_Bet:
                        return "25,121,122";
                    case BetTypes.First_Goal_Last_Goal:
                        return "14,127";
                    case BetTypes.Outright:
                        return "10";
                }

                return "1, 2, 3, 4, 5, 6, 7, 8, 12, 13, 15, 20, 21, 24, 25, 26, 27, 28, 30, 121, 122, 123, 9, 29, 10";
            }

            set
            {
                _betType = value;
            }
        }

        /// <summary>
        /// Override Get Store Procedure Name.
        /// </summary>
        /// <returns>Return Store Procedure Name.</returns>
        public override string GetSettingKey()
        {
            return "Age_Agent_GetAllMainShareHolder";
        }

        /// <summary>
        /// Override check is validate parameter.
        /// </summary>
        /// <returns>Return true or false.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
