using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WeText.Web.Models
{
    public class EditTextViewModel
    {
        public string TextId { get; set; }

        [Required]
        [StringLength(32)]
        public string Title { get; set; }

        [Required]
        [StringLength(128)]
        public string Content { get; set; }
    }
}