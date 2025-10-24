using GestionHuacales.Api9.DATA;
using GestionHuacales.Api9.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionHuacales.Api9.Services;

public class TiposHuacalesServices(IDbContextFactory<Contexto> factory)
{
    public async Task<TiposHuacales?> Buscar(int tipoId)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.TiposHuacales
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TipoId == tipoId);
    }

    public async Task<List<TiposHuacales>> Listar(Expression<Func<TiposHuacales, bool>> criterio)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.TiposHuacales
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    private async Task<bool> Existe(int id)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.TiposHuacales.AnyAsync(t => t.TipoId == id);
    }

    public async Task<bool> Guardar(TiposHuacales tipoHuacal)
    {
        await using var contexto = await factory.CreateDbContextAsync();

        if (tipoHuacal.TipoId == 0)
        {
            contexto.TiposHuacales.Add(tipoHuacal);
        }
        else
        {
            if (!await Existe(tipoHuacal.TipoId))
            {
                return false;
            }
            contexto.TiposHuacales.Update(tipoHuacal);
            contexto.Entry(tipoHuacal).State = EntityState.Modified;
        }
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await factory.CreateDbContextAsync();

        var tipoHuacal = await contexto.TiposHuacales.FindAsync(id);

        if (tipoHuacal == null)
        {
            return false;
        }

        contexto.TiposHuacales.Remove(tipoHuacal);

        return await contexto.SaveChangesAsync() > 0;
    }

}
