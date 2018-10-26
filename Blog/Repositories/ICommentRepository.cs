using System.Collections.Generic;
using Blog.Models;

namespace Blog.Repositories
{
    public interface ICommentRepository
    {
        void Add(Comment comment);
        bool CommentExists(int id);
        IEnumerable<Comment> GetApprovedCommentsForPost(int postId);
        Comment GetComment(int id);
        IEnumerable<Comment> GetCommentsForPost(int postId);
        IEnumerable<Comment> GetCommentsWithPosts();
        Comment GetCommentWithPostAndParentComment(int id);
        Comment GetCommentWithReplies(int id);
        IEnumerable<Comment> GetPendingCommentsForPost(int postId);
        void Remove(Comment comment);
    }
}