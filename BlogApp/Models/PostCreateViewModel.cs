using BlogApp.Data;

namespace BlogApp.Models
{
    public class PostCreateViewModel{
        public required string? Context { get; set; }
        public IFormFile? imageFile { get; set; }
        public string? image { get; set; }

        public int? GroupId {get;set;}
    }
}