using BlogApp.Data;
using BlogApp.Models;

namespace BlogApp.Repositories.Abstract
{
    public interface IPostRepository{
        Task<IEnumerable<Post>> GetPostsByUserIdAsync(string userId);
        IQueryable<Post> posts {get;}
        Task<int> CreatePost(Post post);
        Task<int> EditPost(PostEditViewModel post);
        Task<int> DeletePost(PostEditViewModel model);

        Task<int> DeleteComment(Comment comment);
        Task<Post> GetPost(int postId);
        Task<List<Reaction>> GetReactions(int postId);
        
        Task<Reaction> GetReaction(string userId,int postId);
        Task AddReaction(string userId,int postId,int reactionId);
        Task RemoveReaction(Reaction reaction);
    }
}