// --------------------------------------------------------------------------
// <copyright file="Language.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.29">Created</change>
// </history>
namespace Apollo.Age.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Language
    {
        public const string English = "en_US";
        public const string TraditionChinese = "zh_TW";
        public const string SimplifiedChinese = "zh_GB";
        public const string Japanese = "ja_JP";
        public const string Korean = "ko_KR";
        public const string Vietnamese = "vm_VM";

        public string Name { get; set; }

        /// <summary>
        /// Override string.
        /// </summary>
        /// <returns>Return string.</returns>
        public override string ToString()
        {
            return base.ToString(); 
        }
    }
}
