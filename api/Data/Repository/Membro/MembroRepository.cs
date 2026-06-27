using api.Enums;
using api.Model.Dtos.Membro;
using api.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Repository.Membro;

public class MembroRepository(ApplicationDbContext db) : IMembroRepository
{
    private readonly ApplicationDbContext _db = db;

    public async Task<MembroDb> AddAsync(AddMembroDto dto)
    {
        var membro = new MembroDb(dto.HortaId, dto.PerfilId, dto.Role);
        
        _db.Membros.Add(membro);
        await _db.SaveChangesAsync();
        return membro;
    }

    public async Task<MembroDb?> GetByIdAsync(int id)
    {
        return await _db.Membros
            .Include(m => m.Horta)
            .Include(m => m.Profile)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<List<MembroDb>> GetByHortaIdAsync(int hortaId)
    {
        return await _db.Membros
            .Include(m => m.Profile)
            .Where(m => m.HortaId == hortaId)
            .ToListAsync();
    }

    public async Task<List<MembroDb>> GetByPerfilIdAsync(string perfilId)
    {
        return await _db.Membros
            .Include(m => m.Horta)
            .Where(m => m.PerfilId == perfilId)
            .ToListAsync();
    }

    public async Task<MembroDb?> UpdateAsync(int id, UpdateMembroDto dto)
    {
        var membro = await _db.Membros.FindAsync(id);
        if (membro == null) return null;

        if (dto.Role.HasValue) membro.Role = dto.Role.Value;

        await _db.SaveChangesAsync();
        return membro;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var membro = await _db.Membros.FindAsync(id);
        if (membro == null) return false;

        _db.Membros.Remove(membro);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsMemberOfHortaAsync(int hortaId, string perfilId)
    {
        return await _db.Membros.AnyAsync(m => m.HortaId == hortaId && m.PerfilId == perfilId);
    }

    public async Task<bool> IsAdminInAnyHortaAsync(string perfilId)
    {
        return await _db.Membros.AnyAsync(m => m.PerfilId == perfilId && m.Role == ROLE.Admin);
    }

    public async Task<MembroDb?> GetByHortaAndPerfilAsync(int hortaId, string perfilId)
    {
        return await _db.Membros
            .FirstOrDefaultAsync(m => m.HortaId == hortaId && m.PerfilId == perfilId);
    }

    public async Task<List<ProfileDb>> GetAvailableProfilesForHortaAsync(int hortaId)
    {
        return await _db.Profiles
            .Where(p => !_db.Membros.Any(m => m.HortaId == hortaId && m.PerfilId == p.Id))
            .OrderBy(p => p.Name)
            .ToListAsync();
    }
}
