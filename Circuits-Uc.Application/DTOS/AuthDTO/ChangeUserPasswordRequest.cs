using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.Models.AuthDTO
{
    public class ChangeUserPasswordRequest
    {
        public Guid Id { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword", ErrorMessage = "PasswordNotMatch")]
        public string ConfirmPassword { get; set; }
    }
}
