// --------------------------------------------------------------------------
// <copyright file="LogController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.5.16">Created</change>
// </history>

namespace Apollo.Age.Account.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using System.Xml.Linq;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Account.Services;
    using Apollo.Age.Account.ViewModels;
    using Apollo.Age.Enumerations;
    using Apollo.Age.PermissionControl.Models;
    using Apollo.Age.Resources;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Web;
    using Fanex.Logging;
    using Apollo.Age.PermissionControl.Entities;

    /// <summary>
    /// Log controller.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class LogController : BaseController
    {
        private readonly ILogRunSpService _logService;

        private readonly IAccountService _accountService;

        private readonly UserService _userServicel;

        private readonly ServiceBase _serviceBase;

        public LogController()
            : this(null, null, null, null)
        {
        }

        public LogController(ILogRunSpService logService, IAccountService accountService, ServiceBase serviceBase, UserService userService)
        {
            _logService = logService ?? new LogRunSpService();
            _accountService = accountService ?? new AccountService();
            _serviceBase = serviceBase ?? new ServiceBase();
            _userServicel = userService ?? new UserService();
        }

        /// <summary>
        /// View log action.
        /// </summary>
        /// <param name="isAgent">Customer is agent.</param>
        /// <param name="username">Username of customer.</param>
        /// <param name="actionTypeId">Log action type id.</param>
        /// <param name="from">View log from date.</param>
        /// <param name="to">View log to date.</param>
        /// <param name="showOldValue">Is show old value before update.</param>
        /// <param name="pageSize">Page size of view.</param>
        /// <param name="pageIndex">Current page index.</param>
        /// <returns>View log details from date to date.</returns>
        public ViewResult ViewLog(
            bool isAgent = true,
            string username = "",
            int actionTypeId = 0,
            DateTime? from = null,
            DateTime? to = null,
            bool showOldValue = true,
            int pageSize = 50,
            int pageIndex = 1)
        {
            IEnumerable<LogRunSpModel> list;
            List<LogRunSpModel> results = new List<LogRunSpModel>();

            var criteria = new GetLogCustSettingCriteria
            {
                ActionTypeId = actionTypeId,

                Username = username,
                ActorAgentId = this.CurrentSession.AgentId,
                From = from == null ? DateTime.Now : (DateTime)from,
                To = to == null ? DateTime.Now : (DateTime)to,
                PageIndex = pageIndex,
                PageSize = pageSize,
                IsAgent = isAgent,
                ShowOldValue = showOldValue
            };

            criteria.ActorAgentId = this.CurrentSession.AgentId;
            criteria.From = new DateTime(criteria.From.Year, criteria.From.Month, criteria.From.Day, 0, 0, 0);
            criteria.To = new DateTime(criteria.To.Year, criteria.To.Month, criteria.To.Day, 23, 59, 59);

            list = _logService.Search(ref criteria);
            results = list != null ? list.ToList() : new List<LogRunSpModel>();

            if (!ReferenceEquals(results, null) && results.Any())
            {
                if (criteria.IsAgent)
                {
                    var agentInfo = _userServicel.InternalGetAgentByUsername(criteria.Username);

                    ViewBag.AgentLevel = agentInfo.Agent.Level;
                }
            }

            ViewBag.LogData = results;
            ViewBag.IsAdmin = this.CurrentSession.AdminId > 0 || this.CurrentSession.AgentLevel == (int)AgentLevel.Super;

            return View(criteria);
        }

        /// <summary>
        /// Log action for discount.
        /// </summary>
        /// <param name="oldData">Data before update.</param>
        /// <param name="data">Date after update.</param>
        /// <returns>Log action view for discount.</returns>
        public ViewResult LogActionType2(string oldData, string data)
        {
            var oldGroup = string.Empty;
            var group = string.Empty;

            var list = new List<DiscountModel>();

            try
            {
                if (!string.IsNullOrWhiteSpace(oldData))
                {
                    var xmlData = XDocument.Parse(oldData);
                    var groupXml = xmlData.Descendants("r");

                    if (!ReferenceEquals(groupXml, null) && groupXml.Any())
                    {
                        foreach (var itemGroup in groupXml)
                        {
                            var discountXml = itemGroup.Descendants("z");

                            if (!ReferenceEquals(itemGroup.Attribute("g"), null))
                            {
                                oldGroup = itemGroup.Attribute("g").Value.ToString();

                                foreach (var itemDiscount in discountXml)
                                {
                                    var model = new DiscountModel
                                    {
                                        SportType = itemDiscount.Attribute("s") != null ? Convert.ToInt32(itemDiscount.Attribute("s").Value) : 0,
                                        BetType = itemDiscount.Attribute("b") != null ? Convert.ToInt32(itemDiscount.Attribute("b").Value) : 0,
                                        GroupA = itemDiscount.Attribute("d") != null ? Convert.ToDecimal(itemDiscount.Attribute("d").Value) : 0,
                                        OldGroupA = itemDiscount.Attribute("d") != null ? Convert.ToDecimal(itemDiscount.Attribute("d").Value) : 0
                                    };

                                    list.Add(model);
                                }
                            }
                            else
                            {
                                foreach (var itemDiscount in discountXml)
                                {
                                    var model = new DiscountModel
                                    {
                                        SportType = itemDiscount.Attribute("s") != null ? Convert.ToInt32(itemDiscount.Attribute("s").Value) : 0,
                                        BetType = itemDiscount.Attribute("b") != null ? Convert.ToInt32(itemDiscount.Attribute("b").Value) : 0,
                                        GroupA = itemDiscount.Attribute("ga") != null ? Convert.ToDecimal(itemDiscount.Attribute("ga").Value) : 0,
                                        OldGroupA = itemDiscount.Attribute("ga") != null ? Convert.ToDecimal(itemDiscount.Attribute("ga").Value) : 0,
                                        GroupB = itemDiscount.Attribute("gb") != null ? Convert.ToDecimal(itemDiscount.Attribute("gb").Value) : 0,
                                        OldGroupB = itemDiscount.Attribute("gb") != null ? Convert.ToDecimal(itemDiscount.Attribute("gb").Value) : 0,
                                        GroupC = itemDiscount.Attribute("gc") != null ? Convert.ToDecimal(itemDiscount.Attribute("gc").Value) : 0,
                                        OldGroupC = itemDiscount.Attribute("gc") != null ? Convert.ToDecimal(itemDiscount.Attribute("gc").Value) : 0,
                                        GroupD = itemDiscount.Attribute("gd") != null ? Convert.ToDecimal(itemDiscount.Attribute("gd").Value) : 0,
                                        OldGroupD = itemDiscount.Attribute("gd") != null ? Convert.ToDecimal(itemDiscount.Attribute("gd").Value) : 0
                                    };

                                    list.Add(model);
                                }
                            }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(data))
                {
                    var xmlData = XDocument.Parse(data);
                    var groupXml = xmlData.Descendants("r");

                    if (!ReferenceEquals(groupXml, null) && groupXml.Any())
                    {
                        foreach (var itemGroup in groupXml)
                        {
                            var discountXml = itemGroup.Descendants("z");

                            if (!ReferenceEquals(itemGroup.Attribute("g"), null))
                            {
                                group = itemGroup.Attribute("g").Value.ToString();

                                foreach (var itemDiscount in discountXml)
                                {
                                    var model = new DiscountModel
                                    {
                                        SportType = itemDiscount.Attribute("s") != null ? Convert.ToInt32(itemDiscount.Attribute("s").Value) : 0,
                                        BetType = itemDiscount.Attribute("b") != null ? Convert.ToInt32(itemDiscount.Attribute("b").Value) : 0,
                                        GroupA = itemDiscount.Attribute("d") != null ? Convert.ToDecimal(itemDiscount.Attribute("d").Value) : 0
                                    };

                                    // Log action for update
                                    for (int i = 0; i < list.Count; i++)
                                    {
                                        var itemOld = list[i];
                                        if (itemOld.SportType == model.SportType
                                            && itemOld.BetType == model.BetType)
                                        {
                                            list[i].GroupA = model.GroupA;
                                        }
                                    }

                                    // Log action for insert
                                    if (string.IsNullOrWhiteSpace(oldData))
                                    {
                                        list.Add(model);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var itemDiscount in discountXml)
                                {
                                    var model = new DiscountModel
                                    {
                                        SportType = itemDiscount.Attribute("s") != null ? Convert.ToInt32(itemDiscount.Attribute("s").Value) : 0,
                                        BetType = itemDiscount.Attribute("b") != null ? Convert.ToInt32(itemDiscount.Attribute("b").Value) : 0,
                                        GroupA = itemDiscount.Attribute("ga") != null ? Convert.ToDecimal(itemDiscount.Attribute("ga").Value) : 0,
                                        GroupB = itemDiscount.Attribute("gb") != null ? Convert.ToDecimal(itemDiscount.Attribute("gb").Value) : 0,
                                        GroupC = itemDiscount.Attribute("gc") != null ? Convert.ToDecimal(itemDiscount.Attribute("gc").Value) : 0,
                                        GroupD = itemDiscount.Attribute("gd") != null ? Convert.ToDecimal(itemDiscount.Attribute("gd").Value) : 0
                                    };

                                    // Log action for update
                                    for (int i = 0; i < list.Count; i++)
                                    {
                                        var itemOld = list[i];
                                        if (itemOld.SportType == model.SportType
                                            && itemOld.BetType == model.BetType)
                                        {
                                            list[i].GroupA = model.GroupA;
                                            list[i].GroupB = model.GroupB;
                                            list[i].GroupC = model.GroupC;
                                            list[i].GroupD = model.GroupD;
                                        }
                                    }

                                    // Log action for insert
                                    if (string.IsNullOrWhiteSpace(oldData))
                                    {
                                        list.Add(model);
                                    }
                                }
                            }
                        }
                    }
                }

                ViewBag.OldGroup = !string.IsNullOrWhiteSpace(oldGroup) ? _logService.RevertGroupName(oldGroup) : oldGroup;
                ViewBag.Group = !string.IsNullOrWhiteSpace(group) ? _logService.RevertGroupName(group) : group;
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message);
            }

            return View(list);
        }

        /// <summary>
        /// Log action for bet setting.
        /// </summary>
        /// <param name="oldData">Data before update.</param>
        /// <param name="data">Date after update.</param>
        /// <returns>Log action view for bet setting.</returns>
        public ViewResult LogActionType3(string oldData, string data)
        {
            var list = new Dictionary<string, BetSettingModel>();

            try
            {
                if (!string.IsNullOrWhiteSpace(oldData))
                {
                    var xmlData = XDocument.Parse(oldData);
                    var betSetting = xmlData.Descendants("z");

                    if (!ReferenceEquals(betSetting, null) && betSetting.Any())
                    {
                        foreach (var item in betSetting)
                        {
                            var model = new BetSettingModel
                            {
                                SportType = item.Attribute("s") != null ? Convert.ToInt32(item.Attribute("s").Value) : 0,
                                BetType = item.Attribute("b") != null ? Convert.ToInt32(item.Attribute("b").Value) : 0,
                                OldMinBet = item.Attribute("mib") != null ? Convert.ToDecimal(item.Attribute("mib").Value) : 0,
                                MinBet = item.Attribute("mib") != null ? Convert.ToDecimal(item.Attribute("mib").Value) : 0,
                                OldMaxBet = item.Attribute("mab") != null ? Convert.ToDecimal(item.Attribute("mab").Value) : 0,
                                MaxBet = item.Attribute("mab") != null ? Convert.ToDecimal(item.Attribute("mab").Value) : 0,
                                OldMaxPerMatch = item.Attribute("mam") != null ? Convert.ToDecimal(item.Attribute("mam").Value) : 0,
                                MaxPerMatch = item.Attribute("mam") != null ? Convert.ToDecimal(item.Attribute("mam").Value) : 0
                            };

                            string key = string.Format("{0}_{1}", model.SportType.ToString(), model.BetType.ToString());

                            list.Add(key, model);
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(data))
                {
                    var xmlData = XDocument.Parse(data);
                    var betSetting = xmlData.Descendants("z");

                    if (!ReferenceEquals(betSetting, null) && betSetting.Any())
                    {
                        foreach (var item in betSetting)
                        {
                            var model = new BetSettingModel
                            {
                                SportType = item.Attribute("s") != null ? Convert.ToInt32(item.Attribute("s").Value) : 0,
                                BetType = item.Attribute("b") != null ? Convert.ToInt32(item.Attribute("b").Value) : 0,
                                MinBet = item.Attribute("mib") != null ? Convert.ToDecimal(item.Attribute("mib").Value) : 0,
                                MaxBet = item.Attribute("mab") != null ? Convert.ToDecimal(item.Attribute("mab").Value) : 0,
                                MaxPerMatch = item.Attribute("mam") != null ? Convert.ToDecimal(item.Attribute("mam").Value) : 0
                            };

                            string key = string.Format("{0}_{1}", model.SportType.ToString(), model.BetType.ToString());

                            // Log action is update
                            if (list.ContainsKey(key))
                            {
                                list[key].MinBet = model.MinBet;
                                list[key].MaxBet = model.MaxBet;
                                list[key].MaxPerMatch = model.MaxPerMatch;
                            }
                            else if (string.IsNullOrWhiteSpace(oldData))
                            {
                                // Log action for insert
                                list.Add(key, model);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message);
            }

            return View(list);
        }

        /// <summary>
        /// Log action for share.
        /// </summary>
        /// <param name="oldData">Data before update.</param>
        /// <param name="data">Date after update.</param>
        /// <param name="agentLevel">Agent level.</param>
        /// <param name="isAgent">Customer is agent.</param>
        /// <returns>Log action view for position taking.</returns>
        public ViewResult LogActionType4(string oldData, string data, int agentLevel, bool isAgent)
        {
            var list = new Dictionary<string, PositionTakingModel>();

            try
            {
                if (!string.IsNullOrWhiteSpace(oldData))
                {
                    var xmlData = XDocument.Parse(oldData);
                    var positionTaking = xmlData.Descendants("z");

                    // Check view log for update all direct member of agent
                    var xmlDirectMember = xmlData.Descendants("r");
                    if (!ReferenceEquals(xmlDirectMember, null))
                    {
                        foreach (var itemRoot in xmlDirectMember)
                        {
                            if (itemRoot.Attribute("a") != null)
                            {
                                isAgent = false;
                            }

                            break;
                        }
                    }

                    if (!ReferenceEquals(positionTaking, null) && positionTaking.Any())
                    {
                        foreach (var item in positionTaking)
                        {
                            if (isAgent)
                            {
                                var model = new PositionTakingModel
                                {
                                    SportType = item.Attribute("s") != null ? Convert.ToInt32(item.Attribute("s").Value) : 0,
                                    BetType = item.Attribute("b") != null ? Convert.ToInt32(item.Attribute("b").Value) : 0,
                                    IsLive = item.Attribute("i") != null ? (item.Attribute("i").Value.ToString() == "1" ? true : false) : false,
                                    MaxUpper = item.Attribute("p") != null ? Convert.ToDecimal(item.Attribute("p").Value) : 0,
                                    OldMaxUpper = item.Attribute("p") != null ? Convert.ToDecimal(item.Attribute("p").Value) : 0,
                                    MaxGiven = item.Attribute("ma") != null ? Convert.ToDecimal(item.Attribute("ma").Value) : 0,
                                    OldMaxGiven = item.Attribute("ma") != null ? Convert.ToDecimal(item.Attribute("ma").Value) : 0,
                                    MinGiven = item.Attribute("mi") != null ? Convert.ToDecimal(item.Attribute("mi").Value) : 0,
                                    OldMinGiven = item.Attribute("mi") != null ? Convert.ToDecimal(item.Attribute("mi").Value) : 0
                                };

                                string key = string.Format("{0}_{1}_{2}", model.SportType.ToString(), model.BetType.ToString(), model.IsLive.ToString().ToLower());

                                list.Add(key, model);
                            }
                            else
                            {
                                var model = new PositionTakingModel
                                {
                                    SportType = item.Attribute("s") != null ? Convert.ToInt32(item.Attribute("s").Value) : 0,
                                    BetType = item.Attribute("b") != null ? Convert.ToInt32(item.Attribute("b").Value) : 0,
                                    IsLive = item.Attribute("i") != null ? (item.Attribute("i").Value.ToString() == "1" ? true : false) : false,
                                    MaxUpper = item.Attribute("p") != null ? Convert.ToDecimal(item.Attribute("p").Value) : 0,
                                    OldMaxUpper = item.Attribute("p") != null ? Convert.ToDecimal(item.Attribute("p").Value) : 0
                                };

                                string key = string.Format("{0}_{1}_{2}", model.SportType.ToString(), model.BetType.ToString(), model.IsLive.ToString().ToLower());

                                list.Add(key, model);
                            }
                        }
                    }
                }

                string agent = string.Empty;

                if (!string.IsNullOrWhiteSpace(data))
                {
                    var xmlDataRoot = XDocument.Parse(data);
                    var xmlData = xmlDataRoot.Descendants("r");

                    foreach (var itemRoot in xmlData)
                    {
                        var positionTaking = itemRoot.Descendants("z");

                        if (!ReferenceEquals(positionTaking, null) && positionTaking.Any())
                        {
                            agent = itemRoot.Attribute("a") != null ? (!isAgent ? itemRoot.Attribute("a").Value : string.Empty) : string.Empty;

                            foreach (var item in positionTaking)
                            {
                                if (isAgent)
                                {
                                    var model = new PositionTakingModel
                                    {
                                        SportType = item.Attribute("s") != null ? Convert.ToInt32(item.Attribute("s").Value) : 0,
                                        BetType = item.Attribute("b") != null ? Convert.ToInt32(item.Attribute("b").Value) : 0,
                                        IsLive = item.Attribute("i") != null ? (item.Attribute("i").Value.ToString() == "1" ? true : false) : false,
                                        MaxUpper = item.Attribute("p") != null ? Convert.ToDecimal(item.Attribute("p").Value) : 0,
                                        MaxGiven = item.Attribute("ma") != null ? Convert.ToDecimal(item.Attribute("ma").Value) : 0,
                                        MinGiven = item.Attribute("mi") != null ? Convert.ToDecimal(item.Attribute("mi").Value) : 0,
                                        IsApply = item.Attribute("ia") != null ? (item.Attribute("ia").Value.ToString() == "1" ? true : false) : false
                                    };

                                    string key = string.Format("{0}_{1}_{2}", model.SportType.ToString(), model.BetType.ToString(), model.IsLive.ToString().ToLower());

                                    // Log action for update
                                    if (list.ContainsKey(key))
                                    {
                                        list[key].MaxUpper = model.MaxUpper;
                                        list[key].MaxGiven = model.MaxGiven;
                                        list[key].MinGiven = model.MinGiven;
                                        list[key].IsApply = model.IsApply;
                                    }
                                    else if (string.IsNullOrWhiteSpace(oldData))
                                    {
                                        // Log action for insert
                                        list.Add(key, model);
                                    }
                                }
                                else
                                {
                                    var model = new PositionTakingModel
                                    {
                                        SportType = item.Attribute("s") != null ? Convert.ToInt32(item.Attribute("s").Value) : 0,
                                        BetType = item.Attribute("b") != null ? Convert.ToInt32(item.Attribute("b").Value) : 0,
                                        IsLive = item.Attribute("i") != null ? (item.Attribute("i").Value.ToString() == "1" ? true : false) : false,
                                        MaxUpper = item.Attribute("p") != null ? Convert.ToDecimal(item.Attribute("p").Value) : 0
                                    };

                                    string key = string.Format("{0}_{1}_{2}", model.SportType.ToString(), model.BetType.ToString(), model.IsLive.ToString().ToLower());

                                    if (list.ContainsKey(key))
                                    {
                                        list[key].MaxUpper = model.MaxUpper;
                                        list[key].MaxGiven = model.MaxGiven;
                                        list[key].MinGiven = model.MinGiven;
                                    }
                                    else if (string.IsNullOrWhiteSpace(oldData))
                                    {
                                        // Log action for insert
                                        list.Add(key, model);
                                    }
                                }
                            }
                        }
                    }
                }

                ViewBag.AgentLevel = agentLevel;
                ViewBag.IsAgent = isAgent;
                ViewBag.Agent = agent;
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message);
            }

            return View(list);
        }

        /// <summary>
        /// Log action for general info.
        /// </summary>
        /// <param name="oldData">Data before update.</param>
        /// <param name="data">Date after update.</param>
        /// <returns>Log action for general information.</returns>
        public ViewResult LogActionType5(string oldData, string data)
        {
            var list = new List<LogSettingViewModel>();

            try
            {
                if (!string.IsNullOrWhiteSpace(data))
                {
                    var xmlData = XDocument.Parse(data);
                    var general = xmlData.Descendants("z");

                    foreach (var item in general)
                    {
                        var username = item.Attribute("u");
                        if (!ReferenceEquals(username, null))
                        {
                            list.Add(new LogSettingViewModel
                            {
                                FieldName = Lang.lblUserName,
                                Value = username.Value
                            });
                        }

                        var agent = item.Attribute("a");
                        if (!ReferenceEquals(agent, null))
                        {
                            list.Add(new LogSettingViewModel
                            {
                                FieldName = Lang.agent,
                                Value = agent.Value
                            });
                        }

                        var parent = item.Attribute("p");
                        if (!ReferenceEquals(parent, null))
                        {
                            list.Add(new LogSettingViewModel
                            {
                                FieldName = Lang.Parent,
                                Value = parent.Value
                            });
                        }

                        var isMaster = item.Attribute("im");
                        if (!ReferenceEquals(isMaster, null))
                        {
                            list.Add(new LogSettingViewModel
                            {
                                FieldName = Lang.IsMaster,
                                Value = string.Compare(isMaster.Value.ToString(), "1") == 0 ? Lang.lblNo : Lang.lblYes
                            });
                        }

                        var currency = item.Attribute("c");
                        if (!ReferenceEquals(currency, null))
                        {
                            list.Add(new LogSettingViewModel
                            {
                                FieldName = Lang.Currency,
                                Value = currency.Value
                            });
                        }

                        var type = item.Attribute("t");
                        var typeName = Lang.status;
                        if (!ReferenceEquals(type, null))
                        {
                            if (string.Compare(type.Value, "1") == 0)
                            {
                                typeName = Lang.status;
                            }
                            else
                            {
                                typeName = Lang.betstatus;
                            }
                        }

                        var status = item.Attribute("s");
                        if (!ReferenceEquals(status, null))
                        {
                            string statusName = GetStringStatus(status.Value);

                            if (!ReferenceEquals(oldData, null))
                            {
                                var xmlOldData = XDocument.Parse(oldData);
                                var generalOld = xmlOldData.Descendants("z");

                                foreach (var itemOld in generalOld)
                                {
                                    var statusOld = itemOld.Attribute("s");
                                    if (!ReferenceEquals(statusOld, null))
                                    {
                                        list.Add(new LogSettingViewModel
                                        {
                                            FieldName = typeName,
                                            Value = statusName,
                                            OldValue = GetStringStatus(statusOld.Value)
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message);
            }

            return View(list);
        }

        /// <summary>
        /// Logs the action permission user control.
        /// </summary>
        /// <param name="oldData">The old data.</param>
        /// <param name="data">The data.</param>
        /// <returns>Log action permission user control/</returns>
        public ViewResult LogActionType9(string oldData, string data)
        {
            var listFunctionPolicy = new List<LogPermissionModel>();

            try
            {
                if (!string.IsNullOrWhiteSpace(oldData))
                {
                    var xmlData = XDocument.Parse(oldData);
                    var permission = xmlData.Descendants("z");

                    if (!ReferenceEquals(permission, null) && permission.Any())
                    {
                        foreach (var itemFunctionPolicy in permission)
                        {
                            var func = new LogPermissionModel
                            {
                                Function = new Function
                                {
                                    FunctionName = GetResourceData(itemFunctionPolicy.Attribute("f") != null ?
                                    itemFunctionPolicy.Attribute("f").Value : string.Empty),
                                    FunctionGroup = new FunctionGroup
                                    {
                                        GroupName = GetResourceData(itemFunctionPolicy.Attribute("fg") != null ?
                                    itemFunctionPolicy.Attribute("fg").Value : string.Empty)
                                    }
                                },
                                OldPolicy = (byte)(itemFunctionPolicy.Attribute("pl") != null ?
                                    Convert.ToByte(itemFunctionPolicy.Attribute("pl").Value) : 0)
                            };

                            listFunctionPolicy.Add(func);
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(data))
                {
                    var xmlData = XDocument.Parse(data);
                    var permission = xmlData.Descendants("z");

                    if (!ReferenceEquals(permission, null) && permission.Any())
                    {
                        foreach (var itemFunctionPolicy in permission)
                        {
                            var func = new LogPermissionModel
                            {
                                Function = new Function
                                {
                                    FunctionName = GetResourceData(itemFunctionPolicy.Attribute("f") != null ?
                                    itemFunctionPolicy.Attribute("f").Value : string.Empty),
                                    FunctionGroup = new FunctionGroup
                                    {
                                        GroupName = GetResourceData(itemFunctionPolicy.Attribute("fg") != null ?
                                    itemFunctionPolicy.Attribute("fg").Value : string.Empty)
                                    }
                                },
                                Policy = (byte)(itemFunctionPolicy.Attribute("pl") != null ?
                                    Convert.ToByte(itemFunctionPolicy.Attribute("pl").Value) : 0)
                            };

                            for (int i = 0; i < listFunctionPolicy.Count; i++)
                            {
                                var item = listFunctionPolicy[i];

                                if (string.Equals(item.Function.FunctionName, func.Function.FunctionName))
                                {
                                    item.Policy = func.Policy;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message);
            }

            return View(listFunctionPolicy);
        }

        /// <summary>
        /// To string status by status id.
        /// </summary>
        /// <param name="value">Status id.</param>
        /// <returns>Name of status.</returns>
        private string GetStringStatus(string value)
        {
            var statusName = string.Empty;

            switch (value)
            {
                case "1":
                    statusName = Lang.lblActive;
                    break;
                case "2":
                    statusName = Lang.stopped;
                    break;
                default:
                    statusName = Lang.locked;
                    break;
            }

            return statusName;
        }
    }
}