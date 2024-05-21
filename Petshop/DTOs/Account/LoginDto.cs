using System.ComponentModel.DataAnnotations;

namespace Petshop.DTOs.Account
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
