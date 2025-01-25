
using LocalCrafts.Models;
using LocalCrafts.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using LocalCrafts.Models.ViewModels;
using LocalCrafts.Migrations;

namespace LocalCrafts.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int sellerId)
        {
            var userId = User.Identity.Name; 
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId, Items = new List<CartItem>() };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                var cartItem = cart.Items.FirstOrDefault(ci => ci.ProductId == productId && ci.SellerId == sellerId);
                if (cartItem == null)
                {
                    cart.Items.Add(new CartItem
                    {
                        ProductId = productId,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = 1,
                        Subtotal = product.Price,
                        SellerId = sellerId 
                    });
                }
                else
                {
                    cartItem.Quantity++;
                    cartItem.Subtotal = cartItem.Quantity * cartItem.Price;
                }

                cart.Total = cart.Items.Sum(item => item.Subtotal);
                await _context.SaveChangesAsync();
            }

            var cartHtml = string.Join("", cart.Items.Select(item => $@"
        <li>
            <a href='#' class='image'><img src='{item.Product.Image}' alt='Product Image'></a>
            <div class='content'>
                <a href='#' class='title'>{item.ProductName}</a>
                <span class='quantity-price'>{item.Quantity} x <span class='amount'>${item.Price:F2}</span></span>
                <a href='#' class='remove' onclick='removeFromCart({item.ProductId}, {item.SellerId})'>×</a>
            </div>
        </li>
    "));

            return Json(new
            {
                success = true,
                total = cart.Total,
                totalItems = cart.Items.Sum(i => i.Quantity), 
                cartHtml = cartHtml 
            });
        }




        public async Task<IActionResult> Index()
        {
            var userId = User.Identity.Name; 
            var cart = await _context.Carts
                                      .Include(c => c.Items)
                                      .ThenInclude(ci => ci.Product)  
                                      .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return View(new Cart()); 
            }

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCart(int cartItemId, int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                _context.Update(cartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId, int sellerId)
        {
            var userId = User.Identity.Name;

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.SellerId == sellerId && ci.Cart.UserId == userId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);

                var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);
                if (cart != null)
                {
                    cart.Total = cart.Items.Where(item => item.ProductId != productId || item.SellerId != sellerId).Sum(item => item.Subtotal);
                    _context.Update(cart);
                }

                await _context.SaveChangesAsync();
            }

            var updatedCart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);
            var updatedCartTotal = updatedCart?.Total ?? 0;

            var updatedCartHtml = updatedCart?.Items != null && updatedCart.Items.Any()
                ? string.Join("", updatedCart.Items.Select(item => $@"
            <li>
                <a href='#' class='image'><img src='{item.Product.Image}' alt='Product Image'></a>
                <div class='content'>
                    <a href='#' class='title'>{item.ProductName}</a>
                    <span class='quantity-price'>{item.Quantity} x <span class='amount'>${item.Price:F2}</span></span>
                    <a href='#' class='remove' onclick='removeFromCart({item.ProductId}, {item.SellerId})'>×</a>
                </div>
            </li>
        "))
                : "<li class='empty-cart'>Your cart is empty.</li>"; 

            return Json(new
            {
                success = true,
                total = updatedCartTotal,
                totalItems = updatedCart?.Items.Sum(i => i.Quantity) ?? 0, 
                cartHtml = updatedCartHtml 
            });
        }







        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                cartItem.Subtotal = cartItem.Quantity * cartItem.Price;
                _context.Update(cartItem);

                var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.CartId == cartItem.CartId);
                if (cart != null)
                {
                    cart.Total = cart.Items.Sum(item => item.Subtotal);
                    _context.Update(cart);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> GetCartDetails()
        {
            var userId = User.Identity.Name;
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            string cartHtml = cart.Items.Any()
                ? string.Join("", cart.Items.Select(item => $@"
            <li>
                <a href='#' class='image'><img src='{item.Product.Image}' alt='Product Image'></a>
                <div class='content'>
                    <a href='#' class='title'>{item.ProductName}</a>
                    <span class='quantity-price'>{item.Quantity} x <span class='amount'>${item.Price:F2}</span></span>
                    <a href='#' class='remove' onclick='removeFromCart({item.ProductId}, {item.SellerId})'>×</a>
                </div>
            </li>
        "))
                : "";

            return Json(new
            {
                success = true,
                total = cart?.Total ?? 0,
                totalItems = cart?.Items.Sum(i => i.Quantity) ?? 0,
                cartHtml = cartHtml
            });
        }



       
            [HttpPost]
            public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
            {
                var userId = User.Identity.Name;
                var cart = await _context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || cart.Items == null || !cart.Items.Any())
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
                        OrderItemStatus = "Pending"
                    }).ToList()

                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();


            return RedirectToAction("Index", "Home");

        }
    }



    }

