using api.Model.Dtos.Especie;
using api.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Repository.Especie;

public class EspecieRepository(ApplicationDbContext db) : IEspecieRepository
{
    private readonly ApplicationDbContext _db = db;

    public async Task<EspecieDb> CreateAsync(CreateEspecieDto dto)
    {
        var especie = new EspecieDb(dto.Nome, dto.DiasParaRegar)
        {
            ImageLink = dto.ImageLink,
            DiasParaColher = dto.DiasParaColher,
            MesesPlantio = dto.MesesPlantio,
            MesesColheita = dto.MesesColheita,
            Descricao = dto.Descricao
        };

        _db.Especies.Add(especie);
        await _db.SaveChangesAsync();
        return especie;
    }

    public async Task<EspecieDb?> GetByIdAsync(int id)
    {
        return await _db.Especies.FindAsync(id);
    }

    public async Task<List<EspecieDb>> GetAllAsync()
    {
        return await _db.Especies.ToListAsync();
    }

    public async Task<EspecieDb?> UpdateAsync(int id, UpdateEspecieDto dto)
    {
        var especie = await _db.Especies.FindAsync(id);
        if (especie == null) return null;

        if (dto.Nome != null) especie.Nome = dto.Nome;
        if (dto.ImageLink != null) especie.ImageLink = dto.ImageLink;
        if (dto.DiasParaRegar.HasValue) especie.DiasParaRegar = dto.DiasParaRegar.Value;
        if (dto.DiasParaColher.HasValue) especie.DiasParaColher = dto.DiasParaColher.Value;
        if (dto.MesesPlantio != null) especie.MesesPlantio = dto.MesesPlantio;
        if (dto.MesesColheita != null) especie.MesesColheita = dto.MesesColheita;
        if (dto.Descricao != null) especie.Descricao = dto.Descricao;

        await _db.SaveChangesAsync();
        return especie;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var especie = await _db.Especies.FindAsync(id);
        if (especie == null) return false;

        _db.Especies.Remove(especie);
        await _db.SaveChangesAsync();
        return true;
    }
}