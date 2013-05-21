// --------------------------------------------------------------------------
// <copyright file="LogRunSpModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.5.16">Created</change>
// </history>

namespace Apollo.Age.Account.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Web;
    using System.Xml.Linq;
    using Apollo.Age.Account.Core;
    using Fanex.Logging;
    using RazorEngine;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class LogRunSpModel
    {
        public long LogId { get; set; }

        public DateTime LogDate { get; set; }

        public string Username { get; set; }

        public int ActionTypeId { get; set; }

        public string ActionName { get; set; }

        public string OldData { get; set; }

        public string Data { get; set; }

        public string GetContent(bool isShowOldValue, int agentLevel, bool isAgent)
        {
            string result = string.Empty;

            //<r><z s="1" b="1" mib="270.0000" mab="800.0000" mam="8000.0000"/></r>
            if (this.ActionTypeId == Constants.LogActionBetSetting)
            {
                var listBetSetting = this.GetBetSetting();
                var bodyTemplate = ReadTextFile(HttpContext.Current.Server.MapPath("~/Views/Log/Templates/BetSetting.cshtml"));

                RazorEngine.Templating.DynamicViewBag viewBag = new RazorEngine.Templating.DynamicViewBag();
                viewBag.AddValue("IsShowOldValue", isShowOldValue);

                result = Razor.Parse(bodyTemplate, listBetSetting, viewBag, null);
            }

            //<r><z s="1" b="1" i="0" p="800" ma="8000" mi="0"/></r>
            if (this.ActionTypeId == Constants.LogActionPositionTaking)
            {
                var listPositionTaking = this.GetPositionTaking(isAgent);
                var bodyTemplate = ReadTextFile(HttpContext.Current.Server.MapPath("~/Views/Log/Templates/PositionTaking.cshtml"));

                RazorEngine.Templating.DynamicViewBag viewBag = new RazorEngine.Templating.DynamicViewBag();
                viewBag.AddValue("IsShowOldValue", isShowOldValue);
                viewBag.AddValue("AgentLevel", agentLevel);
                viewBag.AddValue("IsAgent", isAgent);

                result = Razor.Parse(bodyTemplate, listPositionTaking, viewBag, null);
            }

            //<r><z s="1" b="1" ga="0.025" gb="0.0075" gc="0" gd="0"/></r>
            //or
            //<r><z s="1" b="1" d="0.025"/></r>
            if (this.ActionTypeId == Constants.LogActionDiscount)
            {
                string oldGroup = string.Empty;
                string group = string.Empty;
                string bodyTemplate = string.Empty;

                var listDiscount = this.GetDiscount(ref oldGroup, ref group);

                if (!string.IsNullOrWhiteSpace(oldGroup) || !string.IsNullOrWhiteSpace(group))
                {
                    bodyTemplate = ReadTextFile(HttpContext.Current.Server.MapPath("~/Views/Log/Templates/CustomerDiscount.cshtml"));
                }

                if (string.IsNullOrWhiteSpace(oldGroup) && string.IsNullOrWhiteSpace(group))
                {
                    bodyTemplate = ReadTextFile(HttpContext.Current.Server.MapPath("~/Views/Log/Templates/AgentDiscount.cshtml"));
                }

                RazorEngine.Templating.DynamicViewBag viewBag = new RazorEngine.Templating.DynamicViewBag();
                viewBag.AddValue("IsShowOldValue", isShowOldValue);
                viewBag.AddValue("OldGroup", oldGroup);
                viewBag.AddValue("Group", group);

                result = Razor.Parse(bodyTemplate, listDiscount, viewBag, null);
            }

            return result;
        }

        private List<Apollo.Age.Account.Models.DiscountModel> GetDiscount(ref string oldGroup, ref string group)
        {
            var list = new List<Apollo.Age.Account.Models.DiscountModel>();

            if (!string.IsNullOrWhiteSpace(this.OldData))
            {
                var xmlData = XDocument.Parse(this.OldData);
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

            if (!string.IsNullOrWhiteSpace(this.Data))
            {
                var xmlData = XDocument.Parse(this.Data);
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

                            for (int i = 0; i < list.Count; i++)
                            {
                                var itemOld = list[i];
                                if (itemOld.SportType == model.SportType
                                    && itemOld.BetType == model.BetType)
                                {
                                    list[i].GroupA = model.GroupA;
                                }
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

                            for (int i = 0; i < list.Count; i++)
                            {
                                var itemOld = list[i];
                                if (itemOld.SportType == model.SportType
                                    && itemOld.BetType == model.BetType)
                                {
                                    list[i].GroupA = model.GroupA;
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        private List<Apollo.Age.Account.Models.PositionTakingModel> GetPositionTaking(bool isAgent)
        {
            var list = new List<Apollo.Age.Account.Models.PositionTakingModel>();

            if (!string.IsNullOrWhiteSpace(this.OldData))
            {
                var xmlData = XDocument.Parse(this.OldData);
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

                        list.Add(model);
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

                        list.Add(model);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(this.Data))
            {
                var xmlData = XDocument.Parse(this.Data);
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

                        for (int i = 0; i < list.Count; i++)
                        {
                            var itemOld = list[i];
                            if (itemOld.SportType == model.SportType
                                && itemOld.BetType == model.BetType
                                && itemOld.IsLive == model.IsLive)
                            {
                                list[i].MaxUpper = model.MaxUpper;
                                list[i].MaxGiven = model.MaxGiven;
                                list[i].MinGiven = model.MinGiven;
                            }
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

                        for (int i = 0; i < list.Count; i++)
                        {
                            var itemOld = list[i];
                            if (itemOld.SportType == model.SportType
                                && itemOld.BetType == model.BetType
                                && itemOld.IsLive == model.IsLive)
                            {
                                list[i].MaxUpper = model.MaxUpper;
                            }
                        }
                    }
                }
            }

            return list;
        }

        private List<Apollo.Age.Account.Models.BetSettingModel> GetBetSetting()
        {
            var list = new List<BetSettingModel>();

            if (!string.IsNullOrWhiteSpace(this.OldData))
            {
                var xmlData = XDocument.Parse(this.OldData);
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

                    list.Add(model);
                }
            }

            if (!string.IsNullOrWhiteSpace(this.Data))
            {
                var xmlData = XDocument.Parse(this.Data);
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

                    for (int i = 0; i < list.Count; i++)
                    {
                        var itemOld = list[i];
                        if (itemOld.SportType == model.SportType
                            && itemOld.BetType == model.BetType)
                        {
                            list[i].MinBet = model.MinBet;
                            list[i].MaxBet = model.MaxBet;
                            list[i].MaxPerMatch = model.MaxPerMatch;
                        }
                    }
                }
            }

            return list;
        }

        private string ReadTextFile(string filePath)
        {
            try
            {
                using (var sr = new StreamReader(filePath))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                return string.Empty;
            }
        }
    }
}
