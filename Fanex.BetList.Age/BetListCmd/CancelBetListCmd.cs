// ---------------------------------------------------------------------------------
// <copyright file="CancelBetListCmd.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2013.02.19">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.BetListCmd
{
    using System.Collections.Generic;
    using System.Data;
    using Sunplus.Agent.Model.BL.BetList;
    using Sunplus.Agent.Model.BL.Definitions;

    /// <summary>
    /// Summary description for CancelBetListCmd
    /// </summary>
    public class CancelBetListCmd : BetListCmd
    {
        private BetListBO betListBO = null;
        private DataTable outDataTable = null;

        private int masterId = 0;
        private int agentId = 0;
        private int memberId = 0;

        public CancelBetListCmd(string custId)
        {
            this.betListBO = new BetListBO(CurrentUserSession.NetSessionId);
        }

        public override void SetParams(System.Collections.Hashtable hash)
        {
            this.masterId = (int)hash["masterId"];
            this.agentId = (int)hash["agentId"];
            this.memberId = (int)hash["memberId"];
        }

        /// <summary>
        /// Return Dictionary<string, DataTable> : dtCombo1, dtCombo2, dtCombo3, dtDeletedBets
        /// </summary>
        /// <returns>dtCombo1, dtCombo2, dtCombo3, dtDeletedBets</returns>
        public override object Execute()
        {
            Dictionary<string, DataTable> results = new Dictionary<string, DataTable>();

            DataTable combo1 = this.betListBO.GetCancelBetList_UserList(CurrentUserSession.CustId, CurrentUserSession.RoleId);
            DataTable combo2 = new DataTable();
            DataTable combo3 = new DataTable();

            if (CurrentUserSession.IsSuper)
            {
                // dtCombo1 is list master
                if (this.masterId == 0)
                {
                    if (combo1.Rows.Count > 0)
                    {
                        this.masterId = (int)combo1.Rows[0]["custid"];
                    }
                }

                combo2 = this.betListBO.GetCancelBetList_UserList(this.masterId, (int)UserTypes.Master);

                if (this.agentId > 0 && combo2.Rows.Count > 0 && combo2.Select("custid=" + this.agentId).Length > 0)
                {
                    combo3 = this.betListBO.GetCancelBetList_UserList(this.agentId, (int)UserTypes.Agent);
                }
            }
            else if (CurrentUserSession.IsMaster)
            {
                // dtCombo1 is list agent
                if (this.agentId == 0)
                {
                    if (combo1.Rows.Count > 0)
                    {
                        this.agentId = (int)combo1.Rows[0]["custid"];
                    }
                }

                combo2 = this.betListBO.GetCancelBetList_UserList(this.agentId, (int)UserTypes.Agent);
            }
            else if (CurrentUserSession.IsAgent)
            {
                // dtCombo1 is list member
                if (this.memberId == 0)
                {
                    if (combo1.Rows.Count > 0)
                    {
                        this.memberId = (int)combo1.Rows[0]["custid"];
                    }
                }
            }

            results.Add("dtCombo1", combo1);
            results.Add("dtCombo2", combo2);
            results.Add("dtCombo3", combo3);

            DataTable deletedBets = new DataTable();

            bool isGet = false;
            if (this.memberId > 0)
            {
                if ((combo3.Rows.Count > 0 && combo3.Select("custid=" + this.memberId).Length > 0)
                    || (combo2.Rows.Count > 0 && combo2.Select("custid=" + this.memberId).Length > 0)
                    || (combo1.Rows.Count > 0 && combo1.Select("custid=" + this.memberId).Length > 0))
                {
                    deletedBets = this.betListBO.GetCancelBetList_DeletedBets(this.memberId, (int)UserTypes.Member, out this.outDataTable);
                    isGet = true;
                }
            }

            if (this.agentId > 0 && isGet == false)
            {
                if ((combo2.Rows.Count > 0 && combo2.Select("custid=" + this.agentId).Length > 0) || (combo1.Rows.Count > 0 && combo1.Select("custid=" + this.agentId).Length > 0))
                {
                    deletedBets = this.betListBO.GetCancelBetList_DeletedBets(this.agentId, (int)UserTypes.Agent, out this.outDataTable);
                    isGet = true;
                }
            }

            if (this.masterId > 0 && isGet == false)
            {
                if (combo1.Rows.Count > 0 && combo1.Select("custid=" + this.masterId).Length > 0)
                {
                    deletedBets = this.betListBO.GetCancelBetList_DeletedBets(this.masterId, (int)UserTypes.Master, out this.outDataTable);
                }
            }

            results.Add("dtDeletedBets", deletedBets);

            return results;
        }

        public override DataTable OutDataTable()
        {
            return this.outDataTable;
        }
    }
}