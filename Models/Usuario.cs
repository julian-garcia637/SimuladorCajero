using SimuladorCajero.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Usuario
{
    [Key]
    public int UsuarioId { get; set; }

    [Required(ErrorMessage = "El usuario es obligatorio")]
    [Display(Name = "Nombre de usuario")]
    [StringLength(30, MinimumLength = 4, ErrorMessage = "El usuario debe tener entre 4 y 30 caracteres.")]
    public string NombreUsuario { get; set; } = default!;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [Display(Name = "Contraseña")]
    [StringLength(80, MinimumLength = 6, ErrorMessage = "La contraseña debe tener mínimo 6 caracteres.")]
    [DataType(DataType.Password)]
    public string Contraseña { get; set; } = default!;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [Display(Name = "Nombre completo")]
    [StringLength(80, MinimumLength = 3)]
    public string Nombre { get; set; } = default!;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Ingresa un correo válido")]
    [Display(Name = "Correo electrónico")]
    [StringLength(120)]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "El celular es obligatorio")]
    [Display(Name = "Celular")]
    [Phone(ErrorMessage = "Ingresa un número válido")]
    [StringLength(20)]
    [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessage = "Solo números, con o sin + inicial.")]
    public string Celular { get; set; } = default!;

    // Relación 1:1 con Cuenta
    [ForeignKey("UsuarioId")]
    public Cuenta? Cuenta { get; set; } = default!;

    // Relación 1:1 con Tarjeta de Crédito
    public TarjetaCredito? TarjetaCredito { get; set; }
}