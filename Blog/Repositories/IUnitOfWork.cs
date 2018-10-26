namespace Blog.Repositories
{
    public interface IUnitOfWork
    {
        ICommentRepository Comments { get; }
        IPostRepository Posts { get; }
        IResourceRepository Resources { get; }
        IUserRepository Users { get; }

        void Complete();
    }
}