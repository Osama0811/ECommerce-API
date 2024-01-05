using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.DTOS.PageContentDTO
{
    public class PageContentInput
    {
        [StringLength(500)]
        public required string NameAr { get; set; }

        [StringLength(500)]
        public required string NameEn { get; set; }
        [StringLength(500)]
        public required string ShortDescriptionAr { get; set; }
        [StringLength(500)]
        public required string ShortDescriptionEn { get; set; }
        public  string DescriptionAr { get; set; }
        public  string DescriptionEn { get; set; }
        public int TypeID { get; set; }
        public string? PostedBy { get; set; }
        public DateTime? PostedDate { get; set; }
        public Nullable<int> Rank { get; set; }
        public string? ImageBase64 { get; set; }
        public string? FileName { get; set; }
    }
}
