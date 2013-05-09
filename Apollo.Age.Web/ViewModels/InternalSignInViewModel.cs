// --------------------------------------------------------------------------
// <copyright file="SignIn2ViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.5.3">Created</change>
// </history>

namespace Apollo.Age.Web.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class InternalSignInViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Captcha { get; set; }

        public bool AllowUpdates { get; set; }

        public string Language { get; set; }

        public IEnumerable<SelectListItem> Languages
        {
            get
            {
                var langs = new List<SelectListItem>();

                langs.Add(new SelectListItem() { Text = "English", Value = "en-US", Selected = true });
                langs.Add(new SelectListItem() { Text = "繁體中文", Value = "zh-TW", Selected = false });
                langs.Add(new SelectListItem() { Text = "简体中文", Value = "zh-CN", Selected = false });
                langs.Add(new SelectListItem() { Text = "日本語", Value = "ja-JP", Selected = false });
                langs.Add(new SelectListItem() { Text = "ภาษาไทย", Value = "th-TH", Selected = false });
                langs.Add(new SelectListItem() { Text = "한국어", Value = "ko-KR", Selected = false });
                langs.Add(new SelectListItem() { Text = "Tiếng Việt", Value = "vi-VN", Selected = false });

                return langs;
            }
        }

        public bool RememberMe { get; set; }
    }
}