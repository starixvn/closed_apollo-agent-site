// --------------------------------------------------------------------------
// <copyright file="SignInViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.5.3">Created</change>
// </history>


using System.ComponentModel.DataAnnotations;
namespace Apollo.Age.Web.ViewModels
{
    public class SignInViewModel
    {
        [Required]
        public string Username { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string Message { get; set; }
    }
}