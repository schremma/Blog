using Blog.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace Blog.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly ApplicationDbContext _context;

        public ResourceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Resource resource)
        {
            _context.Resources.Add(resource);
        }

        public void Remove(Resource resource)
        {
            _context.Resources.Remove(resource);
        }

        public Resource GetResource(int id)
        {
            return _context.Resources.SingleOrDefault(r => r.Id == id);
        }

        public IEnumerable<Resource> GetResources()
        {
            return _context.Resources;
        }

        public IEnumerable<Resource> GetResourcesForPost(int postId)
        {
            return _context.Resources.Where(r => r.Posts.Any(p => p.Id == postId));
        }

        public IEnumerable<Resource> GetResourcesWithPosts()
        {
            return _context.Resources.Include(r=> r.Posts);
        }

        public IEnumerable<Resource> GetResourcesWithIds(IEnumerable<int> ids)
        {
            return _context.Resources.Where(r => ids.Contains(r.Id)).ToList();
        }
    }
}