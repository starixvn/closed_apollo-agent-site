<%@ WebHandler Language="C#" Class="Fanex.Age.UI.LiveCasino.MemberPTComm_Handler" %>
/*
    Created by: Andy
    Description: Handler for processing agent edits member pt setting
*/
using System;
using System.Web;
using System.Collections.Generic;

using Fanex.LiveCasinoSetting.Request;
using Fanex.LiveCasinoSetting.Model;

using System.ComponentModel;

namespace Fanex.Age.UI.LiveCasino
{
    public class MemberPTComm_Handler : LiveCasinoHandler, System.Web.SessionState.IReadOnlySessionState
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
            var pt = PtSetting.GetMemberPTSetting(RequestCustId, CurrentUserSession.CustId);

            if ((Custids.Count == 0 && !IsSyncLiveCasino) || Custids.Count > 0)
            {
                pt.PropertiesChangedList.Add("Comm");
                ((SuperPTSetting)pt.Upline).PropertiesChangedList.Add("PresetPT");
            }
            else
            {
                ((INotifyPropertyChanged)pt).PropertyChanged
                                 += (o, arg) =>
                                 {
                                     if (!pt.PropertiesChangedList.Contains(arg.PropertyName))
                                     {
                                         pt.PropertiesChangedList.Add(arg.PropertyName);
                                     }
                                 };

                ((INotifyPropertyChanged)pt.Upline).PropertyChanged
                         += (o, arg) =>
                         {
                             if (!((SuperPTSetting)pt.Upline).PropertiesChangedList.Contains(arg.PropertyName))
                             {
                                 ((SuperPTSetting)pt.Upline).PropertiesChangedList.Add(arg.PropertyName);
                             }
                         };
            }

            ((SuperPTSetting)pt.Upline).PresetPT = PresetPT;
            pt.Comm = Comm;

            var results = new Dictionary<int, Fanex.LiveCasinoSetting.Model.Result>();

            if (Custids.Count > 0)//Multiple updates
            {

                results = PtSetting.UpdateMemberPTSetting
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
                LiveCasinoSetting.Model.Result result = PtSetting.UpdateMemberPTSetting
                (
                    new RequestContext()
                    {
                        CustId = RequestCustId,
                        SignUpIp = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                        Language = CurrentUserSession.LangKey,
                        SubAccId = CurrentUserSession.SubAccId,
                        UplineId = CurrentUserSession.CustId,
                        UserId = CurrentUserSession.UserId,
                        IsSyncLiveCasino = IsSyncLiveCasino
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
                        pt.Upline.IsPresetPTChanged,
                        false
                    );

            return null;
        }
    }
}