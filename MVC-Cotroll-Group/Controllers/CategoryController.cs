using Microsoft.AspNetCore.Mvc;
using MVC_Cotroll_Group.Models;
using MVC_Cotroll_Group.Repositories;


namespace MVC_Cotroll_Group.Controllers
{
    public class CategoryController : Controller
    {
        
        private readonly CategoryRepository _categoryRepository;

        public CategoryController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categories = _categoryRepository.GetAllWithProductsAsync().Result;
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.AddAsync(category).Wait();
                _categoryRepository.SaveChangesAsync().Wait();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            Category? category = _categoryRepository.GetByIdAsync(id).Result;
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                _categoryRepository.UpdateAsync(category).Wait();
                _categoryRepository.SaveChangesAsync().Wait();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        public IActionResult Delete(int id)
        {
            Category category = _categoryRepository.GetByIdWithProductsAsync(id).Result;

            if (category == null)
                return NotFound();

            if (category.Products.Any())
            {
                TempData["Error"] = "Неможливо видалити категорію , бо вона містить продукти.";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _categoryRepository.GetByIdAsync(id).Result;
            if (category == null)
                return NotFound();
            _categoryRepository.DeleteAsync(id).Wait();
            _categoryRepository.SaveChangesAsync().Wait();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int id)
        {
            Category? category = _categoryRepository.GetByIdWithProductsAsync(id).Result;
            if (category == null)
                return NotFound();
            return View(category);
        }
    }
}