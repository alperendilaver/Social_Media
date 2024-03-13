using BlogApp.Data;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Models
{
    public class PostIndexViewModel{
        public List<Post> posts =  new List<Post>();
        public List<AppUser> users =new List<AppUser>();

        public List<Comment> comments = new List<Comment>();
        public string? text { get; set; }
        public string? userId { get; set; }
        public int postId { get; set; }
    }
}