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
    public class Product:Auditable
    {
        [StringLength(500)]
        public required string NameAr { get; set; }
        
        [StringLength(500)]
        public required string NameEn { get; set; }
        [StringLength(500)]
        public required string ShortDescriptionAr { get; set; }
        [StringLength(500)]
        public required string ShortDescriptionEn { get; set; }
        public required string DescriptionAr { get; set; }
        public required string DescriptionEn { get; set; }

        public required Guid CategoryID { get; set; }
        [ForeignKey(nameof(CategoryID))]
        public virtual ProductCategory? Category { get; set; }
    }
}
