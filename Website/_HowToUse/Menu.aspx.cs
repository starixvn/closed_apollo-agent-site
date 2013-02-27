using System.Web;
using System.Web.UI;
using Fanex.Age.UI;
using Sunplus.Agent.Model.BL.Definitions;
using Sunplus.Agent.Model.Entities;

/*
 * Created 20100610@Lee - Render left menu page
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._HowToUse
{
    public partial class Menu_Page : Fanex.Age.UI.TplPage
    {
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            return null;
        }

        public override void RenderContent(HtmlTextWriter writer)
        {
            Menu_Template tpl = new Menu_Template();

            if (CurrentUserSession.IsSuper || Sunplus.Agent.Model.Entities.Permission.UserPermission.IsNeutral)
                tpl.AgUI = null;

            if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsNeutral)
                tpl.AgWapUI = null;

            RenderMenu(ref tpl);

            writer.Write(tpl);
        }

        protected void RenderMenu(ref Menu_Template tpl)
        {
            int roleId = CurrentUserSession.RoleId;
            SiteMapNode rootNode = SiteMap.RootNode;
            SiteMapNode contextNode = null;

            if (roleId == (int)(UserTypes.Agent))
            {
                contextNode = rootNode.ChildNodes[3];
            }
            else if (roleId == (int)(UserTypes.Master))
            {
                contextNode = rootNode.ChildNodes[4];
            }
            else if (roleId == (int)(UserTypes.Super))
            {
                contextNode = rootNode.ChildNodes[5];
            }

            RenderMenu(contextNode, ref tpl);
        }

        protected void RenderMenu(SiteMapNode node, ref Menu_Template tpl)
        {
            if (!node.HasChildNodes) return;
            if (tpl.SubMenu == null) return;

            Menu_SubMenu_Block subBlock = new Menu_SubMenu_Block();

            int subMenuCount = node.ChildNodes.Count;
            tpl.SubMenu.Clear();

            for (int i = 0; i < subMenuCount; i++) // Sub-menu loop
            {
                SiteMapNode subMenu = node.ChildNodes[i];
                string expand = subMenu.GetAttribute("expand");

                if (!subMenu.HasChildNodes)
                    continue;

                if (RenderSubMenu(subMenu, ref subBlock, i, (expand == "true")))
                    tpl.SubMenu.Append(subBlock);
            }
        }

        private bool RenderSubMenu(SiteMapNode subMenu, ref Menu_SubMenu_Block subBlock, int index, bool isExpand)
        {
            int itemCount = subMenu.ChildNodes.Count;

            subBlock.sub_display = isExpand ? "Bleft_ParentAc" : "Bleft_Parent";
            subBlock.item_display = isExpand ? "block" : "none";

            subBlock.sub_id = index.ToString();
            subBlock.sub_title = GetGlobalResourceObject("HowToUse", subMenu.Title) as string;

            Menu_SubMenu_MenuItem_Block itemBlock = new Menu_SubMenu_MenuItem_Block();
            subBlock.MenuItem.Clear();

            bool isShowSiteMapNode = false;
            for (int j = 0; j < itemCount; j++) // Menu item loop
            {
                SiteMapNode item = subMenu.ChildNodes[j];

                if (item.Description == "WinLimitGuide")
                {
                }

                // Agent, Master(DisableAutoM) don't have AutoPT
                if (item.Description == "AutoPT")
                {
                    if (CurrentUserSession.IsAgent) continue;
                    if (CurrentUserSession.IsMaster && Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledAutoMaster) continue;
                }

                // Check Show View Log
                if (item.Description == "ViewLog")
                {
                    if (CurrentUserSession.IsSubAcc)
                    {
                        PageAccess pageAccess = CurrentUserSession.SubPageAccess;
                        if (!(pageAccess.CreateAccount || pageAccess.UpdateAccount))
                            continue;
                    }

                    if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.ShowViewLog) continue;
                }

                // Check Show Casino
                if (item.Description == "CasinoGuide")
                {
                    if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsNeutral) continue;
                }

                // Check Show NumberGame
                if (item.Description == "NumberGame")
                {
                    if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsNeutral || !Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowNumberGame || CurrentUserSession.IsNumberGameDisabled) continue;
                }

                RenderMenuItem(item, ref itemBlock, j, index);
                subBlock.MenuItem.Append(itemBlock);
                isShowSiteMapNode = true;
            }
            return isShowSiteMapNode;
        }

        /// <summary>
        /// Render menu item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="block"></param>
        /// <param name="index"></param>
        private void RenderMenuItem(SiteMapNode menuItem, ref Menu_SubMenu_MenuItem_Block itemBlock, int index, int parentIndex)
        {
            itemBlock.item_title = GetGlobalResourceObject("HowToUse", menuItem.Title) as string;
            itemBlock.order = (parentIndex + 1).ToString() + (index + 1).ToString();

            if (menuItem.Title == "MemberWinLimit")
            {
                itemBlock.item_url = "Resources/HTML/WinLimit/WinLimit_" + CurrentUserSession.Language + ".html";
            }
            else
            {
                itemBlock.item_url = "Content.aspx" +
                    "?fw=" + menuItem.GetAttribute("width") +
                    "&fh=" + menuItem.GetAttribute("height") +
                    "&folder=" + menuItem.Description;
            }
        }
    }
}