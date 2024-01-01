using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.DTOS.SecurityUserDTO
{
    public class SecurityUserUpdateInput
    {
        public Guid Id { get; set; }    
        [Required]
        public string UserName { get; set; }
        
        public int RoleId { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string NationalNum { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string? AlterPhone { get; set; }
        public string? ImageBase64 { get; set; }
        public string? FileName { get; set; }
    }
}
