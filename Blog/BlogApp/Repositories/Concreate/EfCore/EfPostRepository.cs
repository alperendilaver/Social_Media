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
        public async Task<int> CreatePost(Post post)
        {
            _context.Add(post);
            return await _context.SaveChangesAsync();
            
        }
        public async Task<int> EditPost(PostEditViewModel model){
            var post = posts.FirstOrDefault(x=>x.PostId==model.PostId);
            if(post==null)
                return 0;
            post.Context = model.Context;
            post.image = model.image;
            post.Published = DateTime.Now;
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeletePost(PostEditViewModel model){
            var post=_context.Posts.FirstOrDefault(x=>x.PostId==model.PostId);
            if(post!=null){
                _context.Remove(post);
                if(post.image!=null){
                var imagePath=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",post.image);
                if(File.Exists(imagePath))
                    File.Delete(imagePath);
                }

                return await _context.SaveChangesAsync();
            }
            return 0;
        }
        public async Task<int> DeleteComment(Comment comment){
            _context.comments.Remove(comment);
            return await _context.SaveChangesAsync();
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

        public async Task<List<Reaction>> GetReactions(int postId)
        {
            return await  _context.reactions.Where(x=>x.PostId==postId).ToListAsync();
        }

        public async Task<Reaction> GetReaction(string userId,int postId)
        {
            return await _context.reactions.Where(x=>x.UserId==userId && x.PostId==postId).FirstOrDefaultAsync()??new Reaction{
                PostId=0,
                UserId=""
            };
        }

        public async Task<Post> GetPost(int postId)
        {
            return  await _context.Posts.Where(x=>x.PostId==postId).Include(x=>x.user).FirstOrDefaultAsync() ?? new Post{
                user = new AppUser(),
                image = null
            };
        }
    }
}