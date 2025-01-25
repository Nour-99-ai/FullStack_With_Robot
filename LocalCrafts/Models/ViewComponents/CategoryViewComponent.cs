using LocalCrafts.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocalCrafts.Models.ViewComponents
{
    public class CategoryViewComponent:ViewComponent
    {
        private AppDbContext db;
        public CategoryViewComponent(AppDbContext _db)
        {
            db = _db;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await db.Categories.ToListAsync();
            return View(categories);

        }
    }
}
