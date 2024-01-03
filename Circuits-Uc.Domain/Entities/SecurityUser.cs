using CircuitsUc.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Domain.Entities
{
    public class SecurityUser : Auditable
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
        public bool IsActive { get; set; } = true;

        public bool IsOnline { get; set; }
        public DateTime? LastLoginDate { get; set; }

    }
}
