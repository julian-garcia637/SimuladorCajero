using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SimuladorCajero.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            return FinalizarSesion();
        }

        public IActionResult OnPost()
        {
            return FinalizarSesion();
        }

        private IActionResult FinalizarSesion()
        {
            HttpContext.Session.Clear();
            TempData["AuthMessage"] = "Tu sesión se cerró correctamente.";
            return RedirectToPage("/Account/Login");
        }
    }
}
