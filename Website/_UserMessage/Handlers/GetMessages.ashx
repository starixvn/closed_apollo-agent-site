<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._UserMessage.GetMessages_Handler" %>

using System;
using System.Web;
using System.Data;
using System.Configuration;

using Sunplus.Agent.Model.BL.UserMessage;
using Sunplus.Agent.Model.BL.Definitions;

using Sunplus.Agent.Model.Entities;

using Sunplus.Shared.ViewLogLib;
using Sunplus.Shared.ViewLogLib.Services;
using Sunplus.Shared.ViewLogLib.Entities;

using Fanex.Caching;
using CasinoSetting;
using Sunplus.Agent.Model.BL.Bingo;
using Sunplus.Agent.Model.BL.Casino;

/*
 * Created Don@20100616 - Handler class of user message function: high-light message, balloon message
 * Revision:
 *
 *
 */

namespace Sunplus.Agent.UI._UserMessage
{
    using System.Web.SessionState;

    public class GetMessages_Handler : Fanex.Age.UI.TplHandler, IReadOnlySessionState
    {
        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            object[] publicMessages = new object[3];
            object[] privateMessages = new object[2];

            object[] dataView = View as object[];
            DataTable messages = dataView[0] as DataTable;
            if ((messages != null) && (messages.Rows.Count > 0))
            {
                // Public - 0:message ID, 1:message content, 2:message type (normall/allSpecial)
                var publicMessageContent = UserMessageBO.GetContentMsg(messages.Rows[0], CurrentUserSession.Language);
                publicMessageContent = publicMessageContent.Replace("\n", string.Empty);
                publicMessageContent = publicMessageContent.Replace("\r", string.Empty);
                publicMessageContent = publicMessageContent.Replace("'", "\'");

                publicMessages[0] = messages.Rows[0]["MessageId"];
                publicMessages[1] = publicMessageContent;
                publicMessages[2] = "normal";

                if ((DBNull.Value != messages.Rows[0]["IsSticky"]) && ((bool)messages.Rows[0]["IsSticky"]))
                {
                    publicMessages[2] = "allSpecial";
                }

                // Private - 0:message ID, 1:message content
                if (messages.Rows.Count == 2)
                {
                    var privateMessageContent = UserMessageBO.GetContentMsg(messages.Rows[1], CurrentUserSession.Language);
                    privateMessageContent = privateMessageContent.Replace("\n", string.Empty);
                    privateMessageContent = privateMessageContent.Replace("\r", string.Empty);
                    privateMessageContent = privateMessageContent.Replace("'", "\'");

                    privateMessages[0] = messages.Rows[1]["MessageId"];
                    privateMessages[1] = privateMessageContent;
                }
            }

            data.Add("public_message", publicMessages);
            data.Add("private_message", privateMessages);
            data.Add("unread_private_messages", (int)dataView[1]);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            InitializeCaching();
        }

        void GetMessages_Handler_LoadComplete(object sender, EventArgs e)
        {

        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            DataTable messages = null;
            UserMessageBO userMessageBO = new UserMessageBO(CurrentUserSession.NetSessionId);
            int siteId = 3000;
            siteId = RoleToSiteId(ref siteId);

            // Get all public messages and private messages
            messages = userMessageBO.GetMessages(3, siteId);
            int count = userMessageBO.CountUnreadPriMessage();

            return new object[] { messages, count };
        }

        private int RoleToSiteId(ref int siteId)
        {
            if (CurrentUserSession.RoleId == (int)UserTypes.Super)
            {
                siteId = 3100;
            }
            else if (CurrentUserSession.RoleId == (int)UserTypes.Master)
            {
                siteId = 3200;
            }
            else if (CurrentUserSession.RoleId == (int)UserTypes.Agent)
            {
                siteId = 3300;
            }
            return siteId;
        }

        /// <summary>
        /// Initialize all configurations and caching one time only.
        /// </summary>
        public void InitializeCaching()
        {
            if (!initialized)
            {
                lock (locker)
                {
                    if (!initialized)
                    {
                        // View log cache
                        ICache cache = CacheService.CreateInstance(CacheType.MemoryCache);
                        cache.Remove(Constants.CacheKeyTransformObject);

                        // Casino caching
                        try
                        {
                            SettingMgr.GetInstance().Initialize(SiteEnum.AgentSite);
                        }
                        catch { }

                        BingoBO bingo = new BingoBO(CurrentUserSession.NetSessionId); // APILogger injection
                        CasinoBO casino = new CasinoBO(CurrentUserSession.NetSessionId); // APILogger injection

                        // Init one time only
                        initialized = true;
                    }
                }
            }
        }

        public static object locker = new object();
        public static bool initialized = false;
    }

}