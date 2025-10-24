using GestionHuacales.Api9.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using GestionHuacales.Api9.DATA;

namespace GestionHuacales.Api9.Services;

public class EntradasHuacalesService(IDbContextFactory<Contexto> factory)
{
    private enum TipoOperacion
    {
        Suma = 1,
        Resta = 2
    }

    public async Task<bool> Guardar(EntradasHuacales huacales)
    {
        await using var contexto = await factory.CreateDbContextAsync();

        if (!await Existe(huacales.IdEntrada))
        {
            return await Insertar(huacales);
        }
        else
        {
            return await Modificar(huacales);
        }
    }

    private async Task AfectarExistencia(EntradasHuacalesDetalle[] detalles, TipoOperacion tipoOperacion)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        foreach (var item in detalles)
        {
            var tipoHuacal = await contexto.TiposHuacales.SingleAsync(t => t.TipoId == item.TipoId);

            if (tipoOperacion == TipoOperacion.Suma)
            {
                tipoHuacal.Existencia += item.Cantidad;
            }
            else
            {
                tipoHuacal.Existencia -= item.Cantidad;
            }
        }

        await contexto.SaveChangesAsync();
    }

    private async Task<bool> Insertar(EntradasHuacales huacal)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        contexto.EntradasHuacales.Add(huacal);
        await AfectarExistencia(huacal.EntradaHuacalDetalle.ToArray(), TipoOperacion.Suma);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(EntradasHuacales huacal)
    {
        await using var contexto = await factory.CreateDbContextAsync();

        var detallesViejos = await contexto.EntradasHuacalesDetalle
            .AsNoTracking()
            .Where(e => e.IdEntrada == huacal.IdEntrada)
            .ToArrayAsync();

        await AfectarExistencia(detallesViejos, TipoOperacion.Resta);

        await contexto.EntradasHuacalesDetalle
            .Where(d => d.IdEntrada == huacal.IdEntrada)
            .ExecuteDeleteAsync();

        contexto.EntradasHuacales.Attach(huacal);
        contexto.Entry(huacal).State = EntityState.Modified;

        foreach (var detalle in huacal.EntradaHuacalDetalle)
        {
            contexto.Entry(detalle).State = EntityState.Added;
        }

        await AfectarExistencia(huacal.EntradaHuacalDetalle.ToArray(), TipoOperacion.Suma);

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Existe(int id)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.EntradasHuacales.AnyAsync(e => e.IdEntrada == id);
    }

    public async Task<EntradasHuacales?> Buscar(int id)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.EntradasHuacales
            .Include(e => e.EntradaHuacalDetalle)
                .ThenInclude(d => d.TipoHuacal)
            .AsNoTracking().FirstOrDefaultAsync(e => e.IdEntrada == id);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        var huacal = await contexto.EntradasHuacales
            .Include(e => e.EntradaHuacalDetalle)
            .FirstOrDefaultAsync(e => e.IdEntrada == id);

        if (huacal == null) return false;

        await AfectarExistencia(huacal.EntradaHuacalDetalle.ToArray(), TipoOperacion.Resta);

        contexto.EntradasHuacalesDetalle.RemoveRange(huacal.EntradaHuacalDetalle);
        contexto.EntradasHuacales.Remove(huacal);

        var cantidad = await contexto.SaveChangesAsync();
        return cantidad > 0;
    }

    public async Task<List<EntradasHuacales>> Listar(Expression<Func<EntradasHuacales, bool>> criterio)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.EntradasHuacales
            .Include(e => e.EntradaHuacalDetalle)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}
