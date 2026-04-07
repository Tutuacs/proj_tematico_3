using api.Data.Repository.Plantio;
using api.Helpers;
using api.Model.Dtos.Plantio;
using api.Model.Entities;

namespace api.Service.Plantio;

public class PlantioService(IPlantioRepository plantioRepository, ILogger<PlantioService> logger) : IPlantioService
{
    private readonly IPlantioRepository _plantioRepository = plantioRepository;
    private readonly ApiLogger<PlantioService> _logger = new(logger);

    public async Task<ServiceResponse<PlantioDb>> CreateAsync(CreatePlantioDto dto)
    {
        try
        {
            var plantio = await _plantioRepository.CreateAsync(dto);

            return new ServiceResponse<PlantioDb>
            {
                Data = plantio,
                Message = "Plantio criado com sucesso",
                StatusCode = System.Net.HttpStatusCode.Created
            };
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, "Erro ao criar plantio: {Message}", ex.Message);
            return new ServiceResponse<PlantioDb>
            {
                Data = null,
                Message = "Erro ao criar plantio",
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }

    public async Task<ServiceResponse<PlantioDb>> GetByIdAsync(int id)
    {
        var plantio = await _plantioRepository.GetByIdAsync(id);

        if (plantio == null)
        {
            return new ServiceResponse<PlantioDb>
            {
                Data = null,
                Message = "Plantio não encontrado",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        return new ServiceResponse<PlantioDb>
        {
            Data = plantio,
            Message = "Plantio encontrado",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task<ServiceResponse<List<PlantioDb>>> GetAllAsync()
    {
        var plantios = await _plantioRepository.GetAllAsync();

        return new ServiceResponse<List<PlantioDb>>
        {
            Data = plantios,
            Message = "Plantios listados com sucesso",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task<ServiceResponse<List<PlantioDb>>> GetByHortaIdAsync(int hortaId)
    {
        var plantios = await _plantioRepository.GetByHortaIdAsync(hortaId);

        return new ServiceResponse<List<PlantioDb>>
        {
            Data = plantios,
            Message = "Plantios da horta listados com sucesso",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task<ServiceResponse<PlantioDb>> UpdateAsync(int id, UpdatePlantioDto dto)
    {
        var plantioExiste = await _plantioRepository.GetByIdAsync(id);
        if (plantioExiste == null)
        {
            return new ServiceResponse<PlantioDb>
            {
                Data = null,
                Message = "Plantio não encontrado",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        var plantio = await _plantioRepository.UpdateAsync(id, dto);

        if (plantio == null)
        {
            return new ServiceResponse<PlantioDb>
            {
                Data = null,
                Message = "Plantio não encontrado",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        return new ServiceResponse<PlantioDb>
        {
            Data = plantio,
            Message = "Plantio atualizado com sucesso",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task<ServiceResponse<object>> DeleteAsync(int id)
    {
        var plantioExiste = await _plantioRepository.GetByIdAsync(id);
        if (plantioExiste == null)
        {
            return new ServiceResponse<object>
            {
                Data = null,
                Message = "Plantio não encontrado",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        var deleted = await _plantioRepository.DeleteAsync(id);

        if (!deleted)
        {
            return new ServiceResponse<object>
            {
                Data = null,
                Message = "Plantio não encontrado",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        return new ServiceResponse<object>
        {
            Data = null,
            Message = "Plantio deletado com sucesso",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }
}