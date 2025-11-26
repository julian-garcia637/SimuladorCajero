using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimuladorCajero.Models;
using SimuladorCajero.Data.Context;

public class RegisterModel : PageModel
{
    private readonly AppDbContext _context;

    public RegisterModel(AppDbContext context) => _context = context;

    [BindProperty]
    public Usuario NuevoUsuario { get; set; } = new();

    [BindProperty]
    public string ConfirmarClave { get; set; } = string.Empty;

    [BindProperty]
    public string TipoCuenta { get; set; } = string.Empty;

    public string? Mensaje { get; set; }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            Mensaje = "Datos inválidos.";
            return Page();
        }

        if (NuevoUsuario.Contraseña != ConfirmarClave)
        {
            Mensaje = "Las contraseñas no coinciden.";
            return Page();
        }

        if (string.IsNullOrEmpty(TipoCuenta))
        {
            Mensaje = "Debe seleccionar un tipo de cuenta.";
            return Page();
        }

        // Validar usuario 鷑ico
        if (_context.Usuarios.Any(u => u.NombreUsuario == NuevoUsuario.NombreUsuario))
        {
            Mensaje = "El usuario ya existe.";
            return Page();
        }

        // Crear la cuenta seg鷑 el tipo seleccionado
        Cuenta cuenta;
        if (TipoCuenta == "Ahorros")
        {
            cuenta = new Ahorros { Saldo = 0 };
        }
        else // Corriente
        {
            cuenta = new Corriente { Saldo = 0 };
        }

        NuevoUsuario.Cuenta = cuenta;

        _context.Usuarios.Add(NuevoUsuario);
        _context.SaveChanges();
        return RedirectToPage("/Account/Login");
    }
}