using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Data
{
    public enum Reactions
    {
        like,unlike,angry,congr
    }
    public class Post{
        public int PostId{get;set;}
        
        public string? Context{get;set;}
        
        public DateTime Published{get;set;}
        public string? UserId { get; set; }
        public AppUser user { get; set; } = null!;
        public List<Comment> Comments = new List<Comment>(); 
        public string? image {get;set;}

        public  List<Reactions> reactions =new List<Reactions>();
        
        public int? GroupId { get; set; }
        public Group? grup {get;set;} 
    }
}