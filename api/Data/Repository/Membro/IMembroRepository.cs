using api.Model.Dtos.Membro;
using api.Model.Entities;

namespace api.Data.Repository.Membro;

public interface IMembroRepository
{
    Task<MembroDb> AddAsync(AddMembroDto dto);
    Task<MembroDb?> GetByIdAsync(int id);
    Task<List<MembroDb>> GetByHortaIdAsync(int hortaId);
    Task<List<MembroDb>> GetByPerfilIdAsync(string perfilId);
    Task<MembroDb?> UpdateAsync(int id, UpdateMembroDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> IsMemberOfHortaAsync(int hortaId, string perfilId);
    Task<bool> IsAdminInAnyHortaAsync(string perfilId);
    Task<MembroDb?> GetByHortaAndPerfilAsync(int hortaId, string perfilId);
    Task<List<api.Model.Entities.ProfileDb>> GetAvailableProfilesForHortaAsync(int hortaId);
}
