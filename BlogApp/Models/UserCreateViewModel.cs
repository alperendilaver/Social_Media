using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class UserCreateViewModel{
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? SurName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [Phone]
        public string? Phone { get; set; }
        [Required]
         [StringLength(8, MinimumLength = 6, ErrorMessage = "Şifre en az 6, en fazla 8 karakter olmalıdır.")]
        public string? Password { get; set; }
    
        [Compare("Password",ErrorMessage ="Parolalar eşleşmiyor")]
        public string? ComparePassword{get;set;}
    }
}