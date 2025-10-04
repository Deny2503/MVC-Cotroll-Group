using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC_Cotroll_Group.Models;

namespace MVC_Cotroll_Group.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
