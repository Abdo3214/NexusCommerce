namespace NexusCommerce.BLL.DTOs.User
{
    public class AuthTokenDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}
