using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.ViewModels
{
    public class ResourceOfPostViewModel
    {
        public Resource Resource { get; set; }
        public int PostId { get; set; }
    }
}