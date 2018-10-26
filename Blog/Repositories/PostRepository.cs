using Blog.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace Blog.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Post post)
        {
            _context.Posts.Add(post);
        }

        public void Remove(Post post)
        {
            _context.Posts.Remove(post);
        }

        public Post GetPost(int id)
        {
            return _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Resources)
                .Include(p => p.Author)
                .SingleOrDefault(p => p.Id == id);
        }

        public bool PostExists(int id)
        {
            return _context.Posts.Any(p => p.Id == id);
        }

        public IEnumerable<Post> GetPosts()
        {
            return _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Author)
                .Include(p => p.Resources);
        }

        public IEnumerable<Post> GetPublishedPosts()
        {
            return _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Author)
                .Where(p => p.Status == PostStatus.published);
        }

        /// <summary>
        /// Returns posts at the specified time period.
        /// </summary>
        /// <param name="year">The year when the posts were published</param>
        /// <param name="month">if month = 0, all the posts in the year are returned, else posts from the given year and month</param>
        /// <param name="day">if day = 0, all the posts in the month are returned, else posts from a specific day</param>
        /// <returns></returns>
        public IEnumerable<Post> GetPublishedPostsFromTimeInterval(int year, int month = 0, int day = 0)
        {
            if (month == 0)
            {
                return _context.Posts.Include(p => p.Comments).Include(p => p.Author).Where(p => p.Status == PostStatus.published && p.PublishDate.Value.Year.Equals(year));
            }
            else if (day == 0)
            {
                return _context.Posts.Include(p => p.Comments).Include(p => p.Author).Where(p => p.Status == PostStatus.published && p.PublishDate.Value.Year.Equals(year) && p.PublishDate.Value.Month.Equals(month));
            }
            else
                return _context.Posts.Include(p => p.Comments).Include(p => p.Author).Where(p => p.Status == PostStatus.published && p.PublishDate.Value.Year.Equals(year) && p.PublishDate.Value.Month.Equals(month) && p.PublishDate.Value.Day.Equals(day));
        }
    }
}