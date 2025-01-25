using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using LocalCrafts.Data;
using LocalCrafts.Models;

namespace LocalCrafts.Controllers
{
   
    public class SellersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SellersController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/Sellers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sellers.ToListAsync());
        }

        // GET: Admin/Sellers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        // GET: Admin/Sellers/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sellers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Bio,PhoneNumber")] Seller seller, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    seller.Image = "/images/" + fileName; 
                }

                _context.Add(seller);
                await _context.SaveChangesAsync();
                TempData["IsSellerCompleted"] = true;
                TempData["ShowSuccess"] = true;

                return RedirectToAction("Create");
            }
            return View(seller);
        }


        // GET: Admin/Sellers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers.FindAsync(id);
            if (seller == null)
            {
                return NotFound();
            }
            return View(seller);
        }

        // POST: Admin/Sellers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Bio,Image,PhoneNumber")] Seller seller, IFormFile ImageFile)
        {
            if (id != seller.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageFile != null)
                    {
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(fileStream);
                        }

                        seller.Image = "/images/" + fileName;
                    }

                    _context.Update(seller);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellerExists(seller.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(seller);
        }


        // GET: Admin/Sellers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        // POST: Admin/Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seller = await _context.Sellers.FindAsync(id);
            if (seller != null)
            {
                _context.Sellers.Remove(seller);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SellerExists(int id)
        {
            return _context.Sellers.Any(e => e.Id == id);
        }



        public async Task<IActionResult> PendingItems(int sellerId)
        {
            var pendingItems = await _context.OrderItems
                                             .Where(item => item.OrderItemStatus == "Pending" && item.Product.SellerId == sellerId)
                                             .Include(item => item.Order) 
                                             .Include(item => item.Product) 
                                             .OrderByDescending(item => item.Order.OrderDate)
                                             .ToListAsync();

            return View(pendingItems);
        }


        public async Task<IActionResult> ApprovedItems(int sellerId)
        {
          
            var approvedItems = await _context.OrderItems
                                              .Where(item => item.OrderItemStatus == "Approved" && item.Product.SellerId == sellerId)
                                              .Include(item => item.Order)
                                              .Include(item => item.Product)
                                              .OrderByDescending(item => item.Order.OrderDate)
                                              .ToListAsync();

            return View(approvedItems);
        }

        public async Task<IActionResult> RejectedItems(int sellerId)
        {

            var rejectedItems = await _context.OrderItems
                                              .Where(item => item.OrderItemStatus == "Rejected" && item.Product.SellerId == sellerId)
                                              .Include(item => item.Order)
                                              .Include(item => item.Product)
                                              .OrderByDescending(item => item.Order.OrderDate)
                                              .ToListAsync();

            return View(rejectedItems);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderItemStatus(int orderItemId, string newStatus, int sellerId)
        {
            var orderItem = await _context.OrderItems
                                          .Include(item => item.Product)
                                          .FirstOrDefaultAsync(item => item.OrderItemId == orderItemId && item.Product.SellerId == sellerId);

            if (orderItem == null)
            {
                return NotFound(); 
            }

            if (newStatus != "Pending" && newStatus != "Approved" && newStatus != "Rejected")
            {
                ModelState.AddModelError("OrderItemStatus", "Invalid status value.");
                return RedirectToAction(nameof(PendingItems), new { sellerId = sellerId });
            }

            orderItem.OrderItemStatus = newStatus;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(PendingItems), new { sellerId = sellerId });
        }

    }
}
