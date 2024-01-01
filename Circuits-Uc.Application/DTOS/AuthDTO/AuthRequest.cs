using System.ComponentModel.DataAnnotations;

namespace CircuitsUc.Application.Models.AuthDTO
{
    public class AuthRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
