using api.Model.Dtos.Todo;
using api.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Repository.Todo;

public class TodoRepository(ApplicationDbContext db) : ITodoRepository
{
    private readonly ApplicationDbContext _db = db;

    public async Task<TodoDb> CreateAsync(CreateTodoDto dto)
    {
        var todo = new TodoDb(dto.Tipo, dto.DataLimite, dto.HortaId)
        {
            Descricao = dto.Descricao,
            PlantioId = dto.PlantioId,
            MembroId = dto.MembroId
        };

        _db.Todos.Add(todo);
        await _db.SaveChangesAsync();
        return todo;
    }

    public async Task<TodoDb?> GetByIdAsync(int id)
    {
        return await _db.Todos
            .Include(t => t.Horta)
            .Include(t => t.Plantio).ThenInclude(p => p!.Especie)
            .Include(t => t.Membro).ThenInclude(m => m!.Profile)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<TodoDb>> GetAllAsync()
    {
        return await _db.Todos
            .Include(t => t.Horta)
            .Include(t => t.Plantio).ThenInclude(p => p!.Especie)
            .Include(t => t.Membro).ThenInclude(m => m!.Profile)
            .OrderBy(t => t.DataLimite)
            .ToListAsync();
    }

    public async Task<TodoDb?> UpdateAsync(int id, UpdateTodoDto dto)
    {
        var todo = await _db.Todos.FindAsync(id);
        if (todo == null) return null;

        if (dto.Tipo.HasValue) todo.Tipo = dto.Tipo.Value;
        if (dto.Descricao != null) todo.Descricao = dto.Descricao;
        if (dto.DataLimite.HasValue) todo.DataLimite = dto.DataLimite.Value;
        if (dto.PlantioId.HasValue) todo.PlantioId = dto.PlantioId.Value;
        if (dto.MembroId.HasValue) todo.MembroId = dto.MembroId.Value;
        if (dto.CompletedAt.HasValue) todo.CompletedAt = dto.CompletedAt.Value;
        if (dto.Status.HasValue) todo.Status = dto.Status.Value;

        await _db.SaveChangesAsync();
        return todo;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var todo = await _db.Todos.FindAsync(id);
        if (todo == null) return false;

        _db.Todos.Remove(todo);
        await _db.SaveChangesAsync();
        return true;
    }
}