namespace GestionHuacales.Api9.DTO;

public class EntradasHuacalesDTO
{
    public string NombreCliente { get; set; }

    public EntradasHuacalesDetallesDTO[] Huacales { get; set; }
}
