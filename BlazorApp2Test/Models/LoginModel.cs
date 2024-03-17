using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp2Test.Models
{
    internal class LoginModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "password is required")]
        public string? Password { get; set; }
    }
}