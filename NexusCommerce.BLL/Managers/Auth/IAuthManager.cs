using NexusCommerce.BLL.DTOs.User;
using NexusCommerce.Common.GeneralResult;

namespace NexusCommerce.BLL.Managers.Auth
{
    public interface IAuthManager
    {
        Task<GeneralResult<AuthTokenDto>> RegisterAsync(RegisterDto registerDto);
        Task<GeneralResult<AuthTokenDto>> LoginAsync(LoginDto loginDto);
    }
}
