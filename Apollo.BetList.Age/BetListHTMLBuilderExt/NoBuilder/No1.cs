// ---------------------------------------------------------------------------------
// <copyright file="No1.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.18">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Age.BetListHTMLBuilderExt.NoBuilder
{
    using Apollo.BetList.Age.ExtTemplate;

    /// <summary>
    /// Number builder.
    /// </summary>
    public class No1 : INo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="No1" /> class.
        /// </summary>
        public No1()
        {
            this.Template = new No_Template();
        }
        
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public No_Template Template { get; set; }

        /// <summary>
        /// Renders the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>No template.</returns>
        public No_Template Render(int index)
        {
            Template.orderNo = index.ToString();
            return this.Template;
        }
    }
}
