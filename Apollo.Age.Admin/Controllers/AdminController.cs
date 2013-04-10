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
    using System.Linq;
    using System.Web.Mvc;
    using Apollo.Age.Admin.Criterias;
    using Apollo.Age.Admin.Services;
    using Apollo.Age.Web;
    using System.Threading;

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

        public JsonResult InsertExchangeRate(string currency, string description,
            double multipliedBy, double baseRate, double minBet, double minBetLive)
        {
            int errCode = 0;
            string errMsg = string.Empty;

            if (string.IsNullOrWhiteSpace(currency))
            {
                errCode = 20;
                errMsg = string.Format(Resources.Lang.FieldIsRequiredTpl, Resources.Lang.currency);
            }
            else // checks if currency existed
            {
                currency = currency.Trim();

                var getExchRateByCurrencyCriteria =
                    new GetExchangeRateByCurrencyCriteria { Currency = currency };
                var exchRateToUpdate =
                    _exchangeRateService.GetByCurrency(ref getExchRateByCurrencyCriteria);

                if (exchRateToUpdate != null)
                {
                    errCode = 30;
                    errMsg = string.Format(Resources.Lang.NameExistedTpl, currency);
                }
                else
                {
                    var criteria = new InsertExchangeRateCriteria
                    {
                        Currency = currency,
                        UserId = CurrentSession.UserId,
                        IP = RequestIP,
                        Description = description,
                        MultipliedBy = multipliedBy,
                        BaseRate = baseRate,
                        MinBet = minBet,
                        MinBetLive = minBetLive
                    };

                    _exchangeRateService.InsertExchangeRate(ref criteria);

                    errCode = criteria.ErrCode;
                    errMsg = criteria.ErrMsg;
                }
            }

            return Json(new { errCode = errCode, errMsg = errMsg }, JsonRequestBehavior.AllowGet);
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
                var criteria = new UpdateExchangeRateCriteria
                {
                    UserId = CurrentSession.UserId,
                    IP = RequestIP,
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

            return Json(new { errCode = errCode, errMsg = errMsg }, JsonRequestBehavior.AllowGet);
        }
    }
}