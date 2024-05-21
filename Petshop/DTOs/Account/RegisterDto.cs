using System.ComponentModel.DataAnnotations;

namespace Petshop.DTOs.Account
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }
        [Required]
        [MaxLength (100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(60)]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Required]
        public string ConPassword { get; set; }
    }
}
