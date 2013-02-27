<%@ WebHandler Language="C#" Class="Fanex.Age.UI.LiveCasino.MemberPTComm_Handler" %>
/*
    Created by: Andy
    Description: Handler for processing master edits member pt setting
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
            var pt = PtSetting.MasterGetMemberPTSetting(RequestCustId, CurrentUserSession.CustId);

            if (Custids.Count == 0)
            {
                ((INotifyPropertyChanged)pt.Upline.Upline).PropertyChanged
                         += (o, arg) =>
                         {
                             if (!((SuperPTSetting)pt.Upline.Upline).PropertiesChangedList.Contains(arg.PropertyName))
                             {
                                 ((SuperPTSetting)pt.Upline.Upline).PropertiesChangedList.Add(arg.PropertyName);
                             }
                         };
            }
            else
            {
                ((SuperPTSetting)pt.Upline.Upline).PropertiesChangedList.Add("PresetPT");
                ((SuperPTSetting)pt.Upline.Upline).PropertiesChangedList.Add("AutoPresetPT");
                ((SuperPTSetting)pt.Upline.Upline).PropertiesChangedList.Add("IsAutoPresetPT");
            }

            ((PTSetting)pt.Upline.Upline).PresetPT = PresetPT;
            ((PTSetting)pt.Upline.Upline).AutoPresetPT = AutoPresetPT;
            ((PTSetting)pt.Upline.Upline).IsAutoPresetPT = IsAutoPresetPT;

            var results = new Dictionary<int, Fanex.LiveCasinoSetting.Model.Result>();

            if (Custids.Count > 0)//Multiple updates
            {
                results = PtSetting.UpdateMemberPTSettingByMaster
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
                var result = PtSetting.UpdateMemberPTSettingByMaster
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

            ProcessMessage
                (
                    results,
                    false,
                    pt.Upline.Upline.IsPresetPTChanged |
                    pt.Upline.Upline.IsIsAutoPresetPTChanged |
                    pt.Upline.Upline.IsAutoPresetPTChanged,
                    false
                );

            return null;
        }
    }
}