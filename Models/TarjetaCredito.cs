using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimuladorCajero.Models
{
    public class TarjetaCredito
    {
        [Key]
        public int TarjetaCreditoId { get; set; } // PK

        [Required]
        [Display(Name = "Número de tarjeta")]
        [CreditCard]
        [StringLength(19, MinimumLength = 12)]
        public string NumeroTarjeta { get; set; } = null!;

        [Required]
        [Display(Name = "Clave de cajero")]
        [StringLength(4, MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Clave { get; set; } = null!;

        [Required]
        [Display(Name = "Cupo asignado")]
        [Range(0, 100000000, ErrorMessage = "El cupo debe ser positivo")]
        public double Cupo { get; set; } = 10000000;

        [Required]
        [Display(Name = "Deuda actual")]
        [Range(0, 100000000, ErrorMessage = "La deuda no puede ser negativa")]
        public double Deuda { get; set; } = 0;

        // 🔗 Relación 1:1 con Usuario (única tarjeta por usuario)
        [Required]
        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; } = null!;
    }
}