using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimuladorCajero.Models
{
    public class Transaccion
    {
        [Key]
        public int TransaccionId { get; set; } // PK

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha del movimiento")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Concepto")]
        [StringLength(120, ErrorMessage = "El concepto no debe superar los 120 caracteres.")]
        public string Concepto { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Valor")]
        public double Valor { get; set; }

        // 🔗 Relación con Cuenta (1:N)
        [Required]
        [Display(Name = "Cuenta asociada")]
        public int CuentaId { get; set; }

        public Cuenta Cuenta { get; set; } = default!;
    }
}