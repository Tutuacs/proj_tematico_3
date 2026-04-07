using api.Model.Dtos.Plantio;
using api.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Repository.Plantio;

public class PlantioRepository(ApplicationDbContext db) : IPlantioRepository
{
    private readonly ApplicationDbContext _db = db;

    public async Task<PlantioDb> CreateAsync(CreatePlantioDto dto)
    {
        var plantio = new PlantioDb(dto.EspecieId, dto.HortaId, dto.DataPlantio)
        {
            Quantidade = dto.Quantidade
        };

        _db.Plantios.Add(plantio);
        await _db.SaveChangesAsync();
        return plantio;
    }

    public async Task<PlantioDb?> GetByIdAsync(int id)
    {
        return await _db.Plantios
            .Include(p => p.Especie)
            .Include(p => p.Horta)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<PlantioDb>> GetAllAsync()
    {
        return await _db.Plantios
            .Include(p => p.Especie)
            .Include(p => p.Horta)
            .ToListAsync();
    }

    public async Task<List<PlantioDb>> GetByHortaIdAsync(int hortaId)
    {
        return await _db.Plantios
            .Include(p => p.Especie)
            .Include(p => p.Horta)
            .Where(p => p.HortaId == hortaId)
            .ToListAsync();
    }

    public async Task<PlantioDb?> UpdateAsync(int id, UpdatePlantioDto dto)
    {
        var plantio = await _db.Plantios.FindAsync(id);
        if (plantio == null) return null;

        if (dto.EspecieId.HasValue) plantio.EspecieId = dto.EspecieId.Value;
        if (dto.HortaId.HasValue) plantio.HortaId = dto.HortaId.Value;
        if (dto.DataPlantio.HasValue) plantio.DataPlantio = dto.DataPlantio.Value;
        if (dto.Quantidade.HasValue) plantio.Quantidade = dto.Quantidade.Value;
        if (dto.Status.HasValue) plantio.Status = dto.Status.Value;

        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var plantio = await _db.Plantios.FindAsync(id);
        if (plantio == null) return false;

        _db.Plantios.Remove(plantio);
        await _db.SaveChangesAsync();
        return true;
    }
}