using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimuladorCajero.Data.Context;
using SimuladorCajero.Models;

public class TransferirModel : PageModel
{
    private readonly AppDbContext _context;
    public TransferirModel(AppDbContext context) => _context = context;

    [BindProperty]
    public int CuentaDestinoId { get; set; }
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

        int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        if (usuarioId == null)
            return RedirectToPage("/Account/Login");

        if (Monto <= 0)
        {
            Mensaje = "El monto debe ser positivo.";
            return Page();
        }

        var cuentaOrigen = _context.Cuentas.FirstOrDefault(c => c.UsuarioId == usuarioId.Value);
        var cuentaDestino = _context.Cuentas.FirstOrDefault(c => c.CuentaId == CuentaDestinoId);

        if (cuentaOrigen == null || cuentaDestino == null)
        {
            Mensaje = "Cuenta de origen o destino no encontrada.";
            return Page();
        }

        if (cuentaOrigen.CuentaId == cuentaDestino.CuentaId)
        {
            Mensaje = "No se puede transferir a la misma cuenta.";
            return Page();
        }

        if (Monto > cuentaOrigen.Saldo)
        {
            Mensaje = "Fondos insuficientes.";
            return Page();
        }

        cuentaOrigen.Saldo -= Monto;
        cuentaDestino.Saldo += Monto;

        _context.Transacciones.Add(new Transaccion
        {
            Fecha = DateTime.Now,
            Concepto = $"Transferencia a cuenta {CuentaDestinoId}",
            Valor = -Monto,
            CuentaId = cuentaOrigen.CuentaId
        });
        _context.Transacciones.Add(new Transaccion
        {
            Fecha = DateTime.Now,
            Concepto = $"Transferencia recibida de cuenta {cuentaOrigen.CuentaId}",
            Valor = Monto,
            CuentaId = cuentaDestino.CuentaId
        });

        _context.SaveChanges();

        NuevoSaldo = cuentaOrigen.Saldo;
        Mensaje = "Transferencia exitosa.";
        return Page();
    }
}