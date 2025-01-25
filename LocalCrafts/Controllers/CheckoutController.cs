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
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.Identity.Name;
            var cart = await _context.Carts
                                      .Include(c => c.Items)
                                      .ThenInclude(ci => ci.Product)  
                                      .Include(c => c.Items)
                                      .ThenInclude(ci => ci.Product.Seller) 
                                      .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart"); 
            }

            var checkoutViewModel = new CheckoutViewModel
            {
                CartItems = cart.Items.ToList(),
                TotalAmount = cart.Total
            };

            return View(checkoutViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.Name;
                var cart = await _context.Carts
                                          .Include(c => c.Items)
                                          .ThenInclude(ci => ci.Product)  
                                          .Include(c => c.Items)
                                          .ThenInclude(ci => ci.Product.Seller) 
                                          .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || !cart.Items.Any())
                {
                    return RedirectToAction("Index", "Cart"); 
                }

                var order = new Order
                {
                    UserId = userId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    City = model.City,
                    Address = model.Address,
                    AdditionalInfo = model.AdditionalInfo,
                    PaymentMethod = model.PaymentMethod,
                    OrderDate = DateTime.Now,
                    TotalAmount = cart.Total, 
                    OrderItems = cart.Items.Select(item => new OrderItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Subtotal = item.Subtotal,
                        SellerId = item.Product.SellerId,
                    }).ToList()
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();

                return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var order = await _context.Orders
                                      .Include(o => o.OrderItems)
                                      .ThenInclude(oi => oi.Product)  
                                      .Include(o => o.OrderItems)
                                      .ThenInclude(oi => oi.Product.Seller)  
                                      .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return RedirectToAction("Index", "Home"); 
            }

            return View(order);
        }
    }
}
