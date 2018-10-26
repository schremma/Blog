using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Blog.Models;
using Blog.Repositories;
using Blog.ViewModels;
using Microsoft.AspNet.Identity;

namespace Blog.Controllers
{
    [Authorize(Roles="Admin")]
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /Post/
        public ActionResult Index()
        {
            var posts = _unitOfWork.Posts.GetPosts().OrderByDescending(p=> p.PublishDate).ThenByDescending(p => p.CreateDate);
            return View(posts);
        }

        // GET: /Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = _unitOfWork.Posts.GetPost(id.Value);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: /Post/Create
        public ActionResult Create()
        {
            var viewModel = new PostViewModel();
            viewModel.PostStatus = new SelectList(PostStatusList(), "Key", "Value", string.Empty);
            return View(viewModel);
        }

        // POST: /Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {

            if (ModelState.IsValid)
            {
                if (post.Status.Equals(PostStatus.published))
                {
                    post.PublishDate = DateTime.Now;
                }
                post.CreateDate = DateTime.Now;
                post.Author = _unitOfWork.Users.GetUser(User.Identity.GetUserId());

                _unitOfWork.Posts.Add(post);
                _unitOfWork.Complete();

                return RedirectToAction("Index");
            }

            var viewModel = new PostViewModel()
            {
                Post = post,
                PostStatus = new SelectList(PostStatusList(), "Key", "Value")
            };

            return View(viewModel);
        }

        // GET: /Post/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = _unitOfWork.Posts.GetPost(id.Value);
            if (post == null)
            {
                return HttpNotFound();
            }
            var viewModel = new PostViewModel()
            {
                Post = post,
                PostStatus = new SelectList(PostStatusList(), "Key", "Value"),
            };
            return View(viewModel);
        }

        // POST: /Post/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                Post originalPost = _unitOfWork.Posts.GetPost(post.Id);
                originalPost.Modify(post);

                _unitOfWork.Complete();

                return RedirectToAction("Index");
            }
            var viewModel = new PostViewModel()
            {
                Post = post,
                PostStatus = new SelectList(PostStatusList(), "Key", "Value")
            };
            return View(viewModel);
        }

        // GET: /Post/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = _unitOfWork.Posts.GetPost(id.Value);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: /Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = _unitOfWork.Posts.GetPost(id);
            if (post == null)
                return HttpNotFound();

            _unitOfWork.Posts.Remove(post);
            _unitOfWork.Complete();

            return RedirectToAction("Index");
        }

        // Creates a dictionary with number - name pairs based on the PostStatus enum
        private Dictionary<int, string> PostStatusList()
        {
            var statusList = new Dictionary<int, string>();
            foreach (var item in Enum.GetValues(typeof(PostStatus)))
            {
                statusList.Add((int)item, Enum.GetName(typeof(PostStatus), item));
            }

            return (statusList);
        }
    }
}
