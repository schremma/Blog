using Blog.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace Blog.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Comment GetComment(int id)
        {
            return _context.Comments.Include(c => c.ApplicationUser).SingleOrDefault(c => c.Id == id);
        }

        public Comment GetCommentWithReplies(int id)
        {
            return _context.Comments.Include(c => c.Replies).SingleOrDefault(c => c.Id == id);
        }

        public Comment GetCommentWithPostAndParentComment(int id)
        {
            return _context.Comments.Include(c => c.Post).Include(c => c.ApplicationUser).Include(c => c.ParentComment).SingleOrDefault(c => c.Id == id);
        }

        public void Add(Comment comment)
        {
            _context.Comments.Add(comment);
        }

        public void Remove(Comment comment)
        {
            _context.Comments.Remove(comment);
        }

        public bool CommentExists(int id)
        {
            return _context.Comments.Any(c => c.Id == id);
        }


        public IEnumerable<Comment> GetCommentsWithPosts()
        {
            return _context.Comments
                .Include(c => c.Post)
                .Include(c => c.ApplicationUser)
                .Include(c => c.ParentComment);
        }

        public IEnumerable<Comment> GetCommentsForPost(int postId)
        {
            return _context.Comments
                .Include(c => c.Post)
                .Where(c => c.PostId == postId)
                .Include(c => c.ApplicationUser)
                .Include(c => c.ParentComment);
        }

        public IEnumerable<Comment> GetPendingCommentsForPost(int postId)
        {
            return _context.Comments
                .Include(c => c.Post)
                .Where(c => c.PostId == postId)
                .Include(c => c.ApplicationUser)
                .Include(c => c.ParentComment)
                .Where(c => c.Approved == false);
        }

        public IEnumerable<Comment> GetApprovedCommentsForPost(int postId)
        {
            return _context.Comments
                .Include(c => c.Post)
                .Where(c => c.PostId == postId)
                .Include(c => c.ApplicationUser)
                .Include(c => c.ParentComment)
                .Where(c => c.Approved == true);
        }
    }
}