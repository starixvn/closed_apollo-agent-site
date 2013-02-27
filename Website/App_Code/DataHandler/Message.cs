using System;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using Fanex.Data.DNAide;
using DNAide.Core;

using Sunplus.Agent.Utilities;
using System.Threading;
using System.Globalization;
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Model.BL.UserMessage;

namespace IBCAgent
{
    public class Message
    {
        #region Collection of column names
        private static string[] colNameOfDataTable = new string[] 
            {
                "Content_ch", "System.String",
                "Content_cs", "System.String",
                "Content_en", "System.String",
                "Content_it", "System.String",
                "Content_jp", "System.String",
                "Content_ko", "System.String",
                "Content_th", "System.String",
                "CreateTime", "System.DateTime",
                "CreatorId", "System.Int32",
                "CreatorName", "System.String",
                "IsSticky", "System.Boolean",
                "LastUpdateName", "System.String",
                "LastUpdateTime", "System.DateTime",
                "LastUpdator", "System.Int32",
                "MessageId", "System.Int32",
                "MessageType", "System.Int32",
                "Poster", "System.String",
                "PostTime", "System.DateTime",
                "UserId", "System.Int32",
                "UserName", "System.String"
            };
        #endregion
        
        private static string _ConnStringMsg = string.Empty;
        private static IBCMessage.Messenger mgr = null;

