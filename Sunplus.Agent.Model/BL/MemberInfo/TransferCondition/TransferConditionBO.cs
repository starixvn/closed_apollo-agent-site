// -----------------------------------------------------------------------
// <copyright file="TransferConditionBO.cs" company="Nexcel Solution">
// TODO: Data access object of Transfer condition function.
// Created 20120719@Jeff
// Revision:
// ?@? - ... 
// </copyright>
// -----------------------------------------------------------------------

namespace Sunplus.Agent.Model.BL.MemberInfo.TransferCondition
{
    using System.Collections;
    using System.Data;
    using Sunplus.Agent.Model.BL.Base;
    using Sunplus.Agent.Model.DL.MemberInfo.TransferCondition;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TransferConditionBO : BOBase
    {
        public TransferConditionBO(string netSessionId)
            : base(netSessionId)
        { 
        }

        public string GetTransferCondition(int custId)
        {
            TransferConditionDAO dao = new TransferConditionDAO();

            return dao.Acc_CustomerSelTransferOption(custId);
        }

        public Hashtable UpdateTransferCondition(string custId, string transferOption, string ip)
        {
            TransferConditionDAO dao = new TransferConditionDAO();

            return dao.Acc_CustomerUpdTransferOption(custId, transferOption, CurrentSession.UserId.ToString(), CurrentSession.SubAccId, CurrentSession.LangKey, ip);
        }
    }
}
