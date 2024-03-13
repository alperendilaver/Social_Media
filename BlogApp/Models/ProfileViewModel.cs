using BlogApp.Data;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Models
{
    public class ProfileViewModel{
        
        public required AppUser user { get; set; }
        public List<Post> posts = new List<Post>();

        public List<Comment> comments = new List<Comment>();
    
    }
}