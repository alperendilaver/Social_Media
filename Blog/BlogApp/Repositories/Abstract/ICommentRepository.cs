using BlogApp.Data;

namespace BlogApp.Repositories.Abstract
{
    public interface ICommentRepository{
        IQueryable<Comment> comments {get;}
        void createComment(Comment comment);
        Task<List<Comment>> GetPostComments(int postId);
        Task<Comment> GetComment(int comId);
    }
}