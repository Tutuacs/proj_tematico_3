using api.Model.Dtos.Horta;
using api.Model.Entities;

namespace api.Data.Repository.Horta;

public interface IHortaRepository
{
    Task<HortaDb> CreateAsync(CreateHortaDto dto);
    Task<HortaResumoDto?> GetByIdAsync(int id);
    Task<List<HortaResumoDto>> GetAllAsync(string perfilId);
    Task<HortaDb?> UpdateAsync(int id, UpdateHortaDto dto);
    Task<bool> DeleteAsync(int id);
}
