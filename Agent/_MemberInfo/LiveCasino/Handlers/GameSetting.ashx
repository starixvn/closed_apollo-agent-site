<%@ WebHandler Language="C#" Class="Fanex.Age.UI.LiveCasino.GameSetting_Handler" %>
/*
    Created by: Andy
    Description: Handler for processing game setting
*/
using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;

using Fanex.Age.UI;
using Fanex.LiveCasinoSetting.Request;
using Fanex.LiveCasinoSetting.Model;
using Fanex.LiveCasinoSetting.Model.Log;

using System.ComponentModel;
using BLL;

using Sunplus.Agent.Model.BL.AccountInfo;

namespace Fanex.Age.UI.LiveCasino
{
    public class GameSetting_Handler : LiveCasinoHandler, System.Web.SessionState.IReadOnlySessionState
    {
        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal)
            {
                return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, "1|" + BeginDivErr + Resources.Lang.gameSetting + ": " + Resources.Lang.lblNotAuthorized + EndDiv);
            }

            return base.ValidateRequest(p);
        }

        private List<GameType> oldValues;
        private List<GameType> newValues;

        private Dictionary<int, List<GameType>> oldValuesDic = new Dictionary<int, List<GameType>>();
        private Dictionary<int, Fanex.LiveCasinoSetting.Model.Result> result;
        private Dictionary<int, Dictionary<int, Fanex.LiveCasinoSetting.Model.Result>> results;

        private System.Threading.Thread tWriteLog;

        private int SubAccId = 0;
        private int UserId = 0;

        private string SubAccName = string.Empty;
        private string Lang = string.Empty;
        private string IP = string.Empty;
        private string errPT = string.Empty;
        private string errGS = string.Empty;

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            data.AsResult(View as Fanex.Age.UI.Result, true);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            var cust = PtSetting.GetCustomerInfo(RequestCustId);

            UserId = CurrentUserSession.UserId;
            SubAccId = CurrentUserSession.SubAccId;
            SubAccName = CurrentUserSession.SubUserName;
            Lang = CurrentUserSession.LangKey;
            IP = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP();

            //Sync mannually
            SynManually(true);

            var res = GameSetting.GetByCustomerId(RequestCustId);

            if ((res == null || res.Count == 0) && CurrentUserSession.RoleId - RequestCustRoleId == 1)
            {
                res = GameSetting.GetByCustomerId(CurrentUserSession.CustId);
            }

            res.ForEach(itemGet => itemGet.BettingLimit.ForEach(item =>
            {
                GameTypes.ForEach(g =>
                {
                    if (g.BetTypeId == itemGet.BetTypeId)
                    {
                        g.BettingLimit.ForEach(b =>
                        {
                            if (b.Id == item.Id && ((b.IsSelected != item.IsSelected) || b.IsSelected))
                            {
                                b.IsDirty = true;
                            }
                        });
                    }
                });
            }));

            oldValues = res;
            newValues = GameTypes;

            if (Custids.Count > 0)//Multiple updates
            {
                var request = new Dictionary<int, InsertAccountRequest>();

                Custids.ForEach(custId =>
                    {
                        var old = GameSetting.GetByCustomerId(custId);

                        if ((old == null || old.Count == 0) && CurrentUserSession.RoleId - RequestCustRoleId == 1)
                        {
                            old = GameSetting.GetByCustomerId(CurrentUserSession.CustId);
                        }

                        oldValuesDic.Add(custId, old);

                        var insertAccReq = GetManualInsertAccountRequest(custId);

                        request.Add(custId, insertAccReq);
                    });

                if (Custids.Count > 0)
                {
                    var tmpIds = new List<int>();

                    Custids.ForEach(o =>
                    {
                        tmpIds.Add(o);
                    });

                    UnsyncTmp.ForEach(id =>
                        {
                            tmpIds.RemoveAll(o => o == id);
                        });

                    if (tmpIds.Count > 0)
                    {
                        results = GameSetting.UpdateGameSetting(tmpIds, CurrentUserSession.RoleId, GameTypes, request);

                        if (IsSubmit && CurrentUserSession.RoleId == 2 && RequestCustRoleId == 1 && !cust.UplineDisableLiveCasino)
                        {
                            Custids.ForEach(id =>
                            {
                                var req = new UpdateLiveCasinoStatusRequest(request[id]);
                                req.IsDisabledLiveCasino = !EnableLiveCS;

                                PtSetting.UpdateStatusLiveCasino(req);
                            });
                        }

                        tmpIds.ForEach(custId =>
                        {
                            UpdateSyncStatus(custId, results[custId]);
                        });
                    }

                    if (results != null && results.Count > 0)
                    {
                        foreach (var item in results.Values)
                        {
                            var code = "";
                            var msg = "";

                            foreach (var item2 in item.Values)
                            {
                                if (item2 != null)
                                {
                                    var codes = item2.ErrorCode.Split('\a');
                                    var msgs = item2.ErrorMessage.Split('\a');

                                    for (int i = 0; i < codes.Length; i++)
                                    {
                                        var enableMsgFailed = string.Empty;

                                        if (IsSubmit && CurrentUserSession.RoleId == 2 && RequestCustRoleId == 1 && !cust.UplineDisableLiveCasino)
                                        {
                                            enableMsgFailed = BeginDivSucc + Resources.Lang.enable + ": " + Resources.Lang.updatesuccessfull + EndDiv;
                                        }
                                        else if (IsSubmit && CurrentUserSession.RoleId == 2 && RequestCustRoleId == 1 && !DisableCheckBox)
                                        {
                                            enableMsgFailed = BeginDivErr + Resources.Lang.enable + ": " + Resources.Lang.updateFailed + EndDiv;
                                        }

                                        if (string.IsNullOrWhiteSpace(codes[i]))
                                        {
                                            var tmpValue = string.Empty;

                                            msg += BeginDivSucc + Resources.Lang.gameSetting + ": " + Resources.Lang.updatesuccessfull + EndDiv + enableMsgFailed + "$";

                                            tmpValue = msg.Contains(Resources.Lang.updateFailed) ? "-1$" : "0$";
                                            code += tmpValue;

                                            continue;
                                        }

                                        code += codes[i] + "$";
                                        msg += BeginDivErr + msgs[i] + EndDiv + enableMsgFailed + "$";
                                    }
                                }
                            }

                            ErrCodeStrs += code;
                            ErrMsg += msg;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(ErrCode))
                        {
                            Custids.ForEach(o =>
                            {
                                ErrCodeStrs += "-1$";
                            });
                        }
                        else
                        {
                            ErrCodeStrs = ErrCode;
                        }

                        if (IsSubmit)
                            ErrMsg = "$";
                    }
                }
            }
            else//Single update
            {
                if (!WrongPtIds.Contains(RequestCustId) && !UnsyncTmp.Contains(RequestCustId))
                {
                    var req = GetManualInsertAccountRequest(RequestCustId);

                    result = GameSetting.UpdateGameSetting(CurrentUserSession.RoleId, GameTypes, req);

                    if (IsSubmit && CurrentUserSession.RoleId == 2 && RequestCustRoleId == 1 && !cust.UplineDisableLiveCasino)
                    {
                        req.IsDisabledLiveCasino = !EnableLiveCS;
                        PtSetting.UpdateStatusLiveCasino(new UpdateLiveCasinoStatusRequest(req));
                    }

                    UpdateSyncStatus(RequestCustId, result);

                    var value = new Fanex.LiveCasinoSetting.Model.Result();

                    if (IsSubmit)
                    {
                        ErrCode = string.Empty;
                        ErrMsg = string.Empty;
                    }

                    foreach (var item in GameTypes)
                    {
                        result.TryGetValue(item.BetTypeId, out value);

                        if (value != null)
                        {
                            ErrCode += value.ErrorCode;
                            ErrMsg += value.ErrorMessage;
                        }
                    }
                }
            }

            var r = new Fanex.Age.UI.Result(0, string.Empty);

            if (ErrCodeStrs.Length > 0)
            {
                r = new Fanex.Age.UI.Result(0, this.FormatMsg(ErrCodeStrs, ErrMsg));
            }
            else
            {
                if (!UnsyncTmp.Contains(RequestCustId) || AccInfo.GetCustInfo(RequestCustId).LiveCasinoSync || !IsSubmit)
                {
                    var codes = ErrCode.Split('\a');
                    var msgs = ErrMsg.Split('\a');

                    ErrCode = "0";

                    if (codes.Length == 1 && codes[0] == NoChangeGameErrorCode)
                    {
                        ErrCode = "0";
                        ErrMsg = ErrMsg + BeginDivSucc + Resources.Lang.gameSetting + ": " + Resources.Lang.updatesuccessfull + EndDiv;
                    }
                    else
                    {
                        for (int i = 0; i < codes.Length - 1; i++)
                        {
                            if (codes[i] != string.Empty && Convert.ToInt32(codes[i]) > 0)
                                ErrCode = "-999";

                            ErrMsg += msgs[i];
                        }
                    }

                    if ((Convert.ToInt32(ErrCode) >= 0 && string.IsNullOrWhiteSpace(ErrMsg)) || (ErrCode == "0" && !IsSubmit && string.IsNullOrWhiteSpace(MsgTmp)))
                        ErrMsg = BeginDivSucc + Resources.Lang.gameSetting + ": " + Resources.Lang.updatesuccessfull + EndDiv;
                    else if (Convert.ToInt32(ErrCode) >= 0 && !ErrMsg.Contains(Resources.Lang.gameSetting))
                        ErrMsg = ErrMsg.TrimEnd('$') + BeginDivSucc + Resources.Lang.gameSetting + ": " + Resources.Lang.updatesuccessfull + EndDiv;
                    else
                        ErrMsg = ErrMsg.TrimEnd('$');

                    if (IsSubmit && CurrentUserSession.RoleId == 2 && RequestCustRoleId == 1 && !cust.UplineDisableLiveCasino)
                    {
                        ErrMsg += BeginDivSucc + Resources.Lang.enable + ": " + Resources.Lang.updatesuccessfull + EndDiv;
                    }
                    else if (IsSubmit && CurrentUserSession.RoleId == 2 && RequestCustRoleId == 1 && !DisableCheckBox)
                    {
                        ErrMsg += BeginDivErr + Resources.Lang.enable + ": " + Resources.Lang.updateFailed + EndDiv;
                    }

                    if (ErrMsg.Contains(Resources.Lang.gameSettingErrMsg) || ErrMsg.Contains(Resources.Lang.gameSettingErrMsgStake) || ErrMsg.Contains(Resources.Lang.updateFailed))
                        ErrCode = "-1";

                    r = new Fanex.Age.UI.Result(0, this.FormatMsg(ErrCode.ToString(), ErrMsg));
                }
                else
                {
                    var tmpCode = string.Empty;

                    if (IsSubmit)
                    {
                        if (string.IsNullOrWhiteSpace(ErrMsg))
                        {
                            UnsyncTmp.ForEach(o =>
                            {
                                tmpCode += "-1$";
                                ErrMsg += BeginDivErr + Resources.Lang.gameSetting + ": " + Resources.Lang.gameSettingErrMsg + EndDiv + "$";
                            });
                        }
                        else
                        {
                            ErrMsg = string.Empty;
                        }

                        if (!cust.UplineDisableLiveCasino && CurrentUserSession.RoleId == 2 && RequestCustRoleId == 1 && (!UnsyncTmp.Contains(RequestCustId) || AccInfo.GetCustInfo(RequestCustId).LiveCasinoSync))
                        {
                            ErrMsg += BeginDivSucc + Resources.Lang.enable + ": " + Resources.Lang.updatesuccessfull + EndDiv;
                        }
                        else if (IsSubmit && CurrentUserSession.RoleId == 2 && RequestCustRoleId == 1 && !DisableCheckBox)
                        {
                            ErrMsg += BeginDivErr + Resources.Lang.enable + ": " + Resources.Lang.updateFailed + EndDiv;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(ErrMsg))
                        {
                            UnsyncTmp.ForEach(o =>
                            {
                                tmpCode += "-1$";
                                ErrMsg += BeginDivErr + Resources.Lang.gameSetting + ": " + Resources.Lang.gameSettingErrMsg + EndDiv + "$";
                            });
                        }
                    }

                    if (string.IsNullOrWhiteSpace(tmpCode))
                    {
                        if (UnsyncTmp.Count > 0)
                        {
                            UnsyncTmp.ForEach(o =>
                            {
                                tmpCode += "-1$";
                            });
                        }
                        else
                        {
                            tmpCode = "-1";
                        }
                    }

                    r = new Fanex.Age.UI.Result(0, this.FormatMsg(string.IsNullOrWhiteSpace(tmpCode) ? "-1" : tmpCode.TrimEnd('$'), ErrMsg.TrimEnd('$')));
                }
            }

            //Write log
            errPT = Resources.Lang.gameSettingErrMsg;
            errGS = Resources.Lang.gameSettingErrMsgStake;

            tWriteLog = new System.Threading.Thread(WriteLog);
            tWriteLog.Start();

            return r;
        }

        private void WriteLog()
        {
            try
            {
                var log = new Fanex.LiveCasinoSetting.Business.LogWriterService();

                if (Custids.Count > 0)
                {
                    foreach (var custid in Custids)
                    {
                        oldValues = oldValuesDic[custid];

                        var res = new LiveCasinoSetting.Model.Result();

                        if (results == null)
                            results = GameResult;

                        if (results[custid] == null)
                        {
                            result = GameResult[custid];
                            res = GameResult[custid].FirstOrDefault().Value;
                        }
                        else
                        {
                            result = results[custid];
                            res = results[custid].FirstOrDefault().Value;
                        }

                        if (result != null && result.Count > 0 && !ErrMsg.Contains(errPT) && !ErrMsg.Contains(errGS))
                        {
                            log.WriteLog
                                    (
                                        new LogUpdateStakeLevel(oldValues, newValues, result)
                                        {
                                            Action = LogActionUpdateStakeLevel,
                                            ErrorCode = res.ErrorCode,
                                            ErrorMessage = res.ErrorMessage,
                                            RoleId = RequestCustRoleId,
                                            SubAccId = SubAccId,
                                            SubAccName = SubAccName,
                                            CustId = custid,
                                            SignUpIp = IP,
                                            Language = Lang,
                                            UserId = UserId
                                        }
                                    );
                        }
                    }
                }
                else if (!IsSubmit || !UnsyncTmp.Contains(RequestCustId) || (IsSubmit && string.IsNullOrWhiteSpace(MsgTmp)))
                {
                    if (result == null)
                        result = GameResult[RequestCustId];

                    if (result != null && result.Count > 0 && !ErrMsg.Contains(errPT) && !ErrMsg.Contains(errGS))
                    {
                        log.WriteLog
                                (
                                    new LogUpdateStakeLevel(oldValues, newValues, result)
                                    {
                                        Action = LogActionUpdateStakeLevel,
                                        ErrorCode = result.FirstOrDefault().Value.ErrorCode,
                                        ErrorMessage = result.FirstOrDefault().Value.ErrorMessage,
                                        RoleId = RequestCustRoleId,
                                        SubAccId = SubAccId,
                                        SubAccName = SubAccName,
                                        CustId = RequestCustId,
                                        SignUpIp = IP,
                                        Language = Lang,
                                        UserId = UserId
                                    }
                                );
                    }
                }
            }
            catch (Exception ex)
            {
                tWriteLog.Abort();
                Fanex.Logging.LogService.Error(ex.Message, ex);
                return;
            }
        }

        private void UpdateSyncStatus(int custId, Dictionary<int, LiveCasinoSetting.Model.Result> results)
        {
            if (!WrongPtIds.Contains(custId) && !AccInfo.GetCustInfo(custId).LiveCasinoSync && string.IsNullOrWhiteSpace(results.FirstOrDefault().Value.ErrorCode))
            {
                var req = GetManualInsertAccountRequest(custId);

                if (IsSubmit && CurrentUserSession.RoleId == 2 && RequestCustRoleId == 1 && !PtSetting.GetCustomerInfo(custId).UplineDisableLiveCasino)
                {
                    req.IsDisabledLiveCasino = !EnableLiveCS;
                }

                GameSetting.UpdateSyncLiveCasinoInfo(req);
            }
        }
    }
}