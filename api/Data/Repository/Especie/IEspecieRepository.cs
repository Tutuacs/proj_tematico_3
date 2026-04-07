using api.Model.Dtos.Especie;
using api.Model.Entities;

namespace api.Data.Repository.Especie;

public interface IEspecieRepository
{
    Task<EspecieDb> CreateAsync(CreateEspecieDto dto);
    Task<EspecieDb?> GetByIdAsync(int id);
    Task<List<EspecieDb>> GetAllAsync();
    Task<EspecieDb?> UpdateAsync(int id, UpdateEspecieDto dto);
    Task<bool> DeleteAsync(int id);
}