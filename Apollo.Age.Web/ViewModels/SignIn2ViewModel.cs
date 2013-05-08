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
    using System.ComponentModel.DataAnnotations;

    public class SignIn2ViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Captcha")] 
        public string Captcha { get; set; }

        public bool RememberMe { get; set; }
    }
}