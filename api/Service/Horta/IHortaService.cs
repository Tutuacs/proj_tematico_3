using api.Helpers;
using api.Model.Dtos.Horta;
using api.Model.Entities;

namespace api.Service.Horta;

public interface IHortaService
{
    Task<ServiceResponse<HortaDb>> CreateAsync(CreateHortaDto dto, string perfilId);
    Task<ServiceResponse<HortaResumoDto>> GetByIdAsync(int id);
    Task<ServiceResponse<List<HortaResumoDto>>> GetAllAsync(string perfilId);
    Task<ServiceResponse<HortaDb>> UpdateAsync(int id, UpdateHortaDto dto, string perfilId);
    Task<ServiceResponse<object>> DeleteAsync(int id, string perfilId);
}
