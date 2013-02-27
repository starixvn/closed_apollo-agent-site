using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fanex.Age.UI;
using Sunplus.Agent.Utilities;
using DNAide.Core;

namespace Sunplus.Agent.UI._BetList.P2P
{
    public partial class TableSession_Page : TplPage
    {
        protected const string EVEN_ROW_CLASS = Fanex.Age.UI.Common.RowBgEven;
        protected const string ODD_ROW_CLASS = Fanex.Age.UI.Common.RowBgOdd;
        private string gameType = "GameType";
        private long ibcrefno = 0;
        private long custId = 0;
        private string betType = "BetType";
        private string uname = "uname";
        //private string sessionId = "1694313869";
        //private string currency = "Currency";

        public override object ValidateRequest(Hashtable p)
        {
            return null;
        }

        public override void ProcessRequest(ref Hashtable p)
        {
            ibcrefno = CastAide.AsInt64(Request["ibcrefno"], 0);
            custId = CastAide.AsInt64(Request["custid"], 0);
            gameType = Request["typegame"];
            betType = Request["bettype"];
            uname = Request["uname"];
        }

        public override object InvokeData(Hashtable hash)
        {
            //string userCurrency = CurrentUserSession.Currency;
            if (ibcrefno > 0 && custId > 0)
            {
                var business = new Fanex.Age.Business.BetList.P2P.TableSessionBusiness();
                var data = business.GetTableSession(custId.ToString(), ibcrefno.ToString());
                //var data = business.GetTableSession("1382257", sessionId);
                return data;
            }

            return null;
        }

