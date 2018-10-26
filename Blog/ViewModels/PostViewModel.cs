using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.ViewModels
{
    public class PostViewModel
    {
        public Post Post { get; set; }
        public SelectList PostStatus { get; set; }
    }
}