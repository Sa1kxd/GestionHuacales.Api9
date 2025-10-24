using System.ComponentModel.DataAnnotations;

namespace GestionHuacales.Api9.DTO;

public class TiposHuacalesDTO
{
    public int TipoId { get; set; }

    [Required(ErrorMessage = "La descripción es requerida")]
    public string Descripcion { get; set; }

    public int Existencia { get; set; }
}
