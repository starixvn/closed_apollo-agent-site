using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Age.DTO.Reports.TreeView
{
    public class TreeViewNode
    {
        #region Properties
        /// <summary>
        /// Gets or sets the node identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the node text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets whether node is expanded
        /// </summary>
        public bool Expanded { get; set; }

        /// <summary>
        /// Gets or sets whether node has children
        /// </summary>
        public bool HasChildren { get; set; }

        /// <summary>
        /// Gets or sets additional CSS classes for node
        /// </summary>
        public string Classes { get; set; }

        /// <summary>
        /// Gets or sets node childrens
        /// </summary>
        public TreeViewNode[] Children { get; set; }
        #endregion
    }
}
