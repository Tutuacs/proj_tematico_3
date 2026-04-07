using api.Helpers;
using api.Model.Dtos.Especie;
using api.Model.Entities;

namespace api.Service.Especie;

public interface IEspecieService
{
    Task<ServiceResponse<EspecieDb>> CreateAsync(CreateEspecieDto dto);
    Task<ServiceResponse<EspecieDb>> GetByIdAsync(int id);
    Task<ServiceResponse<List<EspecieDb>>> GetAllAsync();
    Task<ServiceResponse<EspecieDb>> UpdateAsync(int id, UpdateEspecieDto dto);
    Task<ServiceResponse<object>> DeleteAsync(int id);
}