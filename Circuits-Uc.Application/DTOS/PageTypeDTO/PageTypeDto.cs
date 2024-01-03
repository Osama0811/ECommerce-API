using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.DTOS.PageTypeDTO
{
    public class PageTypeDto
    {
      
        public Guid Id { get; set; }
        public string PageName { get; set; }
    }
}
