using SimuladorCajero.Models;
using System.ComponentModel.DataAnnotations;

namespace SimuladorCajero.Models
{
    public class Ahorros : Cuenta
    {
        // Por defecto, interés mensual del 1.5% (0.015)
        [Display(Name = "Interés mensual")]
        [Range(0, 1, ErrorMessage = "El interés debe estar entre 0% y 100%.")]
        public double InteresMensual { get; set; } = 0.015;

        /// <summary>
        /// Realiza un retiro aplicando el interés mensual antes de retirar.
        /// Registra las transacciones correspondientes.
        /// </summary>
        /// <param name="monto">Monto a retirar</param>
        /// <param name="context">DbContext para persistencia</param>
        /// <returns>Mensaje de resultado</returns>
        public string Retirar(double monto, Data.Context.AppDbContext context)
        {
            if (monto <= 0)
            {
                return "El monto a retirar no es válido.";
            }
            else if (monto > Saldo)
            {
                return "No hay saldo suficiente para realizar el retiro.";
            }
            else
            {
                // Calcular y aplicar interés mensual
                double interesGanado = Saldo * InteresMensual;
                Saldo += interesGanado;

                if (interesGanado > 0)
                {
                    var transInteres = new Transaccion
                    {
                        Fecha = DateTime.Now,
                        Concepto = "Interés Ganado",
                        Valor = interesGanado,
                        CuentaId = this.CuentaId
                    };
                    Transacciones.Add(transInteres);
                    context.Transacciones.Add(transInteres);
                }

                // Realizar el retiro
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