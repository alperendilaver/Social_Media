using BlogApp.Data;
using BlogApp.Repositories.Abstract;

namespace BlogApp.Repositories.Concreate.EfCore
{
    public class EfCommentRepository : ICommentRepository
    {
        private BlogContext _context;
        public EfCommentRepository(BlogContext context)
        {
            _context=context;
        }
        public IQueryable<Comment> comments => _context.comments;
        public void createComment(Comment comment)
        {   
            _context.Add(comment);
            _context.SaveChanges();
        }
    }
}