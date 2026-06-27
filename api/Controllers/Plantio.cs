using api.Helpers;
using api.Model.Dtos.Plantio;
using api.Service.Plantio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class PlantioController(IPlantioService plantioService) : ControllerBase
{
    private readonly IPlantioService _plantioService = plantioService;

    private static PlantioResponseDto MapToResponseDto(api.Model.Entities.PlantioDb plantio)
    {
        return new PlantioResponseDto
        {
            Id = plantio.Id,
            EspecieId = plantio.EspecieId,
            HortaId = plantio.HortaId,
            DataPlantio = plantio.DataPlantio,
            Quantidade = plantio.Quantidade,
            Status = plantio.Status.ToString(),
            Especie = plantio.Especie != null ? new EspecieResponseDto
            {
                Id = plantio.Especie.Id,
                Nome = plantio.Especie.Nome,
                Descricao = plantio.Especie.Descricao,
                ImageLink = plantio.Especie.ImageLink,
                DiasParaRegar = plantio.Especie.DiasParaRegar,
                DiasParaColher = plantio.Especie.DiasParaColher,
                MesesPlantio = plantio.Especie.MesesPlantio,
                MesesColheita = plantio.Especie.MesesColheita
            } : null
        };
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePlantioDto data)
    {
        var result = await _plantioService.CreateAsync(data);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data != null ? MapToResponseDto(result.Data) : null,
            Message = result.Message
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _plantioService.GetByIdAsync(id);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data != null ? MapToResponseDto(result.Data) : null,
            Message = result.Message
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _plantioService.GetAllAsync();
        var mappedData = result.Data?.Select(MapToResponseDto).ToList() ?? new List<PlantioResponseDto>();
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = mappedData,
            Message = result.Message
        });
    }

    [HttpGet("horta/{hortaId}")]
    public async Task<IActionResult> GetByHortaId(int hortaId)
    {
        var result = await _plantioService.GetByHortaIdAsync(hortaId);
        var mappedData = result.Data?.Select(MapToResponseDto).ToList() ?? new List<PlantioResponseDto>();
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = mappedData,
            Message = result.Message
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePlantioDto data)
    {
        var result = await _plantioService.UpdateAsync(id, data);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data != null ? MapToResponseDto(result.Data) : null,
            Message = result.Message
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _plantioService.DeleteAsync(id);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data,
            Message = result.Message
        });
    }
}