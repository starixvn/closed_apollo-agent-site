// ---------------------------------------------------------------------------------
// <copyright file="ViewDetail.aspx.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Kaden.Trinh" date="2013.05.08">View detail Agent Comm</change>
// </history>
// ---------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fanex.Age.UI;

public abstract class BetListBase : TplPage
{
    #region Member Variables

    private int _PageIndex = 1;
    private bool _IsPaging = true;

    protected int TotalRows { get; set; }

    protected bool IsPaging
    {
        get { return _IsPaging; }
        set { _IsPaging = value; }
    }

    public int PageIndex
    {
        get { return _PageIndex; }
        set { _PageIndex = value; }
    }

    protected int PageSize = 50;
    protected List<object> PAGE_SIZES = new List<object>(new object[] {10, 20, 50, 100, 200, 500 });

    #endregion Member Variables

    /// <summary>
    /// Initializes a new instance of the <see cref="BetListBase" /> class.
    /// </summary>
    public BetListBase()
    {
    }
}