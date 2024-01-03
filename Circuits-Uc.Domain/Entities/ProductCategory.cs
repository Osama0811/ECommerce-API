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
    public class ProductCategory:Auditable
    {
        //[Required]
        [StringLength(500)]
        public required string NameAr { get; set; }
        //[Required]
        [StringLength(500)]
        public required string NameEn { get; set; }
        public  string? Icon { get; set; }
        [StringLength(500)]
        public string? DescriptionAr { get; set; }
        [StringLength(500)]
        public string? DescriptionEn { get; set; }
        public Nullable<Guid> ParentID { get; set; }
        [ForeignKey(nameof(ParentID))]
        public virtual ProductCategory? Parent { get; set; } 
    }
}
