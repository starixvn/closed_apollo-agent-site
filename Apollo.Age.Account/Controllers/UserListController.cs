namespace Apollo.Age.Account.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Services;
    using Apollo.Age.Account.ViewModels;
    using Apollo.Age.Web;

    public class UserListController : BaseController
    {
        private readonly UserListService _userListService;

        public UserListController()
            : this(null)
        {
        }

        public UserListController(UserListService userListService = null)
        {
            _userListService = userListService ?? new UserListService();
        }

        public ActionResult AgentList(int agentId, int pageIndex, int totalPages, string userName, int pageSize)
        {
            AgentListCriteria criteria = new AgentListCriteria()
            {
                AgentId = agentId,
                PageIndex = pageIndex,
                TotalPages = totalPages,
                UserName = userName,
                PageSize = pageSize
            };

            IEnumerable<AgentListViewModel> models = _userListService.GetAgentList(ref criteria);

            return Json(new
                    {
                        TotalPage = criteria.TotalPages,
                        ListData = models
                    });
        }

        public ActionResult CustomerList(int agentId, int pageIndex, int totalPages, string userName, int pageSize)
        {
            CustomerListCriteria criteria = new CustomerListCriteria()
            {
                AgentId = agentId,
                PageIndex = pageIndex,
                TotalPages = totalPages,
                UserName = userName,
                PageSize = pageSize
            };

            IEnumerable<CustomerListViewModel> models = _userListService.GetCustomerList(ref criteria);

            return Json(new
            {
                TotalPage = criteria.TotalPages,
                ListData = models
            });
        }
    }
}