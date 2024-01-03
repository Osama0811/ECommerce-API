using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.DTOS.SystemParameterDTO
{
    public class SystemParameterDto
    {
        public Guid Id { get; set; }
        public string SettingKey { get; set; }
        public object SettingValueEN { get; set; }
        public object SettingValueAR { get; set; }
        public bool IsSystemKey { get; set; }
        public object? SettingValue { get; set; }

      
    }
}
