using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Blog.Models;
using Blog.Repositories;
using Blog.ViewModels;
using Microsoft.AspNet.Identity;

namespace Blog.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // GET: /Comment/
        // Returns comments based on the provided parameters:
        // no parameters: all comments in the database
        // postId: id of the post to list the comments for
        // pending: true, if only comments that have not been approved are to be returned
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int? postId, bool pending = false)
        {
            if (postId == null)
            {
                var comments = _unitOfWork.Comments.GetCommentsWithPosts().OrderByDescending(c => c.CreateDate);

                if (comments == null)
                    return HttpNotFound();

                return View(comments.ToList());
            }

            IEnumerable<Comment> commentsForPost = null;
            if (pending == false)
                commentsForPost = _unitOfWork.Comments.GetCommentsForPost(postId.Value).OrderByDescending(c => c.CreateDate);
            else
                commentsForPost = _unitOfWork.Comments.GetPendingCommentsForPost(postId.Value).OrderByDescending(c => c.CreateDate);

            if (commentsForPost == null)
                return HttpNotFound();

            return View(commentsForPost.ToList());

        }

        // Returns a list with only approved comments that belong to the specified post
        // In descending order of date of creation
        [AllowAnonymous]
        public ActionResult CommentsForPost(int? postId)
        {
            if (postId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var comments = _unitOfWork.Comments.GetApprovedCommentsForPost(postId.Value);
            if (comments == null)
                return HttpNotFound();

            return PartialView(comments.OrderByDescending(c => c.CreateDate));
        }


         [Authorize(Roles="Admin")]
        // GET: /Comment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Comment comment = _unitOfWork.Comments.GetComment(id.Value);
            if (comment == null)
                return HttpNotFound();
            
            return View(comment);
        }


        /// <summary>
        ///GET: /Comment/Create
        /// Returns a partial view for creating comments if the user is authenticated.
        /// Sets the user's email as the default value for the email associated with the comment.
        /// If the id of the parent comment is not null, sets a message
        /// that displays user name and date for the comment the reply is sent to.
        /// If the user is not authenticated, a partial view is returned for logging in.
        /// </summary>
        /// <param name="parentId">Id if the comment the new comment is a reply to </param>
        /// <param name="postId">The id of the post to be commented</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Create(int? parentId, int postId, string requestUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = _unitOfWork.Users.GetUser(User.Identity.GetUserId());
                if (user == null)
                    return HttpNotFound();
                Comment comment = new Comment() { Email = user.Email, PostId = postId };
                CommentViewModel viewModel = new CommentViewModel();

                if (parentId != null)
                {
                    Comment parentComment = _unitOfWork.Comments.GetComment(parentId.Value);
                    if (parentComment != null)
                    {
                        viewModel.ResponseToMessage = string.Format("Reply to: {0} {1} ", parentComment.ApplicationUser.UserName, parentComment.CreateDate);
                        comment.ParentCommentId = parentComment.Id;
                    }
                    else { viewModel.ResponseToMessage = ""; }
                }
                viewModel.Comment = comment;
                return PartialView(viewModel);
            }

            ViewBag.RequestUrl = requestUrl ?? Request.RawUrl;
           
            return PartialView("LogIn");
        }


        // POST: /Comment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.ApplicationUserId = User.Identity.GetUserId();
                comment.CreateDate = DateTime.Now;
                if (!_unitOfWork.Posts.PostExists(comment.PostId) 
                    || (comment.ParentCommentId !=null && !_unitOfWork.Comments.CommentExists(comment.ParentCommentId.Value)))
                {
                    return HttpNotFound();
                }
                _unitOfWork.Comments.Add(comment);
                _unitOfWork.Complete();

                ValueProviderResult commentContent = new ValueProviderResult("", "", System.Globalization.CultureInfo.CurrentCulture);
                ModelState.SetModelValue("comment.Content", commentContent);
                var viewModel = new CommentViewModel()
                {
                    Comment = new Comment(),
                    CommentStatusMesssage = "Your comment has been registered and will appear after approval from the admin"
                };
                return PartialView(viewModel);
            }

            var returnViewModel = new CommentViewModel()
            {
                Comment = comment,
                CommentStatusMesssage = "Your comment has not been registered"
            };
            return PartialView(returnViewModel);
        }

        /// <summary>
        /// Changes the approved attribute of the comment: from true to false or from false to true.
        /// </summary>
        /// <param name="id">Comment id</param>
        /// <returns>A partial view with the updated comment.</returns>
        [HttpPost, Authorize(Roles="Admin")]
        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = _unitOfWork.Comments.GetCommentWithPostAndParentComment(id.Value);
            if (comment != null)
            {
                if (comment.Approved == true)
                    comment.Approved = false;
                else
                    comment.Approved = true;

                _unitOfWork.Complete();
            }
            return PartialView(comment);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            var comments = _unitOfWork.Comments.GetCommentsWithPosts().OrderByDescending(c => c.CreateDate);
            return PartialView(comments.ToList());
        }

 
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Comment comment = _unitOfWork.Comments.GetComment(id.Value);
            if (comment == null)
                return HttpNotFound();
            
            return View(comment);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            List<Comment> replies = new List<Comment>();
            CascadeComments(id, replies);

            Comment comment = _unitOfWork.Comments.GetComment(id);
            replies.Add(comment);

            for (int i = 0; i < replies.Count; i++)
            {
                _unitOfWork.Comments.Remove(replies[i]);
            }
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Fills a list with all the replies to a comment.
        /// </summary>
        /// <param name="id">The id of the comment for which the replies are to be collected</param>
        /// <param name="replies">The list with reply comments</param>
        private void CascadeComments(int id, List<Comment> replies)
        {
            Comment comment = _unitOfWork.Comments.GetCommentWithReplies(id);
            foreach (var reply in comment.Replies)
            {
                if (reply.Replies.Count() > 0)
                {
                    CascadeComments(reply.Id, replies);
                }
                replies.Add(reply);
            }
        }

    }
}
