using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimuladorCajero.Data.Context;

public class SaldoModel : PageModel
{
    private readonly AppDbContext _context;
    public SaldoModel(AppDbContext context) => _context = context;

    public double Saldo { get; set; }

    public int? UsuarioAutenticado { get; set; }

    public IActionResult OnGet()
    {
        UsuarioAutenticado = HttpContext.Session.GetInt32("UsuarioId");
        if (UsuarioAutenticado == null)
        {
            return RedirectToPage("/Account/Login");
        }

        var cuenta = _context.Cuentas.FirstOrDefault(c => c.UsuarioId == UsuarioAutenticado);
        Saldo = cuenta?.Saldo ?? 0;

        return Page();
    }
}