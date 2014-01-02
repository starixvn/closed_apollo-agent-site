// --------------------------------------------------------------------------
// <copyright file="SignInViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.5.3">Created</change>
// </history>

namespace Apollo.Age.Web.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// Sign in view model.
    /// </summary>
    public class SignInViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Captcha { get; set; }

        public string Language { get; set; }

        public List<SelectListItem> Languages
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = "English", Value = "en-US" },
                    new SelectListItem { Text = "繁體中文", Value = "zh-TW" },
                    new SelectListItem { Text = "简体中文", Value = "zh-CN" },
                    new SelectListItem { Text = "日本語", Value = "ja-JP" },
                    new SelectListItem { Text = "한국어", Value = "ko-KR" },
                    new SelectListItem { Text = "ภาษาไทย", Value = "th-TH" },
                    new SelectListItem { Text = "Tiếng Việt", Value = "vi-VN" }
                };
            }
        }

        public bool RememberMe { get; set; }
    }
}