        public static string ConnStringMsg
        {
            get
            {
                if (string.IsNullOrEmpty(_ConnStringMsg))
                {
                    string strConn =
                        DNAide.Core.CastAide.AsString(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MessageDB"]);
                    _ConnStringMsg =
                        DNAide.Utils.Encryption.GetDeCryptConnectionString(strConn);
                }
                return _ConnStringMsg;
            }
        }

        public static void InitializeMessage()
        {
            mgr = new IBCMessage.Messenger(ConnStringMsg);
        }

        public static DataTable GetMessage(int messageid)
        {
            return null;
        }
        public static DataTable GetMessage(
            int targetId,
            int ispublic,
            int isspecial,
            int record)
        {
            DataTable dtMsg = new DataTable();
            InitializeMessage();
            IBCMessage.Message[] msgOf = mgr.GetMessage(targetId, ispublic, isspecial, record);
            if (msgOf != null && msgOf.Length > 0)
            {
                dtMsg = ConvertToDataTableForMsg(msgOf);
            }
            return dtMsg;
        }
        public static DataTable GetMessage(
            int targetId,
            int siteid,
            int ispublic,
            int isspecial,
            int record
        )
        {
            DataTable dtMsg = new DataTable();
            InitializeMessage();
            IBCMessage.Message[] msgOf = mgr.GetMessage(targetId, siteid, ispublic, isspecial, record);
            if (msgOf != null && msgOf.Length > 0)
            {
                dtMsg = ConvertToDataTableForMsg(msgOf);
            }
            return dtMsg;
        }
        public static DataTable GetMessage(
            int targetId,
            int ispublic,
            int isspecial,
            int record,
            string startDate,
            string endDate
        )
        {
            DataTable dtMsg = new DataTable();
            InitializeMessage();
            IBCMessage.Message[] msgOf = mgr.GetMessage(targetId, ispublic, isspecial, record, startDate, endDate);
            if (msgOf != null && msgOf.Length > 0)
            {
                dtMsg = ConvertToDataTableForMsg(msgOf);
            }
            return dtMsg;
        }
        public static DataTable GetMessage(
            int targetId,
            int siteid,
            int ispublic,
            int isspecial,
            int record,
            string startDate,
            string endDate
        )
        {
            DataTable dtMsg = new DataTable();
            InitializeMessage();
            IBCMessage.Message[] msgOf = mgr.GetMessage(targetId, siteid, ispublic, isspecial, record, startDate, endDate);
            if (msgOf != null && msgOf.Length > 0)
            {
                dtMsg = ConvertToDataTableForMsg(msgOf);
            }
            return dtMsg;
        }
        public static DataTable GetBalloonMessage(int targetid, string targetid2)
        {
            Hashtable htb = new Hashtable();

            htb.Add("targetid", targetid + "," + targetid2);

            DbResult dbResult = DNAide.Data.DbHelper.Execute("Dot_Msg_GetMessage_Simple", htb, 600);

            return dbResult.ResultSet.Tables[0];
        }

        public static string CountUnreadPriMessage(int userID, string userName)
        {
            string count = string.Empty;
            InitializeMessage();
            count = mgr.CountUnreadPriMsg(userID, userName);
            return (string.IsNullOrEmpty(count) ? "0" : count);
        }

        public static void SetReadPriMessage(int userID, string userName)
        {
            InitializeMessage();
            mgr.SetReadPriMsg(userID, userName);
        }
        
        public static string GetContentMsg(DataRow msg, string language)
        {
            string content = string.Empty;
            switch (language)
            {
                case "en-US":
                    content = DNAide.Core.CastAide.AsString(msg["Content_en"], string.Empty);
                    break;
                case "th-TH":
                    content = DNAide.Core.CastAide.AsString(msg["Content_th"], string.Empty);
                    break;
                case "zh-TW":
                    content = DNAide.Core.CastAide.AsString(msg["Content_ch"], string.Empty);
                    if (string.IsNullOrEmpty(content))
                    {
                        content = DNAide.Core.CastAide.AsString(msg["Content_cs"], string.Empty);
                    }
                    break;
                case "zh-CN":
                    content = DNAide.Core.CastAide.AsString(msg["Content_cs"], string.Empty);
                    if (string.IsNullOrEmpty(content))
                    {
                        content = DNAide.Core.CastAide.AsString(msg["Content_ch"], string.Empty);
                    }
                    break;
                case "ja-JP":
                    content = DNAide.Core.CastAide.AsString(msg["Content_jp"], string.Empty);
                    break;
                case "ko-KR":
                    content = DNAide.Core.CastAide.AsString(msg["Content_ko"], string.Empty);
                    break;
            }
            if (string.IsNullOrEmpty(content))
            {
                content = DNAide.Core.CastAide.AsString(msg["Content_en"], string.Empty);
            }
            return content;
        }

        public static DataTable CreateDataTableForMsg()
        {
            DataTable dt = new DataTable();
            DataColumn dc = null;

            for (int idxElement = 0; idxElement < colNameOfDataTable.Length; idxElement += 2)
            {
                dc = new DataColumn();
                dc.ColumnName = colNameOfDataTable[idxElement];
                dc.DataType = Type.GetType(colNameOfDataTable[idxElement+1]);
                dt.Columns.Add(dc);
            }

            return dt;
        }
        public static void FillDataForMsg(IBCMessage.Message msg, DataTable dtMsg)
        {
            DataRow dr = dtMsg.NewRow();
            dr["Content_ch"] = msg.Content_ch;
            dr["Content_cs"] = msg.Content_cs;
            dr["Content_en"] = msg.Content_en;
            dr["Content_it"] = msg.Content_it;
            dr["Content_jp"] = msg.Content_jp;
            dr["Content_ko"] = msg.Content_ko;
            dr["Content_th"] = msg.Content_th;
            dr["CreateTime"] = msg.CreateTime;
            dr["CreatorId"] = msg.CreatorId;
            dr["CreatorName"] = msg.CreatorName;
            dr["IsSticky"] = msg.IsSticky;
            dr["LastUpdateName"] = msg.LastUpdateName;
            dr["LastUpdateTime"] = msg.LastUpdateTime;
            dr["LastUpdator"] = msg.LastUpdator;
            dr["MessageId"] = msg.MessageId;
            dr["MessageType"] = msg.MessageType;
            dr["Poster"] = msg.Poster;
            dr["PostTime"] = msg.PostTime;
            dr["UserId"] = msg.UserId;
            dr["UserName"] = msg.UserName;
            
            dtMsg.Rows.Add(dr);
        }
        public static DataTable ConvertToDataTableForMsg(IBCMessage.Message[] msgOf)
        {
            DataTable dt = CreateDataTableForMsg();
            foreach (IBCMessage.Message msg in msgOf)
            {
                if (msg != null)
                {
                    FillDataForMsg(msg, dt);
                }
            }
            return dt;
        }
        public static DataTable ConvertToDataTableForMsg(List<IBCMessage.Message> msgOf)
        {
            DataTable dt = CreateDataTableForMsg();
            foreach (IBCMessage.Message msg in msgOf)
            {
                FillDataForMsg(msg, dt);
            }
            return dt;
        }
        /// <summary>
        /// Querry message directly from database
        /// </summary>
        /// <returns></returns>
        public static DataTable DirectGetTopMessage()
        {
            Hashtable htb = new Hashtable();
            htb.Add("messagetype", 3);
            htb.Add("site1", "3000");
            if(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId==4)
                htb.Add("site2", "3100");
            else if(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId==3)
                htb.Add("site2", "3200");
            else
                htb.Add("site2", "3300");
            htb.Add("site3", "");
            htb.Add("istest", false);
            DbResult result = null;
            //RESULT DATATABLE
            DataTable table = new DataTable();
            table.Columns.Add("content_en", typeof(string));
            table.Columns.Add("content_cs", typeof(string));
            table.Columns.Add("content_ch", typeof(string));
            table.Columns.Add("content_jp", typeof(string));
            table.Columns.Add("content_th", typeof(string));
            table.Columns.Add("content_it", typeof(string));
            table.Columns.Add("content_ko", typeof(string));
            table.Columns.Add("IsSticky", typeof(bool));

            Hashtable htb2 = new Hashtable();
            htb2.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.CustId);
            htb2.Add("istest", false);


            //PUBLIC MESSAGE
            result = DNAide.Data.DbHelper.Execute("Dot_Msg_GetTopPubMessage", htb);
            if (result != null && result.ResultSet.Tables.Count > 0 && result.ResultSet.Tables[0].Rows.Count > 0)
            {
                DataRow row = table.NewRow();
                row["content_en"] = result.ResultSet.Tables[0].Rows[0]["content_en"];
                row["content_cs"] = result.ResultSet.Tables[0].Rows[0]["content_cs"];
                row["content_ch"] = result.ResultSet.Tables[0].Rows[0]["content_ch"];
                row["content_jp"] = result.ResultSet.Tables[0].Rows[0]["content_jp"];
                row["content_th"] = result.ResultSet.Tables[0].Rows[0]["content_th"];
                row["content_it"] = result.ResultSet.Tables[0].Rows[0]["content_it"];
                row["content_ko"] = result.ResultSet.Tables[0].Rows[0]["content_ko"];
                row["IsSticky"] = result.ResultSet.Tables[0].Rows[0]["IsSticky"];
                table.Rows.Add(row);
            }
            //PRIVATE MESSAGE
            result = DNAide.Data.DbHelper.Execute("Dot_Msg_GetTopPriMessage24", htb2);
            if (result != null && result.ResultSet.Tables.Count > 0 && result.ResultSet.Tables[0].Rows.Count > 0)
            {
                DataRow row = table.NewRow();
                row["content_en"] = result.ResultSet.Tables[0].Rows[0]["content_en"];
                row["content_cs"] = result.ResultSet.Tables[0].Rows[0]["content_cs"];
                row["content_ch"] = result.ResultSet.Tables[0].Rows[0]["content_ch"];
                row["content_jp"] = result.ResultSet.Tables[0].Rows[0]["content_jp"];
                row["content_th"] = result.ResultSet.Tables[0].Rows[0]["content_th"];
                row["content_it"] = result.ResultSet.Tables[0].Rows[0]["content_it"];
                row["content_ko"] = result.ResultSet.Tables[0].Rows[0]["content_ko"];
                row["IsSticky"] = result.ResultSet.Tables[0].Rows[0]["IsSticky"];
                table.Rows.Add(row);
            }
                    

            return table;
            
        }


