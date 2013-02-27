<%@ WebHandler Language="C#" Class="Fanex.Age.UI.LiveCasino.MasterPTComm_Handler" %>
/*
    Created by: Andy
    Description: Handler for processing super edits master pt setting
*/
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;

using Fanex.Age.UI;
using Fanex.LiveCasinoSetting.Request;
using Fanex.LiveCasinoSetting.Model;

using System.ComponentModel;
using BLL;

namespace Fanex.Age.UI.LiveCasino
{
    public class MasterPTComm_Handler : LiveCasinoHandler, System.Web.SessionState.IReadOnlySessionState
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
            var pt = PtSetting.GetMasterPTSeting(RequestCustId, CurrentUserSession.CustId);

            if ((Custids.Count == 0 && !IsSyncLiveCasino) || Custids.Count > 0)
            {
                pt.PropertiesChangedList.Add("MaxPT");
                pt.PropertiesChangedList.Add("MinPT");
                pt.PropertiesChangedList.Add("Comm");

                ((SuperPTSetting)pt.Upline).PropertiesChangedList.Add("PresetPT");
                ((SuperPTSetting)pt.Upline).PropertiesChangedList.Add("AutoPresetPT");
                ((SuperPTSetting)pt.Upline).PropertiesChangedList.Add("IsAutoPresetPT");
            }
            else
            {
                //Handles Master values changed
                ((INotifyPropertyChanged)pt).PropertyChanged
                                 += (o, arg) =>
                                 {
                                     if (!pt.PropertiesChangedList.Contains(arg.PropertyName))
                                     {
                                         pt.PropertiesChangedList.Add(arg.PropertyName);
                                     }
                                 };

                //Handles Super values changed
                ((INotifyPropertyChanged)pt.Upline).PropertyChanged
                                 += (o, arg) =>
                                 {
                                     if (!((SuperPTSetting)pt.Upline).PropertiesChangedList.Contains(arg.PropertyName))
                                     {
                                         ((SuperPTSetting)pt.Upline).PropertiesChangedList.Add(arg.PropertyName);
                                     }
                                 };
            }

            //Super values
            ((SuperPTSetting)pt.Upline).PresetPT = PresetPT;
            ((SuperPTSetting)pt.Upline).AutoPresetPT = AutoPresetPT;
            ((SuperPTSetting)pt.Upline).IsAutoPresetPT = IsAutoPresetPT;
            ((SuperPTSetting)pt.Upline).IsUpdateAllMember = IsUpdateAllMember;
            ((SuperPTSetting)pt.Upline).NewMemberPT = NewMemberPT;

            //Master values
            pt.MaxPT = MaxPT;
            pt.MinPT = MinPT;
            pt.Comm = Comm;

            var results = new Dictionary<int, Fanex.LiveCasinoSetting.Model.Result>();

            if (Custids.Count > 0)//Multiple updates
            {
                results = PtSetting.UpdateMasterPTSetting
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
                var result = PtSetting.UpdateMasterPTSetting
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