using api.Data.Repository.Membro;
using api.Helpers;
using api.Model.Dtos.Membro;
using api.Model.Entities;

namespace api.Service.Membro;

public class MembroService(IMembroRepository membroRepository, ILogger<MembroService> logger) : IMembroService
{
    private readonly IMembroRepository _membroRepository = membroRepository;
    private readonly ApiLogger<MembroService> _logger = new(logger);

    public async Task<ServiceResponse<MembroDb>> AddAsync(AddMembroDto dto)
    {
        try
        {
            // Verifica se já é membro
            var isMember = await _membroRepository.IsMemberOfHortaAsync(dto.HortaId, dto.PerfilId);
            if (isMember)
            {
                return new ServiceResponse<MembroDb>
                {
                    Data = null,
                    Message = "Usuário já é membro desta horta",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            var membro = await _membroRepository.AddAsync(dto);
            
            return new ServiceResponse<MembroDb>
            {
                Data = membro,
                Message = "Membro adicionado com sucesso",
                StatusCode = System.Net.HttpStatusCode.Created
            };
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, "Erro ao adicionar membro: {Message}", ex.Message);
            return new ServiceResponse<MembroDb>
            {
                Data = null,
                Message = "Erro ao adicionar membro",
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }

    public async Task<ServiceResponse<MembroDb>> GetByIdAsync(int id)
    {
        var membro = await _membroRepository.GetByIdAsync(id);
        
        if (membro == null)
        {
            return new ServiceResponse<MembroDb>
            {
                Data = null,
                Message = "Membro não encontrado",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        return new ServiceResponse<MembroDb>
        {
            Data = membro,
            Message = "Membro encontrado",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task<ServiceResponse<List<MembroDb>>> GetByHortaIdAsync(int hortaId)
    {
        var membros = await _membroRepository.GetByHortaIdAsync(hortaId);
        
        return new ServiceResponse<List<MembroDb>>
        {
            Data = membros,
            Message = "Membros listados com sucesso",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task<ServiceResponse<List<api.Model.Entities.ProfileDb>>> GetAddMembersByHortaIdAsync(int hortaId)
    {
        var perfisDisponiveis = await _membroRepository.GetAvailableProfilesForHortaAsync(hortaId);
        
        return new ServiceResponse<List<api.Model.Entities.ProfileDb>>
        {
            Data = perfisDisponiveis,
            Message = "Usuários disponíveis listados com sucesso",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task<ServiceResponse<List<MembroDb>>> GetByPerfilIdAsync(string perfilId)
    {
        var membros = await _membroRepository.GetByPerfilIdAsync(perfilId);
        
        return new ServiceResponse<List<MembroDb>>
        {
            Data = membros,
            Message = "Hortas do usuário listadas com sucesso",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task<ServiceResponse<MembroDb>> UpdateAsync(int id, UpdateMembroDto dto)
    {
        var membro = await _membroRepository.UpdateAsync(id, dto);
        
        if (membro == null)
        {
            return new ServiceResponse<MembroDb>
            {
                Data = null,
                Message = "Membro não encontrado",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        return new ServiceResponse<MembroDb>
        {
            Data = membro,
            Message = "Membro atualizado com sucesso",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task<ServiceResponse<object>> DeleteAsync(int id)
    {
        var deleted = await _membroRepository.DeleteAsync(id);
        
        if (!deleted)
        {
            return new ServiceResponse<object>
            {
                Data = null,
                Message = "Membro não encontrado",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        return new ServiceResponse<object>
        {
            Data = null,
            Message = "Membro removido com sucesso",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }
}
