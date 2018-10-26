using Blog.Models;

namespace Blog.Repositories
{
    public interface IUserRepository
    {
        ApplicationUser GetUser(string userId);
    }
}