using api.Model.Dtos.Horta;
using api.Model.Entities;

namespace api.Data.Repository.Horta;

public interface IHortaRepository
{
    Task<HortaDb> CreateAsync(CreateHortaDto dto);
    Task<HortaDb?> GetByIdAsync(int id);
    Task<List<HortaDb>> GetAllAsync();
    Task<HortaDb?> UpdateAsync(int id, UpdateHortaDto dto);
    Task<bool> DeleteAsync(int id);
}
