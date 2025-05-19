using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.DTOs.AuthDTOs
{
   public class GetAllUserForSelectDTO
    {
        public Guid Userid { get; set; }
        public string Email { get; set; }
    }
}
