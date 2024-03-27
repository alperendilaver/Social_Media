using BlogApp.Data;
namespace BlogApp.Models
{
    public class GroupDetailViewModel{
        public required Group group  {get;set;}
        public List<Post> posts =  new List<Post>();
        public List<AppUser> users =new List<AppUser>();

        public List<Comment> comments = new List<Comment>();
        public string? text { get; set; }
        public string? userId { get; set; }
        public int postId { get; set; }
        public int GroupId { get; set; }
        public bool isCurrUserA_Admin {get;set;} = false;      
    }    
}