using System.ComponentModel.DataAnnotations;

namespace GestionHuacales.Api9.DTO;

public class TiposHuacalesCrearDTO
{
    [Required(ErrorMessage = "La descripción es requerida")]
    public string Descripcion { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "La existencia no puede ser inferior a 0")]
    public int Existencia { get; set; }
}
