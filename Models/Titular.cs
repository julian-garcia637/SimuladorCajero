using SimuladorCajero.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Titular
{
    [Key]
    public int TitularId { get; set; }

    [Required(ErrorMessage = "El usuario es obligatorio")]
    [Display(Name = "Usuario")]
    [StringLength(30, MinimumLength = 4)]
    public string Usuario { get; set; } = default!;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [Display(Name = "Contraseña")]
    [DataType(DataType.Password)]
    [StringLength(80, MinimumLength = 6)]
    public string Contraseña { get; set; } = default!;

    [Required(ErrorMessage = "El nombre completo es obligatorio")]
    [Display(Name = "Nombre completo")]
    [StringLength(90, MinimumLength = 3)]
    public string Nombre { get; set; } = default!;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress]
    [Display(Name = "Correo electrónico")]
    [StringLength(120)]
    public string Email { get; set; } = default!;

    // Relación 1:1 con Cuenta
    [Required]
    [ForeignKey("TitularId")]
    [Display(Name = "Cuenta principal")]
    public Cuenta Cuenta { get; set; } = default!;

    // Relación 1:1 con Tarjeta de Crédito
    public TarjetaCredito TarjetaCredito { get; set; } = default!;

    public Titular()
    { }
}