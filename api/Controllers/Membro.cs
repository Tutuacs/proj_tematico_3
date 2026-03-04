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

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddMembroDto data)
    {
        var result = await _membroService.AddAsync(data);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data,
            Message = result.Message
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _membroService.GetByIdAsync(id);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data,
            Message = result.Message
        });
    }

    [HttpGet("horta/{hortaId}")]
    public async Task<IActionResult> GetByHortaId(int hortaId)
    {
        var result = await _membroService.GetByHortaIdAsync(hortaId);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data,
            Message = result.Message
        });
    }

    [HttpGet("perfil/{perfilId}")]
    public async Task<IActionResult> GetByPerfilId(string perfilId)
    {
        var result = await _membroService.GetByPerfilIdAsync(perfilId);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data,
            Message = result.Message
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMembroDto data)
    {
        var result = await _membroService.UpdateAsync(id, data);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data,
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
