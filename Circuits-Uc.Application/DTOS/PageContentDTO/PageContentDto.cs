using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.DTOS.PageContentDTO
{
    public class PageContentDto
    {
        public Guid Id { get; set; }
        public  string NameAr { get; set; }

        public  string NameEn { get; set; }
        public  string Name { get; set; }
       
        public  string ShortDescriptionAr { get; set; }
        
        public  string ShortDescriptionEn { get; set; }
        public  string ShortDescription { get; set; }
        public  string DescriptionAr { get; set; }
        public  string DescriptionEn { get; set; }
        public  string Description { get; set; }
        public  string? ImagePath { get; set; }
        public int TypeID { get; set; }
        public string? TypeName { get; set; }
        public string? PostedBy { get; set; }
        public string? PostedDate { get; set; }
        public Nullable<int> Rank { get; set; }
    }
}
