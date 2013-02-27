// -----------------------------------------------------------------------
// <copyright file="TransferConditionBO.cs" company="Nexcel Solution">
// TODO: Data access object of Transfer condition function.
// Created 20120719@Jeff
// Revision:
// ?@? - ... 
// </copyright>
// -----------------------------------------------------------------------

namespace Sunplus.Agent.Model.DL.MemberInfo.TransferCondition
{
    using System.Collections;
    using System.Data;
    using Sunplus.Agent.Model.DL.Base;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TransferConditionDAO : DAOBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public string Acc_CustomerSelTransferOption(int custId)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("custid", custId);
            parameters.Add("transferoption", "00000000");

            Hashtable outputs = ExecSpHashtable("Acc_CustomerSelTransferOption", parameters);

            return (string)outputs["transferoption"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public Hashtable Acc_CustomerUpdTransferOption(string custId, string transferOption, string userId, int subAccID, string langKey, string ip)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("custid", custId);
            parameters.Add("transferoption", transferOption);
            parameters.Add("userid", userId);
            parameters.Add("subaccid", subAccID);
            parameters.Add("language", langKey);
            parameters.Add("ip", ip);
            parameters.Add("returnerrcode", 1);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);

            return ExecSpHashtable("Acc_CustomerUpdTransferOption", parameters);
        }
        
    }
}
