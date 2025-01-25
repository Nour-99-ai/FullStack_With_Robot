using LocalCrafts.Data;
using LocalCrafts.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LocalCrafts.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult CategoryProducts(int categoryId)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);

            if (category == null)
            {
                return NotFound();
            }

            var products = _context.Products.Where(p => p.CategoryId == categoryId).ToList();

            ViewData["CategoryId"] = categoryId;
            ViewData["CategoryName"] = category.CategoryName;

            return View("CategoryProducts", products); 
        }

        public IActionResult SellerProducts(int sellerId)
        {
            var seller = _context.Sellers.FirstOrDefault(s => s.Id == sellerId);

            if (seller == null)
            {
                return NotFound();
            }

            var products = _context.Products.Where(p => p.SellerId == sellerId).ToList();

            var viewModel = new SellerProductsViewModel
            {
                SellerId = seller.Id,
                SellerName = seller.Name,
                SellerBio = seller.Bio,
                SellerImage = seller.Image,
                Products = products,
                UserId = seller.UserId
            };

            return View("SellerProducts", viewModel); 
        }
    }
}
