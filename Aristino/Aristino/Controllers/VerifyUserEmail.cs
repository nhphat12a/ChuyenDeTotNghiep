using Microsoft.AspNetCore.Mvc;

namespace Aristino.Controllers
{
    public class VerifyUserEmail : Controller
    {
        public async Task<IActionResult> VerifyPage()
        {
            return View();
        }

    }
}
