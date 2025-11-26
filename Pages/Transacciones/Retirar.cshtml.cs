using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimuladorCajero.Data.Context;
using SimuladorCajero.Models;

public class RetirarModel : PageModel
{
    private readonly AppDbContext _context;
    public RetirarModel(AppDbContext context) => _context = context;

    [BindProperty]
    public double Monto { get; set; }

    public string? Mensaje { get; set; }
    public double? NuevoSaldo { get; set; }

    public int? UsuarioAutenticado { get; set; }

    public IActionResult OnGet()
    {
        UsuarioAutenticado = HttpContext.Session.GetInt32("UsuarioId");
        if (UsuarioAutenticado == null)
        {
            return RedirectToPage("/Account/Login");
        }
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            Mensaje = "Por favor, corrige los errores del formulario.";
            return Page();
        }

        UsuarioAutenticado = HttpContext.Session.GetInt32("UsuarioId");
        if (UsuarioAutenticado == null)
        {
            return RedirectToPage("/Account/Login");
        }

        // Buscar la cuenta del usuario autenticado
        var cuenta = _context.Cuentas.FirstOrDefault(c => c.UsuarioId == UsuarioAutenticado);

        if (cuenta == null)
        {
            Mensaje = "Cuenta no encontrada.";
            return Page();
        }

        // Si la cuenta es de tipo Ahorros, usar la lógica de intereses
        if (cuenta is Ahorros ahorros)
        {
            Mensaje = ahorros.Retirar(Monto, _context);
            NuevoSaldo = ahorros.Saldo;
            return Page();
        }

        // Si la cuenta es de tipo Corriente, usar la lógica de sobregiro
        if (cuenta is Corriente corriente)
        {
            Mensaje = corriente.Retirar(Monto, _context);
            NuevoSaldo = corriente.Saldo;
            return Page();
        }

        // Si es otro tipo de cuenta, lógica estándar
        if (Monto <= 0)
        {
            Mensaje = "El monto debe ser positivo.";
            return Page();
        }

        if (Monto > cuenta.Saldo)
        {
            Mensaje = "Fondos insuficientes.";
            return Page();
        }

        cuenta.Saldo -= Monto;
        _context.Transacciones.Add(new Transaccion
        {
            Fecha = DateTime.Now,
            Concepto = "Retiro",
            Valor = -Monto,
            CuentaId = cuenta.CuentaId
        });
        _context.SaveChanges();

        NuevoSaldo = cuenta.Saldo;
        Mensaje = "Retiro exitoso.";
        return Page();
    }
}