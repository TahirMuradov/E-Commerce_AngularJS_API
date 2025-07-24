namespace Shop.Application.DTOs.AuthDTOs
{
    public class RemoveRoleUserDTO
    {
        public Guid UserId { get; set; }
        public Guid[] RoleId { get; set; }
    }
}
