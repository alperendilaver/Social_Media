using BlogApp.Data;
using BlogApp.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Comment> GetComment(int comId)
        {
            return await _context.comments.Where(x=>x.CommentId==comId).FirstOrDefaultAsync()??new Comment();
        }

        public Task<List<Comment>> GetPostComments(int postId)
        {
            return _context.comments.Where(x=>x.postId==postId).Include(x=>x.user).Include(x=>x.post).ToListAsync();
        }
    }
}