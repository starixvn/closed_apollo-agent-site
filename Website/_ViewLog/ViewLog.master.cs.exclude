using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Sunplus.Shared.ViewLogLib;
using Sunplus.Shared.ViewLogLib.Services;
using Sunplus.Shared.ViewLogLib.Entities;
using Fanex.Age.UI.Components;

namespace Sunplus.Agent.UI._ViewLog
{
    using Sunplus.Agent.Model.Entities;

    public partial class ViewLog_Page : System.Web.UI.MasterPage
    {
        public IUserSession CurrentUserSession
        {
            get
            {
                return UserSession.Current;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!UserSession.Current.SubPageAccess.ViewLog)
                    Response.Redirect("../AccessDenied.aspx");

                if (Request.QueryString["tabid"] != null && Request.QueryString["tabid"].ToString().Equals("CustomerSetting", StringComparison.OrdinalIgnoreCase))
                {
                    LoadActionType();
                }

                if (CurrentUserSession.RoleId == 2 && Request.QueryString["tabid"].ToLower() == "login")
                {
                    divUserName.Style.Add("display", "none");
                }


                DateRangeSelect_Control DateRangeSelect = new DateRangeSelect_Control(null, false, DateTime.Today, DateTime.Today);

                if ((Request["todate"] == null && Request["fromdate"] == null) && DateTime.Today.Day == 1)
                {
                    DateRangeSelect.InitializeDropdownList(DateTime.Today, DateTime.Today, NumberOfMonth());
                }
                else
                {
                    DateRangeSelect.InitializeDropdownList(Request["fromdate"] == null ? DateTime.Today.AddDays(-1) : DateTime.Parse(Request["fromdate"]), Request["todate"] == null ? DateTime.Today : DateTime.Parse(Request["todate"]), NumberOfMonth());
                }
                DateRangeSelect.HideTodayYesterdayButton = true;

                ltrDate.Text = DateRangeSelect.ToString();

            }

            // Hot fix for Opera browser (IIS does sniffing user agent)
            Response.ContentType = "text/html";

            base.OnLoad(e);
        }

        protected int NumberOfMonth()
        {
            DateTime startdate = new DateTime(2011, 1, 1);
            DateTime enddate = DateTime.Today;

            int M = Math.Abs((startdate.Year - enddate.Year));
            int numofmonth = ((M * 12) + Math.Abs((startdate.Month - enddate.Month)));
            return numofmonth;
        }

        protected void LoadActionType()
        {
            List<LogActionType> list = (new LogActionTypeService()).GetAll();

            if (!ReferenceEquals(list, null))
            {
                ddlActionType.DataValueField = "ActionTypeID";
                ddlActionType.DataTextField = "ActionTypeName" + PageBase.ToLanguageCode(Sunplus.Agent.Model.Entities.UserSession.Current.LangKey).ToString();
                ddlActionType.DataSource = list;
                ddlActionType.DataBind();

                //add first dropdownlist item (all)
                var listItem = new ListItem { Text = Resources.Viewlog.All, Value = "-1" };
                ddlActionType.Items.Insert(0, listItem);
            }
        }

        /// <summary>
        /// sub account doest not have update permission can
        ///  view loglogin.aspx only
        /// </summary>
        public bool VisibleMenuItem
        {
            get
            {
                ViewLogPageBase viewLogPageBase = (this.Page as ViewLogPageBase);
                if (viewLogPageBase != null)
                {
                    return !viewLogPageBase.IsSubAaccountAndCannotViewLog;
                }
                return false;
            }
        }
    }
}
