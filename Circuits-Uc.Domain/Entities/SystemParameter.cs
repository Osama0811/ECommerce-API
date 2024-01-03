using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircuitsUc.Domain.Entities.Base;

namespace CircuitsUc.Domain.Entities
{
    public class SystemParameter : Auditable
    {

        [StringLength(500)]
        public required string SettingKey { get; set; }
        [StringLength(500)]
        public required string SettingValueEN { get; set; }
        [StringLength(500)]
        public required string SettingValueAR{ get; set; }
       
        public bool IsSystemKey { get; set; }
      
    }
}
