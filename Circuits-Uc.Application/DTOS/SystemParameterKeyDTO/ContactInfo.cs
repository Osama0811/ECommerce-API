using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.DTOS.SystemParameterKeyDTO
{
    public class ContactInfo
    {
        public string? Facebook { set; get; }
        public string? twitter { set; get; }
        public string? WhatsApp { set; get; }
        public string? WhatsAppDefaultMessage { set; get; }
        public string? CustomerService { set; get; }
        public string? WebSite { set; get; }
        public string? Instagram { set; get; }
    }
}
