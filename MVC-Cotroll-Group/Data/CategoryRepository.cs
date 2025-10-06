using Microsoft.EntityFrameworkCore;
using MVC_Cotroll_Group.Data;
using MVC_Cotroll_Group.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC_Cotroll_Group.Repositories
{
    public class CategoryRepository : Repository<Category>
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetAllWithProductsAsync()
        {
            return await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdWithProductsAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => EF.Functions.Like(c.Name, $"%{name}%"));
        }
    }
}
