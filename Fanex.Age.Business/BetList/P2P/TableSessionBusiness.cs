namespace Fanex.Age.Business.BetList.P2P
{
    using System;
    using Fanex.Age.DTO.BetList.P2P;
    using Fanex.Age.Utility;

    /// <summary>
    /// TableSessionBusiness
    /// </summary>
    public class TableSessionBusiness : BaseBusiness
    {
        public TableSession GetTableSession(string custId, string sessionId)
        {
            using (WebServiceHandle service = new WebServiceHandle())
            {
                var sessionDetail = service.getLoginSessionDetail(SiteId, custId, sessionId);
                if (sessionDetail != null)
                {
                    var result = new TableSession();
                    result.ErrorCode = sessionDetail.Error_code;
                    string errMsg = string.Empty;
                    switch (sessionDetail.Error_code)
                    {
                        case 0:
                            errMsg = "Successfully executed";
                            break;

                        case 1:
                            errMsg = "Failed executed";
                            break;

                        case 2:
                            errMsg = "Data not found";
                            break;

                        case 3:
                            errMsg = "User not found";
                            break;

                        case 4:
                            errMsg = "Site id error";
                            break;

                        case 8:
                            errMsg = "This system does not open";
                            break;

                        case 9:
                            errMsg = "This function does not open";
                            break;
                        default:
                            errMsg = "Unknown Error";
                            break;
                    }

                    result.ErrorMessage = errMsg;

                    if (sessionDetail.Error_code == 0)
                    {
                        for (int i = 0; i < sessionDetail.TableListCount; i++)
                        {
                            var x = sessionDetail.TableList[i];
                            double? xrate = 0;
                            if (x.Table_Exchange_Rate != 0)
                            {
                                xrate = x.User_Exchange_Rate / x.Table_Exchange_Rate;
                            }
                            else
                            {
                                xrate = null;
                                Logs.Critical(string.Format("Table_Exchange_Rate must not be zero {custId: {0}, sessionId: {1}}", custId, sessionId));
                            }

                            var item = new SessionItem
                            {
                                Date = Convert.ToDateTime(x.Table_Into_Time).AddHours(-12),
                                Table = new IdName
                                {
                                    Id = x.Table_ID,
                                    Name = "Table"
                                },
                                Tournament = new IdName
                                {
                                    Id = x.Tournament_Id,
                                    Name = "Tournament"
                                },
                                GamingId = x.Gaming_id,
                                GameMode = x.Game_Mode,
                                XRate = xrate,
                                UserCurrency = new SessionCurrency
                                {
                                    CurrencyName = x.User_Currency,
                                    WinLoss = x.WinLoss,
                                    Comm = x.RakeBack_User,
                                    GrossComm = x.TotalRake
                                },
                                TableCurrency = new SessionCurrency
                                {
                                    CurrencyName = x.Table_Currency,
                                    WinLoss = xrate != null ? x.WinLoss / xrate : null,
                                    Comm = xrate != null ? x.RakeBack_User / xrate : null,
                                    GrossComm = xrate != null ? x.TotalRake / xrate : null
                                }
                            };
                            result.SessionItems.Add(item);
                        }
                    }

                    return result;
                }

                return null;
            }
        }
    }
}