// --------------------------------------------------------------------------
// <copyright file="SafeCodeViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.7.17">Created</change>
// </history>

namespace Apollo.Age.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class SafeCodeViewModel
    {
        [Required]
        [Display(Name = "Security Code")]
        public string SecurityCode { get; set; }

        public string Language { get; set; }

        public string AdminName { get; set; }

        public int AdminId { get; set; }

        public string Agent { get; set; }
    }
}