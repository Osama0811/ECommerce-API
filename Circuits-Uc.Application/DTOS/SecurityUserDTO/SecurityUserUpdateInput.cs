using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CircuitsUc.Application.Helpers.CommenEnum;

namespace CircuitsUc.Application.DTOS.SecurityUserDTO
{
    public class SecurityUserUpdateInput
    {
        public Guid Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public Nullable<int> RoleId { get; set; } = (int)RoleType.Admin;

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string? AlternativePhone { get; set; }

        public Nullable<int> PreferedContact { get; set; }
        public string? ImageBase64 { get; set; }
        public string? FileName { get; set; }
    }
}
