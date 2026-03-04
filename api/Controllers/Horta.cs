using api.Helpers;
using api.Model.Dtos.Horta;
using api.Service.Horta;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class HortaController(IHortaService hortaService) : ControllerBase
{
    private readonly IHortaService _hortaService = hortaService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHortaDto data)
    {
        var currentUser = JwtHelper.GetCurrentUser(User);
        if (currentUser == null)
        {
            return Unauthorized(new ApiResponse<object>
            {
                Data = null,
                Message = "Usuário não autenticado"
            });
        }

        var result = await _hortaService.CreateAsync(data, currentUser.Id);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data,
            Message = result.Message
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _hortaService.GetByIdAsync(id);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data,
            Message = result.Message
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _hortaService.GetAllAsync();
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data,
            Message = result.Message
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateHortaDto data)
    {
        var currentUser = JwtHelper.GetCurrentUser(User);
        if (currentUser == null)
        {
            return Unauthorized(new ApiResponse<object>
            {
                Data = null,
                Message = "Usuário não autenticado"
            });
        }

        var result = await _hortaService.UpdateAsync(id, data, currentUser.Id);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data,
            Message = result.Message
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var currentUser = JwtHelper.GetCurrentUser(User);
        if (currentUser == null)
        {
            return Unauthorized(new ApiResponse<object>
            {
                Data = null,
                Message = "Usuário não autenticado"
            });
        }

        var result = await _hortaService.DeleteAsync(id, currentUser.Id);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data,
            Message = result.Message
        });
    }
}
