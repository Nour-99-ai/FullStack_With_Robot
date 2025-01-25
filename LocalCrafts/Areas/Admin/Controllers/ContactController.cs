// Controllers/Admin/ContactMessagesController.cs
using Microsoft.AspNetCore.Mvc;
using LocalCrafts.Data;
using Microsoft.EntityFrameworkCore;
using LocalCrafts.Models;

namespace LocalCrafts.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _context.Contactus.ToListAsync();
            return View(messages); 
        }

        public async Task<IActionResult> Details(int id)
        {
            var message = await _context.Contactus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }
    }
}
