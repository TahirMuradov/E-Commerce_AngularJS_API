using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.DTOs.AuthDTOs
{
    public class RemoveRoleUserDTO
    {
        public Guid UserId { get; set; }
        public Guid[] RoleId { get; set; }
    }
}
