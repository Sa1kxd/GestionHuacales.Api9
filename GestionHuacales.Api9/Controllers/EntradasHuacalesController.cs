using GestionHuacales.Api9.DTO;
using GestionHuacales.Api9.Models;
using GestionHuacales.Api9.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionHuacales.Api9.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EntradasHuacalesController (EntradasHuacalesService entradasHuacalesService) : ControllerBase
{
    // GET: api/<EntradasHuacalesController>
    [HttpGet]
    public async Task<ActionResult<EntradasHuacalesDTO[]>> Get()
    {
        var lista = await entradasHuacalesService.Listar(h => true);

        return Ok(lista.Select(e => new EntradasHuacalesDTO
        {
            NombreCliente = e.NombreCliente,
            Huacales = e.EntradaHuacalDetalle.Select(d => new EntradasHuacalesDetallesDTO
            {
                TipoId = d.TipoId,
                Cantidad = d.Cantidad,
                Precio = d.Precio
            }).ToArray()
        }).ToArray());
    }

    // GET api/<EntradasHuacalesController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<EntradasHuacalesDTO>> Get(int id)
    {
        var entrada = await entradasHuacalesService.Buscar(id);

        if (entrada == null)
        {
            return NotFound($"No se encontró una entrada de huacales con Id: {id}");
        }

        var entradaDTO = new EntradasHuacalesDTO
        {
            NombreCliente = entrada.NombreCliente,
            Huacales = entrada.EntradaHuacalDetalle.Select(d => new EntradasHuacalesDetallesDTO
            {
                TipoId = d.TipoId,
                Cantidad = d.Cantidad,
                Precio = d.Precio
            }).ToArray()
        };

        return Ok(entradaDTO);
    }

    // POST api/<EntradasHuacalesController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] EntradasHuacalesDTO entradaHuacal)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var entrada = new EntradasHuacales
        {
            Fecha = DateTime.Now,
            NombreCliente = entradaHuacal.NombreCliente,
            EntradaHuacalDetalle = entradaHuacal.Huacales.Select(h => new EntradasHuacalesDetalle
            {
                TipoId = h.TipoId,
                Cantidad = h.Cantidad,
                Precio = h.Precio
            }).ToArray()
        };
        var guardado = await entradasHuacalesService.Guardar(entrada);

        if (!guardado)
        {
            return StatusCode(500, "Error al intentar guardar la entrada de huacales.");
        }

        return NoContent();
    }

    // PUT api/<EntradasHuacalesController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] EntradasHuacalesDTO entradaHuacal)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var entradaExistente = await entradasHuacalesService.Buscar(id);
        if (entradaExistente == null)
        {
            return NotFound($"No se encontró una entrada de huacales con Id: {id} para actualizar.");
        }

        var entrada = new EntradasHuacales
        {
            IdEntrada = id,
            Fecha = entradaExistente.Fecha,
            NombreCliente = entradaHuacal.NombreCliente,
            EntradaHuacalDetalle = entradaHuacal.Huacales.Select(h => new EntradasHuacalesDetalle
            {
                IdEntrada = id,
                TipoId = h.TipoId,
                Cantidad = h.Cantidad,
                Precio = h.Precio
            }).ToArray()
        };

        var modificado = await entradasHuacalesService.Guardar(entrada);

        if (!modificado)
        {
            return StatusCode(500, "Error al intentar modificar la entrada de huacales.");
        }

        return NoContent();
    }

    // DELETE api/<EntradasHuacalesController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await entradasHuacalesService.Eliminar(id);

        if (!eliminado)
        {
            var existe = await entradasHuacalesService.Buscar(id);
            if (existe == null)
            {
                return NotFound($"No se encontró una entrada de huacales con Id: {id} para eliminar.");
            }
            return StatusCode(500, "Error al intentar eliminar la entrada de huacales.");
        }

        return NoContent();
    }
}
