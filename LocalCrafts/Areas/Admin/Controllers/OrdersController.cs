using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocalCrafts.Data; 
using LocalCrafts.Models;
using LocalCrafts.Models.ViewModels; 
namespace LocalCrafts.Areas.Admin.Controllers
{
    [Area("Admin")] 

    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                                       .Include(o => o.OrderItems) 
                                       .OrderByDescending(o => o.OrderDate) 
                                       .ToListAsync();

            return View(orders);
        }




        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _context.Orders
                                      .Include(o => o.OrderItems)
                                        .ThenInclude(oi => oi.Product)
                                        .ThenInclude(p => p.Seller)
                                      .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound(); 
            }

            return View(order);
        }

        public async Task<IActionResult> PendingItems()
        {
            var pendingItems = await _context.OrderItems
                                             .Where(item => item.OrderItemStatus == "Pending")
                                             .Include(item => item.Order) 
                                             .Include(item => item.Product) 
                                             .Include(item => item.Product.Seller) 
                                             .OrderByDescending(item => item.Order.OrderDate) 
                                             .ToListAsync();

            return View(pendingItems); 
        }


        public async Task<IActionResult> ApprovedItems()
        {
            var approvedItems = await _context.OrderItems
                                              .Where(item => item.OrderItemStatus == "Approved")
                                              .Include(item => item.Order) 
                                              .Include(item => item.Product) 
                                              .Include(item => item.Product.Seller) 
                                              .OrderByDescending(item => item.Order.OrderDate) 
                                              .ToListAsync();

            return View(approvedItems); 
        }


        public async Task<IActionResult> RejectedItems(int sellerId)
        {
            var rejectedItems = await _context.OrderItems
                                              .Where(item => item.OrderItemStatus == "Rejected") 
                                              .Include(item => item.Order) 
                                              .Include(item => item.Product) 
                                              .Include(item => item.Product.Seller) 
                                              .OrderByDescending(item => item.Order.OrderDate) 
                                              .ToListAsync();

            return View(rejectedItems); 
        }


        [HttpPost]
        public async Task<IActionResult> UpdateOrderItemStatus(IFormCollection form)
        {
            
            var orderItemId = Convert.ToInt32(form["orderItemId"]);
            var newStatus = form["item.OrderItemStatus"];

            var orderItem = await _context.OrderItems.FindAsync(orderItemId);

            if (orderItem == null)
            {
                return NotFound();
            }

            
            if (newStatus != "Pending" && newStatus != "Approved" && newStatus != "Rejected")
            {
                ModelState.AddModelError("OrderItemStatus", "Invalid status value.");
                return RedirectToAction(nameof(PendingItems)); 
            }

            orderItem.OrderItemStatus = newStatus;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(PendingItems));
        }



        public async Task<IActionResult> ItemDetails(int id)
        {
            var item = await _context.OrderItems
                                     .Include(oi => oi.Order)
                                     .Include(oi => oi.Product)
                                     .Include(oi => oi.Product.Seller)
                                     .FirstOrDefaultAsync(oi => oi.OrderItemId == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }


        public async Task<IActionResult> ItemsList()
        {
            var items = await _context.OrderItems
                                      .Include(oi => oi.Order) 
                                      .Include(oi => oi.Product) 
                                      .Include(oi => oi.Product.Seller) 
                                      .OrderByDescending(item => item.Order.OrderDate)
                                      .ToListAsync();

            return View(items); 
        }











    }
}
