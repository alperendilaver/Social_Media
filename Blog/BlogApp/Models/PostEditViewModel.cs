namespace BlogApp.Models{
    public class PostEditViewModel{
        public string? Context{get;set;}
        public string? image { get; set; }
        public IFormFile? imageFile { get; set; }
        public int PostId { get; set; }
        public bool deletePhoto { get; set; } =false;
        public int? GroupId { get; set; }
    }
}