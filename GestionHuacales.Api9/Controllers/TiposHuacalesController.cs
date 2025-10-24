using GestionHuacales.Api9.DTO;
using GestionHuacales.Api9.Models;
using GestionHuacales.Api9.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestionHuacales.Api9.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TiposHuacalesController(TiposHuacalesServices tiposHuacalesService) : ControllerBase
{
    private TiposHuacalesDTO MapearADTO(TiposHuacales tipoHuacal)
    {
        return new TiposHuacalesDTO
        {
            TipoId = tipoHuacal.TipoId,
            Descripcion = tipoHuacal.Descripcion,
            Existencia = tipoHuacal.Existencia
        };
    }

    // GET: api/<TiposHuacalesController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TiposHuacalesDTO>>> Get()
    {
        var listaEntidades = await tiposHuacalesService.Listar(h => true);
        var listaDTO = listaEntidades.Select(MapearADTO).ToList();
        return Ok(listaDTO);
    }

    // GET api/<TiposHuacalesController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TiposHuacalesDTO>> Get(int id)
    {
        var tipoHuacal = await tiposHuacalesService.Buscar(id);

        if (tipoHuacal == null)
        {
            return NotFound($"Tipo de Huacal con Id: {id} no encontrado.");
        }

        return Ok(MapearADTO(tipoHuacal));
    }

    // POST api/<TiposHuacalesController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TiposHuacalesCrearDTO tipoHuacalDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var nuevoTipo = new TiposHuacales
        {
            Descripcion = tipoHuacalDTO.Descripcion,
            Existencia = tipoHuacalDTO.Existencia
        };

        var guardado = await tiposHuacalesService.Guardar(nuevoTipo);

        if (!guardado)
        {
            return StatusCode(500, "Error al intentar guardar el Tipo de Huacal.");
        }

        return CreatedAtAction(nameof(Get), new { id = nuevoTipo.TipoId }, MapearADTO(nuevoTipo));
    }

    // PUT api/<TiposHuacalesController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] TiposHuacalesCrearDTO tipoHuacalDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var tipoAActualizar = new TiposHuacales
        {
            TipoId = id,
            Descripcion = tipoHuacalDTO.Descripcion,
            Existencia = tipoHuacalDTO.Existencia
        };

        var modificado = await tiposHuacalesService.Guardar(tipoAActualizar);

        if (!modificado)
        {
            if (await tiposHuacalesService.Buscar(id) == null)
            {
                return NotFound($"Tipo de Huacal con Id: {id} no encontrado para actualizar.");
            }
            return StatusCode(500, "Error al intentar modificar el Tipo de Huacal.");
        }

        return NoContent();
    }

    // DELETE api/<TiposHuacalesController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await tiposHuacalesService.Eliminar(id);

        if (!eliminado)
        {
            return NotFound($"Tipo de Huacal con Id: {id} no encontrado para eliminar.");
        }

        return NoContent();
    }
}