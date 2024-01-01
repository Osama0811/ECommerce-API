using CircuitsUc.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Domain.Entities
{
    public class Document : Auditable
    {
       
        public string RefEntityTypeID { get; set; }
        public string RefEntityID { get; set; }
        public bool IsMain { get; set; }
        public string FileName { get; set; }
        public string? FileExtension { get; set; }
        public string? Notes { get; set; }
        public int? Rank { get; set; }
    }
}
