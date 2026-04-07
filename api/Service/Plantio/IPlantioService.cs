using api.Helpers;
using api.Model.Dtos.Plantio;
using api.Model.Entities;

namespace api.Service.Plantio;

public interface IPlantioService
{
    Task<ServiceResponse<PlantioDb>> CreateAsync(CreatePlantioDto dto);
    Task<ServiceResponse<PlantioDb>> GetByIdAsync(int id);
    Task<ServiceResponse<List<PlantioDb>>> GetAllAsync();
    Task<ServiceResponse<List<PlantioDb>>> GetByHortaIdAsync(int hortaId);
    Task<ServiceResponse<PlantioDb>> UpdateAsync(int id, UpdatePlantioDto dto);
    Task<ServiceResponse<object>> DeleteAsync(int id);
}