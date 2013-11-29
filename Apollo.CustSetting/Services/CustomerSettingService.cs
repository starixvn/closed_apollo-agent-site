// --------------------------------------------------------------------------
// <copyright file="CustomerSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.5">Created</change>
//   <change who="Ricky.Nguyen" date="2013.3.25">Update PT New Rule</change>
//   <change who="Ricky.Nguyen" date="2013.4.12">Update New Rules</change>
// </history>

namespace Apollo.CustSetting.Service
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Xml.Linq;
    using Fanex.Common;
    using Apollo.CustSetting.Core;
    using Apollo.CustSetting.Criteria;
    using Apollo.CustSetting.Criteria.Agent;
    using Apollo.CustSetting.Criteria.Member;
    using Apollo.CustSetting.Models;
    using Apollo.CustSetting.Repositories;
    using Apollo.CustSetting.ViewData;
    using Fanex.Logging;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Customer Service.
    /// </summary>
    [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
    public class CustomerSettingService : ICustomerSettingService
    {
        private readonly ICustomerSettingRepository _customerSettingRepository;

        public CustomerSettingService()
            : this(null)
        {
        }

        public CustomerSettingService(ICustomerSettingRepository customerSettingRepository)
        {
            _customerSettingRepository = customerSettingRepository ?? new CustomerSettingRepository();
        }

        /// <summary>
        /// Get Data for Updating Position Taking Setting.
        /// </summary>
        /// <param name="settingContext">Object contains information of Current Actor User and user was updated.</param>
        /// <returns>Position taking setting.</returns>
        public PositionTakingSetting GetPositionTakingSettingForUpdating(SettingContext settingContext)
        {
            PositionTakingSetting positionTakingSetting = new PositionTakingSetting();
            try
            {
                //// Load Pt Configuration
                XDocument positionTakingDoc = _Configuration.PositionTakingGridConfig;
                XElement root = positionTakingDoc.Root;
                Dictionary<string, PositionTaking> positionTakingData = null;
                if (settingContext.TargetRoleId == (int)Role.Member)
                {
                    positionTakingData = _customerSettingRepository.GetCustomerPositionTakingSettingForUpdating(
                        new GetMemberPositionTakingForUpdateCriteria
                        {
                            AgentId = settingContext.CustId,
                            CustId = settingContext.TargetCustId
                        });
                }
                else
                {
                    positionTakingData = _customerSettingRepository.GetAgentPositionTakingSettingForUpdating(
                        new GetAgentPositionTakingForUpdateCriteria
                        {
                            AgentId = settingContext.TargetCustId
                        });
                }

                foreach (XElement sport in root.Elements())
                {
                    SportType sportType = new SportType
                    {
                        Id = Converter.ToInt(sport.Attribute("id").Value),
                        IsLive = Converter.ToBool(sport.Attribute("isLive").Value),
                        Name = Converter.ToString2(sport.Attribute("name").Value),
                        Collapsed = Converter.ToBool(sport.Attribute("collapsed").Value),
                        ResourceKey = sport.Attribute("resourcekey").Value,
                        BetTypeGroups = new List<BetTypeGroup>()
                    };

                    //// Read All Group
                    foreach (XElement betTypeGroupElement in sport.Elements())
                    {
                        BetTypeGroup betTypeGroup = new BetTypeGroup
                        {
                            Id = Converter.ToInt(betTypeGroupElement.Attribute("id") != null ? betTypeGroupElement.Attribute("id").Value : "0")
                        };

                        //// Read all bet type
                        foreach (XElement betType in betTypeGroupElement.Elements())
                        {
                            BetType bt = new BetType
                            {
                                Id = Converter.ToInt(betType.Attribute("id").Value),
                                Name = Converter.ToString2(betType.Attribute("name").Value),
                                ResourceKey = betType.Attribute("resourcekey").Value
                            };
                            bt.Title = bt.Name;

                            //// Get Data From Dictionary
                            string positionTakingKey = string.Format("{0}_{1}_{2}", sportType.Id, bt.Id, sportType.IsLive);
                            if (positionTakingData.ContainsKey(positionTakingKey))
                            {
                                bt.Pt = positionTakingData[positionTakingKey];

                                if (settingContext.TargetRoleId == (int)Role.MainShareHolder)
                                {
                                    bt.Pt.Upline.Max = Constant.MAX_POSITION_TAKING_AGENT_LEVEL_1;
                                }

                                betTypeGroup.BetTypes.Add(bt);
                            }
                        }

                        //// If has any bettype then add bettype Group
                        if (betTypeGroup.BetTypes.Count > 0)
                        {
                            sportType.BetTypeGroups.Add(betTypeGroup);
                        }
                    }

                    //// If has any bettypegroup then add sport type
                    if (sportType.BetTypeGroups.Count > 0)
                    {
                        positionTakingSetting.SportTypes.Add(sportType);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }

            return positionTakingSetting;
        }

        /// <summary>
        /// Get Data for Updating Betting Limit Setting.
        /// </summary>
        /// <param name="settingContext">Object contains information of Current Actor User and user was updated.</param>
        /// <param name="maxBetLimit">The maximum bet limit.</param>
        /// <returns>
        /// Betting limit setting.
        /// </returns>
        public IList<BettingLimit> GetBettingLimitSettingForUpdating(SettingContext settingContext, ref int maxBetLimit)
        {
            try
            {
                IList<BettingLimit> bettingLimits = null;
                if (settingContext.TargetRoleId != (int)Role.Member)
                {
                    bettingLimits = _customerSettingRepository.GetBettingLimitSettingForUpdating(
                        settingContext,
                        new GetAgentBetLimitForUpdateCriteria
                        {
                            AgentId = settingContext.TargetCustId,                            
                            MaxBetLimit = 0
                        },
                        ref maxBetLimit);
                }
                else
                {
                    bettingLimits = _customerSettingRepository.GetBettingLimitSettingForUpdating(
                        settingContext,
                        new GetMemberBetLimitForUpdateCriteria
                        {                            
                            CustId = settingContext.TargetCustId,                            
                            MaxBetLimit = 0
                        },
                        ref maxBetLimit);
                }

                return bettingLimits;
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }

            return null;
        }

        /// <summary>
        /// Get Data for Updating Commission Setting.
        /// </summary>
        /// <param name="settingContext">Object contains information of Current Actor User and user was updated.</param>
        /// <param name="userGroup">Current user group.</param>
        /// <returns>Commission Setting.</returns>
        public CommissionSetting GetCommissionSettingForUpdating(SettingContext settingContext, string userGroup = "")
        {
            try
            {
                CommissionSetting commissionSetting = new CommissionSetting();
                if (settingContext.TargetRoleId != (int)Role.Member)
                {
                    commissionSetting.CommissionItems = _customerSettingRepository.GetAgentCommissionSetingForUpdating(new GetAgentDiscountForUpdateCriteria
                    {
                        AgentId = settingContext.TargetCustId
                    });
                }
                else
                {
                    commissionSetting.CommissionItems = _customerSettingRepository.GetCustomerCommissionSetingForUpdating(new GetMemberDiscountForUpdateCriteria
                    {                        
                        CustId = settingContext.TargetCustId
                    });
                }

                return commissionSetting;
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }

            return null;
        }

        /// <summary>
        /// Update Position Taking Setting.
        /// </summary>
        /// <param name="settingContext">Current Context.</param>
        /// <param name="positionTakingSetting">PT Setting Data.</param>
        /// <returns>Update result.</returns>
        public Result UpdatePositionTakingSetting(SettingContext settingContext, PositionTakingSetting positionTakingSetting)
        {
            if (positionTakingSetting == null || settingContext == null)
            {
                return new Result(Constant.WARNING_CODE_DEFAULT, Constant.WARNING_MESSAGE_DEFAULT);
            }

            try
            {
                ICriteria criteria = null;
                string tempString = string.Empty;
                XMLStringData xmlStringData = BuildPositionTakingSettingXML(settingContext, positionTakingSetting);

                if (xmlStringData.XMLSetting == string.Empty
                    && xmlStringData.XMLSetting3rd == string.Empty)
                {
                    return new Result(Constant.WARNING_CODE_DEFAULT, Constant.WARNING_MESSAGE_DEFAULT);
                }

                var positionTakingXML = xmlStringData.XMLSetting != string.Empty 
                    ? string.Format(Constant.FORMAT_XML_ROOT, xmlStringData.XMLSetting) 
                    : string.Empty;
                if (settingContext.TargetRoleId != (int)Role.Member)
                {
                    criteria = new UpdateAgentPositionTakingCriteria
                    {
                        ActorAgentId = settingContext.ActorCustId,
                        AgentId = settingContext.TargetCustId,
                        AdminId = settingContext.AdminId,
                        PositionTaking = positionTakingXML
                    };
                }
                else
                {
                    if (settingContext.FormId == FormType.EditMultiple)
                    {
                        criteria = new UpdateMultipleMemberPositionTakingCriteria
                        {
                            ActorAgentId = settingContext.ActorCustId,
                            CustIds = settingContext.TargetCustIds,
                            AgentId = settingContext.CustId,
                            AdminId = settingContext.AdminId,
                            PositionTaking = positionTakingXML
                        };
                    }
                    else
                    {
                        criteria = new UpdateMemberPositionTakingCriteria
                        {
                            ActorAgentId = settingContext.ActorCustId,
                            CustId = settingContext.TargetCustId,
                            AgentId = settingContext.CustId,
                            AdminId = settingContext.AdminId,
                            PositionTaking = positionTakingXML
                        };
                    }
                }

                try
                {
                    return _customerSettingRepository.UpdateSetting(
                        criteria,
                        new LogCriteria
                    {
                        IP = settingContext.IP,
                        Language = settingContext.Language
                    });
                }
                catch (Exception ex)
                {
                    var errorMessage = BuildLogErrorDetail(
                        ex.Message,
                        settingContext.TargetRoleId != (int)Role.Member,
                        settingContext.ActorCustId,
                        settingContext.FormId == FormType.EditMultiple ? settingContext.TargetCustIds : settingContext.TargetCustId.ToString(),
                        settingContext.AdminId,
                        string.Empty,
                        string.Empty,
                        positionTakingXML);
                    Logger.Log.Error(errorMessage, ex);
                    return new Result(Constant.ERROR_CODE_DEFAULT, ex.Message);
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                return new Result(Constant.ERROR_CODE_DEFAULT, ex.Message);
            }
        }

        /// <summary>
        /// Update Betting Limit Setting.
        /// </summary>
        /// <param name="settingContext"> Current Context.</param>
        /// <param name="bettingLimits">Betting limit data.</param>
        /// <returns>Update result status.</returns>
        public Result UpdateBettingLimitSetting(SettingContext settingContext, IList<BettingLimit> bettingLimits)
        {
            if (bettingLimits == null || settingContext == null)
            {
                return new Result(Constant.WARNING_CODE_DEFAULT, Constant.WARNING_MESSAGE_DEFAULT);
            }

            try
            {
                ICriteria criteria = null;
                XMLStringData xmlStringData = BuildBetLimitSettingXML(settingContext, bettingLimits);

                if (xmlStringData.XMLSetting == string.Empty && xmlStringData.XMLSetting3rd == string.Empty)
                {
                    return new Result(Constant.WARNING_CODE_DEFAULT, Constant.WARNING_MESSAGE_DEFAULT);
                }

                if (settingContext.TargetRoleId != (int)Role.Member)
                {
                    criteria = new UpdateAgentBetLimitCriteria
                    {
                        ActorAgentId = settingContext.ActorCustId,
                        AgentId = settingContext.TargetCustId,
                        AdminId = settingContext.AdminId,
                        BetLimit = xmlStringData.XMLSetting != string.Empty ? string.Format(Constant.FORMAT_XML_ROOT, xmlStringData.XMLSetting) : string.Empty
                    };
                }
                else
                {
                    criteria = new UpdateMemberBetLimitCriteria
                    {
                        ActorAgentId = settingContext.ActorCustId,
                        CustId = settingContext.TargetCustId,
                        AdminId = settingContext.AdminId,
                        BetLimit = xmlStringData.XMLSetting != string.Empty ? string.Format(Constant.FORMAT_XML_ROOT, xmlStringData.XMLSetting) : string.Empty
                    };
                }

                return _customerSettingRepository.UpdateSetting(
                    criteria,
                    new LogCriteria
                {
                    IP = settingContext.IP,
                    Language = settingContext.Language
                });
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                return new Result(Constant.ERROR_CODE_DEFAULT, ex.Message);
            }
        }

        /// <summary>
        /// Update Commission Setting.
        /// </summary>
        /// <param name="settingContext">Current context.</param>
        /// <param name="commissionSetting">Commission setting.</param>
        /// <returns>Update commission setting result status.</returns>
        public Result UpdateCommissionSetting(
            SettingContext settingContext, 
            CommissionSetting commissionSetting)
        {
            if (commissionSetting == null || settingContext == null)
            {
                return new Result(Constant.WARNING_CODE_DEFAULT, Constant.WARNING_MESSAGE_DEFAULT);
            }

            try
            {
                ICriteria criteria = null;
                XMLStringData xmlStringData = BuildCommissionSettingXML(settingContext, commissionSetting);

                if (xmlStringData.XMLSetting == string.Empty && xmlStringData.XMLSetting3rd == string.Empty)
                {
                    return new Result(Constant.WARNING_CODE_DEFAULT, Constant.WARNING_MESSAGE_DEFAULT);
                }

                if (settingContext.TargetRoleId != (int)Role.Member)
                {
                    criteria = new UpdateAgentDiscountCriteria
                    {
                        ActorAgentId = settingContext.ActorCustId,
                        AgentId = settingContext.TargetCustId,
                        AdminId = settingContext.AdminId,
                        Discount = xmlStringData.XMLSetting != string.Empty ? string.Format(Constant.FORMAT_XML_ROOT, xmlStringData.XMLSetting) : string.Empty
                    };
                }
                else
                {
                    criteria = new UpdateMemberDiscountCriteria
                    {
                        ActorAgentId = settingContext.ActorCustId,
                        CustId = settingContext.TargetCustId,
                        UGroup = commissionSetting.UserGroup.ToLower(),
                        AdminId = settingContext.AdminId,
                        Discount = xmlStringData.XMLSetting != string.Empty ? string.Format(Constant.FORMAT_XML_ROOT, xmlStringData.XMLSetting) : string.Empty
                    };
                }

                return _customerSettingRepository.UpdateSetting(
                    criteria,
                    new LogCriteria
                {
                    IP = settingContext.IP,
                    Language = settingContext.Language
                });
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                return new Result(Constant.ERROR_CODE_DEFAULT, ex.Message);
            }
        }

        /// <summary>
        /// Update All Setting.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="commissionSetting">Commission setting data.</param>
        /// <param name="bettingLimits">Betting limit setting data.</param>
        /// <param name="positionTakingSetting">Position taking setting data.</param>
        /// <returns>Update result status.</returns>
        public Result UpdateSetting(
            SettingContext settingContext, 
            CommissionSetting commissionSetting, 
            IList<BettingLimit> bettingLimits, 
            PositionTakingSetting positionTakingSetting)
        {
            try
            {
                ICriteria criteria = null;
                var discountXmlStringData = BuildCommissionSettingXML(settingContext, commissionSetting);
                var betLimitXmlStringData = BuildBetLimitSettingXML(settingContext, bettingLimits);
                var positionTakingXmlStringData = BuildPositionTakingSettingXML(settingContext, positionTakingSetting);

                var discountXML = discountXmlStringData.XMLSetting != string.Empty 
                    ? string.Format(Constant.FORMAT_XML_ROOT, discountXmlStringData.XMLSetting) 
                    : string.Empty;
                var betLimitXML = betLimitXmlStringData.XMLSetting != string.Empty 
                    ? string.Format(Constant.FORMAT_XML_ROOT, betLimitXmlStringData.XMLSetting) 
                    : string.Empty;
                var positionTakingXML = positionTakingXmlStringData.XMLSetting != string.Empty 
                    ? string.Format(Constant.FORMAT_XML_ROOT, positionTakingXmlStringData.XMLSetting) 
                    : string.Empty;

                if (settingContext.TargetRoleId != (int)Role.Member)
                {
                    criteria = new UpdateAgentSettingCriteria
                    {
                        ActorAgentId = settingContext.ActorCustId,
                        AgentId = settingContext.TargetCustId,
                        AdminId = settingContext.AdminId,
                        Discount = discountXML,
                        BetLimit = betLimitXML,
                        PositionTaking = positionTakingXML
                    };
                }
                else
                {
                    if (settingContext.FormId == FormType.EditMemberNonDirect)
                    {
                        criteria = new UpdateMemberPositionTakingCriteria
                        {
                            ActorAgentId = settingContext.ActorCustId,
                            CustId = settingContext.TargetCustId,
                            AgentId = settingContext.CustId,
                            AdminId = settingContext.AdminId,
                            PositionTaking = positionTakingXML
                        };
                    }
                    else if (settingContext.FormId == FormType.EditMultiple)
                    {
                        criteria = new UpdateMultipleMemberSettingCriteria
                        {
                            ActorAgentId = settingContext.ActorCustId,
                            CustIds = settingContext.TargetCustIds,
                            UGroup = commissionSetting.UserGroup == null ? string.Empty : commissionSetting.UserGroup.ToLower(),
                            AgentId = (int?)settingContext.CustId,
                            AdminId = settingContext.AdminId,
                            Discount = discountXML,
                            BetLimit = betLimitXML,
                            PositionTaking = positionTakingXML
                        };
                    }
                    else
                    {
                        criteria = new UpdateMemberSettingCriteria
                        {
                            ActorAgentId = settingContext.ActorCustId,
                            CustId = settingContext.TargetCustId,
                            UGroup = commissionSetting.UserGroup == null ? string.Empty : commissionSetting.UserGroup.ToLower(),
                            AgentId = (int?)settingContext.CustId,
                            AdminId = settingContext.AdminId,
                            Discount = discountXML,
                            BetLimit = betLimitXML,
                            PositionTaking = positionTakingXML
                        };
                    }
                }

                try
                {
                    return _customerSettingRepository.UpdateSetting(
                        criteria,
                        new LogCriteria
                    {
                        IP = settingContext.IP,
                        Language = settingContext.Language
                    });
                }
                catch (Exception ex)
                {
                    var errorMessage = BuildLogErrorDetail(
                        ex.Message,
                        settingContext.TargetRoleId != (int)Role.Member,
                        settingContext.ActorCustId,
                        settingContext.FormId == FormType.EditMultiple ? settingContext.TargetCustIds : settingContext.TargetCustId.ToString(),
                        settingContext.AdminId,
                        discountXML,
                        betLimitXML,
                        positionTakingXML);
                    Logger.Log.Error(errorMessage, ex);
                    return new Result(Constant.ERROR_CODE_DEFAULT, ex.Message);
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                return new Result(Constant.ERROR_CODE_DEFAULT, ex.Message);
            }
        }

        /// <summary>
        /// Get Member information.
        /// </summary>
        /// <param name="memberId">Current member id.</param>
        /// <returns>Member information.</returns>
        public MemberInfo GetMemberInfo(int memberId)
        {
            try
            {
                return _customerSettingRepository.GetMemberInfo(new GetMemberGroupCriteria
                {
                    CustId = memberId
                });
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Build XML String for Updating.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="commissionSetting">Commission setting data.</param>
        /// <returns>String data for update to database.</returns>
        private XMLStringData BuildCommissionSettingXML(SettingContext settingContext, CommissionSetting commissionSetting)
        {
            XMLStringData xmlStringData = new XMLStringData();
            string tempString = string.Empty;
            if (commissionSetting != null
                && commissionSetting.CommissionItems != null
                && commissionSetting.CommissionItems.Count > 0)
            {
                if (settingContext.TargetRoleId != (int)Role.Member)
                {
                    foreach (CommissionItem comItem in commissionSetting.CommissionItems)
                    {
                        tempString = string.Format(
                            Constant.FORMAT_AGENT_COMMISSION_SETTING_XML,
                            comItem.SportId,
                            comItem.BetTypeId,
                            comItem.DiscountA / Constant.STEP_MUL_DIV,
                            comItem.DiscountB / Constant.STEP_MUL_DIV,
                            comItem.DiscountC / Constant.STEP_MUL_DIV,
                            comItem.DiscountD / Constant.STEP_MUL_DIV);
                        if (comItem.SportId == Constant.NUMBER_GAME_SPORT_ID && settingContext.EnableNumberGame)
                        {
                            xmlStringData.XMLSetting3rd += tempString;
                        }
                        else
                        {
                            xmlStringData.XMLSetting += tempString;
                        }
                    }
                }
                else
                {
                    foreach (CommissionItem comItem in commissionSetting.CommissionItems)
                    {
                        tempString = string.Format(
                            Constant.FORMAT_MEMBER_COMMISSION_SETTING_XML,
                            comItem.SportId,
                            comItem.BetTypeId,
                            comItem.Discount / Constant.STEP_MUL_DIV);
                        if (comItem.SportId == Constant.NUMBER_GAME_SPORT_ID && settingContext.EnableNumberGame)
                        {
                            xmlStringData.XMLSetting3rd += tempString;
                        }
                        else
                        {
                            xmlStringData.XMLSetting += tempString;
                        }
                    }
                }
            }

            return xmlStringData;
        }

        /// <summary>
        /// Build XML String for Updating.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="bettingLimits">Betting limits data.</param>
        /// <returns>String data to update database.</returns>
        private XMLStringData BuildBetLimitSettingXML(SettingContext settingContext, IList<BettingLimit> bettingLimits)
        {
            XMLStringData xmlStringData = new XMLStringData();
            string tempString = string.Empty;
            if (bettingLimits != null
                && bettingLimits.Count > 0)
            {
                foreach (BettingLimit betLimit in bettingLimits)
                {
                    if (betLimit.SportId == Constant.NUMBER_GAME_SPORT_ID && settingContext.EnableNumberGame)
                    {
                        xmlStringData.XMLSetting3rd += string.Format(
                            Constant.FORMAT_BETTING_LIMIT_XML_3RD,
                            betLimit.SportId,
                            betLimit.BetTypeId,
                            betLimit.MinBet,
                            betLimit.MaxBet,
                            betLimit.MaxPerMatch,
                            betLimit.MaxPerBall);
                    }
                    else
                    {
                        xmlStringData.XMLSetting += string.Format(
                            Constant.FORMAT_BETTING_LIMIT_XML,
                            betLimit.SportId,
                            betLimit.BetTypeId,
                            betLimit.MinBet,
                            betLimit.MaxBet,
                            betLimit.MaxPerMatch);
                    }
                }
            }

            return xmlStringData;
        }

        /// <summary>
        /// Build XML String for Updating.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="positionTakingSetting">Position taking data.</param>
        /// <returns>String data to update database.</returns>
        private XMLStringData BuildPositionTakingSettingXML(
            SettingContext settingContext, 
            PositionTakingSetting positionTakingSetting)
        {
            XMLStringData xmlStringData = new XMLStringData();
            string tempString = string.Empty;
            string applyOnMemberTempString = string.Empty;
            if (positionTakingSetting != null
                && positionTakingSetting.SportTypes != null
                && positionTakingSetting.SportTypes.Count > 0)
            {
                foreach (SportType sport in positionTakingSetting.SportTypes)
                {
                    foreach (BetTypeGroup betTypeGroup in sport.BetTypeGroups)
                    {
                        foreach (BetType betType in betTypeGroup.BetTypes)
                        {
                            if (betType != null && betType.Pt != null)
                            {
                                if (settingContext.TargetRoleId != (int)Role.Member)
                                {
                                    tempString = string.Format(
                                            Constant.FORMAT_AGENT_POSITION_TAKING_XML,
                                            sport.Id,
                                            betType.Id,
                                            sport.IsLive ? 1 : 0,
                                            (int)(betType.Pt.Preset * Constant.STEP_MUL_DIV),
                                            (int)(betType.Pt.Max * Constant.STEP_MUL_DIV),
                                            (int)(betType.Pt.Min * Constant.STEP_MUL_DIV));
                                }
                                else
                                {
                                    tempString = string.Format(
                                            Constant.FORMAT_MEMBER_POSITION_TAKING_XML,
                                            sport.Id,
                                            betType.Id,
                                            sport.IsLive ? 1 : 0,
                                            (int)(betType.Pt.Value * Constant.STEP_MUL_DIV));
                                }

                                if (sport.Id == Constant.NUMBER_GAME_SPORT_ID && settingContext.EnableNumberGame)
                                {
                                    xmlStringData.XMLSetting3rd += tempString;
                                }
                                else
                                {
                                    if (betType.Pt.IsChangePt)
                                    {
                                        xmlStringData.XMLSetting += tempString;
                                    }
                                    ////if (betType.Pt.IsApplyMember)
                                    ////{
                                    ////if (settingContext.TargetRoleId == (int)Role.Agent)
                                    ////{
                                    ////    presetOnMember.XMLSetting += tempString;
                                    ////}
                                    ////}
                                }
                            }
                        }
                    }
                }
            }

            return xmlStringData;
        }

        /// <summary>
        /// Log Error with more details.
        /// </summary>
        /// <param name="errorMessage">Error message.</param>
        /// <param name="isEditAgent">Current form is edit agent or edit member.</param>
        /// <param name="actorId">Current agent actor id.</param>
        /// <param name="custId">Current customer is being edited.</param>
        /// <param name="adminId">Current admin id.</param>
        /// <param name="commissionXml">Commission XML data.</param>
        /// <param name="betSettingXml">Betting limit XML data.</param>
        /// <param name="ptSettingXml">Position taking setting data.</param>
        /// <returns>Message details string.</returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
        private string BuildLogErrorDetail(
            string errorMessage, 
            bool isEditAgent, 
            int actorId, 
            string custId, 
            int adminId, 
            string commissionXml, 
            string betSettingXml, 
            string ptSettingXml)
        {
            try
            {
                string errorString = "Message: {0} \nForm: {1} \nActor Id: {2} \nCust Id(s): {3} \nAdmin Id: {4} \n"
                    + "CommissionSetting: {5} \n"
                    + "Betting Limit: {6} \n"
                    + "Share Setting: {7} \n";

                return string.Format(
                    errorString,
                    errorMessage,
                    isEditAgent ? "Edit Agent" : "Edit Member(s)",
                    actorId,
                    custId,
                    adminId,
                    commissionXml,
                    betSettingXml,
                    ptSettingXml);
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                return errorMessage;
            }
        }
    }

    /// <summary>
    /// Save XML to Update database.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    internal class XMLStringData
    {
        public XMLStringData()
        {
            XMLSetting = string.Empty;
            XMLSetting3rd = string.Empty;
        }

        public string XMLSetting { get; set; }

        public string XMLSetting3rd { get; set; }
    }
}