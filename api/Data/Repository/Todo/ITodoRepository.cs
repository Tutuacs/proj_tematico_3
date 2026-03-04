// using api.Model.Dtos.Todo;
// using api.Model.Entities;

// namespace api.Data.Repository.Todo;

// public interface ITodoRepository
// {
//     Task<int> CreateAsync(CreateTodoDto todo);
//     Task<List<TodoDb>> GetAllAsync(string? type, string? value);
//     Task<TodoDb?> GetByIdAsync(Guid id);
//     Task<int> UpdateAsync(Guid id, UpdateTodoDto todo);
//     Task<int> DeleteAsync(Guid id);
// }