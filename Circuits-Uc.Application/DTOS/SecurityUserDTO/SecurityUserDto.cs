using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.DTOS.SecurityUserDTO
{
    public class SecurityUserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? AlternativePhone { get; set; }

        public int? PreferedContact { get; set; }
        public long RoleId { get; set; }
        public Nullable<long> Code { get; set; }
        public bool IsActive { get; set; }

        public bool IsOnline { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string CreatedBynamed { get; set; }
        public string? UpdatedBynamed { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
