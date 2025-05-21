using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.DTOs.AuthDTOs
{
   public class GetAllUserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
