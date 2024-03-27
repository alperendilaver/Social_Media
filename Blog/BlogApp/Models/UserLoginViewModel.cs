using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class UserLoginViewModel{
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        
        public string? Password { get; set; }
    }
}