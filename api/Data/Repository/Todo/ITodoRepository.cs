using api.Model.Dtos.Todo;
using api.Model.Entities;

namespace api.Data.Repository.Todo;

public interface ITodoRepository
{
    Task<TodoDb> CreateAsync(CreateTodoDto dto);
    Task<TodoDb?> GetByIdAsync(int id);
    Task<List<TodoDb>> GetAllAsync();
    Task<TodoDb?> UpdateAsync(int id, UpdateTodoDto dto);
    Task<bool> DeleteAsync(int id);
}