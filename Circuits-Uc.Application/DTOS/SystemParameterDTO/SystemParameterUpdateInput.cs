using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.DTOS.SystemParameterDTO
{
    public class SystemParameterUpdateInput
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string SettingKey { get; set; }
        [Required]
        public string SettingValueEN { get; set; }
        [Required]
        public string SettingValueAR { get; set; }
        [Required]
        public bool IsSystemKey { get; set; }
        public string? ImageBase64 { get; set; }
        public string? FileName { get; set; }
    }
}
