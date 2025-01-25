using Microsoft.AspNetCore.Mvc;
using LocalCrafts.Data;
using LocalCrafts.Models;
using System.Linq;
using LocalCrafts.Models.ViewModels;

public class SearchController : Controller
{
    private readonly AppDbContext _context;

    public SearchController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            return View(new SearchResultsViewModel());
        }

        var categories = _context.Categories
            .Where(c => c.CategoryName.Contains(query))
            .ToList();

        var sellers = _context.Sellers
            .Where(s => s.Name.Contains(query))
            .ToList();

        var products = _context.Products
            .Where(p => p.Name.Contains(query) || p.Description.Contains(query))
            .ToList();

        var viewModel = new SearchResultsViewModel
        {
            Query = query,
            Categories = categories,
            Sellers = sellers,
            Products = products
        };

        return View(viewModel);
    }
}
