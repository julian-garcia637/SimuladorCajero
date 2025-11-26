using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimuladorCajero.Data.Context;
using SimuladorCajero.Models;
using System.Collections.Generic;
using System.Linq;

public class MovimientosModel : PageModel
{
    private readonly AppDbContext _context;
    public MovimientosModel(AppDbContext context) => _context = context;

    public List<Transaccion> Movimientos { get; set; } = new();

    public int? UsuarioAutenticado { get; set; }

    public IActionResult OnGet()
    {
        UsuarioAutenticado = HttpContext.Session.GetInt32("UsuarioId");
        if (UsuarioAutenticado == null)
        {
            return RedirectToPage("/Account/Login");
        }
        // Lógica de la página usando usuarioId.Value
        var cuenta = _context.Cuentas.FirstOrDefault(c => c.UsuarioId == UsuarioAutenticado);

        if (cuenta != null)
        {
            Movimientos = _context.Transacciones
                .Where(t => t.CuentaId == cuenta.CuentaId)
                .OrderByDescending(t => t.Fecha)
                .Take(20)
                .ToList();
        }

        return Page();
    }
}