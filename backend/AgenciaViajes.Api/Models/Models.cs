using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenciaViajes.Api.Models;

[Table("destinos")]
public class Destino
{
    [Key, Column("destino_id")] public int DestinoId { get; set; }
    [Required, MaxLength(80), Column("pais")] public string Pais { get; set; } = "";
    [Required, MaxLength(100), Column("ciudad")] public string Ciudad { get; set; } = "";
    [Required, MaxLength(300), Column("descripcion")] public string Descripcion { get; set; } = "";
    [Required, MaxLength(80), Column("temporada")] public string Temporada { get; set; } = "";
    public ICollection<Paquete> Paquetes { get; set; } = new List<Paquete>();
}

[Table("paquetes")]
public class Paquete
{
    [Key, Column("paquete_id")] public int PaqueteId { get; set; }
    [Column("destino_id")] public int DestinoId { get; set; }
    [Required, MaxLength(150), Column("nombre")] public string Nombre { get; set; } = "";
    [Column("precio", TypeName="numeric(12,2)")] public decimal Precio { get; set; }
    [Range(1,365), Column("duracion")] public int Duracion { get; set; }
    public Destino? Destino { get; set; }
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}

[Table("clientes")]
public class Cliente
{
    [Key, Column("cliente_id")] public int ClienteId { get; set; }
    [Required, MaxLength(120), Column("nombre")] public string Nombre { get; set; } = "";
    [Required, MaxLength(30), Column("documento")] public string Documento { get; set; } = "";
    [Required, MaxLength(20), Column("telefono")] public string Telefono { get; set; } = "";
    [Required, EmailAddress, MaxLength(150), Column("email")] public string Email { get; set; } = "";
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}

[Table("reservas")]
public class Reserva
{
    [Key, Column("reserva_id")] public int ReservaId { get; set; }
    [Column("cliente_id")] public int ClienteId { get; set; }
    [Column("paquete_id")] public int PaqueteId { get; set; }
    [Column("fecha_reserva")] public DateTime FechaReserva { get; set; }
    [Required, MaxLength(30), Column("estado")] public string Estado { get; set; } = "Pendiente";
    public Cliente? Cliente { get; set; }
    public Paquete? Paquete { get; set; }
}
