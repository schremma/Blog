using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Models
{
    [Table("Post", Schema = "Blog")]
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150, ErrorMessage="Titel is maximum 150 caharcters long")]
        public string Titel { get; set; }

        [Required, DataType(DataType.MultilineText), AllowHtml]
        public string Content { get; set; }

        [Display(Name = "Created"), DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Last modified date")]
        public DateTime ?ModifyDate { get; set; }

        [Display(Name ="Published")]
        public DateTime? PublishDate { get; set; }

        public PostStatus Status { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Resource> Resources { get; set; }

        public void Modify(Post updatedPost)
        {
            ModifyDate = DateTime.Now;
            Titel = updatedPost.Titel;
            Content = updatedPost.Content;
            Status = updatedPost.Status;

            if (Status.Equals(PostStatus.published) && PublishDate == null)
                PublishDate = DateTime.Now;
            else if (Status.Equals(PostStatus.draft) && PublishDate != null)
                PublishDate = null;
        }
    }

    public enum PostStatus
    {
        draft,
        published
    }
}