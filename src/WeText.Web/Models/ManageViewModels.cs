using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace WeText.Web.Models
{
    public class IndexViewModel
    {
        [Required]
        [Display(Name = "Display Name:")]
        [StringLength(16, MinimumLength = 4)]
        public string DisplayName { get; set; }

        [Required]
        [Display(Name = "Email:")]
        [EmailAddress]
        public string Email { get; set; }
    }
}