        #region Methods is built to use New design
        
        public static DataTable GetBalloonMessage2(int messageType, int siteid, int targetId)
        {
            DataTable returnTable = CreateDataTable();
            DataTable tmpTable = GetTopPublicMessage(messageType, siteid, 0, false);
            if (tmpTable.Rows.Count > 0)
            {
                CopyDataTable(tmpTable, returnTable);
            }
            tmpTable = GetTopPrivateMessage(targetId, 0, false);
            if (tmpTable.Rows.Count > 0)
            {
                CopyDataTable(tmpTable, returnTable);
            }

            return returnTable;
        }

        public static DataTable GetTopPublicMessage(int messageType, int siteid, int LatestPubMsgID, bool IsTest)
        {
            char[] separator = new char[] { ',' };
            string[] arraySite = GetParentSites1(siteid.ToString()).Split(separator);

            DbResult result = null;
            DataTable returnTable = CreateDataTable();
            Hashtable htb = new Hashtable();
            htb.Add("messagetype", messageType);
            htb.Add("site1", arraySite[0]);
            htb.Add("site2", (arraySite.Length > 1 ? arraySite[1] : string.Empty));
            htb.Add("site3", (arraySite.Length > 2 ? arraySite[2] : string.Empty));
            htb.Add("latestmsgid", LatestPubMsgID);
            htb.Add("istest", IsTest);

            result = DNAide.Data.DbHelper.Execute("Dot_Msg_GetTopPubMessage", htb);
            if (result != null && result.ResultSet.Tables.Count > 0 && result.ResultSet.Tables[0].Rows.Count > 0)
            {
                CopyDataTable(result.ResultSet.Tables[0], returnTable);
            }

            return returnTable;
        }

