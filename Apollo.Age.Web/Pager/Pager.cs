// --------------------------------------------------------------------------
// <copyright file="Pager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Apollo.Age.Web.Pager
{
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;

    /// <summary>
    /// Pager control.
    /// </summary>
    public class Pager : IHtmlString
    {
        private readonly HtmlHelper _htmlHelper;
        private readonly int _pageSize;
        private readonly int _currentPage;
        private readonly int _totalItemCount;
        private readonly PagerOptions _pagerOptions;

        public Pager(HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount)
        {
            this._htmlHelper = htmlHelper;
            this._pageSize = pageSize;
            this._currentPage = currentPage;
            this._totalItemCount = totalItemCount;
            this._pagerOptions = new PagerOptions();
        }

        public Pager Options(Action<PagerOptionsBuilder> buildOptions)
        {
            buildOptions(new PagerOptionsBuilder(this._pagerOptions));
            return this;
        }

        public PaginationModel BuildPaginationModel(Func<int, string> generateUrl)
        {
            var model = new PaginationModel();

            var pageCount = (int)Math.Ceiling(_totalItemCount / (double)_pageSize);

            // Previous
            model.PaginationLinks.Add(_currentPage > 1 ? new PaginationLink { Active = true, DisplayText = "&#171;", PageIndex = _currentPage - 1, Url = generateUrl(_currentPage - 1) } : new PaginationLink { Active = false, DisplayText = "&#171;" });

            var start = 1;
            var end = pageCount;
            var nrOfPagesToDisplay = this._pagerOptions.MaxNrOfPages;

            if (pageCount > nrOfPagesToDisplay)
            {
                var middle = (int)Math.Ceiling(nrOfPagesToDisplay / 2d) - 1;
                var below = _currentPage - middle;
                var above = _currentPage + middle;

                if (below < 2)
                {
                    above = nrOfPagesToDisplay;
                    below = 1;
                }
                else if (above > (pageCount - 2))
                {
                    above = pageCount;
                    below = pageCount - nrOfPagesToDisplay;
                }

                start = below;
                end = above;
            }

            if (start > 1)
            {
                model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = 1, DisplayText = "1", Url = generateUrl(1) });
                //if (start > 3)
                //{
                //    model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = 2, DisplayText = "2", Url = generateUrl(2) });
                //}
                if (start > 2)
                {
                    model.PaginationLinks.Add(new PaginationLink { Active = true, DisplayText = ".." });
                }
            }

            for (var i = start; i <= end; i++)
            {
                if (i == _currentPage || (_currentPage <= 0 && i == 0))
                {
                    model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = i, IsCurrent = true, DisplayText = i.ToString() });
                }
                else
                {
                    model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = i, DisplayText = i.ToString(), Url = generateUrl(i) });
                }
            }

            if (end < pageCount)
            {
                if (end < pageCount - 1)
                {
                    model.PaginationLinks.Add(new PaginationLink { Active = true, DisplayText = ".." });
                }
                //if (pageCount - 2 > end)
                //{
                //    model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = pageCount - 1, DisplayText = (pageCount - 1).ToString(), Url = generateUrl(pageCount - 1) });
                //}

                model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = pageCount, DisplayText = pageCount.ToString(), Url = generateUrl(pageCount) });
            }

            // Next
            model.PaginationLinks.Add(_currentPage < pageCount ? new PaginationLink { Active = true, PageIndex = _currentPage + 1, DisplayText = "&#187;", Url = generateUrl(_currentPage + 1) } : new PaginationLink { Active = false, DisplayText = "&#187;" });

            // AjaxOptions
            if (_pagerOptions.AjaxOptions != null)
            {
                model.AjaxOptions = _pagerOptions.AjaxOptions;
            }

            model.Options = _pagerOptions;
            return model;
        }

        public string ToHtmlString()
        {
            var model = BuildPaginationModel(GeneratePageUrl);

            if (!string.IsNullOrEmpty(this._pagerOptions.DisplayTemplate))
            {
                var templatePath = string.Format("DisplayTemplates/{0}", this._pagerOptions.DisplayTemplate);
                return _htmlHelper.Partial(templatePath, model).ToHtmlString();
            }
            else
            {
                var sb = new StringBuilder();

                foreach (var paginationLink in model.PaginationLinks)
                {
                    if (paginationLink.Active)
                    {
                        if (paginationLink.IsCurrent)
                        {
                            sb.AppendFormat("<span class=\"current\">{0}</span>", paginationLink.DisplayText);
                        }
                        else if (!paginationLink.PageIndex.HasValue)
                        {
                            sb.AppendFormat(paginationLink.DisplayText);
                        }
                        else
                        {
                            var linkBuilder = new StringBuilder("<a");

                            if (_pagerOptions.AjaxOptions != null)
                            {
                                foreach (var ajaxOption in _pagerOptions.AjaxOptions.ToUnobtrusiveHtmlAttributes())
                                {
                                    linkBuilder.AppendFormat(" {0}=\"{1}\"", ajaxOption.Key, ajaxOption.Value);
                                }
                            }

                            linkBuilder.AppendFormat(" href=\"{0}\">{1}</a>", paginationLink.Url, paginationLink.DisplayText);
                            sb.Append(linkBuilder.ToString());
                        }
                    }
                    else
                    {
                        sb.AppendFormat("<span class=\"disabled\">{0}</span>", paginationLink.DisplayText);
                    }
                }

                return sb.ToString();
            }
        }

        private string GeneratePageUrl(int pageNumber)
        {
            var viewContext = this._htmlHelper.ViewContext;
            var routeDataValues = viewContext.RequestContext.RouteData.Values;
          
            RouteValueDictionary pageLinkValueDictionary;

            // Avoid canonical errors when pageNumber is equal to 1.
            if (pageNumber == 1 && !this._pagerOptions.AlwaysAddFirstPageNumber)
            {
                pageLinkValueDictionary = new RouteValueDictionary(this._pagerOptions.RouteValues);
                if (routeDataValues.ContainsKey(this._pagerOptions.PageRouteValueKey))
                {
                    routeDataValues.Remove(this._pagerOptions.PageRouteValueKey);
                }
            }
            else
            {
                pageLinkValueDictionary = new RouteValueDictionary(this._pagerOptions.RouteValues) { { this._pagerOptions.PageRouteValueKey, pageNumber } };
            }

            // To be sure we get the right route, ensure the controller and action are specified.
            if (!pageLinkValueDictionary.ContainsKey("controller") && routeDataValues.ContainsKey("controller"))
            {
                pageLinkValueDictionary.Add("controller", routeDataValues["controller"]);
            }

            if (!pageLinkValueDictionary.ContainsKey("action") && routeDataValues.ContainsKey("action"))
            {
                pageLinkValueDictionary.Add("action", routeDataValues["action"]);
            }

            // 'Render' virtual path.
            //var virtualPathForArea = RouteTable.Routes.GetVirtualPathForArea(viewContext.RequestContext, pageLinkValueDictionary);
            var virtualPathForArea = RouteTable.Routes.GetVirtualPathForArea(this._htmlHelper.ViewContext.RequestContext, pageLinkValueDictionary);

            var url = virtualPathForArea == null ? null : virtualPathForArea.VirtualPath;
            if (url != null)
            {
                var paramsList = HttpContext.Current.Request.QueryString;
                var isExistedParam = url.IndexOf('?') != -1;
                foreach (string key in paramsList)
                {
                    if (key != PagerOptions.DefaultDefaults.DEFAULTPAGEROUTEVALUEKEY)
                    {
                        if (isExistedParam)
                        {
                            url += "&amp;" + key + "=" + paramsList[key];
                        }
                        else
                        {
                            url += "?" + key + "=" + paramsList[key];
                        }
                    }
                }
            }

            return url;
        }
    }
}