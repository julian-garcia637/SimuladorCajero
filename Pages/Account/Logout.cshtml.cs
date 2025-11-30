using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SimuladorCajero.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId is null)
            {
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            return FinalizarSesion();
        }

        private IActionResult FinalizarSesion()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Session");
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            TempData["AuthMessage"] = "Tu sesión se cerró correctamente.";
            return RedirectToPage("/Account/Login");
        }
    }
}
