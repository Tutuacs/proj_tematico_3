using api.Data.Repository.Horta;
using api.Data.Repository.Membro;
using api.Helpers;
using api.Model.Dtos.Horta;
using api.Model.Dtos.Membro;
using api.Model.Entities;

namespace api.Service.Horta;

public class HortaService(IHortaRepository hortaRepository, IMembroRepository membroRepository, ILogger<HortaService> logger) : IHortaService
{
    private readonly IHortaRepository _hortaRepository = hortaRepository;
    private readonly IMembroRepository _membroRepository = membroRepository;
    private readonly ApiLogger<HortaService> _logger = new(logger);

    public async Task<ServiceResponse<HortaDb>> CreateAsync(CreateHortaDto dto, string perfilId)
    {
        try
        {
            // Verifica se o usuário é Admin em alguma horta
            var isAdmin = await _membroRepository.IsAdminInAnyHortaAsync(perfilId);
            
            if (!isAdmin)
            {
                return new ServiceResponse<HortaDb>
                {
                    Data = null,
                    Message = "Apenas membros Administradores podem criar hortas",
                    StatusCode = System.Net.HttpStatusCode.Forbidden
                };
            }
            
            var horta = await _hortaRepository.CreateAsync(dto);
            var membro = await _membroRepository.AddAsync(new AddMembroDto
            {
                HortaId = horta.Id,
                PerfilId = perfilId,
                Role = Enums.ROLE.Admin
            });
            
            return new ServiceResponse<HortaDb>
            {
                Data = horta,
                Message = "Horta criada com sucesso",
                StatusCode = System.Net.HttpStatusCode.Created
            };
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, "Erro ao criar horta: {Message}", ex.Message);
            return new ServiceResponse<HortaDb>
            {
                Data = null,
                Message = "Erro ao criar horta",
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }

    public async Task<ServiceResponse<HortaResumoDto>> GetByIdAsync(int id)
    {
        var horta = await _hortaRepository.GetByIdAsync(id);
        
        if (horta == null)
        {
            return new ServiceResponse<HortaResumoDto>
            {
                Data = null,
                Message = "Horta não encontrada",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        return new ServiceResponse<HortaResumoDto>
        {
            Data = horta,
            Message = "Horta encontrada",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task<ServiceResponse<List<HortaResumoDto>>> GetAllAsync()
    {
        var hortas = await _hortaRepository.GetAllAsync();
        
        return new ServiceResponse<List<HortaResumoDto>>
        {
            Data = hortas,
            Message = "Hortas listadas com sucesso",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task<ServiceResponse<HortaDb>> UpdateAsync(int id, UpdateHortaDto dto, string perfilId)
    {
        // Verifica se a horta existe
        var hortaExiste = await _hortaRepository.GetByIdAsync(id);
        if (hortaExiste == null)
        {
            return new ServiceResponse<HortaDb>
            {
                Data = null,
                Message = "Horta não encontrada",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        // Verifica se o usuário é Admin da horta
        var membro = await _membroRepository.GetByHortaAndPerfilAsync(id, perfilId);
        if (membro == null || membro.Role != Enums.ROLE.Admin)
        {
            return new ServiceResponse<HortaDb>
            {
                Data = null,
                Message = "Apenas administradores da horta podem atualizá-la",
                StatusCode = System.Net.HttpStatusCode.Forbidden
            };
        }

        var horta = await _hortaRepository.UpdateAsync(id, dto);
        
        if (horta == null)
        {
            return new ServiceResponse<HortaDb>
            {
                Data = null,
                Message = "Horta não encontrada",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        return new ServiceResponse<HortaDb>
        {
            Data = horta,
            Message = "Horta atualizada com sucesso",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task<ServiceResponse<object>> DeleteAsync(int id, string perfilId)
    {
        // Verifica se a horta existe
        var hortaExiste = await _hortaRepository.GetByIdAsync(id);
        if (hortaExiste == null)
        {
            return new ServiceResponse<object>
            {
                Data = null,
                Message = "Horta não encontrada",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        // Verifica se o usuário é Admin da horta
        var membro = await _membroRepository.GetByHortaAndPerfilAsync(id, perfilId);
        if (membro == null || membro.Role != Enums.ROLE.Admin)
        {
            return new ServiceResponse<object>
            {
                Data = null,
                Message = "Apenas administradores da horta podem deletá-la",
                StatusCode = System.Net.HttpStatusCode.Forbidden
            };
        }

        var deleted = await _hortaRepository.DeleteAsync(id);
        
        if (!deleted)
        {
            return new ServiceResponse<object>
            {
                Data = null,
                Message = "Horta não encontrada",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        return new ServiceResponse<object>
        {
            Data = null,
            Message = "Horta deletada com sucesso",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }
}
