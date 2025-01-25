using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LocalCrafts.Data;
using LocalCrafts.Models;
using System.Linq;
using System.Threading.Tasks;
using LocalCrafts.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

public class SellerViewComponent : ViewComponent
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;

    public SellerViewComponent(UserManager<ApplicationUser> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Fetch all sellers
        var sellers = await _context.Sellers.ToListAsync();

        // Default value for IsSeller
        ViewBag.IsSeller = false;

        // Check if the user is authenticated
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User);
            if (user != null)
            {
                // Check if the user is a seller by checking if they are in the 'Seller' role
                bool isSeller = await _userManager.IsInRoleAsync(user, "Seller");
                ViewBag.IsSeller = isSeller;
            }
        }

        return View(sellers);
    }
}
