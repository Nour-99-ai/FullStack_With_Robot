using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocalCrafts.Models;
using LocalCrafts.Data;

namespace LocalCrafts.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductDetailsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category) 
                .Include(p => p.Seller) 
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

    }
}
