using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TplCore.Element;
using Sunplus.Agent.Model.BL.Transfer;
using Fanex.Age.Utility;
using Sunplus.Agent.Model.Entities;
using Fanex.Age.UI.Components.Transfer;

namespace Fanex.Age.UI.Components
{
    public class AllowTransfer_Control : IComponent
    {
        AllowTransfer_Template tpl = new AllowTransfer_Template();

        public AllowTransfer_Control(bool isAllowTransfer, bool isWarningTransfer, bool isTransfer)
        {
            GenerateControl(isAllowTransfer, isWarningTransfer, isTransfer);
        }
        public static implicit operator string(AllowTransfer_Control value)
        {
            return value.ToString();
        }
        public void HideLinkedResources()
        {

        }
        public override string ToString()
        {
            return tpl.ToString();
        }

        private void GenerateControl(bool isAllowTransfer, bool isWarningTransfer, bool isTransfer)
        {
            if (isTransfer)
            {
                if (isAllowTransfer)
                {
                    if (isWarningTransfer)
                    {
                        tpl.Allow_transfer.Warning_transfer.warning_message = Resources.Lang.warningmultipletransfer;
                    }
                    else
                    {
                        tpl.Allow_transfer.Warning_transfer = null;
                    }
                    tpl.Allow_transfer.allowtransfer = TransferCondition(UserSession.Current.CustId);
                }
                else
                {
                    tpl.Allow_transfer = null;
                }
            }
            else
            {
                tpl.Allow_transfer = null;
            }
        }

        #region Generate Transfer Condition

        private string TransferCondition(int custId)
        {
            string transferTime = "";
            string day = "";
            string[] Arrday = { Resources.Lang.sunday, Resources.Lang.monday, Resources.Lang.tuesday, Resources.Lang.wednesday, Resources.Lang.thursday, Resources.Lang.friday, Resources.Lang.saturday };

            TransferBO transfer = new TransferBO(UserSession.Current.NetSessionId);
            string transferOption= transfer.GetOptionTransferCondition(custId);
            if (!string.IsNullOrEmpty(transferOption))
            {
                transferTime += Resources.Lang.allowtransfer;
                char[] arrchar = transferOption.ToCharArray();
                if (arrchar.Length > 0)
                {
                    for (int i = 1; i < arrchar.Length - 1; i++)
                    {
                        if (Cast.AsString(arrchar[i]).Equals("1") == true)
                        {
                            day += " - " + Arrday[i].ToString();
                        }
                    }
                    if (Cast.AsString(arrchar[0]).Equals("1") == true)
                    {
                        day += " - " + Arrday[0].ToString();
                    }
                    if (day.Length > 0)
                    {
                        day = day.Substring(3);
                    }
                    if (Cast.AsString(arrchar[7]).Equals("1") == true)
                    {
                        transferTime += " " + day;
                    }
                    else
                    {
                        transferTime += " " + Resources.Lang.daily;
                    }
                }
                else
                {
                    transferTime += " ";
                }
            }
            return transferTime;
        }

        #endregion
    }
}