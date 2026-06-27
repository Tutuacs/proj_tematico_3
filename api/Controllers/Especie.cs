using api.Helpers;
using api.Model.Dtos.Especie;
using api.Service.Especie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class EspecieController(IEspecieService especieService) : ControllerBase
{
    private readonly IEspecieService _especieService = especieService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEspecieDto data)
    {
        var result = await _especieService.CreateAsync(data);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data,
            Message = result.Message
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _especieService.GetByIdAsync(id);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data,
            Message = result.Message
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _especieService.GetAllAsync();
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data,
            Message = result.Message
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEspecieDto data)
    {
        var result = await _especieService.UpdateAsync(id, data);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data,
            Message = result.Message
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _especieService.DeleteAsync(id);
        return StatusCode((int)result.StatusCode, new ApiResponse<object>
        {
            Data = result.Data,
            Message = result.Message
        });
    }
}