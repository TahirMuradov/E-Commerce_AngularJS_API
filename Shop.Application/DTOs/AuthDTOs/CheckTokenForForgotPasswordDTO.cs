namespace Shop.Application.DTOs.AuthDTOs
{
    public class CheckTokenForForgotPasswordDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
