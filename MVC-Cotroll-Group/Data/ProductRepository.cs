using Microsoft.EntityFrameworkCore;
using MVC_Cotroll_Group.Data;
using MVC_Cotroll_Group.Models;


namespace MVC_Cotroll_Group.Repositories
{
    public class ProductRepository : Repository<Product>
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllWithCategoryAsync()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }
    }
}
