using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp2Test.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "E-mail is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "password is required")]
        public string? Password { get; set; }
    }
}