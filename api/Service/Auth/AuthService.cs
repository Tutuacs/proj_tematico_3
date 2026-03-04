using api.Data.Repository.Auth;
using api.Helpers;
using api.Model.Dtos.Auth;
using api.Model.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace api.Service.Auth
{
    public class AuthService(IAuthRepository authRepository, ILogger<AuthService> logger, JwtHelper jwtHelper) : IAuthService
    {
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly ApiLogger<AuthService> _logger = new(logger);
        private readonly JwtHelper _jwtHelper = jwtHelper;

        public async Task<ServiceResponse<LoginResponse>> LoginAsync(LoginDto data)
        {
            // print email and password to console for debugging
            _logger.Log(LogLevel.Information, "Attempting login for email: {Email}", data.Email);

            var profile = await _authRepository.Login(data);
            
            if (profile == null)
            {
                return new ServiceResponse<LoginResponse>
                {
                    Data = null,
                    Message = "Incorrect email or password",
                    StatusCode = System.Net.HttpStatusCode.Unauthorized
                };
            }

            var (token, expiration) = _jwtHelper.GenerateToken(profile);

            var loginResponse = new LoginResponse
            {
                Profile = new ProfileResponse
                {
                    Id = profile.Id,
                    Name = profile.Name,
                    Email = profile.Email,
                    CreatedAt = profile.CreatedAt
                },
                Token = token,
                Expiration = expiration
            };

            return new ServiceResponse<LoginResponse>
            {
                Data = loginResponse,
                Message = "Login successful",
            };
        }

        public async Task<ServiceResponse<LoginResponse>> RegisterAsync(RegisterDto data)
        {
            // Implementação do registro de usuário
            if (await _authRepository.UserExists(data.Email))
            {
                return new ServiceResponse<LoginResponse>
                {
                    Data = null,
                    Message = "User already exists",
                    StatusCode = System.Net.HttpStatusCode.Conflict
                };
            }

            ProfileDb profile = await _authRepository.Register(data);

            // Gera token JWT automaticamente após registro
            var (token, expiration) = _jwtHelper.GenerateToken(profile);

            var loginResponse = new LoginResponse
            {
                Profile = new ProfileResponse
                {
                    Id = profile.Id,
                    Name = profile.Name,
                    Email = profile.Email,
                    CreatedAt = profile.CreatedAt
                },
                Token = token,
                Expiration = expiration
            };

            return new ServiceResponse<LoginResponse>
            {
                Data = loginResponse,
                Message = "Registration successful. You are now logged in.",
            };
        }

    }
}