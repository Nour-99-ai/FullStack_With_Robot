using LocalCrafts.Data;
using LocalCrafts.Models;
using LocalCrafts.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalCrafts.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> PendingItems(int sellerId)
        {
            var pendingItems = await _context.OrderItems
                                             .Where(item => item.OrderItemStatus == "Pending" && item.Product.SellerId == sellerId) // تصفية حسب SellerId
                                             .Include(item => item.Order) 
                                             .Include(item => item.Product) 
                                             .Include(item => item.Product.Seller) 
                                             .OrderByDescending(item => item.Order.OrderDate) 
                                             .ToListAsync();

            if (pendingItems.Count == 0)
            {
                Console.WriteLine("No pending items found for the given sellerId.");
            }
            else
            {
                Console.WriteLine($"Found {pendingItems.Count} pending items.");
            }

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
        public async Task<IActionResult> UpdateOrderItemStatus(int orderItemId, string newStatus, int sellerId, DateTime? deliveryDate, string additionalInfo)
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

            if (newStatus == "Approved" && deliveryDate.HasValue)
            {
                orderItem.DeliveryDate = deliveryDate; 
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(PendingItems), new { sellerId = sellerId });
        }




    }
}
