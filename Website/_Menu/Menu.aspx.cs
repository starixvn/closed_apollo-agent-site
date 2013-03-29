/*
 * Created 20100126@Lee - Render left menu page
 * Revision:
 * 20111227@Andy - Check for more 3 permissions
 *               - Related:
 *                  + Sub-menu of Member information index
 *                  + Sub-account cannot access, Customer List, Credit balance, PT, CSPT, BGPT, Member information
 *                  + Forecast permission
 *                  + Member information
 *                  + Moved PageAccess to global variable
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._Menu
{
    using System;
    using System.Web;
    using System.Web.UI;
    using Fanex.Age.UI;
    using Fanex.Agent.UI._Menu;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;

    public partial class Menu_Page : Fanex.Age.UI.TplPage
    {
        // Role descriptions
        public const string HR = "hr"; // Horse racing
        public const string HR_FO = "hr-fo"; // Horse racing fix odds
        public const string FI = "fi"; // Finacial
        public const string CS = "cs"; // Casino
        public const string NG = "numbergame"; // Number game
        public const string LiveCasino = "LiveCasino"; // Live Casino
        public const string SUBACC = "subacc"; // Sub-account
        public const string CR = "consolidatedreport"; //

        // Sub-menu index
        public const int TOTALBETS = 0;
        public const int FORECAST = 1;
        public const int REPORTS = 2;
        public const int CHARTS = 3;
        public const int MEMBERINFO = 4;

        // Sub-menu of Member information index
        public const int NEWMEMBER = 0;
        public const int SUBACCOUNT = 1;
        public const int MEMBERLIST = 2;
        public const int CREDITBALANCE = 3;
        public const int PT = 4;
        public const int CSPT = 5;
        public const int BGPT = 6;
        public const int MEMBERCOMM = 7;

        //Moved PageAccess to global variable
        PageAccess pageAccess = null;

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            pageAccess = CurrentUserSession.SubPageAccess;

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            return null;
        }

        public override void RenderContent(HtmlTextWriter writer)
        {
            Menu_Template tpl = new Menu_Template();

            // First logged-in
            if (!CurrentUserSession.IsLogged)
            {
                tpl.MenuTab = null;
                //tpl.MenuTab1 = null;
                //tpl.SubMenu = null;
            }

            RenderMenu(ref tpl);

            writer.Write(tpl);
        }

        protected void RenderMenu(ref Menu_Template tpl)
        {
            int roleId = CurrentUserSession.RoleId;
            SiteMapNode rootNode = SiteMap.RootNode;
            SiteMapNode contextNode = null;

            contextNode = rootNode.ChildNodes[0];
            RenderMenu(contextNode, ref tpl);
        }

        /// <summary>
        /// Render menu for a role.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="tpl"></param>
        private void RenderMenu(SiteMapNode node, ref Menu_Template tpl)
        {
            //if (isMobileBrowser())
            //    tpl.IphoneAd = null;
            //else
            //    tpl.IphoneAd_Gif = null;

            if (!node.HasChildNodes) return;
            if (tpl.SubMenu == null) return;

            Menu_SubMenu_Block subBlock = new Menu_SubMenu_Block();

            bool isSubAcc = CurrentUserSession.IsSubAcc;

            int subMenuCount = node.ChildNodes.Count;
            tpl.SubMenu.Clear();
            for (int i = 0; i < subMenuCount; i++) // Sub-menu loop
            {
                SiteMapNode subMenu = node.ChildNodes[i];
                string expand = subMenu.GetAttribute("expand");

                if (!subMenu.HasChildNodes)
                    continue;

                RenderSubMenu(subMenu, ref subBlock, i, (expand == "true"));
                tpl.SubMenu.Append(subBlock);
            }
        }

        /// <summary>
        /// Render sub-menu.
        /// </summary>
        /// <param name="subItem"></param>
        /// <param name="subBlock"></param>
        /// <param name="index"></param>
        private void RenderSubMenu(SiteMapNode subMenu, ref Menu_SubMenu_Block subBlock, int index, bool isExpand)
        {
            int itemCount = subMenu.ChildNodes.Count;

            subBlock.sub_display = isExpand ? "Bleft_ParentAc" : "Bleft_Parent";
            subBlock.item_display = isExpand ? "block" : "none";

            subBlock.sub_id = index.ToString();
            subBlock.sub_title = GetGlobalResourceObject("Lang", subMenu.Title) as string;

            bool isSubAcc = CurrentUserSession.IsSubAcc;

            Menu_SubMenu_MenuItem_Block itemBlock = new Menu_SubMenu_MenuItem_Block();
            subBlock.MenuItem.Clear();

            Sunplus.Agent.Model.BL.Authorization.AuthorizationBO authorize = new Sunplus.Agent.Model.BL.Authorization.AuthorizationBO(CurrentUserSession.NetSessionId);

            int k = 0;
            for (int j = 0; j < itemCount; j++) // Menu item loop
            {
                SiteMapNode item = subMenu.ChildNodes[j];

                // Sub-account can not access SubAcc function
                if (isSubAcc && item.Description == SUBACC) continue;

                //if (isSubAcc && itemCount == 8 && (j == CREDITBALANCE || j == MEMBERLIST) && !pageAccess.ViewMemberInfo && !pageAccess.UpdateAccount) continue;

                //// Sub-account cannot access, Customer List, Credit balance, PT, CSPT, BGPT, Member information
                //if (isSubAcc && itemCount == 8 && (j == PT || j == CSPT || j == BGPT || j == MEMBERCOMM) && !pageAccess.ViewMemberInfo) continue;

                // Authorize AGE permission
                // if (!authorize.AuthorizeUrl(0, 0, false, item.GetAttribute("href"))) continue;

                RenderMenuItem(item, ref itemBlock, j, index);
                subBlock.MenuItem.Append(itemBlock);

                k++;
            }

            if (k == 0) subBlock.Clear();
        }

        /// <summary>
        /// Render menu item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="block"></param>
        /// <param name="index"></param>
        private void RenderMenuItem(SiteMapNode menuItem, ref Menu_SubMenu_MenuItem_Block itemBlock, int index, int parentIndex)
        {
            string title = menuItem.Title;
            int bettypeId = 0;

            if (title.Contains("bettype") && Int32.TryParse(title.Substring(7), out bettypeId))//get from remoting ref
            {
                itemBlock.item_title = CacheRef.GetBettype(bettypeId);
            }
            else//get from Resources
            {
                itemBlock.item_title = GetGlobalResourceObject("Lang", menuItem.Title).ToString().Trim();
            }
            itemBlock.item_url = "../" + menuItem.GetAttribute("href");
            itemBlock.order = (parentIndex + 1).ToString() + (index + 1).ToString();
            itemBlock.menuId = menuItem.Description;
        }
    }
}