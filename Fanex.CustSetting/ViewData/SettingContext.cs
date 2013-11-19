// --------------------------------------------------------------------------
// <copyright file="SettingContext.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.1">Created</change>
//   <change who="Ricky.Nguyen" date="2013.4.12">Update Simple Mode</change>
// </history>

namespace Fanex.CustSetting.ViewData
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustSetting.Core;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:EnumerationItemsMustBeDocumented", Justification = "Reviewed.")]
    public enum FormType : int
    {
        Add = 0,
        EditPT = 1,
        EditMemberNonDirect = 2,
        ViewAllSetting = 3,
        ShadowViewSetting = 4,
        EditMultiple = 5
    }

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:EnumerationItemsMustBeDocumented", Justification = "Reviewed.")]
    public enum Role : int
    {
        Super = 4,
        MainShareHolder = 3,
        Agent = 2,
        Member = 1,
    }

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:EnumerationItemsMustBeDocumented", Justification = "Reviewed.")]
    public enum FormSettingType : int
    {
        Commission = 1,
        BetLimit = 2,
        PositionTaking = 3,
        All = 4
    }

    /// <summary>
    /// Class name.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1609:PropertyDocumentationMustHaveValue", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed.")]
    public class SettingContext
    {
        public SettingContext()
        {
            Language = Constant.DEFAULT_LANGUAGE;
        }

        public SettingContext(int custId, int userId, int subAccId, int roleId, int targetRoleId, FormType formId)
        {
            CustId = custId;
            AdminId = userId;
            SubAccId = subAccId;
            RoleId = roleId;
            TargetRoleId = targetRoleId;
            FormId = formId;
            Language = Constant.DEFAULT_LANGUAGE;
        }

        /// <summary>
        /// Current login customer id or customer that is performing.
        /// </summary>
        public int CustId { get; set; }

        public int ActorCustId { get; set; }

        public int AgentLevel { get; set; }

        /// <summary>
        /// Current login customer user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Target customer id of edited customer.
        /// </summary>
        public int TargetCustId { get; set; }

        /// <summary>
        /// List target customer id for edit multiple.
        /// </summary>
        public string TargetCustIds { get; set; }

        /// <summary>
        /// Administrator id, using in Accounting or internal agent.
        /// </summary>
        public int AdminId { get; set; }

        /// <summary>
        /// Sub-Account id using in Agency.
        /// </summary>
        public int SubAccId { get; set; }

        /// <summary>
        /// Current role id of customer that is performing.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Currency id.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Site name.
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// Role id of edited customer.
        /// </summary>
        public int TargetRoleId { get; set; }

        /// <summary>
        /// Form type of edit: Edit Min/Max PT, Edit MinPT or Edit PT.
        /// </summary>
        public FormType FormId { get; set; }

        public int FormSetting { get; set; }

        /// <summary>
        /// IP of current request.
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// Language of current request.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// LangKey of current request.
        /// </summary>
        public string LangKey { get; set; }

        public bool EnableNumberGame { get; set; }

        public bool IsSimpleMode { get; set; }

        public bool HasMinConstrain { get; set; }

        public bool CanEditComGroup { get; set; }

        public bool IsUpdateMember { get; set; }

        public bool IsChangePositionTaking { get; set; }
    }
}