using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.ViewModels
{
    public class CommentViewModel
    {
        public Comment Comment { get; set; }
        public string ResponseToMessage { get; set; }
        public string CommentStatusMesssage { get; set; }

        public CommentViewModel()
        {
            ResponseToMessage = string.Empty;
            CommentStatusMesssage = string.Empty;
        }
    }
}