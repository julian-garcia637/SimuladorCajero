using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimuladorCajero.Data.Context;

public class LoginModel : PageModel
{
    private readonly AppDbContext _context;
    
    public LoginModel(AppDbContext context) => _context = context;

    [BindProperty]
    public string Usuario { get; set; } = string.Empty;

    [BindProperty]
    public string Clave { get; set; } = string.Empty;

    public string? Mensaje { get; set; }


    private const string SessionKeyIntentos = "IntentosLogin";

    public void OnGet()
    {
        HttpContext.Session.SetInt32(SessionKeyIntentos, 0);
    }

    public IActionResult OnPost()
    {
        int intentos = HttpContext.Session.GetInt32(SessionKeyIntentos) ?? 0;

        var user = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == Usuario && u.Contraseña == Clave);

        if (user == null)
        {
            intentos++;
            HttpContext.Session.SetInt32(SessionKeyIntentos, intentos);

            if (intentos >= 3)
            {
                Mensaje = "Cuenta bloqueada por intentos fallidos.";
                return RedirectToPage("Lockout");
            }

            Mensaje = $"Usuario o clave incorrectos. Intentos: {intentos}/3";
            return Page();
        }

        // Autenticaci髇 exitosa
        HttpContext.Session.SetInt32(SessionKeyIntentos, 0);

        // Guardar el usuarioId en la sesi髇
        HttpContext.Session.SetInt32("UsuarioId", user.UsuarioId);
        HttpContext.Session.SetString("UsuarioNombre", user.Nombre);

        return RedirectToPage("/Transacciones/Index");
    }
}