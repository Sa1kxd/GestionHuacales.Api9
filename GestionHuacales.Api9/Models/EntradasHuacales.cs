using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionHuacales.Api9.Models;

public class EntradasHuacales
{
    [Key]
    public int IdEntrada { get; set; }

    [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
    public int Cantidad { get; set; }

    [Required(ErrorMessage = "Fecha es requerida")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Nombre es requerido")]
    [StringLength(30, ErrorMessage = "El nombre no puede contener mas de 30 caracteres")]
    public String NombreCliente { get; set; }

    [Required(ErrorMessage = "Precion requerido")]
    public double Precio { get; set; }

    [InverseProperty("EntradaHuacal")]
    public virtual ICollection<EntradasHuacalesDetalle> EntradaHuacalDetalle { get; set; } = new List<EntradasHuacalesDetalle>();
}
