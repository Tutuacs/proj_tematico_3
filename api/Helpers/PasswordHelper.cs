using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace api.Helpers;

public static class PasswordHelper
{
    private const int SaltSize = 128 / 8; // 16 bytes
    private const int HashSize = 256 / 8; // 32 bytes
    private const int Iterations = 100000;

    /// <summary>
    /// Gera um hash seguro da senha com salt embutido
    /// </summary>
    /// <param name="password">Senha em texto plano</param>
    /// <returns>String contendo salt e hash combinados no formato: $salt$hash</returns>
    public static string HashPassword(string password)
    {
        // Gera um salt aleatório
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        
        // Gera o hash usando PBKDF2
        byte[] hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: Iterations,
            numBytesRequested: HashSize);

        // Combina salt e hash em uma única string: $iterations$salt$hash
        return $"${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    /// <summary>
    /// Verifica se a senha corresponde ao hash armazenado
    /// </summary>
    /// <param name="password">Senha em texto plano para verificar</param>
    /// <param name="storedHash">Hash completo armazenado (formato: $iterations$salt$hash)</param>
    /// <returns>True se a senha corresponder</returns>
    public static bool VerifyPassword(string password, string storedHash)
    {
        try
        {
            // Extrai as partes do hash armazenado
            // Formato: $iterations$salt$hash
            // Split gera: ["", "iterations", "salt", "hash"]
            var parts = storedHash.Split('$');
            if (parts.Length != 4)
                return false;

            int iterations = int.Parse(parts[1]);
            byte[] salt = Convert.FromBase64String(parts[2]);
            byte[] hash = Convert.FromBase64String(parts[3]);
            
            // Gera hash com a mesma senha e salt
            byte[] hashToVerify = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: iterations,
                numBytesRequested: HashSize);

            // Compara os hashes de forma segura (timing-safe)
            return CryptographicOperations.FixedTimeEquals(hash, hashToVerify);
        }
        catch
        {
            return false;
        }
    }
}
