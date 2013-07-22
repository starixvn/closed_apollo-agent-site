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
    using Apollo.Age.Web;
    using Apollo.Age.Account.ViewModels;
    using Apollo.Age.Resources;
    using Apollo.Age.Services.Impl;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class LogController : BaseController
    {
        private ILogRunSpService _logService;

        private IAccountService _accountService;

        private ServiceBase _serviceBase;

        public LogController()
            : this(null, null, null)
        {
        }

        public LogController(ILogRunSpService logService, IAccountService accountService, ServiceBase serviceBase)
        {
            _logService = logService ?? new LogRunSpService();
            _accountService = accountService ?? new AccountService();
            _serviceBase = serviceBase ?? new ServiceBase();
        }

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
                    var agentInfo = _accountService.GetAgentInfo(new GetAgentInfoCriteria
                    {
                        ActorAgentId = this.CurrentSession.AgentId,
                        ExcludeActor = false,
                        Username = criteria.Username
                    });

                    ViewBag.AgentLevel = agentInfo.Level;
                }
            }

            ViewBag.LogData = results;
            ViewBag.IsAdmin = this.CurrentSession.AdminId > 0;

            return View(criteria);
        }

        /// <summary>
        /// Log action for discount
        /// </summary>
        /// <param name="oldData"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public ViewResult LogActionType2(string oldData, string data)
        {
            var oldGroup = string.Empty;
            var group = string.Empty;

            var list = new List<DiscountModel>();

            if (!string.IsNullOrWhiteSpace(oldData))
            {
                var xmlData = XDocument.Parse(oldData);
                var groupXml = xmlData.Descendants("r");

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
                                SportType = Convert.ToInt32(itemDiscount.Attribute("s").Value),
                                BetType = Convert.ToInt32(itemDiscount.Attribute("b").Value),
                                GroupA = Convert.ToDecimal(itemDiscount.Attribute("d").Value),
                                OldGroupA = Convert.ToDecimal(itemDiscount.Attribute("d").Value)
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
                                SportType = Convert.ToInt32(itemDiscount.Attribute("s").Value),
                                BetType = Convert.ToInt32(itemDiscount.Attribute("b").Value),
                                GroupA = Convert.ToDecimal(itemDiscount.Attribute("ga").Value),
                                OldGroupA = Convert.ToDecimal(itemDiscount.Attribute("ga").Value),
                                GroupB = Convert.ToDecimal(itemDiscount.Attribute("gb").Value),
                                OldGroupB = Convert.ToDecimal(itemDiscount.Attribute("gb").Value),
                                GroupC = Convert.ToDecimal(itemDiscount.Attribute("gc").Value),
                                OldGroupC = Convert.ToDecimal(itemDiscount.Attribute("gc").Value),
                                GroupD = Convert.ToDecimal(itemDiscount.Attribute("gd").Value),
                                OldGroupD = Convert.ToDecimal(itemDiscount.Attribute("gd").Value)
                            };

                            list.Add(model);
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(data))
            {
                var xmlData = XDocument.Parse(data);
                var groupXml = xmlData.Descendants("r");

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
                                SportType = Convert.ToInt32(itemDiscount.Attribute("s").Value),
                                BetType = Convert.ToInt32(itemDiscount.Attribute("b").Value),
                                GroupA = Convert.ToDecimal(itemDiscount.Attribute("d").Value)
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
                                SportType = Convert.ToInt32(itemDiscount.Attribute("s").Value),
                                BetType = Convert.ToInt32(itemDiscount.Attribute("b").Value),
                                GroupA = Convert.ToDecimal(itemDiscount.Attribute("ga").Value),
                                GroupB = Convert.ToDecimal(itemDiscount.Attribute("gb").Value),
                                GroupC = Convert.ToDecimal(itemDiscount.Attribute("gc").Value),
                                GroupD = Convert.ToDecimal(itemDiscount.Attribute("gd").Value)
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

            ViewBag.OldGroup = !string.IsNullOrWhiteSpace(oldGroup) ? _logService.RevertCommGroupName(oldGroup) : oldData;
            ViewBag.Group = !string.IsNullOrWhiteSpace(group) ? _logService.RevertCommGroupName(group) : group;

            return View(list);
        }

        /// <summary>
        /// Log action for bet setting
        /// </summary>
        /// <param name="oldData"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public ViewResult LogActionType3(string oldData, string data)
        {
            var list = new Dictionary<string, BetSettingModel>();

            if (!string.IsNullOrWhiteSpace(oldData))
            {
                var xmlData = XDocument.Parse(oldData);
                var betSetting = xmlData.Descendants("z");

                foreach (var item in betSetting)
                {
                    var model = new BetSettingModel
                    {
                        SportType = Convert.ToInt32(item.Attribute("s").Value),
                        BetType = Convert.ToInt32(item.Attribute("b").Value),
                        OldMinBet = Convert.ToDecimal(item.Attribute("mib").Value),
                        MinBet = Convert.ToDecimal(item.Attribute("mib").Value),
                        OldMaxBet = Convert.ToDecimal(item.Attribute("mab").Value),
                        MaxBet = Convert.ToDecimal(item.Attribute("mab").Value),
                        OldMaxPerMatch = Convert.ToDecimal(item.Attribute("mam").Value),
                        MaxPerMatch = Convert.ToDecimal(item.Attribute("mam").Value)
                    };

                    string key = string.Format("{0}_{1}", model.SportType.ToString(), model.BetType.ToString());

                    list.Add(key, model);
                }
            }

            if (!string.IsNullOrWhiteSpace(data))
            {
                var xmlData = XDocument.Parse(data);
                var betSetting = xmlData.Descendants("z");

                foreach (var item in betSetting)
                {
                    var model = new BetSettingModel
                    {
                        SportType = Convert.ToInt32(item.Attribute("s").Value),
                        BetType = Convert.ToInt32(item.Attribute("b").Value),
                        MinBet = Convert.ToDecimal(item.Attribute("mib").Value),
                        MaxBet = Convert.ToDecimal(item.Attribute("mab").Value),
                        MaxPerMatch = Convert.ToDecimal(item.Attribute("mam").Value),
                    };

                    string key = string.Format("{0}_{1}", model.SportType.ToString(), model.BetType.ToString());

                    // Log action is update
                    if (list.ContainsKey(key))
                    {
                        list[key].MinBet = model.MinBet;
                        list[key].MaxBet = model.MaxBet;
                        list[key].MaxPerMatch = model.MaxPerMatch;
                    }
                    else if (string.IsNullOrWhiteSpace(oldData))    // Log action for insert
                    {
                        list.Add(key, model);
                    }
                }
            }

            return View(list);
        }

        /// <summary>
        /// Log action for share
        /// </summary>
        /// <param name="oldData"></param>
        /// <param name="data"></param>
        /// <param name="agentLevel"></param>
        /// <param name="isAgent"></param>
        /// <returns></returns>
        public ViewResult LogActionType4(string oldData, string data, int agentLevel, bool isAgent)
        {
            var list = new Dictionary<string, PositionTakingModel>();

            if (!string.IsNullOrWhiteSpace(oldData))
            {
                var xmlData = XDocument.Parse(oldData);
                var positionTaking = xmlData.Descendants("z");

                foreach (var item in positionTaking)
                {
                    if (isAgent)
                    {
                        var model = new PositionTakingModel
                        {
                            SportType = Convert.ToInt32(item.Attribute("s").Value),
                            BetType = Convert.ToInt32(item.Attribute("b").Value),
                            IsLive = item.Attribute("i").Value.ToString() == "1" ? true : false,
                            MaxUpper = Convert.ToDecimal(item.Attribute("p").Value),
                            OldMaxUpper = Convert.ToDecimal(item.Attribute("p").Value),
                            MaxGiven = Convert.ToDecimal(item.Attribute("ma").Value),
                            OldMaxGiven = Convert.ToDecimal(item.Attribute("ma").Value),
                            MinGiven = Convert.ToDecimal(item.Attribute("mi").Value),
                            OldMinGiven = Convert.ToDecimal(item.Attribute("mi").Value)
                        };

                        string key = string.Format("{0}_{1}_{2}", model.SportType.ToString(), model.BetType.ToString(), model.IsLive.ToString().ToLower());

                        list.Add(key, model);
                    }
                    else
                    {
                        var model = new PositionTakingModel
                        {
                            SportType = Convert.ToInt32(item.Attribute("s").Value),
                            BetType = Convert.ToInt32(item.Attribute("b").Value),
                            IsLive = item.Attribute("i").Value.ToString() == "1" ? true : false,
                            MaxUpper = Convert.ToDecimal(item.Attribute("p").Value),
                            OldMaxUpper = Convert.ToDecimal(item.Attribute("p").Value)
                        };

                        string key = string.Format("{0}_{1}_{2}", model.SportType.ToString(), model.BetType.ToString(), model.IsLive.ToString().ToLower());

                        list.Add(key, model);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(data))
            {
                var xmlData = XDocument.Parse(data);
                var positionTaking = xmlData.Descendants("z");

                foreach (var item in positionTaking)
                {
                    if (isAgent)
                    {
                        var model = new PositionTakingModel
                        {
                            SportType = Convert.ToInt32(item.Attribute("s").Value),
                            BetType = Convert.ToInt32(item.Attribute("b").Value),
                            IsLive = item.Attribute("i").Value.ToString() == "1" ? true : false,
                            MaxUpper = Convert.ToDecimal(item.Attribute("p").Value),
                            MaxGiven = Convert.ToDecimal(item.Attribute("ma").Value),
                            MinGiven = Convert.ToDecimal(item.Attribute("mi").Value)
                        };

                        string key = string.Format("{0}_{1}_{2}", model.SportType.ToString(), model.BetType.ToString(), model.IsLive.ToString().ToLower());

                        // Log action for update
                        if (list.ContainsKey(key))
                        {
                            list[key].MaxUpper = model.MaxUpper;
                            list[key].MaxGiven = model.MaxGiven;
                            list[key].MinGiven = model.MinGiven;
                        }
                        else if (string.IsNullOrWhiteSpace(oldData))    // Log action for insert
                        {
                            list.Add(key, model);
                        }
                    }
                    else
                    {
                        var model = new PositionTakingModel
                        {
                            SportType = Convert.ToInt32(item.Attribute("s").Value),
                            BetType = Convert.ToInt32(item.Attribute("b").Value),
                            IsLive = item.Attribute("i").Value.ToString() == "1" ? true : false,
                            MaxUpper = Convert.ToDecimal(item.Attribute("p").Value)
                        };

                        string key = string.Format("{0}_{1}_{2}", model.SportType.ToString(), model.BetType.ToString(), model.IsLive.ToString().ToLower());

                        if (list.ContainsKey(key))
                        {
                            list[key].MaxUpper = model.MaxUpper;
                            list[key].MaxGiven = model.MaxGiven;
                            list[key].MinGiven = model.MinGiven;
                        }
                        else if (string.IsNullOrWhiteSpace(oldData))    // Log action for insert
                        {
                            list.Add(key, model);
                        }
                    }
                }
            }

            ViewBag.AgentLevel = agentLevel;
            ViewBag.IsAgent = isAgent;

            return View(list);
        }

        /// <summary>
        /// Log action for general info
        /// </summary>
        /// <param name="oldData"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public ViewResult LogActionType5(string oldData, string data)
        {
            var list = new List<LogSettingViewModel>();
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
                            Value = string.Compare(isMaster.Value.ToString(), "1") == 0 ? Lang.lblYes : Lang.lblNo
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

            return View(list);
        }

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