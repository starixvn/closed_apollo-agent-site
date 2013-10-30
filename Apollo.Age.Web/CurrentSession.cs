// --------------------------------------------------------------------------
// <copyright file="IUserSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.6">Created</change>
// </history>

namespace Apollo.Age.Web
{
    using System;
    using System.Runtime.Serialization;
    using Apollo.Age.Entities;

    /// <summary>
    ///  It contains all properties of an authenticated user
    ///  The implementation is applied when user config is state server mode
    /// </summary>
    [Serializable]
    public class CurrentSession : ICurrentSession, ISerializable
    {
        public CurrentSession(User user, int adminId = 0, string adminName = "", int onUserId = 0, bool isInternalSignIn = false, string language = "en-US")
        {
            AgentId = user.Agent.Id;
            AgentLevel = user.Agent.Level;
            AgentName = user.Agent.Username;
            UserId = user.Id;
            Username = user.Username;
            IsMaster = user.IsMaster;
            MatchLimit = user.Agent.MatchLimit;
            Language = language;
            CurrencyId = user.Agent.Currency;
            IsInternalSignIn = isInternalSignIn;
            AdminId = adminId;
            AdminName = adminName;
            OnUserId = onUserId;
        }

        public CurrentSession(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException("info");

            AgentId = info.GetInt32("AgentId");
            AgentLevel = info.GetInt16("AgentLevel");
            AgentName = info.GetString("AgentName");
            UserId = info.GetInt32("UserId");
            Username = info.GetString("Username");
            IsMaster = info.GetBoolean("IsMaster");
            CurrencyId = info.GetByte("CurrencyId");
            MatchLimit = info.GetDecimal("MatchLimit");
            VirtualMultiplier = info.GetDecimal("VirtualMultiplier");
            DoubleCommLosing = info.GetBoolean("DoubleCommLosing");
            Language = info.GetString("Language");
            IsInternalSignIn = info.GetBoolean("IsInternalSignIn");
            AdminId = info.GetInt32("AdminId");
            AdminName = info.GetString("AdminName");
            OnUserId = info.GetInt32("OnUserId"); 
        }

        public int AgentId { get; set; }

        public short AgentLevel { get; set; }

        public string AgentName { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public bool IsMaster { get; set; }

        public bool IsSuper
        {
            get { return AgentLevel == 0; }
        }

        public int CurrencyId { get; set; }

        public decimal MatchLimit { get; set; }

        public decimal VirtualMultiplier { get; set; }

        public bool DoubleCommLosing { get; set; }

        public string Language { get; set; }

        public bool IsInternalSignIn { get; set; }

        public int AdminId
        {
            get;
            set;
        }

        public string AdminName
        {
            get;
            set;
        }

        public int OnUserId
        {
            get;
            set;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException("info");

            info.AddValue("AgentId", AgentId);
            info.AddValue("AgentLevel", AgentLevel);
            info.AddValue("AgentName", AgentName);
            info.AddValue("UserId", UserId);
            info.AddValue("Username", Username);
            info.AddValue("IsMaster", IsMaster);
            info.AddValue("CurrencyId", CurrencyId);
            info.AddValue("MatchLimit", MatchLimit);
            info.AddValue("VirtualMultiplier", VirtualMultiplier);
            info.AddValue("DoubleCommLosing", DoubleCommLosing);
            info.AddValue("Language", Language);
            info.AddValue("IsInternalSignIn", IsInternalSignIn);
            info.AddValue("AdminId", AdminId);
            info.AddValue("AdminName", AdminName);
            info.AddValue("OnUserId", OnUserId);
        }
    }
}