// ---------------------------------------------------------------------------------
// <copyright file="INo.cs" company="Nexcel Solutions Vietnam">
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
    /// TODO: Update summary.
    /// </summary>
    public interface INo
    {
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        No_Template Template { get; set; }

        /// <summary>
        /// Renders the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>None template.</returns>
        No_Template Render(int index);
    }
}
