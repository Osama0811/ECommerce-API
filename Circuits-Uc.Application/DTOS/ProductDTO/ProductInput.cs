using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.DTOS.ProductDTO
{
    public class ProductInput
    {
        
        [StringLength(500)]
        public required string NameAr { get; set; }

        [StringLength(500)]
        public required string NameEn { get; set; }
        [StringLength(500)]
        public required string ShortDescriptionAr { get; set; }
        [StringLength(500)]
        public required string ShortDescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }

        public required Guid CategoryID { get; set; }
        public required string ImageBase64 { get; set; }
        public required string ImageName { get; set; }
        public string? FileBase64 { get; set; }
        public string? FileName { get; set; }
    }
}
