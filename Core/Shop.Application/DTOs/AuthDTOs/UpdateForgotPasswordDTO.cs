namespace Shop.Application.DTOs.AuthDTOs
{
    public class UpdateForgotPasswordDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string NewConfirmPassword { get; set; }
    }
}
