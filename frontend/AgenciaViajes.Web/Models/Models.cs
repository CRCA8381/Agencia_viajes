using System.ComponentModel.DataAnnotations;

namespace AgenciaViajes.Web.Models;

public class DestinoDto
{
    public int DestinoId { get; set; }
    [Required, StringLength(80)] public string Pais { get; set; } = "";
    [Required, StringLength(100)] public string Ciudad { get; set; } = "";
    [Required, StringLength(300, MinimumLength=5)] public string Descripcion { get; set; } = "";
    [Required, StringLength(80)] public string Temporada { get; set; } = "";
}
public class PaqueteDto
{
    public int PaqueteId { get; set; }
    [Range(1,int.MaxValue)] public int DestinoId { get; set; }
    [Required, StringLength(150)] public string Nombre { get; set; } = "";
    [Range(0.01,99999999)] public decimal Precio { get; set; }
    [Range(1,365)] public int Duracion { get; set; }
    public string? DestinoNombre { get; set; }
}
public class ClienteDto
{
    public int ClienteId { get; set; }
    [Required, StringLength(120)] public string Nombre { get; set; } = "";
    [Required, StringLength(30)] public string Documento { get; set; } = "";
    [Required, StringLength(20)] public string Telefono { get; set; } = "";
    [Required, EmailAddress, StringLength(150)] public string Email { get; set; } = "";
}
public class ReservaDto
{
    public int ReservaId { get; set; }
    [Range(1,int.MaxValue)] public int ClienteId { get; set; }
    [Range(1,int.MaxValue)] public int PaqueteId { get; set; }
    public DateTime FechaReserva { get; set; } = DateTime.Today.AddDays(1);
    [Required] public string Estado { get; set; } = "Pendiente";
    public string? ClienteNombre { get; set; }
    public string? PaqueteNombre { get; set; }
}
