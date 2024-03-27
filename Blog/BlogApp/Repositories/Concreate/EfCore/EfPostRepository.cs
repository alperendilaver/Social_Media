using BlogApp.Data;
using BlogApp.Models;
using BlogApp.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories.Concreate.EfCore
{
    public class EfPostRepository : IPostRepository
    {
        private BlogContext _context;
        public EfPostRepository(BlogContext blogContext)
        {
            _context = blogContext;
        }
        public IQueryable<Post> posts => _context.Posts;
        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(string userName)
        {
            return await _context.Posts.Where(x => x.UserId == userName).ToListAsync();

        }
        public void CreatePost(Post post)
        {
            post.user=null;
            _context.Add(post);
            _context.SaveChanges();
            
        }
        public void EditPost(PostEditViewModel model){
            var post = posts.FirstOrDefault(x=>x.PostId==model.PostId);
            post.Context = model.Context;
            post.image = model.image;
            post.Published = DateTime.Now;
            _context.SaveChanges();
        }
        public void DeletePost(PostEditViewModel model){
            var post=_context.Posts.FirstOrDefault(x=>x.PostId==model.PostId);
            if(post!=null){
                _context.Remove(post);
                _context.SaveChanges();
                if(post.image!=null){
                var imagePath=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",post.image);
                if(File.Exists(imagePath))
                    File.Delete(imagePath);
                }

            }
        }
        public void DeleteComment(Comment comment){
            _context.comments.Remove(comment);
            _context.SaveChanges();
        }

        public async Task AddReaction(string userId,int postId, int reactionId){
            var reactionType=new Reactions();
            if(reactionId == 0)
                reactionType = Reactions.like;
            if(reactionId == 1)
                reactionType = Reactions.unlike;
            if(reactionId == 2)
                reactionType = Reactions.angry;
            if(reactionId == 3)
                reactionType = Reactions.congr;
            var reaction = new Reaction{
                PostId=postId,
                UserId=userId,
                reaction = reactionType,
               
            };
            await _context.reactions.AddAsync(reaction);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Hata oluştuğunda yapılacak işlemler
                var mesaj = ex;
            }
           }
        public async Task RemoveReaction(Reaction reaction){
            _context.Remove(reaction);
            await _context.SaveChangesAsync();
        }
    }
}