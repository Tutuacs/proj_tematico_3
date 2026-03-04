using api.Model.Dtos.Auth;
using api.Model.Entities;

namespace api.Data.Repository.Auth;

public interface IAuthRepository
{
    Task<ProfileDb> Register(RegisterDto registerDto);
    Task<ProfileDb?> Login(LoginDto loginDto);
    Task<bool> UserExists(string email);
}