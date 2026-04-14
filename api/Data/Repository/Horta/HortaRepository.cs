using api.Model.Dtos.Horta;
using api.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Repository.Horta;

public class HortaRepository(ApplicationDbContext db) : IHortaRepository
{
    private readonly ApplicationDbContext _db = db;

    public async Task<HortaDb> CreateAsync(CreateHortaDto dto)
    {
        var horta = new HortaDb(dto.Nome, dto.Local, dto.Largura, dto.Profundidade)
        {
            Descricao = dto.Descricao
        };
        
        _db.Hortas.Add(horta);
        await _db.SaveChangesAsync();
        return horta;
    }

    public async Task<HortaResumoDto?> GetByIdAsync(int id)
    {
        return await _db.Hortas
            .Where(h => h.Id == id)
            .Select(h => new HortaResumoDto
            {
                Id = h.Id,
                Nome = h.Nome,
                Descricao = h.Descricao,
                Local = h.Local,
                Largura = h.Largura,
                Profundidade = h.Profundidade,
                Status = h.Status,
                CreatedAt = h.CreatedAt,
                CountMembros = _db.Membros.Count(m => m.HortaId == h.Id),
                CountPlantios = _db.Plantios.Count(p => p.HortaId == h.Id)
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<HortaResumoDto>> GetAllAsync()
    {
        return await _db.Hortas
            .Select(h => new HortaResumoDto
            {
                Id = h.Id,
                Nome = h.Nome,
                Descricao = h.Descricao,
                Local = h.Local,
                Largura = h.Largura,
                Profundidade = h.Profundidade,
                Status = h.Status,
                CreatedAt = h.CreatedAt,
                CountMembros = _db.Membros.Count(m => m.HortaId == h.Id),
                CountPlantios = _db.Plantios.Count(p => p.HortaId == h.Id)
            })
            .ToListAsync();
    }

    public async Task<HortaDb?> UpdateAsync(int id, UpdateHortaDto dto)
    {
        var horta = await _db.Hortas.FindAsync(id);
        if (horta == null) return null;

        if (dto.Nome != null) horta.Nome = dto.Nome;
        if (dto.Descricao != null) horta.Descricao = dto.Descricao;
        if (dto.Local != null) horta.Local = dto.Local;
        if (dto.Largura.HasValue) horta.Largura = dto.Largura.Value;
        if (dto.Profundidade.HasValue) horta.Profundidade = dto.Profundidade.Value;
        if (dto.Status.HasValue) horta.Status = dto.Status.Value;

        await _db.SaveChangesAsync();
        return horta;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var horta = await _db.Hortas.FindAsync(id);
        if (horta == null) return false;

        _db.Hortas.Remove(horta);
        await _db.SaveChangesAsync();
        return true;
    }
}
