using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Cotroll_Group.Models;
using MVC_Cotroll_Group.Repositories;

namespace MVC_Cotroll_Group.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductRepository _productRepository;
        private readonly CategoryRepository _categoryRepository;

        public ProductController(ProductRepository productRepository, CategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index(string? category, string? sortOrder, int? rating, string? name)
        {
            var products = await _productRepository.GetAllWithCategoryAsync();
            if (!string.IsNullOrEmpty(category))
            {
                Category? category_object = await _categoryRepository.GetByNameAsync(category);
                if (category_object == null) return NotFound();

                products = products
                    .Where(p => p.CategoryId == category_object.Id);
                ViewData["SeacrchCategory"] = category_object.Name;
                
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                ViewData["Order"] = sortOrder switch
                {
                    "price_asc" => "Price ↑ (Lowest First)",
                    "price_desc" => "Price ↓ (Highest First)",
                    "rating_asc" => "Rating ↑ (Lowest First)",
                    "rating_desc" => "Rating ↓ (Highest First)",
                    "name_asc" => "Name A → Z",
                    "name_desc" => "Name Z → A",
                    _ => "Default Order"
                };
                
                products = sortOrder switch
                {
                    "price_asc" => products.OrderBy(p => p.Price),
                    "price_desc" => products.OrderByDescending(p => p.Price),
                    "rating_asc" => products.OrderBy(p => p.Rating),
                    "rating_desc" => products.OrderByDescending(p => p.Rating),
                    "name_asc" => products.OrderBy(p => p.Name),
                    "name_desc" => products.OrderByDescending(p => p.Name),
                    _ => products.OrderBy(p => p.Id)
                };
            }
            else
            {
                ViewData["Order"] = "Default Order";
            }

            if (rating.HasValue && rating.Value >= 1 && rating.Value <= 5)
            {
                products = products.Where(p => p.Rating >= rating.Value);
                ViewData["SearchRating"] = rating.Value;
            }


            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ModelState.Remove("Category");
            if (!ModelState.IsValid)
            {
                var categories = await _categoryRepository.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
                return View(product);
            }

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id) return BadRequest();

            ModelState.Remove("Category");
            if (!ModelState.IsValid)
            {
                var categories = await _categoryRepository.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
                return View(product);
            }

            await _productRepository.UpdateAsync(product);
            await _productRepository.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                await _productRepository.DeleteAsync(id);
                await _productRepository.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