        public static DataTable GetTopPrivateMessage(int targetId, int LatestPriMsgID, bool IsTest)
        {
            DbResult result = null;
            DataTable returnTable = CreateDataTable();

            if (!HasPrivateMessage24(targetId, IsTest))
            {
                return returnTable;
            }

            Hashtable htb2 = new Hashtable();
            htb2.Add("userid", targetId);
            htb2.Add("latestmsgid", LatestPriMsgID);
            htb2.Add("istest", IsTest);

            result = DNAide.Data.DbHelper.Execute("Dot_Msg_GetTopPriMessage24", htb2);
            if (result != null && result.ResultSet.Tables.Count > 0 && result.ResultSet.Tables[0].Rows.Count > 0)
            {
                CopyDataTable(result.ResultSet.Tables[0], returnTable);
            }

            return returnTable;
        }

        public static DataTable GetMessage(int targetId, int siteid, int ispublic, int isspecial, int record, string startDate, string endDate, bool IsTest)
        {
            string str = targetId.ToString();
            string nSPname = "";
            string str3 = GetParentSites1(siteid.ToString());
            switch (ispublic)
            {
                case 0:
                    nSPname = "Dot_Msg_GetPrivateMessage";
                    break;

                case 1:
                    nSPname = "Dot_Msg_GetPublicMessage";
                    break;

                default:
                    switch (isspecial)
                    {
                        case 0:
                            nSPname = "Dot_Msg_GetNotSpecialMessage";
                            break;

                        case 1:
                            nSPname = "Dot_Msg_GetSpecialMessage";
                            break;

                    }
                    nSPname = (string.IsNullOrEmpty(nSPname) ? "Dot_Msg_GetMessage" : nSPname);
                    break;

            }

            DbResult result = null;
            DataTable returnTable = CreateDataTable();
            Hashtable htb = new Hashtable();
            htb.Add("targetid", str);
            htb.Add("targetid2", str3);
            htb.Add("special", isspecial);
            htb.Add("record", record);
            htb.Add("sdate", startDate);
            htb.Add("edate", endDate);
            htb.Add("istest", IsTest);

            try
            {
                result = DNAide.Data.DbHelper.Execute(nSPname, htb);
            }
            catch 
            {
                return returnTable;
                
            }
            if (result != null && result.ResultSet.Tables.Count > 0 && result.ResultSet.Tables[0].Rows.Count > 0)
            {
                CopyDataTable(result.ResultSet.Tables[0], returnTable);
            }

            return returnTable;
        }

        public static bool HasPrivateMessage24(int userId, bool isTest)
        {
            int msgId = 0;
            DbResult result = null;
            Hashtable htb = new Hashtable();
            htb.Add("userid", userId);
            htb.Add("istest", isTest);
            result = DNAide.Data.DbHelper.Execute("Dot_Msg_HasPriMessage24", htb);
            if (result != null && result.ResultSet.Tables.Count > 0)
            {
                msgId = CastAide.AsInt32(result.ResultSet.Tables[0].Rows[0]["MessageID"], 0);
            }

            return (msgId > 0 ? true : false);
        }

        #region Helper
        private static object GetValue(DataRow row, string fieldname)
        {
            object returnObj = null;
            if (row.Table.Columns.Contains(fieldname) && (!row.IsNull(fieldname)))
            {
                returnObj = row[fieldname];
            }
            return returnObj;
        }
        private static DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            DataColumn dc = null;

            for (int idxElement = 0; idxElement < colNameOfDataTable.Length; idxElement += 2)
            {
                dc = new DataColumn();
                dc.ColumnName = colNameOfDataTable[idxElement];
                dc.DataType = Type.GetType(colNameOfDataTable[idxElement + 1]);
                dt.Columns.Add(dc);
            }

