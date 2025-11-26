using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimuladorCajero.Data.Context;
using SimuladorCajero.Models;

public class ConsignarModel : PageModel
{
    private readonly AppDbContext _context;
    public ConsignarModel(AppDbContext context) => _context = context;

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
        // Lógica de la página usando usuarioId.Value
        return Page();
    }

    public IActionResult OnPost()
    {
        if (Monto <= 0)
        {
            Mensaje = "El monto debe ser positivo.";
            return Page();
        }

        // Suponiendo que el usuario está autenticado y su id está en sesión
        UsuarioAutenticado = HttpContext.Session.GetInt32("UsuarioId");
        var cuenta = _context.Cuentas.FirstOrDefault(c => c.UsuarioId == UsuarioAutenticado);

        if (cuenta == null)
        {
            Mensaje = "Cuenta no encontrada.";
            return Page();
        }

        cuenta.Saldo += Monto;
        _context.Transacciones.Add(new Transaccion
        {
            Fecha = DateTime.Now,
            Concepto = "Consignación",
            Valor = Monto,
            CuentaId = cuenta.CuentaId
        });
        _context.SaveChanges();

        NuevoSaldo = cuenta.Saldo;
        Mensaje = "Consignación exitosa.";

        return Page();
    }
}