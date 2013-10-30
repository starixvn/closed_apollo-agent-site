// --------------------------------------------------------------------------
// <copyright file="SignIn2ViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.5.8">Created</change>
//   <change who="Terry.Nguyen" date="2013.7.18">Add AllowUpdate and SafeCode</change>
// </history>

namespace Apollo.Age.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class SelectAccountViewModel
    {
        [Required]
        public string Agent { get; set; }

        [Display(Name = "Security Code")]
        public string SafeCode { get; set; }

        public string Language { get; set; }

        public string AdminName { get; set; }

        public int AdminId { get; set; }
    }
}