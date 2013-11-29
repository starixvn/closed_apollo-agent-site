// --------------------------------------------------------------------------
// <copyright file="CustomerSettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.5">Created</change>
// </history>

namespace Apollo.CustSetting.Repositories
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Common.Extensions;
    using Apollo.CustSetting.Core;
    using Apollo.CustSetting.Models;
    using Apollo.CustSetting.ViewData;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Customer repository.
    /// </summary>
    public class CustomerSettingRepository : ICustomerSettingRepository
    {
        /// <summary>
        /// Update Com, Bet or PT Setting.
        /// </summary>
        /// <param name="criteria">Criteria for updating data.</param>
        /// <param name="logCriteria">Log criteria.</param>
        /// <returns>Current updating status.</returns>
        public Result UpdateSetting(ICriteria criteria, ICriteria logCriteria)
        {
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                Hashtable parameters = logCriteria.AddSpParams(criteria.GetSpParams());
                objDb.ExecuteNonQuery(parameters);
                return new Result(Convert.ToInt32(parameters["errcode"]), Convert.ToString(parameters["errmsg"]));
            }
        }

        /// <summary>
        /// Get Agent Position taking for updating.
        /// </summary>
        /// <param name="criteria">Parameters query database.</param>
        /// <returns>Position taking list.</returns>
        public Dictionary<string, PositionTaking> GetAgentPositionTakingSettingForUpdating(ICriteria criteria)
        {
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                Dictionary<string, PositionTaking> positionTakingDataList = new Dictionary<string, PositionTaking>();
                IDataReader reader = objDb.ExecuteReader(criteria.GetSpParams());
                int sportId, betTypeId;
                bool isLive;
                while (reader.Read())
                {
                    sportId = reader.ToInt("SportType");
                    betTypeId = reader.ToInt("BetType");
                    isLive = reader.ToBool("IsLive");
                    PositionTaking positionTakingData = new PositionTaking
                    {
                        Preset = reader.ToDecimal("MaxUpperPositionTaking") / Constant.STEP_MUL_DIV,
                        Max = reader.ToDecimal("MaxGivenPositionTaking") / Constant.STEP_MUL_DIV,
                        Min = reader.ToDecimal("MinGivenPositionTaking") / Constant.STEP_MUL_DIV,
                        Upline = new PositionTaking
                        {
                            MinGiven = reader.ToDecimal("MinGivenPositionTakingOfParent") / Constant.STEP_MUL_DIV,
                            Min = reader.ToDecimal("MinOfMaxGivenPositionTaking") / Constant.STEP_MUL_DIV,
                            Max = reader.ToDecimal("ParentMaxGivenPositionTaking") / Constant.STEP_MUL_DIV
                        }
                    };

                    positionTakingDataList.Add(
                        string.Format(
                        "{0}_{1}_{2}",
                        sportId,
                        betTypeId,
                        isLive),
                        positionTakingData);
                }

                return positionTakingDataList;
            }
        }

        /// <summary>
        /// Get Customer position taking for updating.
        /// </summary>
        /// <param name="criteria">Parameters for updating.</param>
        /// <returns>Position taking list.</returns>
        public Dictionary<string, PositionTaking> GetCustomerPositionTakingSettingForUpdating(ICriteria criteria)
        {
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                Dictionary<string, PositionTaking> positionTakingDataList = new Dictionary<string, PositionTaking>();
                IDataReader reader = objDb.ExecuteReader(criteria.GetSpParams());
                int sportId, betTypeId;
                bool isLive;
                while (reader.Read())
                {
                    sportId = reader.ToInt("SportType");
                    betTypeId = reader.ToInt("BetType");
                    isLive = reader.ToBool("IsLive");
                    var minPositionTaking = reader.ToDecimal("MinGivenPositionTaking");
                    minPositionTaking = minPositionTaking < 0 ? 0 : minPositionTaking;
                    PositionTaking positionTakingData = new PositionTaking
                    {
                        Value = reader.ToDecimal("PositionTaking") / Constant.STEP_MUL_DIV,
                        Upline = new PositionTaking
                        {
                            Min = minPositionTaking / Constant.STEP_MUL_DIV,
                            Max = reader.ToDecimal("MaxGivenPositionTaking") / Constant.STEP_MUL_DIV
                        }
                    };

                    positionTakingDataList.Add(
                        string.Format(
                        "{0}_{1}_{2}",
                        sportId,
                        betTypeId,
                        isLive),
                        positionTakingData);
                }

                return positionTakingDataList;
            }
        }

        /// <summary>
        /// Get Bet limit setting for update.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="criteria">Criteria for get data from database.</param>
        /// <param name="maxBetLimit">The maximum bet limit.</param>
        /// <returns>
        /// Betting limit list data.
        /// </returns>
        public IList<BettingLimit> GetBettingLimitSettingForUpdating(SettingContext settingContext, ICriteria criteria, ref int maxBetLimit)
        {
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                IList<BettingLimit> bettingLimits = new List<BettingLimit>();
                IDataReader reader = objDb.ExecuteReader(criteria.GetSpParams());
                string bettingItemDicKey = string.Empty;
                while (reader.Read())
                {
                    var sportType = reader.ToInt("SportType");
                    if (!settingContext.EnableNumberGame && Constant.NUMBER_GAME_SPORT_ID == sportType)
                    {
                        continue;
                    }

                    BettingLimit bettingLimit = new BettingLimit
                    {
                        SportId = sportType,
                        BetTypeId = reader.ToInt("BetType"),
                        MinBet = reader.ToDecimal("MinBet"),
                        MaxBet = reader.ToDecimal("MaxBet"),
                        MaxPerMatch = reader.ToDecimal("MaxPerMatch"),
                        MinBetMax = reader.ToDecimal("MaxMinBet"),
                        MaxBetMin = reader.ToDecimal("MinMaxBet"),
                        MaxPerMatchMin = reader.ToDecimal("MinMaxPerMatch")
                    };

                    if (settingContext.EnableNumberGame)
                    {
                        bettingLimit.MaxPerBall = reader.ToDecimal("MaxPerBall");
                        bettingLimit.MaxPerBallMin = reader.ToDecimal("MinMaxPerBall");
                    }

                    bettingItemDicKey = string.Format("{0}_{1}", bettingLimit.SportId.ToString(), bettingLimit.BetTypeId.ToString());
                    bettingLimit.Name = Constant.BETTING_LIMIT_ITEMS_DEFAULT[bettingItemDicKey].Name;
                    bettingLimit.ResourceKey = Constant.BETTING_LIMIT_ITEMS_DEFAULT[bettingItemDicKey].ResourceKey;
                    bettingLimits.Add(bettingLimit);
                }

                reader.Close();

                maxBetLimit = Convert.ToInt32(objDb.GetParameterValue("maxbetlimit"));

                return bettingLimits.OrderBy(bt => bt.SportId).OrderBy(bt => bt.BetTypeId).ToList();
            }
        }

        /// <summary>
        /// Get Agent commission for update.
        /// </summary>
        /// <param name="criteria">Get data criteria.</param>
        /// <returns>List commission item.</returns>
        public IList<CommissionItem> GetAgentCommissionSetingForUpdating(ICriteria criteria)
        {
            IList<CommissionItem> commissionItems = new List<CommissionItem>();
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                IDataReader reader = objDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    commissionItems.Add(new CommissionItem
                    {
                        SportId = reader.ToInt("SportType"),
                        BetTypeId = reader.ToInt("BetType"),
                        DiscountA = Math.Round((reader.ToDecimal("DiscountA") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                        DiscountB = Math.Round((reader.ToDecimal("DiscountB") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                        DiscountC = Math.Round((reader.ToDecimal("DiscountC") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                        DiscountD = Math.Round((reader.ToDecimal("DiscountD") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                        Upline = new CommissionItem
                        {
                            DiscountA = Math.Round((reader.ToDecimal("UplineMinDiscountA") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                            DiscountB = Math.Round((reader.ToDecimal("UplineMinDiscountB") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                            DiscountC = Math.Round((reader.ToDecimal("UplineMinDiscountC") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                            DiscountD = Math.Round((reader.ToDecimal("UplineMinDiscountD") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST)
                        }
                    });
                }
            }

            return commissionItems;
        }

        /// <summary>
        /// Get Member commission for update.
        /// </summary>
        /// <param name="criteria">Get data commission.</param>
        /// <returns>List commission data.</returns>
        public IList<CommissionItem> GetCustomerCommissionSetingForUpdating(ICriteria criteria)
        {
            IList<CommissionItem> commissionItems = new List<CommissionItem>();
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                IDataReader reader = objDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    commissionItems.Add(new CommissionItem
                        {
                            SportId = reader.ToInt("SportType"),
                            BetTypeId = reader.ToInt("BetType"),
                            Discount = Math.Round((reader.ToDecimal("Discount") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                            Upline = new CommissionItem
                            {
                                DiscountA = Math.Round((reader.ToDecimal("UplineMinDiscountA") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                                DiscountB = Math.Round((reader.ToDecimal("UplineMinDiscountB") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                                DiscountC = Math.Round((reader.ToDecimal("UplineMinDiscountC") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                                DiscountD = Math.Round((reader.ToDecimal("UplineMinDiscountD") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST)
                            }
                        });
                }
            }

            return commissionItems;
        }

        /// <summary>
        /// Get Member information.
        /// </summary>
        /// <param name="criteria">Get data criteria.</param>
        /// <returns>Member information details.</returns>
        public MemberInfo GetMemberInfo(ICriteria criteria)
        {
            using (ObjectDb odb = new ObjectDb(criteria.GetSettingKey()))
            {                
                IDataReader reader = odb.ExecuteReader(criteria.GetSpParams());
                reader.Read();
                return new MemberInfo
                {
                    UGroup = reader.ToString("UGroup"),
                    IsUGroupChanged = reader.ToBool("IsUGroupChanged")
                };
            }
        }
    }
}