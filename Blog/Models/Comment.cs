using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    [Table("Comment", Schema = "Blog")]
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }

        // The user who has left the comment
        public string ApplicationUserId { get; set; }

        // not null if comment is a reply to another comment
        public int? ParentCommentId { get; set; }

        [Required, DataType(DataType.MultilineText)]
        [Display(Name="Comment")]
        public string Content { get; set; }

        [Display(Name="Created"), DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }

        public bool Approved { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public virtual Post Post { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public Comment ParentComment { get; set; }
        public virtual ICollection<Comment> Replies { get; set; }

    }

}