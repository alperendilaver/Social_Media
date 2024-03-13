using BlogApp.Data;
using BlogApp.Models;

namespace BlogApp.Repositories.Abstract
{
    public interface IPostRepository{
        Task<IEnumerable<Post>> GetPostsByUserIdAsync(string userId);
        IQueryable<Post> posts {get;}
        void CreatePost(Post post);
        void EditPost(PostEditViewModel post);
        void DeletePost(PostEditViewModel model);

        void DeleteComment(Comment comment);
        Task AddReaction(string userId,int postId,int reactionId);
        Task RemoveReaction(Reaction reaction);
    }
}