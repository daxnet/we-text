using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeText.Web.Models
{
    public class TextViewModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }


        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime DateCreated { get; set; }
    }
}