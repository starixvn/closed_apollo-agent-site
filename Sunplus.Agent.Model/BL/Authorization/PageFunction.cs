/*
 * Created 20100522@Lee (Convert from old code) - Implement PageFunction and PageFunctionCollection from App_Data\PageAccess.xml.
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.Model.BL.Authorization
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public class RolePermissionPair
    {
        /// <summary>
        /// [2], [3], [4]...Agent, Master, Super.
        /// </summary>
        private byte role;
        private string permission;

        public byte Role
        {
            get { return this.role; }
            set { this.role = value; }
        }

        /// <summary>
        /// [A], [B], [C], [D], [E]...
        /// </summary>
        public string Permission
        {
            get { return this.permission; }
            set { this.permission = value; }
        }
    }

    public class PageFunction
    {
        /// <summary>
        /// Id of the function.
        /// </summary>
        public int Id;

        /// <summary>
        /// Related url.
        /// </summary>
        public string Url;

        /// <summary>
        /// Name of the function.
        /// </summary>
        public string Name;

        /// <summary>
        /// Name of the sub-function.
        /// </summary>
        public string SubName;

        /// <summary>
        /// Array of 2-[A], 3-[D]...
        /// </summary>
        public RolePermissionPair[] RoleRules;

        public PageFunction()
        {
        }
    }

    public class PageFunctionCollection
    {
        protected static Dictionary<string, PageFunction> collection;

        /// <summary>
        /// Get collection from a file
        /// </summary>
        static PageFunctionCollection()
        {
            collection = new Dictionary<string, PageFunction>(100);

            // Load description from xml file
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "App_Data\\PagesAccess.xml");

            XmlNodeList pageList = xmlDoc.GetElementsByTagName("add");
            string[] rolePermissions;
            foreach (XmlNode node in pageList)
            {
                try
                {
                    PageFunction function = new PageFunction();
                    function.Url = node.Attributes["url"].Value.ToLower();
                    function.Id = int.Parse(node.Attributes["id"].Value);
                    function.Name = node.Attributes["fun"].Value;
                    function.SubName = node.Attributes["subfun"].Value;
                    if (!string.IsNullOrEmpty(node.Attributes["role"].Value))
                    {
                        // Format: 2-[A],3-[D]...
                        rolePermissions = node.Attributes["role"].Value.Split(',');
                        RolePermissionPair[] rolePairs = new RolePermissionPair[rolePermissions.Length];

                        for (int i = 0; i < rolePermissions.Length; ++i)
                        {
                            rolePairs[i] = new RolePermissionPair();
                            rolePairs[i].Role = byte.Parse(rolePermissions[i].Substring(0, 1));
                            rolePairs[i].Permission = rolePermissions[i].Substring(2);
                        }

                        function.RoleRules = rolePairs;
                    }
                    else
                    {
                        function.RoleRules = null;
                    }

                    // Add to collection
                    collection.Add(function.Url, function);
                }
                catch (Exception e)
                {
                    Sunplus.Agent.Utilities.Logs.Write(e + "\r\n" + node.Attributes["url"].Value);
                }
            }
        }

        public static PageFunction GetPageFunctionByUrl(string url)
        {
            url = url.ToLower();
            if (!collection.ContainsKey(url))
            {
                return null;
            }

            return collection[url];
        }
    }
}
