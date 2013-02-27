<%@ WebHandler Language="C#" Class="Fanex.Age.UI.LiveCasino.AgentPTComm_Handler" %>
/*
    Created by: Andy
    Description: Handler for processing master edits agent pt setting
*/
using System;
using System.Web;
using System.Collections.Generic;

using Fanex.Age.UI;
using Fanex.LiveCasinoSetting.Request;
using Fanex.LiveCasinoSetting.Model;

using System.ComponentModel;

namespace Fanex.Age.UI.LiveCasino
{
    public class AgentPTComm_Handler : LiveCasinoHandler, System.Web.SessionState.IReadOnlySessionState
    {
        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal)
            {
                return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, "1|" + BeginDivErr + Resources.Lang.PositionTakingCommission + ": " + Resources.Lang.lblNotAuthorized + EndDiv);
            }

            return base.ValidateRequest(p);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            var pt = PtSetting.GetAgentPTSetting(RequestCustId, CurrentUserSession.CustId);

            if ((Custids.Count == 0 && !IsSyncLiveCasino) || Custids.Count > 0)
            {
                pt.PropertiesChangedList.Add("MaxPT");
                pt.PropertiesChangedList.Add("MinPT");
                pt.PropertiesChangedList.Add("Comm");

                ((PTSetting)pt.Upline).PropertiesChangedList.Add("PresetPT");
                ((PTSetting)pt.Upline).PropertiesChangedList.Add("AutoPresetPT");
                ((PTSetting)pt.Upline).PropertiesChangedList.Add("IsAutoPresetPT");
            }
            else
            {
                //Handles Agent values changed
                ((INotifyPropertyChanged)pt).PropertyChanged
                                 += (o, arg) =>
                                 {
                                     if (!pt.PropertiesChangedList.Contains(arg.PropertyName))
                                     {
                                         pt.PropertiesChangedList.Add(arg.PropertyName);
                                     }
                                 };

                //Handles Master values changed
                ((INotifyPropertyChanged)pt.Upline).PropertyChanged
                                 += (o, arg) =>
                                 {
                                     if (!((PTSetting)pt.Upline).PropertiesChangedList.Contains(arg.PropertyName))
                                     {
                                         ((PTSetting)pt.Upline).PropertiesChangedList.Add(arg.PropertyName);
                                     }
                                 };
            }

            //Master values
            ((PTSetting)pt.Upline).PresetPT = PresetPT;
            ((PTSetting)pt.Upline).AutoPresetPT = AutoPresetPT;
            ((PTSetting)pt.Upline).IsAutoPresetPT = IsAutoPresetPT;
            ((PTSetting)pt.Upline).IsUpdateAllMember = IsUpdateAllMember;
            ((PTSetting)pt.Upline).NewMemberPT = NewMemberPT;

            //Agent values
            pt.Comm = Comm;
            pt.MinPT = MinPT;
            pt.MaxPT = MaxPT;

            var results = new Dictionary<int, Fanex.LiveCasinoSetting.Model.Result>();

            if (Custids.Count > 0)//Multiple updates
            {
                results = PtSetting.UpdateAgentPTSetting
                (
                    new RequestContext()
                    {
                        CustId = RequestCustId,
                        SignUpIp = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                        Language = CurrentUserSession.LangKey,
                        SubAccId = CurrentUserSession.SubAccId,
                        UplineId = CurrentUserSession.CustId,
                        UserId = CurrentUserSession.UserId
                    },
                    pt,
                    Custids
                );
            }
            else//Single update
            {
                var result = PtSetting.UpdateAgentPTSetting
                (
                    new RequestContext()
                    {
                        CustId = RequestCustId,
                        SignUpIp = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                        Language = CurrentUserSession.LangKey,
                        SubAccId = CurrentUserSession.SubAccId,
                        UplineId = CurrentUserSession.CustId,
                        UserId = CurrentUserSession.UserId
                    },
                    pt
                );

                results.Add(RequestCustId, result);
            }

            SynManually(false, results);

            ProcessMessage
                (
                    results,
                    pt.IsCommChanged,
                    pt.Upline.IsPresetPTChanged |
                    pt.Upline.IsIsAutoPresetPTChanged |
                    pt.Upline.IsAutoPresetPTChanged |
                    pt.IsMaxPTChanged,
                    pt.IsMinPTChanged
                );

            return null;
        }
    }
}