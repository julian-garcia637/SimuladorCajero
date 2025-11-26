using SimuladorCajero.Models;
using SimuladorCajero.Data.Context;
using System.ComponentModel.DataAnnotations;

namespace SimuladorCajero.Models
{
    public class Corriente : Cuenta
    {
        // Por defecto, sobregiro del 20% (0.2)
        [Display(Name = "Cupo de sobregiro")]
        [Range(0, 1, ErrorMessage = "El cupo debe estar entre 0 y 100%.")]
        public double CupoSobregiro { get; set; } = 0.2;

        /// <summary>
        /// Realiza un retiro permitiendo sobregiro.
        /// Registra la transacción correspondiente.
        /// </summary>
        /// <param name="monto">Monto a retirar</param>
        /// <param name="context">DbContext para persistencia</param>
        /// <returns>Mensaje de resultado</returns>
        public string Retirar(double monto, AppDbContext context)
        {
            double limiteDisponible = Saldo + (Saldo * CupoSobregiro);

            if (monto <= 0)
            {
                return "El monto a retirar no es válido.";
            }
            else if (monto > limiteDisponible)
            {
                return "El monto a retirar excede el límite disponible incluyendo sobregiro.";
            }
            else
            {
                Saldo -= monto;
                var transRetiro = new Transaccion
                {
                    Fecha = DateTime.Now,
                    Concepto = "Retiro",
                    Valor = -monto,
                    CuentaId = this.CuentaId
                };
                Transacciones.Add(transRetiro);
                context.Transacciones.Add(transRetiro);
                context.SaveChanges();

                return "La transacción se hizo de forma exitosa.";
            }
        }
    }
}