// using api.Model.Dtos.Todo;
// using api.Model.Entities;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.VisualBasic;

// namespace api.Data.Repository.Todo;

// public class TodoRepository(ApplicationDbContext db) : ITodoRepository
// {
//     private readonly ApplicationDbContext _db = db;

//     public async Task<int> CreateAsync(CreateTodoDto todo)
//     {
//         _db.Todo.Add(new TodoDb(todo.Description));
//         var affectedRows = await _db.SaveChangesAsync();
//         return affectedRows;
//     }

//     public async Task<List<TodoDb>> GetAllAsync(string? type, string? value)
//     {
//         if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(value))
//         {
//             return await _db.Todo.ToListAsync();
//         }

//         if (Strings.Equals(type.ToLower(), "id"))
//         {
//             var todo = await _db.Todo.FindAsync(value);
//             return todo != null ? [todo] : [];
//         }

//         if (Strings.Equals(type.ToLower(), "description"))
//         {
//             var todos = await _db.Todo
//                 .Where(t => EF.Functions.Like(t.Description, $"%{value}%"))
//                 .ToListAsync();
//             return todos;
//         }

//         return await _db.Todo.ToListAsync();
//     }

//     public async Task<TodoDb?> GetByIdAsync(Guid id)
//     {
//         return await _db.Todo.FindAsync(Convert.ToString(id));
//     }

//     public async Task<int> UpdateAsync(Guid id, UpdateTodoDto todo)
//     {
//         var UpdatedRows = -1;
//         var exists = await _db.Todo.FindAsync(Convert.ToString(id));
//         if (exists != null)
//         {
//             var savedDate = exists.Completed;
//             var savedDescription = exists.Description;
//             exists.Description = !string.IsNullOrEmpty(todo.Description) ? todo.Description : exists.Description;
//             if (todo.Completed.HasValue)
//             {
//                 exists.Completed = todo.Completed.Value 
//                     ? DateOnly.FromDateTime(DateTime.Now) 
//                     : null;
//             }
//             UpdatedRows = await _db.SaveChangesAsync();

//             // Quando não há mudanças, SaveChangesAsync retorna 0
//             if (savedDate == exists.Completed && savedDescription == exists.Description)
//             {
//                 UpdatedRows = 1;
//             }
//         }

//         return UpdatedRows;
//     }
    
//     public async Task<int> DeleteAsync(Guid id)
//     {
//         var DeletedRows = -1;
//         var todo = await _db.Todo.FindAsync(Convert.ToString(id));
//         if (todo != null)
//         {
//             _db.Todo.Remove(todo);
//             DeletedRows = await _db.SaveChangesAsync();
//         }

//         return DeletedRows;
//     }
// }