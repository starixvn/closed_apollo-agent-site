// -----------------------------------------------------------------------
// <copyright file="ServiceBase.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

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
    }
}
