using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using Blog.Repositories;
using Blog.ViewModels;

namespace Blog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ResourceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ResourceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var resources = _unitOfWork.Resources.GetResources();
            return View(resources);
        }

        public ActionResult Upload()
        {
            return View();
        }

        // Saves an uploaded file to the database, with its name and content-type.
        // Returns a Json object with the result (success or failure)
        [HttpPost]
        public JsonResult UploadFile(HttpPostedFileBase qqfile)
        {
            HttpPostedFileBase file = Request.Files[0];
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName) &&  !string.IsNullOrEmpty(file.ContentType))
            {
                string fileName = file.FileName;
                string fileContentType = file.ContentType;
                byte[] fileBytes = new byte[file.ContentLength];
                file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                Resource resource = new Resource()
                {
                    FileName = fileName,
                    ContentType = fileContentType,
                    FileBytes = fileBytes
                };
                _unitOfWork.Resources.Add(resource);
                _unitOfWork.Complete();
                return Json(new { success = true }, "application/json");
            }
            return Json(new { success = false }, "application/json");
        }

        // Returns the specified resource for download
        [AllowAnonymous]
        public ActionResult GetFile(int id)
        {
            var resource = _unitOfWork.Resources.GetResource(id);
            if (resource != null)
            {
                byte[] data;

                if (resource != null)
                {
                    data = (byte[])resource.FileBytes.ToArray();
                    return File(data, resource.ContentType, resource.FileName);
                }

                return File(new byte[0], string.Empty, string.Empty);
            }
            return new HttpNotFoundResult();
        }

        // Lists the resources for a specified, published post
        [AllowAnonymous]
        public ActionResult List(int id)
        {
            Post post = _unitOfWork.Posts.GetPost(id);
            if (post != null && post.Status == PostStatus.published)
            {
                List<Resource> resources = _unitOfWork.Resources.GetResourcesForPost(id).ToList();
                return PartialView(resources);
            }
            return PartialView(new List<Resource>());
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Resource resource = _unitOfWork.Resources.GetResource(id.Value);
            if (resource == null)
                return HttpNotFound();
            
            return View(resource);
        }

        // Edits description for a resource
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Resource resource)
        {
            var res = _unitOfWork.Resources.GetResource(resource.Id);
            if (ModelState.IsValid)
            {
                if (res != null)
                {
                    res.Description = resource.Description;
                    _unitOfWork.Complete();
                    return RedirectToAction("Index");
                }
            }

            return View(res);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Resource resource = _unitOfWork.Resources.GetResource(id.Value);
            if (resource == null)
                return HttpNotFound();
            
            return View(resource);
        }

        // POST: /Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Resource resource = _unitOfWork.Resources.GetResource(id);
            _unitOfWork.Resources.Remove(resource);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        // Returns view for associating resources with posts
        public ActionResult ToPost(int postId)
        {
            var resources = _unitOfWork.Resources.GetResourcesWithPosts();
            var viewModel = new ResourcesToPostViewModel()
            {
                PostId = postId,
                Resources = resources
            };
            return View(viewModel);
        }

        // Associate a post with the resouces that the user has chosen to add.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ToPost()
        {
            int postId = 0;
            if (Request["postId"] != null)
            {
                if (int.TryParse(Request["postId"].ToString(), out postId))
                {
                    var post = _unitOfWork.Posts.GetPost(postId);

                    if (post != null)
                    {
                        if (Request["add"] != null)
                        {
                            List<int> ids = new List<int>();
                            int id = 0;
                            foreach (var selection in Request["add"].Split(','))
                            {
                                if (int.TryParse(selection, out id))
                                {
                                    ids.Add(id);
                                }
                            }
                            var resources = _unitOfWork.Resources.GetResourcesWithIds(ids).ToList();
                            foreach (var item in post.Resources)
                                resources.Add(item);
                            post.Resources = resources.ToList();
                            _unitOfWork.Complete();
                            ViewBag.Message = "Resource has been added to post!";
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Error adding post";
                    }
                }
            }

            var resourcesBack = _unitOfWork.Resources.GetResourcesWithPosts();
            var viewModel = new ResourcesToPostViewModel()
            {
                PostId = postId,
                Resources = resourcesBack.ToList()
            };
            return View(viewModel);
        }

        public ActionResult RemoveFromPost(int id, int postId)
        {
            var resource = _unitOfWork.Resources.GetResource(id);
            if (resource == null)
                return HttpNotFound();

            var viewModel = new ResourceOfPostViewModel()
            {
                Resource = resource,
                PostId = postId
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFromPost()
        {
            if (Request["id"] != null && Request["postId"] != null )
            {
                int id = 0;
                int postId = 0;
                if (int.TryParse(Request["id"].ToString(), out id) && int.TryParse(Request["postId"].ToString(), out postId))
                {

                    var post = _unitOfWork.Posts.GetPost(postId);
                    var resource = _unitOfWork.Resources.GetResource(id);
                    if (post != null)
                    {
                        post.Resources.Remove(resource);
                        _unitOfWork.Complete();
                    }
                    return RedirectToAction("ToPost", new {postId});
                }
            }

             return HttpNotFound();
        }

    }
}