using api.Helpers;
using api.Model.Dtos.Todo;
using api.Model.Entities;

namespace api.Service.Todo;

public interface ITodoService
{
    Task<ServiceResponse<TodoDb>> CreateAsync(CreateTodoDto dto);
    Task<ServiceResponse<TodoDb>> GetByIdAsync(int id);
    Task<ServiceResponse<List<TodoDb>>> GetAllAsync();
    Task<ServiceResponse<TodoDb>> UpdateAsync(int id, UpdateTodoDto dto);
    Task<ServiceResponse<object>> DeleteAsync(int id);
}