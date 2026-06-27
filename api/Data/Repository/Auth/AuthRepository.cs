using api.Helpers;
using api.Model.Dtos.Auth;
using api.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Repository.Auth;

public class AuthRepository(ApplicationDbContext db) : IAuthRepository
{
    private readonly ApplicationDbContext _db = db;

    public async Task<ProfileDb> Register(RegisterDto registerDto)
    {
        // Hash da senha antes de salvar (salt embutido no hash)
        var hashedPassword = PasswordHelper.HashPassword(registerDto.Password);
        
        var profile = new ProfileDb(registerDto.Name, registerDto.Email, hashedPassword);
        _db.Profiles.Add(profile);
        await _db.SaveChangesAsync();
        return profile;
    }

    public async Task<ProfileDb?> Login(LoginDto loginDto)
    {
        // Busca o usuário pelo email
        var profile = await _db.Profiles.FirstOrDefaultAsync(p => p.Email == loginDto.Email);
        
        if (profile == null)
        {
            return null;
        }

        // Verifica se a senha corresponde ao hash armazenado
        bool isPasswordValid = PasswordHelper.VerifyPassword(
            loginDto.Password, 
            profile.Password);

        if (!isPasswordValid)
        {
            return null;
        }

        return profile;
    }

    public Task<bool> UserExists(string email)
    {
        return _db.Profiles.AnyAsync(p => p.Email == email);
    }


}