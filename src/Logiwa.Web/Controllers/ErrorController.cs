using Microsoft.AspNetCore.Mvc;

namespace Logiwa.Web.Controllers;

public class ErrorController : Controller
{
    private const string ErrorMessage = "An error occurred. Please try again later.";

    public IActionResult Index()
    {
        ViewBag.ErrorMessage = ErrorMessage;
        return View();
    }
}