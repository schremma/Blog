using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Net;
using Blog.Repositories;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Returns all the published posts in descending order based on publish date.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var posts = _unitOfWork.Posts.GetPublishedPosts().ToList();
            return View(posts.OrderByDescending(p => p.PublishDate));
        }

        public ActionResult PostsAt(int year, int month, int day)
        {
            if (year == 0)
                return HttpNotFound();

            var posts = _unitOfWork.Posts.GetPublishedPostsFromTimeInterval(year, month, day);

            return View("Index", posts);
            
        }

        // Returns a list of the 5 most-commented published posts, starting with the most popular.
        public ActionResult TopList()
        {
            var posts = _unitOfWork.Posts.GetPublishedPosts().ToList();

            return PartialView(posts.OrderByDescending(p => p.Comments.Count()).Take(5));
        }

        // Returns a post with the specified id, only if it is published.
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = _unitOfWork.Posts.GetPost(id.Value);
            if (post == null || post.Status != PostStatus.published)
            {
                return HttpNotFound();
            }

            return View(post);
        }



        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}