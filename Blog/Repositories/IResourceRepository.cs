using System.Collections.Generic;
using Blog.Models;

namespace Blog.Repositories
{
    public interface IResourceRepository
    {
        void Add(Resource resource);
        Resource GetResource(int id);
        IEnumerable<Resource> GetResources();
        IEnumerable<Resource> GetResourcesForPost(int postId);
        IEnumerable<Resource> GetResourcesWithIds(IEnumerable<int> ids);
        IEnumerable<Resource> GetResourcesWithPosts();
        void Remove(Resource resource);
    }
}