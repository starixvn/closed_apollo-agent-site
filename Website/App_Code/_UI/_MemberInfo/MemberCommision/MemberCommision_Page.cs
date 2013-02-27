// --------------------------------------------------------------------------
// <copyright file="OnMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="MemberCommision.Trinh" date="2012.06.01">Improve performance, Check StypeCop</change>
// </history>

namespace Sunplus.Agent.UI._MemberCommision
{
    using System;
    using System.Data;
    using System.Text;
    using Fanex.Age.UI;
    using Resources;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.UI._MemberInfo.Commission;

    /// <summary>
    /// Defines permissions for sub-account
    /// </summary>
    public abstract class MemberCommision_Page : TplPage
    {
        public string GetComboCustomer(bool isComboAgent, DataTable dtb, int idMaster, int idAgent, ref int idFistAgent)
        {
            string selected = string.Empty;
            StringBuilder result = new StringBuilder();

            if (dtb.Rows.Count > 0)
            {
                if (isComboAgent)
                {
                    result.Append("<option value=\"" + idMaster + "\" selected>" + Lang.allagent + "</option>");
                }

                DataRowCollection dtr = dtb.Rows;
                foreach (DataRow row in dtr)
                {
                    selected = idAgent != Convert.ToInt32(row["custid"]) ? string.Empty : " selected";
                    result.Append("<option value=\"" + row["custid"] + "\"" + selected + ">" + row["username"] + "</option>");
                }

                idFistAgent = Convert.ToInt32(dtb.Rows[0]["custid"]);
            }
            else
            {
                idFistAgent = 0;
                result.Append("<option value=0>" + Lang.noInfo + "</option>");
            }

            return result.ToString();
        }

        public string GroupOptionMemberCommission(string isOption)
        {
            string strChecked = "checked='checked'";
            CommonGroup_Template tplGroup = new CommonGroup_Template();

            tplGroup.CheckedSP = isOption == "SB" ? strChecked : string.Empty;
            tplGroup.RaceBook.CheckedRC = isOption == "RC" ? strChecked : string.Empty;
            tplGroup.Casino.CheckedCA = isOption == "CA" ? strChecked : string.Empty;
            tplGroup.P2P.CheckedP2P = isOption == "P2P" ? strChecked : string.Empty;
            tplGroup.Bingo.CheckedBG = isOption == "BG" ? strChecked : string.Empty;
            tplGroup.LiveCasino.CheckedLC = isOption == "LC" ? strChecked : string.Empty;

            if (!UserPermission.IsShowRacing)
            {
                tplGroup.RaceBook.Clear();
            }

            if (!UserPermission.IsShowCasino)
            {
                tplGroup.Casino.Clear();
            }

            if (!UserPermission.IsShowP2P)
            {
                tplGroup.P2P.Clear();
            }

            if (!UserPermission.IsShowBingo || Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoCommission)
            {
                tplGroup.Bingo.Clear();
            }

            if (!UserPermission.IsShowLiveCasino)
            {
                tplGroup.LiveCasino.Clear();
            }

            return tplGroup;
        }
    }
}