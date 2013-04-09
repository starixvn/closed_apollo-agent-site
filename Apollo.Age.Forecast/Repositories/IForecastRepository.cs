// --------------------------------------------------------------------------
// <copyright file="IForecastRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.5">Created</change>
// </history>

namespace Apollo.Age.Forecast.Repositories
{
    using System.Diagnostics.CodeAnalysis;
    using Apollo.Age.Forecast.Models;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IForecastRepository
    {
        ScoreMapResult GetScoreMapResult(ICriteria criteria);
    }
}