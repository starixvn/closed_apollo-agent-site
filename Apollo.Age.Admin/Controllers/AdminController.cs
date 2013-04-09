// --------------------------------------------------------------------------
// <copyright file="AdminController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.4.9">Created</change>
// </history>

namespace Apollo.Age.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Apollo.Age.Web;
    using Sunplus.Agent.Model.Entities;
    using Apollo.Age.Admin.Services;
    using Apollo.Age.DTO.Admin.ExchangeRate;
    using Apollo.Age.Admin.Criterias;

    public class AdminController : BaseController
    {
        private readonly IExchangeRateService _exchangeRateService;

        public AdminController()
            : this(null) { }

        public AdminController(IExchangeRateService adminService)
        {
            _exchangeRateService = adminService ?? new ExchangeRateService();
        }

        public ActionResult ExchangeRate()
        {
            var criteria = new GetAllExchangeRateCriteria();
            var exchangeRateList = _exchangeRateService.GetAll(ref criteria);

            ViewBag.UserId = CurrentSession.UserId;

            return View(exchangeRateList.ToList());
        }

        public JsonResult UpdateExchangeRate(int exchangeId, string description,
            double multipliedBy, double baseRate, double minBet, double minBetLive)
        {
            var getExchRateByIdCriteria = new GetExchangeRateByIdCriteria { ExchangeId = exchangeId };
            var exchRateToUpdate = _exchangeRateService.GetById(ref getExchRateByIdCriteria);
            int errCode = 0;
            string errMsg = string.Empty;

            if (exchRateToUpdate == null)
            {
                // return not found error
                errCode = 404;
                errMsg = string.Empty;
            }
            else
            {
                //description = p["description"] == null ? string.Empty : HttpUtility.HtmlDecode(p["description"].ToString());

                var criteria = new UpdateExchangeRateCriteria
                {
                    userId = CurrentSession.UserId,
                    ip = "",
                    ExchangeId = exchangeId,
                    Description = description,
                    MultipliedBy = multipliedBy,
                    BaseRate = baseRate,
                    MinBet = minBet,
                    MinBetLive = minBetLive
                };

                _exchangeRateService.UpdateExchangeRate(ref criteria);

                errCode = criteria.ErrCode;
                errMsg = criteria.ErrMsg;
            }

            return Json(new { errCode = errCode, errMsg = errMsg });
        }
    }
}