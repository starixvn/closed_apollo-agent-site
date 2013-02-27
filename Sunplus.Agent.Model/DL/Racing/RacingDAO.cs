/*
 * Created 20100115@Lee - Data access object of horse racing function
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.Model.DL.Racing
{
    using System.Collections;
    using System.Data;
    using Fanex.Data.DNAide;

    public class RacingDAO : Sunplus.Agent.Model.DL.Base.DAOBase
    {
        /// <summary>
        /// Get currencies which allow to play Horse racing.
        /// </summary>
        /// <returns></returns>
        public Hashtable Acc_SelHRAllowedCurrency()
        {
            Hashtable result = new Hashtable();
            DataTable dt = ExecSpTable("Acc_SelHRAllowedCurrency", null);

            int c = dt.Rows.Count;
            result = new Hashtable();
            for (int i = 0; i < c; i++)
            {
                DataRow row = dt.Rows[i];
                result.Add(row["currency"], row["currencyname"]);
            }

            return result;
        }

        public DataSet Acc_CustomerSelHRMember_All(int custId, out Hashtable outputs)
        {
            DbResult result = null;
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            result = ExecSp("Acc_CustomerSelHRMember_All", parameters);

            outputs = result.Outputs;

            return result.ResultSet;
        }

        public DataSet Acc_CustomerSelHRAgent_All(int custId, out Hashtable outputs)
        {
            DbResult result = null;
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            result = ExecSp("Acc_CustomerSelHRAgent_All", parameters);

            outputs = result.Outputs;

            return result.ResultSet;
        }

        public DataSet Acc_CustomerSelHRMaster_All(int custId, out Hashtable outputs)
        {
            DbResult result = null;
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            result = ExecSp("Acc_CustomerSelHRMaster_All", parameters);

            outputs = result.Outputs;

            return result.ResultSet;
        }

        public DataSet Acc_CustomerSelHRSuper_All(int custId, out Hashtable outputs)
        {
            DbResult result = null;
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            result = ExecSp("Acc_CustomerSelHRSuper_All", parameters);

            outputs = result.Outputs;

            return result.ResultSet;
        }

        public DataSet Acc_CustomerSelHRAgent_SAg(int custId, out Hashtable outputs)
        {
            DbResult result = null;
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            result = ExecSp("Acc_CustomerSelHRAgent_SAg", parameters);

            outputs = result.Outputs;

            return result.ResultSet;
        }

        public Hashtable Acc_CustomerUpdAllSettingHR(Hashtable parameters)
        {
            DbResult result = null;
            Hashtable resultMsg = new Hashtable();
            result = ExecSp("Acc_CustomerUpdAllSettingHR", parameters);
            if (result != null)
            {
                resultMsg.Add("errCode", result.Outputs["errcode"]);
                resultMsg.Add("errMsg", result.Outputs["errmsg"]);
            }

            return resultMsg;
        }

        public DataSet Acc_CustomerUpdMasterHR_All(Hashtable parameters, out Hashtable outputs)
        {
            DbResult result = ExecSp("Acc_CustomerUpdMasterHR_All", parameters);

            outputs = result.Outputs;

            return result.ResultSet;
        }

        public DataSet Acc_CustomerUpdAgentHR_All(Hashtable parameters, out Hashtable outputs)
        {
            DbResult result = ExecSp("Acc_CustomerUpdAgentHR_All", parameters);

            outputs = result.Outputs;

            return result.ResultSet;
        }

        public DataSet Acc_CustomerUpdMemberHR_All(Hashtable parameters, out Hashtable outputs)
        {
            DbResult result = ExecSp("Acc_CustomerUpdMemberHR_All", parameters);

            outputs = result.Outputs;

            return result.ResultSet;
        }

        public DataSet Acc_CustomerUpdMemberHR_SAg(Hashtable parameters, out Hashtable outputs)
        {
            DbResult result = ExecSp("Acc_CustomerUpdMemberHR_SAg", parameters);

            outputs = result.Outputs;

            return result.ResultSet;
        }

        public DataSet Acc_CustomerUpdMemberHR_MAg(Hashtable parameters, out Hashtable outputs)
        {
            DbResult result = ExecSp("Acc_CustomerUpdMemberHR_MAg", parameters);

            outputs = result.Outputs;

            return result.ResultSet;
        }

        public DataSet Acc_CustomerSelHREditMember_SAg(int custId, out Hashtable outputs)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);            

            DbResult result = ExecSp("Acc_CustomerSelHREditMember_SAg", parameters);

            outputs = result.Outputs;

            return result.ResultSet;
        }

        public DataSet Acc_CustomerSelHREditMember_MAg(int custId, out Hashtable outputs)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            
            DbResult result = ExecSp("Acc_CustomerSelHREditMember_MAg", parameters);

            outputs = result.Outputs;

            return result.ResultSet;
        }

        public DataSet Acc_CustomerSelHRComEditOverride_MAg(int custId, out Hashtable outputs)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);

            DbResult result = ExecSp("Acc_CustomerSelHRComEditOverride_MAg", parameters);

            outputs = result.Outputs;

            return result.ResultSet;
        }

        public DataSet Acc_CustomerSelHRComEditOverride_Ag(int custId, out Hashtable outputs)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);

            DbResult result = ExecSp("Acc_CustomerSelHRComEditOverride_Ag", parameters);

            outputs = result.Outputs;

            return result.ResultSet;
        }

        public DataSet Acc_CustOverrideUpdHR(Hashtable parameters, out Hashtable outputs)
        {
            DbResult result = ExecSp("Acc_CustOverrideUpdHR", parameters);

            outputs = result.Outputs;

            return result.ResultSet;
        }

        public void Acc_CustomerUpdRBStatus(Hashtable parameters, out string errCode, out string errMsg)
        {
            DbResult result = ExecSp("Acc_CustomerUpdRBStatus", parameters);

            errCode = result.Outputs["errcode"] as string;
            errMsg = result.Outputs["errmsg"] as string;
        }

        public DataTable Acc_CustomerSelRBStatus(int custId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);

            return ExecSpTable("Acc_CustomerSelRBStatus", parameters);
        }
    }
}