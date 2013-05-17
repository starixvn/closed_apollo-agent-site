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
    using System.Diagnostics.CodeAnalysis;
    using System.Collections.Generic;
    using Apollo.Age.Account.Core;
    using System.Xml.Linq;
    using System.IO;
    using Fanex.Logging;
    using System.Web;
    using RazorEngine;


    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class LogRunSpModel
    {
        public long LogId { get; set; }

        public DateTime LogDate { get; set; }

        public string Username { get; set; }

        public int ActionId { get; set; }

        public string ActionName { get; set; }

        public string OldData { get; set; }

        public string Data { get; set; }

        public string Content
        {
            get
            {
                string result = string.Empty;

                //<r><z s="1" b="1" mib="270.0000" mab="800.0000" mam="8000.0000"/></r>
                if (this.ActionId == Constants.LOGACTIONBETSETTING)
                {
                    var listBetSetting = this.GetBetSetting();
                    var bodyTemplate = ReadTextFile(HttpContext.Current.Server.MapPath("~/Views/Log/Templates/BetSetting.cshtml"));

                    result = Razor.Parse(bodyTemplate, listBetSetting);
                }

                return result;
            }
        }

        private List<BetSettingModel> GetBetSetting()
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
                        OldMinBet = Convert.ToDecimal(item.Attribute("mib").Value),
                        OldMaxBet = Convert.ToDecimal(item.Attribute("mab").Value),
                        OldMaxPerMatch = Convert.ToDecimal(item.Attribute("mam").Value),
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
