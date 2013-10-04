// --------------------------------------------------------------------------
// <copyright file="AgentUserAssignFunctionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.2">Created</change>
// </history>

namespace Apollo.Age.PermissionControl.Criterias
{
    using System.Collections.Generic;
    using Apollo.Age.PermissionControl.Core;
    using Apollo.Age.PermissionControl.Entities;
    using Apollo.Age.PermissionControl.Models;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get all function filter by function group and active.
    /// </summary>
    public class AgentUserAssignFunctionCriteria : CriteriaBase
    {
        protected readonly string AssignPermissionTpl = "<FunctionPolicy FunctionId='{0}' Policy='{1}'></FunctionPolicy>";

        public int ActorAgentId { get; set; }

        public int AgentUserId { get; set; }

        public string Permission
        {
            get
            {
                string result = "<ROOT>";
                string temp = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.FuncRead))
                {
                    string[] funcHost = this.FuncHost != null ? this.FuncHost.Split(',') : null;
                    string[] funcRead = this.FuncRead.Split(',');

                    foreach (var itemRead in funcRead)
                    {
                        if (!string.IsNullOrWhiteSpace(itemRead))
                        {
                            if (!ReferenceEquals(funcHost, null))
                            {
                                foreach (var itemHost in funcHost)
                                {
                                    if (string.Equals(itemRead, itemHost))
                                    {
                                        temp = string.Format(AssignPermissionTpl, itemRead, (int)Policy.Edit);

                                        break;
                                    }
                                }
                            }

                            if (string.IsNullOrWhiteSpace(temp))
                            {
                                temp = string.Format(AssignPermissionTpl, itemRead, (int)Policy.Query);
                            }

                            result += temp;
                            temp = string.Empty;
                        }
                    }
                }

                result += "</ROOT>";

                return result;
            }
        }

        [SpParam(Ignored = true)]
        public string FuncRead { get; set; }

        [SpParam(Ignored = true)]
        public string FuncHost { get; set; }

        [SpParam(Ignored = true)]
        public bool IsMaster { get; set; }

        [SpParam(Ignored = true)]
        public List<FunctionPolicy> ListFunctionPolicy { get; set; }

        [SpParam(Ignored = true)]
        public List<FunctionPolicy> ListParentFunctionPolicy { get; set; }

        [SpParam(Ignored = true)]
        public List<Function> ListFunction { get; set; }

        [SpParam(Ignored = true)]
        public bool HasEdit { get; set; }

        [SpParam(Ignored = true)]
        public string AgentUsername { get; set; }

        [SpParam(Ignored = true)]
        public string ShadowUsername { get; set; }
        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>Return Age_Function_GetAll.</returns>
        public override string GetSettingKey()
        {
            return "Age_AgentUser_AssignFunction";
        }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>Return true when function group id >= 0.</returns>
        public override bool IsValid()
        {
            return ActorAgentId > 0 && AgentUserId > 0;
        }
    }
}
