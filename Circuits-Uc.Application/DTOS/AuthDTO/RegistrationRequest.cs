
using System.ComponentModel.DataAnnotations;

namespace CircuitsUc.Application.Models.AuthDTO
{
    public class RegistrationRequest
    {

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public int RoleId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string? AlternativePhone { get; set; }

        public Nullable<int> PreferedContact { get; set; }
        //public Nullable<long> Code { get; set; }
        //public bool IsActive { get; set; } = true;

        //public bool IsOnline { get; set; }
       // public DateTime? LastLoginDate { get; set; }

    }
}