        public override void RenderContent(HtmlTextWriter writer)
        {
            TableSession_Template tpl = new TableSession_Template();
            tpl.Title.UserName = uname;
            tpl.DataRow.Clear();
            tpl.User_Currency = string.Format("({0})", CurrentUserSession.Currency);
            var data = (Fanex.Age.DTO.BetList.P2P.TableSession)View;
            TableSession_DataRow_Block row_Block = new TableSession_DataRow_Block();
            if (data == null)
            {
                tpl.NoInfoRow.Info = Resources.Lang.noInfo;
                tpl.TotalRow.Hide();
            }
            else
            {
                if (data.ErrorCode == 0)
                {
                    if (data.SessionItems.Count > 0)
                    {
                        Dictionary<string, Fanex.Age.DTO.BetList.P2P.SessionCurrency> dict = new Dictionary<string, Fanex.Age.DTO.BetList.P2P.SessionCurrency>();
                        Fanex.Age.DTO.BetList.P2P.SessionCurrency totalUserCurrency = new Fanex.Age.DTO.BetList.P2P.SessionCurrency
                        {
                            CurrencyName = CurrentUserSession.Currency,
                            WinLoss = 0,
                            Comm = 0,
                            GrossComm = 0
                        };

                        int id = 1;
                        foreach (var item in data.SessionItems)
                        {
                            row_Block.ColorClass = id % 2 == 1 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
                            row_Block.OrderId = id.ToString();
                            row_Block.Currency = item.TableCurrency.CurrencyName;
                            row_Block.Date = item.Date.ToString();
                            row_Block.Table = "";
                            row_Block.Tournament = "";
                            if (item.GameMode == "0")
                            {
                                row_Block.Table = string.Format("{0}", item.Table.Id);
                            }
                            else if (item.GameMode == "1")
                            {
                                row_Block.Tournament = string.Format("{0}", item.Tournament.Id);
                            }
                            row_Block.Currency = item.TableCurrency.CurrencyName;
                            row_Block.WinLoss = item.TableCurrency.WinLoss != null ? Formater.FormatNumber2(item.TableCurrency.WinLoss, CurrentUserSession.LangKey) : "N/A";
                            row_Block.WinLoss_Class = GetTdClass(item.TableCurrency.WinLoss);
                            row_Block.Comm = item.TableCurrency.Comm != null ? Formater.FormatNumber2(item.TableCurrency.Comm, CurrentUserSession.LangKey) : "N/A";
                            row_Block.Comm_Class = GetTdClass(item.TableCurrency.Comm);
                            row_Block.GrossComm = item.TableCurrency.GrossComm != null ? Formater.FormatNumber2(item.TableCurrency.GrossComm, CurrentUserSession.LangKey) : "N/A";
                            row_Block.GrossComm_Class = GetTdClass(item.TableCurrency.GrossComm);
                            row_Block.XRate = item.XRate != null ? Formater.FormatNumber2(item.XRate, CurrentUserSession.LangKey) : "N/A";
                            row_Block.User_WinLoss = Formater.FormatNumber2(item.UserCurrency.WinLoss, CurrentUserSession.LangKey);
                            row_Block.User_WinLoss_Class = GetTdClass(item.UserCurrency.WinLoss);
                            row_Block.User_Comm = Formater.FormatNumber2(item.UserCurrency.Comm, CurrentUserSession.LangKey);
                            row_Block.User_Comm_Class = GetTdClass(item.UserCurrency.Comm);
                            row_Block.User_GrossComm = Formater.FormatNumber2(item.UserCurrency.GrossComm, CurrentUserSession.LangKey);
                            row_Block.User_GrossComm_Class = GetTdClass(item.UserCurrency.GrossComm);
                            tpl.DataRow.Append(row_Block);

                            // calculate total Table Currency
                            if (dict.ContainsKey(item.TableCurrency.CurrencyName))
                            {
                                var currency = dict[item.TableCurrency.CurrencyName];
                                currency.WinLoss += item.TableCurrency.WinLoss != null ? item.TableCurrency.WinLoss : 0;
                                currency.Comm += item.TableCurrency.Comm != null ? item.TableCurrency.Comm : 0;
                                currency.GrossComm += item.TableCurrency.GrossComm != null ? item.TableCurrency.GrossComm : 0;
                            }
                            else
                            {
                                var currency = new Fanex.Age.DTO.BetList.P2P.SessionCurrency()
                                {
                                    CurrencyName = item.TableCurrency.CurrencyName,
                                    WinLoss = item.TableCurrency.WinLoss,
                                    Comm = item.TableCurrency.Comm,
                                    GrossComm = item.TableCurrency.GrossComm
                                };
                                dict.Add(item.TableCurrency.CurrencyName, currency);
                            }

                            totalUserCurrency.WinLoss += item.UserCurrency.WinLoss;
                            totalUserCurrency.Comm += item.UserCurrency.Comm;
                            totalUserCurrency.GrossComm += item.UserCurrency.GrossComm;
                            id++;
                        }

                        if (dict.Keys.Count > 0)
                        {
                            id = 0;
                            foreach (Fanex.Age.DTO.BetList.P2P.SessionCurrency currency in dict.Values)
                            {
                                string currencyName = currency.CurrencyName;
                                if (id == 0)
                                {
                                    tpl.TotalRow.Total_Currency = "";
                                    tpl.TotalRow.Total_WinLoss = "";
                                    tpl.TotalRow.Total_Comm = "";
                                    tpl.TotalRow.Total_GrossComm = "";
                                }
                                else
                                {
                                    tpl.TotalRow.Total_Currency += "<div class='div_horline'></div>";
                                    tpl.TotalRow.Total_WinLoss += "<div class='div_horline'></div>";
                                    tpl.TotalRow.Total_Comm += "<div class='div_horline'></div>";
                                    tpl.TotalRow.Total_GrossComm += "<div class='div_horline'></div>";
                                }
                                tpl.TotalRow.Total_Currency += currencyName;
                                tpl.TotalRow.Total_WinLoss += Formater.FormatNumber2(currency.WinLoss, CurrentUserSession.LangKey);
                                tpl.TotalRow.Total_Comm += Formater.FormatNumber2(currency.Comm, CurrentUserSession.LangKey);
                                tpl.TotalRow.Total_GrossComm += Formater.FormatNumber2(currency.GrossComm, CurrentUserSession.LangKey);
                                id++;
                            }

                            tpl.TotalRow.Total_User_WinLoss = Formater.FormatNumber2(totalUserCurrency.WinLoss, CurrentUserSession.LangKey);
                            tpl.TotalRow.Total_User_Comm = Formater.FormatNumber2(totalUserCurrency.Comm, CurrentUserSession.LangKey);
                            tpl.TotalRow.Total_User_GrossComm = Formater.FormatNumber2(totalUserCurrency.GrossComm, CurrentUserSession.LangKey);
                        }
                        else
                        {
                            tpl.NoInfoRow.Info = Resources.Lang.noInfo;
                            tpl.TotalRow.Hide();
                        }
                        tpl.NoInfoRow.Hide();
                    }
                    else
                    {
                        tpl.NoInfoRow.Info = Resources.Lang.noInfo;
                        tpl.TotalRow.Hide();
                    }
                }
                else
                {
                    tpl.NoInfoRow.Info = data.ErrorMessage;
                    tpl.TotalRow.Hide();
                }
            }

            writer.Write(tpl);
        }

        private string GetTdClass(double? value)
        {
            return (value == null || value < 0) ? "negative" : "positive";
        }
    }
}