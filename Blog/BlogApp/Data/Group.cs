using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Data
{
    public class Group{
        public int GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? image{get;set;}
        public List<AppUser>? GroupUsers = new List<AppUser>();

        [ForeignKey(nameof(AppUser.Id))]
        public string? userId { get; set; }
        public AppUser user { get; set; } = null!;
        public List<Post>? Posts = new List<Post>();
    }
}