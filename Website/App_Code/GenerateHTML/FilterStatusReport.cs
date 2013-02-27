using System;
using System.Collections.Generic;
using System.Web;
using DNAide.Web;
using DNAide.Core;
using Sunplus.Agent.Model.Entities.Permission;
/// <summary>
/// Summary description for FilterStatusReport
/// </summary>
public class FilterStatusReport
{
    private static bool _ShowSB, _ShowCasino, _ShowP2P, _ShowRB, _ShowFI, _ShowAll, _IsFirst = true, _IsCross, _ShowNG, _ShowBI;
    private static byte[] _ArrayShowAll = new byte[7] { 0, 0, 0, 0, 0, 0, 0 };
    private const byte SB = 0, CS = 1, P2P = 2, HR = 3, FI = 4, NG = 5, BI = 6;

    public static bool ShowSB
    {
        get
        {
            return _ShowSB;
        }
        set
        {
            _ShowSB = value;
        }
    }
    public static bool ShowCasino
    {
        get
        {
            return _ShowCasino;
        }
        set
        {
            _ShowCasino = value;
        }
    }

    public static bool ShowP2P
    {
        get
        {
            return _ShowP2P;
        }
        set
        {
            _ShowP2P = value;
        }
    }
    public static bool ShowRB
    {
        get
        {
            return _ShowRB;
        }
        set
        {
            _ShowRB = value;
        }
    }
    public static bool ShowFI
    {
        get
        {
            return _ShowFI;
        }
        set
        {
            _ShowFI = value;
        }
    }
    public static bool ShowNG
    {
        get
        {
            return _ShowNG;
        }
        set
        {
            _ShowNG = value;
        }
    }
    public static bool ShowBI
    {
        get
        {
            return _ShowBI;
        }
        set
        {
            _ShowBI = value;
        }
    }
    public static bool ShowAll
    {
        get
        {
            return _ShowAll;
        }
        set
        {
            _ShowAll = value;
        }
    }
    public static bool IsFirst
    {
        get
        {
            return _IsFirst;
        }
        set
        {
            _IsFirst = value;
        }
    }
    public static bool IsCross
    {
        get
        {
            return _IsCross;
        }
        set
        {
            _IsCross = value;
        }
    }
    public static byte[] ArrayShowAll
    {
        get
        {
            return _ArrayShowAll;
        }
        set
        {
            _ArrayShowAll = value;
        }
    }
    public static bool LoadFilteringOptions(ref string headhtml, HttpContext context)
    {
        int count = 0;
        _ShowAll = _ShowSB = _ShowCasino = _ShowP2P = _ShowRB = _ShowFI = _ShowNG=_ShowBI=false;

        if (Sunplus.Agent.Model.Entities.UserSession.Current.IsCasinoSync)
        {

            count++;
            _ArrayShowAll[CS]++;
            if (!string.IsNullOrEmpty(context.Request["chk_showcasino"]))
            {
                _IsFirst = (_IsFirst && false);
                if (HttpContext.Current.Request["chk_showcasino"].Equals("on"))
                {
                    _ShowCasino = true;
                    _ArrayShowAll[CS]++;
                }
            }
            TemplateHandler.Assign(ref headhtml, "Casino", Resources.Lang.casino);
        }
        else
        {
            TemplateHandler.RemoveBlock(ref headhtml, "casino");
        }

        if (UserPermission.IsShowBingo)
        {

            count++;
            _ArrayShowAll[BI]++;
            if (!string.IsNullOrEmpty(context.Request["chk_showbi"]))
            {
                _IsFirst = (_IsFirst && false);
                if (HttpContext.Current.Request["chk_showbi"].Equals("on"))
                {
                    _ShowBI = true;
                    _ArrayShowAll[BI]++;
                }
            }
            TemplateHandler.Assign(ref headhtml, "BI", Resources.Bingo.bingo);
        }
        else
        {
            TemplateHandler.RemoveBlock(ref headhtml, "bi");
        }

        #region NumberGame
        if (UserPermission.IsShowNumberGame)
        {

            count++;
            _ArrayShowAll[NG]++;
            if (!string.IsNullOrEmpty(context.Request["chk_showng"]))
            {
                _IsFirst = (_IsFirst && false);
                if (HttpContext.Current.Request["chk_showng"].Equals("on"))
                {
                    _ShowNG = true;
                    _ArrayShowAll[NG]++;
                }
            }
            TemplateHandler.Assign(ref headhtml, "NG", Resources.Lang.numbergame);
        }
        else
        {
            TemplateHandler.RemoveBlock(ref headhtml, "ng");
        }
        #endregion

        if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing)
        {
            count++;
            _ArrayShowAll[HR]++;
            if (!string.IsNullOrEmpty(context.Request["chk_showrb"]))
            {
                _IsFirst = (_IsFirst && false);
                if (HttpContext.Current.Request["chk_showrb"].Equals("on"))
                {
                    _ShowRB = true;
                    _ArrayShowAll[HR]++;
                }
            }
            TemplateHandler.Assign(ref headhtml, "HR", Resources.Lang.Horse);
        }
        else
        {
            TemplateHandler.RemoveBlock(ref headhtml, "hr");
        }

