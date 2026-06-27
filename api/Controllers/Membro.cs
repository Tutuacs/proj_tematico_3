using api.Helpers;
using api.Model.Dtos.Membro;
using api.Service.Membro;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class MembroController(IMembroService membroService) : ControllerBase
{
    private readonly IMembroService _membroService = membroService;

    private static MembroResponseDto MapToResponseDto(api.Model.Entities.MembroDb membro)
    {
        return new MembroResponseDto
        {
            Id = membro.Id,
            HortaId = membro.HortaId,
            PerfilId = membro.PerfilId,
            Role = membro.Role.ToString(),
            Profile = membro.Profile != null ? new ProfileResponseDto
            {
                Id = membro.Profile.Id,
                Name = membro.Profile.Name,
                Email = membro.Profile.Email
            } : null,
            Horta = membro.Horta != null ? new HortaResponseDto
            {
                Id = membro.Horta.Id,
                Nome = membro.Horta.Nome,
                Descricao = membro.Horta.Descricao,
                Local = membro.Horta.Local
            } : null
        };
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddMembroDto data)
    {
        var result = await _membroService.AddAsync(data);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data != null ? MapToResponseDto(result.Data) : null,
            Message = result.Message
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _membroService.GetByIdAsync(id);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data != null ? MapToResponseDto(result.Data) : null,
            Message = result.Message
        });
    }

    [HttpGet("add/members/horta/{hortaId}")]
    public async Task<IActionResult> GetAddMembersByHortaId(int hortaId)
    {
        var result = await _membroService.GetAddMembersByHortaIdAsync(hortaId);
        var mappedData = result.Data?.Select(p => new ProfileResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Email = p.Email
        }).ToList() ?? new List<ProfileResponseDto>();

        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = mappedData,
            Message = result.Message
        });
    }

    [HttpGet("horta/{hortaId}")]
    public async Task<IActionResult> GetByHortaId(int hortaId)
    {
        var result = await _membroService.GetByHortaIdAsync(hortaId);
        if (result.Data == null)
        {
            return StatusCode((int)result.StatusCode, new ApiResponse<object>
            {
                Data = new List<MembroResponseDto>(),
                Message = result.Message
            });
        }
        var mappedData = result.Data.Select(MapToResponseDto).ToList();
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = mappedData,
            Message = result.Message
        });
    }

    [HttpGet("perfil/{perfilId}")]
    public async Task<IActionResult> GetByPerfilId(string perfilId)
    {
        var result = await _membroService.GetByPerfilIdAsync(perfilId);
        var mappedData = result.Data?.Select(MapToResponseDto).ToList() ?? [];
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = mappedData,
            Message = result.Message
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMembroDto data)
    {
        var result = await _membroService.UpdateAsync(id, data);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data != null ? MapToResponseDto(result.Data) : null,
            Message = result.Message
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _membroService.DeleteAsync(id);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data,
            Message = result.Message
        });
    }
}
