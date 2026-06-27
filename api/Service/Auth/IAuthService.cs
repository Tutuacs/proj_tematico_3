using api.Helpers;
using api.Model.Dtos.Auth;
using api.Model.Entities;

namespace api.Service.Auth
{
    public interface IAuthService
    {
        Task<ServiceResponse<LoginResponse>> LoginAsync(LoginDto loginDto);
        Task<ServiceResponse<LoginResponse>> RegisterAsync(RegisterDto registerDto);
    }
}