// --------------------------------------------------------------------------
// <copyright file="AgentUserAssignFunctionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.2">Created</change>
// </history>

namespace Apollo.Age.Criterias
{
    using System.Collections.Generic;
    using Apollo.Age.Core;
    using Apollo.Age.Entities;
    using Fanex.NAP.Framework.Data;
    using Apollo.Age.Models;

    /// <summary>
    /// Get all function filter by function group and active.
    /// </summary>
    public class UserAssignFunctionCriteria : CriteriaBase
    {
        protected readonly string AssignPermissionTpl = "<FunctionPolicy FunctionId='{0}' Policy='{1}'></FunctionPolicy>";

        public int ActorAgentId { get; set; }

        public int AdminId { get; set; }

        public int AgentUserId { get; set; }

        public string Permission
        {
            get
            {
                string result = "<ROOT>";
                string temp = string.Empty;
                var newPermissionDictionary = new Dictionary<string, byte>();
                var oldPermissionDictionary = new Dictionary<string, byte>();

                // Get new permission
                if (!string.IsNullOrWhiteSpace(this.FuncRead))
                {
                    string[] funcHost = this.FuncHost != null ? this.FuncHost.Split(',') : null;
                    string[] funcRead = this.FuncRead.Split(',');

                    foreach (var itemRead in funcRead)
                    {
                        if (!string.IsNullOrWhiteSpace(itemRead)
                            && !newPermissionDictionary.ContainsKey(itemRead))
                        {
                            newPermissionDictionary.Add(itemRead, (byte)Policy.Query);
                        }
                    }

                    foreach (var itemHost in funcHost)
                    {
                        if (!string.IsNullOrWhiteSpace(itemHost)
                            && newPermissionDictionary.ContainsKey(itemHost))
                        {
                            newPermissionDictionary[itemHost] = (byte)Policy.Edit;
                        }
                    }
                }

                // Get old permission of user in DB
                if (!ReferenceEquals(ListFunctionPolicy, null))
                {
                    foreach (var itemFunctionPolicy in ListFunctionPolicy)
                    {
                        if (!oldPermissionDictionary.ContainsKey(itemFunctionPolicy.Function.Id.ToString()))
                        {
                            oldPermissionDictionary.Add(itemFunctionPolicy.Function.Id.ToString(), itemFunctionPolicy.Policy);
                        }
                    }
                }

                // Get only permission is changed
                foreach (var itemFunctionPolicy in oldPermissionDictionary)
                {
                    // Get permission change policy
                    if (newPermissionDictionary.ContainsKey(itemFunctionPolicy.Key))
                    {
                        if (newPermissionDictionary[itemFunctionPolicy.Key] != itemFunctionPolicy.Value)
                        {
                            result += string.Format(AssignPermissionTpl, itemFunctionPolicy.Key, newPermissionDictionary[itemFunctionPolicy.Key]);
                        }
                    }
                    else
                    {
                        if (itemFunctionPolicy.Value != (byte)Policy.None)
                        {
                            result += string.Format(AssignPermissionTpl, itemFunctionPolicy.Key, (byte)Policy.None);
                        }
                    }
                }

                result += "</ROOT>";

                return result;
            }
        }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

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

        [SpParam(Ignored = true)]
        public int AgentId { get; set; }

        [SpParam(Ignored = true)]
        public int ParentAgentId { get; set; }

        [SpParam(Ignored = true)]
        public bool IsDirectAgent { get; set; }

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
