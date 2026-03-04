using api.Helpers;
using api.Model.Dtos.Membro;
using api.Model.Entities;

namespace api.Service.Membro;

public interface IMembroService
{
    Task<ServiceResponse<MembroDb>> AddAsync(AddMembroDto dto);
    Task<ServiceResponse<MembroDb>> GetByIdAsync(int id);
    Task<ServiceResponse<List<MembroDb>>> GetByHortaIdAsync(int hortaId);
    Task<ServiceResponse<List<MembroDb>>> GetByPerfilIdAsync(string perfilId);
    Task<ServiceResponse<MembroDb>> UpdateAsync(int id, UpdateMembroDto dto);
    Task<ServiceResponse<object>> DeleteAsync(int id);
}
