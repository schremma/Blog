using System.Collections.Generic;
using Blog.Models;

namespace Blog.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        Post GetPost(int id);
        IEnumerable<Post> GetPosts();
        IEnumerable<Post> GetPublishedPosts();
        IEnumerable<Post> GetPublishedPostsFromTimeInterval(int year, int month = 0, int day = 0);
        bool PostExists(int id);
        void Remove(Post post);
    }
}