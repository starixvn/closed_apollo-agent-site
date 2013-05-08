// --------------------------------------------------------------------------
// <copyright file="SignIn2ViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.5.8">Created</change>
// </history>

namespace Apollo.Age.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class InternalSelectAccountViewModel
    {
        [Required]
        public string Agent { get; set; }
        
        public bool RememberMe { get; set; }
    }
}