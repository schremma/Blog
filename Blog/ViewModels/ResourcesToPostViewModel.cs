using Blog.Models;
using System.Collections.Generic;

namespace Blog.ViewModels
{
    public class ResourcesToPostViewModel
    {
        public int PostId { get; set; }
        public IEnumerable<Resource> Resources { get; set; }
    }
}