using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class CreateGroupViewModel{
        
        [Required]
        public string? name { get; set; }

        public string? image { get; set; }
        public IFormFile? groupPhoto {get;set;}
    }
}