            return dt;
        }
        private static void AddDataRow(DataRow row, DataTable dtMsg)
        {
            DataRow dr = dtMsg.NewRow();
            bool isBatch = CastAide.AsBoolean(GetValue(row, "IsBatch"), false);
            string bettransId = CastAide.AsString(GetValue(row, "BetTransID"), string.Empty);

            dr["Content_ch"] =
                (isBatch ? CastAide.AsString(GetValue(row, "Content_ch"), string.Empty).Replace("@@", bettransId) : CastAide.AsString(GetValue(row, "Content_ch"), string.Empty));
            dr["Content_cs"] =
                (isBatch ? CastAide.AsString(GetValue(row, "Content_cs"), string.Empty).Replace("@@", bettransId) : CastAide.AsString(GetValue(row, "Content_cs"), string.Empty));
            dr["Content_en"] =
                (isBatch ? CastAide.AsString(GetValue(row, "Content_en"), string.Empty).Replace("@@", bettransId) : CastAide.AsString(GetValue(row, "Content_en"), string.Empty));
            dr["Content_it"] =
                (isBatch ? CastAide.AsString(GetValue(row, "Content_it"), string.Empty).Replace("@@", bettransId) : CastAide.AsString(GetValue(row, "Content_it"), string.Empty));
            dr["Content_jp"] =
                (isBatch ? CastAide.AsString(GetValue(row, "Content_jp"), string.Empty).Replace("@@", bettransId) : CastAide.AsString(GetValue(row, "Content_jp"), string.Empty));
            dr["Content_ko"] =
                (isBatch ? CastAide.AsString(GetValue(row, "Content_ko"), string.Empty).Replace("@@", bettransId) : CastAide.AsString(GetValue(row, "Content_ko"), string.Empty));
            dr["Content_th"] =
                (isBatch ? CastAide.AsString(GetValue(row, "Content_th"), string.Empty).Replace("@@", bettransId) : CastAide.AsString(GetValue(row, "Content_th"), string.Empty));

            dr["CreateTime"] = CastAide.AsDateTime(GetValue(row, "CreateTime"));
            dr["CreatorId"] = CastAide.AsInt32(GetValue(row, "Creator")); 
            dr["CreatorName"] = CastAide.AsString(GetValue(row, "CreatorName"));
            dr["IsSticky"] = CastAide.AsBoolean(GetValue(row, "IsSticky")); 
            dr["LastUpdateName"] = CastAide.AsString(GetValue(row, "LastUpdateName")); 
            dr["LastUpdateTime"] = CastAide.AsDateTime(GetValue(row, "LastUpdateTime")); 
            dr["LastUpdator"] = CastAide.AsInt32(GetValue(row, "LastUpdator")); 
            dr["MessageId"] = CastAide.AsInt32(GetValue(row, "MessageId")); 
            dr["MessageType"] = CastAide.AsInt32(GetValue(row, "MsgType")); 
            //dr["Poster"] = ?;
            dr["PostTime"] = CastAide.AsDateTime(GetValue(row, "PostTime"));
            dr["UserId"] = CastAide.AsInt32(GetValue(row, "TargetId")); 
            dr["UserName"] = CastAide.AsInt32(GetValue(row, "UserName"));

            dtMsg.Rows.Add(dr);
        }
        private static string GetParentSites1(string SiteId)
        {
            ArrayList list = new ArrayList();
            int num = 0;
            for (int i = SiteId.Length - 1; i >= 0; i--)
            {
                if (SiteId[i] != '0')
                {
                    num = i;
                    break;
                }
            }
            for (int j = 1; j <= num; j++)
            {
                string str = "";
                for (int m = j; m < SiteId.Length; m++)
                {
                    str = str + "0";
                }
                list.Add(SiteId.Remove(j) + str);
            }
            string str2 = "";
            for (int k = 0; k < list.Count; k++)
            {
                str2 = str2 + list[k] + ",";
            }
            return (str2 + SiteId);
        }
        private static void CopyDataTable(DataTable srcTable, DataTable desTable)
        {
            foreach (DataRow row in srcTable.Rows)
            {
                AddDataRow(row, desTable);
            }
        }
        #endregion

        #endregion


    }

}