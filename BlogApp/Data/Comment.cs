using Microsoft.AspNetCore.Identity;

namespace BlogApp.Data
{
    public class Comment{
        public int CommentId { get; set; }
        public string? Text { get; set; }

        public string? userId { get; set; }
        public AppUser user { get; set; } = null!;
        public int postId { get; set; }
        public Post post { get; set; } = null!;
    }
}