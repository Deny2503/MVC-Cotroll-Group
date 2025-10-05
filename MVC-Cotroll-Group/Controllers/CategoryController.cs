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
        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> categories = await _categoryRepository.GetAllWithProductsAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            ModelState.Remove("Products");
            if (ModelState.IsValid)
            {
                await _categoryRepository.AddAsync(category);
                await _categoryRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Category? category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }
            ModelState.Remove("Products");
            if (ModelState.IsValid)
            {
                await _categoryRepository.UpdateAsync(category);
                await _categoryRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        public async Task<IActionResult> Delete(int id)
        {
            Category? category = await _categoryRepository.GetByIdWithProductsAsync(id);

            if (category == null)
                return NotFound();

            if (category.Products.Any())
            {
                TempData["Error"] = "Неможливо видалити категорію, бо вона містить продукти.";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound();
            await _categoryRepository.DeleteAsync(id);
            await _categoryRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}