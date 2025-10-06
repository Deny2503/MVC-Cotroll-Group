using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC_Cotroll_Group.Models;
using MVC_Cotroll_Group.Repositories;

namespace MVC_Cotroll_Group.Controllers
{
    public class HomeController : Controller
    {
        private readonly CategoryRepository _categoryRepository;

        public HomeController(CategoryRepository categoryController)
        {
            _categoryRepository = categoryController;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewData["Categories"] = categories;
            return View();
        }
    }
}
