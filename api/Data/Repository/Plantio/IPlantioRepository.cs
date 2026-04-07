using api.Model.Dtos.Plantio;
using api.Model.Entities;

namespace api.Data.Repository.Plantio;

public interface IPlantioRepository
{
    Task<PlantioDb> CreateAsync(CreatePlantioDto dto);
    Task<PlantioDb?> GetByIdAsync(int id);
    Task<List<PlantioDb>> GetAllAsync();
    Task<List<PlantioDb>> GetByHortaIdAsync(int hortaId);
    Task<PlantioDb?> UpdateAsync(int id, UpdatePlantioDto dto);
    Task<bool> DeleteAsync(int id);
}