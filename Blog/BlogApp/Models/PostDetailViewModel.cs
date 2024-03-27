using BlogApp.Data;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Models
{
    public class PostDetailViewModel{
        public AppUser user { get; set; } = null!;
        public Post post { get; set; } = null!;
         public List<Comment> comments = new List<Comment>();
        public string? text { get; set; }

        public List<Reaction> reactions =new List<Reaction>();
        public string? userId { get; set; }
        public int postId { get; set; }
        public Reaction userReaction { get; set; } = null!;
    }
}