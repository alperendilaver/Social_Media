using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Models
{
    public class ResetPasswordViewModel
    {
        public string token { get; set; } = string.Empty;
        [Required]
         [StringLength(8, MinimumLength = 6, ErrorMessage = "Şifre en az 6, en fazla 8 karakter olmalıdır.")]
        public string password { get; set; } = string.Empty;
        
        [Compare("password",ErrorMessage ="Parolalar Eşleşmiyor")]
        public string confirm_password { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
    }
}