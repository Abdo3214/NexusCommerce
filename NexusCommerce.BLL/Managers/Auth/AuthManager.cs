using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NexusCommerce.BLL.DTOs.User;
using NexusCommerce.Common;
using NexusCommerce.Common.GeneralResult;
using NexusCommerce.DAL.Data.Models;
using NexusCommerce.DAL.UnitOfWork;

namespace NexusCommerce.BLL.Managers.Auth
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;

        public AuthManager(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<GeneralResult<AuthTokenDto>> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return GeneralResult<AuthTokenDto>.FailResult(Errors.CreateSingle("Auth", "DuplicateEmail", "Email is already in use."));
            }

            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(user, registerDto.Password);
            if (!createResult.Succeeded)
            {
                var firstError = createResult.Errors.FirstOrDefault()?.Description ?? "Registration failed.";
                return GeneralResult<AuthTokenDto>.FailResult(Errors.CreateSingle("Auth", "RegistrationFailed", firstError));
            }

            if (!await _roleManager.RoleExistsAsync("Customer"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Customer"));
            }
            await _userManager.AddToRoleAsync(user, "Customer");

            var cart = new NexusCommerce.DAL.Data.Models.Cart
            {
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Carts.AddAsync(cart);
            await _unitOfWork.SaveChangesAsync();

            var tokenDto = await GenerateTokenAsync(user);
            return GeneralResult<AuthTokenDto>.SuccessResult(tokenDto, "Registration successful.");
        }

        public async Task<GeneralResult<AuthTokenDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return GeneralResult<AuthTokenDto>.FailResult(Errors.CreateSingle("Auth", "InvalidCredentials", "Invalid email or password."));
            }

            var tokenDto = await GenerateTokenAsync(user);
            return GeneralResult<AuthTokenDto>.SuccessResult(tokenDto, "Login successful.");
        }

        private async Task<AuthTokenDto> GenerateTokenAsync(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("FullName", user.FullName)
            };

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddDays(_jwtSettings.DurationInDays),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new AuthTokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName
            };
        }
    }
}
