// --------------------------------------------------------------------------
// <copyright file="ServiceBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.7.7">Created</change>
// </history>
namespace Apollo.Age.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ServiceBase
    {
        protected void HandleException(Exception ex)
        {
            Fanex.Logging.LogService.Error(ex.Message, ex);
        }

        #region Group Commission Reverting

        /// <summary>
        /// Revert Group Commission Name.
        /// </summary>
        /// <param name="groupCommName"> Input group commission name normally (a,b,c,d).</param>
        /// <returns>Return group commission name that need to revert(d,c,b,a).</returns>
        protected string RevertGroupCommName(string groupCommName)
        {
            switch (groupCommName.ToLower())
            {
                case "a":
                    {
                        return "D";
                    }
                case "b":
                    {
                        return "C";
                    }
                case "c":
                    {
                        return "B";
                    }
                default:
                    {
                        return "A";
                    }
            }
        }

        #endregion
    }
}
