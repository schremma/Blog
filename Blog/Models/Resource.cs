using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    [Table("Resource", Schema="Blog")]
    public class Resource
    {
        public int Id { set; get; }

        //Attribute to avoid a length limit of 4000
        [MaxLength]
        public byte[] FileBytes { set; get; }
        public string ContentType { set; get; }
        public string FileName { set; get; }
        [Display(Name = "Short description"), StringLength(300, ErrorMessage = "Desciprion is maximum 300 caharcters long")]
        public string Description { set; get; }

        // Posts the resource has been added to
        public virtual ICollection<Post> Posts { get; set; }
    }

}