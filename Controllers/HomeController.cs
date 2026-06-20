using System.Diagnostics;
using AppCompleta.DB;
using Microsoft.AspNetCore.Mvc;

namespace AppCompleta.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppCompletaContext _db;
        public HomeController(AppCompletaContext logger)
        {
            _db = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
