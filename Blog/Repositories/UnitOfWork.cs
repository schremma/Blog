using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Posts = new PostRepository(_context);
            Comments = new CommentRepository(_context);
            Users = new UserRepository(_context);
            Resources = new ResourceRepository(_context);
        }

        public IPostRepository Posts { get; private set; }
        public ICommentRepository Comments { get; private set; }
        public IUserRepository Users { get; private set; }
        public IResourceRepository Resources { get; private set; }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}