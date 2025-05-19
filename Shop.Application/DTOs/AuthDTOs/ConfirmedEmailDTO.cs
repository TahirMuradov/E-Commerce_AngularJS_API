using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.DTOs.AuthDTOs
{
   public class ConfirmedEmailDTO
    {
        public string Email { get; set; }
        public string token { get; set; }
    }
}
