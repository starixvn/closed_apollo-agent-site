// --------------------------------------------------------------------------
// <copyright file="TotalBetsRazorViewEngine.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.27">Created</change>
// </history>


namespace Apollo.Age.TotalBets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// Relocate view searching, it should be added to Application_Start()  
    /// </summary>
    public class TotalBetsRazorViewEngine : RazorViewEngine
    {
        public TotalBetsRazorViewEngine()
        {
            AreaViewLocationFormats = new string[]
            {
                //~/Areas/[AreasName]/Views/[ControllerName]
                "~/Areas/{2}/Views/{1}.cshtml"
            };
        }
    }
}
