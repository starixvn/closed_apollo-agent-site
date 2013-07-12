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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSupper"></param>
        /// <param name="CurrentAgentId"></param>
        /// <param name="parentId"></param>
        /// <param name="agentId"></param>
        /// <param name="sporttypes"></param>
        /// <param name="bettypes"></param>
        /// <returns></returns>
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

        public MemberOutstadingViewModel GetMemberOutstadingReport(int? CurrentAgentId, int? agentId, string sporttypes, string bettypes, int mainShareHolderId, string mainShareHolderUsername)
        {
            IEnumerable<OutstandingModel> models = GetData<OutstandingModel>(new MemberOutstandingCriteria
            {
                AgentId = CurrentAgentId.Value,
                Sporttypes = sporttypes,
                Bettypes = bettypes
            });

            //// Sum total amount
            OutstandingModel totalModels = new OutstandingModel();
            if (models != null && models.Any())
            {
                foreach (OutstandingModel item in models)
                {
                    totalModels.Outstanding += item.Outstanding;
                    totalModels.ShareAmount += item.ShareAmount;
                    totalModels.GameOutstanding += item.GameOutstanding;
                    totalModels.GameShareAmount += item.GameShareAmount;
                }
            }

            return new MemberOutstadingViewModel
            {
                ListOutstanding = models,
				Total = totalModels,
				MainShareHolderId = mainShareHolderId,
				MainShareHolderUsername = mainShareHolderUsername
            };
        }

        public HSSFWorkbook ExportAgentOutstandingReport(OutstandingViewModel outstandingViewModel)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet = hssfworkbook.CreateSheet("Out Standing Report");

            if (outstandingViewModel != null
                && outstandingViewModel.ListOutStanding != null
                && outstandingViewModel.ListOutStanding.Any())
            {
                var length = outstandingViewModel.ListOutStanding.Count();
                for (int i = 0; i < length; i++)
                {
                    IRow row = sheet.CreateRow(i);
                    var userNameCell = row.CreateCell(0);
                    userNameCell.SetCellType(CellType.STRING);
                    userNameCell.SetCellValue(outstandingViewModel.ListOutStanding.ElementAt(i).UserName);
                    var outstadingValue = row.CreateCell(1);
                    outstadingValue.SetCellType(CellType.NUMERIC);
                    outstadingValue.SetCellValue((double)outstandingViewModel.ListOutStanding.ElementAt(i).Outstanding);
                    var shareAmountValue = row.CreateCell(2);
                    shareAmountValue.SetCellType(CellType.NUMERIC);
                    shareAmountValue.SetCellValue((double)outstandingViewModel.ListOutStanding.ElementAt(i).ShareAmount);
                }
            }

            return hssfworkbook;
        }
    }
}