        if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowFinance)
        {
            count++;
            _ArrayShowAll[FI]++;
            if (!string.IsNullOrEmpty(context.Request["chk_showfi"]))
            {
                _IsFirst = (_IsFirst && false);
                if (HttpContext.Current.Request["chk_showfi"].Equals("on"))
                {
                    _ShowFI = true;
                    _ArrayShowAll[FI]++;
                }
            }
            TemplateHandler.Assign(ref headhtml, "FI", Resources.Lang.Financial);
        }
        else
        {
            TemplateHandler.RemoveBlock(ref headhtml, "fi");
        }


        if (count > 0)
        {
            _ArrayShowAll[SB]++;
            if (!string.IsNullOrEmpty(context.Request["chk_showsb"]))
            {
                _IsFirst = (_IsFirst && false);
                if (HttpContext.Current.Request["chk_showsb"].Equals("on"))
                {
                    _ShowSB = true;
                    _ArrayShowAll[SB]++;
                }
            }

            TemplateHandler.Assign(ref headhtml, "All", Resources.Lang.all);
            TemplateHandler.Assign(ref headhtml, "SportBook", Resources.Lang.SB);

        }
        else if (count == 0)
        {
            _ShowSB = true;
            TemplateHandler.RemoveBlock(ref headhtml, "p2pCasino");
        }

        if (_IsCross)
        {
            ProcessOptionsOld();
        }

        int idx;
        for (idx = 0; idx < _ArrayShowAll.Length; idx++)
        {
            if (_ArrayShowAll[idx] == 1)
            {
                break;
            }
        }
        _ShowAll = (idx == _ArrayShowAll.Length ? true : false);

        if (_IsFirst)
        {
            InitOptionsStatusForTheFirst();
        }

        return (count > 0);
    }

    public static void InitOptionsStatusForTheFirst()
    {
        for (int idx = 0; idx < _ArrayShowAll.Length; idx++)
        {
            if (_ArrayShowAll[idx] == 1)
            {
                switch (idx)
                {
                    case SB:
                        _ShowSB = true;
                        break;
                    case CS:
                        _ShowCasino = true;
                        break;
                    case P2P:
                        _ShowP2P = false;
                        break;
                    case HR:
                        _ShowRB = true;
                        break;
                    case FI:
                        _ShowFI = true;
                        break;
                    case NG:
                        _ShowNG = true;
                        break;
                    case BI:
                        _ShowBI = true;
                        break;
                }
            }
        }
    }

    public static void SetStateForFilteringOptions(ref string headHtml, bool isViewDownline)
    {
        if (!isViewDownline)
        {
            if (_IsFirst)
            {
                TemplateHandler.Assign(ref headHtml, "checked_All", "checked='checked'");
                TemplateHandler.Assign(ref headHtml, "checked_SB", "checked='checked'");
                TemplateHandler.Assign(ref headHtml, "checked_Casino", "checked='checked'");
                TemplateHandler.Assign(ref headHtml, "checked_P2P", "checked='checked'");
                TemplateHandler.Assign(ref headHtml, "checked_HR", "checked='checked'");
                TemplateHandler.Assign(ref headHtml, "checked_FI", "checked='checked'");
                TemplateHandler.Assign(ref headHtml, "checked_BI", "checked='checked'");
                TemplateHandler.Assign(ref headHtml, "checked_NG", "checked='checked'");
            }
            else
            {
                TemplateHandler.Assign(ref headHtml, "checked_All", (_ShowAll ? "checked='checked'" : ""));
                TemplateHandler.Assign(ref headHtml, "checked_SB", (_ShowSB ? "checked='checked'" : ""));
                TemplateHandler.Assign(ref headHtml, "checked_Casino", (_ShowCasino ? "checked='checked'" : ""));
                TemplateHandler.Assign(ref headHtml, "checked_P2P", (_ShowP2P ? "checked='checked'" : ""));
                TemplateHandler.Assign(ref headHtml, "checked_HR", (_ShowRB ? "checked='checked'" : ""));
                TemplateHandler.Assign(ref headHtml, "checked_FI", (_ShowFI ? "checked='checked'" : ""));
                TemplateHandler.Assign(ref headHtml, "checked_NG", (_ShowNG ? "checked='checked'" : ""));
                TemplateHandler.Assign(ref headHtml, "checked_BI", (_ShowBI ? "checked='checked'" : ""));
            }
        }
        else
        {
            TemplateHandler.Assign(ref headHtml, "checked_All", (_ShowAll ? "1" : "0"));
            TemplateHandler.Assign(ref headHtml, "checked_SB", (_ShowSB ? "1" : "0"));
            TemplateHandler.Assign(ref headHtml, "checked_Casino", (_ShowCasino ? "1" : "0"));
            TemplateHandler.Assign(ref headHtml, "checked_P2P", (_ShowP2P ? "1" : "0"));
            TemplateHandler.Assign(ref headHtml, "checked_HR", (_ShowRB ? "1" : "0"));
            TemplateHandler.Assign(ref headHtml, "checked_FI", (_ShowFI ? "1" : "0"));
            TemplateHandler.Assign(ref headHtml, "checked_BI", (_ShowBI ? "1" : "0"));
            TemplateHandler.Assign(ref headHtml, "checked_NG", (_ShowNG ? "1" : "0"));
        }
    }
    public static void SetStateForFilteringOptions(ref string headHtml)
    {
        if (_IsFirst)
        {
            TemplateHandler.Assign(ref headHtml, "checked_All", "checked='checked'");
            TemplateHandler.Assign(ref headHtml, "checked_SB", "checked='checked'");
            TemplateHandler.Assign(ref headHtml, "checked_Casino", "checked='checked'");
            TemplateHandler.Assign(ref headHtml, "checked_P2P", "checked='checked'");
            TemplateHandler.Assign(ref headHtml, "checked_HR", "checked='checked'");
            TemplateHandler.Assign(ref headHtml, "checked_FI", "checked='checked'");
            TemplateHandler.Assign(ref headHtml, "checked_NG", "checked='checked'");
            TemplateHandler.Assign(ref headHtml, "checked_BI", "checked='checked'");
        }
        else
        {
            TemplateHandler.Assign(ref headHtml, "checked_All", (_ShowAll) ? "checked='checked'" : "");
            TemplateHandler.Assign(ref headHtml, "checked_SB", (_ShowSB) ? "checked='checked'" : "");
            TemplateHandler.Assign(ref headHtml, "checked_Casino", (_ShowCasino) ? "checked='checked'" : "");
            TemplateHandler.Assign(ref headHtml, "checked_P2P", (_ShowP2P) ? "checked='checked'" : "");
            TemplateHandler.Assign(ref headHtml, "checked_HR", (_ShowRB) ? "checked='checked'" : "");
            TemplateHandler.Assign(ref headHtml, "checked_FI", (_ShowFI) ? "checked='checked'" : "");
            TemplateHandler.Assign(ref headHtml, "checked_NG", (_ShowNG) ? "checked='checked'" : "");
            TemplateHandler.Assign(ref headHtml, "checked_BI", (_ShowBI) ? "checked='checked'" : "");
        }
    }

    public static void SetCross(DateTime fromDate)
    {
        DateTime dtTmp = CastAide.AsDateTime(Sunplus.Agent.Model.BL.Definitions.AppSettings.SB_HR_FI_Date);
        _IsCross = !(dtTmp <= fromDate);
    }

    public static void ProcessOptionsOld()
    {
        // Set status of all old options (SB, HR, FI)
        bool isExitFor = false;
        for (int idx = 0; idx < _ArrayShowAll.Length; idx++)
        {
            if (_ArrayShowAll[idx] == 2)
            {
                switch (idx)
                {
                    case SB:
                    case HR:
                    case FI:
                        if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing)
                        {
                            _ShowRB = true;
                            _ArrayShowAll[HR] = 2;
                        }
                        _ShowSB = true;
                        _ArrayShowAll[SB] = 2;

                        isExitFor = true;

                        break;
                }
            }
            if (isExitFor)
            {
                break;
            }
        }
    }
}
