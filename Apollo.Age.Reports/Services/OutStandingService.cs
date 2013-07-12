// --------------------------------------------------------------------------
// <copyright file="OutstandingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.12">Add Export Excel</change>
// </history>

namespace Apollo.Age.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Services.Impl;
    using NPOI.HSSF.UserModel;
    using Apollo.Age.Reports.Criterias;
    using NPOI.SS.UserModel;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.Models;


    public class OutstandingService : ReportService
    {
        public OutstandingViewModel GetAgentOutStadingReport(bool isSupper, int? CurrentAgentId, int parentId, int? agentId, string sporttypes, string bettypes)
        {
            
            var mainShareHolder = new Apollo.Age.Entities.Agent();

            var outstandingViewModel = new OutstandingViewModel()
            {
                CurrentAgentId = CurrentAgentId.Value,
                MainShareHolder = mainShareHolder,
                Sporttypes = sporttypes,
                Bettypes = bettypes
            };

            if (sporttypes != null)
            {
                IEnumerable<Agent> modelAgentDownLine = AgentDownlinePath(parentId, CurrentAgentId.Value);

                if (isSupper)
                {
                    if (modelAgentDownLine.Count() > 1)
                    {
                        mainShareHolder = modelAgentDownLine.ToList()[1];
                    }
                }

                IEnumerable<OutstandingModel> models = GetData<OutstandingModel>(new OutstandingCriteria
                {
                    AgentId = CurrentAgentId.Value,
                    Sporttypes = sporttypes,
                    Bettypes = bettypes
                });

                outstandingViewModel.ListOutStanding = models;
                outstandingViewModel.DownlinePath = modelAgentDownLine;
                outstandingViewModel.AgentLevel = modelAgentDownLine.LastOrDefault().Level;
            }

            outstandingViewModel.SportList = SingbetSportService.GetSport(isOther: false);
            outstandingViewModel.BettypeList = GetBettype();
            return outstandingViewModel;
        }

        public HSSFWorkbook ExportExcel(OutstandingCriteria outstandingCriteria)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet = hssfworkbook.CreateSheet("Out Standing Report");


            return null;
        }
    }
}