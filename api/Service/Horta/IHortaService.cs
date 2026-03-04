using api.Helpers;
using api.Model.Dtos.Horta;
using api.Model.Entities;

namespace api.Service.Horta;

public interface IHortaService
{
    Task<ServiceResponse<HortaDb>> CreateAsync(CreateHortaDto dto, string perfilId);
    Task<ServiceResponse<HortaDb>> GetByIdAsync(int id);
    Task<ServiceResponse<List<HortaDb>>> GetAllAsync();
    Task<ServiceResponse<HortaDb>> UpdateAsync(int id, UpdateHortaDto dto, string perfilId);
    Task<ServiceResponse<object>> DeleteAsync(int id, string perfilId);
}
