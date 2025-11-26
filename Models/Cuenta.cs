using SimuladorCajero.Models;
using System.ComponentModel.DataAnnotations;

public abstract class Cuenta
{
    [Key]
    public int CuentaId { get; set; }

    [Required]
    [Display(Name = "Saldo actual")]
    public double Saldo { get; set; } = 0;

    // FK obligatoria → 1:1 con Usuario
    [Required]
    [Display(Name = "Titular")]
    public int UsuarioId { get; set; }

    public Usuario Usuario { get; set; } = default!;

    // 🔗 Relación 1:N con Transacciones
    public List<Transaccion> Transacciones { get; set; } = new();
}