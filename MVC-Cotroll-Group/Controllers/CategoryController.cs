using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC_Cotroll_Group.Models;

namespace MVC_Cotroll_Group.Controllers
{
    public class CategoryController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
