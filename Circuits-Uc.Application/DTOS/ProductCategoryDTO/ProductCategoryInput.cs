using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.DTOS.ProductCategoryDTO
{
    public class ProductCategoryInput
    {
        public required string NameAr { get; set; }

        public required string NameEn { get; set; }
        public string? Icon { get; set; }

        public string? DescriptionAr { get; set; }

        public string? DescriptionEn { get; set; }
        public Nullable<Guid> ParentID { get; set; } //= default(Nullable<Guid>);
        public string? ImageBase64 { get; set; }
        public string? FileName { get; set; }
    }
}
