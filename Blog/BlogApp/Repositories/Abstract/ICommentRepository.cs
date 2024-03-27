using BlogApp.Data;

namespace BlogApp.Repositories.Abstract
{
    public interface ICommentRepository{
        IQueryable<Comment> comments {get;}
        void createComment(Comment comment);
        
    }
}