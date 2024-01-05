using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.DTOS.SystemParameterKeyDTO
{
    public class ContactEmailConfiguration
    {
      
        public string FromMailAddress { get; set; }
        public string ToMailAddress { get; set; }
        public string Password { get; set; }
        public string SMTP { get; set; }
        public string Port { get; set; }
    }
}
