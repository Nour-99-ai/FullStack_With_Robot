// Controllers/ContactController.cs
using Microsoft.AspNetCore.Mvc;
using LocalCrafts.Data;
using LocalCrafts.Models;
using Microsoft.EntityFrameworkCore;

namespace LocalCrafts.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendMessage(Contactus contactus)
        {
            if (ModelState.IsValid)
            {
                _context.Contactus.Add(contactus); 
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home"); 
            }

            return View("Index", "Home"); 

        }

       
    }
}
