// --------------------------------------------------------------------------
// <copyright file="RepositoryBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.26">Created</change>
// </history>

namespace Apollo.Age.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Repository base.
    /// </summary>
    /// <typeparam name="T">Input a parameter optional.</typeparam>
    public class RepositoryBase<T> : ReadonlyRepository<T>
    {       
    }
}
