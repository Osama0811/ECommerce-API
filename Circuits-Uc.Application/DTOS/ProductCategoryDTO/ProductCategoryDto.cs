using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.DTOS.ProductCategoryDTO
{
    public class ProductCategoryDto
    {
        public Guid Id { get; set; }    
        public  string Name { get; set; }
        
        public string? Icon { get; set; }
        
        public string? Description{ get; set; }
        public string? ImagePath{ get; set; }
       
        public Nullable<Guid> ParentID { get; set; }
        public string? ParentName { get; set; }
    }
}
