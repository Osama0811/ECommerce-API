using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.Models.AuthDTO
{
    public class ChangeUserPasswordResponse
    {
        public Guid Id { get; set; }
  
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
