using api.Data.Repository.Especie;
using api.Helpers;
using api.Model.Dtos.Especie;
using api.Model.Entities;

namespace api.Service.Especie;

public class EspecieService(IEspecieRepository especieRepository, ILogger<EspecieService> logger) : IEspecieService
{
    private readonly IEspecieRepository _especieRepository = especieRepository;
    private readonly ApiLogger<EspecieService> _logger = new(logger);

    public async Task<ServiceResponse<EspecieDb>> CreateAsync(CreateEspecieDto dto)
    {
        try
        {
            var especie = await _especieRepository.CreateAsync(dto);

            return new ServiceResponse<EspecieDb>
            {
                Data = especie,
                Message = "Espécie criada com sucesso",
                StatusCode = System.Net.HttpStatusCode.Created
            };
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, "Erro ao criar espécie: {Message}", ex.Message);
            return new ServiceResponse<EspecieDb>
            {
                Data = null,
                Message = "Erro ao criar espécie",
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }

    public async Task<ServiceResponse<EspecieDb>> GetByIdAsync(int id)
    {
        var especie = await _especieRepository.GetByIdAsync(id);

        if (especie == null)
        {
            return new ServiceResponse<EspecieDb>
            {
                Data = null,
                Message = "Espécie não encontrada",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        return new ServiceResponse<EspecieDb>
        {
            Data = especie,
            Message = "Espécie encontrada",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task<ServiceResponse<List<EspecieDb>>> GetAllAsync()
    {
        var especies = await _especieRepository.GetAllAsync();

        return new ServiceResponse<List<EspecieDb>>
        {
            Data = especies,
            Message = "Espécies listadas com sucesso",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task<ServiceResponse<EspecieDb>> UpdateAsync(int id, UpdateEspecieDto dto)
    {
        var especieExiste = await _especieRepository.GetByIdAsync(id);
        if (especieExiste == null)
        {
            return new ServiceResponse<EspecieDb>
            {
                Data = null,
                Message = "Espécie não encontrada",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        var especie = await _especieRepository.UpdateAsync(id, dto);

        if (especie == null)
        {
            return new ServiceResponse<EspecieDb>
            {
                Data = null,
                Message = "Espécie não encontrada",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        return new ServiceResponse<EspecieDb>
        {
            Data = especie,
            Message = "Espécie atualizada com sucesso",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task<ServiceResponse<object>> DeleteAsync(int id)
    {
        var especieExiste = await _especieRepository.GetByIdAsync(id);
        if (especieExiste == null)
        {
            return new ServiceResponse<object>
            {
                Data = null,
                Message = "Espécie não encontrada",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        var deleted = await _especieRepository.DeleteAsync(id);

        if (!deleted)
        {
            return new ServiceResponse<object>
            {
                Data = null,
                Message = "Espécie não encontrada",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        return new ServiceResponse<object>
        {
            Data = null,
            Message = "Espécie deletada com sucesso",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }
}