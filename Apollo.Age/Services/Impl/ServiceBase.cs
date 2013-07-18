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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ServiceBase
    {
        /// <summary>
        /// Handle Exception.
        /// </summary>
        /// <param name="ex">Exception.</param>
        protected void HandleException(Exception ex)
        {
            Fanex.Logging.LogService.Error(ex.Message, ex);
        }

        #region Commission Group Reverting

        /// <summary>
        /// Revert Commission Group Name.
        /// </summary>
        /// <param name="commGroupName"> Input commission group name normally (a,b,c,d).</param>
        /// <returns>Return commission group name that need to revert(d,c,b,a).</returns>
        protected string RevertCommGroupName(string commGroupName)
        {
            switch (commGroupName.ToLower())
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

                case "d":
                    {
                        return "A";
                    }

                default:
                    {
                        return "All";
                    }
            }
        }

        #endregion Commission Group Reverting
    }
}